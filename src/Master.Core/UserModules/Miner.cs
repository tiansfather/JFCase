using Master.Authentication;
using Master.Module;
using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Domain
{
    [InterModule("矿工日常管理",BaseType =typeof(User))]
    public class Miner
    {
        [InterColumn(ColumnName = "姓名", Sort = 1)]
        public string Name { get; set; }
        [InterColumn(ColumnName ="律师事务所")]
        public string WorkLocation { get; set; }
        [InterColumn(ColumnName = "有效电子邮箱")]
        public string Email { get; set; }
        [InterColumn(ColumnName = "所属组织", ColumnType = ColumnTypes.Text, Renderer = "lay-departchoose", DisplayPath = "Organization.DisplayName", Templet = "{{d.organizationId_display?d.organizationId_display:''}}")]
        public int? OrganizationId { get; set; }
    }
}
