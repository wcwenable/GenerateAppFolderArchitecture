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
        public IActionResult GenerateFileStructAndFile(string generatedPath, string coreEnglishName)
        {
            var msg = new StringBuilder();
            if (string.IsNullOrWhiteSpace(generatedPath) || string.IsNullOrWhiteSpace(coreEnglishName))
            {
                msg.AppendLine("生成失败：参数非法！");
            }
            if (FolderAndFileHelper.checkDir(generatedPath))//已存在或成功创建了该文件夹目录
            {
                #region 生成DAO相关文件目录结构
                ProcessBusinessLayerRelevantPart(generatedPath, @"\Platform\SCM\SCM.DataAccess", coreEnglishName,"Dao", "DALFactory_", "Dao", msg);
                #endregion

                #region 生成BLL相关文件目录结构
                ProcessBusinessLayerRelevantPart(generatedPath, @"\Platform\SCM\SCM.Service", coreEnglishName, "Business", "DALFactory_", "Business", msg);
                #endregion

                #region 生成Model层相关文件目录结构
                var commonModelDir = @"\Platform\SCM\SCM.Model\Custom";
                ProcessModelLayerRelevantPart(generatedPath, commonModelDir, coreEnglishName, "", "_Result", msg);
                ProcessModelLayerRelevantPart(generatedPath, commonModelDir, coreEnglishName, "", "_Q", msg);
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

        private static void ProcessBusinessLayerRelevantPart(string generatedPath, string layerPath, string coreEnglishName,string dirSuffix,string filePrefix, string fileSuffix, StringBuilder msg)
        {
            var commonDaoDir = layerPath;
            var businessLayerDir = string.Format(@"{0}{1}\{2}", generatedPath, commonDaoDir, dirSuffix);
            var iBusinessLayerDir = string.Format(@"{0}{1}\I{2}", generatedPath, commonDaoDir, dirSuffix);
            var businessLayerFactoryDir = string.Format(@"{0}{1}\Factory", generatedPath, commonDaoDir);

            var strBusinessLayerFile = string.Format("{0}{1}", coreEnglishName, fileSuffix);
            var strBusinessLayerFileName = FolderAndFileHelper.GenerateFile(businessLayerDir, ".cs", strBusinessLayerFile);
            msg.AppendFormat("已创建文件{0}", strBusinessLayerFileName).AppendLine();

            var strIBusinessLayerFile = string.Format("I{0}{1}", coreEnglishName, fileSuffix);
            var strIBusinessLayerFileName = FolderAndFileHelper.GenerateFile(iBusinessLayerDir, ".cs", strIBusinessLayerFile);
            msg.AppendFormat("已创建文件{0}", strIBusinessLayerFileName).AppendLine();

            var strFactoryFile = string.Format("{2}{0}_{1}", coreEnglishName, fileSuffix,filePrefix);
            var strFactoryFileName = FolderAndFileHelper.GenerateFile(businessLayerFactoryDir, ".cs", strFactoryFile);
            msg.AppendFormat("已创建文件{0}", strFactoryFileName).AppendLine();
        }

        private static void ProcessModelLayerRelevantPart(string generatedPath, string modelPath, string coreEnglishName, string prefix, string suffix, StringBuilder msg)
        {
            var strModelRetName = string.Format("{0}{1}{2}", prefix,coreEnglishName,suffix);
            var strModelRetDir = string.Format("{0}{1}", generatedPath, modelPath);
            var strModelRetFileName = FolderAndFileHelper.GenerateFile(strModelRetDir, ".cs", strModelRetName);
            msg.AppendFormat("已创建文件{0}", strModelRetFileName).AppendLine();
        }



        //public IActionResult About()
        //{
        //    ViewData["Message"] = "Your application description page.";

        //    return View();
        //}

        //public IActionResult Contact()
        //{
        //    ViewData["Message"] = "Your contact page.";

        //    return View();
        //}

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
