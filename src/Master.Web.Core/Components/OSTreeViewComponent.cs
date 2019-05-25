using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Web.Components
{
    public class OSTreeViewComponent : MasterViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(OSTreeViewParam param)
        {

            return View(param);
        }
    }
    public class OSTreeViewParam
    {
        /// <summary>
        /// 树标志，如UnitType
        /// </summary>
        public string TreeKey { get; set; }
        /// <summary>
        /// 树名称
        /// </summary>
        public string TreeName { get; set; }
        /// <summary>
        /// 树内容
        /// </summary>
        public List<string> TreeDate { get; set; }
    }
}
