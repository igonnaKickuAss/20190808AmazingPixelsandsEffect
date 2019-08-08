// 这是自动生成的类

using System;
using System.Collections.Generic;

namespace OLiOYouxiAttributes.Editor
{
    static public class DPropertyDrawCondition
    {
        static private  Dictionary<Type, APropertyDrawCondition> drawConditionsByAttributeType;

        static DPropertyDrawCondition()
        {
            drawConditionsByAttributeType = new Dictionary<Type, APropertyDrawCondition>();
            drawConditionsByAttributeType[typeof(HideIfAttribute)] = new HideIfPropertyDrawCondition();
            drawConditionsByAttributeType[typeof(ShowIfAttribute)] = new ShowIfPropertyDrawCondition();
            
        }

        static public APropertyDrawCondition GetDrawConditionForAttribute(Type attributeType)
        {
            APropertyDrawCondition drawCondition;
            if (drawConditionsByAttributeType.TryGetValue(attributeType, out drawCondition))
            {
                return drawCondition;
            }
            else
            {
                return null;
            }
        }
    }
}

