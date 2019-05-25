using Abp.Runtime.Session;
using Master.Entity;
using Master.Module;
using Master.MultiTenancy;
using Master.Units;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Configuration
{
    public class MESRelativeDataProvider : IRelativeDataProvider<Unit>
    {
        public UnitManager UnitManager { get; set; }
        public TenantManager TenantManager { get; set; }
        public IAbpSession AbpSession { get; set; }
        public async Task FillRelativeData(ModuleDataContext context)
        {
            var entity = context.Entity;
            var unit = await UnitManager.GetByIdFromCacheAsync(int.Parse(entity["Id"].ToString()));
            var tenantId = unit.GetPropertyValue<int?>("TenantId");
            if (tenantId != null)
            {
                var tenant = await TenantManager.GetByIdFromCacheAsync(tenantId.Value);
                context.Entity.Add("TenantId", tenantId.Value);
                context.Entity.Add("TenancyName", tenant.TenancyName);
            }
            else
            {
                context.Entity.Add("Inviter", AbpSession.TenantId.Value);
            }
        }
    }
}
