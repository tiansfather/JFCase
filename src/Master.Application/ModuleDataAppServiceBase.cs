using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.UI;
using Master.Entity;
using Master.Search;
using Master.Module;
using Master.Dto;
using Master.Extension;
using Master.Base;
using Master.Domain;

namespace Master
{
    public abstract class ModuleDataAppServiceBase<TEntity, TPrimaryKey> : MasterAppServiceBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>, IHaveProperty, new()
    {
        public IDynamicSearchParser DynamicSearchParser { get; set; }
        public IDynamicOrderParser DynamicOrderParser { get; set; }
        public IDynamicQuery DynamicQuery { get; set; }
        protected abstract string ModuleKey();

        protected async Task<ModuleInfo> ModuleInfo(RequestDto request = null)
        {
            ModuleInfo moduleInfo = null;
            //如果参数中有模块信息，优先使用参数中的模块
            if (request != null && !string.IsNullOrEmpty(request.ModuleKey))
            {
                moduleInfo = await ModuleManager.GetModuleInfo(request.ModuleKey);
            }
            else
            {
                moduleInfo = await ModuleManager.GetModuleInfo(ModuleKey());
            }
            return moduleInfo;
        }

        #region 分页及数据过滤
        /// <summary>
        /// 重写获取基础查询方法
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override async Task<IQueryable<TEntity>> GetBaseQuery(RequestPageDto request)
        {
            ModuleInfo moduleInfo =await ModuleInfo(request);
            return ModuleManager.GetQuery(moduleInfo) as IQueryable<TEntity>;
        }

        protected override async Task<IQueryable<TEntity>> GetQueryable(RequestPageDto request)
        {
            ModuleInfo moduleInfo = await ModuleInfo(request);
            var query= await base.GetQueryable(request);
            ////模块的查询需要增加高级查询内容
            //if (!request.SearchCondition.IsNullOrWhiteSpace())
            //{
            //    query = DynamicSearchParser.Parse<TEntity>(request.SearchCondition, moduleInfo, query) as IQueryable<TEntity>;
            //}

            if (request.OrderField.IsNullOrEmpty())
            {
                //使用模块排序
                //默认排序
                if (moduleInfo.SortField != "Id")
                {
                    query = DynamicOrderParser.Parse<TEntity>(moduleInfo.SortField, moduleInfo.SortType, moduleInfo, query) as IQueryable<TEntity>;
                }
                else
                {
                    query = query.OrderBy($"{moduleInfo.SortField} {moduleInfo.SortType.ToString()}");
                }
            }
            return query;
        }
        /// <summary>
        /// 模块的高级查询
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected override async Task<IQueryable<TEntity>> BuildDynamicSearchQueryAsync(RequestPageDto requestPageDto, IQueryable<TEntity> query)
        {
            ModuleInfo moduleInfo = await ModuleInfo(requestPageDto);
            return DynamicSearchParser.Parse<TEntity>(requestPageDto.SearchCondition, moduleInfo, query) as IQueryable<TEntity>;
        }
        /// <summary>
        /// soultable查询接入
        /// </summary>
        /// <param name="requestPageDto"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected override async Task<IQueryable<TEntity>> BuildSoulTableSearchQueryAsync(RequestPageDto requestPageDto, IQueryable<TEntity> query)
        {
            ModuleInfo moduleInfo = await ModuleInfo(requestPageDto);
            return DynamicSearchParser.ParseSoulTable<TEntity>(requestPageDto.FilterSos, moduleInfo, query) as IQueryable<TEntity>;
        }
        /// <summary>
        /// 模块分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [DontWrapResult]
        public override async Task<ResultPageDto> GetPageResult(RequestPageDto request)
        {
            ModuleInfo moduleInfo = await ModuleInfo(request);
            //如果有过滤字段则需要返回过滤字段对应的数据
            if (!string.IsNullOrEmpty(request.FilterColumns))
            {
                var filterColumns = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(request.FilterColumns);
                var query = await GetQueryable(request);
                var fitlerColumnResult = await GetFilterColumnsResult(moduleInfo, filterColumns, query);
                return new ResultPageDto()
                {
                    data= fitlerColumnResult
                };
                
            }
            //var query =await GetBaseQuery(request);
            ////五种查询
            ////1.where里的查询是直接lamda查询
            //if (!request.Where.IsNullOrWhiteSpace())
            //{
            //    query = query.Where(request.Where);
            //}
            ////2.高级查询，通过高级查询表单提交过来的数据
            //if (!request.SearchCondition.IsNullOrWhiteSpace())
            //{
            //    query = DynamicSearchParser.Parse<TEntity>(request.SearchCondition, moduleInfo, query);
            //}
            ////3.内置查询,写死在页面上的查询过滤
            //if (!request.SearchKeys.IsNullOrWhiteSpace())
            //{
            //    query = await BuildSearchQueryAsync(Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, string>>(request.SearchKeys), query as IQueryable<TEntity>);
            //}
            ////4.表头过滤
            //if (!request.TableFilter.IsNullOrWhiteSpace())
            //{
            //    query = await BuildTableFilterQueryAsync(Newtonsoft.Json.JsonConvert.DeserializeObject<List<FilterColumnDto>>(request.TableFilter), query as IQueryable<TEntity>);
            //}
            ////5.关键字查询,一般用于字段引用时的下拉查询
            //if (!request.Keyword.IsNullOrEmpty())
            //{
            //    query = await BuildKeywordQueryAsync(request.Keyword, query as IQueryable<TEntity>);
            //}
            //if (!request.OrderField.IsNullOrWhiteSpace())
            //{
            //    query = DynamicOrderParser.Parse<TEntity>(request.OrderField, request.OrderType?.ToLower() == "asc" ? SortType.Asc : SortType.Desc, moduleInfo, query);
            //}
            //else
            //{
            //    //默认排序
            //    if (moduleInfo.SortField != "Id")
            //    {
            //        query = DynamicOrderParser.Parse<TEntity>(moduleInfo.SortField, moduleInfo.SortType, moduleInfo, query);
            //    }
            //    else
            //    {
            //        query = query.OrderBy($"{moduleInfo.SortField} {moduleInfo.SortType.ToString()}");
            //    }


            //}
            //var pageResult = query.PageResult(request.Page, request.Limit);
            var pageResult = await GetPageResultQueryable(request);

            var dataResult = await ModuleManager.GetModuleDataListAsync(moduleInfo, pageResult.Queryable);

            var result = new ResultPageDto()
            {
                code = 0,
                count = pageResult.RowCount,
                data = dataResult
            };

            return result;
        }

        private async Task<object> GetFilterColumnsResult(ModuleInfo moduleInfo,List<string> filterColumns,IQueryable<TEntity> queryable)
        {
            var result = new Dictionary<string,object>();
            var manager = Manager as ModuleServiceBase<TEntity, TPrimaryKey>;
            foreach(var columnKey in filterColumns)
            {
                var column = moduleInfo.ColumnInfos.SingleOrDefault(o => o.ColumnKey.ToLower() == columnKey.ToLower());
                if (column != null && column.IsShownInAdvanceSearch)
                {
                    object data;
                    //直接数据列
                    if (column.IsDirectiveColumn)
                    {
                        var fieldPath = column.ColumnKey;
                        if (!string.IsNullOrEmpty(column.ValuePath))
                        {
                            fieldPath = column.ValuePath;
                        }
                        if (!string.IsNullOrEmpty(column.DisplayPath))
                        {
                            fieldPath = column.DisplayPath;
                        }
                        result.Add(columnKey, (await queryable
                            .GroupBy(fieldPath)
                            .Select($"new(Key)")
                            .Take(200)
                            .ToDynamicListAsync())
                            .Where(o => o.Key!=null).Select(o =>
                                {
                                    //数字和日期进行格式化后输出
                                    if (column.ColumnType == ColumnTypes.Number || column.ColumnType==ColumnTypes.DateTime)
                                    {
                                        return o.Key.ToString(column.DisplayFormat);
                                    }
                                    return o.Key.ToString();
                                })

                            );
                    }
                    else if (column.IsPropertyColumn)
                    {
                        //属性列
                        result.Add(columnKey, DynamicQuery.Select($"select property->>\"$.{column.ColumnKey}\" as Value from {typeof(TEntity).Name} where tenantId={AbpSession.TenantId.Value} and property->>\"$.{column.ColumnKey}\"!='' and property->>\"$.{column.ColumnKey}\" is not null   and isdeleted=0 GROUP BY Value order by Value limit 200").Where(o => !string.IsNullOrEmpty(o.Value)).Select(o => o.Value.ToString()).Where(o=>o!=null));
                    }
                }
            }

            return result;
            
        }

        /// <summary>
        /// 输入提示接口
        /// </summary>
        /// <param name="requestSuggestDto"></param>
        /// <returns></returns>
        [DontWrapResult]
        public override async Task<ResultDto> GetSuggestResult(RequestSuggestDto requestSuggestDto)
        {
            var result = new ResultDto();

            var moduleInfo = await ModuleInfo(requestSuggestDto);
            var manager = Manager as ModuleServiceBase<TEntity, TPrimaryKey>;
            var column = moduleInfo.ColumnInfos.SingleOrDefault(o => o.ColumnKey == requestSuggestDto.ColumnKey);
            if (column != null)
            {
                //直接数据列
                if (column.IsDirectiveColumn)
                {
                    result.data=await manager.GetFilteredQuery(moduleInfo.ModuleKey)
                        .Where($"{requestSuggestDto.ColumnKey}.Contains(\"{requestSuggestDto.Keyword}\")")
                        .GroupBy(requestSuggestDto.ColumnKey)                       
                        .Select($"new(Key as {requestSuggestDto.ColumnKey})")
                        .OrderBy(requestSuggestDto.ColumnKey)
                        .Take(20)
                        .ToDynamicListAsync();
                }else if (column.IsPropertyColumn)
                {
                    //属性列
                    result.data = DynamicQuery.Select($"select property->>\"$.{requestSuggestDto.ColumnKey}\" as {requestSuggestDto.ColumnKey} from {typeof(TEntity).Name} where tenantId={AbpSession.TenantId.Value} and property->>\"$.{requestSuggestDto.ColumnKey}\"!='' and property->>\"$.{requestSuggestDto.ColumnKey}\" is not null  and property->>\"$.{requestSuggestDto.ColumnKey}\"  like '%{requestSuggestDto.Keyword}%' and isdeleted=0 GROUP BY {requestSuggestDto.ColumnKey} order by {requestSuggestDto.ColumnKey} limit 20");
                }
            }

            return result;
        }
        #endregion

        #region 表单提交
        public override async Task FormSubmit(FormSubmitRequestDto request)
        {
            var moduleInfo = await ModuleInfo(request);
            //通用模块增加修改表单提交
            //暂只支持添加和修改的提交
            if (request.Action == "Add")
            {
                await ModuleManager.ManageFormAdd(moduleInfo, request.Datas);
            }
            else if (request.Action == "Edit")
            {
                var id = Convert.ToInt32(request.Datas.GetDataOrException("ids"));
                await ModuleManager.ManageFormEdit(moduleInfo, request.Datas, id);
            }
            else if (request.Action == "MultiEdit")
            {
                var ids = request.Datas.GetDataOrException("ids").Split(',').ToList().ConvertAll(o => int.Parse(o));
                await ModuleManager.ManageFormMultiEdit(moduleInfo, request.Datas, ids);
            }
            else
            {
                throw new UserFriendlyException("Only Add , Edit ,MultiEdit form available in common module-formsubmit handling");
            }
        }

        #endregion


    }
}
