using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Master.Module;

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
                ConfirmMsg = "确认推荐此案例？",
                Sort = 5
            };
            moduleButtons.Add(RecommandButton);
            return moduleButtons;
        }
    }
}
