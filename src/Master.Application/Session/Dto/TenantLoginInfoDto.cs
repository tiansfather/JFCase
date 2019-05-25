using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Master.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Session.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }
        public string EditionName { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string ExpireDate { get; set; }
    }
}
