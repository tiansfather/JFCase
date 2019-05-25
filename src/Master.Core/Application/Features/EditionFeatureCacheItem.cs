using System;
using System.Collections.Generic;

namespace Master.Application.Features
{
    [Serializable]
    public class EditionFeatureCacheItem
    {
        public const string CacheStoreName = "MasterEditionFeatures";

        public IDictionary<string, string> FeatureValues { get; set; }

        public EditionFeatureCacheItem()
        {
            FeatureValues = new Dictionary<string, string>();
        }
    }
}