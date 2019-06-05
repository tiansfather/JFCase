using Abp.AutoMapper;
using Master.Entity;
using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Authentication
{
    [InterModule("审核新矿工")]
    [AutoMap(typeof(User))]
    public class NewMiner:BaseFullEntity
    {      
        public string OpenId { get; set; }
        
        [InterColumn(ColumnName = "微信头像",Templet = "<img class=\"thumb\" src=\"{{d.avata}}\" width=30 />")]
        public string Avata { get; set; }
        [InterColumn(ColumnName = "昵称")]
        public string NickName { get; set; }
        [InterColumn(ColumnName = "姓名")]
        public string Name { get; set; }
        [InterColumn(ColumnName = "律师事务所")]
        public string WorkLocation { get; set; }
        [InterColumn(ColumnName = "手机号码")]
        public string PhoneNumber { get; set; }
        [InterColumn(ColumnName = "邮箱")]
        public string Email { get; set; }
        [InterColumn(ColumnName = "申请时间",ColumnType =Module.ColumnTypes.DateTime)]
        public override DateTime CreationTime { get; set; }
        [InterColumn(ColumnName = "留言")]
        public override string Remarks { get; set; }
        public bool Verified { get; set; }
    }
}
