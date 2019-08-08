using System.Reflection;
using UnityEditor;

namespace OLiOYouxiAttributes.Editor
{
    [NativePropertyDrawer(typeof(NativePropertyAttribute))]
    public class NativePropertyNativePropertyDrawer : ANativePropertyDrawer
    {
        public override void DrawNativeProperty(UnityEngine.Object target, PropertyInfo property)
        {
            object value = property.GetValue(target, null);

            if (value == null)
            {
                string warning = string.Format("{0} 需要 {1} 类型", typeof(NativePropertyNativePropertyDrawer).Name, "Reference");
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target, logToConsole: false);
            }
            else if (!EditorDrawUtility.DrawLayoutField(value, property.Name))  //画他！！
            {
                string warning = string.Format("{0} 不支持 {1} 类型", typeof(NativePropertyNativePropertyDrawer).Name, property.PropertyType.Name);
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target, logToConsole: false);
            }
        }
    }
}
