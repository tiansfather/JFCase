using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.Module
{
    public class ModuleDataManager : ModuleServiceBase<ModuleData, int>, IModuleDataManager
    {


        public override IQueryable<ModuleData> GetFilteredQuery(string moduleKey = "")
        {
            var qry = base.GetFilteredQuery(moduleKey);
            qry = qry.Where(o => o.ModuleInfo.ModuleKey == moduleKey);

            return qry;
        }
    }
}
