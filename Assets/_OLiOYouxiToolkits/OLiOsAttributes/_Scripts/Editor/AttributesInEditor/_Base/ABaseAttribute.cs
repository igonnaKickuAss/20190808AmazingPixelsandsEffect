using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxiAttributes.Editor
{
    /// <summary>
    /// 关于类的抽象类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class ABaseAttribute : Attribute, IAttribute
    {
        public ABaseAttribute(Type targetAttributeType)
        {
            this.TargetAttributeType = targetAttributeType;
        }

        public Type TargetAttributeType
        {
            get;
            private set;
        }
    }
}