using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxi.OObjects
{
    using System;
    using OLiOYouxi.OSystem;

    [DisallowMultipleComponent]
    [ExecuteInEditMode]
	public class PallatteController : MonoBehaviour
	{
        #region -- 单例 --
        static private PallatteController pallatteController = null;
        static public PallatteController instance
        {
            get
            {
                if (!pallatteController)
                {
                    pallatteController = FindObjectOfType<PallatteController>() as PallatteController;
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

        }

        #endregion

        #region -- Private Data --
        private int tileWidth = 80;
        private int tileHeight = 80;

        #endregion

    }
}