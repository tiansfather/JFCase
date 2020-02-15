using Master.Module;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Authentication
{
    public class NewMinerManager:ModuleServiceBase<NewMiner, int>
    {
        public override async Task FillEntityDataAfter(IDictionary<string, object> data, ModuleInfo moduleInfo, object entity)
        {
            data["IsDeleted"] = (entity as NewMiner).IsDeleted;
        }
    }
}
