
using Abp.Domain.Repositories;
using Master.Authentication;
using Master.Entity;
using Master.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Logs
{
    /// <summary>
    /// 登录日志的请求api
    /// </summary>
    public class UserLoginAttemptsAppService : MasterAppServiceBase<UserLoginAttempt, int>
    {
        ///// <summary>
        ///// 获取登录日志
        ///// </summary>
        ///// <returns></returns>
        //public async virtual Task<object> GetLoginMessage()
        //{
        //    var result = await Repository.GetAll().ToListAsync();

        //    return new { allCount = allStaffCount, inCount = inJobStaffCount, offCount = offJobStaffCount };
        //}

        protected override async Task<IQueryable<UserLoginAttempt>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<UserLoginAttempt> query)
        {
            
            
            
            if (searchKeys.ContainsKey("keyword"))
            {
                var keyword = searchKeys["keyword"];
                var userRepository = Resolve<IRepository<User, long>>();
                query = from userloginAttempt in query
                        join user in userRepository.GetAll() on userloginAttempt.UserId equals user.Id
                        where user.UserName.Contains(keyword) || MasterDbContext.GetJsonValueString(user.Property, "$.NickName").Contains(keyword)
                        select userloginAttempt;
            }
            if (searchKeys.ContainsKey("roles"))
            {
                var roles = searchKeys["roles"];
                var userRoleRepository = Resolve<IRepository<UserRole, int>>();
                var roleIds = roles.Split(',').ToList().Select(o => int.Parse(o));
                query = from userloginAttempt in query
                        join userRole in userRoleRepository.GetAll() on userloginAttempt.UserId equals userRole.UserId
                        where roleIds.Contains(userRole.RoleId)
                        select userloginAttempt;
            }
            return query;
        }

        protected override object PageResultConverter(UserLoginAttempt entity)
        {
            var nickName = "";
            if (entity.UserId.HasValue)
            {
                var user = Resolve<UserManager>().GetByIdAsync(entity.UserId.Value).Result;
                nickName = user.GetPropertyValue<string>("NickName");
            }
            return new
            {
                entity.UserNameOrPhoneNumber,
                entity.ClientIpAddress,
                entity.CreationTime,
                nickName
            };
        }
    }
}
