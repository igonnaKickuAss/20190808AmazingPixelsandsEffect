#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace OLiOYouxi.Toolkits
{
    using OLiOYouxi.OSystem;

#if UNITY_EDITOR
    public class Tools
    {
        [MenuItem("OLiOYouxiToolkits/删除组件/MeshColliders")]
        static public void RemoveMeshCollider()
        {
            GameObject[] colliders = Selection.gameObjects;
            for (int i = 0; i < colliders.Length; i++)
            {
                MeshCollider mc = colliders[i].GetComponent<MeshCollider>();
                if (mc)
                {
                    GameObject.DestroyImmediate(mc);
                    DebuggerFather.instance.ToDebugLogWarn("移除MeshColliders成功!", EnumCentre.ColorName.green);
                }
            }
        }

        [MenuItem("OLiOYouxiToolkits/删除组件/MissingScripts")]
        static public void CleanupMissingScripts()
        {
            int ci;
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                ci = 0;
                var gameObject = Selection.gameObjects[i];

                // We must use the GetComponents array to actually detect missing components
                var components = gameObject.GetComponents<Component>();

                // Create a serialized object so that we can edit the component list
                var serializedObject = new SerializedObject(gameObject);
                // Find the component list property
                var prop = serializedObject.FindProperty("m_Component");

                // Track how many components we've removed
                int r = 0;

                // Iterate over all components
                for (int j = 0; j < components.Length; j++)
                {
                    // Check if the ref is null
                    if (components[j] == null)
                    {
                        // If so, remove from the serialized component array
                        prop.DeleteArrayElementAtIndex(j - r);
                        ci++;
                        DebuggerFather.instance.ToDebugLogWarn(string.Format("移除{0}里的MissingScript成功!", components[i].name), EnumCentre.ColorName.green);
                        // Increment removed count
                        r++;
                    }
                }
                if (ci == 0)
                {
                    DebuggerFather.instance.ToDebugLogWarn(string.Format("没有在{0}里找到MissingScript!", components[i].name), EnumCentre.ColorName.red);
                }


                // Apply our changes to the game object
                serializedObject.ApplyModifiedProperties();
            }
        }

        [MenuItem("OLiOYouxiToolkits/折叠所有")]
        static public void UnfoldSelection()
        {
            EditorApplication.ExecuteMenuItem("Window/Hierarchy");
            var hierarchyWindow = EditorWindow.focusedWindow;
            var expandMethodInfo = hierarchyWindow.GetType().GetMethod("SetExpandedRecursive");
            foreach (GameObject root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                expandMethodInfo.Invoke(hierarchyWindow, new object[] { root.GetInstanceID(), false });
            }
        }



    }


#endif
}
