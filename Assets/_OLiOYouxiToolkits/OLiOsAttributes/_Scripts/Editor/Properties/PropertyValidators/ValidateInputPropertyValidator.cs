using System;
using System.Reflection;
using UnityEditor;

namespace OLiOYouxiAttributes.Editor
{
    [PropertyValidator(typeof(ValidateInputAttribute))]
    public class ValidateInputPropertyValidator : APropertyValidator
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            ValidateInputAttribute validateInputAttribute = PropertyUtility.GetAttribute<ValidateInputAttribute>(property);
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);

            MethodInfo validationCallback = ReflectionUtility.GetMethod(target, validateInputAttribute.CallbackName);

            if (validationCallback != null &&
                validationCallback.ReturnType == typeof(bool) &&
                validationCallback.GetParameters().Length == 1)
            {
                FieldInfo fieldInfo = ReflectionUtility.GetField(target, property.name);
                Type fieldType = fieldInfo.FieldType;
                Type parameterType = validationCallback.GetParameters()[0].ParameterType;

                if (fieldType == parameterType)
                {
                    if (!(bool)validationCallback.Invoke(target, new object[] { fieldInfo.GetValue(target) }))
                    {
                        if (string.IsNullOrEmpty(validateInputAttribute.Message))
                        {
                            EditorDrawUtility.DrawHelpBox(property.name + " 不是有效的", MessageType.Error, context: target, logToConsole: false);
                        }
                        else
                        {
                            EditorDrawUtility.DrawHelpBox(validateInputAttribute.Message, MessageType.Error, context: target, logToConsole: false);
                        }
                    }
                }
                else
                {
                    string warning = "这个字段类型跟回调函数参数类型不同啊！";
                    EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target, logToConsole: false);
                }
            }
            else
            {
                string warning =
                    validateInputAttribute.GetType().Name +
                    " 需要一个返回布尔类型的回调函数和一个与字段类型相同的单参！";


                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target, logToConsole: false);
            }
        }
    }
}
