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
                query.projectCommonDir = string.IsNullOrWhiteSpace(query.projectCommonDir) ? @"\Platform\SCM" : query.projectCommonDir;
                #region 生成DAO相关文件目录结构
                query.daoDir = string.IsNullOrWhiteSpace(query.daoDir) ? @"\SCM.DataAccess" : query.daoDir;
                query.daoDir = GetSpecifiedDir(query.projectCommonDir,query.daoDir);
                ProcessBusinessLayerRelevantPart(query.generatedDir, query.daoDir, query.coreEnglishName,"Dao", "DALFactory_", "Dao",".cs", msg);
                #endregion

                #region 生成BLL相关文件目录结构
                query.bllDir = string.IsNullOrWhiteSpace(query.bllDir) ? @"\SCM.Service" : query.bllDir;
                query.bllDir = GetSpecifiedDir(query.projectCommonDir, query.bllDir);
                ProcessBusinessLayerRelevantPart(query.generatedDir, query.bllDir, query.coreEnglishName, "Business", "DALFactory_", "Business", ".cs", msg);
                #endregion

                #region 生成Model层相关文件目录结构
                query.modelDir = string.IsNullOrWhiteSpace(query.modelDir) ? @"\SCM.Model\Custom" : query.modelDir;
                query.modelDir = GetSpecifiedDir(query.projectCommonDir, query.modelDir);
                ProcessModelLayerRelevantPart(query.generatedDir, query.modelDir, query.coreEnglishName, "", "_Result", ".cs", msg);
                ProcessModelLayerRelevantPart(query.generatedDir, query.modelDir, query.coreEnglishName, "", "_Q", ".cs", msg);
                #endregion

                #region 生成前端MVC相关文件目录结构
                //生成控制器类文件
                query.controllerDir = string.IsNullOrWhiteSpace(query.controllerDir) ? @"\SCM.Web\Controllers" : query.controllerDir;
                query.controllerDir = GetSpecifiedDir(query.projectCommonDir, query.controllerDir);
                ProcessModelLayerRelevantPart(query.generatedDir, query.controllerDir, query.coreEnglishName, "", "Controller", ".cs", msg);

                //生成视图类文件 
                query.viewDir = string.IsNullOrWhiteSpace(query.viewDir) ? @"\SCM.Web\Views" : query.viewDir;
                query.viewDir = GetSpecifiedDir(query.projectCommonDir, query.viewDir);
                ProcessModelLayerRelevantPart(query.generatedDir, string.Format(@"{1}\{0}", query.coreEnglishName,query.viewDir), "Index", "", "", ".cshtml", msg);

                //生成html静态文件
                query.htmlDir = string.IsNullOrWhiteSpace(query.htmlDir) ? @"\SCM.Web\Htmls" : query.htmlDir;
                query.htmlDir = GetSpecifiedDir(query.projectCommonDir, query.htmlDir);
                ProcessModelLayerRelevantPart(query.generatedDir, string.Format(@"{1}\{0}", query.coreEnglishName,query.htmlDir), query.coreEnglishName, "addEdit", "", ".html", msg);

                //生成js静态文件
                query.jsDir = string.IsNullOrWhiteSpace(query.jsDir) ? @"\SCM.Web\Scripts" : query.jsDir;
                query.jsDir = GetSpecifiedDir(query.projectCommonDir, query.jsDir);
                ProcessModelLayerRelevantPart(query.generatedDir, string.Format(@"{1}\{0}", query.coreEnglishName,query.jsDir), query.coreEnglishName, "AddEdit_", "", ".js", msg);
                ProcessModelLayerRelevantPart(query.generatedDir, string.Format(@"{1}\{0}", query.coreEnglishName, query.jsDir), query.coreEnglishName, "Index_", "", ".js", msg);

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

        private static string GetSpecifiedDir(string projectCommonDir, string specifiedDir)
        {
            return string.Format(@"{0}{1}", projectCommonDir, specifiedDir).Replace(@"\\", @"\");
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
