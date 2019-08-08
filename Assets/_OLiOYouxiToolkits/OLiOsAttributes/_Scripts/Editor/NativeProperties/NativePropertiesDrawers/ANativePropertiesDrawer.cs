using System.Reflection;

namespace OLiOYouxiAttributes.Editor
{
    /// <summary>
    /// 关于NativeProperty绘制的抽象类
    /// </summary>
    public abstract class ANativePropertyDrawer
	{
        public abstract void DrawNativeProperty(UnityEngine.Object target, PropertyInfo propertyInfo); 
	}
}