using UnityEngine;

namespace OLiOYouxi.OObjects.Character
{
    using OLiOYouxiAttributes;

    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : ACharacterMovement
    {
        #region -- Fields SerializeField --
        [SerializeField] [BoxGroup("角色操作：")] [Dropdown("Keys")] [Label("向左")] private KeyCode m_Left;
        [SerializeField] [BoxGroup("角色操作：")] [Dropdown("Keys")] [Label("向右")] private KeyCode m_Right;
        [SerializeField] [BoxGroup("角色操作：")] [Dropdown("Keys")] [Label("向上")] private KeyCode m_Jump;
        [SerializeField] [BoxGroup("角色操作：")] [Dropdown("Keys")] [Label("向下")] private KeyCode m_Down;
        [SerializeField] [BoxGroup("角色操作：")] [Dropdown("Keys")] [Label("射击")] private KeyCode m_Shoot;
        #endregion

        #region -- Private Data --
        private float prevLTime = -1f;
        private float prevRTime = -1f;

        private DropdownList<KeyCode> Keys = new DropdownList<KeyCode>()
        {
            {"十字键向左", KeyCode.LeftArrow },
            {"十字键向右", KeyCode.RightArrow },
            {"十字键向下", KeyCode.DownArrow },
            {"十字键跳跃", KeyCode.UpArrow },
            {"十字键射击", KeyCode.K },
            {"WASD向左", KeyCode.A },
            {"WASD键向右", KeyCode.D },
            {"WASD键向下", KeyCode.S },
            {"WASD键跳跃", KeyCode.W },
            {"WASD键射击", KeyCode.T }
        };

        #endregion

        #region -- MONO APIMethods --
        protected override void Update()
        {
            // Move
            float move = 0f;

            if (Input.GetKey(m_Left))
            {
                if (prevLTime < 0f)
                {
                    prevLTime = Time.time;
                }
                if (prevLTime > prevRTime)
                {
                    move = -1f;
                }
            }
            else
            {
                prevLTime = -1f;
            }

            if (Input.GetKey(m_Right))
            {
                if (prevRTime < 0f)
                {
                    prevRTime = Time.time;
                }
                if (prevRTime > prevLTime)
                {
                    move = 1f;
                }
            }
            else
            {
                prevRTime = -1f;
            }

            Move(move);

            // Jump
            if (Input.GetKeyDown(m_Jump))
            {
                Jump();
            }
            if (Input.GetKeyUp(m_Jump))
            {
                SwitchGravityScale(false);
            }

            base.Update();

        }

        #endregion
    }

}
