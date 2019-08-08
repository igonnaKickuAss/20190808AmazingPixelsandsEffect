using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现一个最大值限定
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MaxValueAttribute : AValidatorAttribute
    {
        public float MaxValue { get; private set; }

        /// <summary>
        /// 实现一个最大值限定
        /// </summary>
        /// <param name="maxValue">最大值float</param>
        public MaxValueAttribute(float maxValue)
        {
            this.MaxValue = maxValue;
        }

        /// <summary>
        /// 实现一个最大值限定
        /// </summary>
        /// <param name="maxValue">最大值int</param>
        public MaxValueAttribute(int maxValue)
        {
            this.MaxValue = maxValue;
        }
    }
}
