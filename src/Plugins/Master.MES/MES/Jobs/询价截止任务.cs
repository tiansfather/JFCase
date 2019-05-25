using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.MES.Jobs
{
    /// <summary>
    /// 询价单到截止时间后任务
    /// </summary>
    public class CheckExpireQuotesJobs : BackgroundJob<CheckExpireQuotesJobsArgs>, ITransientDependency
    {
        public ProcessQuoteManager ProcessQuoteManager { get; set; }

        [UnitOfWork]
        public override void Execute(CheckExpireQuotesJobsArgs args)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant))
            {
                //获取对应的询价中询价单
                var quote = ProcessQuoteManager.GetAll()
                    .Where(o=>o.QuoteStatus==QuoteStatus.询价中)
                    .Include(o => o.ProcessQuoteBids)
                    .Where(o => o.Id == args.QuoteId)
                    .SingleOrDefault();
                if (quote != null)
                {
                    if (quote.ProcessQuoteBids.Count(o => o.BidType != null) > 0)
                    {
                        //有人投标，则设询价单为已截止状态
                        quote.QuoteStatus = QuoteStatus.已截止;
                    }
                    else
                    {
                        quote.QuoteStatus = QuoteStatus.已流标;
                    }

                    CurrentUnitOfWork.SaveChanges();
                }

                
            }
        }
    }

    public class CheckExpireQuotesJobsArgs
    {
        public int QuoteId { get; set; }
    }
}
