using Abp.Domain.Entities.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Cache
{
    public interface IEntityCoreCache<TEntity> : IEntityCache<TEntity>
    {
    }
}
