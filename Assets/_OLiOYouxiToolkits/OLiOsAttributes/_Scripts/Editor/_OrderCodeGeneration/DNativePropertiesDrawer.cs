// 这是自动生成的类

using System;
using System.Collections.Generic;

namespace OLiOYouxiAttributes.Editor
{
    static public class DNativePropertiesDrawer
    {
        static private  Dictionary<Type, ANativePropertyDrawer> drawersByAttributeType;

        static DNativePropertiesDrawer()
        {
            drawersByAttributeType = new Dictionary<Type, ANativePropertyDrawer>();
            drawersByAttributeType[typeof(ExposePropertyAttribute)] = new ExposePropertyNativePropertyDrawer();
            drawersByAttributeType[typeof(NativePropertyAttribute)] = new NativePropertyNativePropertyDrawer();
            
        }

        static public ANativePropertyDrawer GetDrawerForAttribute(Type attributeType)
        {
            ANativePropertyDrawer drawer;
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

