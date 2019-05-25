using Abp.Specifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Master.MES.Specifications
{
    public class TimeLineTaskSpecification : Specification<ProcessTask>
    {
        private DateTime startDate;
        private DateTime endDate;

        public TimeLineTaskSpecification(DateTime from,DateTime to)
        {
            startDate = from;
            endDate = to;
        }

        public override Expression<Func<ProcessTask, bool>> ToExpression()
        {
            return o =>
                (o.ArrangeDate == null && o.StartDate == null && o.AppointDate==null && (DateTime.Now >= startDate && DateTime.Now <= endDate)) || //如果没有设置任何时间，则按当前时间查询
                (o.ArrangeDate == null && o.StartDate == null && o.AppointDate.HasValue && (o.AppointDate.Value >= startDate && o.AppointDate.Value <= endDate)) ||//如果未排机，只显示未上机的，则按预约时间查询
                (o.EquipmentId != null && o.StartDate == null && o.ArrangeDate != null && (o.ArrangeDate.Value >= startDate && o.ArrangeDate.Value <= endDate ||
                o.ArrangeEndDate >= startDate && o.ArrangeEndDate <= endDate)) ||//如果未上机，则按安排上下机时间查询
                (o.EquipmentId != null && o.StartDate != null && o.EndDate == null && (o.StartDate.Value >= startDate && o.StartDate.Value <= endDate ||
                o.StartDate.Value.AddHours(Convert.ToDouble(o.EstimateHours ?? 0)) >= startDate && o.StartDate.Value.AddHours(Convert.ToDouble(o.EstimateHours ?? 0)) <= endDate)) || //如果已上机未下机，则按实际上机时间查询
                (o.EquipmentId != null && o.StartDate != null && o.EndDate != null && (o.StartDate.Value >= startDate && o.StartDate.Value <= endDate ||
                o.EndDate >= startDate && o.EndDate <= endDate));//已下机的
        }
    }
}
