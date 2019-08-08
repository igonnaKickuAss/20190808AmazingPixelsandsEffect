using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 当满足指定条件时只读某字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DisableIfAttribute : EnableIfAttribute
    {
        /// <summary>
        /// 当满足指定条件时只读某字段
        /// </summary>
        /// <param name="condition">条件（可以是有效的布尔值字段，也可以是返回布尔值的方法）</param>
        public DisableIfAttribute(string condition)
            : base(condition)
        {
            Reversed = true;
        }

        /// <summary>
        /// 当满足指定条件时只读某字段
        /// </summary>
        /// <param name="conditionOperator">交并关系</param>
        /// <param name="conditions">条件（可以是有效的布尔值字段，也可以是返回布尔值的方法）</param>
        public DisableIfAttribute(ConditionOperator conditionOperator, params string[] conditions)
            : base(conditionOperator, conditions)
        {
            Reversed = true;
        }
    }
}
