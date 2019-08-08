// 这是自动生成的类

using System;
using System.Collections.Generic;

namespace OLiOYouxiAttributes.Editor
{
    static public class DPropertyDrawer
    {
        static private  Dictionary<Type, APropertyDrawer> drawersByAttributeType;

        static DPropertyDrawer()
        {
            drawersByAttributeType = new Dictionary<Type, APropertyDrawer>();
            drawersByAttributeType[typeof(AssetPreviewAttribute)] = new ShowAssetPreviewPropertyDrawer();
            drawersByAttributeType[typeof(DisableIfAttribute)] = new DisableIfPropertyDrawer();
            drawersByAttributeType[typeof(DropdownAttribute)] = new DropdownPropertyDrawer();
            drawersByAttributeType[typeof(EnableIfAttribute)] = new EnableIfPropertyDrawer();
            drawersByAttributeType[typeof(LabelAttribute)] = new LabelPropertyDrawer();
            drawersByAttributeType[typeof(MinMaxSliderAttribute)] = new MinMaxSliderPropertyDrawer();
            drawersByAttributeType[typeof(ProgressBarAttribute)] = new ProgressBarPropertyDrawer();
            drawersByAttributeType[typeof(ReadOnlyAttribute)] = new ReadOnlyPropertyDrawer();
            drawersByAttributeType[typeof(ReorderableListAttribute)] = new ReorderableListPropertyDrawer();
            drawersByAttributeType[typeof(ResizableTextAreaAttribute)] = new ResizableTextAreaPropertyDrawer();
            drawersByAttributeType[typeof(SliderAttribute)] = new SliderPropertyDrawer();
            drawersByAttributeType[typeof(TagAttribute)] = new TagPropertyDrawer();
            
        }

        static public APropertyDrawer GetDrawerForAttribute(Type attributeType)
        {
            APropertyDrawer drawer;
            if (drawersByAttributeType.TryGetValue(attributeType, out drawer))
            {
                return drawer;
            }
            else
            {
                return null;
            }
        }

        static public void ClearCache()
        {
            foreach (var kvp in drawersByAttributeType)
            {
                kvp.Value.ClearCache();
            }
        }
    }
}

