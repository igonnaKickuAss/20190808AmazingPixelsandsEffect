using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace OLiOYouxiAttributes.Editor
{
    [PropertyMeta(typeof(OnValueChangedAttribute))]
    public class OnValueChangedPropertyMeta : APropertyMeta
    {
        public override void ApplyPropertyMeta(SerializedProperty property, AMetaAttribute metaAttribute)
        {
            OnValueChangedAttribute onValueChangedAttribute = (OnValueChangedAttribute)metaAttribute;
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);

            MethodInfo callbackMethod = ReflectionUtility.GetMethod(target, onValueChangedAttribute.CallbackName);
            if (callbackMethod != null &&
                callbackMethod.ReturnType == typeof(void) &&
                callbackMethod.GetParameters().Length == 0)
            {
                property.serializedObject.ApplyModifiedProperties(); // 我们必须应用已编辑元数据，这样回调函数就可以被执行

                callbackMethod.Invoke(target, null);
            }
            else
            {
                EditorDrawUtility.DrawHelpBox(onValueChangedAttribute.GetType().Name + " 只作用于无返回值且无参数的函数", MessageType.Warning, context: target);
            }
        }
    }
}
