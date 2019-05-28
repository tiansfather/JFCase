using Abp.Application.Features;
using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus;
using Abp.Extensions;
using Abp.Localization;
using Abp.Reflection;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching;
using Abp.UI;
using Master.Authentication;
using Master.Domain;
using Master.Extension;
using Master.Menu;
using Master.Module.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Master.Module
{
    public class ModuleInfoManager : DomainServiceBase<ModuleInfo, int>, IModuleInfoManager
    {
        //public ITypeFinder TypeFinder { get; set; }
        ////public IMenuManager MenuManager { get; set; }
        //public IFeatureManager FeatureManager { get; set; }

        //private IRepository<ModuleInfo, int> _moduleInfoRepository;
        //private IRepository<ColumnInfo, int> _columnInfoRepository;
        //private IRepository<ModuleButton, int> _buttonInfoRepository;
        //private IRepository<PermissionSetting> _permissionRepository;
        //private IColumnReader _columnReader;
        //private IRelativeDataParser _relativeDataParser;
        //private IValuePathParser _valuePathParser;
        //private IIocManager _iocManager;
        //public ModuleInfoManager(IRepository<ModuleInfo, int> moduleInfoRepository, 
        //    IRepository<ColumnInfo, int> columnInfoRepository,
        //    IRepository<ModuleButton, int> buttonInfoRepository,
        //    IRepository<PermissionSetting> permissionRepository,
        //    ICacheManager cacheManager,
        //    IColumnReader columnReader, 
        //    IRelativeDataParser relativeDataParser,
        //    IIocManager iocManager, 
        //    IValuePathParser valuePathParser)
        //{
        //    _moduleInfoRepository = moduleInfoRepository;
        //    _columnInfoRepository = columnInfoRepository;
        //    _buttonInfoRepository = buttonInfoRepository;
        //    _columnReader = columnReader;
        //    _relativeDataParser = relativeDataParser;
        //    _iocManager = iocManager;
        //    _valuePathParser = valuePathParser;
        //    _permissionRepository = permissionRepository;
        //}

        public override IQueryable<ModuleInfo> GetAll()
        {
            if (AbpSession.TenantId.HasValue)
            {
                //加入特性的筛选，查询不到当前拥有特性不支持的模块
                var conditions = new List<string>();
                conditions.Add("string.IsNullOrEmpty(RequiredFeature)");
                foreach (var feature in GetEnabledFeatures().Result)
                {
                    conditions.Add("RequiredFeature.Contains(\"" + feature + "\")");
                }
                return base.GetAll()
                    .Where(string.Join(" Or ", conditions));
            }
            else
            {
                return base.GetAll();
            }

        }
        /// <summary>
        /// 获取所有启用的特性
        /// </summary>
        /// <returns></returns>
        protected async Task<IEnumerable<string>> GetEnabledFeatures()
        {
            var enabledFeatures = new List<string>();

            var features = Resolve<IFeatureManager>().GetAll();
            var feachChecker = Resolve<IFeatureChecker>();
            foreach (var feature in features)
            {
                if (await feachChecker.IsEnabledAsync(feature.Name))
                {
                    enabledFeatures.Add(feature.Name);
                }
            }
            return enabledFeatures.AsEnumerable();
        }
        public virtual async Task<ModuleInfo> GetModuleInfo(string moduleKey)
        {
            //此处不能用abpsession,因为Host管理界面会模拟tenantid进行提交查询
            var tenantId = CurrentUnitOfWork.GetTenantId();
            //return await _moduleInfoRepository.GetAllIncluding(o => o.ColumnInfos, o => o.Buttons).Where(o => o.ModuleKey == moduleKey).FirstOrDefaultAsync();
            var key = moduleKey + "@" + (tenantId ?? 0);
            return await CacheManager.GetCache<string, ModuleInfo>("ModuleInfo")
                .GetAsync(key, async () => { return await Repository.GetAllIncluding(o => o.ColumnInfos, o => o.Buttons).Where(o => o.ModuleKey == moduleKey).FirstOrDefaultAsync(); });

        }

        public virtual async Task RemoveModuleInfoCache(string moduleKey, int tenantId)
        {
            var key = moduleKey + "@" + tenantId;
            CacheManager.GetCache<string, ModuleInfo>("ModuleInfo").Remove(key);
            //同时更新权限表，对于不存在的按钮，取消权限
            var buttonPermissions = Resolve<IRepository<ModuleButton, int>>().GetAll().Where(o => o.ModuleInfo.ModuleKey == moduleKey).Select(o => $"Module.{moduleKey}.Button.{o.ButtonKey}").ToList();
            Resolve<IRepository<PermissionSetting, int>>().Delete(o => !buttonPermissions.Contains(o.Name) && o.Name.StartsWith($"Module.{moduleKey}.Button"));
            //取消权限缓存
            CacheManager.GetCache<int, IList<Permission>>("ModuleInfoPermission").Remove(tenantId);
            CacheManager.GetCache<int, IList<Permission>>("Permissions").Remove(tenantId);
        }
        #region 通过模块寻找对应的定义类
        /// <summary>
        /// 获取模块的定义类
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public virtual Type FindRelativeType(ModuleInfo moduleInfo)
        {
            var types = Resolve<ITypeFinder>().Find(o => o.GetSingleAttributeOrNull<InterModuleAttribute>() != null && o.Name == moduleInfo.ModuleKey);
            return types.Length > 0 ? types[0] : null;
        }
        /// <summary>
        /// 获取模块的定义菜单
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public virtual MenuItemDefinition FindRelativeMenuDefinition(string moduleKey)
        {
            var menus = Resolve<MenuManager>().GetAllMenus();
            return menus
                .Where(o => o.Name.Contains("Tenancy") && o.Name.Substring(o.Name.LastIndexOf('.') + 1) == moduleKey)
                .FirstOrDefault();
        }
        #endregion

        #region 通过权限名称寻找对应的模块按钮
        /// <summary>
        /// 通过权限名称寻找对应的模块按钮
        /// </summary>
        /// <param name="permissionName"></param>
        /// <returns></returns>
        public virtual async Task<ModuleButton> FindButtonByPermissionName(string permissionName)
        {
            //Host登录直接返回null
            if (AbpSession.MultiTenancySide == Abp.MultiTenancy.MultiTenancySides.Host)
            {
                return null;
            }

            if (permissionName.IndexOf("Module") < 0 || permissionName.IndexOf("Button") < 0)
            {
                return null;
            }
            var permissionNameArr = permissionName.Split('.');
            if (permissionNameArr.Length != 4)
            {
                return null;
            }
            var moduleKey = permissionNameArr[1];
            var buttonKey = permissionNameArr[3];
            //优化，使用模块缓存，避免产生多次数据库查询
            var moduleInfo = await GetModuleInfo(moduleKey);
            if (moduleInfo != null)
            {
                return moduleInfo.Buttons.SingleOrDefault(o => o.ButtonKey == buttonKey);
            }
            //var button = await _buttonInfoRepository.GetAll().Where(o => o.ModuleInfo.ModuleKey == moduleKey && o.ButtonKey == buttonKey).SingleOrDefaultAsync();
            return null;
        }
        #endregion

        #region 模块维护
        public async Task AddModuleInfo(ModuleInfo moduleInfo)
        {
            if ((await Repository.CountAsync(o => o.ModuleKey == moduleInfo.ModuleKey || o.ModuleName == moduleInfo.ModuleName)) > 0)
            {
                throw new UserFriendlyException(L("相同模块名称已存在"));
            }
            moduleInfo.MakeDefaultColumnsAndButtons();

            await InsertAsync(moduleInfo);
        }
        public async Task UpdateColumns(IList<ColumnInfo> columnInfos, ModuleInfo moduleInfo)
        {
            foreach (var column in columnInfos)
            {
                column.ModuleInfo = moduleInfo;
            }
            //moduleInfo.ColumnInfos.ToList().RemoveAll(column => columnInfos.SingleOrDefault(o => o.Id == column.Id) == null);
            var _columnInfoRepository = Resolve<IRepository<ColumnInfo, int>>();
            var _buttonInfoRepository = Resolve<IRepository<ModuleButton, int>>();
            foreach (var column in moduleInfo.ColumnInfos)
            {
                var newColumn = columnInfos.SingleOrDefault(o => o.Id == column.Id);
                if (newColumn == null)
                {
                    _columnInfoRepository.Delete(column);
                }
                else
                {
                    newColumn.MapTo(column);
                }
            }
            foreach (var column in columnInfos.Where(o => o.Id == 0))
            {
                moduleInfo.ColumnInfos.Add(column);
            }
            //列信息预处理
            foreach (var column in moduleInfo.ColumnInfos)
            {
                column.Normalize();
            }
            //触发事件
            await EventBus.Default.TriggerAsync(new ModuleInfoChangedEventData() { ModuleKey = moduleInfo.ModuleKey, TenantId = moduleInfo.TenantId });

        }
        public async Task UpdateBtns(IList<ModuleButton> btns, ModuleInfo moduleInfo)
        {
            var _buttonInfoRepository = Resolve<IRepository<ModuleButton, int>>();
            foreach (var btn in btns)
            {
                btn.ModuleInfo = moduleInfo;
            }
            foreach (var btn in moduleInfo.Buttons)
            {
                var newBtn = btns.SingleOrDefault(o => o.Id == btn.Id);
                if (newBtn == null)
                {
                    _buttonInfoRepository.Delete(btn);
                }
                else
                {
                    newBtn.MapTo(btn);
                }
            }
            foreach (var btn in btns.Where(o => o.Id == 0))
            {
                moduleInfo.Buttons.Add(btn);
            }
            //触发事件
            await EventBus.Default.TriggerAsync(new ModuleInfoChangedEventData() { ModuleKey = moduleInfo.ModuleKey, TenantId = moduleInfo.TenantId });

        }
        #endregion

        #region 模块数据


        public virtual Task<IEnumerable<IDictionary<string, object>>> GetModuleDataListAsync(ModuleInfo moduleInfo, IQueryable query)
        {
            var entityType = Resolve<ITypeFinder>().Find(o => o.FullName == moduleInfo.EntityFullName)[0];

            var toListMethod = typeof(Enumerable).GetTypeInfo().GetDeclaredMethod("ToList").MakeGenericMethod(entityType);
            var entities = toListMethod.Invoke(null, new object[] { query });


            var method = GetType().GetTypeInfo().GetMethods().Where(o => o.Name == nameof(FillModuleDataListAsync));

            try
            {
                var task = method.First().MakeGenericMethod(entityType).Invoke(this, new object[] { entities, moduleInfo, null }) as Task<IEnumerable<IDictionary<string, object>>>;

                return task;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("数据解析失败:" + ex.Message));
            }
        }

        public virtual Task<IEnumerable<IDictionary<string, object>>> GetModuleDataListAsync(ModuleInfo moduleInfo, string whereCondition = "", string orderBy = "")
        {
            var entityType = Resolve<ITypeFinder>().Find(o => o.FullName == moduleInfo.EntityFullName)[0];

            var qry = GetQuery(moduleInfo);

            if (!whereCondition.IsNullOrEmpty()) { qry = qry.Where(whereCondition); }
            //排序，若没有排序参数则使用模块默认排序
            if (!orderBy.IsNullOrEmpty()) { qry = qry.OrderBy(orderBy); }
            else { qry = qry.OrderBy($"{moduleInfo.SortField} {moduleInfo.SortType.ToString()}"); }

            return GetModuleDataListAsync(moduleInfo, qry);

            //var toListMethod = typeof(Enumerable).GetTypeInfo().GetDeclaredMethod("ToList").MakeGenericMethod(entityType);
            //var entities = toListMethod.Invoke(null, new object[] { qry });


            //var method = GetType().GetTypeInfo().GetMethods().Where(o => o.Name == nameof(FillModuleDataListAsync));

            //try
            //{
            //    var task = method.First().MakeGenericMethod(entityType).Invoke(this, new object[] { entities, moduleInfo, null }) as Task<IEnumerable<DynamicClass>>;

            //    return task;
            //}
            //catch (Exception ex)
            //{
            //    throw new UserFriendlyException(L("数据解析失败:" + ex.Message));
            //}

        }

        public virtual async Task<IEnumerable<IDictionary<string, object>>> FillModuleDataListAsync<TEntity>(IEnumerable<TEntity> entities, ModuleInfo moduleInfo, Expression<Func<ColumnInfo, bool>> columnFilterExpression = null)
            where TEntity : class, new()
        {
            var manager = GetManager(typeof(TEntity));
            //模块所有列
            var columnInfos = columnFilterExpression != null ? moduleInfo.ColumnInfos.Where(columnFilterExpression.Compile()) : moduleInfo.ColumnInfos;

            //var navigations = GetNavigationStrings(columnInfos);
            //await entities.EnsurePropertyLoadedAsync(navigations);

            var result = new List<IDictionary<string, object>>();
            //加载原生数据及原生关联数据、自定义属性
            var dynamicSelectString = BuildDynamicSelectString(columnInfos);
            var dynamicList = entities.AsQueryable().Select(dynamicSelectString).AsEnumerable();

            //for(var i=0;i<entities.Count();i++)
            var index = 0;
            var _relativeDataParser = Resolve<IRelativeDataParser>();
            var _columnReader = Resolve<IColumnReader>();
            foreach (DynamicClass obj in dynamicList)
            {
                var oriEntity = entities.ElementAt(index);
                index++;

                var entity = obj.ToDictionary();
                //数据前处理
                await manager.FillEntityDataBefore(entity, moduleInfo, oriEntity);
                //读取属性列
                var propertyColumns = columnInfos.Where(o => o.IsPropertyColumn);
                if (propertyColumns.Count() > 0)
                {
                    var properties = (obj.GetDynamicPropertyValue("Property") as JsonObject<IDictionary<string, object>>);
                    if (properties?.Object != null)
                    {
                        foreach (var column in propertyColumns)
                        {
                            //entity[column.ColumnKey] = properties.FirstOrDefault(o => o.PropertyName == column.ColumnKey)?.PropertyValue;
                            if (properties.Object.ContainsKey(column.ColumnKey))
                            {
                                entity[column.ColumnKey] = properties.Object[column.ColumnKey];
                            }

                        }
                    }

                    //移除属性信息
                    entity.Remove("Property");
                }


                //加载自定义引用数据
                var relativeDataContext = new ModuleDataContext() { Entity = entity, ModuleInfo = moduleInfo, ModuleInfoManager = this };
                await _relativeDataParser.Parse(relativeDataContext);

                //列解析

                foreach (var columnInfo in columnInfos)
                {
                    columnInfo.Normalize();
                    var context = new ColumnReadContext() { Entity = entity, ColumnInfo = columnInfo };
                    if (columnInfo.ColumnType == ColumnTypes.Customize)
                    {
                        //自定义控件的列信息读取
                        await columnInfo.GetCustomizeControl().Read(context);
                    }
                    else
                    {
                        await _columnReader.Read(context);
                    }

                }

                //数据后处理,交给具体的实体管理类

                await manager.FillEntityDataAfter(entity, moduleInfo, oriEntity);

                result.Add(entity);
            }
            //}
            //catch(Exception ex)
            //{
            //    Logger.Error(ex.Message, ex);
            //    throw new UserFriendlyException(L("数据解析错误:") + dynamicSelectString);
            //}





            return result.AsEnumerable();
        }
        /// <summary>
        /// 对于直接属性、关联属性以及自定义属性直接构造动态查询返回
        /// </summary>
        /// <param name="columnInfos"></param>
        /// <returns></returns>
        private string BuildDynamicSelectString(IEnumerable<ColumnInfo> columnInfos)
        {
            var propertyStrList = new List<string>();
            var _valuePathParser = Resolve<IValuePathParser>();
            foreach (var column in columnInfos.Where(o => o.IsDirectiveColumn))
            {
                propertyStrList.Add($"{_valuePathParser.Parse(column.ValuePath)} as {column.ColumnKey}");

                if (!string.IsNullOrEmpty(column.DisplayPath))
                {
                    propertyStrList.Add($"{_valuePathParser.Parse(column.DisplayPath)} as {column.ColumnKey}_display");
                }
            }
            if (columnInfos.Count(o => o.IsPropertyColumn) > 0)
            {
                propertyStrList.Add("Property");
            }
            if (!propertyStrList.Contains("Id"))
            {
                propertyStrList.Add("Id");
            }
            return $"new({string.Join(",", propertyStrList)})";
        }

        public IQueryable GetQuery(ModuleInfo moduleInfo)
        {
            var entityType = Resolve<ITypeFinder>().Find(o => o.FullName == moduleInfo.EntityFullName)[0];
            //var entityType = Type.GetType(moduleInfo.EntityFullName);
            var managerType = GetEntityManagerType(entityType);
            using (var managerWrapper = IocManager.Instance.ResolveAsDisposable(managerType))
            {
                var manager = managerWrapper.Object;
                var qry = (manager.GetType().GetTypeInfo().GetDeclaredMethod("GetFilteredQuery").Invoke(manager, new object[] { moduleInfo.ModuleKey }) as IQueryable);
                return qry;
            }
        }
        #endregion

        #region 增加删除表单提交
        public async Task ManageFormAdd(ModuleInfo moduleInfo, IDictionary<string, string> Datas)
        {
            var entityType = Resolve<ITypeFinder>().Find(o => o.FullName == moduleInfo.EntityFullName)[0];
            var manager = GetManager(entityType) as IFormModule;
            await manager.DoAdd(moduleInfo, Datas);
        }
        public async Task ManageFormEdit(ModuleInfo moduleInfo, IDictionary<string, string> Datas, int id)
        {

            var entityType = Resolve<ITypeFinder>().Find(o => o.FullName == moduleInfo.EntityFullName)[0];
            var manager = GetManager(entityType) as IFormModule;
            await manager.DoEdit(moduleInfo, Datas, id);
        }
        public async Task ManageFormMultiEdit(ModuleInfo moduleInfo, IDictionary<string, string> Datas, IEnumerable<int> ids)
        {
            var entityType = Resolve<ITypeFinder>().Find(o => o.FullName == moduleInfo.EntityFullName)[0];
            var manager = GetManager(entityType) as IFormModule;
            await manager.DoMultiEdit(moduleInfo, Datas, ids.Cast<object>());
        }
        #endregion

        #region 模块权限
        public virtual IList<Permission> GetAllModulePermissions()
        {


            Func<int, IList<Permission>> p = (a) =>
            {
                //Host登录不进行模块查询
                if (CurrentUnitOfWork.GetTenantId() == null)
                //if (AbpSession.MultiTenancySide == Abp.MultiTenancy.MultiTenancySides.Host)
                {
                    return new List<Permission>();
                }
                var permissions = new List<Permission>();
                var moduleInfos = Repository.GetAllIncluding(o => o.Buttons, o => o.ColumnInfos).ToList();
                //所有按钮的权限
                permissions.AddRange(moduleInfos.SelectMany(o => o.Buttons.Where(b => b.RequirePermission && b.IsEnabled).Select(b => new Permission(b.ButtonPermissionName, new LocalizableString(b.ButtonName, MasterConsts.LocalizationSourceName)))));

                //启用了权限的字段
                var fields = moduleInfos.SelectMany(o => o.ColumnInfos).Where(o => o.EnableFieldPermission);

                permissions.AddRange(fields.SelectMany(o =>
                {
                    var fieldPermisssions = new List<Permission>();
                    fieldPermisssions.Add(new Permission(o.ColumnAddPermission, new LocalizableString(o.ColumnName + "_添加", MasterConsts.LocalizationSourceName)));
                    fieldPermisssions.Add(new Permission(o.ColumnEditPermission, new LocalizableString(o.ColumnName + "_修改", MasterConsts.LocalizationSourceName)));
                    fieldPermisssions.Add(new Permission(o.ColumnViewPermission, new LocalizableString(o.ColumnName + "_查看", MasterConsts.LocalizationSourceName)));
                    return fieldPermisssions;
                }));
                return permissions;
            };
            var cacheKey = AbpSession.TenantId ?? 0;

            //return p(cacheKey);

            return CacheManager.GetCache<int, IList<Permission>>("ModuleInfoPermission")
                .Get(cacheKey, p);


        }
        #endregion



    }
}
