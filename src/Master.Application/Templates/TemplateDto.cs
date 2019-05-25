using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Templates
{
    [AutoMap(typeof(Template))]
    public class TemplateDto
    {
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public string TemplateType { get; set; }
        public string TemplateContent { get; set; }
        
    }
}
