using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Navigation;
using Abp.Runtime.Session;
using Abp.Extensions;
using Master.Web.Components;
using Master.Menu;

namespace Master.Web.Components
{
    public class SideBarNavViewComponent : MasterViewComponent
    {
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly IMenuManager _menuManager;
        private readonly IAbpSession _abpSession;
        private readonly IMenuAppService _menuAppService;

        public SideBarNavViewComponent(
            IUserNavigationManager userNavigationManager,
            IMenuManager menuManager,
            IAbpSession abpSession, IMenuAppService menuAppService)
        {
            _userNavigationManager = userNavigationManager;
            _menuManager = menuManager;
            _abpSession = abpSession;
            _menuAppService = menuAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string activeMenu = "")
        {
            //var mainMenu = await _userNavigationManager.GetMenuAsync("MainMenu", _abpSession.ToUserIdentifier());
            //var userMenus = await _menuManager.LoadUserMenu(_abpSession.ToUserIdentifier());
            var userMenus = await _menuAppService.GetUserMenu(_abpSession.ToUserIdentifier());
            var model = new SideBarNavViewModel
            {
                MenuItems= userMenus,
                ActiveMenuItemName = activeMenu
            };

            return View(model);
        }
    }
}
