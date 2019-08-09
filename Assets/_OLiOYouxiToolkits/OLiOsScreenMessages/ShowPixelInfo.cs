using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxi.Toolkits
{
    using OLiOYouxi.OObjects;

    public class ShowPixelInfo : MonoBehaviour
    {
        #region -- Public Data --
        public bool isShow = false;
        public Rect rect = new Rect(5, -350, 600, 300);
        public DrawerController drawer = null;

        #endregion

        #region -- Private Data --
        private string[] stringArray = null;
        private string sceneText = string.Empty;
        private DrawerController.State stats;
        #endregion

        void OnEnable()
        {
            stringArray = new string[5]
            {
                "仍然活着的对象数量(livePixelStacks活的): ",
                "死掉的对象数量(livePixelStacks死的): ",
                "活着像素数量(livePixels活的): ",
                "已经回收像素数量(livePixels死的): ",
                "总对象回收池子数量(Dic::Stack<LivePixel>): "
            };
        }

        void OnGUI()
        {
            if (!isShow)
                return;

            //在指定位置输出参数信息
            GUI.Label(rect, sceneText);
        }

        void Update()
        {
            if (!isShow)
                return;

            //获取像素粒子信息
            stats = drawer.GetStats();
            sceneText = string.Format(
                "{5}{0}\n{6}{1}\n{7}{2}\n{8}{3}\n{9}{4}",
                stats.livePixelStackCount,
                stats.lpsRecycelCount,
                stats.livePixelCount,
                stats.recyclePixelCount,
                stats.recyclePoolCount,
                stringArray[0],
                stringArray[1],
                stringArray[2],
                stringArray[3],
                stringArray[4]
                );
        }
    }
}