using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现一个面板可拖拉排序的数组或者集合
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReorderableListAttribute : ADrawerAttribute
    {
    }
}
