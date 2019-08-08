using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 关于元数据的抽象类
    /// </summary>
	public abstract class AMetaAttribute : OLiOYouxiAttribute
	{
        public int Order { get; set; }
	}
}