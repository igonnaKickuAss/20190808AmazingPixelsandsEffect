using System;
using UnityEditor;
using UnityEngine;

namespace OLiOYouxiAttributes.Editor
{
    static public class EditorDrawUtility
    {
        #region -- Public APIMethods --
        /// <summary>
        /// 绘制头部
        /// </summary>
        /// <param name="headerLable"></param>
        static public void DrawHeader(string headerLable)
        {
            //换行
            EditorGUILayout.Space();
            //绘制头部标签
            EditorGUILayout.LabelField(headerLable, EditorStyles.boldLabel);
        }

        /// <summary>
        /// 绘制头部（HeaderAttribute是内置特性）
        /// </summary>
        /// <param name="headerLable"></param>
        static public bool DrawHeader(SerializedProperty property)
        {
            HeaderAttribute attribute = PropertyUtility.GetAttribute<HeaderAttribute>(property);
            if (attribute != null)
            {
                DrawHeader(attribute.header);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 绘制帮助盒子（HelpBox是内置方法）
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <param name="context"></param>
        /// <param name="logToConsole"></param>
        static public void DrawHelpBox(string message, MessageType type, UnityEngine.Object context = null, bool logToConsole = true)
        {
            //绘制HelpBox
            EditorGUILayout.HelpBox(message, type);

            if (logToConsole)
            {
                switch (type)
                {
                    case MessageType.None:
                    case MessageType.Info:
                        Debug.LogFormat(context, "<color=black>{0}</color>", message);
                        break;
                    case MessageType.Warning:
                        Debug.LogWarningFormat(context, "<color=yellow>{0}</color>", message);
                        break;
                    case MessageType.Error:
                        Debug.LogErrorFormat(context, "<color=red>{0}</color>", message);
                        break;
                }
            }
        }

        /// <summary>
        /// 绘制序列化字段（这是单纯的画画罢了）
        /// </summary>
        /// <param name="property"></param>
        /// <param name="includeChildren"></param>
        static public void DrawPropertyField(SerializedProperty property, bool includeChildren = true)
        {
            EditorGUILayout.PropertyField(property, includeChildren);
        }

        /// <summary>
        /// 绘制单行字段（这也是单纯的画画而已）（只读状态）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        static public bool DrawLayoutField(object value, string label)
        {
            //先关闭这行字段的显示
            GUI.enabled = false;

            bool isDrawn = true;
            Type valueType = value.GetType();

            if (valueType == typeof(bool))
            {
                EditorGUILayout.Toggle(label, (bool)value);
            }
            else if (valueType == typeof(int))
            {
                EditorGUILayout.IntField(label, (int)value);

            }
            else if (valueType == typeof(long))
            {
                EditorGUILayout.LongField(label, (long)value);
            }
            else if (valueType == typeof(float))
            {
                EditorGUILayout.FloatField(label, (float)value);
            }
            else if (valueType == typeof(double))
            {
                EditorGUILayout.DoubleField(label, (double)value);
            }
            else if (valueType == typeof(string))
            {
                EditorGUILayout.TextField(label, (string)value);
            }
            else if (valueType == typeof(Vector2))
            {
                EditorGUILayout.Vector2Field(label, (Vector2)value);
            }
            else if (valueType == typeof(Vector3))
            {
                EditorGUILayout.Vector3Field(label, (Vector3)value);
            }
            else if (valueType == typeof(Vector4))
            {
                EditorGUILayout.Vector4Field(label, (Vector4)value);
            }
            else if (valueType == typeof(Vector2Int))
            {
                EditorGUILayout.Vector2IntField(label, (Vector2Int)value);
            }
            else if (valueType == typeof(Vector3Int))
            {
                EditorGUILayout.Vector3IntField(label, (Vector3Int)value);
            }
            else if (valueType == typeof(Color))
            {
                EditorGUILayout.ColorField(label, (Color)value);
            }
            else if (valueType == typeof(Bounds))
            {
                EditorGUILayout.BoundsField(label, (Bounds)value);
            }
            else if (valueType == typeof(BoundsInt))
            {
                EditorGUILayout.BoundsIntField(label, (BoundsInt)value);
            }
            else if (valueType == typeof(Rect))
            {
                EditorGUILayout.RectField(label, (Rect)value);
            }
            else if (valueType == typeof(RectInt))
            {
                EditorGUILayout.RectIntField(label, (RectInt)value);
            }
            else if (typeof(UnityEngine.Object).IsAssignableFrom(valueType))
            {
                EditorGUILayout.ObjectField(label, (UnityEngine.Object)value, valueType, true);
            }
            //还有什么类型的字段，来补充啊！！！
            else
            {
                //玛德，我画不出来
                isDrawn = false;
            }

            //打开这行字段的显示
            //不起作用？
            GUI.enabled = true;

            return isDrawn;

        }

        static public object DrawPropertyLayoutField(object value, string label, out Type type)
        {
            //先关闭这行字段的显示
            GUI.enabled = true;
            
            Type valueType = value.GetType();
            type = valueType;

            if (valueType == typeof(int))
            {
                return EditorGUILayout.IntField(label, (int)value);

            }
            else if (valueType == typeof(long))
            {
                return EditorGUILayout.LongField(label, (long)value);
            }
            else if (valueType == typeof(float))
            {
                return EditorGUILayout.FloatField(label, (float)value);
            }
            else if (valueType == typeof(double))
            {
                return EditorGUILayout.DoubleField(label, (double)value);
            }
            else if (valueType == typeof(string))
            {
                return EditorGUILayout.TextField(label, (string)value);
            }
            else if (valueType == typeof(Vector2))
            {
                return EditorGUILayout.Vector2Field(label, (Vector2)value);
            }
            else if (valueType == typeof(Vector3))
            {
                return EditorGUILayout.Vector3Field(label, (Vector3)value);
            }
            else if (valueType == typeof(Vector4))
            {
                return EditorGUILayout.Vector4Field(label, (Vector4)value);
            }
            else if (valueType == typeof(Vector2Int))
            {
                return EditorGUILayout.Vector2IntField(label, (Vector2Int)value);
            }
            else if (valueType == typeof(Vector3Int))
            {
                return EditorGUILayout.Vector3IntField(label, (Vector3Int)value);
            }
            else if (valueType == typeof(Color))
            {
                return EditorGUILayout.ColorField(label, (Color)value);
            }
            else if (valueType == typeof(Bounds))
            {
                return EditorGUILayout.BoundsField(label, (Bounds)value);
            }
            else if (valueType == typeof(BoundsInt))
            {
                return EditorGUILayout.BoundsIntField(label, (BoundsInt)value);
            }
            else if (valueType == typeof(Rect))
            {
                return EditorGUILayout.RectField(label, (Rect)value);
            }
            else if (valueType == typeof(RectInt))
            {
                return EditorGUILayout.RectIntField(label, (RectInt)value);
            }
            else if (typeof(UnityEngine.Object).IsAssignableFrom(valueType))
            {
                return EditorGUILayout.ObjectField(label, (UnityEngine.Object)value, valueType, true);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 比较两对象，不呢个为空
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        static public bool OldNewComparer(object oldValue, object newValue, Type valueType)
        {
            if (valueType == typeof(int))
            {
                return CompareOldNew<int>((int)oldValue, (int)newValue);
            }
            else if (valueType == typeof(long))
            {
                return CompareOldNew<long>((long)oldValue, (long)newValue);
            }
            else if (valueType == typeof(float))
            {
                return CompareOldNew<float>((float)oldValue, (float)newValue);
            }
            else if (valueType == typeof(double))
            {
                return CompareOldNew<double>((double)oldValue, (double)newValue);
            }
            else if (valueType == typeof(string))
            {
                return CompareOldNew<string>((string)oldValue, (string)newValue);
            }
            else if (valueType == typeof(Vector2))
            {
                return CompareOldNew<Vector2>((Vector2)oldValue, (Vector2)newValue);
            }
            else if (valueType == typeof(Vector3))
            {
                return CompareOldNew<Vector3>((Vector3)oldValue, (Vector3)newValue);
            }
            else if (valueType == typeof(Vector4))
            {
                return CompareOldNew<Vector4>((Vector4)oldValue, (Vector4)newValue);
            }
            else if (valueType == typeof(Vector2Int))
            {
                return CompareOldNew<Vector2Int>((Vector2Int)oldValue, (Vector2Int)newValue);
            }
            else if (valueType == typeof(Vector3Int))
            {
                return CompareOldNew<Vector3Int>((Vector3Int)oldValue, (Vector3Int)newValue);
            }
            else if (valueType == typeof(Color))
            {
                return CompareOldNew<Color>((Color)oldValue, (Color)newValue);
            }
            else if (valueType == typeof(Bounds))
            {
                return CompareOldNew<Bounds>((Bounds)oldValue, (Bounds)newValue);
            }
            else if (valueType == typeof(BoundsInt))
            {
                return CompareOldNew<BoundsInt>((BoundsInt)oldValue, (BoundsInt)newValue);
            }
            else if (valueType == typeof(Rect))
            {
                return CompareOldNew<Rect>((Rect)oldValue, (Rect)newValue);
            }
            else if (valueType == typeof(RectInt))
            {
                return CompareOldNew<RectInt>((RectInt)oldValue, (RectInt)newValue);
            }
            else if (typeof(UnityEngine.Object).IsAssignableFrom(valueType))
            {
                return CompareOldNew<UnityEngine.Object>((UnityEngine.Object)oldValue, (UnityEngine.Object)newValue);
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 比较两值是否有变化，不能为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        static private bool CompareOldNew<T>(T oldValue, T newValue)
        {
            return oldValue.Equals(newValue);
        }
        #endregion


    }
}