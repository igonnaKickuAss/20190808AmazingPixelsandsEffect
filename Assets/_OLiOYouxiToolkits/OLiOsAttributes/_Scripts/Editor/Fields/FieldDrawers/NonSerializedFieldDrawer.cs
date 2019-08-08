using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace OLiOYouxiAttributes.Editor
{
    /// <summary>
    /// 封装非序列化字段绘制（DrawHelpBox）的一些内置方法，单纯画画罢了
    /// </summary>
    [FieldDrawer(typeof(NonSerializedFieldAttribute))]
    public class NonSerializedFieldDrawer : AFieldDrawer
    {
        public override void DrawField(UnityEngine.Object target, FieldInfo fieldInfo)
        {
            object value = fieldInfo.GetValue(target);

            if(value == null)
            {
                string warning = string.Format("{0} 需要 {1} 类型", typeof(NonSerializedFieldDrawer).Name, "Reference");
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target, logToConsole: false);
            }
            else if(!EditorDrawUtility.DrawLayoutField(value, fieldInfo.Name))
            {
                string warning = string.Format("{0} 不支持 {1} 类型", typeof(NonSerializedFieldDrawer).Name, fieldInfo.FieldType.Name);
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target, logToConsole: false);
            }
        }
    }
}
