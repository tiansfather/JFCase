using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Master.Controllers;
using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Controllers
{
    [AbpMvcAuthorize]
    public class BaseTreeController : MasterControllerBase
    {
        public BaseTreeManager BaseTreeManager { get; set; }
        public IActionResult Add(string treeKey,int? parentId)
        {
            ViewBag.TreeKey = treeKey;
            ViewBag.ParentId = parentId;
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            var baseTree = await BaseTreeManager.GetByIdFromCacheAsync(id);
            return View(baseTree);
        }

        public IActionResult Knowledge()
        {
            return View();
        }
        public IActionResult AddKnowledgeRoot()
        {
            return View();
        }
        public IActionResult Types()
        {
            return View();
        }
    }
}