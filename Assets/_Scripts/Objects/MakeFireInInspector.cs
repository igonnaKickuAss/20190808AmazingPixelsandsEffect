using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

namespace OLiOYouxi.OObjects
{
    using OLiOYouxi.OSystem;

    [DisallowMultipleComponent]
    public class MakeFireInInspector : MonoBehaviour
    {
        #region -- Private Data --

        [OLiOYouxiAttributes.BoxGroup("火焰参数"), OLiOYouxiAttributes.Label("必须引用：")]
        [SerializeField]
        private DrawerController drawer = null;

        [OLiOYouxiAttributes.BoxGroup("火焰参数"), OLiOYouxiAttributes.Label("排量：")]
        [Range(minValues, maxValues)]
        [SerializeField]
        private int cc = 1;

        #endregion

        #region -- Public Data --
        [HideInInspector] public Color fireStartColor;
        [HideInInspector] public Color fireEndColor;
        [HideInInspector] public Color[] flammableColors;

        #endregion

        #region -- Const Data --
        const int maxValues = 6;
        const int minValues = 1;

        #endregion

        //这是一个单例
        static private MakeFireInInspector makeFireInInspector = null;

        static public MakeFireInInspector instance
        {
            get
            {
                if (!makeFireInInspector)
                {
                    makeFireInInspector = FindObjectOfType<MakeFireInInspector>() as MakeFireInInspector;
                    if (!makeFireInInspector)
                        Debug.LogErrorFormat("<color=red>{0}</color>", "你得在Scene中激活一个携带MakeFireInInspector组件的GameObject。");
                    else
                        makeFireInInspector.Init();
                }
                return makeFireInInspector;
            }
        }
        /*颜色表（可以被点燃的颜色）
         *156 73  49
          156 77  57
          156 170 74
          159 101 55
          159 169 71
          162 90  52
          165 93  49
          173 117 49
          181 189 82
          198 210 63
          198 211 66
          222 231 99
          228 242 86
          244 205 101
        */
        private void Init()
        {
            fireStartColor = new Color(255f / 255, 191f / 255, 0, 1f);
            fireEndColor = new Color(255f / 255, 0, 0, 181f / 255);
            flammableColors = new Color[] {
                new Color(156f / 255, 73f / 255, 49f / 255),
                new Color(156f / 255, 77f / 255, 57f / 255),
                new Color(156f / 255, 170f/ 255, 74f / 255),
                new Color(159f / 255, 101f/ 255, 55f / 255),
                new Color(159f / 255, 169f/ 255, 71f / 255),
                new Color(162f / 255, 90f / 255, 52f / 255),
                new Color(165f / 255, 93f / 255, 49f / 255),
                new Color(173f / 255, 117f/ 255, 49f / 255),
                new Color(181f / 255, 189f/ 255, 82f / 255),
                new Color(198f / 255, 210f/ 255, 63f / 255),
                new Color(198f / 255, 211f/ 255, 66f / 255),
                new Color(222f / 255, 231f/ 255, 99f / 255),
                new Color(228f / 255, 242f/ 255, 86f / 255),
                new Color(244f / 255, 205f/ 255, 101f/ 255),
                new Color(101f/255, 69f/255, 27f/255)           //else
            };
        }

        #region -- Mono APIMethods --
        private void Update()
        {
            if (!Input.GetMouseButton(0)) return;
            if (!EventSystem.current.IsPointerOverGameObject())
            {

                Vector2 pos;
                int c;

                if (drawer.PixelPosAtScreenPos(Input.mousePosition, out pos))
                {
                    pos.Set(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

                    for (c = 0; c < cc; c++)
                    {
                        Fire.CreateFlameAt(drawer, pos + UnityEngine.Random.insideUnitCircle * 5f);
                    }
                }
            }
        }

        #endregion

        #region -- Public APIMethods --
        /// <summary>
        /// 判断这是不是可以骚的东西
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool IsFlammable(Color c)
        {
            if (c.a < 0.1f)
                return false;   // 不是那就快点把火拿开！！！
            foreach (Color flammableColor in flammableColors)
            {
                if (c.ApproxEqual(flammableColor))
                    return true;
            }
            return false;
        }

        #endregion

    }

}
