using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OLiOYouxi.OObjects
{
    using OLiOYouxi.OSystem;

    [DisallowMultipleComponent]
    public class DropSnowInInspector : MonoBehaviour
    {
        #region -- Private Data --
        [OLiOYouxiAttributes.BoxGroup("雪花参数"), OLiOYouxiAttributes.Label("必须引用：")]
        [SerializeField]
        private DrawerController drawer = null;

        [OLiOYouxiAttributes.BoxGroup("雪花参数"), OLiOYouxiAttributes.Label("排量：")]
        [Range(minValues, maxValues)]
        [SerializeField]
        private int cc = 1;

        [OLiOYouxiAttributes.BoxGroup("雪花参数"), OLiOYouxiAttributes.Label("降雪概率：")]
        [Range(minValues, maxValues * 10)]
        [SerializeField]
        private int probability = 10;

        [OLiOYouxiAttributes.BoxGroup("雪花参数"), OLiOYouxiAttributes.Label("降雪延迟：")]
        [Range(minValues, maxValues)]
        [SerializeField]
        private int snowFallDelay = 1;

        [OLiOYouxiAttributes.BoxGroup("雪花参数"), OLiOYouxiAttributes.Label("继续降雪：")]
        [SerializeField]
        public bool isSnow = true;

        private EnumCentre.BrushType brush = EnumCentre.BrushType.Snow;

        private Coroutine snowFallCoro = null;

        #endregion
        
        #region -- Const Data --
        const int maxValues = 10;
        const int minValues = 1;
        
        #endregion

        #region -- Mono APIMethods --
        private void OnEnable()
        {
            if (snowFallCoro != null)
                return;

            snowFallCoro = StartCoroutine(SnowFall());
        }

        private void OnDisable()
        {
            if (!isSnow)
            {
                StopAllCoroutines();
                isSnow = !isSnow;
            }
        }
        public void Cancel()
        {
            StopAllCoroutines();
        }

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
                        drawer.CreateLivePixel<Snow>(pos, brush);
                    }
                }
            }
        }
        #endregion

        #region -- Corotines --
        private IEnumerator SnowFall()
        {
            Vector2 pos = Vector2.zero;
            PallettePrevDataModel pallettePrevData = null;
            drawer.OutPallettePrevData(out pallettePrevData);

            while (Application.isPlaying) 
            {
                yield return YieReturns.instance.GetWaitForFixedUpdate();       //等待fixedUpdate
                if (Time.frameCount % snowFallDelay == 0)
                {
                    if (UnityEngine.Random.Range(0, 100) < probability)
                    {
                        int x = Random.Range(0, pallettePrevData.prevTotalWidth);
                        pos.Set(x, pallettePrevData.prevTotalHeight);
                        drawer.CreateLivePixel<Snow>(pos, brush);
                    }
                }
            }
        }
        #endregion
    }

}
