using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxi.OObjects
{
    using OLiOYouxi.OSystem;
    using OLiOYouxi.OSystem.Helpers;

    [RequireComponent(typeof(DrawerController), typeof(BrushController))]
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
	public class PalletteController : MonoBehaviour
	{
        #region -- 单例 --
        static private PalletteController pallatteController = null;
        static public PalletteController instance
        {
            get
            {
                if (!pallatteController)
                {
                    pallatteController = FindObjectOfType<PalletteController>() as PalletteController;
                    if (!pallatteController)
                    {
                        DebuggerFather.instance.ToDebugLogErr("你得在Scene中激活一个携带PallatteController组件的GameObject。", EnumCentre.ColorName.red);
                    }
                    else
                    {
                        pallatteController.InitData();
                    }
                }
                return pallatteController;
            }
        }

        private void InitData()
        {
            this.palleteData = new PalletteDataModel()
            {
                tileObjs = new GameObject[tilesCols, tilesRows],
                tileTexs = new Texture2D[tilesCols, tilesRows],
                tileMats = new Material[tilesCols, tilesRows],
                tileNeedsApply = new bool[tilesCols, tilesRows]
            };

            this.palletePrevData = new PallettePrevDataModel
            {
                prevTileWidth = tileWidth,
                prevTilesCols = tilesCols,
                prevTileHeight = tileHeight,
                prevTilesRows = tilesRows,
                prevTotalWidth = totalWidth,
                prevTotalHeight = totalHeight,
                prevRenderStartEndW = renderStartEndH,
                prevRenderStartEndH = renderStartEndH,
                prevPixelsPerUnit = pixelsPerUnit,
                prevStartWithTestPattern = startWithPattern
            };
        }

        #endregion

        #region -- Public Data --
        public PalletteDataModel palleteData = null;
        public PallettePrevDataModel palletePrevData = null;

        #endregion

        #region -- Private Data --
        //瓦片
        [OLiOYouxiAttributes.BoxGroup("瓦片"), OLiOYouxiAttributes.Label("宽度：")]
        [OLiOYouxiAttributes.MinValue(1), OLiOYouxiAttributes.MaxValue(totalWidth)]
        [OLiOYouxiAttributes.OnValueChanged("CalculateCols")]
        [SerializeField]
        private int tileWidth = 80;

        [OLiOYouxiAttributes.BoxGroup("瓦片"), OLiOYouxiAttributes.Label("列数：")]
        [OLiOYouxiAttributes.MinValue(1), OLiOYouxiAttributes.MaxValue(totalWidth)]
        [OLiOYouxiAttributes.OnValueChanged("CalculateWidth")]
        [SerializeField]
        private int tilesCols = 12;

        [OLiOYouxiAttributes.BoxGroup("瓦片"), OLiOYouxiAttributes.Label("高度：")]
        [OLiOYouxiAttributes.MinValue(1), OLiOYouxiAttributes.MaxValue(totalHeight)]
        [OLiOYouxiAttributes.OnValueChanged("CalculateRows")]
        [SerializeField]
        private int tileHeight = 80;


        [OLiOYouxiAttributes.BoxGroup("瓦片"), OLiOYouxiAttributes.Label("行数：")]
        [OLiOYouxiAttributes.MinValue(1), OLiOYouxiAttributes.MaxValue(totalHeight)]
        [OLiOYouxiAttributes.OnValueChanged("CalculateHeight")]
        [SerializeField]
        private int tilesRows = 8;

        //渲染范围
        [OLiOYouxiAttributes.BoxGroup("渲染范围")]
        [OLiOYouxiAttributes.MinMaxSlider(0, screenWidth)]
        [SerializeField]
        private Vector2 renderStartEndW = Vector2.one;

        [OLiOYouxiAttributes.BoxGroup("渲染范围")]
        [OLiOYouxiAttributes.MinMaxSlider(0, screenHeight)]
        [SerializeField]
        private Vector2 renderStartEndH = Vector2.one;


        //杂项
        [OLiOYouxiAttributes.BoxGroup("杂项"), OLiOYouxiAttributes.Label("像素每单元：")]
        [OLiOYouxiAttributes.MinValue(1)]
        [SerializeField]
        private float pixelsPerUnit = 80f;

        [OLiOYouxiAttributes.BoxGroup("杂项"), OLiOYouxiAttributes.Label("开启模板：")]
        [SerializeField]
        private bool startWithPattern = false;

        [OLiOYouxiAttributes.BoxGroup("杂项")]
        [OLiOYouxiAttributes.AssetPreview(80, 80)]
        [OLiOYouxiAttributes.ShowIf("startWithPattern")]
        [SerializeField]
        private Sprite preview = null;

        [OLiOYouxiAttributes.BoxGroup("杂项"), OLiOYouxiAttributes.Label("瓦片着色器：")]
        [SerializeField]
        private Shader tileShader = null;

        #endregion

        #region -- Const Data --
        const int totalWidth = 480;
        const int totalHeight = 320;
        const int screenWidth = 960;
        const int screenHeight = 640;

        #endregion

        #region -- OnValueChange APIMethods --
        void CalculateCols()
        {
            tileWidth = tileWidth <= 0 ? 1 : tileWidth;
            tilesCols = Mathf.CeilToInt(totalWidth / tileWidth);
        }

        void CalculateWidth()
        {
            tilesCols = tilesCols <= 0 ? 1 : tilesCols;
            tileWidth = Mathf.CeilToInt(totalWidth / tilesCols);
        }

        void CalculateRows()
        {
            tileHeight = tileHeight <= 0 ? 1 : tileHeight;
            tilesRows = Mathf.CeilToInt(totalHeight / tileHeight);
        }

        void CalculateHeight()
        {
            tilesRows = tilesRows <= 0 ? 1 : tilesRows;
            tileHeight = Mathf.CeilToInt(totalHeight / tilesRows);
        }

        #endregion

        #region -- MONO APIMethods --

        #endregion

        #region -- Public APIMethods --
        [OLiOYouxiAttributes.Button("创建瓦片对象")]
        public bool CreateTileGameObjects()
        {
            if (tileShader == null)
                tileShader = Shader.Find("Sprites/Default");

            int cols = tilesCols;
            int rows = tilesRows;
            
            //下面这个两行代码很好的解释了 pixel 和 pixelsPerUnit在世界中的关系
            float worldTileWidth = tileWidth / pixelsPerUnit;
            float worldTileHeight = tileHeight / pixelsPerUnit;

            //初始化瓦片数据
            if (Application.isPlaying)
            {
                this.palleteData.tileObjs = new GameObject[cols, rows];
                this.palleteData.tileTexs = new Texture2D[cols, rows];
                this.palleteData.tileMats = new Material[cols, rows];
                this.palleteData.tileNeedsApply = new bool[cols, rows];
            }


            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    //生成瓦片
                    GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Quad);

                    tile.name = "Tile " + col + " ," + row;
                    tile.transform.localScale = new Vector3(worldTileWidth, worldTileHeight, 1f);
                    tile.transform.localPosition = new Vector3(worldTileWidth * (col + .5f), worldTileHeight * (row + .5f), 1f);
                    tile.transform.SetParent(transform, false);

                    Material material = new Material(tileShader);
                    material.SetFloat("PixelSnap", 1f);

                    //使用这一个材质球
                    tile.GetComponent<Renderer>().sharedMaterial = material;

                    //新建一个纹理
                    Texture2D texture2D = new Texture2D(tileWidth, tileHeight, TextureFormat.ARGB32, false);
                    texture2D.wrapMode = TextureWrapMode.Repeat;
                    texture2D.filterMode = FilterMode.Point;
                    
                    material.mainTexture = texture2D;

                    //赋值给tileTexs
                    if (Application.isPlaying)
                    {
                        this.palleteData.tileTexs[col, row] = texture2D;
                    }

                    //是否给Tile放置模板
                    if (startWithPattern) 
                    {
                        DrawPattern(ref texture2D);
                    }
                    else
                    {
                        material.mainTexture = null;
                        material.color = Color.clear;
                    }
                }
            }

            if (Application.isPlaying)
            {
                this.palletePrevData.prevTileWidth = tileWidth;
                this.palletePrevData.prevTilesCols = tilesCols;
                this.palletePrevData.prevTileHeight = tileHeight;
                this.palletePrevData.prevTilesRows = tilesRows;
                this.palletePrevData.prevTotalWidth = totalWidth;
                this.palletePrevData.prevTotalHeight = totalHeight;
                this.palletePrevData.prevPixelsPerUnit = pixelsPerUnit;
                this.palletePrevData.prevStartWithTestPattern = startWithPattern;
            }

            return true;
        }

        [OLiOYouxiAttributes.Button("销毁所有瓦片对象")]
        public bool ClearTileGameObjects()
        {
            if (Application.isPlaying && this.palleteData != null)
            {
                this.palleteData.tileObjs = null;
                this.palleteData.tileTexs = null;
                this.palleteData.tileMats = null;
                this.palleteData.tileNeedsApply = null;
                Resources.UnloadUnusedAssets();
            }
            transform.ClearChildrenImmediately();
            return true;
        }

        #endregion

        #region -- Private APIMethods --
        /// <summary>
        /// 绘制模板
        /// </summary>
        /// <param name="texture2D"></param>
        private void DrawPattern(ref Texture2D texture2D)
        {
            //Set and Apply
            for (int y = 0; y < texture2D.height; y++)
            {
                for (int x = 0; x < texture2D.width; x++)
                {
                    Color color = ((x & y) != 0 ? Color.white : Color.gray);
                    texture2D.SetPixel(x, y, color);
                }
            }
            texture2D.Apply();
        }

        #endregion

    }
}