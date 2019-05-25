using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Session.Dto
{
    public class LoginInformationDto
    {
        public ApplicationInfoDto Application { get; set; }

        public UserLoginInfoDto User { get; set; }

        public TenantLoginInfoDto Tenant { get; set; }
    }
}
