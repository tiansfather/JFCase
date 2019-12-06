using Abp.Auditing;
using Abp.Domain.Repositories;
using Master.Authentication;
using Master.Session.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Abp.Runtime.Security;
using Master.Entity;
using Microsoft.AspNetCore.Http;

namespace Master.Session
{
    public class SessionAppService : MasterAppServiceBase, ISessionAppService
    {
        private IRepository<User, long> _userRepository;
        private IRepository<UserRole> _userRoleRepository;
        private IRepository<Role> _roleRepository;
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        public SessionAppService(
            IRepository<User, long> userRepository,
            IRepository<UserRole> userRoleRepository,
            IRepository<Role> roleRepository

            )
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
        }

        [DisableAuditing]
        public async Task<LoginInformationDto> GetCurrentLoginInformations()
        {
            var output = new LoginInformationDto
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>
                    {
                        //{ "SignalR", SignalRFeature.IsAvailable },
                        //{ "SignalR.AspNetCore", SignalRFeature.IsAspNetCore }
                    }
                }
            };

            if (AbpSession.UserId.HasValue)
            {

                var user = await GetCurrentUserAsync();
                output.User = ObjectMapper.Map<UserLoginInfoDto>(user);
                //获取用户的角色
                var roleNameList = await Resolve<UserManager>().GetRolesAsync(user);
                //var roleNameList = (from userrole in _userRoleRepository.GetAll()
                //                    join u in _userRoleRepository.GetAll() on userrole.UserId equals u.Id
                //                    join role in _roleRepository.GetAll() on userrole.RoleId equals role.Id
                //                    where u.Id == user.Id
                //                    select new { role.DisplayName ,role.Name}).ToList();
                output.User.RoleNames = roleNameList.Select(o=>o.Name).ToList();
                output.User.RoleDisplayNames = roleNameList.Select(o => o.DisplayName).ToList();
            }

            return output;
        }

        public virtual async Task<bool> CheckPassword(string password)
        {
            var user = await _userRepository.GetAsync(AbpSession.UserId.Value);
            if(user!=null && user.Password== SimpleStringCipher.Instance.Encrypt(password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual string RefreshWechatLoginId()
        {
            //生成一个标识存入Ｓｅｓｓｉｏｎ
            var guid = Guid.NewGuid();
            HttpContextAccessor.HttpContext.Session.Set("WeChatLoginId", guid);
            return guid.ToString();
        }
    }
}
