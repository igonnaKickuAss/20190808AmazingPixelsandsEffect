using UnityEditor;

namespace OLiOYouxiAttributes.Editor
{
    [PropertyValidator(typeof(MinValueAttribute))]
    public class MinValuePropertyValidator : APropertyValidator
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            MinValueAttribute minValueAttribute = PropertyUtility.GetAttribute<MinValueAttribute>(property);

            if (property.propertyType == SerializedPropertyType.Integer)
            {
                if (property.intValue < minValueAttribute.MinValue)
                {
                    property.intValue = (int)minValueAttribute.MinValue;
                }
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                if (property.floatValue < minValueAttribute.MinValue)
                {
                    property.floatValue = minValueAttribute.MinValue;
                }
            }
            else
            {
                string warning = minValueAttribute.GetType().Name + " 只作用于整型和单精度浮点型字段！";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: PropertyUtility.GetTargetObject(property), logToConsole: false);
            }
        }
    }
}
