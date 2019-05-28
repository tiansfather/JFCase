using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Master.Module;

namespace Master.EntityFrameworkCore.Seed.BaseData.SystemModules
{
    public class NewMinerBaseModules : BaseSystemModules
    {
        public override List<ModuleButton> GetModuleButtons()
        {
            var moduleButtons = new List<ModuleButton>();
            var VerifyButton = new ModuleButton()
            {
                ButtonKey = "Verify",
                ButtonName = "审核",
                ButtonType = ButtonType.ForSelectedRows,
                ButtonActionType = ButtonActionType.Ajax,
                ButtonActionUrl = $"abp.services.app.newMiner.verify",
                ConfirmMsg= "审核通过后，这些用户将可以通过微信账户登录案例工厂，确定继续吗？",
                ButtonClass = "layui-danger",
                Sort = 1
            };
            moduleButtons.Add(VerifyButton);

            return moduleButtons;
        }
        public override void SetColumnInfosMoreData(ICollection<ColumnInfo> ColumnInfos)
        {
            var toDelColumnKeys = new string[] { "CreatorUserId", "LastModifierUserId", "LastModificationTime" };
            foreach(var columnKey in toDelColumnKeys)
            {
                var column = ColumnInfos.Single(o => o.ColumnKey == columnKey);
                ColumnInfos.Remove(column);
            }
        }
        public override void SetButtonsInfosMoreData(ICollection<ModuleButton> ButtonInfos)
        {
            var createBtn = ButtonInfos.Single(o => o.ButtonKey == "Add");
            ButtonInfos.Remove(createBtn);
            var editBtn= ButtonInfos.Single(o => o.ButtonKey == "Edit");
            ButtonInfos.Remove(editBtn);
            var delBtn = ButtonInfos.Single(o => o.ButtonKey == "Delete");
            delBtn.ButtonName = "拒绝";
            delBtn.ConfirmMsg = "您确定拒绝这些用户的申请吗？";
        }
    }
}
