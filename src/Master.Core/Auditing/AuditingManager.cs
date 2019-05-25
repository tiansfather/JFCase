using Abp.Auditing;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Master.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Auditing
{
    /// <summary>
    /// 审记日志
    /// </summary>
    public class AuditLogManager : DomainServiceBase<AuditLog,int>, IAuditingStore, ITransientDependency
    {        

        public virtual Task SaveAsync(AuditInfo auditInfo)
        {
            return Repository.InsertAsync(AuditLog.CreateFromAuditInfo(auditInfo));
        }
    }
}
