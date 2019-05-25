using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.Language
{
    public class LanguageAppService:MasterAppServiceBase
    {
        public ILanguageManager LanguageManager { get; set; }

        /// <summary>
        /// 获取所有可用语种
        /// </summary>
        /// <returns></returns>
        public virtual LanguageDto Get()
        {
            var dto = new LanguageDto()
            {
                LanguageInfos = LanguageManager.GetLanguages().Where(o => !o.IsDisabled),
                CurrentLanguage = LanguageManager.CurrentLanguage.Name
            };
            return dto;
        }
    }
}
