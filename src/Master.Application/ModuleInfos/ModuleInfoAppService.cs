using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.UI;
using Abp.Web.Models;
using Master.Dto;
using Master.EntityFrameworkCore;
using Master.EntityFrameworkCore.Seed.Tenants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Master.Module
{
    [AbpAuthorize]
    public class ModuleInfoAppService:MasterAppServiceBase<ModuleInfo,int>
    {
        private IModuleInfoManager _moduleManager;
        private IHttpContextAccessor _httpContextAccessor;
        public ModuleInfoAppService(IModuleInfoManager moduleManager, IHttpContextAccessor httpContextAccessor)
        {
            _moduleManager = moduleManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public virtual async Task AddModuleInfo(string moduleName,string moduleKey)
        {
            if (string.IsNullOrEmpty(moduleName))
            {
                throw new UserFriendlyException(L("模块名不能为空"));
            }
            var moduleInfo = new ModuleInfo()
            {
                ModuleKey=moduleKey,
                ModuleName=moduleName
            };
            await _moduleManager.AddModuleInfo(moduleInfo);
        }
        /// <summary>
        /// 获取模块数量汇总
        /// </summary>
        /// <returns></returns>
        public virtual async Task<object> GetModuleCountSummary()
        {
            var tenantId = AbpSession.TenantId;
            //如果是Host登录，从session中获取账套信息
            if (AbpSession.MultiTenancySide == Abp.MultiTenancy.MultiTenancySides.Host)
            {
                tenantId = _httpContextAccessor.HttpContext.Session.Get<int?>("TenantId");
            }
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var query = Manager.GetAll();
                var inCount = query.Where(o => o.IsInterModule).Count();
                var outCount = query.Where(o => !o.IsInterModule).Count();
                var allCount = query.Count();

                return new { allCount = allCount, inCount = inCount, outCount = outCount };
            }
                
        }
        /// <summary>
        /// 列信息
        /// </summary>
        /// <param name="moduleKey"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<ColumnInfoDto>> GetColumnInfos(string moduleKey)
        {
            var tenantId = AbpSession.TenantId;
            //如果是Host登录，从session中获取账套信息
            if (AbpSession.MultiTenancySide == Abp.MultiTenancy.MultiTenancySides.Host)
            {
                tenantId = _httpContextAccessor.HttpContext.Session.Get<int?>("TenantId");
            }
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var moduleInfo = await _moduleManager.GetModuleInfo(moduleKey);
                var columns = ObjectMapper.Map<List<ColumnInfoDto>>(moduleInfo.ColumnInfos.OrderBy(o => o.Sort));
                return columns;
            }
                
        }
        /// <summary>
        /// 按钮信息
        /// </summary>
        /// <param name="moduleKey"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<BtnInfoDto>> GetBtnInfos(string moduleKey)
        {
            var tenantId = AbpSession.TenantId;
            //如果是Host登录，从session中获取账套信息
            if (AbpSession.MultiTenancySide == Abp.MultiTenancy.MultiTenancySides.Host)
            {
                tenantId = _httpContextAccessor.HttpContext.Session.Get<int?>("TenantId");
            }
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var moduleInfo = await _moduleManager.GetModuleInfo(moduleKey);
                var btns = ObjectMapper.Map<List<BtnInfoDto>>(moduleInfo.Buttons.OrderBy(o => o.Sort));
                foreach (var btn in btns)
                {
                    btn.IsForNoneRow = btn.ButtonType.HasFlag(ButtonType.ForNoneRow);
                    btn.IsForSelectedRows = btn.ButtonType.HasFlag(ButtonType.ForSelectedRows);
                    btn.IsForSingleRow = btn.ButtonType.HasFlag(ButtonType.ForSingleRow);
                }
                return btns;
            }
        }
                
        
        /// <summary>
        /// 更新列信息
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="moduleInfoId"></param>
        /// <returns></returns>
        public async Task UpdateColumns(IEnumerable<ColumnInfo> columns,int moduleInfoId)
        {
            var module = await Repository.GetAllIncluding(o => o.ColumnInfos).Where(o => o.Id == moduleInfoId).SingleAsync();
            
            await _moduleManager.UpdateColumns(columns.ToList(), module);
        }
        /// <summary>
        /// 更新按钮信息
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="moduleInfoId"></param>
        /// <returns></returns>
        public async Task UpdateBtns(IEnumerable<BtnInfoDto> btns, int moduleInfoId)
        {
            foreach(var btn in btns)
            {
                btn.Normalize();
            }
            var moduleButtons = ObjectMapper.Map<List<ModuleButton>>(btns);
            var module = await Repository.GetAllIncluding(o => o.Buttons).Where(o => o.Id == moduleInfoId).SingleAsync();

            await _moduleManager.UpdateBtns(moduleButtons, module);
        }

        /// <summary>
        /// 获取模块的列数据用于laytable展示
        /// </summary>
        /// <param name="moduleKey"></param>
        /// <returns></returns>
        public virtual async Task<object> GetColumnLayData(string moduleKey)
        {
            var moduleInfo = await _moduleManager.GetModuleInfo(moduleKey);
            var columns = moduleInfo.FilterdColumnInfos(FormType.List).Select(o => o.ToLayData());

            return new { Plugin = moduleInfo.GetPluginName(), Columns = columns };
        }

        #region 分页
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [DontWrapResult]
        public override async Task<ResultPageDto> GetPageResult(RequestPageDto request)
        {            
            var pageResult =await GetPageResultQueryable(request);
            
            var data = await pageResult.Queryable.Include(o => o.CreatorUser).Include(o => o.ColumnInfos)
                
                .Select(o => new { o.Id, o.ModuleKey, o.ModuleName, o.IsInterModule, Creator = o.CreatorUser.Name, CreationTime = o.CreationTime.ToString("yyyy-MM-dd HH:mm"), ColumnCount = o.ColumnInfos.Count, BtnCount = o.Buttons.Count }).ToListAsync();

            var result = new ResultPageDto()
            {
                code = 0,
                count = pageResult.RowCount,
                data = data
            };

            return result;
        }
        protected async override Task<PagedResult<ModuleInfo>> GetPageResultQueryable(RequestPageDto request)
        {
            var query= await GetQueryable(request);
            //如果是Host登录，从session中获取账套信息
            if (AbpSession.MultiTenancySide == Abp.MultiTenancy.MultiTenancySides.Host)
            {
                var tenantId = _httpContextAccessor.HttpContext.Session.Get<int?>("TenantId");
                if (tenantId != null)
                {
                    query = query.IgnoreQueryFilters().Where(o => o.TenantId == tenantId.Value);
                }
            }
            return query.PageResult(request.Page, request.Limit);
        }

        protected override async Task<IQueryable<ModuleInfo>> BuildKeywordQueryAsync(string keyword, IQueryable<ModuleInfo> query)
        {
            return (await (base.BuildKeywordQueryAsync(keyword, query)))
                .Where(o=>o.ModuleKey.Contains(keyword) || o.ModuleName.Contains(keyword));
        }
        
        #endregion

        #region 初始化
        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <returns></returns>
        public virtual async Task InitAllModuleInfo()
        {
            var context = Repository.GetDbContext() as MasterDbContext;
            new TenantDefaultModuleBuilder(context, AbpSession.TenantId.Value).Create();
        }
        /// <summary>
        /// 重置模块
        /// </summary>
        /// <param name="moduleInfoIds"></param>
        /// <returns></returns>
        public virtual async Task InitModuleInfo(IEnumerable<int> moduleInfoIds)
        {
            var manager = Manager as ModuleInfoManager;
            var context = Repository.GetDbContext() as MasterDbContext;
            var builder=new TenantDefaultModuleBuilder(context, AbpSession.TenantId.Value);

            var modules = await Manager.GetListByIdsAsync(moduleInfoIds);
            //直接删除
            await Repository.HardDeleteAsync(o=>modules.Select(m=>m.Id).Contains(o.Id));
            await CurrentUnitOfWork.SaveChangesAsync();
            foreach (var module in modules)
            {
                
                var type = manager.FindRelativeType(module);
                var menuItemDefinition = manager.FindRelativeMenuDefinition(module.ModuleKey);
                if (type != null)
                {
                    builder.CreateInterModulesFromType(type);
                }
                else
                {
                    
                    builder.CreateAddInMenuFromMenuDefinition(menuItemDefinition);
                }
            }
        }
        #endregion

        #region 前端调整表格的记忆接口
        private async Task<ColumnInfo> GetModuleColumn(string moduleKey,string columnKey)
        {
            var manager = Manager as ModuleInfoManager;
            var moduleInfo = await manager.GetAll()
                .Include(o => o.ColumnInfos)
                .Where(o => o.ModuleKey == moduleKey)
                .SingleOrDefaultAsync();
            if (moduleInfo == null)
            {
                throw new UserFriendlyException(L("参数错误"));
            }
            var columnInfo = moduleInfo.ColumnInfos.Where(o => o.ColumnKey.ToLower() == columnKey.ToLower()).SingleOrDefault();
            if (columnInfo == null)
            {
                throw new UserFriendlyException(L("参数错误"));
            }
            return columnInfo;
        }
        /// <summary>
        /// 固定列设置
        /// </summary>
        /// <param name="moduleKey"></param>
        /// <param name="columnKey"></param>
        /// <param name="isFixed"></param>
        /// <returns></returns>
        public virtual async Task SetColumnFixed(string moduleKey,string columnKey,bool isFixed=true)
        {
            var manager = Manager as ModuleInfoManager;
            var moduleInfo = await manager.GetAll()
                .Include(o => o.ColumnInfos)
                .Where(o => o.ModuleKey == moduleKey)
                .SingleOrDefaultAsync();
            if (moduleInfo == null)
            {
                throw new UserFriendlyException(L("参数错误"));
            }
            var columnInfo = moduleInfo.ColumnInfos.Where(o => o.ColumnKey.ToLower() == columnKey.ToLower()).SingleOrDefault();
            if (columnInfo == null)
            {
                throw new UserFriendlyException(L("参数错误"));
            }

            if (isFixed)
            {
                //固定列需要将对应列左侧所有列均设置为固定列
                foreach(var column in moduleInfo.ColumnInfos.Where(o=>o.IsShownInList && o.Sort < columnInfo.Sort))
                {
                    column.SetData("fixed", "left");
                }
            }
            else
            {
                //取消固定列需要将对应列右侧所有固定列取消固定
                foreach (var column in moduleInfo.ColumnInfos.Where(o => o.IsShownInList && o.Sort > columnInfo.Sort && o.GetData<string>("fixed")=="left"))
                {
                    column.RemoveData("fixed");
                }
            }
        }
        /// <summary>
        /// 设置列宽
        /// </summary>
        /// <param name="moduleKey"></param>
        /// <param name="columnKey"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public virtual async Task SetColumnWidth(string moduleKey,string columnKey,int width)
        {
            var columnInfo =await GetModuleColumn(moduleKey, columnKey);

            columnInfo.SetData("width", width.ToString());
        }
        /// <summary>
        /// 设置列的显示与否
        /// </summary>
        /// <param name="moduleKey"></param>
        /// <param name="columnKey"></param>
        /// <param name="visible"></param>
        /// <returns></returns>
        public virtual async Task SetColumnVisible(string moduleKey,string columnKey,bool visible)
        {
            var columnInfo = await GetModuleColumn(moduleKey, columnKey);

            columnInfo.IsShownInList = visible;
        }
        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="moduleKey"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public virtual async Task SetColumnSort(string moduleKey, List<string> columnKeys)
        {
            var manager = Manager as ModuleInfoManager;
            var moduleInfo = await manager.GetAll()
                .Include(o => o.ColumnInfos)
                .Where(o => o.ModuleKey == moduleKey)
                .SingleOrDefaultAsync();
            var oldColumnInfos = moduleInfo.ColumnInfos;

            for (var sort = 1; sort <= columnKeys.Count(); sort++)
            {
                var columnKey = columnKeys[sort-1];
                var columnInfo = oldColumnInfos.Where(o => o.ColumnKey.ToLower()== columnKey.ToLower()).SingleOrDefault();
                if (columnInfo != null) {
                    columnInfo.Sort = sort;
                }
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
