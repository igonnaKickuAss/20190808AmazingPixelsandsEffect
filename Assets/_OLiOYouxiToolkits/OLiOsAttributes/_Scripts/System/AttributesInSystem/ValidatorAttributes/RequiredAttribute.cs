using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现一个提示，类似内置的类特性(RequireComponent)，提示你某个字段是不可少的
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class RequiredAttribute : AValidatorAttribute
    {
        public string Message { get; private set; }

        /// <summary>
        /// 实现一个提示，类似内置的类特性，提示你某个字段是不可少的
        /// </summary>
        /// <param name="message">提示说明文</param>
        public RequiredAttribute(string message = null)
        {
            this.Message = message;
        }
    }
}