using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Master.Domain
{
    public interface IDynamicQuery : ITransientDependency
    {
        /// <summary>
        /// 动态查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        List<dynamic> Select(string sql, params object[] args);
        List<T> Select<T>(string sql, params object[] args);
        Task<List<T>> SelectAsync<T>(string sql, params object[] args);
        T Single<T>(string sql, params object[] args);
        Task<T> SingleAsync<T>(string sql, params object[] args);
        T SingleOrDefault<T>(string sql, params object[] args);
        Task<T> SingleOrDefaultAsync<T>(string sql, params object[] args);
        T First<T>(string sql, params object[] args);
        Task<T> FirstAsync<T>(string sql, params object[] args);
        T FirstOrDefault<T>(string sql, params object[] args);
        Task<T> FirstOrDefaultAsync<T>(string sql, params object[] args);

        int Execute(string sql, params object[] args);
        Task<int> ExecuteAsync(string sql, params object[] args);
        DataTable ExecuteDataTable(string sql, params object[] args);
        Task<DataTable> ExecuteDataTableAsync(string sql, params object[] args);
    }
}
