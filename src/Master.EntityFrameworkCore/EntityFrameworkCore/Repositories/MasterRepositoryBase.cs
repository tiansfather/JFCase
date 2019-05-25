using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Repositories;
using Master.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.EntityFrameworkCore.Repositories
{
    public class MasterRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<MasterDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public MasterRepositoryBase(IDbContextProvider<MasterDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
        // Add your common methods for all repositories
    }

    public class MasterRepositoryBase<TEntity> : MasterRepositoryBase<TEntity, int>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        public MasterRepositoryBase(IDbContextProvider<MasterDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        // Do not add any method here, add to the class above (since this inherits it)!!!
    }
}
