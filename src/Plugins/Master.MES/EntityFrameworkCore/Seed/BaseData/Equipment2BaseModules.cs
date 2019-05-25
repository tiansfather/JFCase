using System;
using System.Collections.Generic;
using System.Text;
using Abp.Reflection.Extensions;
using Master.MES;
using Master.Module;
using Master.Module.Attributes;

namespace Master.EntityFrameworkCore.Seed.BaseData
{
    public class Equipment2BaseModules : BaseSetModules
    {
        public override List<ColumnInfo> GetColumnInfos()
        {
            var columns= new List<ColumnInfo>();
            var property = typeof(Equipment).GetProperty("EquipmentSN");
            columns.Add(property.GetSingleAttributeOrNull<InterColumnAttribute>().BuildColumnInfo(property));
            return columns;
        }

        public override List<ModuleButton> GetModuleButtons()
        {
            return new List<ModuleButton>();
        }

        public override ModuleInfo GetModuleInfo()
        {
            var moduleInfo = new ModuleInfo()
            {
                ModuleKey = GetModulesKey(),
                ModuleName = "设备信息2",
                EntityFullName = typeof(Equipment).FullName,
                IsInterModule = true
            };
            return moduleInfo;
        }


        public override void SetColumnInfosMoreData(ICollection<ColumnInfo> ColumnInfos)
        {
            ;
        }
    }
}
