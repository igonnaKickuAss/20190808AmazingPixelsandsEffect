// 这是自动生成的类

using System;
using System.Collections.Generic;

namespace OLiOYouxiAttributes.Editor
{
    static public class DPropertyGrouper
    {
        static private  Dictionary<Type, APropertyGrouper> groupersByAttributeType;

        static DPropertyGrouper()
        {
            groupersByAttributeType = new Dictionary<Type, APropertyGrouper>();
            groupersByAttributeType[typeof(BoxGroupAttribute)] = new BoxGroupPropertyGrouper();
            
        }

        static public APropertyGrouper GetGrouperForAttribute(Type attributeType)
        {
            APropertyGrouper grouper;
            if (groupersByAttributeType.TryGetValue(attributeType, out grouper))
            {
                return grouper;
            }
            else
            {
                return null;
            }
        }
    }
}

