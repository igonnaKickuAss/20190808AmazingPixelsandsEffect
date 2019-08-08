using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现一个普通slider
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SliderAttribute : ADrawerAttribute
    {
        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }

        /// <summary>
        /// 实现一个普通sliderFloat
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        public SliderAttribute(float minValue, float maxValue)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }

        /// <summary>
        /// 实现一个普通sliderInt
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        public SliderAttribute(int minValue, int maxValue)
        {
            this.MaxValue = minValue;
            this.MaxValue = maxValue;
        }
    }
}
