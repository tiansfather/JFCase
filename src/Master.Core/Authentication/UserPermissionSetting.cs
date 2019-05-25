using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Authentication
{
    public class UserPermissionSetting:PermissionSetting
    {
        public virtual long UserId { get; set; }
    }
}
