using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    public interface IRoleManagementConfig
    {
        List<StaticRoleDefinition> StaticRoles { get; }
    }
}
