using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OLiOYouxi.OObjects
{
    using OLiOYouxi.OSystem;

    [DisallowMultipleComponent]
    public class MakeExplosionInInspector : MonoBehaviour
    {
        #region -- Private Data --
        [OLiOYouxiAttributes.BoxGroup("爆炸参数"), OLiOYouxiAttributes.Label("必须引用：")]
        [SerializeField]
        private DrawerController drawer = null;

        [OLiOYouxiAttributes.BoxGroup("爆炸参数"), OLiOYouxiAttributes.Label("半径：")]
        [Range(minValues, maxValues)]
        [SerializeField]
        private int radius = 1;

        private EnumCentre.BrushType brush = EnumCentre.BrushType.Default;

        #endregion

        #region -- Const Data --
        const int maxValues = 60;
        const int minValues = 1;

        #endregion

        //这是一个单例
        static private MakeExplosionInInspector makeExplosionInInspector = null;
        static public MakeExplosionInInspector instance
        {
            get
            {
                if (!makeExplosionInInspector)
                {
                    makeExplosionInInspector = FindObjectOfType<MakeExplosionInInspector>() as MakeExplosionInInspector;
                    if (!makeExplosionInInspector)
                        Debug.LogErrorFormat("<color=red>{0}</color>", "你得在Scene中激活一个携带MakeExplosionInInspector组件的GameObject。");
                    else
                        makeExplosionInInspector.Init();
                }
                return makeExplosionInInspector;
            }
        }

        private void Init()
        {
            drawer = GetComponent<DrawerController>();
        }

        #region -- Mono APIMethods --
        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            if (!EventSystem.current.IsPointerOverGameObject())
            {

                Vector2 pos;

                if (drawer.PixelPosAtScreenPos(Input.mousePosition, out pos))
                {
                    pos.Set(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
                    Splode(pos, radius);
                }
            }
        }
        #endregion


        #region -- Public APIMethods --
        //VAR
        Vector2 pos = Vector2.zero;
        public void Splode(Vector2 origin, int radius = 20)
        {
            if (drawer == null)
                Init();

            ///https://blog.csdn.net/u010141928/article/details/79514514 算法参考该博客
            ///Start....
            
            Rect r = new Rect(origin.x - radius, origin.y - radius, radius * 2, radius * 2);
            int radSqr = radius * radius;     //没有Π的圆的面积

            int yMin = (int)r.yMin, yMax = (int)r.yMax, xMin = (int)r.xMin, xMax = (int)r.xMax, x = (int)origin.x, y = (int)origin.y;
            for (int j = yMin; j < yMax; j++)
            {
                for (int i = xMin; i < xMax; i++)
                {
                    //i,j为当前位置
                    int dSqr = (i - x)*(i - x) + (j - y)*(j - y);   //平面解析几何，操
                    if (dSqr > radSqr)      //越过这个圆的边界了
                        continue;

                    drawer.ClearLivePixels(i, j);     //把这个区域可能存在的活着的对象杀了

                    if (UnityEngine.Random.Range(0, 1f) > .4f)      //有可能可以产生爆炸溅射效果 ，见下面代码。。
                        continue;

                    Color c = drawer.GetPixel(i, j);  //获取这个位置的背景色

                    if (c.a == 0)       //没有东西？那溜了溜了
                        continue;

                    //这是一个爆炸溅射物
                    //万物都是由沙子组成的。。
                    pos.Set(i, j);
                    Dross dross = drawer.CreateLivePixel<Dross>(pos, brush);
                    dross.velocity = (dross.position - origin) * 7f;     //原点坐标指向粒子位置的方向 射出去
                }
            }

            drawer.FillEllipse(r, Color.clear);

        }

        #endregion
    }


}
