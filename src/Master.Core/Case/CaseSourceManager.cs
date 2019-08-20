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

            if(!entity.Court1Id.HasValue && !entity.Court2Id.HasValue)
            {
                throw new UserFriendlyException("请至少选择一审法院和二审法院中的一个");
            }
        }

        /// <summary>
        /// 清除案例所有加工内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task ClearCaseContent(int id,bool fromAdmin=false)
        {
            var caseSource = await GetByIdAsync(id);
            //设置案源状态
            caseSource.OwerId = null;
            caseSource.CaseSourceStatus = CaseSourceStatus.待选;
            //清空加工内容
            var manager = Resolve<CaseInitialManager>();
            var caseInitial = await manager.GetAll().Where(o => o.CaseSourceId == id).FirstOrDefaultAsync();
            if (caseInitial != null)
            {
                await Resolve<IRepository<CaseNode,int>>().DeleteAsync(o => o.CaseInitialId == caseInitial.Id);
                await Resolve<IRepository<CaseLabel, int>>().DeleteAsync(o => o.CaseInitialId == caseInitial.Id);
                await Resolve<CaseCardManager>().Repository.DeleteAsync(o => o.CaseInitialId == caseInitial.Id);
                await Resolve<CaseFineManager>().Repository.DeleteAsync(o => o.CaseInitialId == caseInitial.Id);
                
                await manager.Repository.DeleteAsync(caseInitial);

                if (fromAdmin)
                {
                    //管理员操作的记录放回日志
                    //增加判例记录
                    var caseHistory = new CaseSourceHistory()
                    {
                        CaseSourceId = id,
                        Reason = "管理员释放",
                        CreatorUserId =caseInitial.CreatorUserId
                    };
                    await Resolve<CaseSourceHistoryManager>().InsertAsync(caseHistory);
                }
            }
        }
        /// <summary>
        /// 清空用户所有加工内容
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task ClearCaseContentByUserId(int userId)
        {
            var caseSources = await GetAll().Where(o => o.OwerId == userId).ToListAsync();
            foreach(var caseSource in caseSources)
            {
                await ClearCaseContent(caseSource.Id,true);
            }
        }
    }
}
