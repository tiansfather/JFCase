using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Domain.Entities;
using Master.Module;

namespace Master.EntityFrameworkCore.Seed.BaseData
{
    public class UserBaseModules : BaseSystemModules
    {
        public override List<ColumnInfo> GetColumnInfos()
        {
            var columns= new List<ColumnInfo>();

            return columns;
        }

        public override List<ModuleButton> GetModuleButtons()
        {
            return new List<ModuleButton>();
        }

        public override void SetButtonsInfosMoreData(ICollection<ModuleButton> ButtonInfos)
        {
            
        }

        public override void SetColumnInfosMoreData(ICollection<ColumnInfo> ColumnInfos)
        {
            //移除民族列和婚姻状态列
            var column_nation = ColumnInfos.SingleOrDefault(o => o.ColumnKey == "Nation");
            if (column_nation != null)
            {
                ColumnInfos.Remove(column_nation);
            }
            var column_marriage = ColumnInfos.SingleOrDefault(o => o.ColumnKey == "Marriage");
            if (column_nation != null)
            {
                ColumnInfos.Remove(column_marriage);
            }
            //设置操作列宽度
            var column_operation = ColumnInfos.SingleOrDefault(o => o.ColumnKey == "Operation");
            if (column_operation != null)
            {
                column_operation.SetData("width", "240");
            }
        }
    }
}
