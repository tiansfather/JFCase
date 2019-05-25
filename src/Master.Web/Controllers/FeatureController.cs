using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.AspNetCore.Mvc.Authorization;
using Master.Application.Editions;
using Master.Controllers;
using Master.MultiTenancy;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Controllers
{
    [AbpMvcAuthorize]
    public class FeatureController : MasterControllerBase
    {
        public EditionManager EditionManager { get; set; }
        public TenantManager TenantManager { get; set; }
        public async Task<IActionResult> Assign(string type,string data)
        {
            Dictionary<string, object> Values=null;

            if (type == "edition")
            {
                Values = (await EditionManager.GetFeatureValuesAsync(int.Parse(data))).ToDictionary(o=>o.Name,o=>(object)o.Value);
            }else if (type == "tenant")
            {
                Values= (await TenantManager.GetFeatureValuesAsync(int.Parse(data))).ToDictionary(o => o.Name, o => (object)o.Value);
            }

            var features = FeatureManager.GetAll();
            ViewData["Features"] = features;
            ViewData["Values"] = Values;
            return View();
        }
    }
}