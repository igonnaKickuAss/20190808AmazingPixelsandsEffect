using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxi.OObjects
{
    [CreateAssetMenu(menuName = "OLiOYouxi/Character/Health")]
    public class ObjectHealth : AObjectHealth
    {
        #region -- Private Data --
        private bool isInit = false;
        [HideInInspector] public int curLifeAmount;
        [HideInInspector] public float curArmorAmount;
        [HideInInspector] public float curHealthAmount;


        #endregion

        #region -- 初始化 --

        /// <summary>
        /// 用于初始化或者回春
        /// </summary>
        public override void InitData()
        {
            this.curArmorAmount = armorValue;
            this.curHealthAmount = healthValue;
            this.curLifeAmount = lifeTime;

            this.isInit = true;
        }

        /// <summary>
        /// 用于生命值护甲值恢复成默认
        /// </summary>
        public override void InitHealthData()
        {
            this.curArmorAmount = armorValue;
            this.curHealthAmount = healthValue;
        }


        /// <summary>
        /// 更新生命属性值
        /// </summary>
        /// <param name="curHealthAmount"></param>
        /// <param name="curArmorAmount"></param>
        public override void UpdateData(ref int curLifeAmount, ref float curHealthAmount, ref float curArmorAmount)
        {
            curLifeAmount = this.curLifeAmount;
            curHealthAmount = this.curHealthAmount;
            curArmorAmount = this.curArmorAmount;
        }


        #endregion

        #region -- Override APIMethods --
        public override void HealthArmorDamageOperation(float damageAmount)
        {
            /*伤害量分三段阈值
             * 第一段： 0 < damageAmount < 10 => 护甲防御量 defenseAmountStage1
             * 第二段： 10 < damageAmount < 60 => 护甲防御量 defenseAmountStage2
             * 第三段： 60 < damageAmount < 9999 => 护甲防御量 defenseAmountStage3
             * 第四段： armorValue = 0（无护甲状态） => 护甲防御量 defenseAmountStage4
             */

            //TODO..
            if (!isInit)
                return;   //未初始化，不操作

            if (damageAmount > 9999f || damageAmount < 0)
                return;       //爆表了，不操作

            if (!damageTrigger)
                return;       //不受伤，不操作

            if (curArmorAmount > 0)
            {
                //有护甲状态
                if (damageAmount > 0 && damageAmount < 10f)
                {
                    //效率0.8
                    curArmorAmount -= damageAmount * defenseAmountStage1;
                    curHealthAmount -= damageAmount *= (1 - defenseAmountStage1);
                }
                else if (damageAmount < 60f && damageAmount > 10f)
                {
                    //效率0.2
                    curArmorAmount -= damageAmount * defenseAmountStage2;
                    curHealthAmount -= damageAmount *= (1 - defenseAmountStage2);
                }
                else if (damageAmount < 9999f && damageAmount > 60f)
                {
                    //效率0.1
                    curArmorAmount -= damageAmount * defenseAmountStage3;
                    curHealthAmount -= damageAmount *= (1 - defenseAmountStage3);
                }
                return;  //操作结束
            }

            //无护甲状态
            curHealthAmount -= damageAmount *= (1 - defenseAmountStage4);

            //操作结束..
        }

        public override void HealthFitOperation(float increaseAmount)
        {
            float h = this.curHealthAmount + increaseAmount;
            this.curHealthAmount = h > healthValue ? healthValue : h;
        }

        public override void ArmorFitOperation(float increaseAmount)
        {
            float a = this.curArmorAmount + increaseAmount;
            this.curArmorAmount = a > armorValue ? armorValue : a;
        }

        #endregion
    }
}