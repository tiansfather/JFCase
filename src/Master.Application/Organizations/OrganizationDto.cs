using Abp.AutoMapper;
using Master.BaseTrees;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Organizations
{
    [AutoMap(typeof(Organization))]
    public class OrganizationDto:BaseTreeDto
    {
        
    }
}
