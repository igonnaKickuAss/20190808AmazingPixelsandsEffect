using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 面板中增加一个方法按钮
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonAttribute : ADrawerAttribute
    {
        public string Text { get; private set; }

        /// <summary>
        /// 面板中增加一个方法按钮，你可以给他取一个名字，否则他的标签就用方法的签名，作用于没有参数的方法
        /// </summary>
        /// <param name="text">你的名字。。以及你的证件号码。。</param>
        public ButtonAttribute(string text = null)
        {
            this.Text = text;
        }
    }
}
