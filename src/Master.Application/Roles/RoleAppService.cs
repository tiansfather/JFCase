using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Abp.Web.Models;
using Master.Authentication;
using Master.Dto;
using Master.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Roles
{
    [AbpAuthorize]
    public class RoleAppService : MasterAppServiceBase<Role, int>
    {
        public RoleManager RoleManager { get; set; }
        public UserManager UserManager { get; set; }
        public IRepository<UserRole,int> UserRoleRepository { get; set; }
        public virtual async Task AddRole(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new UserFriendlyException(L("角色名不能为空"));
            }
            var role = new Role(AbpSession.TenantId, name, name);

            await RoleManager.InsertAsync(role);

            //var grantedPermissions = PermissionManager
            //    .GetAllPermissions()
            //    .Where(p => role.Permissions.Contains(p.Name))
            //    .ToList();

            //await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }

        public virtual async Task EditRole(int id, string name)
        {
            var role = await RoleManager.GetByIdAsync(id);
            role.Name = name;
            role.DisplayName = name;

            await RoleManager.UpdateAsync(role);
        }

        public override async Task DeleteEntity(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                var role = await RoleManager.GetByIdAsync(id);
                var users = await UserManager.GetUsersInRoleAsync(id);
                //同时删除对应的用户角色对应信息
                foreach (var user in users)
                {
                    await UserManager.RemoveFromRoleAsync(user.Id, id);
                }
                await RoleManager.DeleteAsync(role);
            }
        }
        [DontWrapResult]
        public override async Task<ResultPageDto> GetPageResult(RequestPageDto request)
        {
            var pageResult = await GetPageResultQueryable(request);


            var data = await (pageResult.Queryable.Include(o => o.CreatorUser))
                .Select( o => new {
                    o.Id,
                    Creator =o.CreatorUser==null?"":o.CreatorUser.Name,
                    CreationTime = o.CreationTime.ToString("yyyy-MM-dd HH:mm"),
                    o.DisplayName,
                    o.IsStatic,
                    o.Remarks
                }).ToListAsync();

            var resultData = new List<object>();
            foreach(var d in data)
            {
                //var users = (await UserManager.GetUsersInRoleAsync(d.Id)).Select(o => new { o.Id, o.Name });
                //resultData.Add(new { d.Id,d.Creator,d.CreationTime,d.DisplayName,d.IsStatic,d.Remarks,Users=users});
                resultData.Add(new { d.Id, d.Creator, d.CreationTime, d.DisplayName, d.IsStatic, d.Remarks });
            }

            var result = new ResultPageDto()
            {
                code = 0,
                count = pageResult.RowCount,
                data = resultData
            };

            return result;
        }

        /// <summary>
        /// 通过账套id获取账套的角色
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public virtual async Task<object> GetTenantRoles(int tenantId)
        {
            var tenantManager = Resolve<TenantManager>();
            var tenant = await tenantManager.GetByIdFromCacheAsync(tenantId);
            var adminUser = await tenantManager.GetTenantAdminUser(tenant);
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                using (AbpSession.Use(tenantId, adminUser.Id))
                {
                    return (await Manager.GetAllList()).Select(o => new { o.Id, o.DisplayName });
                }
            }


        }

        public virtual async Task UpdateField(int roleId,string field,string value)
        {
            var role = await Manager.GetByIdAsync(roleId);
            if (field == "displayName")
            {
                role.Name = value;
                role.DisplayName = value;
            }else if (field == "remarks")
            {
                role.Remarks = value;
            }
        }
    }
}
