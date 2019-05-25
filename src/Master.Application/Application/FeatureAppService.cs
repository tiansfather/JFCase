using Abp;
using Abp.Application.Features;
using Abp.Authorization;
using Master.Application.Editions;
using Master.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Application
{
    [AbpAuthorize]
    public class FeatureAppService:MasterAppServiceBase
    {
        public EditionManager EditionManager { get; set; }
        public TenantManager TenantManager { get; set; }
        public IFeatureManager FeatureManager { get; set; }
        /// <summary>
        /// 设置特性
        /// </summary>
        /// <param name="featureSubmitDto"></param>
        /// <returns></returns>
        public virtual async Task SubmitFeature(FeatureSubmitDto featureSubmitDto)
        {
            var allFeatures = FeatureManager.GetAll();
            var nameValueList =  featureSubmitDto.Values.Select(o => new NameValue(o.Key, o.Value)).ToList();
            
            //前台switch off状态会提交不过来,所以对于不存在的键直接设成空
            foreach(var feature in allFeatures.Where(o => !featureSubmitDto.Values.ContainsKey(o.Name)))
            {
                nameValueList.Add(new NameValue(feature.Name, ""));
            }

            if (featureSubmitDto.Type == "edition")
            {
                await EditionManager.SetFeatureValuesAsync(featureSubmitDto.Data, nameValueList.ToArray());
            }else if (featureSubmitDto.Type == "tenant")
            {
                await TenantManager.SetFeatureValuesAsync(featureSubmitDto.Data, nameValueList.ToArray());
            }
        }

        /// <summary>
        /// 重置账套特性
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public virtual async Task ResetTenantFeature(int tenantId)
        {
            await TenantManager.ResetAllFeaturesAsync(tenantId);
        }
    }
}
