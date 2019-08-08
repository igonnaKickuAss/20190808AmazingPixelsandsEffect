// 这个是自动生成的类

using System;
using System.Collections.Generic;

namespace OLiOYouxiAttributes.Editor
{
    static public class DFieldDrawer
    {
        static private  Dictionary<Type, AFieldDrawer> drawersByAttributeType;

        static DFieldDrawer()
        {
            drawersByAttributeType = new Dictionary<Type, AFieldDrawer>();
            drawersByAttributeType[typeof(NonSerializedFieldAttribute)] = new NonSerializedFieldDrawer();
            
        }

        static public AFieldDrawer GetDrawerForAttribute(Type attributeType)
        {
            AFieldDrawer drawer;
            if (drawersByAttributeType.TryGetValue(attributeType, out drawer))
            {
                return drawer;
            }
            else
            {
                return null;
            }
        }
    }
}

