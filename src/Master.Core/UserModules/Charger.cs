using Master.Authentication;
using Master.Module;
using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Domain
{
    [InterModule("主管日常管理",BaseType =typeof(User))]
    public class Charger
    {
        [InterColumn(ColumnName = "姓名", VerifyRules = "required", Sort = 1)]
        public string Name { get; set; }
        [InterColumn(ColumnName = "登录用户名", VerifyRules = "required")]
        public string UserName { get; set; }
        [InterColumn(ColumnName ="律师事务所", VerifyRules = "required")]
        public string WorkLocation { get; set; }
        [InterColumn(ColumnName = "所属组织", ColumnType = ColumnTypes.Text, Renderer = "lay-departchoose", DisplayPath = "Organization.DisplayName", Templet = "{{d.organizationId_display?d.organizationId_display:''}}")]
        public int? OrganizationId { get; set; }
    }
}
