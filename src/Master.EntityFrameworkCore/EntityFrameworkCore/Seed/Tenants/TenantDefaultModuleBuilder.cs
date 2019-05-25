using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Reflection;
using Abp.Reflection.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abp.Domain.Entities;
using System.Text;
using Master.EntityFrameworkCore.Seed.BaseData;
using Master.Module;
using Master.Module.Attributes;
using Master.Menu;
using Abp.Localization;
using Abp.Application.Navigation;
using Abp.Application.Features;

namespace Master.EntityFrameworkCore.Seed.Tenants
{
    /// <summary>
    /// 构建默认模块信息至数据库
    /// </summary>
    public class TenantDefaultModuleBuilder
    {
        private readonly MasterDbContext _context;
        private readonly int _tenantId;

        public TenantDefaultModuleBuilder(MasterDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        /// <summary>
        /// 创建默认模块
        /// </summary>
        public void Create()
        {
            //默认模块设置
            CreateAddInModules();
            ////默认模块按钮
            //CreateAddInModuleButtons();

            //构建基于菜单的资源模块
            CreateAddinMenuModules();



            //默认数据
            CreateDefaultData();
        }

        #region 1.默认模块配置

        /// <summary>
        /// 创建默认模块设置
        /// </summary>
        private void CreateAddInModules()
        {
            //构建内置实体的默认模块
            CreateAddInModulesByModel();
            //构建自定义设定的内置模块
            CreateAddInModulesBySet();
        }

        #region 构建内置实体的默认模块 CreateAddInModulesByModel

        /// <summary>
        /// 通过实体定义为默认模块的标记去设置模块
        /// </summary>
        private void CreateAddInModulesByModel()
        {
            //寻找实体的发起
            var typeFinder = IocManager.Instance.Resolve<ITypeFinder>();
            //获取所有内置模块包括了 InterModuleAttribute 标记的实体
            var interModuleTypes = typeFinder.Find(o => {
                //o.GetSingleAttributeOrNull<InterModuleAttribute>()
                return o.GetCustomAttributes(typeof(InterModuleAttribute), false).Length > 0;
            });

            foreach (var type in interModuleTypes)
            {
                //根据定义模块增加内置模块
                CreateInterModulesFromType(type);
            }
        }

         
        /// <summary>
        /// 根据定义模块增加内置模块
        /// </summary>
        /// <param name="type"></param>
        public void CreateInterModulesFromType(Type type)
        {
            //ModuleInfo表
            var typeName = type.Name;
            var moduleAttr = type.GetSingleAttributeOrNull<InterModuleAttribute>();
            var moduleInfo = _context.ModuleInfo.IgnoreQueryFilters().SingleOrDefault(o => o.TenantId == _tenantId && o.ModuleKey == typeName);
            //没有对应模块创建对应模块
            if (moduleInfo == null)
            {                
                moduleInfo = new ModuleInfo()
                {
                    TenantId = _tenantId,
                    ModuleKey = typeName,
                    EntityFullName = type.FullName,
                    IsInterModule = true
                };
                moduleAttr.MapTo(moduleInfo);
                //如果模块设置了基类型，以基类型做为绑定实体
                if (moduleAttr.BaseType!=null)
                {
                    moduleInfo.EntityFullName = moduleAttr.BaseType.FullName;
                }
                //获取模块所在程序集的名称，用于生成模块的数据接口/api/services/storage/getpageresult
                moduleInfo.SetData("PluginName", type.GetAssembly().GetName().Name.Replace("Master.","").ToLower());
                using (var moduleInfoManagerWrapper = IocManager.Instance.ResolveAsDisposable<ModuleInfoManager>())
                {
                    var menu = moduleInfoManagerWrapper.Object.FindRelativeMenuDefinition(typeName);
                    if (menu != null)
                    {
                        var featureDependency = menu.FeatureDependency as SimpleFeatureDependency;
                        if (featureDependency != null)
                        {
                            moduleInfo.RequiredFeature = string.Join(',', featureDependency.Features);
                        }
                    }
                }
                //moduleInfo.MakeDefaultColumns();
                _context.ModuleInfo.Add(moduleInfo);
            }
            #region 构建列

            //构建内置列
            moduleInfo.CreateInterColumnsFromType(type);

            if (moduleAttr.GenerateDefaultColumns)
            {
                //构建系统列
                moduleInfo.MakeDefaultColumns();
            }

            //构建自定义列
            moduleInfo.CreateCustomizeColumns(type.Name);




            #endregion


            #region 构建按钮

            if (moduleAttr.GenerateDefaultButtons)
            {
                //构建默认按钮
                moduleInfo.MakeDefaultButtons();
            }
            //构建自定义按钮
            moduleInfo.CreateCustomizeButtons(type.Name);


            #endregion



            _context.SaveChanges();
        }
        



        #endregion


        #region 构建自定义设定的内置模块 CreateAddInModulesBySet

        /// <summary>
        /// 构建自定义设定的内置模块
        /// </summary>
        private void CreateAddInModulesBySet()
        {
            var typeFinder = IocManager.Instance.Resolve<ITypeFinder>();
            var types = typeFinder.Find(type => typeof(BaseSetModules).IsAssignableFrom(type) && !type.IsAbstract);

            foreach (var type in types)
            {
                var t = Activator.CreateInstance(type) as BaseSetModules;
                CreateSetModule(t);
            }
        }
        
        #region Base自定义内置模块
        /// <summary>
        /// 自定义模块构建
        /// </summary>
        private void CreateSetModule(BaseSetModules moduleProvider)
        {
            var moduleKey = moduleProvider.GetModulesKey();
            var moduleInfo = _context.ModuleInfo.IgnoreQueryFilters().SingleOrDefault(o => o.TenantId == _tenantId && o.ModuleKey == moduleProvider.GetModulesKey());
            if (moduleInfo == null)
            {

                //根据反射得到模块实体
                moduleInfo = moduleProvider.GetModuleInfo();
                moduleInfo.TenantId = _tenantId;
                //Type t = typeof(DepartBaseModules);
                //获取模块所在程序集的名称，用于生成模块的数据接口/api/services/storage/getpageresult
                moduleInfo.SetData("PluginName", moduleProvider.GetType().GetAssembly().GetName().Name.Replace("Master.", "").ToLower());
                //#region 构建列
                ////构建自定义列
                moduleInfo.CreateCustomizeColumns(moduleKey);
                ////构建系统列
                moduleInfo.MakeDefaultColumns();
                //#endregion

                //#region 构建按钮
                ////构建按钮
                moduleInfo.MakeDefaultButtons();
                ////构建自定义按钮
                moduleInfo.CreateCustomizeButtons(moduleKey);
                //#endregion


                _context.ModuleInfo.Add(moduleInfo);
                _context.SaveChanges();
            }
        }

        #endregion

        #endregion

        #endregion

        #region 2.菜单模块
        private void CreateAddinMenuModules()
        {
            using (var menuManagerWrapper = IocManager.Instance.ResolveAsDisposable<IMenuManager>())
            {
                //获取所有基于账套的菜单
                var menus = menuManagerWrapper.Object.GetAllMenus().Where(o => o.Name.Contains("Tenancy"));
                foreach (var menu in menus)
                {
                    //Setting.Tenancy.BaseSetting=>BaseSetting
                    CreateAddInMenuFromMenuDefinition(menu);
                }
            }
        }

        public void CreateAddInMenuFromMenuDefinition(MenuItemDefinition menu)
        {
            using (var locaizationContextWrapper = IocManager.Instance.ResolveAsDisposable<ILocalizationContext>())
            {
                var moduleKey = menu.Name.Substring(menu.Name.LastIndexOf('.') + 1);
                var moduleInfo = _context.ModuleInfo.IgnoreQueryFilters().SingleOrDefault(o => o.TenantId == _tenantId && o.ModuleKey == moduleKey);
                if (moduleInfo == null)
                {
                    //特性
                    var featureDependency = menu.FeatureDependency as SimpleFeatureDependency;
                    moduleInfo = new ModuleInfo()
                    {
                        ModuleKey = moduleKey,
                        ModuleName = menu.DisplayName.Localize(locaizationContextWrapper.Object),
                        IsInterModule = true,
                        TenantId = _tenantId,
                    };
                    if (featureDependency != null)
                    {
                        moduleInfo.RequiredFeature = string.Join(',', featureDependency.Features);
                    }
                    moduleInfo.SetData("PluginName", menu.Name.Substring(0, menu.Name.IndexOf('.')).ToLower());
                    moduleInfo.Buttons = new List<ModuleButton>();
                    _context.ModuleInfo.Add(moduleInfo);
                }
                //内置资源权限
                var customData = menu.CustomData?.ToString();
                if (!string.IsNullOrEmpty(customData))
                {

                    var resourceArr = customData.Split(',');
                    foreach (var resource in resourceArr)
                    {
                        var buttonInfoArr = resource.Split('|');
                        var buttonKey = buttonInfoArr[0];
                        if (moduleInfo.Buttons.Count(o => o.ButtonKey == buttonKey) == 0)
                        {
                            var btn = new ModuleButton()
                            {
                                ButtonKey = buttonInfoArr[0],
                                ButtonName = buttonInfoArr[1],
                                RequirePermission = buttonInfoArr[buttonInfoArr.Length - 1] == "0" ? false : true,//形如Set|工艺设定|0,表示此功能默认不开启权限验证
                                ButtonActionType = ButtonActionType.Resource,
                                TenantId = _tenantId
                            };
                            moduleInfo.Buttons.Add(btn);
                        }

                    }
                }

                _context.SaveChanges();
            }
                
        }
        #endregion

        #region 3.默认数据配置
        /// <summary>
        /// 生成默认数据
        /// </summary>
        private void CreateDefaultData()
        {
            
        }

        #endregion


        


    }
}
