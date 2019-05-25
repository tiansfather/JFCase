using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Master.Configuration;
using Master.Configuration.Dictionaries;
using Master.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Controllers
{
    [AbpMvcAuthorize]
    public class DictionaryController : MasterControllerBase
    {
        private IDictionaryManager _dictionaryManager;
        public DictionaryController(IDictionaryManager dictionaryManager)
        {
            _dictionaryManager = dictionaryManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(string data)
        {
            var dic = await _dictionaryManager.GetUserDicByNameAsync(data);
            if (dic == null)
            {
                return Error(L("未找到可编辑字典,内置字典不可编辑"));
            }
            return View(dic);
        }
    }
}