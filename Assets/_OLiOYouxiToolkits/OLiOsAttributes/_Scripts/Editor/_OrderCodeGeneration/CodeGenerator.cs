using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

namespace OLiOYouxiAttributes.Editor
{
    /// <summary>
    /// 控制自动生成更新代码
    /// </summary>
    public class CodeGenerator : UnityEditor.Editor
	{
        #region -- Private ReadOnly Data --
        static private readonly string GENERATED_CODE_TARGET_FOLDER =
            (Application.dataPath.Replace("Assets", string.Empty) + AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("CodeGenerator")[0]))
            .Replace("CodeGenerator.cs", string.Empty)
            .Replace("/", "\\");

        static private readonly string CLASS_NAME_PLACEHOLDER = "__classname__";
        static private readonly string ENTRIES_PLACEHOLDER = "__entries__";
        static private readonly string META_ENTRY_FORMAT = "metasByAttributeType[typeof({0})] = new {1}();" + Environment.NewLine + "            ";
        static private readonly string DRAWER_ENTRY_FORMAT = "drawersByAttributeType[typeof({0})] = new {1}();" + Environment.NewLine + "            ";
        static private readonly string GROUPER_ENTRY_FORMAT = "groupersByAttributeType[typeof({0})] = new {1}();" + Environment.NewLine + "            ";
        static private readonly string VALIDATOR_ENTRY_FORMAT = "validatorsByAttributeType[typeof({0})] = new {1}();" + Environment.NewLine + "            ";
        static private readonly string DRAW_CONDITION_ENTRY_FORMAT = "drawConditionsByAttributeType[typeof({0})] = new {1}();" + Environment.NewLine + "            ";

        #endregion

        [MenuItem("OLiOYouxiToolkits/奥利奥特性/更新奥利奥特性数据库")]
        static public void GenerateCode()
        {
            //Properties
            GenerateScript<APropertyMeta, PropertyMetaAttribute>("DPropertyMeta", "APropertyMetaDatabaseTemplate", META_ENTRY_FORMAT);
            GenerateScript<APropertyGrouper, PropertyGrouperAttribute>("DPropertyGrouper", "APropertyGrouperDatabaseTemplate", GROUPER_ENTRY_FORMAT); 
            GenerateScript<APropertyDrawer, PropertyDrawerAttribute>("DPropertyDrawer", "APropertyDrawerDatabaseTemplate", DRAWER_ENTRY_FORMAT);
            GenerateScript<APropertyValidator, PropertyValidatorAttribute>("DPropertyValidator", "APropertyValidatorDatabaseTemplate", VALIDATOR_ENTRY_FORMAT);
            GenerateScript<APropertyDrawCondition, PropertyDrawConditionAttribute>("DPropertyDrawCondition", "APropertyDrawConditionDatabaseTemplate", DRAW_CONDITION_ENTRY_FORMAT);

            //Fields
            GenerateScript<AFieldDrawer, FieldDrawerAttribute>("DFieldDrawer", "AFieldDrawerDatabaseTemplate", DRAWER_ENTRY_FORMAT);
            //Methods
            GenerateScript<AMethodDrawer, MethodDrawerAttribute>("DMethodDrawer", "AMethodDrawerDatabaseTemplate", DRAWER_ENTRY_FORMAT);
            //NativeProperties
            GenerateScript<ANativePropertyDrawer, NativePropertyDrawerAttribute>("DNativePropertiesDrawer", "ANativePropertyDrawerDbTemplate", DRAWER_ENTRY_FORMAT);

            //更新
            AssetDatabase.Refresh();
        }


        #region -- Helper --
        static private void GenerateScript<TClass, TAttribute>(string scriptName, string templateName, string entryFormate) where TAttribute : IAttribute
        {
            //拿到指定模板
            string[] templateAssets = AssetDatabase.FindAssets(templateName);
            if (templateName.Length == 0)
                return;

            string templateGUID = templateAssets[0];
            string templateRelativePath = AssetDatabase.GUIDToAssetPath(templateGUID);
            string templateFormate = (AssetDatabase.LoadAssetAtPath(templateRelativePath, typeof(TextAsset)) as TextAsset).ToString();

            StringBuilder entriesBuider = new StringBuilder();
            //拿到签名类型
            List<Type> subTypes = GetAllSubTypes(typeof(TClass));

            //把所有类型占位符替换
            foreach (Type type in subTypes) 
            {
                IAttribute[] attributes = (IAttribute[])type.GetCustomAttributes(typeof(TAttribute), true);
                if (attributes.Length > 0)
                    entriesBuider.AppendFormat(entryFormate, attributes[0].TargetAttributeType.Name, type.Name);
            }

            //把类占位符替换
            string scriptContent = templateFormate.Replace(CLASS_NAME_PLACEHOLDER, scriptName).Replace(ENTRIES_PLACEHOLDER, entriesBuider.ToString());

            //行尾规范
            scriptContent = Regex.Replace(scriptContent, @"\r\n|\n\r|\r|\n", Environment.NewLine);

            //文件名
            string scriptPath = GENERATED_CODE_TARGET_FOLDER + scriptName + ".cs";

            //把数据写入文件
            IOUtility.WriteToFile(scriptPath, scriptContent);

        }

        static private List<Type> GetAllSubTypes(Type baseClass)
        {
            var result = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assemly in assemblies)
            {
                Type[] types = assemly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsSubclassOf(baseClass))
                    {
                        result.Add(type);
                    }
                }
            }

            return result;
        }
        #endregion
    }
}