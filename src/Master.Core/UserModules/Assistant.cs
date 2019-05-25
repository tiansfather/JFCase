using Master.Authentication;
using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Domain
{
    [InterModule("助理日常管理",BaseType =typeof(User))]
    public class Assistant
    {
        [InterColumn(ColumnName = "姓名", VerifyRules = "required", Sort = 1)]
        public string Name { get; set; }        
        [InterColumn(ColumnName ="律师事务所", VerifyRules = "required")]
        public string WorkLocation { get; set; }
        [InterColumn(ColumnName = "登录用户名", VerifyRules = "required")]
        public string UserName { get; set; }
        [InterColumn(ColumnName = "有效电子邮箱", VerifyRules = "required")]
        public string Email { get; set; }
        [InterColumn(ColumnName = "判例汇总",ValuePath ="Property",IsShownInAdd =false,IsShownInEdit =false,IsShownInAdvanceSearch =false)]
        public int InputCaseNumber { get; set; }
    }
}
