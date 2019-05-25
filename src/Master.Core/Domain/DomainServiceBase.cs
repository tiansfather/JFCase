using Abp.Application.Features;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities.Caching;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Master.Authentication;
using Master.Cache;
using Master.Entity;
using Master.Module;
using Master.MultiTenancy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Domain
{
    /// <summary>
    /// 基于实体的领域基类，封装基本增删改查操作
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class DomainServiceBase<TEntity, TPrimaryKey> : DomainServiceBase, IEventHandler<EntityChangedEventData<TEntity>>
        where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        //public UserManager UserManager { get; set; }
        public IFeatureChecker FeatureChecker { get; set; }
        public IHostingEnvironment HostingEnvironment { get; set; }
        /// <summary>
        /// 基于id的实体缓存
        /// </summary>
        public IEntityCoreCache<TEntity> EntityCoreCache { get; set; }
        public IRepository<TEntity, TPrimaryKey> Repository { get; set; }
        

        #region 数据获取接口
        public virtual async Task<TEntity> GetByIdAsync(TPrimaryKey id)
        {
            try
            {
                return await Repository.GetAsync(id);
            }
            catch
            {
                return null;
            }

        }
        public Task<TEntity> GetByIdFromCacheAsync(int id)
        {
            return Task.FromResult(EntityCoreCache[id]);
            //return await CacheManager.GetCache<TPrimaryKey, TEntity>(typeof(TEntity).Name)
            //    .GetAsync(id, async () => { return await Repository.GetAsync(id); });
        }
        public virtual async Task<IEnumerable<TEntity>> GetListByIdsAsync(IEnumerable<TPrimaryKey> ids)
        {
            return await Repository.GetAll().Where(o => ids.Contains(o.Id)).ToListAsync();
        }
        public virtual IQueryable<TEntity> GetAll()
        {
            //add 20181210增加过滤,如果当前用户是独立用户
            //removed 20190318 由于独立用户有很多bug
            //if (AbpSession.IsSeparateUser() && typeof(CreationAuditedEntity<TPrimaryKey>).IsAssignableFrom(typeof(TEntity)))
            //{
            //    return Repository.GetAll().Where(o => (o as CreationAuditedEntity<TPrimaryKey>).CreatorUserId == null || (o as CreationAuditedEntity<TPrimaryKey>).CreatorUserId == AbpSession.UserId);
            //}

            return Repository.GetAll();
        }
        /// <summary>
        /// 缓存方式获取实体所有数据
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetAllList()
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();

            var key = typeof(TEntity).FullName + "@" + (tenantId ?? 0);
            var result= await CacheManager.GetCache<string, List<TEntity>>(typeof(TEntity).FullName)
                .GetAsync(key, async () => { return await Repository.GetAll().ToListAsync(); });
            //if(result==null || result.Count == 0)
            //{
            //    await CacheManager.GetCache<string, List<TEntity>>(typeof(TEntity).FullName).RemoveAsync(key);
            //    result= await GetAll().ToListAsync();
            //}

            return result;
        }
        /// <summary>
        /// 实体数据改变后清空实体缓存
        /// </summary>
        /// <param name="eventData"></param>
        public void HandleEvent(EntityChangedEventData<TEntity> eventData)
        {
            var typeName = typeof(TEntity).FullName;
            var tenantId = 0;
            if (typeof(IMustHaveTenant).IsAssignableFrom(typeof(TEntity)))
            {
                tenantId = (eventData.Entity as IMustHaveTenant).TenantId;
            }
            else if (typeof(IMayHaveTenant).IsAssignableFrom(typeof(TEntity)))
            {
                tenantId = (eventData.Entity as IMayHaveTenant).TenantId??0;
            }
            {

            }
            var key = typeName + "@" + tenantId;
            CacheManager.GetCache<string, List<TEntity>>(typeName).Remove(key);
        }
        #endregion

        #region 增删改
        /// <summary>
        /// 子类中重写验证实体有效性
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task ValidateEntity(TEntity entity)
        {

        }

        public virtual async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            await ValidateEntity(entity);
            return await Repository.InsertAndGetIdAsync(entity);
        }
        public virtual async Task InsertAsync(TEntity entity)
        {
            await ValidateEntity(entity);
            await Repository.InsertAsync(entity);
        }
        public virtual async Task UpdateAsync(TEntity entity)
        {
            await ValidateEntity(entity);
            await Repository.UpdateAsync(entity);
        }
        public virtual async Task SaveAsync(TEntity entity)
        {
            if (EqualityComparer<TPrimaryKey>.Default.Equals(entity.Id, default(TPrimaryKey)))
            {
                await InsertAsync(entity);
            }
            else
            {
                await UpdateAsync(entity);
            }
        }
        public virtual async Task DeleteAsync(IEnumerable<TPrimaryKey> ids)
        {
            await Repository.DeleteAsync(o => ids.Contains(o.Id));
        }
        public virtual async Task DeleteAsync(TEntity entity)
        {
            await Repository.DeleteAsync(entity);
        }
        public virtual async Task SetPropertyValue(TPrimaryKey primaryKey, string propertyName, object propertyValue)
        {
            var entity = GetByIdAsync(primaryKey) as IHaveProperty;
            if (entity != null)
            {
                entity.SetPropertyValue(propertyName, propertyValue);
            }

        }
        #endregion

        #region 当前登录用户及账套信息
        public virtual Task<User> GetCurrentUserAsync()
        {
            var userManager = Resolve<UserManager>();
            var user = userManager.GetByIdAsync(AbpSession.GetUserId());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }
        public virtual Task<Tenant> GetCurrentTenantAsync()
        {
            var tenantManager = Resolve<TenantManager>();
            return tenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }
        #endregion

        #region 数据提交
        /// <summary>
        /// 将提交来的数据存为实体
        /// </summary>
        /// <param name="Datas"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Task<TEntity> LoadEntityFromDatas(IDictionary<string, string> Datas, TEntity obj = null)
        {
            return Task.Run(() =>
            {
                var entity = obj;
                //如果是添加方式
                if (obj == null)
                {
                    //通过反序列化方式进行赋值
                    var serializedString = JsonConvert.SerializeObject(Datas);
                    entity = JsonConvert.DeserializeObject<TEntity>(serializedString);

                }
                else
                {
                    //遍历datas赋值
                    foreach (var property in typeof(TEntity).GetProperties())
                    {
                        if (Datas.ContainsKey(property.Name))
                        {
                            var propertyType = property.PropertyType;
                            object targetValue = null;
                            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                            {
                                if (!string.IsNullOrEmpty(Datas[property.Name]))
                                {
                                    targetValue = Convert.ChangeType(Datas[property.Name], propertyType.GetGenericArguments()[0]);
                                }

                            }
                            else if (typeof(System.Enum).IsAssignableFrom(propertyType))
                            {
                                targetValue = Enum.Parse(propertyType, Datas[property.Name]);
                            }
                            else if (propertyType == typeof(bool) && string.IsNullOrEmpty(Datas[property.Name]))
                            {
                                targetValue = false;
                            }
                            else
                            {
                                targetValue = Convert.ChangeType(Datas[property.Name], propertyType);
                            }

                            property.SetValue(entity, targetValue);
                        }
                    }
                }

                return entity;
            });


        }

        /// <summary>
        /// 表单提交建立实体
        /// </summary>
        /// <param name="Datas"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> DoAdd(IDictionary<string, string> Datas)
        {
            return await LoadEntityFromDatas(Datas);
        }

        /// <summary>
        /// 表单提交修改实体
        /// </summary>
        /// <param name="Datas"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> DoEdit(IDictionary<string, string> Datas, TPrimaryKey id)
        {
            var entity = await GetByIdAsync(id);
            return await LoadEntityFromDatas(Datas, entity);
        } 
        #endregion

    }

    public abstract class DomainServiceBase : DomainService
    {
        public IAbpSession AbpSession { get; set; }
        public ICacheManager CacheManager { get; set; }
        public IPermissionChecker PermissionChecker { get; set; }
        public DomainServiceBase()
        {
            LocalizationSourceName = MasterConsts.LocalizationSourceName;
        }

        protected TEntity Resolve<TEntity>()
        {
            using (var objectWrapper=IocManager.Instance.ResolveAsDisposable<TEntity>())
            {
                return objectWrapper.Object;
            }
        }

        #region 管理类获取
        public Type GetEntityManagerType(Type entityType)
        {
            return CacheManager.GetCache<Type, Type>("EntityManagerTypeCache").Get(entityType, o => {
                var asmQualifiedname = entityType.AssemblyQualifiedName.Split(',')[1];
                var prefix = entityType.FullName.Substring(0, entityType.FullName.LastIndexOf('.'));
                //var returnType = Type.GetType($"{prefix}.I{entityType.Name}Manager,{asmQualifiedname}");
                var returnType = entityType.Assembly.GetType($"{prefix}.I{entityType.Name}Manager");
                if (returnType == null)
                {
                    //returnType = Type.GetType($"{prefix}.{entityType.Name}Manager,{asmQualifiedname}");
                    returnType = entityType.Assembly.GetType($"{prefix}.{entityType.Name}Manager");
                }
                return returnType;
            });

        }


        /// <summary>
        /// 获取实体类对应的管理类
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public DomainServiceBase GetManager(Type entityType)
        {
            var managerType = GetEntityManagerType(entityType);
            using (var managerWrapper = IocManager.Instance.ResolveAsDisposable(managerType))
            {
                var manager = managerWrapper.Object as DomainServiceBase;
                return manager;
            }
        }
        #endregion

        #region 模块数据
        /// <summary>
        /// 对模块返回的数据的前处理(当基础信息加载完毕，加载自定义属性、关联数据之前调用)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="moduleInfo"></param>
        /// <param name = "entity" ></ param >
        /// <returns></returns>
        public virtual async Task FillEntityDataBefore(IDictionary<string, object> data, ModuleInfo moduleInfo,object entity)
        {

        }
        /// <summary>
        /// 对模块返回的数据的后处理(当自定义属性、关联数据都加载完毕后调用)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="moduleInfo"></param>
        /// <param name = "entity" ></ param >
        /// <returns></returns>
        public virtual async Task FillEntityDataAfter(IDictionary<string, object> data, ModuleInfo moduleInfo,object entity)
        {

        }
        #endregion
    }
}
