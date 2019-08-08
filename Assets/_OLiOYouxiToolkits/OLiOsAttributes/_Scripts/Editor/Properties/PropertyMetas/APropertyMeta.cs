using UnityEditor;

namespace OLiOYouxiAttributes.Editor
{
	public abstract class APropertyMeta
	{
        public abstract void ApplyPropertyMeta(SerializedProperty property, AMetaAttribute metaAttribute);
	}
}