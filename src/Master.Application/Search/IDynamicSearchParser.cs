using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Abp.Dependency;
using Master.Module;

namespace Master.Search
{
    public interface IDynamicSearchParser:ITransientDependency
    {
        /// <summary>
        /// 动态查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="searchConditionStr"></param>
        /// <param name="moduleInfo"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable Parse<TEntity>(string searchConditionStr, ModuleInfo moduleInfo, IQueryable query);
        /// <summary>
        /// 基于SoulTable的动态查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filterSos"></param>
        /// <param name="moduleInfo"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable ParseSoulTable<TEntity>(string filterSos, ModuleInfo moduleInfo, IQueryable query);
        /// <summary>
        /// 构建属性列的lamda表达式
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="columnType"></param>
        /// <param name="columnKey"></param>
        /// <returns></returns>
        LambdaExpression GeneratePropertyLamda<TEntity>(ColumnTypes columnType, string columnKey);
    }
}
