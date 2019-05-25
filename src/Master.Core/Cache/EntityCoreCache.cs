using Abp.Domain.Entities;
using Abp.Domain.Entities.Caching;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Cache
{
    public class EntityCoreCache<TEntity> : EntityCache<TEntity, TEntity>, IEntityCoreCache<TEntity>
        where TEntity : class, IEntity<int>
    {
        public EntityCoreCache(ICacheManager cacheManager, IRepository<TEntity> repository)
        : base(cacheManager, repository)
        {

        }

    }
}
