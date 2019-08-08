using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections;
using System;
using System.Collections.Generic;

namespace OLiOYouxiAttributes.Editor
{
    [PropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownPropertyDrawer : APropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            EditorDrawUtility.DrawHeader(property);

            DropdownAttribute dropdownAttribute = PropertyUtility.GetAttribute<DropdownAttribute>(property);
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);

            FieldInfo fieldInfo = ReflectionUtility.GetField(target, property.name);
            FieldInfo valuesFieldInfo = ReflectionUtility.GetField(target, dropdownAttribute.ValuesFieldName);

            if (valuesFieldInfo == null)
            {
                EditorDrawUtility.DrawHelpBox(string.Format("{0} 不能找到叫做 \"{1}\" 的值", dropdownAttribute.GetType().Name, dropdownAttribute.ValuesFieldName), MessageType.Warning, context: target, logToConsole: false);
                EditorDrawUtility.DrawPropertyField(property);
            }
            else if (valuesFieldInfo.GetValue(target) is IList &&
                     fieldInfo.FieldType == this.GetElementType(valuesFieldInfo))
            {
                // 所选的值
                object selectedValue = fieldInfo.GetValue(target);

                // 所有值和可选项
                IList valuesList = (IList)valuesFieldInfo.GetValue(target);
                object[] values = new object[valuesList.Count];
                string[] displayOptions = new string[valuesList.Count];

                for (int i = 0; i < values.Length; i++)
                {
                    object value = valuesList[i];
                    values[i] = value;
                    displayOptions[i] = value.ToString();
                }

                // 可选择值得索引
                int selectedValueIndex = Array.IndexOf(values, selectedValue);
                if (selectedValueIndex < 0)
                {
                    selectedValueIndex = 0;
                }

                // 绘制下拉栏
                this.DrawDropdown(target, fieldInfo, property.displayName, selectedValueIndex, values, displayOptions);
            }
            else if (valuesFieldInfo.GetValue(target) is IDropdownList)
            {
                // 当前值
                object selectedValue = fieldInfo.GetValue(target);

                // 当前值得索引，和所有值，显示出的选项
                IDropdownList dropdown = (IDropdownList)valuesFieldInfo.GetValue(target);
                IEnumerator<KeyValuePair<string, object>> dropdownEnumerator = dropdown.GetEnumerator();

                int index = -1;
                int selectedValueIndex = -1;
                List<object> values = new List<object>();
                List<string> displayOptions = new List<string>();

                while (dropdownEnumerator.MoveNext())
                {
                    index++;

                    KeyValuePair<string, object> current = dropdownEnumerator.Current;
                    if (current.Value.Equals(selectedValue))
                    {
                        selectedValueIndex = index;
                    }

                    values.Add(current.Value);
                    displayOptions.Add(current.Key);
                }

                if (selectedValueIndex < 0)
                {
                    selectedValueIndex = 0;
                }

                // 绘制下拉栏
                this.DrawDropdown(target, fieldInfo, property.displayName, selectedValueIndex, values.ToArray(), displayOptions.ToArray());
            }
            else
            {
                EditorDrawUtility.DrawHelpBox(typeof(DropdownAttribute).Name + " 只作用于指定字段与指定数组的元素类型相等时！！八嘎", MessageType.Warning, context: target, logToConsole: false);
                EditorDrawUtility.DrawPropertyField(property);
            }
        }

        private Type GetElementType(FieldInfo listFieldInfo)
        {
            if (listFieldInfo.FieldType.IsGenericType)
            {
                return listFieldInfo.FieldType.GetGenericArguments()[0];
            }
            else
            {
                return listFieldInfo.FieldType.GetElementType();
            }
        }

        private void DrawDropdown(UnityEngine.Object target, FieldInfo fieldInfo, string label, int selectedValueIndex, object[] values, string[] displayOptions)
        {
            EditorGUI.BeginChangeCheck();

            int newIndex = EditorGUILayout.Popup(label, selectedValueIndex, displayOptions);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Dropdown");
                fieldInfo.SetValue(target, values[newIndex]);
            }
        }
    }
}
