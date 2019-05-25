using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Service
{
    public class EquipmentOperatorHistoryAppService : ModuleDataAppServiceBase<EquipmentOperatorHistory, int>
    {
        protected override string ModuleKey()
        {
            return nameof(EquipmentOperatorHistory);
        }
    }
}
