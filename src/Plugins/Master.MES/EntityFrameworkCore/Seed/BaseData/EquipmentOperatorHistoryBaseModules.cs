using Master.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.EntityFrameworkCore.Seed.BaseData
{
    public class EquipmentOperatorHistoryBaseModules : BaseSystemModules
    {
        public override void SetButtonsInfosMoreData(ICollection<ModuleButton> ButtonInfos)
        {
            //移除添加、修改、批量修改、删除按钮
            ButtonInfos.Remove(ButtonInfos.Single(o => o.ButtonKey == "Add"));
            ButtonInfos.Remove(ButtonInfos.Single(o => o.ButtonKey == "Edit"));
            ButtonInfos.Remove(ButtonInfos.Single(o => o.ButtonKey == "MultiEdit"));
            ButtonInfos.Remove(ButtonInfos.Single(o => o.ButtonKey == "Delete"));
        }
    }
}
