using System;
using System.Collections;
using System.Collections.Generic;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现一个下拉栏
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DropdownAttribute : ADrawerAttribute
    {
        public string ValuesFieldName { get; private set; }

        /// <summary>
        /// 实现一个下拉栏
        /// </summary>
        /// <param name="valuesFieldName">一个数组或者一个集合，但是！其元素类型要与所指定的字段类型相同</param>
        public DropdownAttribute(string valuesFieldName)
        {
            this.ValuesFieldName = valuesFieldName;
        }
    }

    public interface IDropdownList : IEnumerable<KeyValuePair<string, object>>
    {
    }

    /// <summary>
    /// 这是专门为下拉栏服务的list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DropdownList<T> : IDropdownList
    {
        private List<KeyValuePair<string, object>> values;

        public DropdownList()
        {
            this.values = new List<KeyValuePair<string, object>>();
        }

        /// <summary>
        /// 类似json语言的赶脚，键值对啦
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="value"></param>
        public void Add(string displayName, T value)
        {
            this.values.Add(new KeyValuePair<string, object>(displayName, value));
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.values.GetEnumerator(); 
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        //参考： https://blog.csdn.net/PalmAdorableTiger/article/details/80707456
        static public explicit operator DropdownList<object>(DropdownList<T> target)
        {
            DropdownList<object> result = new DropdownList<object>();
            foreach (var kvp in target)
            {
                result.Add(kvp.Key, kvp.Value);
            }

            return result;
        }
    }
}
