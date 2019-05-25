using Abp.Specifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Master.MES.Specifications
{
    /// <summary>
    /// 延期任务的筛选
    /// </summary>
    public class DelayTaskSpecification : Specification<ProcessTask>
    {
        public override Expression<Func<ProcessTask, bool>> ToExpression()
        {
            return p => (p.EndDate != null && p.EndDate > p.RequireDate) || (p.EndDate == null && DateTime.Now > p.RequireDate);
        }
    }
}
