using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Runtime.Security;
using Abp.Web.Models;
using Master.Application.Editions;
using Master.Authentication;
using Master.Authentication.External;
using Master.Cache;
using Master.Configuration;
using Master.Dto;
using Master.Entity;
using Master.EntityFrameworkCore;
using Master.EntityFrameworkCore.Seed.Tenants;
using Master.MES.Domains;
using Master.MES.Dtos;
using Master.Module;
using Master.MultiTenancy;
using Master.Templates;
using Master.Units;
using Master.WeiXin;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES.Service
{
    public class MesTenancyAppService : MasterAppServiceBase<Tenant,int>
    {
        public ModuleInfoManager ModuleInfoManager { get; set; }
        public TenantManager TenantManager { get; set; }
        public MESTenantManager MESTenantManager { get; set; }
        public RoleManager RoleManager { get; set; }
        public UserManager UserManager { get; set; }
        public UnitManager UnitManager { get; set; }
        public EditionManager EditionManager { get; set; }
        public IRepository<UserLoginAttempt,int> UserLoginAttemptRepository { get; set; }
        public TemplateManager TemplateManager { get; set; }

        public IRepository<ProcessTask,int> ProcessTaskRepository { get; set; }
        public IRepository<ProcessTaskReport,int> ProcessTaskReportRepository { get; set; }

        protected override async Task<IQueryable<Tenant>> GetQueryable(RequestPageDto request)
        {
            return (await base.GetQueryable(request))
                .Include(o=>o.Edition);
        }

        [DontWrapResult]
        public override async Task<ResultPageDto> GetPageResult(RequestPageDto request)
        {
            var query = await GetPageResultQueryable(request);

            var tenants = await query.Queryable.ToListAsync();

            var data = new List<object>();
            foreach(var o in tenants)
            {
                //var count = await ProcessTaskRepository.GetAll().CountAsync();
                //var taskCount=await ProcessTaskRepository.GetAll()
                //    .Where(p => MESDbContext.GetJsonValueNumber(p.Supplier.Property, "$.TenantId") ==o.Id)
                //    .CountAsync();
                //var reportCount=await ProcessTaskReportRepository.GetAll()
                //    .Where(p=> MESDbContext.GetJsonValueNumber(p.ProcessTask.Supplier.Property, "$.TenantId") == o.Id)
                //      .CountAsync();
                var lastLogin = await UserLoginAttemptRepository.GetAll().IgnoreQueryFilters().Where(u => u.TenantId == o.Id && u.Result == LoginResultType.Success).LastOrDefaultAsync();
                data.Add(new
                {
                    o.Id,
                    o.TenancyName,
                    o.Name,
                    o.IsActive,
                    Edition = o.Edition?.DisplayName,
                    ExpireDate = o.GetPropertyValue<DateTime?>("ExpireDate"),
                    LastLoginTime = lastLogin?.CreationTime.ToString("yyyy-MM-dd HH:mm"),
                    //TaskNumber = ProcessTaskRepository.Count(p => p.TenantId == o.Id && p.ProcessTaskStatus != ProcessTaskStatus.Inputed),
                    //ReportNumber = ProcessTaskReportRepository.Count(p => p.TenantId == o.Id),
                    //TaskCount = taskCount,
                    //ReportCount = reportCount,
                    CreationTime = o.CreationTime.ToString("yyyy-MM-dd HH:mm"),
                    Mobile = o.GetPropertyValue<string>("Mobile"),
                    PersonName = o.GetPropertyValue<string>("PersonName")
                });
            }

            //var data = tasks.Select( o => {
            //    var lastLogin= UserLoginAttemptRepository.GetAll().Where(u => u.TenantId == o.Id && u.Result == LoginResultType.Success).LastOrDefault();
            //    return new
            //    {
            //        o.Id,
            //        o.TenancyName,
            //        o.Name,
            //        o.IsActive,
            //        LastLoginTime=lastLogin?.CreationTime.ToString("yyyy-MM-dd HH:mm"),
            //        TaskNumber = ProcessTaskRepository.Count(p=>p.TenantId==o.Id && p.ProcessTaskStatus!=ProcessTaskStatus.Inputed),
            //        ReportNumber= ProcessTaskReportRepository.Count(p=>p.TenantId==o.Id),
            //        CreationTime =o.CreationTime.ToString("yyyy-MM-dd HH:mm"),
            //        Mobile=o.GetPropertyValue<string>("Mobile"),
            //        PersonName=o.GetPropertyValue<string>("PersonName")
            //    };
            //});

            var result = new ResultPageDto()
            {
                code = 0,
                count = query.RowCount,
                data = data
            };

            return result;
        }


        #region 注册
        /// <summary>
        /// 将一个账套绑定到另一个账套下的往来单位
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="inviter"></param>
        /// <param name="unitName"></param>
        /// <returns></returns>
        public virtual async Task BindTenantToUnitName(int tenantId,int inviter,string unitName)
        {
            //将注册的账号绑定到邀请人的往来单位中
            using (CurrentUnitOfWork.SetTenantId(inviter))
            {
                var unit = await UnitManager.GetByUnitNameOrInsert(unitName);
                //设置此往来单位为云单位
                unit.SetPropertyValue("TenantId", tenantId);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 微信端认领加工点接口
        /// </summary>
        /// <param name="inviter"></param>
        /// <param name="unitName"></param>
        /// <returns></returns>
        public virtual async Task ClaimUnit(int inviter,string unitName)
        {
            await BindTenantToUnitName(AbpSession.TenantId.Value, inviter, unitName);
        }
        /// <summary>
        /// 微信端注册提交
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        public virtual async Task Register(RegisterDto registerDto)
        {
            var tenant = new Tenant(registerDto.CompanyName, registerDto.TenancyName);

            //var defaultEdition = await _editionManager.FindByNameAsync(EditionManager.DefaultEditionName);
            //if (defaultEdition != null)
            //{
            //    tenant.EditionId = defaultEdition.Id;
            //}

            await TenantManager.InsertAsync(tenant);
            await CurrentUnitOfWork.SaveChangesAsync(); // To get new tenant's id.

            tenant.SetPropertyValue("Mobile", registerDto.Mobile);
            tenant.SetPropertyValue("PersonName", registerDto.Name);
            
            //如果是被邀请进来的，则设置邀请人id
            if (registerDto.Inviter.HasValue)
            {
                Logger.Info("设置邀请信息");
                tenant.SetPropertyValue("Inviter", registerDto.Inviter.Value);
                //邀请进来的默认激活
                await MESTenantManager.SetActive(tenant.Id, true);

                var edition_processor = await EditionManager.GetAll().Where(o => o.Name == "JIAGONGDIANJICHUBAN").SingleOrDefaultAsync();//加工点版本
                var edition_customer = await EditionManager.GetAll().Where(o => o.Name == "KEHUJICHUBAN").SingleOrDefaultAsync();//客户版本
                //默认加工点版本
                if (edition_processor != null)
                {
                    tenant.EditionId = edition_processor.Id;
                }
                

                if (!string.IsNullOrEmpty(registerDto.InviterUnitName))
                {
                    var unit = await UnitManager.GetAll().Where(o => o.TenantId == registerDto.Inviter.Value && o.UnitName == registerDto.InviterUnitName)
                    .FirstOrDefaultAsync();
                    if (unit != null)
                    {
                        
                        //设置默认版本
                        if (unit.UnitNature==UnitNature.客户 && edition_customer != null)
                        {
                            tenant.EditionId = edition_customer.Id;
                        }

                    }
                    
                    //绑定新注册账号至邀请人的往来单位
                    await BindTenantToUnitName(tenant.Id, registerDto.Inviter.Value, registerDto.InviterUnitName);
                }
            }
            // We are working entities of new tenant, so changing tenant filter
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                // Create static roles for new tenant
                await RoleManager.CreateStaticRoles(tenant.Id);

                await CurrentUnitOfWork.SaveChangesAsync(); // To get static role ids

                // Grant all permissions to admin role
                var adminRole = RoleManager.GetAll().Where(r => r.Name == StaticRoleNames.Tenants.Admin).Single();
                await RoleManager.GrantAllPermissionsAsync(adminRole);

                // Create admin user for the tenant
                var adminUser = await CreateTenantAdminUser(tenant.Id, registerDto);
                //设置管理用户有接收外协提醒的权限
                adminUser.SetStatus(MESStatusNames.ReceiveOuterTask);
                // Assign admin user to role!
                await UserManager.SetRoles(adminUser, new int[] { adminRole.Id });

                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 创建账套管理员用户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        private async Task<User> CreateTenantAdminUser(int tenantId, RegisterDto registerDto)
        {
            var user = new User()
            {
                TenantId = tenantId,
                UserName = registerDto.Mobile,
                Name = registerDto.Name,
                Password = SimpleStringCipher.Instance.Encrypt(registerDto.Password),
                PhoneNumber=registerDto.Mobile
            };
            //同时绑定微信
            var weuser = WeiXinHelper.GetWeiXinUserInfo();
            if (weuser != null)
            {
                user.Logins = new List<UserLogin>
                {
                    new UserLogin
                    {
                        LoginProvider = WeChatAuthProviderApi.Name,
                        ProviderKey = weuser.openid,
                        TenantId = user.TenantId
                    }
                };
            }

            await UserManager.InsertAsync(user);
            await CurrentUnitOfWork.SaveChangesAsync(); // To get admin user's id

            return user;
        }

        #endregion

        #region 管理接口
        /// <summary>
        /// 设置账套是否激活
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public virtual async Task SetActive(int tenantId,bool isActive)
        {
            await MESTenantManager.SetActive(tenantId, isActive);
            //var tenant = await Repository.GetAsync(tenantId);
            //tenant.IsActive = isActive;
            ////如果账套还没有模块,则初始化模块
            //if(!await ModuleInfoManager.GetAll().AnyAsync(o => o.TenantId == tenantId))
            //{
            //    await InitModule(new int[] { tenant.Id });
            //}
            //await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public virtual async Task InitModule(IEnumerable<int> ids)
        {
            await MESTenantManager.InitModule(ids);
            //foreach (var tenantId in ids)
            //{
            //    var modules = await ModuleInfoManager.GetAll().Where(o => o.TenantId == tenantId).ToListAsync();
            //    foreach (var module in modules)
            //    {
            //        await ModuleInfoManager.Repository.HardDelete(module);                    
            //    }
            //    await CurrentUnitOfWork.SaveChangesAsync();
            //    var context = Repository.GetDbContext() as MasterDbContext;
            //    new TenantDefaultModuleBuilder(context, tenantId).Create();
            //}
            
        }

        ///// <summary>
        ///// 获取账套管理员账号
        ///// </summary>
        ///// <param name="tenant"></param>
        ///// <returns></returns>
        //[AbpAuthorize]
        //public virtual async Task<User> GetTenantAdminUser(Tenant tenant)
        //{
        //    //通过手机获取管理用户
        //    var mobile = tenant.GetPropertyValue<string>("Mobile");
        //    var adminUser = await UserManager.GetAll().IgnoreQueryFilters().Where(o => o.TenantId == tenant.Id && (o.UserName==mobile || o.PhoneNumber == mobile)).FirstOrDefaultAsync();
        //    return adminUser;
        //}

        /// <summary>
        /// 初始化管理员权限
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public virtual async Task InitAdminRole(IEnumerable<int> ids)
        {
            await MESTenantManager.InitAdminRole(ids);
            //var tenants = await Manager.GetListByIdsAsync(ids);
            //foreach(var tenant in tenants)
            //{

            //    using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            //    {
            //        //通过手机获取管理用户
            //        var adminUser = await MESTenantManager.GetTenantAdminUser(tenant);
            //        //获取管理员权限
            //        var adminRole = await RoleManager.GetAll().Where(o => o.TenantId == tenant.Id && o.Name == StaticRoleNames.Tenants.Admin).FirstOrDefaultAsync();
            //        if (adminRole != null)
            //        {
            //            await RoleManager.GrantAllPermissionsAsync(adminRole);

            //            //清空角色权限缓存
            //            var cacheKey = adminRole.Id + "@" + (adminRole.TenantId ?? 0);                        
            //            await CacheManager.GetRolePermissionCache().RemoveAsync(cacheKey);
            //        }

            //        if (adminUser != null)
            //        {
            //            await UserManager.SetRoles(adminUser, new int[] { adminRole.Id });
            //        }

            //    }


            //}
        }
        [AbpAuthorize]
        public virtual async Task UpdateField(int tenantId,string field,string value)
        {
            var tenant = await Repository.GetAsync(tenantId);
            switch (field)
            {
                case "tenancyName":
                    tenant.TenancyName = value;
                    break;
                case "name":
                    tenant.Name = value;
                    break;
                case "expireDate":
                    if (string.IsNullOrEmpty(value))
                    {
                        tenant.RemoveProperty("ExpireDate");
                    }
                    else
                    {
                        if(DateTime.TryParse(value,out var _))
                        {
                            tenant.SetPropertyValue("ExpireDate", DateTime.Parse(value));
                        }
                    }
                    break;
                case "password":
                    //设置密码
                    //寻找账套中用户名和账套注册手机一样的用户，或者用户名是admin的用户
                    var user = await UserManager.GetAll().IgnoreQueryFilters().Where(o =>o.TenantId==tenantId && (o.UserName == tenant.GetPropertyValue<string>("Mobile") || o.UserName==User.AdminUserName)).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        await UserManager.SetPassword(user, value);
                    }
                    return;
            }
            await Manager.UpdateAsync(tenant);
        }
        #endregion

        #region 模板
        /// <summary>
        /// 获取企业账套的所有加工单模板
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public virtual async Task<IEnumerable<TemplateDto>> GetTaskTemplates(int tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var templates = await TemplateManager.GetTemplatesByTemplateType(MESTemplateSetting.TemplateType_ProcessTask);
                return templates.MapTo<List<TemplateDto>>();
            }
            //var tenant = await Manager.GetByIdAsync(tenantId);
            //return tenant.GetPropertyValue<string>("TemplateContent");
        }
        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="templateName"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public virtual async Task AddTaskTemplate(int tenantId,string templateName)
        {
            var template = new Template()
            {
                TemplateName = templateName,
                TemplateType = MESTemplateSetting.TemplateType_ProcessTask,
                TenantId = tenantId
            };
            await TemplateManager.InsertAsync(template);
        }
        [AbpAuthorize]
        public virtual async Task DelTaskTemplate(int tenantId,int templateId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                await TemplateManager.DeleteAsync(new int[] { templateId });
            }
                
        }
        /// <summary>
        /// 设置企业账套的加工单模板
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public virtual async Task SetTaskTemplate(TaskTemplateDto taskTemplateDto)
        {
            using (CurrentUnitOfWork.SetTenantId(taskTemplateDto.TenantId))
            {
                var template = await TemplateManager.GetByIdAsync(taskTemplateDto.Id);
                template.TemplateContent = taskTemplateDto.TemplateContent;
            }
            //var tenant = await Manager.GetByIdAsync(taskTemplateDto.TenantId);
            //tenant.SetPropertyValue("TemplateContent", taskTemplateDto.TemplateContent);
        }
        [AbpAuthorize]
        public virtual async Task SetTaskTemplateName(int tenantId,int id,string templateName)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var template = await TemplateManager.GetByIdAsync(id);
                template.TemplateName = templateName;
            }
            //var tenant = await Manager.GetByIdAsync(taskTemplateDto.TenantId);
            //tenant.SetPropertyValue("TemplateContent", taskTemplateDto.TemplateContent);
        }
        #endregion

        #region 加工点相关
        /// <summary>
        /// 设置加工点的基本信息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="processorInfo"></param>
        /// <returns></returns>
        public virtual async Task SetTenantProcessInfo(ProcessorInfo processorInfo)
        {
            var tenant = await GetCurrentTenantAsync();
            tenant.SetPropertyValue("ProcessorInfo", processorInfo);
            tenant.GetPropertyValue<string>("Mobile");
        }

        /// <summary>
        /// 设置加工点的特色照片
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="characteristics"></param>
        /// <returns></returns>
        public virtual async Task SetTenantCharacteristics(List<UploadFileInfo> characteristics)
        {
            var tenant = await GetCurrentTenantAsync();
            tenant.SetPropertyValue("Characteristics", characteristics);
        }
        public virtual async Task<object> GetProcessInfos(int tenantId)
        {
            

            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant))
            {
                var tenant = await Manager.GetByIdAsync(tenantId);
                var equipments = await Resolve<EquipmentManager>().GetAll()
                    .Where(o => o.TenantId == tenantId)
                    .ToListAsync();

                return new
                {
                    ProcessorInfo = tenant.GetPropertyValue<ProcessorInfo>("ProcessorInfo"),
                    Characteristics = tenant.GetPropertyValue<ProcessorInfo>("Characteristics"),
                    Equipments = equipments.Select(o=>new {
                        o.EquipmentPic,
                        o.EquipmentSN,
                        o.Range,
                        o.Price,
                        o.BuyCost,
                        o.BuyYear
                    })
                };
            }
           

            
        }
        #endregion

    }
}
