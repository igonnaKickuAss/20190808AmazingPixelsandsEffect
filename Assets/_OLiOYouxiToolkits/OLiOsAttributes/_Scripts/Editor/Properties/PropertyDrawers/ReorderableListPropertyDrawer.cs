using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

namespace OLiOYouxiAttributes.Editor
{
    [PropertyDrawer(typeof(ReorderableListAttribute))]
    public class ReorderableListPropertyDrawer : APropertyDrawer
    {
        private Dictionary<string, ReorderableList> reorderableListsByPropertyName = new Dictionary<string, ReorderableList>();

        string GetPropertyKeyName(SerializedProperty property)
        {
            return property.serializedObject.targetObject.GetInstanceID() + "/" + property.name;
        }

        public override void DrawProperty(SerializedProperty property)
        {
            EditorDrawUtility.DrawHeader(property);

            if (property.isArray)
            {
                var key = GetPropertyKeyName(property);

                if (!this.reorderableListsByPropertyName.ContainsKey(key))
                {
                    ReorderableList reorderableList = new ReorderableList(property.serializedObject, property, true, true, true, true)
                    {
                        drawHeaderCallback = (Rect rect) =>
                        {
                            EditorGUI.LabelField(rect, string.Format("{0}: {1}", property.displayName, property.arraySize), EditorStyles.label);
                        },

                        drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                        {
                            var element = property.GetArrayElementAtIndex(index);
                            rect.y += 1.0f;
                            rect.x += 10.0f;
                            rect.width -= 10.0f;

                            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, 0.0f), element, true);
                        },

                        elementHeightCallback = (int index) =>
                        {
                            return EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index)) + 4.0f;
                        }
                    };

                    this.reorderableListsByPropertyName[key] = reorderableList;
                }

                this.reorderableListsByPropertyName[key].DoLayoutList();
            }
            else
            {
                string warning = typeof(ReorderableListAttribute).Name + " 只作用于数组或者集合字段！猪头";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: PropertyUtility.GetTargetObject(property),logToConsole: false);

                EditorDrawUtility.DrawPropertyField(property);
            }
        }

        public override void ClearCache()
        {
            this.reorderableListsByPropertyName.Clear();
        }
    }
}
