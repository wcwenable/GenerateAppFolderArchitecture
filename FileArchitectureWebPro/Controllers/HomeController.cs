using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileArchitectureWebPro.Models;
using System.IO;
using FileArchitectureWebPro.Utilities;
using System.Text;

namespace FileArchitectureWebPro.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateFileStructAndFile(Generate_Q query)
        {
            var msg = new StringBuilder();
            if (string.IsNullOrWhiteSpace(query.generatedDir) || string.IsNullOrWhiteSpace(query.coreEnglishName))
            {
                msg.AppendLine("生成失败：参数非法！");
            }
            if (FolderAndFileHelper.checkDir(query.generatedDir))//已存在或成功创建了该文件夹目录
            {
                #region 生成DAO相关文件目录结构
                ProcessBusinessLayerRelevantPart(query.generatedDir, @"\Platform\SCM\SCM.DataAccess", query.coreEnglishName,"Dao", "DALFactory_", "Dao",".cs", msg);
                #endregion

                #region 生成BLL相关文件目录结构
                ProcessBusinessLayerRelevantPart(query.generatedDir, @"\Platform\SCM\SCM.Service", query.coreEnglishName, "Business", "DALFactory_", "Business", ".cs", msg);
                #endregion

                #region 生成Model层相关文件目录结构
                var commonModelDir = @"\Platform\SCM\SCM.Model\Custom";
                ProcessModelLayerRelevantPart(query.generatedDir, commonModelDir, query.coreEnglishName, "", "_Result", ".cs", msg);
                ProcessModelLayerRelevantPart(query.generatedDir, commonModelDir, query.coreEnglishName, "", "_Q", ".cs", msg);
                #endregion

                #region 生成前端MVC相关文件目录结构
                //生成控制器类文件
                ProcessModelLayerRelevantPart(query.generatedDir, @"\Platform\SCM\SCM.Web\Controllers", query.coreEnglishName, "", "Controller", ".cs", msg);

                //生成视图类文件 
                ProcessModelLayerRelevantPart(query.generatedDir, string.Format(@"\Platform\SCM\SCM.Web\Views\{0}", query.coreEnglishName), "Index", "", "", ".cshtml", msg);

                //生成html静态文件
                ProcessModelLayerRelevantPart(query.generatedDir, string.Format(@"\Platform\SCM\SCM.Web\Htmls\{0}", query.coreEnglishName), query.coreEnglishName, "addEdit", "", ".html", msg);

                //生成js静态文件
                ProcessModelLayerRelevantPart(query.generatedDir, string.Format(@"\Platform\SCM\SCM.Web\Scripts\{0}", query.coreEnglishName), query.coreEnglishName, "AddEdit_", "", ".js", msg);
                ProcessModelLayerRelevantPart(query.generatedDir, string.Format(@"\Platform\SCM\SCM.Web\Scripts\{0}", query.coreEnglishName), query.coreEnglishName, "Index_", "", ".js", msg);

                #endregion

                msg.AppendLine("操作成功。");
            }
            else
            {
                msg.AppendLine("文件夹目录非法！");
            }

            ViewBag.Message = msg.ToString();
            return View();
        }

        private static void ProcessBusinessLayerRelevantPart(string generatedPath, string layerPath, string coreEnglishName,string dirSuffix,string filePrefix, string fileSuffix,string extension, StringBuilder msg)
        {
            var commonDaoDir = layerPath;
            var businessLayerDir = string.Format(@"{0}{1}\{2}", generatedPath, commonDaoDir, dirSuffix);
            var iBusinessLayerDir = string.Format(@"{0}{1}\I{2}", generatedPath, commonDaoDir, dirSuffix);
            var businessLayerFactoryDir = string.Format(@"{0}{1}\Factory", generatedPath, commonDaoDir);

            var strBusinessLayerFile = string.Format("{0}{1}", coreEnglishName, fileSuffix);
            var strBusinessLayerFileName = FolderAndFileHelper.GenerateFile(businessLayerDir, extension, strBusinessLayerFile);
            msg.AppendFormat("已创建文件{0}", strBusinessLayerFileName).AppendLine();

            var strIBusinessLayerFile = string.Format("I{0}{1}", coreEnglishName, fileSuffix);
            var strIBusinessLayerFileName = FolderAndFileHelper.GenerateFile(iBusinessLayerDir, extension, strIBusinessLayerFile);
            msg.AppendFormat("已创建文件{0}", strIBusinessLayerFileName).AppendLine();

            var strFactoryFile = string.Format("{2}{0}_{1}", coreEnglishName, fileSuffix,filePrefix);
            var strFactoryFileName = FolderAndFileHelper.GenerateFile(businessLayerFactoryDir, extension, strFactoryFile);
            msg.AppendFormat("已创建文件{0}", strFactoryFileName).AppendLine();
        }

        private static void ProcessModelLayerRelevantPart(string generatedPath, string modelPath, string coreEnglishName, string prefix, string suffix,string extension, StringBuilder msg)
        {
            var strModelRetName = string.Format("{0}{1}{2}", prefix,coreEnglishName,suffix);
            var strModelRetDir = string.Format("{0}{1}", generatedPath, modelPath);
            var strModelRetFileName = FolderAndFileHelper.GenerateFile(strModelRetDir, extension, strModelRetName);
            msg.AppendFormat("已创建文件{0}", strModelRetFileName).AppendLine();
        }
    }
}
