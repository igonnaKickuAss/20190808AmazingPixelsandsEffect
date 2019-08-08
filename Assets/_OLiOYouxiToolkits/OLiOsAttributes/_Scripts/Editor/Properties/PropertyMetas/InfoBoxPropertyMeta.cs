using System.Reflection;
using UnityEditor;

namespace OLiOYouxiAttributes.Editor
{
    [PropertyMeta(typeof(InfoBoxAttribute))]
    public class InfoBoxPropertyMeta : APropertyMeta
    {
        public override void ApplyPropertyMeta(SerializedProperty property, AMetaAttribute metaAttribute)
        {
            InfoBoxAttribute infoBoxAttribute = (InfoBoxAttribute)metaAttribute;
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);

            if (!string.IsNullOrEmpty(infoBoxAttribute.VisibleIf))
            {
                FieldInfo conditionField = ReflectionUtility.GetField(target, infoBoxAttribute.VisibleIf);
                if (conditionField != null &&
                    conditionField.FieldType == typeof(bool))
                {
                    if ((bool)conditionField.GetValue(target))
                    {
                        this.DrawInfoBox(infoBoxAttribute.Text, infoBoxAttribute.Type);
                    }

                    return;
                }

                MethodInfo conditionMethod = ReflectionUtility.GetMethod(target, infoBoxAttribute.VisibleIf);
                if (conditionMethod != null &&
                    conditionMethod.ReturnType == typeof(bool) &&
                    conditionMethod.GetParameters().Length == 0)
                {
                    if ((bool)conditionMethod.Invoke(target, null))
                    {
                        this.DrawInfoBox(infoBoxAttribute.Text, infoBoxAttribute.Type);
                    }

                    return;
                }

                string warning = infoBoxAttribute.GetType().Name + " 作用在一个有效的布尔值字段条件或着一个方法方法名！！！";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: PropertyUtility.GetTargetObject(property), logToConsole: false);
            }
            else
            {
                this.DrawInfoBox(infoBoxAttribute.Text, infoBoxAttribute.Type);
            }
        }

        private void DrawInfoBox(string infoText, InfoBoxType infoBoxType)
        {
            switch (infoBoxType)
            {
                case InfoBoxType.Normal:
                    EditorGUILayout.HelpBox(infoText, MessageType.Info);
                    break;

                case InfoBoxType.Warning:
                    EditorGUILayout.HelpBox(infoText, MessageType.Warning);
                    break;

                case InfoBoxType.Error:
                    EditorGUILayout.HelpBox(infoText, MessageType.Error);
                    break;
            }
        }
    }
}
