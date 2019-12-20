using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Master.Authentication;
using Master.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Entity;
using Master.Authentication.External;
using Master.Configuration;
using Abp.Domain.Entities;
using Newtonsoft.Json.Linq;
using Master.Organizations;
using Master.Case;
using System.Linq;

namespace Master.Users
{
    [AbpAuthorize]
    public class UserAppService:ModuleDataAppServiceBase<User,long>
    {
        private readonly IRepository<UserRole, int> _userRoleRepository;
        private readonly IRepository<Role, int> _roleRepository;
        private readonly IRepository<UserLogin, int> _userLoginRepository;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;

        public MasterConfiguration MasterConfiguration { get; set; }

        public UserAppService(
            IRepository<UserRole, int> userRoleRepository,
            IRepository<Role, int> roleRepository,
            IRepository<UserLogin, int> userLoginRepository,
            IExternalAuthConfiguration externalAuthConfiguration
            )
        {
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _userLoginRepository = userLoginRepository;
            _externalAuthConfiguration = externalAuthConfiguration;
    }

        #region 表单提交



        public async override Task FormSubmit(FormSubmitRequestDto request)
        {
            switch (request.Action)
            {
                case "Account":
                    await Account(request);
                    break;
                default:
                    await base.FormSubmit(request);
                    break;
            }

        }
        /// <summary>
        /// 帐号提交
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task Account(FormSubmitRequestDto request)
        {
            var manager = Manager as UserManager;
            var userId = Convert.ToInt64(request.Datas["ids"]);
            var user = await Repository.GetAsync(userId);
            if (user == null)
            {
                throw new UserFriendlyException(L("未找到对应员工"));
            }
            else
            {
                user.IsActive = request.Datas["isActive"] == "1";//是否启用账号

                if (user.IsActive)
                {
                    user.ToBeVerified = false;//启用账号后自动已审核
                    var username = request.Datas["userName"];
                    if (string.IsNullOrEmpty(username))
                    {
                        //throw new UserFriendlyException(L("用户名不能为空"));
                    }
                    var password = request.Datas["password"];
                    if (!string.IsNullOrEmpty(password))
                    {
                        await manager.SetPassword(user, password);
                    }
                    int[] roles = new int[] { };
                    if (!string.IsNullOrEmpty(request.Datas["roles"]))
                    {
                        roles = request.Datas["roles"].Split(',').ToList().ConvertAll(o=>int.Parse(o)).ToArray();
                    }
                    user.UserName = username;
                    user.IsStrongPwd = request.Datas["isStrongPwd"] == "1";
                    user.MustChangePwd = request.Datas["mustChangePwd"] == "1";
                    //add 20181210  增加独立用户提交,此用户只能查看自己的信息
                    //removed 20190318
                    //user.IsSeparate= request.Datas["Separate"]=="1";
                    //add 20190118  增加标记获取
                    var statusDefinitions = MasterConfiguration.EntityStatusDefinitions.GetValueOrDefault(typeof(User));
                    if (statusDefinitions != null)
                    {
                        foreach (var statusDefinition in statusDefinitions)
                        {
                            if (request.Datas.ContainsKey(statusDefinition.Name) && request.Datas[statusDefinition.Name] == "1")
                            {
                                user.SetStatus(statusDefinition.Name);
                            }
                            else
                            {
                                user.RemoveStatus(statusDefinition.Name);
                            }
                        }
                    }
                    
                    await manager.SetRoles(user, roles);
                }
            }

        }

        #endregion

        protected override async Task<IQueryable<User>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<User> query)
        {
            var baseQuery=await base.BuildSearchQueryAsync(searchKeys, query);
            if (searchKeys.ContainsKey("organizationId"))
            {
                var organizationId = int.Parse(searchKeys["organizationId"]);
                if (organizationId == -1)
                {
                    baseQuery = baseQuery.Where(o => o.OrganizationId == null);
                }
                else
                {
                    var childOrganizations = await Resolve<OrganizationManager>().FindChildrenAsync(organizationId, true);
                    var allOrganizationIds = childOrganizations.Select(o => o.Id).ToList();
                    allOrganizationIds.Add(organizationId);
                    baseQuery = baseQuery.Where(o => o.OrganizationId != null && allOrganizationIds.Contains(o.OrganizationId.Value));
                }                
            }else if (searchKeys.ContainsKey("roleId"))
            {
                var roleId = int.Parse(searchKeys["roleId"]);
                baseQuery = from user in baseQuery
                            join userRole in Resolve<IRepository<UserRole, int>>().GetAll()
                            on user.Id equals userRole.UserId
                            where userRole.RoleId == roleId
                            select user;
            }
            return baseQuery;
        }

        [DontWrapResult]
        public virtual async Task<ResultPageDto> GetSimplePageResult(RequestPageDto requestPageDto)
        {
            var pageResult = await GetPageResultQueryable(requestPageDto);

            var data = await pageResult.Queryable
                .Select(o => new { o.Id, o.Name,o.UserName,o.PhoneNumber })
                .ToListAsync();

            var result = new ResultPageDto()
            {
                code = 0,
                count = pageResult.RowCount,
                data = data
            };

            return result;
        }



        //[DontWrapResult]
        //public override async  Task<ResultPageDto> GetPageResult(RequestPageDto request)
        //{

        //    var pageResult = await GetPageResultQueryable(request);

        //    var data = (await pageResult.Queryable.Include(o=>o.Organization).ToListAsync())
        //        .Select(o => {
        //            var roles = UserManager.GetRolesAsync(o).Result;
        //            return new {o.Id, o.Name, o.UserName, o.IsActive, RoleName = string.Join(",", roles.Select(r => r.DisplayName)),OrganizationName=o.Organization?.DisplayName };
        //        });


        //    var result = new ResultPageDto()
        //    {
        //        code = 0,
        //        count = pageResult.RowCount,
        //        data = data
        //    };

        //    return result;
        //}
        #region 冻结及恢复
        public virtual async Task Freeze(IEnumerable<long> userIds)
        {
            var users = await Manager.GetListByIdsAsync(userIds);
            var caseSourceManager = Resolve<CaseSourceManager>();
            await caseSourceManager.ClearUserUnPublishedCaseSource(userIds);

            foreach (var user in users)
            {
                user.IsActive = false;
            }
        }
        public virtual async Task UnFreeze(IEnumerable<long> userIds)
        {
            var users = await Manager.GetListByIdsAsync(userIds);
            
            foreach (var user in users)
            {
                user.IsActive = true;
            }
        }
        public virtual async Task Revert(IEnumerable<long> userIds)
        {
            var users = await Manager.GetAll().IgnoreQueryFilters().Where(o => userIds.Contains(o.Id)).ToListAsync();
            foreach(var user in users)
            {
                user.IsDeleted = false;
            }
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oriPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public virtual async Task ChangePassword(string oriPassword, string newPassword)
        {
            var user = await Repository.GetAsync(AbpSession.UserId.Value);
            var manager = Manager as UserManager;
            if (!await manager.CheckPasswordAsync(user, oriPassword))
            {
                throw new UserFriendlyException("您输入的现有密码不正确，请重新输入");
            }
            //进行强密码检测
            if (user.IsStrongPwd)
            {
                if (newPassword.Length < 6)
                {
                    throw new UserFriendlyException("密码长度不能小于6位");
                }
                if(!new System.Text.RegularExpressions.Regex("(?=.*?[0-9])(?=.*?[a-z])(?=.*?[A-Z])").IsMatch(newPassword))
                {
                    throw new UserFriendlyException("密码强度不符合要求,密码中请至少包含大小写字母和数字");
                }
            }
            //修改密码后认为不是第一次登录
            user.IsFirstLogin = false;
            await manager.SetPassword(user, newPassword);
        }
        #endregion

        #region 第三方登录
        /// <summary>
        /// 获取可登录的第三方信息
        /// </summary>
        /// <returns></returns>
        public virtual List<ExternalLoginProviderInfo> GetLoginProviders()
        {
            return _externalAuthConfiguration.Providers;
        }

        /// <summary>
        /// 获取用户绑定的所有第三方登录
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<string>> GetBindedLoginProviders(long? userId)
        {
            var user = await Repository.GetAllIncluding(o => o.Logins).SingleOrDefaultAsync(o => o.Id == (userId.HasValue ? userId.Value : AbpSession.UserId.Value));
            return user.Logins.Select(o => o.LoginProvider).ToList();
        }

        /// <summary>
        /// 解除第三方登录绑定
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task UnBindLogin(string provider, long? userId)
        {
            await _userLoginRepository.DeleteAsync(o => o.UserId == (userId.HasValue ? userId.Value : AbpSession.UserId.Value) && o.LoginProvider == provider);
        }
        #endregion

        #region 角色
        /// <summary>
        /// 移除用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual async Task RemoveFromRole(long userId, int roleId)
        {
            var manager = Manager as UserManager;
            await manager.RemoveFromRoleAsync(userId, roleId);
        }
        /// <summary>
        /// 添加用户角色
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual async Task MoveIntoRole(long[] userIds, int roleId)
        {
            var manager = Manager as UserManager;
            foreach (var userId in userIds)
            {
                await manager.MoveIntoRoleAsync(userId, roleId);
            }
        } 
        #endregion
        protected override string ModuleKey()
        {
            return nameof(User);
        }

        /// <summary>
        /// 通过openid返回用户状态1:正常登录，-1:被注销，2：未注册,3:审核中
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public virtual async Task<int> GetUserStatusByWeOpenId(string openId)
        {
            var user = await (Manager as UserManager).FindAsync(new Microsoft.AspNetCore.Identity.UserLoginInfo("Wechat", openId, ""));
            if (user != null)
            {
                return !user.IsDeleted ? 1 : -1;
            }
            else
            {
                var newMiner = await Resolve<NewMinerManager>().GetAll().Where(o => o.OpenId == openId).FirstOrDefaultAsync();
                return newMiner != null ? 3 : 2;
            }

        }


        #region 用户信息
        /// <summary>
        /// 用户是否激活状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> GetUserIsActive(long id)
        {
            var user = await Manager.GetByIdAsync(id);
            return user.IsActive;
        }

        protected override object ResultConverter(User entity)
        {
            //获取用户已完成的案例
            var caseCount = Resolve<CaseInitialManager>().GetAll()
                .Count(o => o.CreatorUserId == entity.Id && (o.CaseStatus == CaseStatus.展示中 ));
            return new
            {
                Avata = entity.GetPropertyValue<string>("Avata"),
                AnYou = entity.GetPropertyValue<string>("AnYou"),//所属领域
                AnYouId = entity.GetPropertyValue<int?>("AnYouId"),//所属领域Id
                AnYouIds = entity.GetPropertyValue<int[]>("AnYouIds"),//所属领域Id
                WorkYear= entity.GetPropertyValue<int>("WorkYear"),
                Introduction = entity.GetPropertyValue<string>("Introduction"),
                entity.Name,
                caseCount,
                entity.Id,
                CreationTime = entity.CreationTime.ToString("yyyy-MM-dd"),
                entity.WorkLocation
            };
        } 

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="anYouId"></param>
        /// <returns></returns>
        public virtual async Task UpdateUserInfo(UserUpdateDto userUpdateDto)
        {
            var user = await Manager.GetByIdAsync(userUpdateDto.Id);
            user.WorkLocation = userUpdateDto.WorkLocation;
            user.Name = userUpdateDto.Name;
            string anYou = "";
            if (userUpdateDto.AnYouIds.Length>0)
            {
                var nodes = await Resolve<BaseTreeManager>().GetListByIdsAsync(userUpdateDto.AnYouIds);
                anYou = string.Join(',', nodes.Select(o => o.DisplayName));             
            }
            user.SetPropertyValue("AnYou", anYou);
            user.SetPropertyValue("AnYouIds", userUpdateDto.AnYouIds);
            user.SetPropertyValue("Avata", userUpdateDto.Avata);
            user.SetPropertyValue("Introduction", userUpdateDto.Introduction);
            user.SetPropertyValue("WorkYear", userUpdateDto.WorkYear);
        }
        #endregion
        /// <summary>
        /// 获取当前登录token用于前台判断账号登出
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<string> GetCurrentToken()
        {
            if (!AbpSession.UserId.HasValue)
            {
                return string.Empty;
            }
            var user = await Manager.GetByIdAsync(AbpSession.UserId.Value);
            return user.GetData<string>("currentToken");
        }

        public override async Task DeleteEntity(IEnumerable<long> ids)
        {
            //有数据的用户不能删除
            var initialsCount = await Resolve<CaseSourceManager>().GetAll().Where(o => ids.ToList().Contains(o.OwerId.Value) && o.CaseSourceStatus==CaseSourceStatus.已加工).CountAsync();
            if (initialsCount > 0)
            {
                throw new UserFriendlyException($"该用户名下有{initialsCount}个已发布案例，请先清除后再删用户");
            }
            var caseSourceManager = Resolve<CaseSourceManager>();
            await caseSourceManager.ClearUserUnPublishedCaseSource(ids);
            //20191219 完全删除
            await Repository.HardDeleteAsync(o => ids.Contains(o.Id));
            try
            {
                await CurrentUnitOfWork.SaveChangesAsync();
            }catch(Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw new UserFriendlyException("用户有数据关联，无法删除");
                
            }
            //await base.DeleteEntity(ids);
        }
    }
}
