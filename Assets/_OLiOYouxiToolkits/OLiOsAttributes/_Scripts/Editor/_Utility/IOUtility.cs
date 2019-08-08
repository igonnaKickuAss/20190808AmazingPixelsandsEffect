using System.IO;
using System.Text;
using UnityEngine;

namespace OLiOYouxiAttributes.Editor
{
	static public class IOUtility
	{
        #region -- Public APIMethods --
        /// <summary>
        /// 返回文件路径
        /// </summary>
        /// <returns></returns>
        static public string GetPersistentDataPath()
        {
            return Application.persistentDataPath + "/";
        }

        /// <summary>
        /// 进行数据写入文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        static public void WriteToFile(string filePath, string content)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, EncodingUtility.UTF8))    //参考： https://www.cnblogs.com/criedshy/archive/2012/08/07/2625358.html
                {
                    streamWriter.WriteLine(content);
                }
            }
        }

        /// <summary>
        /// 返回读取的文件数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        static public string ReadToFile(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read)) 
            {
                using (StreamReader streamReader = new StreamReader(fileStream, EncodingUtility.UTF8)) 
                {
                    string content = streamReader.ReadToEnd();
                    return content;
                }
            }
        }

        /// <summary>
        /// 返回文件在项目里的相对路径（..\\Assets\..\..）
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        static public string GetPathRelativeToProjectFolder(string fullPath)
        {
            int indexOfAssetsWord = fullPath.IndexOf("\\Assets");
            string relativePath = fullPath.Substring(indexOfAssetsWord + 1);
            return relativePath;
        }

        #endregion

    }
}