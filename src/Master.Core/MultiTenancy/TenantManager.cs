using Abp;
using Abp.Application.Features;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.UI;
using Master.Application.Editions;
using Master.Application.Features;
using Master.Authentication;
using Master.Cache;
using Master.Domain;
using Master.Module;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MultiTenancy
{
    public class TenantManager: DomainServiceBase<Tenant,int>,
        IEventHandler<EntityChangedEventData<Tenant>>,
        IEventHandler<EntityDeletedEventData<Edition>>
    {
        public IFeatureManager FeatureManager { get; set; }
        //public EditionManager EditionManager { get; set; }
        //public UserManager UserManager { get; set; }
        public IRepository<TenantFeatureSetting, long> TenantFeatureRepository { get; set; }
        public IMasterFeatureValueStore MasterFeatureValueStore { get; set; }
        /// <summary>
        /// 获取所有帐套
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<Tenant>> All()
        {
            return await GetAll().ToListAsync();
        }
        /// <summary>
        /// 是否存在激活的某企业名称
        /// </summary>
        /// <param name="tenancyName"></param>
        /// <returns></returns>
        public virtual async Task<bool> ExistActiveTenancyName(string tenancyName)
        {
            return (await Repository.GetAll().CountAsync(o => o.TenancyName == tenancyName && o.IsActive)) > 0;
        }
        public override async Task InsertAsync(Tenant tenant)
        {
            await ValidateTenantAsync(tenant);

            if (await Repository.FirstOrDefaultAsync(t => t.TenancyName == tenant.TenancyName) != null)
            {
                throw new UserFriendlyException(string.Format(L("TenancyNameIsAlreadyTaken"), tenant.TenancyName));
            }

            await base.InsertAsync(tenant);
        }

        public override async Task UpdateAsync(Tenant tenant)
        {
            if (await Repository.FirstOrDefaultAsync(t => t.Name == tenant.Name && t.Id != tenant.Id) != null)
            {
                throw new UserFriendlyException("相同账套已存在");
            }
            if (await Repository.FirstOrDefaultAsync(t => t.TenancyName == tenant.TenancyName && t.Id != tenant.Id) != null)
            {
                throw new UserFriendlyException(string.Format(L("TenancyNameIsAlreadyTaken"), tenant.TenancyName));
            }

            await base.UpdateAsync(tenant);
        }

        public virtual Task<Tenant> FindByTenancyNameAsync(string tenancyName)
        {
            return Repository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
        }

        protected virtual async Task ValidateTenantAsync(Tenant tenant)
        {
            //判断是否相同账套名称已存在
            if(await Repository.CountAsync(o => o.Name == tenant.Name) > 0)
            {
                throw new UserFriendlyException(L("相同企业账套已存在"));
            }
        }

        /// <summary>
        /// 获取账套管理员账号
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns> 
        public virtual async Task<User> GetTenantAdminUser(Tenant tenant)
        {
            var adminUser = await Resolve<UserManager>().GetAll().IgnoreQueryFilters().Where(o => o.TenantId == tenant.Id &&!o.IsDeleted).OrderBy(o=>o.Id).FirstOrDefaultAsync();
            return adminUser;
        }
        #region Feature
        public Task<string> GetFeatureValueOrNullAsync(int tenantId, string featureName)
        {
            return MasterFeatureValueStore.GetValueOrNullAsync(tenantId, featureName);
        }

        public virtual async Task<IReadOnlyList<NameValue>> GetFeatureValuesAsync(int tenantId)
        {
            var values = new List<NameValue>();

            foreach (var feature in FeatureManager.GetAll())
            {
                values.Add(new NameValue(feature.Name, await GetFeatureValueOrNullAsync(tenantId, feature.Name) ?? feature.DefaultValue));
            }

            return values;
        }

        public virtual async Task SetFeatureValuesAsync(int tenantId, params NameValue[] values)
        {
            if (values.IsNullOrEmpty())
            {
                return;
            }

            foreach (var value in values)
            {
                await SetFeatureValueAsync(tenantId, value.Name, value.Value);
            }
        }

        public virtual async Task SetFeatureValueAsync(int tenantId, string featureName, string value)
        {
            await SetFeatureValueAsync(await GetByIdAsync(tenantId), featureName, value);
        }

        public virtual async Task SetFeatureValueAsync(Tenant tenant, string featureName, string value)
        {
            //No need to change if it's already equals to the current value
            if (await GetFeatureValueOrNullAsync(tenant.Id, featureName) == value)
            {
                return;
            }

            //Get the current feature setting
            TenantFeatureSetting currentSetting;
            using (UnitOfWorkManager.Current.SetTenantId(tenant.Id))
            {
                currentSetting = await TenantFeatureRepository.FirstOrDefaultAsync(f => f.Name == featureName);
            }

            //Get the feature
            var feature = FeatureManager.GetOrNull(featureName);
            if (feature == null)
            {
                if (currentSetting != null)
                {
                    await TenantFeatureRepository.DeleteAsync(currentSetting);
                }

                return;
            }

            //Determine default value
            var defaultValue = tenant.EditionId.HasValue
                ? (await Resolve<EditionManager>().GetFeatureValueOrNullAsync(tenant.EditionId.Value, featureName) ?? feature.DefaultValue)
                : feature.DefaultValue;

            //No need to store value if it's default
            if (value == defaultValue)
            {
                if (currentSetting != null)
                {
                    await TenantFeatureRepository.DeleteAsync(currentSetting);
                }

                return;
            }

            //Insert/update the feature value
            if (currentSetting == null)
            {
                await TenantFeatureRepository.InsertAsync(new TenantFeatureSetting(tenant.Id, featureName, value));
            }
            else
            {
                currentSetting.Value = value;
            }
        }

        /// <summary>
        /// Resets all custom feature settings for a tenant.
        /// Tenant will have features according to it's edition.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        public virtual async Task ResetAllFeaturesAsync(int tenantId)
        {
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                await TenantFeatureRepository.DeleteAsync(f => f.TenantId == tenantId);
            }
        }

        public void HandleEvent(EntityChangedEventData<Tenant> eventData)
        {
            if (eventData.Entity.IsTransient())
            {
                return;
            }

            CacheManager.GetTenantFeatureCache().Remove(eventData.Entity.Id);
        }

        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<Edition> eventData)
        {
            var relatedTenants = Repository.GetAllList(t => t.EditionId == eventData.Entity.Id);
            foreach (var relatedTenant in relatedTenants)
            {
                relatedTenant.EditionId = null;
            }
        }
        #endregion

        
    }
}
