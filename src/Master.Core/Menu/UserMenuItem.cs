using Abp.Application.Navigation;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Menu
{
    /// <summary>
    /// 用户菜单项
    /// </summary>

    public class CustomUserMenuItem
    {
        /// <summary>
        /// Unique name of the menu item in the application. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Icon of the menu item if exists.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Display name of the menu item.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The Display order of the menu. Optional.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// The URL to navigate when this menu item is selected.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// A custom object related to this menu item.
        /// </summary>
        public object CustomData { get; set; }

        /// <summary>
        /// Target of the menu item. Can be "_blank", "_self", "_parent", "_top" or a frame name.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Can be used to enable/disable a menu item.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Can be used to show/hide a menu item.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Sub items of this menu item.
        /// </summary>
        public IList<CustomUserMenuItem> Items { get; set; }

        /// <summary>
        public CustomUserMenuItem()
        {

        }

        internal CustomUserMenuItem(MenuItemDefinition menuItemDefinition, ILocalizationContext localizationContext)
        {
            Name = menuItemDefinition.Name;
            Icon = menuItemDefinition.Icon;
            DisplayName = menuItemDefinition.DisplayName.Localize(localizationContext);
            Order = menuItemDefinition.Order;
            Url = menuItemDefinition.Url;
            CustomData = menuItemDefinition.CustomData;
            Target = menuItemDefinition.Target;
            IsEnabled = menuItemDefinition.IsEnabled;
            IsVisible = menuItemDefinition.IsVisible;

            Items = new List<CustomUserMenuItem>();
        }
    }
}
