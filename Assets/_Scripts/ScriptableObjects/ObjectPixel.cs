using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxi.OObjects
{
    using OLiOYouxi.OSystem;
    using OLiOYouxiAttributes;

    [CreateAssetMenu(menuName = "OLiOYouxi/Character/ObjectPixel")]
    public class ObjectPixel : AObjectPixel
    {
        public override bool Init()
        {
            dropDown = base.dropDown;
            Name = base.Name;
            brush = base.brush;
            color = base.color; 
            if (Name != string.Empty && color != null)
                return true;
            return false;
        }
    }
}