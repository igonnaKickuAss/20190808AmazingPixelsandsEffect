using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 当满足指定条件时取消只读某字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EnableIfAttribute : ADrawerAttribute
    {
        public string[] Conditions { get; private set; }
        public ConditionOperator ConditionOperator { get; private set; }
        public bool Reversed { get; protected set; }

        /// <summary>
        /// 当满足指定条件时取消只读某字段
        /// </summary>
        /// <param name="condition">条件(同disableif特性)</param>
        public EnableIfAttribute(string condition)
        {
            ConditionOperator = ConditionOperator.And;
            Conditions = new string[1] { condition };
        }

        /// <summary>
        /// 当满足指定条件时取消只读某字段
        /// </summary>
        /// <param name="conditionOperator">交并关系</param>
        /// <param name="conditions">条件(同disableif特性)</param>
        public EnableIfAttribute(ConditionOperator conditionOperator, params string[] conditions)
        {
            ConditionOperator = conditionOperator;
            Conditions = conditions;
        }
    }
}
