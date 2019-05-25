using System;
using System.Collections.Generic;
using System.Text;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Caching;
using Abp.EntityFramework;
using Castle.MicroKernel.Registration;
using Master.Cache;

namespace Master.EntityFrameworkCore.CacheRegistrar
{
    public class EntityCoreCacheRegistrar : IEntityCoreCacheRegistrar,ITransientDependency
    {
        private readonly IDbContextEntityFinder _dbContextEntityFinder;
        public EntityCoreCacheRegistrar(IDbContextEntityFinder dbContextEntityFinder)
        {
            _dbContextEntityFinder = dbContextEntityFinder;
        }
        public void RegisterForDbContext(Type dbContextType, IIocManager iocManager)
        {
            foreach (var entityTypeInfo in _dbContextEntityFinder.GetEntityTypeInfos(dbContextType))
            {
                var repositoryInterface = typeof(IEntityCoreCache<>);
                var repositoryImplementation = typeof(EntityCoreCache<>);
                var primaryKeyType = EntityHelper.GetPrimaryKeyType(entityTypeInfo.EntityType);
                if (primaryKeyType == typeof(int))
                {
                    var genericRepositoryType = repositoryInterface.MakeGenericType(entityTypeInfo.EntityType);
                    if (!iocManager.IsRegistered(genericRepositoryType))
                    {
                        var implType = repositoryImplementation.GetGenericArguments().Length == 1
                            ? repositoryImplementation.MakeGenericType(entityTypeInfo.EntityType)
                            : repositoryImplementation.MakeGenericType(entityTypeInfo.DeclaringType,
                                entityTypeInfo.EntityType);

                        iocManager.IocContainer.Register(
                            Component
                                .For(genericRepositoryType)
                                .ImplementedBy(implType)
                                .Named(Guid.NewGuid().ToString("N"))
                                .LifestyleTransient()
                        );

                        iocManager.Register(implType, DependencyLifeStyle.Transient);
                    }
                }

                //var genericRepositoryTypeWithPrimaryKey = repositoryInterfaceWithPrimaryKey.MakeGenericType(entityTypeInfo.EntityType, primaryKeyType);
                //if (!iocManager.IsRegistered(genericRepositoryTypeWithPrimaryKey))
                //{
                //    var implType = repositoryImplementationWithPrimaryKey.GetGenericArguments().Length == 2
                //        ? repositoryImplementationWithPrimaryKey.MakeGenericType(entityTypeInfo.EntityType, primaryKeyType)
                //        : repositoryImplementationWithPrimaryKey.MakeGenericType(entityTypeInfo.DeclaringType, entityTypeInfo.EntityType, primaryKeyType);

                //    iocManager.IocContainer.Register(
                //        Component
                //            .For(genericRepositoryTypeWithPrimaryKey)
                //            .ImplementedBy(implType)
                //            .Named(Guid.NewGuid().ToString("N"))
                //            .LifestyleTransient()
                //    );
                //}
            }
        }
    }
}
