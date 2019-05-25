using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Reflection;
using Master.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Master.EntityFrameworkCore.Extensions
{
    public static class RepositoryExtensions
    {

        public static IQueryable<TEntity> GetAll<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, string sql)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var repo = ProxyHelper.UnProxy(repository) as MasterRepositoryBase<TEntity, TPrimaryKey>;
            if (repo != null)
            {
                return repo.Context.Set<TEntity>().FromSql(sql);
            }
            throw new ArgumentException($"Given {nameof(repository)} is not inherited from {typeof(MasterRepositoryBase<TEntity, TPrimaryKey>).AssemblyQualifiedName}");
        }
        public static IQueryable<TEntity> GetAll<TEntity>(this IRepository<TEntity> repository, string sql)
            where TEntity : class, IEntity<int>
        {
            var repo = repository as IRepository<TEntity, int>;
            return GetAll(repo, sql);
        }

        public static Task EnsureCollectionLoadedAsync<TEntity, TProperty>(
            this IRepository<TEntity> repository,
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> collectionExpression,
            CancellationToken cancellationToken = default(CancellationToken)
        )
            where TEntity : class, IEntity<int>
            where TProperty : class
        {
            var repo = repository as IRepository<TEntity, int>;
            return repo.EnsureCollectionLoadedAsync(entity, collectionExpression, cancellationToken);
        }

        public static Task EnsurePropertyLoadedAsync<TEntity, TProperty>(
            this IRepository<TEntity> repository,
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken = default(CancellationToken)
        )
            where TEntity : class, IEntity<int>
            where TProperty : class
        {
            var repo = repository as IRepository<TEntity, int>;
            return repo.EnsurePropertyLoadedAsync(entity, propertyExpression, cancellationToken);
        }
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, string where) where T : class
        {
            return condition ? source.Where(where) : source;
        }
    }
}
