using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileArchitectureWebPro.Models;
using System.IO;
using FileArchitectureWebPro.Utilities;

namespace FileArchitectureWebPro.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateFileStructAndFile(string generatedPath,string coreEnglishName)
        {
            var msg =string.Empty;
            if (string.IsNullOrWhiteSpace(generatedPath) || string.IsNullOrWhiteSpace(coreEnglishName))
            {
                #region 生成DAO相关文件目录结构
                var commonDaoDir = @"\Platform\SCM\SCM.DataAccess";
                var daoDir = string.Format(@"{0}{1}\Dao",generatedPath,commonDaoDir);
                var iDaoDir = string.Format(@"{0}{1}\IDao", generatedPath, commonDaoDir);
                var daoFactoryDir = string.Format(@"{0}{1}\Factory", generatedPath, commonDaoDir); 


                #endregion
                msg = "生成失败：参数非法！";
            }
            if (FolderAndFileHelper.checkDir(generatedPath))//已存在或成功创建了该文件夹目录
            {

                msg = "操作成功。";
            }
            else
            {
                msg = "文件夹目录非法！";
            }

            ViewBag.Message = msg;
            return View();
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
