using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Master.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Controllers
{
    [AbpMvcAuthorize]
    public class PermissionController : MasterControllerBase
    {
        /// <summary>
        /// 权限选择
        /// </summary>
        /// <param name="type"></param>
        /// <param name="moduleKey"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public IActionResult Assign(string type, string moduleKey, string data)
        {
            return View();
        }
    }
}