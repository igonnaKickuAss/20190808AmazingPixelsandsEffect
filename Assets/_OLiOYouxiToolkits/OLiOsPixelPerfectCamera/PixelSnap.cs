using UnityEngine;

namespace OLiOYouxi.Toolkits
{
    using OLiOYouxi.OSystem.Helpers;

    [DisallowMultipleComponent]
    public class PixelSnap : MonoBehaviour
    {
        #region -- Private Data --
        private Sprite sprite;
        private Vector3 actualPosition;
        private bool shouldRestorePosition;

        #endregion

        #region -- MONO APIMethods --
        void Start()
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                sprite = renderer.sprite;
            }
            else
            {
                sprite = null;
            }
        }


        void OnWillRenderObject()
        {
            //Debug.Log("on will" + Camera.current);
            Camera cam = Camera.current;
            if (!cam)
                return;

            PixelPerfectCamera pixelPerfectCamera = cam.GetComponent<PixelPerfectCamera>();
            bool retroSnap = (pixelPerfectCamera == null) ? false : pixelPerfectCamera.retroSnap;

#if !REDUCE_JITTER
            if (!retroSnap)
                return;
#endif

            shouldRestorePosition = true;
            actualPosition = transform.position;

            float cameraPPU = (float)cam.pixelHeight / (2f * cam.orthographicSize);
            float cameraUPP = 1.0f / cameraPPU;

            Vector2 camPos = cam.transform.position.xy();
            Vector2 pos = actualPosition.xy();
            Vector2 relPos = pos - camPos;

            Vector2 offset = new Vector2(0, 0);
            // 如果屏幕大小为奇数，这个就是屏幕像素边缘的偏移量
            offset.x = (cam.pixelWidth % 2 == 0) ? 0 : 0.5f;
            offset.y = (cam.pixelHeight % 2 == 0) ? 0 : 0.5f;
            // 精灵图锚点的偏移量
            Vector2 pivotOffset = new Vector2(0, 0);
            if (sprite != null)
            {
                pivotOffset = sprite.pivot - new Vector2(Mathf.Floor(sprite.pivot.x), Mathf.Floor(sprite.pivot.y)); // the fractional part in texture pixels           
                if (retroSnap)
                {
                    // 这里没什么可做的。主偏移量已经以资产像素为单位。
                }
                else
                {
                    float camPixelsPerAssetPixel = cameraPPU / sprite.pixelsPerUnit;
                    pivotOffset *= camPixelsPerAssetPixel; // convert to screen pixels
                }
            }
            if (retroSnap)
            {
                float assetPPU = pixelPerfectCamera.assetsPixelsPerUnit;
                float assetUPP = 1.0f / assetPPU;
                float camPixelsPerAssetPixel = cameraPPU / assetPPU;

                offset.x /= camPixelsPerAssetPixel; // 零个或半个屏幕像素的纹理像素
                offset.y /= camPixelsPerAssetPixel;
                // 当四舍五入时，我们不考虑轴心的偏移量，这是为了避免由于轴心的小数部分而出现两个精灵在不同时间移动的伪影。
                relPos.x = (Mathf.Round(relPos.x / assetUPP - offset.x) + offset.x + pivotOffset.x) * assetUPP;
                relPos.y = (Mathf.Round(relPos.y / assetUPP - offset.y) + offset.y + pivotOffset.y) * assetUPP;

            }
            else
            {
                //将单位都转换为像素，四舍五入，再转换回单位。偏移量确保我们四舍五入的距离是从屏幕像素(片段)边缘到纹理元(texel)边缘。
                relPos.x = (Mathf.Round(relPos.x / cameraUPP - offset.x) + offset.x + pivotOffset.x) * cameraUPP;
                relPos.y = (Mathf.Round(relPos.y / cameraUPP - offset.y) + offset.y + pivotOffset.y) * cameraUPP;
            }

            pos = relPos + camPos;

            transform.position = new Vector3(pos.x, pos.y, actualPosition.z);
        }

        //每个摄像机都会呼叫这个方法
        void OnRenderObject()
        {
            if (shouldRestorePosition)
            {
                shouldRestorePosition = false;
                transform.position = actualPosition;
            }
        }

        #endregion
    }

}

