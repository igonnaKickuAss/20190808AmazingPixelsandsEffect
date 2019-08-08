using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现一个满足给定的限制条件才显示的字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ValidateInputAttribute : AValidatorAttribute
    {
        public string CallbackName { get; private set; }
        public string Message { get; private set; }

        /// <summary>
        /// 实现一个满足给定的限制条件才显示的字段
        /// </summary>
        /// <param name="callbackName">回调函数签名名称（布尔返回值且与指定字段类型一致的单参）</param>
        /// <param name="message">额外显示讯息</param>
        public ValidateInputAttribute(string callbackName, string message = null)
        {
            this.CallbackName = callbackName;
            this.Message = message;
        }
    }
}
