using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.EntityFrameworkCore.CacheRegistrar
{
    public interface IEntityCoreCacheRegistrar
    {
        void RegisterForDbContext(Type dbContextType, IIocManager iocManager);
    }
}
