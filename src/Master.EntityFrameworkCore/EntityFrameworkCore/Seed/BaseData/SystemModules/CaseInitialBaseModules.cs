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
                ButtonActionUrl = $"abp.services.app.case.back",
                ConfirmMsg = "确认退回此案例？",
                ButtonClass = "layui-danger",
                Sort = 1
            };
            moduleButtons.Add(BackButton);
            var DownButton = new ModuleButton()
            {
                ButtonKey = "Down",
                ButtonName = "下架",
                ButtonType = ButtonType.ForSelectedRows,
                ButtonActionType = ButtonActionType.Ajax,
                ButtonActionUrl = $"abp.services.app.case.down",
                ConfirmMsg = "确认下架此案例？",
                ButtonClass = "layui-danger",
                Sort = 1
            };
            moduleButtons.Add(DownButton);
            var RecommandButton = new ModuleButton()
            {
                ButtonKey = "Recommand",
                ButtonName = "推荐",
                ButtonType = ButtonType.ForSelectedRows,
                ButtonActionType = ButtonActionType.Ajax,
                ButtonActionUrl = $"abp.services.app.case.recommand",
                ConfirmMsg = "确认推荐此案例？",
                Sort = 1
            };
            moduleButtons.Add(RecommandButton);
            return moduleButtons;
        }
    }
}
