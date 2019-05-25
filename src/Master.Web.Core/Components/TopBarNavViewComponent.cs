using Abp.Application.Navigation;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Master.Menu;
using Master.Web.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master.Web.Components
{
    public class TopBarNavViewComponent:MasterViewComponent
    {
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly IAbpSession _abpSession;
        private readonly IMenuAppService _menuAppService;

        public TopBarNavViewComponent(
            IUserNavigationManager userNavigationManager,
            IAbpSession abpSession, IMenuAppService menuAppService)
        {
            _userNavigationManager = userNavigationManager;
            _abpSession = abpSession;
            _menuAppService = menuAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var userMenus = await _menuManager.LoadUserMenu(_abpSession.ToUserIdentifier());
            var userMenus = await _menuAppService.GetUserMenu(_abpSession.ToUserIdentifier());
            //var mainMenu = await _userNavigationManager.GetMenuAsync("MainMenu", _abpSession.ToUserIdentifier());


            return View(userMenus);
        }
    }
}
