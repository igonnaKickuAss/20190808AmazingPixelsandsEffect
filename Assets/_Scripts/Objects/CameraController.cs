using UnityEngine;

namespace OLiOYouxi.OObjects
{

    [DisallowMultipleComponent]
    public class CameraController : MonoBehaviour
    {
        #region -- SerializeField --
        [SerializeField] private Vector2 m_LerpRate = new Vector2(10f, 1f);
        [SerializeField] private Rigidbody2D m_Player = null;
        [SerializeField] private BoxCollider2D m_Area = null;
        [SerializeField] private BoxCollider2D c_Area = null;
        #endregion

        #region -- Private Data --
        private Vector3 AimPosition;
        private Bounds AreaMap;
        private Bounds AreaCSight;

        #endregion

        #region -- VAR --
        float radiusX = 0;
        float radiusY = 0;

        #endregion

        #region -- MONO APIMethods --
        private void Awake()
        {
            //拿到摄像机当前瞄准目标
            AimPosition = transform.position;

            //拿到地图区域
            AreaMap = m_Area.bounds;
            m_Area.enabled = false;

            //拿到摄像机视野区域
            AreaCSight = c_Area.bounds;
            c_Area.enabled = false;

            //算出摄像机事视野区域的x半径
            radiusX = AreaCSight.size.x / 2;

            //算出摄像机事视野区域的y半径
            radiusY = AreaCSight.size.y / 2;
        }


        private void LateUpdate()
        {
            //延迟更新瞄准目标
            transform.position = AimPosition;
        }


        void FixedUpdate()
        {
            //x-axis
            if (m_Player.position.x > (AreaMap.min.x + radiusX) && m_Player.position.x < (AreaMap.max.x - radiusX))
            {
                AimPosition.x = Mathf.Lerp(
                    transform.position.x,
                    m_Player.position.x,
                    Time.deltaTime * m_LerpRate.x
                );
            }
            else if (m_Player.position.x <= (AreaMap.min.x + radiusX))
            {
                AimPosition.x = Mathf.Lerp(
                    transform.position.x,
                    AreaMap.min.x + radiusX,
                    Time.deltaTime * m_LerpRate.x
                );
            }
            else if (m_Player.position.x >= (AreaMap.max.x - radiusX))
            {
                AimPosition.x = Mathf.Lerp(
                    transform.position.x,
                    AreaMap.max.x - radiusX,
                    Time.deltaTime * m_LerpRate.x
                );
            }

            //y-axis


            if (m_Player.position.y > (AreaMap.min.y + radiusY) && m_Player.position.y < (AreaMap.max.y - radiusY))
            {
                AimPosition.y = Mathf.Lerp(
                    transform.position.y,
                    m_Player.position.y,
                    Time.deltaTime * m_LerpRate.y
                );
            }
            else if (m_Player.position.y <= (AreaMap.min.y + radiusY))
            {
                AimPosition.y = Mathf.Lerp(
                    transform.position.y,
                    AreaMap.min.y + radiusY,
                    Time.deltaTime * m_LerpRate.y
                );
            }
            else if (m_Player.position.y >= (AreaMap.max.y - radiusY))
            {
                AimPosition.y = Mathf.Lerp(
                    transform.position.y,
                    AreaMap.max.y - radiusY,
                    Time.deltaTime * m_LerpRate.y
                );
            }
            
            //z-axis
            AimPosition.z = transform.position.z;
        }
        #endregion
        
    }
}