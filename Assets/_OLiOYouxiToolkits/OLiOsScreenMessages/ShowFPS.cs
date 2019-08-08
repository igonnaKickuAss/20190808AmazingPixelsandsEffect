using UnityEngine;

namespace OLiOYouxi.Toolkits
{
    [DisallowMultipleComponent]
	public class ShowFPS : MonoBehaviour
	{
        #region -- Public Data --
        public bool isShow = false;

        #endregion

        #region -- Private Data --
        static private int count = 0;//用于控制帧率的显示速度的count
        static private float milliSecond = 0;//毫秒数
        static private float fps = 0;//帧率值
        static private float deltaTime = 0.0f;//用于显示帧率的deltaTime


        #endregion

        #region -- MONO APIMethods --
        void OnGUI()
        {
            if (!isShow)
                return;

            //左上方帧数显示
            if (++count > 10)
            {
                count = 0;
                milliSecond = deltaTime * 1000.0f;
                fps = 1.0f / deltaTime;
            }
            string text = string.Format(" 当前每帧渲染间隔：{0:0.0} ms ({1:0.} 帧每秒)", milliSecond, fps);
            GUILayout.Label(text);
        }
        
        void Update()
        {
            if (!isShow)
                return;

            //帧数显示的计时delataTime
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        }


        #endregion



    }
}