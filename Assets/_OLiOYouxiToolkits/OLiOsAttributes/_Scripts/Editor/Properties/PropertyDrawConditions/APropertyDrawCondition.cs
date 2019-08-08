using UnityEditor;

namespace OLiOYouxiAttributes.Editor
{
    /// <summary>
    /// 关于Property是否绘制的抽象类
    /// </summary>
    public abstract class APropertyDrawCondition
	{
        public abstract bool CanDrawProperty(SerializedProperty property);
	}
}