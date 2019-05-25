using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Web.Components
{
    public class BaseTreeViewComponent : MasterViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(BaseTreeViewParam param)
        {           

            return View(param);
        }
    }

    public class BaseTreeViewParam
    {
        /// <summary>
        /// 树标志，如UnitType
        /// </summary>
        public string TreeKey { get; set; }
        /// <summary>
        /// 树名称
        /// </summary>
        public string TreeName { get; set; }
        public bool EnableAdd { get; set; } = true;
        public bool EnableEdit { get; set; } = true;
        public bool EnableDelete { get; set; } = true;
    }
}
