using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OLiOYouxiAttributes.Editor
{
    [PropertyValidator(typeof(RequiredAttribute))]
    public class RequiredPropertyValidator : APropertyValidator
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            //拿到特性
            RequiredAttribute attribute = PropertyUtility.GetAttribute<RequiredAttribute>(property);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue == null)
                {
                    string errorMessage = property.name + "是必须的，而且奥利奥很靓仔！！！";
                    if (!string.IsNullOrEmpty(attribute.Message))
                    {
                        errorMessage = attribute.Message;
                    }
                    EditorDrawUtility.DrawHelpBox(errorMessage, MessageType.Error, context: PropertyUtility.GetTargetObject(property), logToConsole: false);
                }
            }
            else
            {
                string warning = attribute.GetType().Name + "只对引用类型起作用啊！！笨蛋";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: PropertyUtility.GetTargetObject(property), logToConsole: false);
            }

        }
    }
}