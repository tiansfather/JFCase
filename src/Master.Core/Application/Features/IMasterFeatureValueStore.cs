using Abp.Application.Features;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Application.Features
{
    public interface IMasterFeatureValueStore:  IFeatureValueStore,ITransientDependency
    {
        Task<string> GetValueOrNullAsync(int tenantId, string featureName);
        Task<string> GetEditionValueOrNullAsync(int editionId, string featureName);
        Task SetEditionFeatureValueAsync(int editionId, string featureName, string value);
    }
}
