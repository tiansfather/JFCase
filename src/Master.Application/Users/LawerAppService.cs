using Master.Authentication;
using Master.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Users
{
    public class LawerAppService:MasterAppServiceBase<User,long>
    {
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
