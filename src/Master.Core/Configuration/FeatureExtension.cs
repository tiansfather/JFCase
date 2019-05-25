using Abp.Application.Features;
using Abp.Dependency;
using Abp.Localization;
using Abp.UI.Inputs;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    public static class FeatureExtension
    {
        /// <summary>
        /// 将特性构建成列定义信息
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public static ColumnInfo BuildColumnInfo(this Feature feature)
        {
            using (var localizationContextWrapper = IocManager.Instance.ResolveAsDisposable<ILocalizationContext>())
            {
                var columnInfo = new ColumnInfo()
                {
                    ColumnKey = feature.Name,
                    ColumnName = feature.DisplayName.Localize(localizationContextWrapper.Object)
                };
                //todo:
                if(feature.InputType is CheckboxInputType)
                {
                    columnInfo.ColumnType = ColumnTypes.Switch;
                }
                return columnInfo;
            }

        }
    }
}
