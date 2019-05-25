using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 录入提交Dto
    /// </summary>
    
    public class AccountingDto
    {
        public String Ids { get; set; }
        public bool Flag { get; set; }
        public string Base64 { get; set; }
        public string ImgPath { get; set; }
    }
}
