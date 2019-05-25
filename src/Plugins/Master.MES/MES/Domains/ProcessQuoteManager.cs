using Abp.BackgroundJobs;
using Abp.UI;
using Master.Domain;
using Master.Entity;
using Master.MES.Jobs;
using Master.Module;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES
{
    public class ProcessQuoteManager: ModuleServiceBase<ProcessQuote,int>
    {
        //public RemindLogManager RemindLogManager { get; set; }
        //public MESUnitManager MESUnitManager { get; set; }
        //public ProcessTaskManager ProcessTaskManager { get; set; }
        public IBackgroundJobManager BackgroundJobManager { get; set; }
        private static object _lockObj = new object();



        /// <summary>
        /// 发布一个询价
        /// </summary>
        /// <param name="processQuoteId"></param>
        /// <returns></returns>
        public virtual async Task Publish(int processQuoteId)
        {
            var mesUnitManager = Resolve<MESUnitManager>();
            var remindLogManager = Resolve<RemindLogManager>();

            var processQuote = await GetAll()
                .Include("ProcessQuoteBids.Unit")
                .Where(o => o.Id == processQuoteId)
                .SingleOrDefaultAsync();

            if (processQuote == null)
            {
                throw new UserFriendlyException(L("数据错误"));
            }
            if (processQuote.QuoteStatus != QuoteStatus.草稿 && processQuote.QuoteStatus!=QuoteStatus.询价中)
            {
                throw new UserFriendlyException(L("当前询价状态不允许发布"));
            }
            
            if (processQuote.ProcessQuoteBids.Count(o =>o.Unit==null || !o.Unit.GetTenantId().HasValue) > 0)
            {
                throw new UserFriendlyException(L("所有加工点均绑定账套后方可发布"));
            }
            if (processQuote.QuoteStatus == QuoteStatus.草稿)
            {
                //更改询价状态
                processQuote.QuoteStatus = QuoteStatus.询价中;
                processQuote.PublishDate = DateTime.Now;
                GenerateQuoteSN(processQuote);
            }

            #region 发送提醒
            //对于所有未发送的投标明细进行发送
            foreach (var bidInfo in processQuote.ProcessQuoteBids.Where(o => o.QuoteBidStatus == QuoteBidStatus.未发送))
            {
                bidInfo.QuoteBidStatus = QuoteBidStatus.待投标;
                bidInfo.ToTenantId = bidInfo.Unit.GetTenantId();
                //被提醒人微信openid
                var openId = "";
                try
                {
                    openId = (await mesUnitManager.FindUnitOpenId(bidInfo.Unit))[0];
                }
                catch (Exception ex)
                {

                }
                if (!string.IsNullOrEmpty(openId))
                {
                    //进行发送提醒
                    //先产生一条提醒记录
                    var remindLog = new RemindLog()
                    {
                        RemindType = "询价提醒",
                        Name = bidInfo.Unit.UnitName,
                        TenantId = processQuote.TenantId
                    };
                    var remindLogId = await remindLogManager.InsertAndGetIdAsync(remindLog);
                    var arg = new SendWeiXinMessageJobArgs()
                    {
                        OpenId = openId,
                        DataId = processQuote.Id,
                        RemindLogId = remindLogId
                    };

                    BackgroundJobManager.Enqueue<SendProcessQuoteMessageJob, SendWeiXinMessageJobArgs>(arg);
                }

            }
            #endregion

            #region 到截止日期自动截止
            //设定定时任务，当询价截止时改变询价单状态
            var expireArg = new CheckExpireQuotesJobsArgs()
            {
                QuoteId = processQuoteId
            };
            var jobId = await BackgroundJobManager.EnqueueAsync<CheckExpireQuotesJobs, CheckExpireQuotesJobsArgs>(expireArg, BackgroundJobPriority.Normal, processQuote.ExpireDate - DateTime.Now);
            //将任务Id保存，以便在到期日期发生变化时取消此任务
            processQuote.SetPropertyValue("ExpireJobId", jobId);
            #endregion


        }
        /// <summary>
        /// 选择中标方
        /// </summary>
        /// <param name="processQuote"></param>
        /// <param name="bidId"></param>
        /// <returns></returns>
        public virtual async Task Choose(ProcessQuote processQuote,int bidId)
        {
            var processTaskManager = Resolve<ProcessTaskManager>();
            var mesUnitManager = Resolve<MESUnitManager>();
            var remindLogManager = Resolve<RemindLogManager>();

            var bid = processQuote.ProcessQuoteBids.Where(o => o.Id == bidId).Single();
            if (bid.QuoteBidStatus != QuoteBidStatus.已投标)
            {
                throw new UserFriendlyException(L("只能选择已投标状态的投标方"));
            }
            processQuote.ChooseUserId = AbpSession.UserId;//设置选标人
            processQuote.QuoteStatus = QuoteStatus.已选标;
            bid.QuoteBidStatus = QuoteBidStatus.已中标;
            //加工任务关联的，则直接进行开单
            foreach(var quoteTask in processQuote.ProcessQuoteTasks.Where(o => o.ProcessTaskId != null))
            {
                quoteTask.ProcessTask.SupplierId = bid.UnitId;
                quoteTask.ProcessTask.JobFee = bid.BidData.Cost;//设置中标金额为初始金额
                quoteTask.ProcessTask.SetPropertyValue("SubmitFeeFromProcessorDto", new { Fee=bid.BidData.Cost, Info=bid.Remarks });//设置回单金额
                await processTaskManager.KaiDan(quoteTask.ProcessTask);
            }
            //设置其余投标为未中标,但是已放弃的投标不改变其状态
            foreach (var otherBid in processQuote.ProcessQuoteBids.Where(o => o.Id != bidId && o.QuoteBidStatus != QuoteBidStatus.已放弃))
            {
                otherBid.QuoteBidStatus = QuoteBidStatus.未中标;
            }
            //发送中标提醒
            //被提醒人微信openid
            var openId = "";
            try
            {
                openId = (await mesUnitManager.FindUnitOpenId(bid.Unit))[0];
            }
            catch (Exception ex)
            {

            }
            if (!string.IsNullOrEmpty(openId))
            {
                //进行发送提醒
                //先产生一条提醒记录
                var remindLog = new RemindLog()
                {
                    RemindType = "询价提醒",
                    Name = bid.Unit.UnitName,
                    TenantId = processQuote.TenantId
                };
                var remindLogId = await remindLogManager.InsertAndGetIdAsync(remindLog);
                var arg = new SendWeiXinMessageJobArgs()
                {
                    OpenId = openId,
                    DataId = processQuote.Id,
                    RemindLogId = remindLogId
                };

                BackgroundJobManager.Enqueue<SendProcessQuoteChooseMessageJob, SendWeiXinMessageJobArgs>(arg);
            }
        }

        /// <summary>
        /// 生成询价单号
        /// </summary>
        /// <param name="processQuote"></param>
        /// <returns></returns>
        public virtual void GenerateQuoteSN(ProcessQuote processQuote)
        {
            if (string.IsNullOrEmpty(processQuote.QuoteSN))
            {
                lock (_lockObj)
                {
                    //获取当天所有询价单最后一个单号
                    var dayPrefix = DateTime.Now.ToString("yyyyMMdd");
                    var codeNo = "001";
                    var lastQuote = GetAll().IgnoreQueryFilters().Where(o => o.QuoteSN.StartsWith(dayPrefix)).OrderBy(o => o.QuoteSN).LastOrDefault();
                    if (lastQuote != null)
                    {
                        codeNo = (int.Parse(lastQuote.QuoteSN.Substring(lastQuote.QuoteSN.Length - 3)) + 1).ToString().PadLeft(3, '0');
                    }                    
                    processQuote.QuoteSN = $"{dayPrefix}{codeNo}";
                }
            }

        }
    }
}
