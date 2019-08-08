// 这是自动生成的类

using System;
using System.Collections.Generic;

namespace OLiOYouxiAttributes.Editor
{
    static public class DMethodDrawer
    {
        static private  Dictionary<Type, AMethodDrawer> drawersByAttributeType;

        static DMethodDrawer()
        {
            drawersByAttributeType = new Dictionary<Type, AMethodDrawer>();
            drawersByAttributeType[typeof(ButtonAttribute)] = new ButtonMethodDrawer();
            
        }

        static public AMethodDrawer GetDrawerForAttribute(Type attributeType)
        {
            AMethodDrawer drawer;
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

