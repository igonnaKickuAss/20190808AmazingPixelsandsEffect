using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace OLiOYouxiAttributes.Editor
{
	static public class PropertyUtility
	{
        #region -- Public APIMethods --
        /// <summary>
        /// 返回这个序列化属性的第一个特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        static public T GetAttribute<T>(SerializedProperty property) where T : Attribute
        {
            T[] attributes = GetAttributes<T>(property);
            return attributes.Length > 0 ? attributes[0] : null;
        }

        /// <summary>
        /// 返回这个序列化属性的所有特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        static public T[] GetAttributes<T>(SerializedProperty property) where T : Attribute
        {
            FieldInfo fieldInfo = ReflectionUtility.GetField(GetTargetObject(property), property.name);
            return (T[])fieldInfo.GetCustomAttributes(typeof(T), true);
        }

        /// <summary>
        /// 返回这个序列化属性的对象（property.serializedObject.targetObject）
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        static public UnityEngine.Object GetTargetObject(SerializedProperty property)
        {
            return property.serializedObject.targetObject;
        }

        #endregion
    }
}