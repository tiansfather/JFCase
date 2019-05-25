using Master.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Authentication
{
    public class RolePermissionSetting : PermissionSetting
    {
        /// <summary>
        /// Role id.
        /// </summary>
        public virtual int RoleId { get; set; }
    }
}
