using Abp.Authorization;
using Abp.Localization;
using Abp.Web.Models;
using Master;
using Master.Authentication;
using Master.Dto;
using Master.Menu;
using Master.Module;
using SkyNet.Master.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet.Master.Service
{
    [AbpAuthorize]
    public class PermissionAppService: MasterAppServiceBase
    {
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        private readonly IMenuManager _menuManager;
        private readonly IPermissionManager _permissionManager;
        private readonly IModuleInfoManager _moduleInfoManager;
        private readonly ILocalizationContext _localizationContext;
        public PermissionAppService(RoleManager roleManager,UserManager userManager, IMenuManager menuManager, IPermissionManager permissionManager,IModuleInfoManager moduleInfoManager, ILocalizationContext localizationContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _menuManager = menuManager;
            _permissionManager = permissionManager;
            _moduleInfoManager = moduleInfoManager;
            _localizationContext = localizationContext;
        }
        #region 返回权限
        /// <summary>
        /// 返回用户拥有的所有权限
        /// </summary>
        /// <returns></returns>
        public virtual async Task<string[]> GetAllGrantedPermissions()
        {
            var user = await _userManager.GetByIdAsync(AbpSession.UserId.Value);
            var permissions = await _userManager.GetGrantedPermissionsAsync(user);
            return permissions.Select(o => o.Name).ToArray();
        }
        #endregion
        /// <summary>
        /// 导航权限设置
        /// </summary>
        /// <param name="type">Roles:为角色分配权限,Users:为用户分配权限</param>
        /// <param name="permissionNames"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task AssignAllMenuPermission(string type,string[] permissionNames,int id)
        {
            IList<Permission> allPermissions= _menuManager.GetAllMenuPermissions();

            if (type == "Roles")
            {
                //给角色分配权限
                var role = await _roleManager.GetByIdAsync(id);
                
                foreach (var permission in allPermissions)
                {
                    if (permissionNames.Contains(permission.Name))
                    {
                        await _roleManager.GrantPermissionAsync(role, permission);
                    }
                    else
                    {
                        
                        await _roleManager.ProhibitPermissionAsync(role, permission);
                    }
                }
            }
            else if(type=="User")
            {
                //给用户分配权限
                var user = await _userManager.GetByIdAsync(id);

                foreach (var permission in allPermissions)
                {
                    if (permissionNames.Contains(permission.Name))
                    {
                        await _userManager.GrantPermissionAsync(user, permission);
                    }
                    else
                    {
                        await _userManager.ProhibitPermissionAsync(user, permission);
                    }
                }

            }
        }

        /// <summary>
        /// 获取已分配的导航权限
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<string[]> LoadGrantedMenuPermissions(string type,int id)
        {
            IReadOnlyList<Permission> permissions=new List<Permission>();
            if (type == "Roles")
            {
                permissions=await _roleManager.GetGrantedPermissionsAsync(Convert.ToInt32(id));
            }else if (type == "User")
            {
                var user = await _userManager.GetByIdAsync(id);
                permissions = await _userManager.GetGrantedPermissionsAsync(user);

            }

            return permissions.Select(o => o.Name).Where(o=>o.StartsWith("Menu")).ToArray();
        }
        /// <summary>
        /// 分配权限
        /// </summary>
        /// <param name="type"></param>
        /// <param name="permissionName"></param>
        /// <param name="id"></param>
        /// <param name="isGranted"></param>
        /// <returns></returns>
        public virtual async Task AssignPermission(string type,string permissionName,int id,bool isGranted)
        {
            var permission = new Permission(permissionName);

            if (type == "Roles")
            {
                //给角色分配权限
                var role = await _roleManager.GetByIdAsync(Convert.ToInt32(id));

                if (isGranted)
                {
                    await _roleManager.GrantPermissionAsync(role, permission);
                }
                else
                {

                    await _roleManager.ProhibitPermissionAsync(role, permission);
                }
            }
            else if (type == "User")
            {
                //给用户分配权限
                var user = await _userManager.GetByIdAsync(id);

                if (isGranted)
                {
                    await _userManager.GrantPermissionAsync(user, permission);
                }
                else
                {
                    await _userManager.ProhibitPermissionAsync(user, permission);
                }

            }
        }
        /// <summary>
        /// 点击某个导航后获取此导航下的所有权限
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        [DontWrapResult]
        public virtual async Task<ResultPageDto> GetMenuDetailPermissions(string type,int id,string menu)
        {
            var dtoList = new List<PermissionDto>();
            if (string.IsNullOrEmpty(menu))
            {
                
            }
            else
            {
                var menuName = menu.Substring(menu.LastIndexOf('.')+1);
                var modulePermissions = ( _moduleInfoManager.GetAllModulePermissions()).Where(o=>o.Name.StartsWith($"Module.{menuName}."));//获取对应的模块所有权限

                Action<Permission, PermissionDto> action = (a, b) => { };
                if (type == "Roles")
                {
                    action = (permissson, dto) => { dto.IsGranted = _roleManager.IsGrantedAsync(id, permissson).Result; };
                }
                else if (type == "User")
                {
                    action = (permissson, dto) => { dto.IsGranted = _userManager.IsGrantedAsync(id, permissson).Result; };
                }
                dtoList = modulePermissions.Select( o => {
                    var dto = new PermissionDto() {Name=o.Name,DisplayName=o.DisplayName.Localize(_localizationContext) };
                    action(o, dto);
                    //if (type == "Roles")
                    //{
                    //    dto.IsGranted = _roleManager.IsGrantedAsync(id, o).Result;
                    //}else if (type == "Staff")
                    //{
                    //    dto.IsGranted = _userManager.IsGrantedAsync(id, o).Result;
                    //}
                    
                    return dto;
                }).ToList();
            }

            var result = new ResultPageDto()
            {
                code = 0,
                count = dtoList.Count,
                data = dtoList
            };

            return result;
        }

        /// <summary>
        /// 根据用户id删除对应的所有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task DelUserPermissions(int userId)
        {
            await _userManager.DelUserPermissions(userId);
        }
        /// <summary>
        ///  根据角色id添加所有权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task GrantAllPermissions(int id )
        {
            var role = await _roleManager.GetByIdAsync(id);
            await _roleManager.GrantAllPermissionsAsync(role);
        }
    }
}
