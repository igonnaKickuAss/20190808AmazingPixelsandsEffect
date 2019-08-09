using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;


namespace OLiOYouxi.OObjects
{
    using OLiOYouxi.OSystem;

    [DisallowMultipleComponent]
    public class RainInInspector : MonoBehaviour
    {
        #region -- Private Data --
        [OLiOYouxiAttributes.BoxGroup("雨水参数"), OLiOYouxiAttributes.Label("必须引用：")]
        [SerializeField]
        private DrawerController drawer = null;

        [OLiOYouxiAttributes.BoxGroup("雨水参数"), OLiOYouxiAttributes.Label("排量：")]
        [Range(minValues, maxValues)]
        [SerializeField]
        public int cc = 1;

        [OLiOYouxiAttributes.BoxGroup("雨水参数"), OLiOYouxiAttributes.Label("降雨概率：")]
        [Range(minValues, maxValues * 10)]
        [SerializeField]
        private int probability = 100;

        [OLiOYouxiAttributes.BoxGroup("雨水参数"), OLiOYouxiAttributes.Label("降雨延迟：")]
        [Range(minValues, maxValues / 2)]
        [SerializeField]
        private int rainFallDelay = 1;

        [OLiOYouxiAttributes.BoxGroup("雨水参数"), OLiOYouxiAttributes.Label("继续降雨：")]
        [SerializeField]
        public bool isRain = true;

        private Coroutine rainFallCoro = null;

        private EnumCentre.BrushType brush = EnumCentre.BrushType.Rain;

        #endregion

        #region -- Const Data --
        const int maxValues = 20;
        const int minValues = 1;

        #endregion

        #region -- Mono APIMethods --
        private void OnEnable()
        {
            if (rainFallCoro != null) 
                return;

            rainFallCoro = StartCoroutine(RainFall());
        }

        private void OnDisable()
        {
            if (!isRain)
            {
                StopAllCoroutines();
                isRain = !isRain;
            }
        }

        public void Cancel()
        {
            StopAllCoroutines();
        }

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
                        HelperAPIMethods.RandomVelocity(ref velo, -.5f, 1f, 0, -1f);
                        drawer.CreateLivePixel<Rain>(pos, brush).velocity = 100f * velo;
                    }
                }
            }
        }
        #endregion

        #region -- Corotines --

        private IEnumerator RainFall()
        {
            Vector2 pos = Vector2.zero;
            PallettePrevDataModel pallettePrevData = null;
            drawer.OutPallettePrevData(out pallettePrevData);

            while (Application.isPlaying)
            {
                yield return YieReturns.instance.GetWaitForFixedUpdate();       //等待fixedUpdate
                if (Time.frameCount % rainFallDelay == 0)
                {
                    if (UnityEngine.Random.Range(0, 100) < probability)
                    {
                        int x = UnityEngine.Random.Range(0, pallettePrevData.prevTotalWidth);
                        pos.Set(x, pallettePrevData.prevTotalHeight);
                        drawer.CreateLivePixel<Rain>(pos, brush);
                    }
                }
            }
        }

        #endregion

    }


}
