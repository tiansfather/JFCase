using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Uow;
using Master.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Configuration
{
    public class SettingStore :DomainServiceBase<Setting,int>, ISettingStore, ITransientDependency
    {
        public async Task CreateAsync(SettingInfo settingInfo)
        {
            using (UnitOfWorkManager.Current.SetTenantId(settingInfo.TenantId))
            {
                using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    await Repository.InsertAsync(settingInfo.ToSetting());
                    await UnitOfWorkManager.Current.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteAsync(SettingInfo settingInfo)
        {
            using (UnitOfWorkManager.Current.SetTenantId(settingInfo.TenantId))
            {
                using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    await Repository.DeleteAsync(
                    s => s.UserId == settingInfo.UserId && s.Name == settingInfo.Name && s.TenantId == settingInfo.TenantId
                    );
                    await UnitOfWorkManager.Current.SaveChangesAsync();
                }
            }
        }

        public async Task<List<SettingInfo>> GetAllListAsync(int? tenantId, long? userId)
        {
            /* Combined SetTenantId and DisableFilter for backward compatibility.
             * SetTenantId switches database (for tenant) if needed.
             * DisableFilter and Where condition ensures to work even if tenantId is null for single db approach.
             */
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    var settins = (await Repository.GetAllListAsync(s => s.UserId == userId && s.TenantId == tenantId))
                        .Select(s => s.ToSettingInfo())
                        .ToList();
                    return settins;
                }
            }
        }

        public async Task<SettingInfo> GetSettingOrNullAsync(int? tenantId, long? userId, string name)
        {
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    return (await Repository.FirstOrDefaultAsync(s => s.UserId == userId && s.Name == name && s.TenantId == tenantId))
                    .ToSettingInfo();
                }
            }
        }

        public async Task UpdateAsync(SettingInfo settingInfo)
        {
            using (UnitOfWorkManager.Current.SetTenantId(settingInfo.TenantId))
            {
                using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    var setting = await Repository.FirstOrDefaultAsync(
                        s => s.TenantId == settingInfo.TenantId &&
                             s.UserId == settingInfo.UserId &&
                             s.Name == settingInfo.Name
                        );

                    if (setting != null)
                    {
                        setting.Value = settingInfo.Value;
                    }

                    await UnitOfWorkManager.Current.SaveChangesAsync();
                }
            }
        }
    }

    internal static class SettingExtensions
    {
        /// <summary>
        /// Creates new <see cref="Setting"/> object from given <see cref="SettingInfo"/> object.
        /// </summary>
        public static Setting ToSetting(this SettingInfo settingInfo)
        {
            return settingInfo == null
                ? null
                : new Setting(settingInfo.TenantId, settingInfo.UserId, settingInfo.Name, settingInfo.Value);
        }

        /// <summary>
        /// Creates new <see cref="SettingInfo"/> object from given <see cref="Setting"/> object.
        /// </summary>
        public static SettingInfo ToSettingInfo(this Setting setting)
        {
            return setting == null
                ? null
                : new SettingInfo(setting.TenantId, setting.UserId, setting.Name, setting.Value);
        }
    }
}
