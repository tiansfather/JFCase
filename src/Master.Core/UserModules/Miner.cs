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
        [InterColumn(ColumnName ="律师事务所",Sort =2)]
        public string WorkLocation { get; set; }
        [InterColumn(ColumnName = "有效电子邮箱",Sort =3)]
        public string Email { get; set; }
        [InterColumn(ColumnName = "所属组织", ColumnType = ColumnTypes.Text, Renderer = "lay-departchoose", DisplayPath = "Organization.DisplayName", Templet = "{{d.organizationId_display?d.organizationId_display:''}}",Sort =4)]
        public int? OrganizationId { get; set; }
        [InterColumn(ColumnName ="成品案例",ColumnType =ColumnTypes.Number,IsShownInAdd =false,IsShownInEdit =false,ValuePath ="Property", IsShownInAdvanceSearch = false, Sort = 5,Templet = "<a dataid=\"{{d.id}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"{{d.name}}成品案例\" tips=\"点击查看案例\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/Miner/ShowCase\" onclick=\"func.callModuleButtonEvent()\">{{d.caseNumber}}</a>")]
        public int CaseNumber { get; set; }
    }
}
