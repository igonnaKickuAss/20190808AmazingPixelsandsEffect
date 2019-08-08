using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现一个新的字段名称标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class LabelAttribute : ADrawerAttribute
    {
        public string Label { get; private set; }

        /// <summary>
        /// 实现一个新的字段名称标签
        /// </summary>
        /// <param name="label">你的新名称标签</param>
        public LabelAttribute(string label)
        {
            this.Label = label;
        }
    }
}
