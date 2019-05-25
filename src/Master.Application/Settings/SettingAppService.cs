using Abp.Authorization;
using Abp.Configuration;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Settings
{
    [AbpAuthorize]
    public class SettingAppService:MasterAppServiceBase
    {
        /// <summary>
        /// 更新账套的设置
        /// </summary>
        /// <param name="settingDtos"></param>
        /// <returns></returns>
        public virtual async Task UpdateSettings(List<SettingDto> settingDtos)
        {
            var settingDefinitions = Resolve<ISettingDefinitionManager>().GetAllSettingDefinitions();
            foreach(var settingDto in settingDtos)
            {
                var settingDefinition = settingDefinitions.Single(o => o.Name == settingDto.Name);
                if (settingDefinition.Scopes == SettingScopes.Application)
                {
                    await SettingManager.ChangeSettingForApplicationAsync(settingDto.Name, settingDto.Value);
                }else if (settingDefinition.Scopes == SettingScopes.Tenant)
                {
                    await SettingManager.ChangeSettingForTenantAsync(AbpSession.TenantId.Value, settingDto.Name, settingDto.Value);
                }
                else
                {
                    await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), settingDto.Name, settingDto.Value);
                }

            }
            
        }
    }
}
