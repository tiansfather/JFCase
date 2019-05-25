using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Master.Configuration;
using Master.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.MES.Jobs
{
    /// <summary>
    /// 发送询价提醒
    /// </summary>
    public class SendProcessQuoteMessageJob : BackgroundJob<SendWeiXinMessageJobArgs>, ITransientDependency
    {
        public ProcessQuoteManager ProcessQuoteManager { get; set; }
        public IHostingEnvironment HostingEnvironment { get; set; }
        public RemindLogManager RemindLogManager { get; set; }
        [UnitOfWork]
        public override void Execute(SendWeiXinMessageJobArgs args)
        {
            var processQuote = ProcessQuoteManager.GetAll()
                .Include(o => o.Tenant)
                .Where(o => o.Id == args.DataId).SingleOrDefault();

            if (processQuote != null)
            {
                var config = HostingEnvironment.GetAppConfiguration();
                var baseurl = config["base:url"];
                var url = baseurl + "/MES/ProcessQuoteProcessor#/msg/" + processQuote.Id;

                var templateId = "jd3mdo68Y2mmObCZafu5C8Tq5zFu3f9hdZg6rllGB2A";//模来模往
                //var templateId = "Hm3BL2i0zPKLzfAIl0Po71cnVRVTf8rYvA_qaezHZk8";//云报工
                if (!string.IsNullOrEmpty(config["TemplateId:SendProcessQuote"]))
                {
                    templateId = config["TemplateId:SendProcessQuote"];
                }
                var message = new
                {
                    first = new TemplateDataItem($"收到{processQuote.Tenant.TenancyName}询价信息"),
                    keyword1 = new TemplateDataItem(processQuote.QuoteSN),
                    keyword2 = new TemplateDataItem(processQuote.QuoteName),
                    keyword3 = new TemplateDataItem(processQuote.ExpireDate.ToString("yyyy-MM-dd HH:mm:ss")),
                    remark = new TemplateDataItem("请及时处理")
                };
                SendTemplateMessageResult sendResult;
                try
                {
                    sendResult = WeiXin.WeiXinHelper.SendTemplateMessage(args.OpenId, templateId, url, message);
                }
                catch (Exception ex)
                {
                    sendResult = new SendTemplateMessageResult()
                    {
                        errcode = Senparc.Weixin.ReturnCode.系统繁忙此时请开发者稍候再试,
                        errmsg = ex.Message
                    };
                }
                //更新提醒记录状态
                var remindLog = RemindLogManager.GetAll().IgnoreQueryFilters().Where(o => o.Id == args.RemindLogId).SingleOrDefault();
                if (remindLog != null)
                {
                    remindLog.Success = sendResult.ErrorCodeValue == 0;
                    remindLog.SetPropertyValue("errCode", sendResult.errcode);
                    remindLog.SetPropertyValue("errMsg", sendResult.errmsg);
                    RemindLogManager.UpdateAsync(remindLog).GetAwaiter().GetResult();
                }
            }

        }
    }

    /// <summary>
    /// 询价中标提醒
    /// </summary>
    public class SendProcessQuoteChooseMessageJob : BackgroundJob<SendWeiXinMessageJobArgs>, ITransientDependency
    {
        public ProcessQuoteManager ProcessQuoteManager { get; set; }
        public IHostingEnvironment HostingEnvironment { get; set; }
        public RemindLogManager RemindLogManager { get; set; }
        [UnitOfWork]
        public override void Execute(SendWeiXinMessageJobArgs args)
        {
            var processQuote = ProcessQuoteManager.GetAll()
                .Include(o => o.Tenant)
                .Where(o => o.Id == args.DataId).SingleOrDefault();

            if (processQuote != null)
            {
                var config = HostingEnvironment.GetAppConfiguration();
                var baseurl = config["base:url"];
                var url = baseurl + "/MES/ProcessQuoteProcessor#/msg/" + processQuote.Id;

                var templateId = "nNYnBiwnwqvstTqeNfZ1JByKBNqJIPVIi4g8Wj3Bhog";//模来模往
                //var templateId = "XS1LGGjeKHMsyoiKJLDz94wpgwndY4ilOY6y21aoJ_k";//云报工
                if (!string.IsNullOrEmpty(config["TemplateId:SendProcessQuoteChoose"]))
                {
                    templateId = config["TemplateId:SendProcessQuoteChoose"];
                }
                var message = new
                {
                    first = new TemplateDataItem($"您已成功中标{processQuote.Tenant.TenancyName}询价信息"),
                    keyword1 = new TemplateDataItem(processQuote.QuoteSN),
                    keyword2 = new TemplateDataItem(processQuote.QuoteName),
                    remark = new TemplateDataItem("")
                };
                SendTemplateMessageResult sendResult;
                try
                {
                    sendResult = WeiXin.WeiXinHelper.SendTemplateMessage(args.OpenId, templateId, url, message);
                }
                catch (Exception ex)
                {
                    sendResult = new SendTemplateMessageResult()
                    {
                        errcode = Senparc.Weixin.ReturnCode.系统繁忙此时请开发者稍候再试,
                        errmsg = ex.Message
                    };
                }
                //更新提醒记录状态
                var remindLog = RemindLogManager.GetAll().IgnoreQueryFilters().Where(o => o.Id == args.RemindLogId).SingleOrDefault();
                if (remindLog != null)
                {
                    remindLog.Success = sendResult.ErrorCodeValue == 0;
                    remindLog.SetPropertyValue("errCode", sendResult.errcode);
                    remindLog.SetPropertyValue("errMsg", sendResult.errmsg);
                    RemindLogManager.UpdateAsync(remindLog).GetAwaiter().GetResult();
                }
            }

        }
    }


    //[Serializable]
    //public class SendProcessQuoteMessageJobArgs
    //{
    //    public string OpenId { get; set; }
    //    public int  QuoteDetailId{get;set;}
    //    public int RemindLogId { get; set; }
    //}
}
