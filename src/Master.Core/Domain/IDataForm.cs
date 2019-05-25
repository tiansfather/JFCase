using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Domain
{
    /// <summary>
    /// 数据接口,实现类为DomainServiceBase
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IData<TEntity, TPrimaryKey> : IDomainService
        where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        IRepository<TEntity, TPrimaryKey> Repository { get; set; }

        Task<TEntity> GetByIdAsync(TPrimaryKey id);
        Task<TEntity> GetByIdFromCacheAsync(int id);

        Task<IEnumerable<TEntity>> GetListByIdsAsync(IEnumerable<TPrimaryKey> ids);

        Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task SaveAsync(TEntity entity);
        Task InsertAsync(TEntity entity);
        Task ValidateEntity(TEntity entity);
        IQueryable<TEntity> GetAll();
        Task DeleteAsync(IEnumerable<TPrimaryKey> ids);
        Task DeleteAsync(TEntity entity);
    }

    /// <summary>
    /// 通用数据提交接口
    /// </summary>
    public interface IForm<TEntity, TPrimaryKey>
    {
        /// <summary>
        /// 通用添加表单提交
        /// </summary>
        /// <param name="moduleKey"></param>
        /// <param name="Datas"></param>
        /// <returns></returns>
        Task<TEntity> DoAdd(IDictionary<string, string> Datas);
        /// <summary>
        /// 通用修改表单提交
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <param name="Datas"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<TEntity> DoEdit(IDictionary<string, string> Datas, TPrimaryKey id);
    }


}



