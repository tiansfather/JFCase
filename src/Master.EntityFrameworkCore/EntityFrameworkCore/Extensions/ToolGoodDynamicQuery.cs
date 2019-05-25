using Abp.Runtime.Session;
using Master.Domain;
using Master.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using ToolGood.ReadyGo3;

namespace Master.EntityFrameworkCore.Extensions
{
    public class ToolGoodDynamicQuery : IDynamicQuery
    {
        private readonly IDbPerTenantConnectionStringResolver _connectionStringResolver;
        public IAbpSession AbpSession { get; set; }
        public ToolGoodDynamicQuery(IDbPerTenantConnectionStringResolver connectionStringResolver)
        {
            _connectionStringResolver = connectionStringResolver;
            AbpSession = NullAbpSession.Instance;
        }

        private string GetDbConnString()
        {
            return _connectionStringResolver.GetNameOrConnectionString(new DbPerTenantConnectionStringResolveArgs(AbpSession.TenantId));
        }

        public int Execute(string sql,params object[] args)
        {
            using (var helper = GetSqlHelper())
            {
                return helper.Execute(sql, args);
            }
        }

        public Task<int> ExecuteAsync(string sql, params object[] args)
        {
            using (var helper = GetSqlHelper())
            {
                return helper.ExecuteAsync(sql, args);
            }
        }
        public DataTable ExecuteDataTable(string sql, params object[] args)
        {
            using (var helper = GetSqlHelper())
            {               
                return helper.ExecuteDataTable(sql, args);
            }
        }
        public Task<DataTable> ExecuteDataTableAsync(string sql, params object[] args)
        {
            using (var helper = GetSqlHelper())
            {
                return helper.ExecuteDataTableAsync(sql, args);
            }
        }
        public List<dynamic> Select(string sql, params object[] args)
        {

            using (var helper = GetSqlHelper())
            {
                return helper.Select<dynamic>(sql, args);
            }
        }
        public List<T> Select<T>(string sql, params object[] args)
        {
            using (var helper = GetSqlHelper())
            {
                return helper.Select<T>(sql, args);
            }
        }
        public Task<List<T>> SelectAsync<T>(string sql, params object[] args)
        {
            using (var helper = GetSqlHelper())
            {
                return helper.SelectAsync<T>(sql, args);
            }
        }
        public T Single<T>(string sql, params object[] args)
        {
            using (var helper = GetSqlHelper())
            {
                return helper.Single<T>(sql, args);
            }
        }
        public Task<T> SingleAsync<T>(string sql, params object[] args)
        {
            using (var helper = GetSqlHelper())
            {
                return helper.SingleAsync<T>(sql, args);
            }
        }
        public T SingleOrDefault<T>(string sql, params object[] args)
        {
            using (var helper = GetSqlHelper())
            {
                return helper.SingleOrDefault<T>(sql, args);
            }
        }
        public Task<T> SingleOrDefaultAsync<T>(string sql, params object[] args)
        {
            using (var helper = GetSqlHelper())
            {
                return helper.SingleOrDefaultAsync<T>(sql,args);
            }
        }
        public T First<T>(string sql, params object[] args)
        {
            using (var helper = GetSqlHelper())
            {
                return helper.First<T>(sql, args);
            }
        }
        public Task<T> FirstAsync<T>(string sql, params object[] args)
        {
            using (var helper = GetSqlHelper())
            {
                return helper.FirstAsync<T>(sql, args);
            }
        }
        public T FirstOrDefault<T>(string sql, params object[] args)
        {
            using (var helper = GetSqlHelper())
            {
                return helper.FirstOrDefault<T>(sql, args);
            }
        }
        public Task<T> FirstOrDefaultAsync<T>(string sql, params object[] args)
        {
            using (var helper = GetSqlHelper())
            {
                return helper.FirstOrDefaultAsync<T>(sql, args);
            }
        }
        public SqlHelper GetSqlHelper()
        {
            var connStr = GetDbConnString();
            var helper= SqlHelperFactory.OpenDatabase(connStr, "MySql.Data.MySqlClient", ToolGood.ReadyGo3.SqlType.MySql);
            helper._Config.Select_Single_With_Limit_2 = false;
            helper._Config.Select_First_With_Limit_1 = false;
            return helper;
        }
    }
}
