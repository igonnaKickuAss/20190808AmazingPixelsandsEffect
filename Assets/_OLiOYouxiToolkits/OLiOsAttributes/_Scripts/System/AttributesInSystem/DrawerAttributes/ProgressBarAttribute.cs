using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现一个进度条
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ProgressBarAttribute : ADrawerAttribute
    {
        public string Name { get; private set; }
        public float MaxValue { get; private set; }
        public ProgressBarColor Color { get; private set; }

        /// <summary>
        /// 实现一个进度条
        /// </summary>
        /// <param name="name">你的名字</param>
        /// <param name="maxValue">最大值，只是标签最大值噢</param>
        /// <param name="color">进度颜色</param>
        public ProgressBarAttribute(string name = "", float maxValue = 100, ProgressBarColor color = ProgressBarColor.Blue)
        {
            Name = name;
            MaxValue = maxValue;
            Color = color;
        }
    }

    public enum ProgressBarColor
    {
        Red,
        Pink,
        Orange,
        Yellow,
        Green,
        Blue,
        Indigo,
        Violet,
        White
    }
}