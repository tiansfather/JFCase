using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Domain.Entities;
using Master.Module;

namespace Master.EntityFrameworkCore.Seed.BaseData.SystemModules
{
    public class CaseSourceBaseModules : BaseSystemModules    {

        public override List<ModuleButton> GetModuleButtons()
        {
            var moduleButtons = new List<ModuleButton>();
            var FreezeButton = new ModuleButton()
            {
                ButtonKey = "Freeze",
                ButtonName = "下架",
                ButtonType = ButtonType.ForSingleRow|ButtonType.ForSelectedRows,
                ButtonActionType = ButtonActionType.Ajax,
                ButtonActionUrl = $"abp.services.app.caseSource.freeze",
                ConfirmMsg = "确认下架此判例？",
                ButtonClass = "layui-btn-danger",
                ClientShowCondition = "d.caseSourceStatus==0",
                Sort = 1
            };
            moduleButtons.Add(FreezeButton);
            var UnFreezeButton = new ModuleButton()
            {
                ButtonKey = "UnFreeze",
                ButtonName = "上架",
                ButtonType = ButtonType.ForSingleRow | ButtonType.ForSelectedRows,
                ButtonActionType = ButtonActionType.Ajax,
                ConfirmMsg = "确认上架此判例？",
                ButtonActionUrl = $"abp.services.app.caseSource.unFreeze",
                ButtonClass = "",
                ClientShowCondition = "d.caseSourceStatus==-1",
                Sort = 2
            };
            moduleButtons.Add(UnFreezeButton);
            return moduleButtons;
        }
        public override void SetButtonsInfosMoreData(ICollection<ModuleButton> ButtonInfos)
        {
            var createBtn = ButtonInfos.Single(o => o.ButtonKey == "Add");
            ButtonInfos.Remove(createBtn);
            var editBtn= ButtonInfos.Single(o => o.ButtonKey == "Edit");
            editBtn.ButtonActionParam = "{\"area\": [\"100%\",\"100%\"]}";
            //editBtn.ClientShowCondition = "d.caseSourceStatus==-1";//不下架也能进行修改
            editBtn.ButtonActionUrl = "/CaseSource/Add";
            var delBtn = ButtonInfos.Single(o => o.ButtonKey == "Delete");
            delBtn.ClientShowCondition = "d.caseSourceStatus==-1";
        }
        public override void SetColumnInfosMoreData(ICollection<ColumnInfo> ColumnInfos)
        {
            ColumnInfos.Single(o => o.ColumnKey == "SourceSN").SetData("width", "200");
            ColumnInfos.Single(o => o.ColumnKey == "CityId").SetData("width", "80");
            ColumnInfos.Single(o => o.ColumnKey == "Court1Id").SetData("width", "200");
            ColumnInfos.Single(o => o.ColumnKey == "Court2Id").SetData("width", "200");
            ColumnInfos.Single(o => o.ColumnKey == "AnYouId").SetData("width", "200");
            ColumnInfos.Single(o => o.ColumnKey == "ValidDate").SetData("width", "100");
            ColumnInfos.Single(o => o.ColumnKey == "SourceFile").SetData("width", "100");
            ColumnInfos.Single(o => o.ColumnKey == "CaseSourceStatus").SetData("width", "80");
            ColumnInfos.Single(o => o.ColumnKey == "LastModifierUserId").SetData("width", "100");
            ColumnInfos.Single(o => o.ColumnKey == "CreatorUserId").SetData("width", "100");
        }
    }
}
