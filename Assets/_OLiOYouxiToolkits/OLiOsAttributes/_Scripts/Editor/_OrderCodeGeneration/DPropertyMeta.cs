// 这是自动生成的类

using System;
using System.Collections.Generic;

namespace OLiOYouxiAttributes.Editor
{
    static public class DPropertyMeta
    {
        static private  Dictionary<Type, APropertyMeta> metasByAttributeType;

        static DPropertyMeta()
        {
            metasByAttributeType = new Dictionary<Type, APropertyMeta>();
            metasByAttributeType[typeof(InfoBoxAttribute)] = new InfoBoxPropertyMeta();
            metasByAttributeType[typeof(OnValueChangedAttribute)] = new OnValueChangedPropertyMeta();
            
        }

        static public APropertyMeta GetMetaForAttribute(Type attributeType)
        {
            APropertyMeta meta;
            if (metasByAttributeType.TryGetValue(attributeType, out meta))
            {
                return meta;
            }
            else
            {
                return null;
            }
        }
    }
}

