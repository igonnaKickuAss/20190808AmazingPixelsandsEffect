using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

namespace OLiOYouxi.OObjects
{
    using OLiOYouxi.OSystem;

    [DisallowMultipleComponent]
    public class PullWaterInInspector : MonoBehaviour
    {
        #region -- Private Data --
        [OLiOYouxiAttributes.BoxGroup("水流参数"), OLiOYouxiAttributes.Label("必须引用：")]
        [SerializeField]
        private DrawerController drawer = null;

        [OLiOYouxiAttributes.BoxGroup("水流参数"), OLiOYouxiAttributes.Label("排量：")]
        [Range(minValues, maxValues)]
        [SerializeField]
        private int cc = 1;

        private EnumCentre.BrushType brush = EnumCentre.BrushType.Water;

        #endregion

        #region -- Public Data --
        const int maxValues = 50;
        const int minValues = 1;

        #endregion

        //VAR
        Vector2 velo = Vector2.zero;

        void Update()
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
                        HelperAPIMethods.RandomVelocity(ref velo, -.2f, .5f, 0, -1f);
                        drawer.CreateLivePixel<Water>(pos, brush).velocity = 60f * velo;

                    }
                }
            }
        }
    }


}
