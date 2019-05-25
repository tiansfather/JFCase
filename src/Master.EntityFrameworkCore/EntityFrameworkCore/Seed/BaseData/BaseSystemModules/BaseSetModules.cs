using Master.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.EntityFrameworkCore.Seed.BaseData
{
    public abstract class BaseSetModules:BaseSystemModules
    {
        /// <summary>
        /// 获取模块信息
        /// </summary>
        /// <returns></returns>
        public abstract ModuleInfo GetModuleInfo();

        ///// <summary>
        ///// 获取列的属性
        ///// </summary>
        ///// <param name="Type"></param>
        ///// <returns></returns>
        //public abstract List<SkyNet.Master.Model.ColumnInfo> GetColumnInfos();
        ///// <summary>
        ///// 获取button的属性
        ///// </summary>
        ///// <param name="Type"></param>
        ///// <returns></returns>
        //public abstract List<SkyNet.Master.Model.ModuleButton> GetModuleButtons();

        /// <summary>
        /// 得到模块Key
        /// </summary>
        public virtual string GetModulesKey()
        {
            return this.GetType().Name.Replace("BaseModules", "");
        }
    }
}
