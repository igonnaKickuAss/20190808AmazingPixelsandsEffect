using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxi.OObjects.Character
{
    using OLiOYouxi.OSystem;
    using OLiOYouxiAttributes;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        #region -- Fields SerializeField --
        [SerializeField] [ReadOnly] private string m_MovingKey = "Moving";
        [SerializeField] [ReadOnly] private string m_JumpingKey = "Jumping";
        [SerializeField] [ReadOnly] private string m_SpeedYKey = "SpeedY";

        #endregion

        #region -- Private Data --
        private Animator m_Ani = null;
        private PlayerMovement m_Movement = null;

        #endregion

        #region -- Private ShotC --
        private Animator Ani
        {
            get
            {
                return m_Ani ?? (m_Ani = GetComponent<Animator>());
            }
        }

        private PlayerMovement Movement
        {
            get
            {
                return m_Movement ?? (m_Movement = GetComponentInParent<PlayerMovement>());
            }
        }

        #endregion

        #region -- MONO APIMethods --
        private void Update()
        {
            Ani.SetBool(m_MovingKey, Mathf.Abs(Movement.AimSpeed) > 0.01f);
            Ani.SetBool(m_JumpingKey, !Movement.IsGrounded && Mathf.Abs(Movement.SpeedY) > 0.1f);
            Ani.SetFloat(m_SpeedYKey, Movement.SpeedY);
        }

        #endregion
    }
}