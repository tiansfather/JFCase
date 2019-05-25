using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Master.Domain
{
    /// <summary>
    /// 模块的数据接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityProperty"></typeparam>
    public interface IDataModule<TEntity, TPrimaryKey> : IData<TEntity, TPrimaryKey>, IFormModule
        where TEntity : class, IEntity<TPrimaryKey>, new()
    {

        /// <summary>
        /// 获取已增加过滤器和默认模块筛选器的模块查询器
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetFilteredQuery(string moduleKey = "");
        /// <summary>
        /// 获取空的查询器（如果没有自定义排序，请一直使用GetFilteredQueryAsync方法）
        /// </summary>
        /// <returns></returns>
        //IQueryable<TEntity> GetUnFilterdQuery();
        /// <summary>
        /// 获取模块扁平化数据
        /// </summary>
        /// <param name="moduleKey"></param>
        /// <param name="whereCondition"></param>
        /// <returns></returns>
        Task<IEnumerable<IDictionary<string, object>>> GetModulePlainList(string moduleKey, string whereCondition);
        /// <summary>
        /// 获取模块扁平化数据
        /// </summary>
        /// <param name="moduleKey"></param>
        /// <param name="whereCondition"></param>
        /// <returns></returns>
        Task<IEnumerable<IDictionary<string, object>>> GetModulePlainList(string moduleKey, Expression<Func<TEntity, bool>> whereCondition);
    }

    /// <summary>
    /// 模块的数据提交接口
    /// </summary>
    public interface IFormModule
    {
        /// <summary>
        /// 通用添加表单提交
        /// </summary>
        /// <param name="moduleKey"></param>
        /// <param name="Datas"></param>
        /// <returns></returns>
        Task<object> DoAdd(ModuleInfo moduleInfo, IDictionary<string, string> Datas);
        /// <summary>
        /// 通用修改表单提交
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <param name="Datas"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<object> DoEdit(ModuleInfo moduleInfo, IDictionary<string, string> Datas, object id);
        /// <summary>
        /// 通用批量修改表单提交
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <param name="Datas"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<object> DoMultiEdit(ModuleInfo moduleInfo, IDictionary<string, string> Datas, IEnumerable<object> ids);
    }
}
