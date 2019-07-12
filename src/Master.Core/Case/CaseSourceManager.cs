using Abp.Domain.Repositories;
using Abp.UI;
using Master.Module;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Master.Case
{
    public class CaseSourceManager:ModuleServiceBase<CaseSource,int>
    {
        public override async Task ValidateEntity(CaseSource entity)
        {
            if (entity.Id > 0 && await Repository.GetAll().CountAsync(o => o.SourceSN == entity.SourceSN &&o.Id != entity.Id) > 0)
            {
                throw new UserFriendlyException(L("案号已存在,请调整后再试"));
            }

            if (entity.Id == 0 && await Repository.GetAll().CountAsync(o => o.SourceSN == entity.SourceSN) > 0)
            {
                throw new UserFriendlyException(L("案号已存在,请调整后再试"));
            }
        }

        /// <summary>
        /// 清除案例所有加工内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task ClearCaseContent(int id)
        {
            var manager = Resolve<CaseInitialManager>();
            var caseInitial = await manager.GetAll().Where(o => o.CaseSourceId == id).FirstOrDefaultAsync();
            if (caseInitial != null)
            {
                await Resolve<IRepository<CaseNode,int>>().DeleteAsync(o => o.CaseInitialId == caseInitial.Id);
                await Resolve<IRepository<CaseLabel, int>>().DeleteAsync(o => o.CaseInitialId == caseInitial.Id);
                await Resolve<CaseCardManager>().Repository.HardDeleteAsync(o => o.CaseInitialId == caseInitial.Id);
                await Resolve<CaseFineManager>().Repository.HardDeleteAsync(o => o.CaseInitialId == caseInitial.Id);
                
                await manager.Repository.HardDeleteAsync(caseInitial);
            }
        }
    }
}
