using Abp.UI;
using Master.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Organizations
{
    public class OrganizationManager : DomainServiceBase<Organization,int>
    {

        public virtual async Task CreateAsync(Organization Organization)
        {
            Organization.Code = await GetNextChildCodeAsync(Organization.ParentId);
            await ValidateOrganizationAsync(Organization);
            await Repository.InsertAsync(Organization);
        }

        public override async Task UpdateAsync(Organization Organization)
        {
            await ValidateOrganizationAsync(Organization);
            await Repository.UpdateAsync(Organization);
        }

        public virtual async Task<string> GetNextChildCodeAsync(int? parentId)
        {
            var lastChild = await GetLastChildOrNullAsync(parentId);
            if (lastChild == null)
            {
                var parentCode = parentId != null ? await GetCodeAsync(parentId.Value) : null;
                return Organization.AppendCode(parentCode, Organization.CreateCode(1));
            }

            return Organization.CalculateNextCode(lastChild.Code);
        }

        public virtual async Task<Organization> GetLastChildOrNullAsync(int? parentId)
        {
            var children = await Repository.GetAllListAsync(ou => ou.ParentId == parentId);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }

        public virtual async Task<string> GetCodeAsync(int id)
        {
            return (await Repository.GetAsync(id)).Code;
        }

        public override async Task DeleteAsync(IEnumerable<int> ids)
        {
            var ous =await  GetListByIdsAsync(ids);
            foreach(var ou in ous)
            {
                await DeleteAsync(ou);
            }
        }

        public override async Task DeleteAsync(Organization entity)
        {
            var children = await FindChildrenAsync(entity.Id, true);

            foreach (var child in children)
            {
                await Repository.DeleteAsync(child);
            }

            await Repository.DeleteAsync(entity.Id);
        }

        public virtual async Task MoveAsync(int id, int? parentId)
        {
            var Organization = await Repository.GetAsync(id);
            if (Organization.ParentId == parentId)
            {
                return;
            }

            //Should find children before Code change
            var children = await FindChildrenAsync(id, true);

            //Store old code of OU
            var oldCode = Organization.Code;

            //Move OU
            Organization.Code = await GetNextChildCodeAsync(parentId);
            Organization.ParentId = parentId;

            await ValidateOrganizationAsync(Organization);

            //Update Children Codes
            foreach (var child in children)
            {
                child.Code = Organization.AppendCode(Organization.Code, Organization.GetRelativeCode(child.Code, oldCode));
            }
        }

        public async Task<List<Organization>> FindChildrenAsync(int? parentId, bool recursive = false)
        {
            if (!recursive)
            {
                return await Repository.GetAll().Where(ou => ou.ParentId == parentId).OrderBy(ou=>ou.Sort).ToListAsync();
            }

            if (!parentId.HasValue)
            {
                return await Repository.GetAll().OrderBy(ou=>ou.Sort).ToListAsync();
            }

            var code = await GetCodeAsync(parentId.Value);

            return await Repository.GetAll().Where(
                ou => ou.Code.StartsWith(code) && ou.Id != parentId.Value
            ).OrderBy(ou=>ou.Sort).ToListAsync();
        }

        protected virtual async Task ValidateOrganizationAsync(Organization Organization)
        {
            var siblings = (await FindChildrenAsync(Organization.ParentId))
                .Where(ou => ou.Id != Organization.Id)
                .ToList();

            if (siblings.Any(ou => ou.DisplayName == Organization.DisplayName ))
            {
                throw new UserFriendlyException("部门名称重复");
            }
        }
    }
}
