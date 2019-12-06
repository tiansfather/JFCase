using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Master.Authentication;
using Master.Dto;
using Master.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Users
{
    public class LawerAppService:MasterAppServiceBase<User,long>
    {
        protected override async Task<IQueryable<User>> GetQueryable(RequestPageDto request)
        {
            var minerRole = await Resolve<RoleManager>().FindByNameAsync(StaticRoleNames.Tenants.Miner);
            return (await base.GetQueryable(request))
                .Where(o=>o.Sort<999999)
                .Where(o => o.Roles.Count(r => r.RoleId == minerRole.Id) > 0)
                .OrderBy(o=>o.Sort);

        }

        public override async Task<ResultPageDto> GetPageResult(RequestPageDto request)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant,AbpDataFilters.MustHaveTenant))
            {
                return await base.GetPageResult(request);
            }
            
        }
        protected override object PageResultConverter(User entity)
        {
            return new
            {
                entity.Id,
                entity.PhoneNumber,
                WorkYear=entity.GetPropertyValue("WorkYear"),
                Avata = entity.GetPropertyValue("Avata"),
                AnYou = entity.GetPropertyValue("AnYou"),
                entity.Name,
                entity.WorkLocation
            };
        }
    }
}
