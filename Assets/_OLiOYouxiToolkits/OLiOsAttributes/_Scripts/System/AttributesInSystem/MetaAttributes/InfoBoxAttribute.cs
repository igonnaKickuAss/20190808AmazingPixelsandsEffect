using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现一个消息盒子
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class InfoBoxAttribute : AMetaAttribute
    {
        public string Text { get; private set; }
        public InfoBoxType Type { get; private set; }
        public string VisibleIf { get; private set; }

        /// <summary>
        /// 实现一个消息盒子
        /// </summary>
        /// <param name="text">消息盒子的讯息</param>
        /// <param name="type">这个消息长什么牙膏？</param>
        /// <param name="visibleIf">什么条件显示？（一个布尔值字段或者一个返回布尔值的函数）</param>
        public InfoBoxAttribute(string text, InfoBoxType type = InfoBoxType.Normal, string visibleIf = null)
        {
            this.Text = text;
            this.Type = type;
            this.VisibleIf = visibleIf;
        }

        /// <summary>
        /// 实现一个消息盒子
        /// </summary>
        /// <param name="text">消息盒子的讯息</param>
        /// <param name="visibleIf">什么条件显示？（一个布尔值字段或者一个返回布尔值的函数）</param>
        public InfoBoxAttribute(string text, string visibleIf)
            : this(text, InfoBoxType.Normal, visibleIf)
        {
        }
    }

    public enum InfoBoxType
    {
        Normal,
        Warning,
        Error
    }
}
