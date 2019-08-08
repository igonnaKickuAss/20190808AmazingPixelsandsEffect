using UnityEngine;
using UnityEditor;

namespace OLiOYouxiAttributes.Editor
{
    [PropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderPropertyDrawer : APropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            EditorDrawUtility.DrawHeader(property);

            MinMaxSliderAttribute minMaxSliderAttribute = PropertyUtility.GetAttribute<MinMaxSliderAttribute>(property);

            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                Rect controlRect = EditorGUILayout.GetControlRect();
                float labelWidth = EditorGUIUtility.labelWidth;
                float floatFieldWidth = EditorGUIUtility.fieldWidth;
                float sliderWidth = controlRect.width - labelWidth - 2f * floatFieldWidth;
                float sliderPadding = 5f;

                Rect labelRect = new Rect(
                    controlRect.x,
                    controlRect.y,
                    labelWidth,
                    controlRect.height);

                Rect sliderRect = new Rect(
                    controlRect.x + labelWidth + floatFieldWidth + sliderPadding,
                    controlRect.y,
                    sliderWidth - 2f * sliderPadding,
                    controlRect.height);

                Rect minFloatFieldRect = new Rect(
                    controlRect.x + labelWidth,
                    controlRect.y,
                    floatFieldWidth,
                    controlRect.height);

                Rect maxFloatFieldRect = new Rect(
                    controlRect.x + labelWidth + floatFieldWidth + sliderWidth,
                    controlRect.y,
                    floatFieldWidth,
                    controlRect.height);

                // 绘制标签
                EditorGUI.LabelField(labelRect, property.displayName);

                // 绘制slider
                EditorGUI.BeginChangeCheck();

                Vector2 sliderValue = property.vector2Value;
                EditorGUI.MinMaxSlider(sliderRect, ref sliderValue.x, ref sliderValue.y, minMaxSliderAttribute.MinValue, minMaxSliderAttribute.MaxValue);

                sliderValue.x = EditorGUI.FloatField(minFloatFieldRect, sliderValue.x);
                sliderValue.x = Mathf.Clamp(sliderValue.x, minMaxSliderAttribute.MinValue, Mathf.Min(minMaxSliderAttribute.MaxValue, sliderValue.y));

                sliderValue.y = EditorGUI.FloatField(maxFloatFieldRect, sliderValue.y);
                sliderValue.y = Mathf.Clamp(sliderValue.y, Mathf.Max(minMaxSliderAttribute.MinValue, sliderValue.x), minMaxSliderAttribute.MaxValue);

                if (EditorGUI.EndChangeCheck())
                {
                    property.vector2Value = sliderValue;
                }
            }
            else
            {
                string warning = minMaxSliderAttribute.GetType().Name + " 只用于vector2类型，笨蛋！";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: PropertyUtility.GetTargetObject(property));

                EditorDrawUtility.DrawPropertyField(property);
            }
        }
    }
}
