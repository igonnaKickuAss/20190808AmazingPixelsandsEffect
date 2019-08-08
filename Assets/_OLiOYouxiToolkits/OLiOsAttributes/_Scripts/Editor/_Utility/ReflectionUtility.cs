using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OLiOYouxiAttributes.Editor
{
	static public class ReflectionUtility
	{
        #region -- Public APIMethods --
        /// <summary>
        /// 返回所有“字段”讯息，利用反射原理，使用linq操作(我把他叫做惰性遍历)，以给定的条件来返回所需要的 fieldInfos
        /// </summary>
        /// <param name="target"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        static public IEnumerable<FieldInfo> GetAllFields(object target, Func<FieldInfo, bool> predicate)
        {
            List<Type> types = new List<Type>() { target.GetType() };

            while (types.Last().BaseType != null)
            {
                types.Add(types.Last().BaseType);
            }

            for (int i = types.Count - 1; i >= 0 ; i--)
            {
                IEnumerable<FieldInfo> fieldInfos = types[i].GetFields(
                    BindingFlags.Instance |
                    BindingFlags.Static |
                    BindingFlags.NonPublic |
                    BindingFlags.Public |
                    BindingFlags.DeclaredOnly
                    ).Where(predicate);

                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    yield return fieldInfo;
                }
            }
        }

        /// <summary>
        /// 返回所有“属性”讯息，利用反射原理，使用linq操作(我把他叫做惰性遍历)，以给定的条件来返回所需要的 propertyInfos
        /// </summary>
        /// <param name="target"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        static public IEnumerable<PropertyInfo> GetAllProperties(object target, Func<PropertyInfo, bool> predicate)
        {
            List<Type> types = new List<Type>() { target.GetType() };

            while (types.Last().BaseType != null)
            {
                types.Add(types.Last().BaseType);
            }

            for (int i = types.Count - 1; i >= 0; i--)
            {
                IEnumerable<PropertyInfo> propertyInfos = types[i].GetProperties(
                    BindingFlags.Instance |
                    BindingFlags.Static |
                    BindingFlags.NonPublic |
                    BindingFlags.Public |
                    BindingFlags.DeclaredOnly
                    ).Where(predicate);

                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    yield return propertyInfo;
                }
            }
        }

        /// <summary>
        /// 返回所有“函数（方法）”讯息，利用反射原理，使用linq操作（这是直接返回序列），以给定的条件来返回所需要的 methodInfos
        /// </summary>
        /// <param name="target"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        static public IEnumerable<MethodInfo> GetAllMethods(object target, Func<MethodInfo, bool> predicate)
        {
            IEnumerable<MethodInfo> methodInfos = target.GetType().GetMethods(
                 BindingFlags.Instance |
                 BindingFlags.Static |
                 BindingFlags.NonPublic |
                 BindingFlags.Public
                ).Where(predicate);

            return methodInfos;
        }

        /// <summary>
        /// 返回指定的“字段”讯息， 传递 约束条件给方法 GetAllFields
        /// </summary>
        /// <param name="target"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        static public FieldInfo GetField(object target, string fieldName)
        {
            return GetAllFields(target, fi => fi.Name.Equals(fieldName, StringComparison.InvariantCulture)).FirstOrDefault();
        }

        /// <summary>
        /// 返回指定的“属性”讯息， 传递 约束条件给方法 GetAllProperties
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        static public PropertyInfo GetProperty(object target, string propertyName)
        {
            return GetAllProperties(target, pi => pi.Name.Equals(propertyName, StringComparison.InvariantCulture)).FirstOrDefault();
        }

        /// <summary>
        /// 返回指定的“方法”讯息， 传递 约束条件给方法 GetAllMethods
        /// </summary>
        /// <param name="target"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        static public MethodInfo GetMethod(object target, string methodName)
        {
            return GetAllMethods(target, mi => mi.Name.Equals(methodName, StringComparison.InvariantCulture)).FirstOrDefault();
        }

        #endregion

    }
}