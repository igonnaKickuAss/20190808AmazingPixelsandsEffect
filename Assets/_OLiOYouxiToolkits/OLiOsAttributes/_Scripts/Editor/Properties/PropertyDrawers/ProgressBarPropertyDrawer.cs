using UnityEngine;
using UnityEditor;
using System;

namespace OLiOYouxiAttributes.Editor
{
    [PropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarPropertyDrawer : APropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            EditorDrawUtility.DrawHeader(property);
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);

            if (property.propertyType != SerializedPropertyType.Float && property.propertyType != SerializedPropertyType.Integer)
            {
                EditorDrawUtility.DrawHelpBox("字段 " + property.name + " 不是一个数字啊大哥", MessageType.Warning, context: target, logToConsole: false);
                return;
            }

            var value = property.propertyType == SerializedPropertyType.Integer ? property.intValue : property.floatValue;
            var valueFormatted = property.propertyType == SerializedPropertyType.Integer ? value.ToString() : String.Format("{0:0.00}", value);

            ProgressBarAttribute progressBarAttribute = PropertyUtility.GetAttribute<ProgressBarAttribute>(property);
            var position = EditorGUILayout.GetControlRect();
            var maxValue = progressBarAttribute.MaxValue;
            float lineHight = EditorGUIUtility.singleLineHeight;
            float padding = EditorGUIUtility.standardVerticalSpacing;
            var barPosition = new Rect(position.position.x, position.position.y, position.size.x, lineHight);

            var fillPercentage = value / maxValue;
            var barLabel = (!string.IsNullOrEmpty(progressBarAttribute.Name) ? "[" + progressBarAttribute.Name + "] " : "") + valueFormatted + "/" + maxValue;

            var color = GetColor(progressBarAttribute.Color);
            var color2 = Color.white;
            DrawBar(barPosition, Mathf.Clamp01(fillPercentage), barLabel, color, color2);
        }

        private void DrawBar(Rect position, float fillPercent, string label, Color barColor, Color labelColor)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            Color savedColor = GUI.color;

            var fillRect = new Rect(position.x, position.y, position.width * fillPercent, position.height);

            EditorGUI.DrawRect(position, new Color(0.13f, 0.13f, 0.13f));
            EditorGUI.DrawRect(fillRect, barColor);

            // 设置直线条和缓存他
            var align = GUI.skin.label.alignment;
            GUI.skin.label.alignment = TextAnchor.UpperCenter;

            // 设置颜色和缓存他
            var c = GUI.contentColor;
            GUI.contentColor = labelColor;

            // 计算位置
            var labelRect = new Rect(position.x, position.y - 2, position.width, position.height);

            // 画他
            EditorGUI.DropShadowLabel(labelRect, label);

            // 重置颜色和直线条
            GUI.contentColor = c;
            GUI.skin.label.alignment = align;
        }

        private Color GetColor(ProgressBarColor color)
        {
            switch (color)
            {
                case ProgressBarColor.Red:
                    return new Color32(255, 0, 63, 255);
                case ProgressBarColor.Pink:
                    return new Color32(255, 152, 203, 255);
                case ProgressBarColor.Orange:
                    return new Color32(255, 128, 0, 255);
                case ProgressBarColor.Yellow:
                    return new Color32(255, 211, 0, 255);
                case ProgressBarColor.Green:
                    return new Color32(102, 255, 0, 255);
                case ProgressBarColor.Blue:
                    return new Color32(0, 135, 189, 255);
                case ProgressBarColor.Indigo:
                    return new Color32(75, 0, 130, 255);
                case ProgressBarColor.Violet:
                    return new Color32(127, 0, 255, 255);
                default:
                    return Color.white;
            }
        }
    }
}
