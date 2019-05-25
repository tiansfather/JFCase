using Master.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master.Web.Models.Account
{
    public class LoginFormViewModel
    {
        public string UserName { get; set; }
        public string ReturnUrl { get; set; }

        public List<Tenant> Tenants { get; set; }

        public string CurrentTenancyName { get; set; }
    }
}
