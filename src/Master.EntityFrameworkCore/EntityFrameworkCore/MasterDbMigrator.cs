using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.Extensions;
using Abp.MultiTenancy;
using Master.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Master.EntityFrameworkCore
{
    public class MasterDbMigrator:DbMigrator<MasterDbContext>
    {
        
    }
}
