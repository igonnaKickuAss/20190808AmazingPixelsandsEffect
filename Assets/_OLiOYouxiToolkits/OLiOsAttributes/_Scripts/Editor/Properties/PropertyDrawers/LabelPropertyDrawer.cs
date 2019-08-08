using UnityEngine;
using UnityEditor;

namespace OLiOYouxiAttributes.Editor
{
    [PropertyDrawer(typeof(LabelAttribute))]
    public class LabelPropertyDrawer : APropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            var labelAttribute = PropertyUtility.GetAttribute<LabelAttribute>(property);
            var guiContent = new GUIContent(labelAttribute.Label);
            EditorGUILayout.PropertyField(property, guiContent, true);
        }
    }
}
