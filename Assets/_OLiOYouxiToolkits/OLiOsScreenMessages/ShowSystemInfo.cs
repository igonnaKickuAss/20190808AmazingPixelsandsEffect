using UnityEngine;

namespace OLiOYouxi.Toolkits
{
	public class ShowSystemInfo : MonoBehaviour
	{
        #region -- Public Data --
        public bool isShow = false;
        public Rect rect = new Rect(5, -25, 600, 300);

        #endregion

        #region -- Private Data --
        private string systemInfoLabel;

        #endregion


        void OnGUI()
        {
            if (!isShow)
                return;

            //在指定位置输出参数信息
            GUI.Label(rect, systemInfoLabel);
        }

        void Update()
        {
            if (!isShow)
                return;

            //获取参数信息
            systemInfoLabel = " \n\n\nCPU型号：" + SystemInfo.processorType + "\n (" + SystemInfo.processorCount +
                   " cores核心数, " + SystemInfo.systemMemorySize + "MB RAM内存)\n " + "\n 显卡型号：" + SystemInfo.graphicsDeviceName + "\n " +
                   Screen.width + "x" + Screen.height + " @" + Screen.currentResolution.refreshRate +
                   " (" + SystemInfo.graphicsMemorySize + "MB VRAM显存)";
        }

    }
}