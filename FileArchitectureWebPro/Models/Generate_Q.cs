using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileArchitectureWebPro.Models
{
    public class Generate_Q
    {

        /// <summary>
        ///项目共用目录
        /// </summary>
        public string projectCommonDir { get; set; }

        /// <summary>
        /// 生成的文件目录结构存放在此目录下
        /// </summary>
        public string generatedDir { get; set; }

        /// <summary>
        /// 业务块核心英文名称
        /// </summary>
        public string coreEnglishName { get; set; }

        /// <summary>
        /// dao类所在目录
        /// </summary>
        public string daoDir { get; set; }

        /// <summary>
        /// bll类所在目录
        /// </summary>
        public string bllDir { get; set; }

        /// <summary>
        /// 模型文件所在目录
        /// </summary>
        public string modelDir { get; set; }



        /// <summary>
        /// 视图文件所在目录
        /// </summary>
        public string controllerDir { get; set; }

        /// <summary>
        /// 视图文件所在目录
        /// </summary>
        public string viewDir { get; set; }

        /// <summary>
        /// html文件所在目录
        /// </summary>
        public string htmlDir { get; set; }

        /// <summary>
        /// js文件所在目录
        /// </summary>
        public string jsDir { get; set; }
    }
}
