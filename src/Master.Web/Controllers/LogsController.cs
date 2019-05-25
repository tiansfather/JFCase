using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Master.Controllers;
using Master.Logs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Controllers
{
    [AbpMvcAuthorize]
    public class LogsController : MasterControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ISystemLogAppService _systemLogAppService;

        public LogsController(IHostingEnvironment hostingEnvironment, ISystemLogAppService systemLogAppService)
        {
            _hostingEnvironment = hostingEnvironment;
            _systemLogAppService = systemLogAppService;
        }

        //错误文档的地址
        public string erroraddress = "/App_Data/Logs/Error";

        //警告文档的地址
        public string warnaddress = "/App_Data/Logs/Warn";
        public IActionResult System()
        {


            //获取错误的文档数
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(_hostingEnvironment.ContentRootPath + erroraddress);

            var errorcount = 0;
            if (dir.Exists)
            {
                errorcount = dir.GetFiles().Length;
            }

            //获取警告的文档数
            System.IO.DirectoryInfo dirw = new System.IO.DirectoryInfo(_hostingEnvironment.ContentRootPath + warnaddress);

            var warncount = 0;
            if (dirw.Exists)
            {
                warncount = dirw.GetFiles().Length;
            }


            ViewData["errorCount"] = errorcount;
            ViewData["warnCount"] = warncount;
            return View();
        }


        public async Task<ActionResult> Tip(string url, string type)
        {
            //FileStream fs = new FileStream(url, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //StreamReader sr = new StreamReader(fs, Encoding.Default);
            //var data = sr.ReadToEnd();
            //sr.Close();

            string data = await _systemLogAppService.GetTipContent(url, type);

            ViewData["Data"] = await _systemLogAppService.AddClass(data, type);
            ViewData["type"] = type;
            return View();
        }



        /// <summary>
        /// 操作日志
        /// </summary>
        /// <returns></returns>
        public IActionResult Operations()
        {
            return View();
        }
        /// <summary>
        /// 登录日志
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            return View();
        }
    }
}