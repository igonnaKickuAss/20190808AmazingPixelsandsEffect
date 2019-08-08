using UnityEngine;

namespace OLiOYouxi.OSystem
{
    public class DebuggerFather
    {
        //单例
        static private DebuggerFather debuggerFather = null;
        static public DebuggerFather instance
        {
            get
            {
                if (debuggerFather != null)
                {
                    return debuggerFather;
                }
                else
                {
                    debuggerFather = new DebuggerFather();
                    debuggerFather.Init();
                    return debuggerFather;
                }

            }
        }


        //Data
        private string colorString = string.Empty;
        private string colorName = string.Empty;
        private void Init()
        {
            colorString = "<color=>{0}</color>";
        }

        public void ToDebugLog(string message, EnumCentre.ColorName color)
        {
            colorName = string.Empty;
            HelperAPIMethods.EnumToString(color, out colorName);
            if (colorName == string.Empty)
                return;
            Debug.LogFormat(colorString.Insert(7, colorName), message);
        }

        public void ToDebugLogWarn(string message, EnumCentre.ColorName color)
        {
            colorName = string.Empty;
            HelperAPIMethods.EnumToString(color, out colorName);
            if (colorName == string.Empty)
                return;
            Debug.LogWarningFormat(colorString.Insert(7, colorName), message);
        }

        public void ToDebugLogErr(string message, EnumCentre.ColorName color)
        {
            colorName = string.Empty;
            HelperAPIMethods.EnumToString(color, out colorName);
            if (colorName == string.Empty)
                return;
            Debug.LogErrorFormat(colorString.Insert(7, colorName), message);
        }
    }
}
