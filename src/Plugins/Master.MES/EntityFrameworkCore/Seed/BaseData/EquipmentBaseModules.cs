using System;
using System.Collections.Generic;
using System.Text;
using Master.Module;

namespace Master.EntityFrameworkCore.Seed.BaseData
{
    public class EquipmentBaseModules : BaseSystemModules
    {
        public override List<ColumnInfo> GetColumnInfos()
        {
            var columnInfos = new List<ColumnInfo>();
            //增加工序选择
            var processTypeColumnInfo = new ColumnInfo()
            {
                ColumnKey = "ProcessType",
                ColumnName = "加工工序",
                ColumnType = ColumnTypes.Reference,
                IsInterColumn = true,
                RelativeDataType = RelativeDataType.Url,
                RelativeDataString = "/api/services/app/processType/GetPageResult",
                MaxReferenceNumber = "-1",
                ReferenceSearchColumns = "[{\"field\":\"processTypeName\",\"title\":\"工序\"}]",
                ReferenceItemTpl="processTypeName",
                Sort=0,
                Templet="{{d.processType_display}}"
            };
            columnInfos.Add(processTypeColumnInfo);
            return columnInfos;
        }

        public override List<ModuleButton> GetModuleButtons()
        {
            var buttons = new List<ModuleButton>();

            buttons.Add(new ModuleButton()
            {
                ButtonKey = "Sync",
                ButtonName = "同步",
                ButtonType = ButtonType.ForNoneRow,
                ButtonActionType = ButtonActionType.Ajax,
                ButtonActionUrl = $"abp.services.app.equipment.syncEquipmentFromMES",
                ConfirmMsg= "确认从云平台同步设备信息？",
                IsEnabled=true,
                ButtonClass= "layui-btn-normal"
            });
            buttons.Add(new ModuleButton()
            {
                ButtonKey = "ShowAll",
                ButtonName = "查看所有",
                ButtonActionType = ButtonActionType.Resource,
                IsEnabled = true,
                RequirePermission = false,
                Sort = 6
            });
            return buttons;
        }

        public override void SetButtonsInfosMoreData(ICollection<ModuleButton> ButtonInfos)
        {
            
        }

        public override void SetColumnInfosMoreData(ICollection<ColumnInfo> ColumnInfos)
        {
            
        }
    }
}
