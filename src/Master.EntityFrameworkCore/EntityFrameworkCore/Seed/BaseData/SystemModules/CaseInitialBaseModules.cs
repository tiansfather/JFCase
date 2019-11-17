using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Master.Module;
using Abp.Domain.Entities;

namespace Master.EntityFrameworkCore.Seed.BaseData.SystemModules
{
    public class CaseInitialBaseModules : BaseSystemModules    {


        public override List<ModuleButton> GetModuleButtons()
        {
            var moduleButtons = new List<ModuleButton>();
            var BackButton = new ModuleButton()
            {
                ButtonKey = "Back",
                ButtonName = "退回重做",
                ButtonType = ButtonType.ForSelectedRows,
                ButtonActionType = ButtonActionType.Ajax,
                ButtonActionUrl = $"abp.services.app.caseInitial.back",
                ConfirmMsg = "确认退回此案例？",
                ButtonClass = "layui-btn-danger",
                Sort = 1
            };
            moduleButtons.Add(BackButton);
            var DownButton = new ModuleButton()
            {
                ButtonKey = "Down",
                ButtonName = "下架",
                ButtonType = ButtonType.ForSelectedRows,
                ButtonActionType = ButtonActionType.Ajax,
                ButtonActionUrl = $"abp.services.app.caseInitial.down",
                ConfirmMsg = "确认下架这些案例？",
                ButtonClass = "layui-danger",
                Sort = 2
            };
            moduleButtons.Add(DownButton);
            var UpButton = new ModuleButton()
            {
                ButtonKey = "Up",
                ButtonName = "上架",
                ButtonType = ButtonType.ForSelectedRows,
                ButtonActionType = ButtonActionType.Ajax,
                ButtonActionUrl = $"abp.services.app.caseInitial.up",
                ConfirmMsg = "确认上架这些案例？",
                Sort = 3
            };
            moduleButtons.Add(UpButton);
            var ClearButton = new ModuleButton()
            {
                ButtonKey = "Clear",
                ButtonName = "消除成品",
                ButtonType = ButtonType.ForSelectedRows,
                ButtonActionType = ButtonActionType.Ajax,
                ButtonActionUrl = $"abp.services.app.caseInitial.clearContent",
                ButtonClass = "layui-btn-danger",
                ConfirmMsg = "确认清除这些加工案例？判例将回归判例库",
                Sort = 4
            };
            moduleButtons.Add(ClearButton);
            var RecommandButton = new ModuleButton()
            {
                ButtonKey = "Recommand",
                ButtonName = "推荐",
                ButtonType = ButtonType.ForSelectedRows,
                ButtonActionType = ButtonActionType.Ajax,
                ButtonActionUrl = $"abp.services.app.caseInitial.recommand",
                ConfirmMsg = "确认推荐这些案例？",
                Sort = 5
            };
            moduleButtons.Add(RecommandButton);
            var UnRecommandButton = new ModuleButton()
            {
                ButtonKey = "UnRecommand",
                ButtonName = "取消推荐",
                ButtonClass = "layui-btn-danger",
                ButtonType = ButtonType.ForSelectedRows,
                ButtonActionType = ButtonActionType.Ajax,
                ButtonActionUrl = $"abp.services.app.caseInitial.unRecommand",
                ConfirmMsg = "确认取消推荐这些案例？",
                Sort = 6
            };
            moduleButtons.Add(UnRecommandButton);
            return moduleButtons;
        }

        public override void SetColumnInfosMoreData(ICollection<ColumnInfo> ColumnInfos)
        {
            ColumnInfos.Single(o => o.ColumnKey == "SourceSN").SetData("width", "200");
            ColumnInfos.Single(o => o.ColumnKey == "City").SetData("width", "80");
            ColumnInfos.Single(o => o.ColumnKey == "AnYou").SetData("width", "180");
            ColumnInfos.Single(o => o.ColumnKey == "Title").SetData("width", "400");
            ColumnInfos.Single(o => o.ColumnKey == "Processor").SetData("width", "80");
            ColumnInfos.Single(o => o.ColumnKey == "PublishDate").SetData("width", "100");
            ColumnInfos.Single(o => o.ColumnKey == "PraiseNumber").SetData("width", "80");
            ColumnInfos.Single(o => o.ColumnKey == "BeatNumber").SetData("width", "80");
            ColumnInfos.Single(o => o.ColumnKey == "CaseStatus").SetData("width", "80");
            ColumnInfos.Single(o => o.ColumnKey == "IsActive").SetData("width", "80");
        }
    }
}
