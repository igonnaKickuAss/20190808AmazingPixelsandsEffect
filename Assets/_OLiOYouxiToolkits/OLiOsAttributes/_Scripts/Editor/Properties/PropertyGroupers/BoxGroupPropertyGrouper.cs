using UnityEditor;
using UnityEngine;

namespace OLiOYouxiAttributes.Editor
{
    /// <summary>
    /// 封装了，内置的EditorGUILayout..BeginVertical & EndVertical 布局绘制方式（单纯画画罢了）
    /// </summary>
    [PropertyGrouper(typeof(BoxGroupAttribute))]
    public class BoxGroupPropertyGrouper : APropertyGrouper
    {
        public override void BeginGroup(string label)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            if (!string.IsNullOrEmpty(label))
            {
                EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            }
        }

        public override void EndGroup()
        {
            EditorGUILayout.EndVertical();
        }
    }
}
