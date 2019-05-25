using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Master.Dto;
using Master.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Menu
{
    [AbpAuthorize]
    public class MenuAppService: MasterAppServiceBase, IMenuAppService
    {
        private readonly IMenuManager _menuManager;
        private readonly ILocalizationContext _localizationContext;
        public MenuAppService(IMenuManager menuManager,ILocalizationContext localizationContext)
        {
            _menuManager = menuManager;
            _localizationContext = localizationContext;
        }
        /// <summary>
        /// 获取账套的菜单，用于Host管理页设置帮助文档
        /// </summary>
        /// <returns></returns>
        public List<MenuTreeDto> GetFullTenancyMenuTreeJson()
        {
            List<MenuTreeDto> menuTreeDtos = new List<MenuTreeDto>();
            var menu = _menuManager.LoadDefaultMenus();
            
            menuTreeDtos = menu.MapTo<List<MenuTreeDto>>();
            FilterByTenancySide(menuTreeDtos, MultiTenancySides.Tenant);
            return menuTreeDtos.OrderBy(o=>o.Order).ToList();
        }
        /// <summary>
        /// 获取用户的菜单
        /// </summary>
        /// <returns></returns>
        public async Task<List<MenuTreeDto>> GetMenuTreeJson()
        {
            List<MenuTreeDto> menuTreeDtos = new List<MenuTreeDto>();
            //获取用户设置的菜单
            string usermenudata = _menuManager.LoadUserSettingMenusData(AbpSession.UserId);
            //有设定的菜单
            if(!string.IsNullOrEmpty(usermenudata))
            {

                menuTreeDtos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MenuTreeDto>>(usermenudata);
            }
            //没有从默认的菜单中获取
            else
            {
                var menu = _menuManager.LoadDefaultMenus();
                var userMenu = new List<CustomUserMenuItem>();
                //这里需要直接过滤出当前账套能使用的菜单,但是不使用权限过滤,因为权限设置页需要显示账套所有菜单
                await _menuManager.FillUserMenuItems(AbpSession.ToUserIdentifier(), menu, userMenu, false);
                menuTreeDtos=userMenu.MapTo<List<MenuTreeDto>>();
                //menuTreeDtos = menu.MapTo<List<MenuTreeDto>>();
            }
            //按照登录用户的是Host还是Tenant进行过滤
            FilterByTenancySide(menuTreeDtos, AbpSession.MultiTenancySide);

            return menuTreeDtos;
        }
        private IList<MenuTreeDto> FilterByTenancySide(IList<MenuTreeDto> menuTreeDtos,MultiTenancySides multiTenancySides)
        {
            var result = menuTreeDtos.OrderBy(o=>o.Order).ToList();
            result.RemoveAll(o => !multiTenancySides.HasFlag(MultiTenancySides.Host) && o.Items.Count == 0 && o.Name.Contains("Host"));
            result.RemoveAll(o => !multiTenancySides.HasFlag(MultiTenancySides.Tenant) && o.Items.Count == 0 && o.Name.Contains("Tenancy"));
            foreach(var item in result.Where(o => o.Items.Count > 0))
            {
                item.Items=FilterByTenancySide(item.Items, multiTenancySides);
            }
            return result;
        }
        /// <summary>
        /// 获取用户的初始格式的菜单
        /// </summary>
        /// <returns></returns>
        public List<Abp.Application.Navigation.MenuItemDefinition> GetDefinitionMenu()
        {
            List<Abp.Application.Navigation.MenuItemDefinition> menuItemDefinitions  = new List<Abp.Application.Navigation.MenuItemDefinition>();
            //获取用户设置的菜单
            string usermenudata = _menuManager.LoadUserSettingMenusData(AbpSession.UserId);
            //有设定的菜单
            if (!string.IsNullOrEmpty(usermenudata))
            {

                var menuTreeDtos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MenuTreeDto>>(usermenudata);

                menuItemDefinitions = GetMenuItemDefinitionByMenuTreeDto(menuTreeDtos).ToList();
            }
            //没有从默认的菜单中获取
            else
            {
                menuItemDefinitions = _menuManager.LoadDefaultMenus();
            }



            return menuItemDefinitions;
        }
        /// <summary>
        /// 获取用户可见菜单
        /// </summary>
        /// <param name="userIdentifier"></param>
        /// <returns></returns>
        public async  Task<List<CustomUserMenuItem>> GetUserMenu(Abp.UserIdentifier userIdentifier)
        {
            var userMenus = new List<CustomUserMenuItem>();
            var menus = GetDefinitionMenu();
            await _menuManager.FillUserMenuItems(userIdentifier, menus, userMenus);
            return userMenus;
        }




        /// <summary>
        /// 获取默认菜单
        /// </summary>
        /// <returns></returns>
        public object GetDefaultMenusTreeJson()
        {
            var menu = _menuManager.LoadDefaultMenus();
            return menu.MapTo<List<MenuTreeDto>>();
        }

        /// <summary>
        /// 保存设置的菜单
        /// </summary>
        /// <param name="menuTreeDtos"></param>
        /// <returns></returns>
        public async Task SaveMenuSetting(List<MenuTreeDto> menuTreeDtoList)
        {

            if (menuTreeDtoList.Count >0)
            {
                var settingStr = Newtonsoft.Json.JsonConvert.SerializeObject(menuTreeDtoList);

                //var menu = GetMenuItemDefinitionByMenuTreeDto(menuTreeDtoList).ToList();
                await _menuManager.SaveSettingMenusAsync(null, settingStr);
            }
        }

        public IList<Abp.Application.Navigation.MenuItemDefinition> GetMenuItemDefinitionByMenuTreeDto(IList<MenuTreeDto> menuTreeDtos)
        {
            IList<Abp.Application.Navigation.MenuItemDefinition> menuItemDefinitions = new List<Abp.Application.Navigation.MenuItemDefinition>();
            foreach(var menuTreeDto in menuTreeDtos)
            {
                var temp= _menuManager.LoadMenusByName(menuTreeDto.Name);
                if (temp != null)
                {
                    temp.DisplayName = new LocalizableString(menuTreeDto.DisplayName, MasterConsts.LocalizationSourceName);

                    if (menuTreeDto.Items.Count > 0)
                    {
                        var items = GetMenuItemDefinitionByMenuTreeDto(menuTreeDto.Items);
                        foreach (var item in items)
                        {
                            temp.Items.Add(item);
                        }
                    }
                    menuItemDefinitions.Add(temp);
                }
            }
            return menuItemDefinitions;
        }

    }
}
