using Abp.Dependency;
using Master.Entity;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.Search
{
    public interface IDynamicOrderParser: ITransientDependency
    {
        /// <summary>
        /// 动态排序
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="moduleInfo"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable Parse<TEntity>(string orderField, SortType sortType, ModuleInfo moduleInfo, IQueryable query) where TEntity : IHaveProperty;
    }
}
