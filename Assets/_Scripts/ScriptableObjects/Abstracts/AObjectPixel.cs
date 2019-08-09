using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxi.OObjects
{
    using OLiOYouxiAttributes;
    using OLiOYouxi.OSystem;

    public abstract class AObjectPixel : ScriptableObject
	{
        [BoxGroup("像素粒子"), Label("粒子名字：")] public string Name;     //名称
        [BoxGroup("像素粒子")] [Dropdown("dropDown")] public EnumCentre.BrushType brush;       //笔刷
        [BoxGroup("像素粒子"), Label("颜色：")] public Color color = new Color(0.9f, 0.6f, 0.3f, 1f);         //单个像素颜色

        public abstract bool Init();

        #region -- VAR --
        protected DropdownList<EnumCentre.BrushType> dropDown = new DropdownList<OSystem.EnumCentre.BrushType>
        {
            {"沙子", EnumCentre.BrushType.Sand},
            {"水流", EnumCentre.BrushType.Water},
            {"火焰", EnumCentre.BrushType.Fire},
            {"珊瑚", EnumCentre.BrushType.Coral},
            {"雨水", EnumCentre.BrushType.Rain},
            {"雪花", EnumCentre.BrushType.Snow},
            {"爆炸", EnumCentre.BrushType.Boom},
            {"默认", EnumCentre.BrushType.Default}
        };

        #endregion
    }
}