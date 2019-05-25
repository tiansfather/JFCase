using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Language
{
    public class LanguageDto
    {
        public IEnumerable<LanguageInfo> LanguageInfos { get; set; }
        public string CurrentLanguage { get; set; }
    }
}
