using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 搜索的数据
    /// </summary>
    public class SearchData
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 筛选关键字
        /// </summary>
        public string Keys { get; set; }

        /// <summary>
        /// 查看的表格
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 搜索的类型 search 搜索指定值|  like 模糊查找 | date 时间 | array 指定值
        /// </summary>
        public string SearchType { get; set; }

        /// <summary>
        /// 指定值数据
        /// </summary>
        public List<string> ArrayData { get; set; }

        /// <summary>
        /// 搜索的值是否是两个
        /// </summary>
        public bool CanAnd { get; set; } = false;

        /// <summary>
        /// 搜索保存的值
        /// </summary>
        public string Data { get; set; }

        #region  搜索的值
        /// <summary>
        /// 按搜索指定值
        /// </summary>
        public static string Search = "Search";

        /// <summary>
        /// 按模糊查找
        /// </summary>
        public static string Like = "Like";

        /// <summary>
        /// 按时间类型
        /// </summary>
        public static string Date = "Date";

        /// <summary>
        /// 按数组方式
        /// </summary>
        public static string Array = "Array";

        /// <summary>
        /// 复选
        /// </summary>
        public static string Check = "Check";

        #endregion
    }
}
