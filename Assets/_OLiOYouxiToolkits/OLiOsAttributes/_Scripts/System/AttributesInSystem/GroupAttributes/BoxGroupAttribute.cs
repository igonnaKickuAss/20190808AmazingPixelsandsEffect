using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现一个方形组，主要就是想实现这个功能嘿嘿
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class BoxGroupAttribute : AGroupAttribute
	{
        /// <summary>
        ///  实现一个方形组，主要就是想实现这个功能嘿嘿
        /// </summary>
        /// <param name="name">可填可不填</param>
        public BoxGroupAttribute(string name = "奥利奥是一个靓仔") : base(name)
        {
            
        }

	}
}