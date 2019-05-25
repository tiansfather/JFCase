using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    internal class RoleManagementConfig : IRoleManagementConfig
    {
        public List<StaticRoleDefinition> StaticRoles { get; }

        public RoleManagementConfig()
        {
            StaticRoles = new List<StaticRoleDefinition>();
        }
    }
}
