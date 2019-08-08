using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace OLiOYouxiAttributes.Editor
{
    /// <summary>
    /// 这个类缓存了所有自定义类在inspector上的数据显示
    /// </summary>
    [CustomEditor(typeof(UnityEngine.Object), true)]
    [CanEditMultipleObjects]
	public class InspectorEditor : UnityEditor.Editor
	{
        #region -- SerializedProperty Script --
        private SerializedProperty script = null;

        #endregion

        #region -- SerializedObject --
        private SerializedObject seriObj = null;

        #endregion

        #region -- Private Data --
        private IEnumerable<FieldInfo> fields = null;   //普通字段们

        private IEnumerable<MethodInfo> methods = null;     //普通方法们

        private IEnumerable<PropertyInfo> nativeProperties = null;  //默认属性们

        private IEnumerable<FieldInfo> nonSerializedFields = null;  //非序列化的字段们（不能被编辑那种）

        private HashSet<FieldInfo> groupedFields = null;    //被组化的字段们

        private Dictionary<string, List<FieldInfo>> groupedFieldsByGroupName = null;    //通过组名字被组化的字段们（可查询）

        private Dictionary<string, SerializedProperty> serializedPropertiesByFieldName = null;  //通过字段名字被组化的序列化属性们（可查询）
        
        #endregion

        #region -- VAR --
        private bool useDefaultInspector = false;

        #endregion

        #region -- MONO APIMethods --
        //初始化值
        private void OnEnable()
        {
            seriObj = base.serializedObject;

            script = seriObj.FindProperty("m_Script");
            
            //把所有字段们缓存起来
            fields = ReflectionUtility.GetAllFields(this.target, fi => seriObj.FindProperty(fi.Name) != null);

            if (fields.All(fi => fi.GetCustomAttributes(typeof(OLiOYouxiAttribute), true).Length == 0))
            {
                //哈哈哈哈，居然没有用奥利奥特性，那就用unity的把
                useDefaultInspector = true;
            }
            else
            {
                useDefaultInspector = false;

                //来把groupedFields缓存了
                groupedFields = new HashSet<FieldInfo>(fields.Where(fi => fi.GetCustomAttributes(typeof(AGroupAttribute), true).Length > 0));

                //来把groupedFieldsByGroupName缓存了
                groupedFieldsByGroupName = new Dictionary<string, List<FieldInfo>>();
                foreach (FieldInfo groupedField in groupedFields)
                {
                    //拿到组头名字 //第一个
                    string groupName = (groupedField.GetCustomAttributes(typeof(AGroupAttribute), true)[0] as AGroupAttribute).Name;

                    if (groupedFieldsByGroupName.ContainsKey(groupName))
                    {
                        //那就加入这个组
                        groupedFieldsByGroupName[groupName].Add(groupedField);
                    }
                    else
                    {
                        //新建一个组咯
                        groupedFieldsByGroupName[groupName] = new List<FieldInfo>()
                        {
                            groupedField
                        };
                    }
                }

                //来把serializedPropertiesByFieldName缓存了
                serializedPropertiesByFieldName = new Dictionary<string, SerializedProperty>();
                foreach (FieldInfo fieldInfo in fields)
                {
                    serializedPropertiesByFieldName[fieldInfo.Name] = seriObj.FindProperty(fieldInfo.Name);
                }
            }
            
            //来把nonSerializedFields缓存了
            nonSerializedFields = ReflectionUtility.GetAllFields(this.target,
                fi => fi.GetCustomAttributes(typeof(ADrawerAttribute), true).Length > 0 &&
                seriObj.FindProperty(fi.Name) == null
                );

            //来把nativeProperties缓存了
            nativeProperties = ReflectionUtility.GetAllProperties(this.target,
                pi => pi.GetCustomAttributes(typeof(ADrawerAttribute), true).Length > 0
                );

            //来把methods缓存了
            methods = ReflectionUtility.GetAllMethods(this.target,
                mi => mi.GetCustomAttributes(typeof(ADrawerAttribute), true).Length > 0
                );
        }

        private void OnDisable()
        {
            //清理缓存  
            //这很重要！！！！
            //TODO...
            DPropertyDrawer.ClearCache();
        }

        public override void OnInspectorGUI()
        {
            if (useDefaultInspector)
            {
                //居然没有用奥利奥特性，我不中意你了，你回去unity那里吧
                base.DrawDefaultInspector();
            }
            else
            {
                seriObj.Update();

                //确认一下东西还在内存里
                if (script != null)
                {
                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(script);
                    GUI.enabled = true;
                }

                //绘画字段（fields）
                HashSet<string> drawnGroups = new HashSet<string>();
                foreach (FieldInfo fieldInfo in fields) 
                {
                    if (groupedFields.Contains(fieldInfo))
                    {
                        //绘制在“组织”里边的字段
                        //拿第一个
                        string groupName = (fieldInfo.GetCustomAttributes(typeof(AGroupAttribute), true)[0] as AGroupAttribute).Name;
                        if (!drawnGroups.Contains(groupName))
                        {
                            drawnGroups.Add(groupName);


                            APropertyGrouper grouper = GetPropertyGrouperForField(fieldInfo);
                            if (grouper != null)
                            {
                                //这个字段们是“组织”里的，快安排上
                                grouper.BeginGroup(groupName);
                                ValidateAndDrawFields(groupedFieldsByGroupName[groupName]);
                                grouper.EndGroup();
                            }
                            else
                            {
                                //这个字段们是“群众”面貌。。
                                ValidateAndDrawFields(groupedFieldsByGroupName[groupName]);
                            }
                        }
                    }
                    else
                    {
                        //绘制这个“群众”（不属于组里的）里边的字段
                        ValidateAndDrawField(fieldInfo);
                    }
                }
                //刷新修改的值
                seriObj.ApplyModifiedProperties();
            }

            //绘制非序列化字段（nonSerializedFields）
            foreach (FieldInfo fieldInfo in nonSerializedFields)
            {
                ADrawerAttribute ADrawerAttribute = (ADrawerAttribute)fieldInfo.GetCustomAttributes(typeof(ADrawerAttribute), true)[0];
                AFieldDrawer fieldDrawer = DFieldDrawer.GetDrawerForAttribute(ADrawerAttribute.GetType());
                if (fieldDrawer != null)
                    fieldDrawer.DrawField(this.target, fieldInfo);
            }

            //绘制本地属性(native properties)
            //foreach (PropertyInfo propertyInfo in nativeProperties) 
            //{
            //    ADrawerAttribute ADrawerAttribute = (ADrawerAttribute)propertyInfo.GetCustomAttributes(typeof(ADrawerAttribute), true)[0];
            //    ANativePropertyDrawer nativePropertyDrawer = DNativePropertiesDrawer.GetDrawerForAttribute(ADrawerAttribute.GetType());
            //    if (nativePropertyDrawer != null)
            //        nativePropertyDrawer.DrawNativeProperty(this.target, propertyInfo);
            //}
            var nps = nativeProperties.ToList();
            for (int i = 0; i < nps.Count; i++)
            {
                ADrawerAttribute ADrawerAttribute = (ADrawerAttribute)nps[i].GetCustomAttributes(typeof(ADrawerAttribute), true)[0];
                ANativePropertyDrawer nativePropertyDrawer = DNativePropertiesDrawer.GetDrawerForAttribute(ADrawerAttribute.GetType());
                if (nativePropertyDrawer != null)
                    nativePropertyDrawer.DrawNativeProperty(this.target, nps[i]);
            }



            //绘制方法（Methods）
            foreach (MethodInfo methodInfo in methods) 
            {
                ADrawerAttribute ADrawerAttribute = (ADrawerAttribute)methodInfo.GetCustomAttributes(typeof(ADrawerAttribute), true)[0];
                AMethodDrawer methodDrawer = DMethodDrawer.GetDrawerForAttribute(ADrawerAttribute.GetType());
                if (methodDrawer != null)
                    methodDrawer.DrawMethod(this.target, methodInfo);
            }
        }

        #endregion

        #region -- Helper --
        /// <summary>
        /// 更新数据并且绘制他们
        /// </summary>
        /// <param name="fields"></param>
        private void ValidateAndDrawFields(IEnumerable<FieldInfo> fields)
        {
            foreach (FieldInfo fieldInfo in fields)
            {
                ValidateAndDrawField(fieldInfo);
            }
        }

        private void ValidateAndDrawField(FieldInfo fieldInfo)
        {
            if (!ShouldDrawField(fieldInfo))
                return;

            //更新数据
            //应用覆盖元数据
            //画他
            //TODO...

            ValidateField(fieldInfo);
            ApplyFieldMeta(fieldInfo);
            DrawField(fieldInfo);

        }

        //----------------------------------- FOR Validate&Draw -----------------------------------
        private bool ShouldDrawField(FieldInfo fieldInfo)
        {
            APropertyDrawCondition drawCondition = GetPropertyDrawConditionForField(fieldInfo);
            if (drawCondition != null)
            {
                //这个字段满足绘制条件吗？
                bool canDrawProperty = drawCondition.CanDrawProperty(serializedPropertiesByFieldName[fieldInfo.Name]);
                if (!canDrawProperty)
                    return false;
            }

            HideInInspector[] hideInInspectorAttributes = (HideInInspector[])fieldInfo.GetCustomAttributes(typeof(HideInInspector), true);
            if (hideInInspectorAttributes.Length > 0)
                return false;   //这个字段有HideInInspector特性吗？（他是内置特性）
            return true;
        }

        private void DrawField(FieldInfo fieldInfo)
        {
            //BeginChangeCheck()...EndChangeCheck()的用法！！

            EditorGUI.BeginChangeCheck();
            APropertyDrawer propertyDrawer = GetPropertyDrawForField(fieldInfo);

            if (propertyDrawer != null)
                propertyDrawer.DrawProperty(serializedPropertiesByFieldName[fieldInfo.Name]);   //你的绘制方案
            else
                EditorDrawUtility.DrawPropertyField(serializedPropertiesByFieldName[fieldInfo.Name]);   //那就用默认unity的绘制

            if (EditorGUI.EndChangeCheck())
            {
                OnValueChangedAttribute[] onValueChangedAttributes = (OnValueChangedAttribute[])fieldInfo.GetCustomAttributes(typeof(OnValueChangedAttribute), true);
                foreach (OnValueChangedAttribute onValueChangedAttribute in onValueChangedAttributes) 
                {
                    APropertyMeta propertyMeta = DPropertyMeta.GetMetaForAttribute(onValueChangedAttribute.GetType());
                    if (propertyMeta != null)
                        propertyMeta.ApplyPropertyMeta(serializedPropertiesByFieldName[fieldInfo.Name], onValueChangedAttribute);
                }
            }
        }


        private void ApplyFieldMeta(FieldInfo fieldInfo)
        {
            AMetaAttribute[] metaAttributes = fieldInfo.GetCustomAttributes(typeof(AMetaAttribute), true)
                .Where(a => a.GetType() != typeof(OnValueChangedAttribute))     //Linq操作
                .Select(o => o as AMetaAttribute)           //Linq操作（我很中意啊）
                .ToArray();

            //进行数组数据排序
            Array.Sort(metaAttributes, (x, y) =>        //参考：https://zhidao.baidu.com/question/62348471.html 用户：xyphoenix 的回答
            {
                return x.Order - y.Order;
            });

            //更新元数据
            foreach (AMetaAttribute metaAttribute in metaAttributes) 
            {
                APropertyMeta propertyMeta = DPropertyMeta.GetMetaForAttribute(metaAttribute.GetType());
                if (propertyMeta != null)
                    propertyMeta.ApplyPropertyMeta(serializedPropertiesByFieldName[fieldInfo.Name], metaAttribute);
            }
        }

        private void ValidateField(FieldInfo fieldInfo)
        {
            AValidatorAttribute[] validatorAttributes = (AValidatorAttribute[])fieldInfo.GetCustomAttributes(typeof(AValidatorAttribute), true);

            //更新字段
            foreach (AValidatorAttribute validatorAttribute in validatorAttributes) 
            {
                APropertyValidator propertyValidator = DPropertyValidator.GetValidatorForAttribute(validatorAttribute.GetType());
                if (propertyValidator != null)
                    propertyValidator.ValidateProperty(serializedPropertiesByFieldName[fieldInfo.Name]);
            }
        }

        //----------------------------------- FOR Validate&Draw -----------------------------------




        //----------------------------------- FOR ShouldDrawField -----------------------------------
        private APropertyDrawCondition GetPropertyDrawConditionForField(FieldInfo fieldInfo)
        {
            ADrawConditionAttribute[] drawConditionAttributes = (ADrawConditionAttribute[])fieldInfo.GetCustomAttributes(typeof(ADrawConditionAttribute), true);
            if (drawConditionAttributes.Length > 0)
            {
                //你的绘制条件
                APropertyDrawCondition propertyDrawCondition = DPropertyDrawCondition.GetDrawConditionForAttribute(drawConditionAttributes[0].GetType());
                return propertyDrawCondition;
            }
            else
                return null;
        }


        //----------------------------------- FOR ShouldDrawField -----------------------------------




        //----------------------------------- FOR DrawField -----------------------------------
        private APropertyDrawer GetPropertyDrawForField(FieldInfo fieldInfo)
        {
            ADrawerAttribute[] ADrawerAttributes = (ADrawerAttribute[])fieldInfo.GetCustomAttributes(typeof(ADrawerAttribute), true);
            if (ADrawerAttributes.Length > 0)
            {
                //获取自定义绘制方案
                APropertyDrawer propertyDrawer = DPropertyDrawer.GetDrawerForAttribute(ADrawerAttributes[0].GetType());  
                return propertyDrawer;
            }
            else
                return null;
        }
        //----------------------------------- FOR DrawField -----------------------------------




        //----------------------------------- FOR OnInspectorGUI -----------------------------------
        private APropertyGrouper GetPropertyGrouperForField(FieldInfo fieldInfo)
        {
            AGroupAttribute[] groupAttributes = (AGroupAttribute[])fieldInfo.GetCustomAttributes(typeof(AGroupAttribute), true);
            if (groupAttributes.Length > 0)
            {
                //获取你的组样式
                APropertyGrouper propertyGrouper = DPropertyGrouper.GetGrouperForAttribute(groupAttributes[0].GetType());
                return propertyGrouper;
            }
            else
                return null;

        }
        //----------------------------------- FOR DrawField -----------------------------------


        #endregion

    }
}