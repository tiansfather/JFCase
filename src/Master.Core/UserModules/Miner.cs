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
        [InterColumn(ColumnName ="昵称",ValuePath ="Property",Sort =2)]
        public string NickName { get; set; }
        [InterColumn(ColumnName = "头像", ValuePath = "Property", Sort = 3,IsShownInAdvanceSearch =false)]
        public string Avata { get; set; }
        [InterColumn(ColumnName ="律师事务所",Sort =4)]
        public string WorkLocation { get; set; }
        [InterColumn(ColumnName = "有效电子邮箱",Sort =5)]
        public string Email { get; set; }
        [InterColumn(ColumnName = "所属组织", ColumnType = ColumnTypes.Text, Renderer = "lay-departchoose", DisplayPath = "Organization.DisplayName", Templet = "{{d.organizationId_display?d.organizationId_display:''}}",Sort =6)]
        public int? OrganizationId { get; set; }
        [InterColumn(ColumnName ="成品案例",ColumnType =ColumnTypes.Number,IsShownInAdd =false,IsShownInEdit =false,ValuePath ="Property", IsShownInAdvanceSearch = false, Sort = 7,Templet = "<a dataid=\"{{d.id}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"{{d.name}}成品案例\" tips=\"点击查看案例\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/Miner/ShowCase\" onclick=\"func.callModuleButtonEvent()\">{{d.caseNumber}}</a>")]
        public int CaseNumber { get; set; }
        [InterColumn(ColumnName ="推荐排序",Sort =8, Templet = "<input type=\"text\" value=\"{{(d.sort || 999999) == 999999 ? '' : d.sort}}\" size=5 onblur=\"setSort({{ d.id}},this)\"/>")]
        public int Sort { get; set; }
        [InterColumn(ColumnName ="最后登录时间", ColumnType =ColumnTypes.DateTime,DisplayFormat ="yyyy-MM-dd HH:mm:ss",Sort =6)]
        public DateTime? LastLoginTime { get; set; }
    }
}
