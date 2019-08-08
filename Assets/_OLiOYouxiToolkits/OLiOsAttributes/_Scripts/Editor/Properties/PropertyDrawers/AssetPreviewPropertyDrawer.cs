using UnityEngine;
using UnityEditor;

namespace OLiOYouxiAttributes.Editor
{
    [PropertyDrawer(typeof(AssetPreviewAttribute))]
    public class ShowAssetPreviewPropertyDrawer : APropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            EditorDrawUtility.DrawPropertyField(property);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue != null)
                {
                    Texture2D previewTexture = AssetPreview.GetAssetPreview(property.objectReferenceValue);
                    if (previewTexture != null)
                    {
                        AssetPreviewAttribute showAssetPreviewAttribute = PropertyUtility.GetAttribute<AssetPreviewAttribute>(property);
                        int width = Mathf.Clamp(showAssetPreviewAttribute.Width, 0, previewTexture.width);
                        int height = Mathf.Clamp(showAssetPreviewAttribute.Height, 0, previewTexture.height);

                        GUILayout.Label(previewTexture, GUILayout.MaxWidth(width), GUILayout.MaxHeight(height));
                    }
                    else
                    {
                        string warning = property.name + " 没有一个资源预览！";
                        EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: PropertyUtility.GetTargetObject(property), logToConsole: false);
                    }
                }
            }
            else
            {
                string warning = property.name + " 没有一个资源预览！";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: PropertyUtility.GetTargetObject(property), logToConsole: false);
            }
        }
    }
}
