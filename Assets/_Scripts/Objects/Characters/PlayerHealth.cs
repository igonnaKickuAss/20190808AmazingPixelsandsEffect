using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxi.OObjects
{
    using OLiOYouxiAttributes;
    using OLiOYouxi.OSystem.Helpers;

    public class PlayerHealth : ACharacterHealth
	{
        #region -- Toggle Data --
        public bool displayEditor = false;
        #endregion

        #region -- Editor Data --
        [BoxGroup("模拟操作：")] [ShowIf("displayEditor")] [Slider(0, 100f)] public float damageValue;

        #endregion

        #region -- Private Data --
        private HealthMessage health;

        #endregion

        #region -- Structs --
        struct HealthMessage
        {
            public int lifeTime;
            public float healthValue;
            public float armorValue;
        }

        #endregion

        #region -- 初始化 --
        private void InitData()
        {
            if (!targetHealth)
            {
                //初始化失败
                //关闭这个对象
                transform.TrySetActive(false);  
                return;
            }

            targetHealth.InitData();

            this.health = new HealthMessage()
            {
                lifeTime = targetHealth.curLifeAmount,
                healthValue = targetHealth.curHealthAmount,
                armorValue = targetHealth.curArmorAmount
            };


        }

        #endregion

        #region -- MONO APIMethods --
        private void Start()
        {
            //去初始化
            InitData();

        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!isUpdateData)
                return;

            //更新数据
            targetHealth.UpdateData(ref this.health.lifeTime, ref this.health.healthValue, ref this.health.armorValue);
            isUpdateData = false;
            print("这是鸡肋自动更新 " + "生命值：" + this.health.healthValue + "，" + "护甲值：" + this.health.armorValue + "，" + "生命次数：" + this.targetHealth.curLifeAmount);

        }

        #endregion

        #region -- Editor APIMethods --

#if UNITY_EDITOR
        [Button("模拟受伤")]
        private void Damage()
        {
            if (!displayEditor)
                return;
            HeathDecreased(damageValue);

            print("生命值：" + this.health.healthValue + "，" + "护甲值：" + this.health.armorValue + "，" + "生命次数：" + this.targetHealth.curLifeAmount);
        }
#endif

        #endregion

        #region -- Override APIMethods --
        /// <summary>
        /// 受到伤害handle damageAmount
        /// </summary>
        /// <param name="damageAmount"></param>
        public override void HeathDecreased(float damageAmount)
        {
            targetHealth.HealthArmorDamageOperation(damageAmount);

            //更新数据
            targetHealth.UpdateData(ref this.health.lifeTime, ref this.health.healthValue, ref this.health.armorValue);
        }

        /// <summary>
        /// 恢复生命值increaseAmount
        /// </summary>
        public override void HeathIncreased(float increaseAmount)
        {
            targetHealth.HealthFitOperation(increaseAmount);

            //更新数据
            targetHealth.UpdateData(ref this.health.lifeTime, ref this.health.healthValue, ref this.health.armorValue);
        }

        /// <summary>
        /// 回春
        /// </summary>
        public override void HeathRecovered()
        {
            targetHealth.InitData();
        }
        
        /// <summary>
        /// 恢复护甲值increaseAmount
        /// </summary>
        /// <param name="increaseAmount"></param>
        public override void ArmorIncreased(float increaseAmount)
        {
            targetHealth.ArmorFitOperation(increaseAmount);

            //更新数据
            targetHealth.UpdateData(ref this.health.lifeTime, ref this.health.healthValue, ref this.health.armorValue);
        }
        
        /// <summary>
        /// 当前生命总数减少一
        /// </summary>
        public override void LifeDecreased()
        {
            base.LifeDecreased();
        }

        /// <summary>
        /// 当前生命总数增加time
        /// </summary>
        /// <param name="time"></param>
        public override void LifeIncreased(int time)
        {
            base.LifeIncreased(time);
        }

        #endregion

    }
}