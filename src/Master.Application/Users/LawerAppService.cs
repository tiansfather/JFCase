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
            return (await base.GetQueryable(request)).IgnoreQueryFilters().Where(o=>!o.IsDeleted && o.TenantId!=null);
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
