using System;
using System.Collections.Generic;
using System.Text;
using Master.Module;

namespace Master.EntityFrameworkCore.Seed.BaseData.SystemModules
{
    public class ChargerBaseModules : BaseSystemModules
    {
        public override List<ModuleButton> GetModuleButtons()
        {
            var moduleButtons = new List<ModuleButton>();
            var FreezeButton = new ModuleButton()
            {
                ButtonKey = "Freeze",
                ButtonName = "冻结",
                ButtonType = ButtonType.ForSingleRow,
                ButtonActionType = ButtonActionType.Ajax,
                ButtonActionUrl = $"abp.services.app.user.freeze",
                ConfirmMsg= "确认冻结此用户？",
                ButtonClass = "layui-danger",
                ClientShowCondition="d.isActive",
                Sort = 1
            };
            moduleButtons.Add(FreezeButton);

            var UnFreezeButton = new ModuleButton()
            {
                ButtonKey = "UnFreeze",
                ButtonName = "解冻",
                ButtonType = ButtonType.ForSingleRow,
                ButtonActionType = ButtonActionType.Ajax,
                ConfirmMsg="确认解冻此用户？",
                ButtonActionUrl = $"abp.services.app.user.unFreeze",
                ButtonClass = "",
                ClientShowCondition = "!d.isActive",
                Sort = 2
            };
            moduleButtons.Add(UnFreezeButton);


            return moduleButtons;
        }
    }
}
