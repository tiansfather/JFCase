using Abp.Application.Navigation;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Menu
{
    [AutoMap(typeof(MenuItemDefinition), typeof(CustomUserMenuItem))]
    public class MenuTreeDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int Order { get; set; }
        public IList<MenuTreeDto> Items { get; set; }
    }
}
