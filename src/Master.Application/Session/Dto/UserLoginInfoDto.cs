using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Master.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Session.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string UserName { get; set; }

        public string HomeUrl { get; set; }

        public List<string> RoleNames { get; set; } = new List<string>();
        public List<string> RoleDisplayNames { get; set; } = new List<string>();
        public List<string> DepartNames { get; set; } = new List<string>();
    }
}
