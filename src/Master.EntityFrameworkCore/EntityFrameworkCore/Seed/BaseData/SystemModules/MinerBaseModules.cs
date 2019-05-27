using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Master.Module;

namespace Master.EntityFrameworkCore.Seed.BaseData.SystemModules
{
    public class MinerBaseModules : BaseSystemModules
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
                ConfirmMsg= "矿工冻结后，将不能再从判例库添加新的判例至工作台，已添加的判例可保留，确定继续吗？",
                ButtonClass = "layui-danger",
                ClientShowCondition="d.isActive",
                Sort = 1
            };
            moduleButtons.Add(FreezeButton);

            var UnFreezeButton = new ModuleButton()
            {
                ButtonKey = "UnFreeze",
                ButtonName = "解冻",
                ButtonType = ButtonType.ForSelectedRows,
                ButtonActionType = ButtonActionType.Ajax,
                ConfirmMsg="确认解冻此用户？",
                ButtonActionUrl = $"abp.services.app.user.unFreeze",
                ButtonClass = "",
                ClientShowCondition = "!d.isActive",
                Sort = 2
            };
            moduleButtons.Add(UnFreezeButton);

            var RevertButton = new ModuleButton()
            {
                ButtonKey = "Revert",
                ButtonName = "恢复",
                ButtonType = ButtonType.ForSelectedRows,
                ButtonActionType = ButtonActionType.Ajax,
                ConfirmMsg = "确认恢复此用户？",
                ButtonActionUrl = $"abp.services.app.user.revert",
                ButtonClass = "",
                ClientShowCondition = "d.isDelete",
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
            ButtonInfos.Remove(editBtn);
            var deleteBtn = ButtonInfos.Single(o => o.ButtonKey == "Delete");
            deleteBtn.ButtonName = "注销";
            deleteBtn.ConfirmMsg = "矿工注销后，将不能再登陆平台，但他的数据将会被保留，确定继续吗？";
            deleteBtn.ClientShowCondition = "!d.isDelete";
        }
    }
}
