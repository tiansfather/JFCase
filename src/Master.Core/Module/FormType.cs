using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Module
{
    public enum FormType
    {
        /// <summary>
        /// 添加
        /// </summary>
        Add = 1,
        /// <summary>
        /// 编辑
        /// </summary>
        Edit = 2,
        /// <summary>
        /// 查看页
        /// </summary>
        View = 3,
        /// <summary>
        /// 批量修改页
        /// </summary>
        MultiEdit = 4,
        /// <summary>
        /// 列表页
        /// </summary>
        List = 5,
        /// <summary>
        /// 搜索页
        /// </summary>
        Search = 6
    }
}
