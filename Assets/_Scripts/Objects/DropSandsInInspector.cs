using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OLiOYouxi.OObjects
{
    using OLiOYouxi.OSystem;

    [DisallowMultipleComponent]
    public class DropSandsInInspector : MonoBehaviour
    {
        #region -- Private Data --
        [OLiOYouxiAttributes.BoxGroup("沙子参数"), OLiOYouxiAttributes.Label("必须引用：")]
        [SerializeField]
        private DrawerController drawer = null;

        [OLiOYouxiAttributes.BoxGroup("沙子参数"), OLiOYouxiAttributes.Label("排量：")]
        [Range(minValues, maxValues)]
        [SerializeField]
        private int cc = 1;
        
        private EnumCentre.BrushType brush = EnumCentre.BrushType.Sand;
        
        #endregion

        #region -- Const Data --
        const int maxValues = 10;
        const int minValues = 1;

        #endregion

        #region -- MONO APIMethods --
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
                        drawer.CreateLivePixel<Sands>(pos, brush).velocity =
                            20f * UnityEngine.Random.insideUnitCircle;
                    }
                }
            }

        }

        #endregion

    }
}

