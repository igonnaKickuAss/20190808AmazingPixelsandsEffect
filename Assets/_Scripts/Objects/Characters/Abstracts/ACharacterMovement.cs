using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxi.OObjects.Character
{
    using OLiOYouxi.OSystem;
    using OLiOYouxi.UnityEventManager;
    using OLiOYouxiAttributes;

    [DisallowMultipleComponent]
    public abstract class ACharacterMovement : MonoBehaviour
    {
        #region -- Fields SerializeField --
        [SerializeField] [BoxGroup("角色物理讯息：")] [Slider(1, 20)] private float m_MoveSpeed = 8f;
        [SerializeField] [BoxGroup("角色物理讯息：")] [Slider(11, 30)] private float m_JumpSpeed = 18f;
        [SerializeField] [BoxGroup("角色物理讯息：")] [Slider(1, 10)] private int m_JumpCount = 2;
        [SerializeField] [BoxGroup("角色物理讯息：")] [MinMaxSlider(0, 100)] private Vector2 m_GravityScaleUD = new Vector2(6f, 12f);        //x: up, y: down;
        [SerializeField] [BoxGroup("角色物理讯息：")] [MinMaxSlider(-100, 100)] private Vector2 m_DropSpeedUD = new Vector2(-24f, 24f);        //x: up, y: down;
        [SerializeField] [BoxGroup("角色物理讯息：")] [Label("地面检测宽度：")] [ReadOnly] private float m_GroundCheckWidth = 0.35f;

        #endregion

        #region -- Invokes SerializeField --
        [SerializeField] [BoxGroup("事件：")] private VoidEvent m_OnJump = null;
        [SerializeField] [BoxGroup("事件：")] private BoolEvent m_Moving = null;
        #endregion

        #region -- Private Data --
        private Rigidbody2D m_Rig = null;
        private RaycastHit2D[] GroundCheckHits = new RaycastHit2D[2];
        private float m_AimSpeed = 0f;
        private bool m_IsGrounded = true;
        private int m_CurrentJumpCount = 0;
        private float m_LastJumpTime = float.MinValue;

        #endregion

        #region -- CONST VAR --
        private const float GROUND_CHECK_GAP = 1f;

        #endregion

        #region -- Private ShotC --
        private Rigidbody2D Rig
        {
            get
            {
                return m_Rig ?? (m_Rig = GetComponent<Rigidbody2D>());
            }
        }


        #endregion

        #region -- Public ShotC --
        public bool IsGrounded
        {
            get
            {
                return m_IsGrounded;
            }
            private set
            {
                m_IsGrounded = value;
            }
        }
        
        public float AimSpeed
        {
            get
            {
                return m_AimSpeed;
            }
            private set
            {
                m_AimSpeed = value;
            }
        }
        
        public float SpeedY
        {
            get
            {
                return Rig.velocity.y;
            }
        }

        #endregion

        #region -- MONO APIMethods --
        protected virtual void Update()
        {
            //插入移动事件
            //m_Moving.Invoke(Mathf.Abs(SpeedY) < 0.1f && Mathf.Abs(AimSpeed) > 0.1f);      
        }



        protected virtual void FixedUpdate()
        {

            // Move
            Rig.velocity = new Vector2(
                AimSpeed,
                Mathf.Clamp(Rig.velocity.y, m_DropSpeedUD.x, m_DropSpeedUD.y)   //x: up, y: down;
            );

            // Ground Check
            IsGrounded = GroundCheck(new Vector2(
                transform.position.x - m_GroundCheckWidth * 0.5f,
                transform.position.y + GROUND_CHECK_GAP
            )) || GroundCheck(new Vector2(
                transform.position.x + m_GroundCheckWidth * 0.5f,
                transform.position.y + GROUND_CHECK_GAP
            ));
            
            // Jump Count
            if (IsGrounded && Time.time > m_LastJumpTime + 0.1f)
            {
                m_CurrentJumpCount = 0;
            }

            // Gravity Scale
            if (Rig.gravityScale == m_GravityScaleUD.x && SpeedY <= 0f)     //x: up
            {
                SwitchGravityScale(false);
            }

        }

        #endregion

        #region -- Public APIMethods --
        public void Move(float speedR)
        {
            m_AimSpeed = Mathf.Clamp(speedR, -1f, 1f) * m_MoveSpeed;
            if (speedR != 0f)
            {
                transform.localScale = new Vector3(speedR > 0f ? 1f : -1f, 1f, 1f);
            }
        }



        public void Jump()
        {
            if (m_CurrentJumpCount < m_JumpCount)
            {
                m_CurrentJumpCount++;
                //插入跳跃事件
                m_OnJump.Invoke();
                SwitchGravityScale(true);
                Rig.velocity = new Vector2(Rig.velocity.x, m_JumpSpeed);
                m_LastJumpTime = Time.time;
            }
        }



        public void SwitchGravityScale(bool isU)
        {
            Rig.gravityScale = isU ? m_GravityScaleUD.x : m_GravityScaleUD.y;
        }



        #endregion

        #region -- Helpers --

        private bool GroundCheck(Vector2 from)
        {
            int len = Physics2D.RaycastNonAlloc(from, Vector2.down, GroundCheckHits, GROUND_CHECK_GAP * 2f);
            return len == 2 || (len == 1 && GroundCheckHits[0].transform != transform);
        }



        #endregion
    }
}

