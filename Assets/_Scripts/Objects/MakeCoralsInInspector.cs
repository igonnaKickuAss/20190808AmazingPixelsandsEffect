using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OLiOYouxi.OObjects
{
    using OLiOYouxi.OSystem;

    [DisallowMultipleComponent]
    public class MakeCoralsInInspector : MonoBehaviour
    {
        #region -- Private Data --
        [OLiOYouxiAttributes.BoxGroup("珊瑚参数"), OLiOYouxiAttributes.Label("必须引用：")]
        [SerializeField]
        private DrawerController drawer = null;
        
        [OLiOYouxiAttributes.BoxGroup("珊瑚参数"), OLiOYouxiAttributes.Label("排量：")]
        [Range(minValues, maxValues)]
        [SerializeField]
        private int cc = 1;

        private EnumCentre.BrushType brush = EnumCentre.BrushType.Coral;
        #endregion

        #region -- Const Data --
        const int maxValues = 20;
        const int minValues = 1;

        #endregion

        //Data
        Vector2 velo = Vector2.zero;
        void Update()
        {
            if (!Input.GetMouseButton(0)) return;

            Vector2 pos;
            int c;
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (drawer.PixelPosAtScreenPos(Input.mousePosition, out pos))
                {
                    pos.Set(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

                    for (c = 0; c < cc; c++)
                    {
                        HelperAPIMethods.RandomVelocity(ref velo, -.5f, .5f, -1f, -1f);
                        drawer.CreateLivePixel<Corals>(pos, brush).velocity = 30f * velo;

                    }
                }
            }
        }
    }

}
