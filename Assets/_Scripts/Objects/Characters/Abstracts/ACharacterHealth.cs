using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxi.OObjects
{
    using OLiOYouxi.UnityEventManager;
    using OLiOYouxiAttributes;

    [DisallowMultipleComponent]
    public abstract class ACharacterHealth : MonoBehaviour, IObjectHealth
    {
        #region -- Protected Data --
        [Required] [SerializeField] protected ObjectHealth targetHealth = null;
        protected bool isUpdateData = false;
        #endregion

        #region -- Private Data --
        private bool _isDead = false;

        #endregion
        
        #region -- Invokes SerializeField --
        [SerializeField] [BoxGroup("事件：")] private VoidEvent m_OnDead = null;
        [SerializeField] [BoxGroup("事件：")] private VoidEvent m_OnBirth = null;
        #endregion

        #region -- ShotC --
        public bool isDead
        {
            get
            {
                return _isDead;
            }
            private set
            {
                if (value == false)
                {
                    //TODO..
                    _isDead = value;
                    return;
                }

                //Start..
                //把血加满
                //确定要更新数据
                //生命总量减一 。。。。。生命总量为零时才是真正的死掉了
                //Invoke..死亡事件
                //检查是否还有剩余生命
                //TODO..
                InitHealthData();

                isUpdateData = true;

                LifeDecreased();

                m_OnDead.Invoke();

                if (CheckLifeLeft())
                {
                    _isDead = false;
                    return;
                }

                _isDead = value;

            }
        } 

        #endregion

        #region -- MONO APIMethods --
        protected virtual void FixedUpdate()
        {
            if (!CheckIsDead())
                return;

            isDead = true;
        }


        #endregion

        #region -- Private APIMthods --
        private bool CheckIsDead()
        {
            return this.targetHealth.curHealthAmount < 0 || targetHealth.curHealthAmount == 0;
        }

        private bool CheckLifeLeft()
        {
            return this.targetHealth.curLifeAmount > 0;
        }

        private void InitHealthData()
        {
            this.targetHealth.InitHealthData();
        }

        #endregion
        
        #region -- Interface APIMethods --
        public abstract void HeathDecreased(float damageAmount);
        public abstract void HeathIncreased(float increaseAmount);
        public abstract void HeathRecovered();
        public abstract void ArmorIncreased(float increaseAmount);
        public virtual void LifeDecreased()
        {
            this.targetHealth.curLifeAmount -= 1;
        }
        public virtual void LifeIncreased(int time)
        {
            this.targetHealth.curLifeAmount += time;
        }

        #endregion
    }
}