using Abp.UI;
using Master.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Entity
{
    public class BaseTreeManager : DomainServiceBase<BaseTree, int>
    {
        /// <summary>
        /// 通过树类型和节点名称获取树节点
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="discriminator"></param>
        /// <returns></returns>
        public virtual async Task<BaseTree> GetByName(string displayName,string discriminator)
        {
            return await GetAll().Where(o => o.Discriminator == discriminator && o.DisplayName == displayName)
                .FirstOrDefaultAsync();
        }
        /// <summary>
        /// 新建树节点
        /// </summary>
        /// <param name="BaseTree"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(BaseTree BaseTree)
        {
            BaseTree.TenantId = AbpSession.TenantId;
            BaseTree.Code = await GetNextChildCodeAsync(BaseTree.ParentId,BaseTree.Discriminator);
            await ValidateBaseTreeAsync(BaseTree);
            await Repository.InsertAsync(BaseTree);
        }

        public override async Task UpdateAsync(BaseTree BaseTree)
        {
            await ValidateBaseTreeAsync(BaseTree);
            await Repository.UpdateAsync(BaseTree);
        }

        public virtual async Task<string> GetNextChildCodeAsync(int? parentId,string discriminator)
        {
            var lastChild = await GetLastChildOrNullAsync(parentId,discriminator);
            if (lastChild == null)
            {
                var parentCode = parentId != null ? await GetCodeAsync(parentId.Value) : null;
                return BaseTree.AppendCode(parentCode, BaseTree.CreateCode(1));
            }

            return BaseTree.CalculateNextCode(lastChild.Code);
        }

        public virtual async Task<BaseTree> GetLastChildOrNullAsync(int? parentId,string discriminator)
        {
            var children = await Repository.GetAllListAsync(ou => ou.ParentId == parentId && ou.Discriminator==discriminator);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }

        public virtual async Task<string> GetCodeAsync(int id)
        {
            return (await Repository.GetAsync(id)).Code;
        }

        public override async Task DeleteAsync(IEnumerable<int> ids)
        {
            var ous = await GetListByIdsAsync(ids);
            foreach (var ou in ous)
            {
                await DeleteAsync(ou);
            }
        }

        public override async Task DeleteAsync(BaseTree entity)
        {
            var children = await FindChildrenAsync(entity.Id,entity.Discriminator, true);

            foreach (var child in children)
            {
                await Repository.DeleteAsync(child);
            }

            await Repository.DeleteAsync(entity.Id);
        }

        public virtual async Task MoveAsync(int id, int? parentId)
        {
            var BaseTree = await Repository.GetAsync(id);
            if (BaseTree.ParentId == parentId)
            {
                return;
            }

            //Should find children before Code change
            var children = await FindChildrenAsync(id,BaseTree.Discriminator, true);

            //Store old code of OU
            var oldCode = BaseTree.Code;

            //Move OU
            BaseTree.Code = await GetNextChildCodeAsync(parentId,BaseTree.Discriminator);
            BaseTree.ParentId = parentId;

            await ValidateBaseTreeAsync(BaseTree);

            //Update Children Codes
            foreach (var child in children)
            {
                child.Code = BaseTree.AppendCode(BaseTree.Code, BaseTree.GetRelativeCode(child.Code, oldCode));
            }
        }

        public async Task<List<BaseTree>> FindChildrenAsync(int? parentId,string discriminator, bool recursive = false)
        {
            if (!recursive)
            {
                return await Repository.GetAll().IgnoreQueryFilters().Where(ou =>!ou.IsDeleted && ou.ParentId == parentId && ou.Discriminator==discriminator && ou.TenantId==AbpSession.TenantId).OrderBy(ou => ou.Sort).ToListAsync();
            }

            if (!parentId.HasValue)
            {
                return await Repository.GetAll().IgnoreQueryFilters().Where(o=>!o.IsDeleted && o.Discriminator==discriminator && o.TenantId == AbpSession.TenantId).OrderBy(ou => ou.Sort).ToListAsync();
            }

            var code = await GetCodeAsync(parentId.Value);

            return await Repository.GetAll().Where(
                ou =>ou.Discriminator==discriminator && ou.Code.StartsWith(code) && ou.Id != parentId.Value
            ).OrderBy(ou => ou.Sort).ToListAsync();
        }

        protected virtual async Task ValidateBaseTreeAsync(BaseTree BaseTree)
        {
            //if (BaseTree.Id == 0 && (await Repository.GetAll().IgnoreQueryFilters().Where(o => !o.IsDeleted && o.Discriminator == BaseTree.Discriminator && o.BriefCode == BaseTree.BriefCode && o.TenantId == AbpSession.TenantId).CountAsync()) > 0)
            //{
            //    throw new UserFriendlyException("编码重复");
            //}
            //if (BaseTree.Id > 0 && (await Repository.GetAll().IgnoreQueryFilters().Where(o => !o.IsDeleted && o.Discriminator == BaseTree.Discriminator && o.Id!=BaseTree.Id && o.BriefCode == BaseTree.BriefCode && o.TenantId == AbpSession.TenantId).CountAsync()) > 0)
            //{
            //    throw new UserFriendlyException("编码重复");
            //}
            var siblings = (await FindChildrenAsync(BaseTree.ParentId,BaseTree.Discriminator))
                .Where(ou => ou.Id != BaseTree.Id)
                .ToList();

            if (siblings.Any(ou => ou.DisplayName == BaseTree.DisplayName))
            {
                throw new UserFriendlyException("名称重复");
            }
        }
    }
}
