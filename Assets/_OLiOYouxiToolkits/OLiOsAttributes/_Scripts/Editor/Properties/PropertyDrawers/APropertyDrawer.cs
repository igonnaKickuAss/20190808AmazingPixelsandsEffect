using UnityEditor;

namespace OLiOYouxiAttributes.Editor
{
    /// <summary>
    /// 关于property绘制的抽象类
    /// </summary>
    public abstract class APropertyDrawer
	{
        public abstract void DrawProperty(SerializedProperty property);
        public virtual void ClearCache() { }

	}
}