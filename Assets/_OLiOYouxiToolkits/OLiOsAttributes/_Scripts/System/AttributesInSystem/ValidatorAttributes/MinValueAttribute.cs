using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现一个最小值限定
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MinValueAttribute : AValidatorAttribute
    {
        public float MinValue { get; private set; }

        /// <summary>
        /// 实现一个最小值限定
        /// </summary>
        /// <param name="minValue">最小值float</param>
        public MinValueAttribute(float minValue)
        {
            this.MinValue = minValue;
        }

        /// <summary>
        /// 实现一个最小值限定
        /// </summary>
        /// <param name="minValue">最小值int</param>
        public MinValueAttribute(int minValue)
        {
            this.MinValue = minValue;
        }
    }
}
