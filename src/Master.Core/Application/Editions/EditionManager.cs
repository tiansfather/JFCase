using Abp;
using Abp.Application.Features;
using Abp.Collections.Extensions;
using Abp.UI;
using Master.Application.Editions;
using Master.Application.Features;
using Master.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Application.Editions
{
    public class EditionManager : DomainServiceBase<Edition, int>
    {
        private readonly IMasterFeatureValueStore _featureValueStore;

        public IQueryable<Edition> Editions => Repository.GetAll();

        public IFeatureManager FeatureManager { get; set; }

        public EditionManager(
            IMasterFeatureValueStore featureValueStore)
        {
            _featureValueStore = featureValueStore;
        }

        public override async Task ValidateEntity(Edition entity)
        {
            //不允许相同名称存在
            if (entity.Id > 0 && await Repository.CountAsync(o => (o.Name==entity.Name || o.DisplayName == entity.DisplayName) && o.Id != entity.Id) > 0)
            {
                throw new UserFriendlyException(L("相同名称已存在"));
            }
            if (entity.Id == 0 && await Repository.CountAsync(o => (o.Name == entity.Name || o.DisplayName == entity.DisplayName)) > 0)
            {
                throw new UserFriendlyException(L("相同名称已存在"));
            }
        }

        public virtual Task<string> GetFeatureValueOrNullAsync(int editionId, string featureName)
        {
            return _featureValueStore.GetEditionValueOrNullAsync(editionId, featureName);
        }

        public virtual Task SetFeatureValueAsync(int editionId, string featureName, string value)
        {
            return _featureValueStore.SetEditionFeatureValueAsync(editionId, featureName, value);
        }

        public virtual async Task<IReadOnlyList<NameValue>> GetFeatureValuesAsync(int editionId)
        {
            var values = new List<NameValue>();

            foreach (var feature in FeatureManager.GetAll())
            {
                values.Add(new NameValue(feature.Name, await GetFeatureValueOrNullAsync(editionId, feature.Name) ?? feature.DefaultValue));
            }

            return values;
        }

        public virtual async Task SetFeatureValuesAsync(int editionId, params NameValue[] values)
        {
            if (values.IsNullOrEmpty())
            {
                return;
            }

            foreach (var value in values)
            {
                await SetFeatureValueAsync(editionId, value.Name, value.Value);
            }
        }


        public virtual Task<Edition> FindByNameAsync(string name)
        {
            return Repository.FirstOrDefaultAsync(edition => edition.Name == name);
        }


    }
}
