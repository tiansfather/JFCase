using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Organizations
{
    public interface IMayHaveOrganization
    {
        int? OrganizationId { get; set; }
    }
    public interface IMustHaveOrganization
    {
        int OrganizationId { get; set; }
    }
}
