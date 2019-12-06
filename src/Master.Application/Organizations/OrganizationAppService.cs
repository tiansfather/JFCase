using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Extensions;
using Abp.UI;
using Master.Authentication;
using Master.Dto;
using Master.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Organizations
{
    [AbpAuthorize]
    public class OrganizationAppService:MasterAppServiceBase<Organization,int>
    {
        /// <summary>
        /// 提交组织信息
        /// </summary>
        /// <param name="organizationDto"></param>
        /// <returns></returns>
        public virtual async Task SubmitOrganization(OrganizationDto organizationDto)
        {
            var manager = Manager as OrganizationManager;
            Organization organization = null;
            if (organizationDto.Id == 0)
            {
                organization = organizationDto.MapTo<Organization>();
                await manager.CreateAsync(organization);
            }
            else
            {
                organization = await manager.GetByIdAsync(organizationDto.Id);
                //仅当父级变动
                if (organizationDto.ParentId != organization.ParentId)
                {
                    var childIds = (await manager.FindChildrenAsync(organization.Id, true)).Select(o => o.Id).ToList();
                    if (organization.Id == organizationDto.ParentId)
                    {
                        throw new UserFriendlyException("不允许设置父级为自己");
                    }
                    else if (organizationDto.ParentId != null && childIds.Contains(organizationDto.ParentId.Value))
                    {
                        throw new UserFriendlyException("不允许设置父级为子级");
                    }
                    if (organizationDto.ParentId == null)
                    {
                        await manager.MoveAsync(organization.Id, null);
                    }
                    else
                    {
                        await manager.MoveAsync(organization.Id, organizationDto.ParentId.Value);
                    }
                }
                organizationDto.MapTo(organization);
                await manager.UpdateAsync(organization);
            }
        }
        

        public virtual async Task<object> GetTreeJson(int? parentId,int maxLevel=0)
        {
            var manager = Manager as OrganizationManager;
            var ous = await manager.FindChildrenAsync(parentId, true);
            if (maxLevel > 0)
            {
                ous = ous.Where(o => o.Code.ToCharArray().Count(c => c == '.') < maxLevel).ToList();
            }
            return ous.Select(o =>
            {
                var dto = o.MapTo<OrganizationDto>();
                if (dto.Name.IsNullOrEmpty())
                {
                    dto.Name = dto.DisplayName;
                }
                return dto;
            }
            );
        }
        /// <summary>
        /// 通过账套id获取账套的部门树
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public virtual async Task<object> GetTenantTreeJson(int tenantId)
        {
            var tenantManager = Resolve<TenantManager>();
            var tenant = await tenantManager.GetByIdFromCacheAsync(tenantId);
            var adminUser = await tenantManager.GetTenantAdminUser(tenant);
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                using (AbpSession.Use(tenantId, adminUser.Id))
                {
                    return await GetTreeJson(null, 0);
                }
            }
            
           
        }

        public virtual async Task MoveUserIntoOrganization(int organizationId,long[] userIds)
        {
            var userManager = Resolve<UserManager>();
            var users = await userManager.GetListByIdsAsync(userIds.AsEnumerable());
            if (users.Count(o => o.OrganizationId != null) > 0)
            {
                throw new UserFriendlyException("用户无法加入到两个组织机构中");
            }

            foreach(var user in users)
            {
                user.OrganizationId = organizationId;
            }
        }

        public virtual async Task MoveUserOutOrganization(long[] userIds)
        {
            var userManager = Resolve<UserManager>();
            var users = await userManager.GetListByIdsAsync(userIds.AsEnumerable());
            foreach(var user in users)
            {
                user.OrganizationId = null;
            }
        }
    }
}
