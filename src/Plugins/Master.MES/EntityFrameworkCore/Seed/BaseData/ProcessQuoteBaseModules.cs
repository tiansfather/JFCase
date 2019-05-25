using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Master.Module;

namespace Master.EntityFrameworkCore.Seed.BaseData
{
    public class ProcessQuoteBaseModules : BaseSystemModules
    {
        public override List<ModuleButton> GetModuleButtons()
        {
            var buttons = new List<ModuleButton>();
            //增加查看按钮 
            buttons.Add(new ModuleButton()
            {
                ButtonKey = "View",
                ButtonName = "详情",
                ButtonActionType = ButtonActionType.Form,
                ButtonType = ButtonType.ForSingleRow,
                IsEnabled = true,
                RequirePermission=false,
                ButtonActionUrl = "/ProcessQuote/Show",
                ClientShowCondition= "d.quoteStatus!=0",
                ButtonActionParam = "{\"area\": [\"100%\", \"100%\"],\"btn\":[]}",
                Sort = 1
            });

            return buttons;
        }
        public override void SetButtonsInfosMoreData(ICollection<ModuleButton> ButtonInfos)
        {
            //移除批量修改按钮
            var multiEditBtn = ButtonInfos.Where(o => o.ButtonKey == "MultiEdit").SingleOrDefault();
            if (multiEditBtn != null)
            {
                ButtonInfos.Remove(multiEditBtn);
            }
            //更改添加编辑按钮的
            var addBtn = ButtonInfos.Where(o => o.ButtonKey == "Add").Single();
            addBtn.ButtonActionUrl = "/ProcessQuote/Submit";
            addBtn.ButtonActionParam = "{\"area\": [\"100%\", \"100%\"],\"btn\":[\"发布\",\"暂存\",\"关闭\"]}";
            var editBtn = ButtonInfos.Where(o => o.ButtonKey == "Edit").Single();
            editBtn.ButtonActionUrl = "/ProcessQuote/Submit";
            editBtn.ButtonActionParam = "{\"area\": [\"100%\", \"100%\"],\"btn\":[\"发布\",\"暂存\",\"关闭\"]}";
            editBtn.ClientShowCondition = "d.quoteStatus==0";//仅草稿状态可以编辑
        }
    }
}
