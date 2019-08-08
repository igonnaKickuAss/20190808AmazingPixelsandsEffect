using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现一个富文本，类似winform里边的那个richText
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ResizableTextAreaAttribute : ADrawerAttribute
    {
    }
}
