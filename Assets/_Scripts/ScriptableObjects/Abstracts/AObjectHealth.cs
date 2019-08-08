using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxi.OObjects
{
    using OLiOYouxiAttributes;

    public abstract class AObjectHealth : ScriptableObject
	{
        [BoxGroup("生命数据：")] [Label("生命总数：")] [MinValue(1f)] [MaxValue(999)] [SerializeField] protected int lifeTime = 10;
        [BoxGroup("生命数据：")] [Label("生命值：")] [MinValue(0)] [MaxValue(10000f)] [SerializeField] protected float healthValue = 100f;
        [BoxGroup("生命数据：")] [Label("护甲值：")] [MinValue(0)] [MaxValue(100f)] [SerializeField] protected float armorValue = 20f;
        [BoxGroup("生命数据：")] [Label("是否受伤：")] [SerializeField] protected bool damageTrigger = true;

        [BoxGroup("属性数据：")] [Label("防御量(阶段一)：")] [MinValue(-1)] [MaxValue(1)] [SerializeField] protected float defenseAmountStage1 = .8f;
        [BoxGroup("属性数据：")] [Label("防御量(阶段二)：")] [MinValue(-1)] [MaxValue(1)] [SerializeField] protected float defenseAmountStage2 = .2f;
        [BoxGroup("属性数据：")] [Label("防御量(阶段三)：")] [MinValue(-1)] [MaxValue(1)] [SerializeField] protected float defenseAmountStage3 = .1f;
        [BoxGroup("属性数据：")] [Label("防御量(阶段四)(无护甲状态)：")] [MinValue(-1)] [MaxValue(1)] [SerializeField] protected float defenseAmountStage4 = -.2f;

        /// <summary>
        /// 初始化数据
        /// </summary>
        public abstract void InitData();

        /// <summary>
        /// 生命值护甲默认值
        /// </summary>
        public abstract void InitHealthData();

        public abstract void UpdateData(ref int curLifeAmount, ref float curHealthAmount, ref float curArmorAmount);

        /// <summary>
        /// 实际生命值减少，会受到护甲的影响
        /// </summary>
        /// <param name="damageAmount">伤害量</param>
        public abstract void HealthArmorDamageOperation(float damageAmount);

        /// <summary>
        /// 生命值增加
        /// </summary>
        /// <param name="increaseAmount"></param>
        public abstract void HealthFitOperation(float increaseAmount);

        /// <summary>
        /// 护甲值增加
        /// </summary>
        /// <param name="increaseAmount"></param>
        public abstract void ArmorFitOperation(float increaseAmount);

    }
}