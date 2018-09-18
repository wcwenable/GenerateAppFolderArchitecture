using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileArchitectureWebPro.Models;
using System.IO;

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
                msg = "生成失败：参数非法！";
            }
            if (checkDir(generatedPath))//已存在或成功创建了该文件夹目录
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

        /// <summary>
        /// 检查指定目录是否存在,如不存在则创建
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static bool checkDir(string url)
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
