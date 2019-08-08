using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现一个不可编辑的面板被public数据
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReadOnlyAttribute : ADrawerAttribute
    {
    }
}
