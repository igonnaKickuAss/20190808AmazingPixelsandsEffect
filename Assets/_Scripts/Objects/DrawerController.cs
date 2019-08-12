using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxi.OObjects
{
    using OLiOYouxi.OSystem;

    /// <summary>
    /// 对象类
    /// </summary>
    class LivePixelStack
    {
        public List<LivePixel> livePixels = new List<LivePixel>(3000);

        //这是记录最开始生成粒子的位置
        public int x;
        public int y;

        public bool needsDraw = false;

        public Color topColor
        {
            get { return livePixels.Count > 0 ? livePixels[livePixels.Count - 1].color : backgroundColor; }
        }

        public Color backgroundColor;
    }

    /// <summary>
    /// 传递值结构
    /// </summary>
    struct PassData
    {
        public int x;
        public int y;
        public int col;
        public int row;
        public int width;
        public int height;
        public Color color;
    }

    [DisallowMultipleComponent]
    public class DrawerController : MonoBehaviour
    {
        #region -- Private Data --
        //pallette
        private PalletteController palletteController = null;
        private PalletteDataModel palletteData = null;
        private PallettePrevDataModel pallettePrevData = null;

        //objectPixel
        [OLiOYouxiAttributes.BoxGroup("颜料板")]
        [OLiOYouxiAttributes.ReorderableList()]
        [SerializeField]
        private ObjectPixel[] objectPixels = null;

        private Dictionary<EnumCentre.BrushType, ObjectPixel> dicObjectPixel = null;

        [OLiOYouxiAttributes.BoxGroup("像素粒子"), OLiOYouxiAttributes.Label("粒子速度：")]
        [OLiOYouxiAttributes.MinValue(1), OLiOYouxiAttributes.MaxValue(10)]
        [SerializeField]
        private int sandSpeed = 1;

        /// <summary>
        /// 注意
        /// 字典里的对象是存储 引用对象指向的堆内存上的数据位置
        /// </summary>
        private Stack<LivePixelStack> lpsRecyclePool;                                     //对象（已经回收的）池子
        private Dictionary<int, LivePixelStack> dicMapLivePixelStack;                           //对象（还没被回收的）池子 //这些对象记录了散落在屏幕上的所有粒子
        private Dictionary<Type, Stack<LivePixel>> dicPixelStackPool;       //粒子类型回收池子

        //else items
        [OLiOYouxiAttributes.BoxGroup("杂项"), OLiOYouxiAttributes.Label("摄像机：")]
        [SerializeField]
        private Camera cam = null;
        
        //default frame
        [OLiOYouxiAttributes.BoxGroup("地图"), OLiOYouxiAttributes.Label("使用地图：")]
        [SerializeField]
        private bool displayPreview = false;

        [OLiOYouxiAttributes.BoxGroup("地图")]
        [OLiOYouxiAttributes.AssetPreview(80, 80)]
        [OLiOYouxiAttributes.ShowIf("displayPreview")]
        [SerializeField]
        private Texture2D frame = null;
        
        //coroutines
        private Coroutine tileApplyCoro = null;

        //CPU & GPU
        private CPUVersion cpu = null;
        private GPUVersion gpu = null;

        #endregion

        #region -- COMMOM VAR --
        bool anyNeedsApply = false;
        PassData passData;

        #endregion

        #region -- Struct --
        public struct State
        {
            public int livePixelCount;
            public int recyclePixelCount;
            public int livePixelStackCount;
            public int lpsRecycelCount;
            public int recyclePoolCount;
        }

        public State GetStats()
        {
            State stat = new State
            {
                livePixelCount = 0,
                recyclePixelCount = 0,
                livePixelStackCount = 0,
                lpsRecycelCount = 0,
                recyclePoolCount = 0,
            };

            foreach (LivePixelStack stack in dicMapLivePixelStack.Values)
            {
                stat.livePixelCount += stack.livePixels.Count;
            }

            foreach (Stack<LivePixel> pool in dicPixelStackPool.Values)
            {
                stat.recyclePixelCount += pool.Count;
            }

            stat.livePixelStackCount = dicMapLivePixelStack.Count;
            stat.lpsRecycelCount = lpsRecyclePool.Count;
            stat.recyclePoolCount = dicPixelStackPool.Count;

            return stat;
        }

        #endregion

        #region -- Log APIMethods --
#if UNITY_EDITOR
        void DrawFrameToTileIsFailure()
        {
            DebuggerFather.instance.ToDebugLogErr("绘制地图失败了。。", EnumCentre.ColorName.red);
        }


#endif
        #endregion

        #region -- MONO APIMethods --
        private void OnEnable()
        {
            InitData();
        }

        private void OnDisable()
        {
            //清理

        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                if (Time.frameCount % sandSpeed == 0)
                    UpdateToDoSth();

                //if (!Input.GetKeyDown(KeyCode.P))
                //    return;
                //screenShotName = "沙盒 " + DateTime.Now.ToString("yyyyMMddhhmmss"); ;
                //ScreenShot();
            }

        }

        #region -- TestlateUpdate --

        ////VAR
        //int num = 0, col = 0, row = 0;
        //private void LateUpdate()
        //{
        //    //TODO..判断是否存在需要保存的瓦片纹理  anyNeedsApply
        //    //Start..
        //    if (Time.frameCount % sandSpeed == 0)
        //    {
        //        LateUpdateToDoSth();

        //        if (anyNeedsApply)
        //        {
        //            for (num = 0, col = 0, row = 0; num < tileNeedsApply.Length; num++, col++)
        //            {
        //                if (col > tilesCols - 1)
        //                {
        //                    col = 0;
        //                    row += 1;
        //                }

        //                if (!tileNeedsApply[col, row])
        //                    continue;


        //                tileTexs[col, row].Apply();
        //                tileNeedsApply[col, row] = false;

        //            }
        //            anyNeedsApply = false;
        //        }
        //    }

        //}
        #endregion


        #endregion

        #region -- CORO APIMethods --
        private IEnumerator CheckTilesTextureApply()
        {
            int num = 0, col = 0, row = 0;

            while (Application.isPlaying)
            {
                //TODO..做一些数据变化
                yield return YieReturns.instance.GetWaitForEndOfFrame();

                //TODO..判断是否存在需要保存的瓦片纹理  anyNeedsApply
                //Start..
                if (Time.frameCount % sandSpeed == 0)
                {
                    LateUpdateToDoSth();

                    if (anyNeedsApply)
                    {
                        for (num = 0, col = 0, row = 0; num < palletteData.tileNeedsApply.Length; num++, col++)
                        {
                            if (col > pallettePrevData.prevTilesCols - 1)
                            {
                                col = 0;
                                row += 1;
                            }

                            if (!palletteData.tileNeedsApply[col, row])
                                continue;

                            if (pallettePrevData.prevUseGPUTexture2D)
                                palletteData.tileTexsGPU[col, row].Apply();    //GPU
                            else
                                palletteData.tileTexs[col, row].Apply();   //CPU

                            //palletteData.tileTexs[col, row].Apply();   //CPU

                            palletteData.tileNeedsApply[col, row] = false;

                        }
                        anyNeedsApply = false;
                    }
                }

            }
            //TODO..停止该协程!
        }
        
        #endregion

        #region -- 初始化 --
        /// <summary>
        /// 引用画板里的所有东西
        /// </summary>
        private void InitData()
        {
            InitPallette();
            InitTiles();
            InitObjectPixel();
            InitObjectPool();
            InitDrawer();
            InitCoros();

        }

        private void InitPallette()
        {
            //pallette..
            palletteController = PalletteController.instance;
            palletteData = palletteController.palleteData;
            pallettePrevData = palletteController.palletePrevData;
        }

        private void InitTiles()
        {
            //tiles
            palletteData.tileObjs = new GameObject[pallettePrevData.prevTilesCols, pallettePrevData.prevTilesRows];
            palletteData.tileMats = new Material[pallettePrevData.prevTilesCols, pallettePrevData.prevTilesRows];
            palletteData.tileTexs = new Texture2D[pallettePrevData.prevTilesCols, pallettePrevData.prevTilesRows];
            palletteData.tileTexsGPU = new Texture2D_GPU[pallettePrevData.prevTilesCols, pallettePrevData.prevTilesRows];
            palletteData.tileNeedsApply = new bool[pallettePrevData.prevTilesCols, pallettePrevData.prevTilesRows];

            for (int num = 0, x = 0, y = 0; num < palletteData.tileObjs.Length; num++, x++)
            {
                if (x > pallettePrevData.prevTilesCols - 1)
                {
                    y++;
                    x = 0;
                }
                Transform t = transform.Find("Tile " + x + " ," + y);
                GameObject go = t ? t.gameObject : null;
                if (!go)
                {
                    //TODO.. 清除所有Tile
                    //TODO.. 生成Tile
                    //TODO.. 弹出
                    //Start..
                    palletteController.ClearTileGameObjects();
                    palletteController.CreateTileGameObjects();
                    return;
                }

                palletteData.tileObjs[x, y] = go;
                palletteData.tileMats[x, y] = go.GetComponent<Renderer>().sharedMaterial;            //此处稍加注意！！！！！！
                //CPU
                palletteData.tileTexs[x, y] = palletteData.tileMats[x, y].mainTexture as Texture2D;
                //GPU
                palletteData.tileTexsGPU[x, y] = new Texture2D_GPU(pallettePrevData.prevTileWidth, pallettePrevData.prevTileHeight, palletteController.GetComputeShader());
                palletteData.tileTexsGPU[x, y].texture2D = palletteData.tileMats[x, y].mainTexture as Texture2D;
                palletteData.tileTexsGPU[x, y].SetColorArr();

                palletteData.tileNeedsApply[x, y] = false;
            }
        }

        private void InitObjectPixel()
        {
            dicObjectPixel = new Dictionary<EnumCentre.BrushType, ObjectPixel>(20);
            for (int i = 0; i < objectPixels.Length; i++)
            {
                if (objectPixels[i].Init())
                    dicObjectPixel[objectPixels[i].brush] = objectPixels[i];
            }
        }

        private void InitCoros()
        {
            tileApplyCoro = StartCoroutine(CheckTilesTextureApply());
        }

        private void InitObjectPool()
        {
            lpsRecyclePool = new Stack<LivePixelStack>(3000);
            dicMapLivePixelStack = new Dictionary<int, LivePixelStack>(3000);
            dicPixelStackPool = new Dictionary<Type, Stack<LivePixel>>(20);
        }

        private void InitDrawer()
        {
            cpu = new CPUVersion();
            gpu = new GPUVersion();
        }

        #endregion

        #region -- Inspector APIMethods --
        /// <summary>
        /// 新建地图
        /// </summary>
        /// <returns></returns>
        [OLiOYouxiAttributes.Button("绘制地图")]
        public void DrawFrameToTile()
        {
            if (!Application.isPlaying)
            {
                DrawFrameToTileIsFailure();
                return;
            }

            if (frame == null || palletteData == null || pallettePrevData == null || palletteController == null)
                return;

            int cols = pallettePrevData.prevTilesCols;
            int rows = pallettePrevData.prevTilesRows;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    //TODO..获得frame的tile大小的color[]
                    //TODO..将color[]设置在所在位置的tile上
                    //Start..
                    DrawFrame(col, row, pallettePrevData.prevTileWidth, pallettePrevData.prevTileHeight);
                }
            }
        }

        #endregion

        #region -- Public APIMethods --
        //VAR
        LivePixel pixel = null;
        public T CreateLivePixel<T>(Vector2 pos, EnumCentre.BrushType type) where T : LivePixel, new()
        {
            //TODO...翻一下粒子类型回收池子，看看有没有该类型的回收池子
            //TODO...初始化这个粒子的数据
            //TODO...把该粒子放进堆里(这是还活着的状态的粒子)
            //TODO...返回这个粒子
            //Start...

            Stack<LivePixel> pool = null;

            dicPixelStackPool.TryGetValue(typeof(T), out pool);
            if (pool == null || pool.Count == 0)
            {
                //什么？？没有这个类型的粒子回收池子或者回收池子里没有死掉的粒子
                //那就再生一个粒子
                pixel = new T();
            }
            else
            {
                //哈哈哈哈这个类型的回收池子里有死掉的粒子，拿出来，要活的！！！
                pixel = pool.Pop();
                pixel.isDead = false;
            }


            //初始化
            pixel.position = pos;
            pixel.renderWStart = pallettePrevData.prevRenderStartEndW.x;
            pixel.renderHStart = pallettePrevData.prevRenderStartEndH.x;
            pixel.renderWEnd = pallettePrevData.prevRenderStartEndW.y;
            pixel.renderHEnd = pallettePrevData.prevRenderStartEndH.y;

            pixel.color = dicObjectPixel[type].color;

            //放进活跃堆里
            AddToStack(pixel, pixel.x, pixel.y);

            pixel.Start(this);    //可能会增加GC？


            //返回
            return pixel as T;
        }
        
        public Color GetPixel(int x, int y, bool includeLive = false)
        {
            //TODO...所给位置是否再范围内？
            //TODO...根据位置讯息，翻找对象(还活着的)池子,是否存在对象？ 是活的，那就返回他的topColor，否则就backgroundColor
            //TODO...若没有找到对象， 那就判断对象位置所在的瓦片位置
            //TODO...画他！！！
            //Start...

            if (x >= pallettePrevData.prevTotalWidth || y >= pallettePrevData.prevTotalHeight || x < 0 || y < 0) return Color.clear;

            int key, col, row, tx, ty;

            LivePixelStack stack = null;

            key = KeyForXY(x, y);

            if (dicMapLivePixelStack.TryGetValue(key, out stack))
                return includeLive ? stack.topColor : stack.backgroundColor;

            col = x / pallettePrevData.prevTileWidth; row = y / pallettePrevData.prevTileHeight;

            tx = x % pallettePrevData.prevTileWidth; ty = y % pallettePrevData.prevTileHeight;


            if (pallettePrevData.prevUseGPUTexture2D)
                return palletteData.tileTexsGPU[col, row].GetPixel(tx, ty);      //GPU
            else
                return palletteData.tileTexs[col, row].GetPixel(tx, ty);  //CPU

            //return palletteData.tileTexs[col, row].GetPixel(tx, ty);  //CPU

        }

        public void SetPixel(int x, int y, Color color)
        {
            //TODO..检查这个像素的pos是否在可绘制区域
            //TODO..检查对象（还没被回收的）池子里是否有这个位置的对象，有的话就把这个粒子的背景色覆盖
            //TODO..绘制他！
            //Start..

            if (x < 0 || y < 0 || x >= pallettePrevData.prevTotalWidth || y >= pallettePrevData.prevTotalHeight) return;

            LivePixelStack stack = null;

            if (dicMapLivePixelStack.TryGetValue(KeyForXY(x, y), out stack))
            {
                stack.backgroundColor = color;
                return;
            }
            
            SetPixelIgnoringStacks(x, y, color);       //走，去把纹理设置了
        }

        public bool PixelPosAtScreenPos(Vector3 screenPos, out Vector2 pixelPos)
        {
            if (cam == null) cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(screenPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector2 localPt = transform.InverseTransformPoint(hit.point);
                pixelPos = localPt * pallettePrevData.prevPixelsPerUnit;
                return true;
            }
            pixelPos = Vector2.zero;
            return false;
        }

        /// <summary>
        /// 清除给定区域中可能存在的活着的对象
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void ClearLivePixels(int x, int y)
        {
            if (x < 0 || y < 0 || x >= pallettePrevData.prevTotalWidth || y >= pallettePrevData.prevTotalHeight) return;
            LivePixelStack stack = null;
            if (dicMapLivePixelStack.TryGetValue(KeyForXY(x, y), out stack))
            {
                if (stack.livePixels.Count > 0)
                {
                    //使用using 是为了调用dispose接口的方法
                    using (var lps = stack.livePixels.GetEnumerator())
                    {
                        while (lps.MoveNext())
                        {
                            Recycle(lps.Current);        //让他的粒子回池子里带着
                        }
                    }
                    stack.livePixels.Clear();
                    stack.needsDraw = true;     //既然livePixels.count已经不大于零了，那这个对象的背景色是啥就画啥
                }
            }
        }

        /// <summary>
        /// 绘制一个椭圆(包括圆形)区域
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        public void FillEllipse(Rect rect, Color color)
        {
            int y0 = Mathf.FloorToInt(rect.yMin);
            int y1 = Mathf.FloorToInt(rect.yMax);

            float r = rect.height / 2, ox = rect.center.x, oy = rect.center.y, w = rect.width, h = rect.height;
            float rSqr = r * r;

            for (int y = y0; y <= y1; y++)
            {
                float cy = oy - y;
                float cx = Mathf.Sqrt(rSqr - cy * cy) * (w / h);

                int x0 = Mathf.RoundToInt(ox - cx);
                int x1 = Mathf.RoundToInt(ox + cx);
                for (int x = x0; x <= x1; x++)
                {
                    //如果一整块都是同一种颜色，那就调用FillTile会更有效率
                    if (x % pallettePrevData.prevTileWidth == 0 && y % pallettePrevData.prevTileHeight == 0)
                    {
                        float dSqr = Mathf.Pow(x + pallettePrevData.prevTileWidth - 1 - ox, 2f)
                                   + Mathf.Pow(y + pallettePrevData.prevTileHeight - 1 - ox, 2f);

                        if (dSqr < rSqr)
                        {
                            FillTile(x / pallettePrevData.prevTileWidth, y / pallettePrevData.prevTileHeight, color);
                            x += pallettePrevData.prevTileWidth - 1;
                            continue;
                        }
                    }

                    SetPixel(x, y, color);
                }
            }

        }

        public void OutPallettePrevData(out PallettePrevDataModel pallettePrevData)
        {
            pallettePrevData = this.pallettePrevData;
        }

        public void OutPalletteData(out PalletteDataModel palletteData)
        {
            palletteData = this.palletteData;
        }

        #endregion

        #region -- Private APIMethods --
        private void DrawFrame(int col, int row, int width, int height)
        {
            if (pallettePrevData.prevUseGPUTexture2D)
            {
                passData = new PassData
                {
                    col = col,
                    row = row,
                    width = width,
                    height = height
                };

                Texture2D_GPU[,] tileTexsGPU = palletteData.tileTexsGPU;  //引用tiletexGPUss
                gpu.DrawFrame(ref passData, ref tileTexsGPU, ref frame);

                NeedApplyAtTile(col, row);  // 纹理Apply
            }
            else
            {
                passData = new PassData
                {
                    col = col,
                    row = row,
                    width = width,
                    height = height
                };

                Texture2D[,] tileTexs = palletteData.tileTexs;  //引用tiletexss
                cpu.DrawFrame(ref passData, ref tileTexs, ref frame);

                NeedApplyAtTile(col, row);  // 纹理Apply
                
            }
        }

        private void DrawStack(LivePixelStack stack)
        {
            SetPixelIgnoringStacks(stack.x, stack.y, stack.topColor);
        }

        /// <summary>
        /// 这是实现位移粒子的重要API
        /// </summary>
        /// <param name="pixel"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        //VAR
        LivePixelStack stack = null;
        private void AddToStack(LivePixel pixel, int x, int y)
        {
            //TODO..从对象（还没被回收的）池子中拿到这个对象
            //TODO..这个对象的数据要重新赋值或者是初始化噢！！！！！！！！！！！！！！！！
            //TODO..最后不要忘记把他塞回那个对象（还没被回收的）池子里去！！！
            //TODO..还有把这个对象标记为要渲染状态！ Apply = true
            //Start..

            int key = KeyForXY(x, y);


            if (!dicMapLivePixelStack.TryGetValue(key, out stack))
            {
                if (lpsRecyclePool == null || lpsRecyclePool.Count == 0)
                {
                    stack = new LivePixelStack();       //那就创建一个新的
                }
                else
                {
                    stack = lpsRecyclePool.Pop();   //那就把他推出来！
                }

                //赋值
                stack.x = x;
                stack.y = y;
                stack.backgroundColor = GetPixel(x, y);
                dicMapLivePixelStack[key] = stack;
            }

            stack.livePixels.Add(pixel);
            stack.needsDraw = true;

        }

        private void SetPixelIgnoringStacks(int x, int y, Color color)
        {
            //TODO..检查这个像素的pos是否在可绘制区域
            //TODO..检查颜色是否相同
            //TODO..绘制他！
            //Start..

            if (x < 0 || y < 0 || x >= pallettePrevData.prevTotalWidth || y >= pallettePrevData.prevTotalHeight) return;

            //数据初始化
            int col, row;

            col = x / pallettePrevData.prevTileWidth;
            row = y / pallettePrevData.prevTileHeight;

            if (pallettePrevData.prevUseGPUTexture2D)
            {
                passData = new PassData
                {
                    col = col,
                    row = row,
                    width = pallettePrevData.prevTileWidth,
                    height = pallettePrevData.prevTileHeight,
                    x = x,
                    y = y,
                    color = color
                };

                Texture2D_GPU[,] tileTexsGPU = palletteData.tileTexsGPU;    //引用tiletexGPUss
                gpu.SetPixelIgnoringStacks(ref passData, ref tileTexsGPU);

                NeedApplyAtTile(col, row);              //告诉他，这个位置的瓦片的纹理已经更改了，快去调用Apply()交给GPU渲染吧
            }
            else
            {
                passData = new PassData
                {
                    col = col,
                    row = row,
                    width = pallettePrevData.prevTileWidth,
                    height = pallettePrevData.prevTileHeight,
                    x = x,
                    y = y,
                    color = color
                };

                Texture2D[,] tileTexs = palletteData.tileTexs;      //引用tiletexss
                cpu.SetPixelIgnoringStacks(ref passData, ref tileTexs);

                NeedApplyAtTile(col, row);              //告诉他，这个位置的瓦片的纹理已经更改了，快去调用Apply()交给GPU渲染吧
            }
            
        }

        /// <summary>
        /// 用某种颜色填充一整块瓦片
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="color"></param>
        private void FillTile(int col, int row, Color color)
        {
            Material mat = palletteData.tileObjs[col, row].GetComponent<Renderer>().sharedMaterial;
            mat.mainTexture = null;
            mat.color = color;
        }


        #endregion

        #region -- Life APIMethods --
        Stack<LivePixel> pool = null;
        private void Recycle(LivePixel pixel)
        {
            //TODO..类型池子里找到该粒子所属的对象池子
            //TODO..把他拉出来！！
            //TODO..再把死掉的粒子塞进他的♂去
            //Start..

            if (!dicPixelStackPool.TryGetValue(pixel.GetType(), out pool))
            {
                pool = new Stack<LivePixel>(10);
                dicPixelStackPool[pixel.GetType()] = pool;
            }
            pool.Push(pixel);
        }
        
        NoGCList<int> keys = new NoGCList<int>(3000, 10);
        //NoGCList<LivePixelStack> stacks = new NoGCList<LivePixelStack>(3000, 10);
        //List<LivePixelStack> stacks = new List<LivePixelStack>(3000);
        private void UpdateToDoSth()
        {
            //TODO...遍历对象（还没被回收的）池子中所有粒子对象
            //TODO...拿到各个粒子对象后，各个进行数据操作， 像他们的移动位置，进行纹理更改？
            //TODO...从特定类型的回收对象池子里拿出特定类型的粒子
            //Start...

            if (dicMapLivePixelStack == null | dicMapLivePixelStack.Count == 0) return;

            int key, curframe, i, j, newkey;

            curframe = Time.frameCount;

            #region -- 20190609 --

            //List<LivePixelStack> stacks = new List<LivePixelStack>(dicMapLivePixelStack.Values);

            //foreach (LivePixelStack stack in stacks) 
            //{
            //    List<LivePixel> pixels = stack.livePixels;

            //    if (pixels.Count == 0)
            //        continue;

            //    Color lastTopColor = stack.topColor;

            //    key = KeyForXY(stack.x, stack.y);

            //    for (i = pixels.Count - 1; i >= 0; i--) 
            //    {
            //        LivePixel pixel = pixels[i];

            //        if (pixel.lastUpdateFrame == curframe)
            //            continue;

            //        pixel.Update(this);

            //        pixel.lastUpdateFrame = curframe;

            //        if (i >= pixels.Count || pixels[i] != pixel)        //这个粒子可能已经死了
            //            continue;

            //        newkey = KeyForXY(pixel.x, pixel.y);

            //        if (pixel.isDead)
            //        {
            //            //快把这对象里的粒子移除了，因为他死了
            //            pixels.RemoveAt(i);
            //            //如果这个粒子像素是透明的，那就不要覆盖背景了
            //            if (pixel.color.a > 0)
            //                SetPixel(pixel.x, pixel.y, pixel.color);    //如果是单一像素,so 单一颜色 colors.length = 1··
            //            Recycle(pixel);
            //        }
            //        else if (newkey != key)
            //        {
            //            // 这个粒子已经移动了.  那就把它从池子里移除
            //            // 然后再把新的粒子添加进池子来替换他
            //            pixels.RemoveAt(i);
            //            AddToStack(pixel, pixel.x, pixel.y);
            //        }
            //    }

            //    //检查这两个对象(stack)，颜色是否相同，相同说明这个对象里的粒子已经静止了，不相同说明正在运动，那就清除之前的渲染位置的粒子纹理，从而实现粒子纹理位移
            //    if (stack.topColor != lastTopColor)
            //        stack.needsDraw = true;
            //}
            #endregion

            #region -- 20190614 --
            Dictionary<int, LivePixelStack>.KeyCollection stacksKey = dicMapLivePixelStack.Keys;

            //拿到迭代器
            Dictionary<int, LivePixelStack>.KeyCollection.Enumerator enumerator = stacksKey.GetEnumerator();
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Current);
            }

            for (j = 0; j < keys.Count; j++)
            {

                List<LivePixel> pixels = dicMapLivePixelStack[keys[j]].livePixels;

                if (pixels.Count == 0)
                    continue;

                Color lastTopColor = dicMapLivePixelStack[keys[j]].topColor;

                key = KeyForXY(dicMapLivePixelStack[keys[j]].x, dicMapLivePixelStack[keys[j]].y);

                for (i = pixels.Count - 1; i >= 0; i--)
                {
                    LivePixel pixel = pixels[i];

                    if (pixel.lastUpdateFrame == curframe)
                        continue;

                    pixel.Update(this);

                    pixel.lastUpdateFrame = curframe;

                    if (i >= pixels.Count || pixels[i] != pixel)        //这个粒子可能已经死了
                        continue;

                    newkey = KeyForXY(pixel.x, pixel.y);

                    if (pixel.isDead)
                    {
                        //快把这对象里的粒子移除了，因为他死了
                        pixels.RemoveAt(i);
                        //如果这个粒子像素是透明的，那就不要覆盖背景了
                        if (pixel.color.a > 0)
                            SetPixel(pixel.x, pixel.y, pixel.color);    //如果是单一像素,so 单一颜色 colors.length = 1··
                        Recycle(pixel);
                    }
                    else if (newkey != key)
                    {
                        // 这个粒子已经移动了.  那就把它从池子里移除
                        // 然后再把新的粒子添加进池子来替换他
                        pixels.RemoveAt(i);
                        AddToStack(pixel, pixel.x, pixel.y);
                    }
                }

                //检查这两个对象(stack)，颜色是否相同，相同说明这个对象里的粒子已经静止了，不相同说明正在运动，那就清除之前的渲染位置的粒子纹理，从而实现粒子纹理位移
                if (dicMapLivePixelStack[keys[j]].topColor != lastTopColor)
                    dicMapLivePixelStack[keys[j]].needsDraw = true;
            }

            keys.Clear();

            #endregion

            #region -- 20190615 --
            //Dictionary<int, LivePixelStack>.ValueCollection stacksValues = dicMapLivePixelStack.Values;
            //var enumerator = stacksValues.GetEnumerator();
            //while (enumerator.MoveNext())
            //{
            //    stacks.Add(enumerator.Current);
            //}


            //foreach (LivePixelStack stack in stacks) 
            //{
            //    List<LivePixel> pixels = stack.livePixels;

            //    if (pixels.Count == 0)
            //        continue;

            //    Color lastTopColor = stack.topColor;

            //    key = KeyForXY(stack.x, stack.y);

            //    for (i = pixels.Count - 1; i >= 0; i--)
            //    {
            //        LivePixel pixel = pixels[i];

            //        if (pixel.lastUpdateFrame == curframe)
            //            continue;

            //        pixel.Update(this);

            //        pixel.lastUpdateFrame = curframe;

            //        if (i >= pixels.Count || pixels[i] != pixel)        //这个粒子可能已经死了
            //            continue;

            //        newkey = KeyForXY(pixel.x, pixel.y);

            //        if (pixel.isDead)
            //        {
            //            //快把这对象里的粒子移除了，因为他死了
            //            pixels.RemoveAt(i);
            //            //如果这个粒子像素是透明的，那就不要覆盖背景了
            //            if (pixel.color.a > 0)
            //                SetPixel(pixel.x, pixel.y, pixel.color);    //如果是单一像素,so 单一颜色 colors.length = 1··
            //            Recycle(pixel);
            //        }
            //        else if (newkey != key)
            //        {
            //            // 这个粒子已经移动了.  那就把它从池子里移除
            //            // 然后再把新的粒子添加进池子来替换他
            //            pixels.RemoveAt(i);
            //            AddToStack(pixel, pixel.x, pixel.y);
            //        }
            //    }

            //    //检查这两个对象(stack)，颜色是否相同，相同说明这个对象里的粒子已经静止了，不相同说明正在运动，那就清除之前的渲染位置的粒子纹理，从而实现粒子纹理位移
            //    if (stack.topColor != lastTopColor)
            //        stack.needsDraw = true;
            //}

            //stacks.Clear();
            #endregion

        }
        
        //VAR
        NoGCList<int> condemneds = new NoGCList<int>(3000, 10);
        /// <summary>
        /// 这是零GC代码噢
        /// </summary>
        private void LateUpdateToDoSth()
        {
            //TODO..遍历 对象（还没被回收的）池子中的所有对象粒子
            //TODO..把所有之前被标记成需要渲染的粒子都拿去渲染
            //TODO..如果对象粒子中的粒子集合已经空了，别忘了把他丢到垃圾桶！
            //Start..

            if (dicMapLivePixelStack == null || dicMapLivePixelStack.Count == 0) return;

            int i;

            foreach (LivePixelStack stack in dicMapLivePixelStack.Values)
            {
                if (stack.needsDraw)
                {
                    //承接updatetodosth中的最后一段话
                    DrawStack(stack);
                    stack.needsDraw = false;
                }

                if (stack.livePixels.Count == 0)
                {
                    if (condemneds == null)
                        condemneds = new NoGCList<int>(3000, 10);

                    condemneds.Add(KeyForXY(stack.x, stack.y));
                }
            }

            if (condemneds != null && condemneds.Count != 0)
            {
                for (i = 0; i < condemneds.Count; i++)
                {
                    //进行回收操作(livepixel)
                    lpsRecyclePool.Push(dicMapLivePixelStack[condemneds[i]]);
                    dicMapLivePixelStack.Remove(condemneds[i]);
                }
            }

            condemneds.Clear();
        }

        #endregion

        #region -- Helper --
        void NeedApplyAtTile(int col, int row)
        {
            palletteData.tileNeedsApply[col, row] = true;
            anyNeedsApply = true;
        }

        int KeyForXY(int x, int y)
        {
            return y * pallettePrevData.prevTotalWidth + x;
        }

        #endregion

    }


    class CPUVersion
    {
        #region -- Draw APIMethods --
        internal void DrawFrame(ref PassData passData, ref Texture2D[,] tileTexs, ref Texture2D frame)
        {
            Texture2D tex = tileTexs[passData.col, passData.row];  //CPU

            Color[] cs = frame.GetPixels(passData.col * passData.width, passData.row * passData.height, passData.width, passData.height);
            if (cs != null && cs.Length != 0)
            {
                tex.SetPixels(0, 0, passData.width, passData.height, cs);  //CPU
            }
            else
            {
                //frame某个区域没有像素那就画color.clear
                int length = passData.width * passData.height;
                cs = new Color[length];

                for (int i = 0; i < length; i++)
                {
                    cs[length] = Color.clear;
                }

                tex.SetPixels(0, 0, passData.width, passData.height, cs);   //CPU

            }

            // TODO..纹理Apply
        }

        internal void SetPixelIgnoringStacks(ref PassData passData, ref Texture2D[,] tileTexs)
        {
            Texture2D tex = tileTexs[passData.col, passData.row];             //CPU

            int tx = passData.x % passData.width;
            int ty = passData.y % passData.height;

            Color cs = tex.GetPixel(tx, ty);             //CPU

            if (cs == passData.color) return;

            tex.SetPixel(tx, ty, passData.color);      //这是调用了mono里边的更改纹理API     //CPU
        }


        #endregion
    }

    class GPUVersion
    {
        #region -- Draw APIMethods --
        public void DrawFrame(ref PassData passData, ref Texture2D_GPU[,] tileTexsGPU, ref Texture2D frame)
        {
            Texture2D_GPU texGPU = tileTexsGPU[passData.col, passData.row];       //GPU
            Color[] cs = frame.GetPixels(passData.col * passData.width, passData.row * passData.height, passData.width, passData.height);
            if (cs != null && cs.Length != 0)
            {
                texGPU.SetPixels(cs);     //GPU
            }
            else
            {
                //frame某个区域没有像素那就画color.clear
                int length = passData.width * passData.height;
                cs = new Color[length];

                for (int i = 0; i < length; i++)
                {
                    cs[length] = Color.clear;
                }
                
                texGPU.SetPixels(cs);     //GPU
            }
        }

        public void SetPixelIgnoringStacks(ref PassData passData, ref Texture2D_GPU[,] tileTexsGPU)
        {
            Texture2D_GPU texGPU = tileTexsGPU[passData.col, passData.row];     //GPU

            int tx = passData.x % passData.width;
            int ty = passData.y % passData.height;
            
            Color cs = texGPU.GetPixel(tx, ty);   //GPU

            if (cs == passData.color) return;

            texGPU.SetPixel(tx, ty, passData.color);    //GPU
        }
        #endregion
    }


}