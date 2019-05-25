using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.EntityFramework;
using Common;
using Master.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Master.EntityFrameworkCore.EntityFinder
{
    public class MasterDbContextEntityFinder : IDbContextEntityFinder, ITransientDependency
    {
        public IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type dbContextType)
        {
            //显示定义到DbSet的类型
            var entityTypes =
                (from property in dbContextType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                 where
                     ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>)) &&
                     ReflectionHelper.IsAssignableToGenericType(property.PropertyType.GenericTypeArguments[0], typeof(IEntity<>))
                 select new EntityTypeInfo(property.PropertyType.GenericTypeArguments[0], property.DeclaringType)).ToList();
            //动态继承了IAutoEntity的类型
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in asm.GetTypes().Where(o => typeof(IAutoEntity).IsAssignableFrom(o) && o.IsClass && !o.IsAbstract))
                {
                    if (entityTypes.Count(o => o.EntityType == type) == 0)
                    {
                        entityTypes.Add(new EntityTypeInfo(type, type.DeclaringType));
                    }
                }
            }

            return entityTypes;
        }
    }
}
