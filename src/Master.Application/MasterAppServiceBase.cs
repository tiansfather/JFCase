using Abp.Application.Services;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Extensions;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using Master.Authentication;
using Master.Base;
using Master.Domain;
using Master.Dto;
using Master.Entity;
using Master.Extension;
using Master.Module;
using Master.MultiTenancy;
using Master.Organizations;
using Master.Search;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Master
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class MasterAppServiceBase : ApplicationService
    {
        public IHostingEnvironment HostingEnvironment { get; set; }
        public ICacheManager CacheManager { get; set; }
        //public IDynamicSearchParser DynamicSearchParser { get; set; }
        //public TenantManager TenantManager { get; set; }
        
        //public UserManager UserManager { get; set; }
        /// <summary>
        /// 动态sql查询
        /// </summary>
        public IDynamicQuery DynamicQuery { get; set; }
        protected MasterAppServiceBase()
        {
            LocalizationSourceName = MasterConsts.LocalizationSourceName;
        }

        protected TEntity Resolve<TEntity>()
        {
            using (var objectWrapper = IocManager.Instance.ResolveAsDisposable<TEntity>())
            {
                return objectWrapper.Object;
            }
        }

        #region 当前登录用户及账套信息
        protected virtual Task<User> GetCurrentUserAsync()
        {
            var userManager = Resolve<UserManager>();
            var user = userManager.GetByIdAsync(AbpSession.GetUserId());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }
        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            var tenantManager = Resolve<TenantManager>();
            return tenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }
        #endregion


    }

    /// <summary>
    /// appservice层基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimary"></typeparam>
    public abstract class MasterAppServiceBase<TEntity, TPrimary> : MasterAppServiceBase
        where TEntity : class, IEntity<TPrimary>, new()
    {
        public IRepository<TEntity, TPrimary> Repository { get; set; }

        public IModuleInfoManager ModuleManager { get; set; }


        #region Manager
        /// <summary>
        /// 获取实体的管理类
        /// </summary>
        protected virtual DomainServiceBase<TEntity, TPrimary> Manager
        {
            get
            {
                return ModuleManager.GetManager(typeof(TEntity)) as DomainServiceBase<TEntity, TPrimary>;
                //return DomainServiceBase.GetEntityManager(typeof(TEntity)) as DomainServiceBase<TEntity, TPrimary>;
            }
        }
        #endregion

        #region 分页与数据过滤接口
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [DontWrapResult]
        public async virtual Task<ResultPageDto> GetPageResult(RequestPageDto request)
        {

            var pageResult = await GetPageResultQueryable(request);

            var result = new ResultPageDto()
            {
                code = 0,
                count = pageResult.RowCount,
                data = (await pageResult.Queryable.ToListAsync()).ConvertAll(PageResultConverter)
            };

            return result;
        }
        /// <summary>
        /// 获取分页后的查询IQueryable
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual async Task<PagedResult<TEntity>> GetPageResultQueryable(RequestPageDto request)
        {
            var query = await GetQueryable(request);
            var pageResult = query.PageResult(request.Page, request.Limit);
            return pageResult;
        }
        /// <summary>
        /// 获取基础查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TEntity>> GetBaseQuery(RequestPageDto request)
        {
            return Manager.GetAll();
        }

        /// <summary>
        /// 获取查询IQueryable
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TEntity>> GetQueryable(RequestPageDto request)
        {
            if (request.Page <= 0)
            {
                request.Page = 1;
            }
            if (request.Limit <= 0)
            {
                request.Limit = 20;
            }
            var query =await GetBaseQuery(request);
            //1.where里的查询是直接lamda查询
            if (!request.Where.IsNullOrWhiteSpace())
            {
                //query = query.Where(request.Where);
                query = await BuildWhereQueryAsync(request.Where, query);
            }
            //2.内置查询,写死在页面上的查询过滤
            if (!request.SearchKeys.IsNullOrWhiteSpace())
            {
                query = await BuildSearchQueryAsync(Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, string>>(request.SearchKeys), query);
            }
            //3.表格过滤
            if (!request.TableFilter.IsNullOrWhiteSpace())
            {
                query = await BuildTableFilterQueryAsync(Newtonsoft.Json.JsonConvert.DeserializeObject<List<FilterColumnDto>>(request.TableFilter), query);
            }
            //4.关键字查询,一般用于字段引用时的下拉查询
            if (!request.Keyword.IsNullOrEmpty())
            {
                query = await BuildKeywordQueryAsync(request.Keyword, query );
            }
            //5.高级查询内容
            if (!request.SearchCondition.IsNullOrWhiteSpace()||!request.FilterSos.IsNullOrWhiteSpace())
            {
                query = await BuildDynamicSearchQueryAsync(request, query);
            }
            if (!request.FilterSos.IsNullOrWhiteSpace())
            {
                query = await BuildSoulTableSearchQueryAsync(request, query);
            }
            query = await BuildOrderQueryAsync(request, query);
            //if (!request.OrderField.IsNullOrWhiteSpace())
            //{
            //    //提交过来的排序
            //    query = query.OrderBy($"{request.OrderField} {request.OrderType}");
            //}

            return query;
        }
        protected virtual async Task<IQueryable<TEntity>> BuildOrderQueryAsync(RequestPageDto request, IQueryable<TEntity> query)
        {
            if (!request.OrderField.IsNullOrWhiteSpace())
            {
                //提交过来的排序
                query = query.OrderBy($"{request.OrderField} {request.OrderType}");
            }
            return query;
        }
        /// <summary>
        /// 直接where语句查询构建,where:"ProjectSN.Contains(\"a\") or Property[ProjectCharger]=\"张一\""
        /// </summary>
        /// <param name="where"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TEntity>> BuildWhereQueryAsync(string where, IQueryable<TEntity> query)
        {
            return Resolve<IDynamicSearchParser>().ParseWhere<TEntity>(where, query) as IQueryable<TEntity>;
        }
        /// <summary>
        /// 关键字查询
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TEntity>> BuildKeywordQueryAsync(string keyword, IQueryable<TEntity> query)
        {
            return await Task.FromResult(query);
        }
        /// <summary>
        /// 通过客户端查询条件返回查询,在派生类中重写
        /// </summary>
        /// <param name="searchKeys"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TEntity>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<TEntity> query)
        {
            return await Task.FromResult(query);
        }
        /// <summary>
        /// 构建高级查询
        /// </summary>
        /// <param name="requestPageDto"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TEntity>> BuildDynamicSearchQueryAsync(RequestPageDto requestPageDto, IQueryable<TEntity> query)
        {
            return Resolve<IDynamicSearchParser>().Parse<TEntity>(requestPageDto.SearchCondition, null, query) as IQueryable<TEntity>;
        }
        /// <summary>
        /// SoulTable高级查询
        /// </summary>
        /// <param name="requestPageDto"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TEntity>> BuildSoulTableSearchQueryAsync(RequestPageDto requestPageDto, IQueryable<TEntity> query)
        {
            return Resolve<IDynamicSearchParser>().ParseSoulTable<TEntity>(requestPageDto.FilterSos, null, query) as IQueryable<TEntity>;
        }
        /// <summary>
        /// 构建表单过滤查询条件
        /// </summary>
        /// <param name="filterColumnDtos"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TEntity>> BuildTableFilterQueryAsync(List<FilterColumnDto> filterColumnDtos, IQueryable<TEntity> query )
        {
            foreach(var filterColumnDto in filterColumnDtos)
            {
                var strWhere = filterColumnDto.GetWhereStr();
                if (!string.IsNullOrEmpty(strWhere))
                {
                    query = query.Where(strWhere);
                }
                
            }

            return query;
        }
        /// <summary>
        /// 分页数据处理
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual object PageResultConverter(TEntity entity)
        {
            return entity;
        }
        /// <summary>
        /// 获取字段过滤返回数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async virtual Task<object> GetFilterColumnPageResult(RequestPageDto request)
        {
            //todo:bugfix 返回时间的时候会忽略掉天后面部分
            var query = await GetQueryable(request);

            if (!string.IsNullOrWhiteSpace(request.FilterKey))
            {
                query = query.Where($"{request.FilterField}.Contains(\"{request.FilterKey}\")");
            }
            var filterField = request.FilterField;
            if (filterField.IndexOf(".") > 0)
            {
                query = query.Include(filterField.Substring(0, filterField.LastIndexOf('.')));
            }
            
            var result= await query
                .GroupBy(request.FilterField)                
                .Select("new(Key)")
                .Skip((request.Page-1)*request.Limit)
                .Take(request.Limit)
                .ToDynamicListAsync();


            var rt = result.Where(o => o.Key != null).Select(o => o.Key.ToString());

            


            return rt;
            //return await pageResult.Queryable
            //    .IncludeIf(filterField.IndexOf(".") > 0, filterField.Substring(0, filterField.LastIndexOf('.')))
            //    .GroupBy(request.FilterField)
            //    .Select("new(Key)")
            //    .ToDynamicListAsync();
        }

        /// <summary>
        /// 输入提示接口
        /// </summary>
        /// <param name="requestSuggestDto"></param>
        /// <returns></returns>
        [DontWrapResult]
        public virtual async Task<ResultDto> GetSuggestResult(RequestSuggestDto requestSuggestDto)
        {
            var result = new ResultDto();

            result.data = await Manager.GetAll()
                        .Where($"{requestSuggestDto.ColumnKey}.Contains(\"{requestSuggestDto.Keyword}\")")
                        .GroupBy(requestSuggestDto.ColumnKey)
                        .Select($"new(Key as {requestSuggestDto.ColumnKey})")
                        .OrderBy(requestSuggestDto.ColumnKey)
                        .Take(20)
                        .ToDynamicListAsync();

            return result;
        }
        #endregion

        #region 数据获取
        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <param name="primary"></param>
        /// <returns></returns>
        public virtual async Task<object> GetById(TPrimary primary)
        {
            return ResultConverter(await Manager.GetByIdAsync(primary));
        }

        /// <summary>
        /// 获取多个对象
        /// </summary>
        /// <param name="primaries"></param>
        /// <returns></returns>
        public virtual async Task<object> GetListByIds(IEnumerable<TPrimary> primaries)
        {
            return (await Manager.GetListByIdsAsync(primaries)).Select(o=>ResultConverter(o));
        }

        /// <summary>
        /// 获取实体中某字段分组后的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<List<dynamic>> GetGroupedField(string key)
        {
            var result = new List<dynamic>();
            result.AddRange((await Manager.GetAll().Where($"{key}!=null").GroupBy(key).Select("Key").ToDynamicListAsync()).Distinct());

            return result;
        }
        protected virtual object ResultConverter(TEntity entity)
        {
            return entity;
        }

        #endregion

        #region 删除
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async virtual Task DeleteEntity(IEnumerable<TPrimary> ids)
        {
            await Manager.DeleteAsync(ids);
        }
        #endregion

        #region 表单提交 
        public virtual async Task FormSubmit(FormSubmitRequestDto request)
        {
            //通用模块增加修改表单提交
            //暂只支持添加和修改的提交
            if (request.Action == "Add")
            {
                await Manager.DoAdd(request.Datas);
            }
            else if (request.Action == "Edit")
            {
                var id = (TPrimary)Convert.ChangeType(request.Datas.GetDataOrException("ids"), typeof(TPrimary));
                await Manager.DoEdit(request.Datas, id);
            }
        }
        #endregion

        #region 状态标记
        /// <summary>
        /// 实体状态标记
        /// </summary>
        /// <param name="primaries"></param>
        /// <param name="status"></param>
        /// <param name="isSet"></param>
        /// <returns></returns>
        public virtual async Task SetStatus(TPrimary[] primaries, string status, bool isSet)
        {
            if (typeof(IHaveStatus).IsAssignableFrom(typeof(TEntity)))
            {
                var entities = await Manager.GetListByIdsAsync(primaries) ;
                foreach(IHaveStatus entity in entities)
                {
                    if (isSet)
                    {
                        entity.SetStatus(status);
                    }
                    else
                    {
                        entity.RemoveStatus(status);
                    }
                }           
            }
            else
            {
                throw new UserFriendlyException(L("实体不实现IHaveStatus,接口调用失败"));
            }
        } 
        #endregion

    }
}