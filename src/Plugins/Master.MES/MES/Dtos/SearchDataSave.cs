using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 搜索方案
    /// </summary>
    public class SearchDataSave
    {
        /// <summary>
        /// 方案名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 老方案名称
        /// </summary>
        public string OldName { get; set; }

        /// <summary>
        /// 对应搜索模块
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 页面名称
        /// </summary>
        public string PageName { get; set; }

        /// <summary>
        /// 方案数据
        /// </summary>
        public List<SearchData> searchDatas { get; set; }
    }
}
