using UnityEditor;
using System.Collections.Generic;

namespace OLiOYouxiAttributes.Editor
{
    [PropertyDrawer(typeof(TagAttribute))]
    public class TagPropertyDrawer : APropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                // 生成标签列表和自定义标签
                List<string> tagList = new List<string>();
                tagList.Add("(None)");
                tagList.Add("Untagged");
                tagList.AddRange(UnityEditorInternal.InternalEditorUtility.tags);

                string propertyString = property.stringValue;
                int index = 0;
                // 检查是否有与该条目匹配的条目，并获取索引
                // 我们跳过0的索引
                for (int i = 1; i < tagList.Count; i++)
                {
                    if (tagList[i] == propertyString)
                    {
                        index = i;
                        break;
                    }
                }

                // 用当前选定的索引绘制弹出框
                index = EditorGUILayout.Popup(property.displayName, index, tagList.ToArray());

                // 根据所选内容调整属性的实际字符串值
                if (index > 0)
                {
                    property.stringValue = tagList[index];
                }
                else
                {
                    property.stringValue = string.Empty;
                }
            }
            else
            {
                EditorGUILayout.HelpBox(property.type + " 不被标签特性支持！！\n" +
                "你得用string对象", MessageType.Warning);
            }
        }
    }
}