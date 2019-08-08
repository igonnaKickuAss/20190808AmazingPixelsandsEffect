using System;
using UnityEngine;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现暴露一个你不想被public且是不能在面板编辑的字段，但是你又很想看到他的数值变化。你就是这么变态。用一个实现了get方法的属性去暴露（作用于属性）
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NativePropertyAttribute : ADrawerAttribute
    {

    }
}
