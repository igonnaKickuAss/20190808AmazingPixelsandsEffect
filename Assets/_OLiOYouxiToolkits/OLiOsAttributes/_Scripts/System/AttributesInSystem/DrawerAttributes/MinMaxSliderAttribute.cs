using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现一个带最大最小值范围的slider
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MinMaxSliderAttribute : ADrawerAttribute
    {
        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }

        /// <summary>
        /// 实现一个带最大最小值范围的slider，作用于Vector2
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        public MinMaxSliderAttribute(float minValue, float maxValue)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }
    }
}
