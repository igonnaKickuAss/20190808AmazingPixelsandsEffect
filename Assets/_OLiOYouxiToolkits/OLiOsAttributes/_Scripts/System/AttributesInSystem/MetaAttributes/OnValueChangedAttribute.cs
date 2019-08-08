using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现一个字段值被更改时会执行一个回调函数
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class OnValueChangedAttribute : AMetaAttribute
	{
        public string CallbackName { get; private set; }

        /// <summary>
        /// 实现一个字段值被更改时会执行一个回调函数，具体用在哪里？自由发挥
        /// </summary>
        /// <param name="callbackName">函数签名名称，无参无返回值</param>
        public OnValueChangedAttribute(string callbackName)
        {
            this.CallbackName = callbackName;
        }

    }
}