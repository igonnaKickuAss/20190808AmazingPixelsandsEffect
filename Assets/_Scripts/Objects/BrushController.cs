using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OLiOYouxi.OObjects
{
    using OLiOYouxi.OSystem;

    [DisallowMultipleComponent]
    public class BrushController : MonoBehaviour
    {
        #region -- Private Data --
        [OLiOYouxiAttributes.BoxGroup("笔刷数据")]
        [OLiOYouxiAttributes.Dropdown("dropDown")]
        [OLiOYouxiAttributes.OnValueChanged("OnBrushTypeChange")]
        [SerializeField]
        private EnumCentre.BrushType brush;

        [OLiOYouxiAttributes.BoxGroup("笔刷数据"), OLiOYouxiAttributes.Label("必须引用：")]
        [SerializeField]
        private Transform brushes = null;


        [OLiOYouxiAttributes.BoxGroup("笔刷数据"), OLiOYouxiAttributes.Label("扔沙子：")]
        [SerializeField]
        private DropSandsInInspector dropSands;

        [OLiOYouxiAttributes.BoxGroup("笔刷数据"), OLiOYouxiAttributes.Label("扔雪：")]
        [SerializeField]
        private DropSnowInInspector dropSnow;

        [OLiOYouxiAttributes.BoxGroup("笔刷数据"), OLiOYouxiAttributes.Label("倒水：")]
        [SerializeField]
        private PullWaterInInspector pullWater;

        [OLiOYouxiAttributes.BoxGroup("笔刷数据"), OLiOYouxiAttributes.Label("生火：")]
        [SerializeField]
        private MakeFireInInspector makeFire;

        [OLiOYouxiAttributes.BoxGroup("笔刷数据"), OLiOYouxiAttributes.Label("生珊瑚：")]
        [SerializeField]
        private MakeCoralsInInspector makeCorals;

        [OLiOYouxiAttributes.BoxGroup("笔刷数据"), OLiOYouxiAttributes.Label("生炸弹：")]
        [SerializeField]
        private MakeExplosionInInspector makeBoom;

        [OLiOYouxiAttributes.BoxGroup("笔刷数据"), OLiOYouxiAttributes.Label("下雨：")]
        [SerializeField]
        private RainInInspector rainFall;

        #endregion

        #region -- VAR --
        OLiOYouxiAttributes.DropdownList<EnumCentre.BrushType> dropDown = new OLiOYouxiAttributes.DropdownList<EnumCentre.BrushType>
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

        #region -- OnValueChange APIMethods --
        private void OnBrushTypeChange()
        {
            SetBrush(brush);
        }

        #endregion

        #region -- Mono APIMethods --
        private void OnEnable()
        {
            if (dropSands == null || dropSnow == null || pullWater == null || makeFire == null || makeCorals == null || makeBoom == null || rainFall == null)
            {
                dropSands = brushes.GetComponent<DropSandsInInspector>();
                dropSnow = brushes.GetComponent<DropSnowInInspector>();
                pullWater = brushes.GetComponent<PullWaterInInspector>();
                makeFire = brushes.GetComponent<MakeFireInInspector>();
                makeCorals = brushes.GetComponent<MakeCoralsInInspector>();
                makeBoom = brushes.GetComponent<MakeExplosionInInspector>();
                rainFall = brushes.GetComponent<RainInInspector>();
            }
        }


        #endregion

        public void CancelSnow()
        {
            dropSnow.Cancel();
        }

        public void CancelRain()
        {
            rainFall.Cancel();
        }

        public void SetBrush(EnumCentre.BrushType brush)
        {
            this.brush = brush;
            ChangeBrush();
        }

        #region -- 20190611 --
        //public void SetFlow(int cc)
        //{
        //    switch (this.brush)
        //    {
        //        case Brush.Sand:
        //            dropSands.cc = cc;
        //            slider.minValue = dropSands.minValues;
        //            slider.maxValue = dropSands.maxValues;
        //            break;
        //        case Brush.Water:
        //            pullWater.cc = cc;
        //            slider.minValue = pullWater.minValues;
        //            slider.maxValue = pullWater.maxValues;
        //            break;
        //        case Brush.Fire:
        //            makeFire.cc = cc;
        //            slider.minValue = makeFire.minValues;
        //            slider.maxValue = makeFire.maxValues;
        //            break;
        //        case Brush.Coral:
        //            makeCorals.cc = cc;
        //            slider.minValue = makeCorals.minValues;
        //            slider.maxValue = makeCorals.maxValues;
        //            break;
        //        case Brush.Rain:
        //            rainFall.cc = cc;
        //            slider.minValue = rainFall.minValues;
        //            slider.maxValue = rainFall.maxValues;
        //            break;
        //        case Brush.Snow:
        //            dropSnow.cc = cc;
        //            slider.minValue = dropSnow.minValues;
        //            slider.maxValue = dropSnow.maxValues;
        //            break;
        //        case Brush.Boom:
        //            makeBoom.radius = cc;
        //            slider.minValue = makeBoom.minValues;
        //            slider.maxValue = makeBoom.maxValues;
        //            break;
        //        case Brush.Default:
        //            break;
        //        default:
        //            break;
        //    }
        //}

        #endregion

        public void ChangeBrush()
        {
            dropSands.enabled = brush == EnumCentre.BrushType.Sand;
            pullWater.enabled = brush == EnumCentre.BrushType.Water;
            makeFire.enabled = brush == EnumCentre.BrushType.Fire;
            rainFall.enabled = brush == EnumCentre.BrushType.Rain;
            makeCorals.enabled = brush == EnumCentre.BrushType.Coral;
            dropSnow.enabled = brush == EnumCentre.BrushType.Snow;
            makeBoom.enabled = brush == EnumCentre.BrushType.Boom;
        }

    }


}
