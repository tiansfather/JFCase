using Abp.Application.Navigation;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Menu
{
    public interface IMenuRender:ITransientDependency
    {
        //设置系统菜单
        void RenderMenu(List<MenuItemDefinition> menus);
    }
}
