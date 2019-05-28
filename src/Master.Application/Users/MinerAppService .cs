using Abp.Authorization;
using Master.Authentication;
using Master.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Users
{
    [AbpAuthorize]
    public class MinerAppService : ModuleDataAppServiceBase<User, long>
    {
        protected override string ModuleKey()
        {
            return nameof(Miner);
        }
        protected override async Task<IQueryable<User>> BuildKeywordQueryAsync(string keyword, IQueryable<User> query)
        {
            return (await base.BuildKeywordQueryAsync(keyword, query))
                .Where(o => o.Name.Contains(keyword));
        }
        public virtual async Task<object> GetSummary()
        {
            var manager = Manager as UserManager;
            var query = manager.GetFilteredQuery(ModuleKey());
            var normalCount =await query.Where(o => o.IsActive && !o.IsDeleted).CountAsync();
            var freezeCount = await query.Where(o => !o.IsActive && !o.IsDeleted).CountAsync();
            var deleteCount= await query.Where(o => o.IsDeleted).CountAsync();

            return new
            {
                normalCount,
                freezeCount,
                deleteCount
            };
        }
    }
}
