using Master.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.EntityFrameworkCore.Seed.BaseData
{
    public abstract class  BaseSystemModules
    {
        /// <summary>
        /// 获取列的属性
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public virtual List<ColumnInfo> GetColumnInfos()
        {
            return new List<ColumnInfo>();
        }
        /// <summary>
        /// 获取button的属性
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public virtual List<ModuleButton> GetModuleButtons()
        {
            return new List<ModuleButton>();
        }

        /// <summary>
        /// 补齐列的属性
        /// </summary>
        /// <returns></returns>
        public virtual void SetColumnInfosMoreData(ICollection<ColumnInfo> ColumnInfos)
        {

        }

        /// <summary>
        /// 设置模块按钮
        /// </summary>
        /// <param name="ButtonInfos"></param>
        public virtual void SetButtonsInfosMoreData(ICollection<ModuleButton> ButtonInfos)
        {

        }
    }
}
