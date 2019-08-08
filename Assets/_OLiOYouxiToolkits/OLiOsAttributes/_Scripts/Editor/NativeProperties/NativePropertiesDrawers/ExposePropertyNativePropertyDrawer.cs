using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace OLiOYouxiAttributes.Editor
{
    [NativePropertyDrawer(typeof(ExposePropertyAttribute))]
    public class ExposePropertyNativePropertyDrawer : ANativePropertyDrawer
    {
        private MethodInfo getter;
        private MethodInfo setter;
        public override void DrawNativeProperty(Object target, PropertyInfo propertyInfo)
        {
            if (getter == null)
                getter = propertyInfo.GetGetMethod();

            var oldValue = getter.Invoke(target, null); //拿到属性值
            if (oldValue == null)
            {
                string warning = string.Format("{0} 需要 {1} 类型", typeof(NativePropertyNativePropertyDrawer).Name, "Reference");
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target, logToConsole: false);
            }

            System.Type type;
            var newValue = EditorDrawUtility.DrawPropertyLayoutField(oldValue, propertyInfo.Name, out type);  //画他！！

            if (newValue == null)  
            {
                string warning = string.Format("{0} 不支持 {1} 类型", typeof(NativePropertyNativePropertyDrawer).Name, propertyInfo.PropertyType.Name);
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target, logToConsole: false);
                return;
            }
            if (setter == null)
                setter = propertyInfo.GetSetMethod();   //更改属性值

            if (!EditorDrawUtility.OldNewComparer(oldValue, newValue, type))  
                setter.Invoke(target, new[] { newValue });  


        }
    }
}