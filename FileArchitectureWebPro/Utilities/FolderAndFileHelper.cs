using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileArchitectureWebPro.Utilities
{
    public class FolderAndFileHelper
    {
        #region 文件相关的
        public static string GetTempFileName(string extension)
        {
            string tempFileName = Path.GetTempFileName();
            string newTempFileName = Path.ChangeExtension(tempFileName, extension);
            File.Move(tempFileName, newTempFileName);
            return newTempFileName;
        }

        public static string GetTempFileName(string prefix, string extension)
        {
            return GetTempFileName(prefix, extension, null);
        }


        /// <summary>
        /// 生成临时文件
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <param name="extension">文件后缀名，包含前导句点('.')</param>
        /// <param name="directory">指定在该目录下生成，默认用户目录下的临时目录</param>
        /// <returns>临时文件的完整路径</returns>
        public static string GetTempFileName(string prefix, string extension, string directory)
        {
            var fileName = prefix + Guid.NewGuid().ToString();
            if (string.IsNullOrEmpty(directory))
            {
                directory = Path.GetTempPath();
            }
            return GenerateFile(directory, extension, fileName);
        }

        /// <summary>
        /// 用于在指定目录下生成指定文件扩展名和名称的文件
        /// </summary>
        /// <param name="directory">在哪个目录生成文件</param>
        /// <param name="extension">文件扩展名</param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public static string GenerateFile(string directory, string extension,string fileName)
        {
            string fileNameWithExtension = string.Empty;
            if (string.IsNullOrEmpty(directory))
            {
                directory = Path.GetTempPath();
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            fileNameWithExtension = string.Format("{0}{1}",fileName ,extension);
            fileNameWithExtension = Path.Combine(directory, fileNameWithExtension);
            FileStream fs = new FileInfo(fileNameWithExtension).Create();
            fs.Close();
            return fileNameWithExtension;
        }
        #endregion



        #region 文件目录相关
        /// <summary>
        /// 检查指定目录是否存在,如不存在则创建
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public   static bool checkDir(string url)
        {
            try
            {
                if (!Directory.Exists(url))//如果不存在就创建file文件夹　　             　　              
                    Directory.CreateDirectory(url);//创建该文件夹　　            
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        } 
        #endregion
    }
}
