using Abp.Web.Models;
using Master.Dto;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Logs
{
    public class SystemLogAppService : MasterAppServiceBase, ISystemLogAppService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public SystemLogAppService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        //错误文档的地址
        public string erroraddress = "/App_Data/Logs/Error";

        //警告文档的地址
        public string warnaddress = "/App_Data/Logs/Warn";


        protected class File
        {
            public string FilePath;
            public string filename;
        }

        /// <summary>
        /// 得到日志的全部列表
        /// </summary>
        /// <param name="type">文件类型</param>
        /// <returns></returns>
        [DontWrapResult]
        public Task<ResultPageDto> GetLogs(string type)
        {
            var scount = 0;

            var list=new List<File>();
            if (type== "ERROR")
            {
                //获取错误的文档数
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(_hostingEnvironment.ContentRootPath + erroraddress);
                
                if (dir.Exists)
                {
                    scount = dir.GetFiles().Length;
                    foreach(var f in dir.GetFiles())
                    {
                        var temp = new File(){ FilePath = f.FullName, filename = f.Name };
                        list.Add(temp);
                    }
                    list= list.OrderByDescending(a => a.FilePath).ToList();
                }
            } else
            if (type == "WARN")
            {
                //获取警告的文档数
                System.IO.DirectoryInfo dirw = new System.IO.DirectoryInfo(_hostingEnvironment.ContentRootPath + warnaddress);
                
                if (dirw.Exists)
                {
                    scount = dirw.GetFiles().Length;
                    foreach (var f in dirw.GetFiles())
                    {
                        var temp = new File() { FilePath = f.FullName, filename = f.Name };
                        list.Add(temp);
                    }
                    list = list.OrderByDescending(a => a.FilePath).ToList();
                }
            }
            var result = new ResultPageDto()
            {
                code = 0,
                count = scount,
                data = list
            };
            //HttpContext.Response(Newtonsoft.Json.JsonConvert.SerializeObject(result));

            //HttpContextBase.Response.OutputStream.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result), 0, Newtonsoft.Json.JsonConvert.SerializeObject(result).Length);
            //HttpContext.Response.ContentType = "text/plain";
            //HttpContext.Response.End();
            return Task.FromResult(result);
        }

        /// <summary>
        /// 内容附加显示的样式
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Task<List<dynamic>> AddClass(string str, string type)
        {
            //var dstr = "";
            List<dynamic> list = new List<dynamic>();

            var t = str.Split(type);
            for (var i = 1; i < t.Length; i++)
            {

                var temptitle = "";

                //得到对应问题的时间的数据
                var temptitlearry = t[i].Split(' ');
                for (var j = 0; j < temptitlearry.Length; j++)
                {
                    if (temptitlearry[j] != "")
                    {
                        if (temptitle != "")
                        {
                            temptitle = temptitle + " " + temptitlearry[j];
                            break;
                        }
                        else
                        {
                            temptitle = temptitlearry[j];
                        }
                    }
                }
                //得到对应问题的内容
                var tempcontentarry = t[i].Split("> ");

                if (tempcontentarry.Length >= 2)
                {
                    var tempcontent = tempcontentarry[1].Split("\r\n");
                    temptitle = temptitle + "   " + tempcontent[0];
                }
                temptitle = type + "  " + temptitle;
                dynamic td = new ExpandoObject();
                td.title = temptitle;
                td.content = t[i];
                //dynamic td = new dynamic(){ title = temptitle, content = t[i] };
                list.Add(td);

                //dstr = dstr + "<div class=\"layui - colla - item\"><h2 class=\"layui - colla - title\">" + temptitle + "</h2><div class=\"layui - colla - content layui-show\">" + t[i] + "</div ></div >";
            }
            return Task.FromResult(list);
        }

        /// <summary>
        /// 读取日志文件内容
        /// </summary>
        /// <param name="url">路径</param>
        /// <param name="type">错误类型</param>
        /// <returns></returns>
        public  Task<string> GetTipContent(string url, string type)
        {
            FileStream fs = new FileStream(url, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs, Encoding.Default);
            string data = sr.ReadToEnd();
            sr.Close();
            return Task.FromResult(data);
        }
    }
}
