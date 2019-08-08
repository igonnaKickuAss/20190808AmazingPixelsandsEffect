using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现一个暴露的内置属性，unity3d的内置的公共属性是无法在面板中显示的，这个特性可以克服这个劣势！
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class ExposePropertyAttribute : ADrawerAttribute
	{

	}
}