using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现一个暴露以下拉栏的形式有选择的将GameObject.Tag赋值给字段，应该算方便了。。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class TagAttribute : ADrawerAttribute
    {
    }
}
