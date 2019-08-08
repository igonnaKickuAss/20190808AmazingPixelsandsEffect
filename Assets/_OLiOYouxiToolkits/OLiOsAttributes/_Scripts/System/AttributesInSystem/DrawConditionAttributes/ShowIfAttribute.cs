using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 当满足指定条件时显示某字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowIfAttribute : ADrawConditionAttribute
    {
        public string[] Conditions { get; private set; }
        public ConditionOperator ConditionOperator { get; private set; }
        public bool Reversed { get; protected set; }

        /// <summary>
        /// 当满足指定条件时显示某字段
        /// </summary>
        /// <param name="condition">条件（可以是有效的布尔值字段，也可以是返回布尔值的方法）</param>
        public ShowIfAttribute(string condition)
        {
            ConditionOperator = ConditionOperator.And;
            Conditions = new string[1] { condition };
        }

        /// <summary>
        /// 当满足指定条件时显示某字段
        /// </summary>
        /// <param name="conditionOperator">交并关系</param>
        /// <param name="conditions">条件（可以是有效的布尔值字段，也可以是返回布尔值的方法）</param>
        public ShowIfAttribute(ConditionOperator conditionOperator, params string[] conditions)
        {
            ConditionOperator = conditionOperator;
            Conditions = conditions;
        }
    }
}
