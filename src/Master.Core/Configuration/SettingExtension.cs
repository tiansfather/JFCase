using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Localization;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    public static class SettingExtension
    {
        /// <summary>
        /// 通过设置信息构建列信息
        /// </summary>
        /// <param name="settingDefinition"></param>
        /// <returns></returns>
        public static ColumnInfo BuildColumnInfo(this SettingDefinition settingDefinition)
        {            
            using(var localizationContextWrapper = IocManager.Instance.ResolveAsDisposable<ILocalizationContext>())
            {
                var columnInfo = new ColumnInfo()
                {
                    ColumnKey = settingDefinition.Name,
                    ColumnName = settingDefinition.DisplayName.Localize(localizationContextWrapper.Object)                    
                };

                //通过设置定义中的自定义信息获取
                var settingUIInfo = settingDefinition.CustomData as SettingUIInfo;
                if (settingUIInfo != null)
                {
                    settingUIInfo.MapTo(columnInfo);
                    //columnInfo.ColumnType = settingUIInfo.ColumnType;
                    //columnInfo.Renderer = settingUIInfo.Renderer;
                    //columnInfo.Tips = settingUIInfo.Tips;
                }

                return columnInfo;
            }
            
        }
    }
}
