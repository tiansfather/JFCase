using Abp.Authorization;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Case
{
    [AbpAuthorize]
    public class CaseInitialAppService : ModuleDataAppServiceBase<CaseInitial, int>
    {
        protected override string ModuleKey()
        {
            return nameof(CaseInitial);
        }

        /// <summary>
        /// 退回
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task Back(IEnumerable<int> ids)
        {
            var manager = Manager as CaseInitialManager;
            if(await manager.GetAll().CountAsync(o => ids.Contains(o.Id) && o.CaseStatus != CaseStatus.展示中) > 0)
            {
                throw new UserFriendlyException("只有展示中案例可以退回");
            }
            var caseInitials = await Manager.GetAll().Include(o => o.CaseSource)
                .Where(o => ids.Contains(o.Id)).ToListAsync();
            foreach(var caseInitial in caseInitials)
            {
                caseInitial.CaseStatus = CaseStatus.退回;
                caseInitial.CaseSource.CaseSourceStatus = CaseSourceStatus.加工中;
            }

        }

        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task Down(IEnumerable<int> ids)
        {
            var caseInitials = await Manager.GetAll().Include(o => o.CaseSource)
                .Where(o => ids.Contains(o.Id)).ToListAsync();
            foreach (var caseInitial in caseInitials)
            {
                caseInitial.CaseStatus = CaseStatus.下架;
            }

        }
    }
}
