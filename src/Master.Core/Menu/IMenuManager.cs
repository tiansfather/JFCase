using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Menu
{
    public interface IMenuManager : ITransientDependency
    {
        /// <summary>
        /// 获取所有导航菜单
        /// </summary>
        /// <returns></returns>
        IList<MenuItemDefinition> GetAllMenus();
        /// <summary>
        /// 获取所有导航菜单权限集合
        /// </summary>
        /// <returns></returns>
        IList<Permission> GetAllMenuPermissions();
        /// <summary>
        /// 获取默认菜单
        /// </summary>
        /// <returns></returns>
        List<MenuItemDefinition> LoadDefaultMenus();
        /// <summary>
        /// 获取设置的菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //List<MenuItemDefinition> LoadSettingMenus(long? userId);
        /// <summary>
        /// 获取用户设定的菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        string LoadUserSettingMenusData(long? userId);
        /// <summary>
        /// 修改菜单设置
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="menuItems"></param>
        /// <returns></returns>
        Task SaveSettingMenusAsync(long? userId, List<MenuItemDefinition> menuItems);
        Task SaveSettingMenusAsync(long? userId, string settingStr);
        /// <summary>
        /// 获取用户可见菜单
        /// </summary>
        /// <param name="userIdentifier"></param>
        /// <returns></returns>
        //Task<IList<CustomUserMenuItem>> LoadUserMenu(Abp.UserIdentifier userIdentifier);
        Task<int> FillUserMenuItems(Abp.UserIdentifier user, IList<MenuItemDefinition> menuItemDefinitions, IList<CustomUserMenuItem> userMenuItems,bool filterUserPermission=true);
        /// <summary>
        /// 通过菜单name（唯一标识）获取菜单
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        MenuItemDefinition LoadMenusByName(string name, IList<MenuItemDefinition> menus = null);
    }
}
