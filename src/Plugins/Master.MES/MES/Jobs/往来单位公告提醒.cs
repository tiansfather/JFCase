using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Master.Configuration;
using Master.Entity;
using Master.Notices;
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
    /// 发送往来单位公告提醒
    /// </summary>
    public class SendUnitNoticeMessageJob : BackgroundJob<SendWeiXinMessageJobArgs>, ITransientDependency
    {
        public IHostingEnvironment HostingEnvironment { get; set; }
        public RemindLogManager RemindLogManager { get; set; }
        public NoticeManager NoticeManager { get; set; }
        [UnitOfWork]
        public override void Execute(SendWeiXinMessageJobArgs args)
        {
            var notice = NoticeManager.GetAll()
                .IgnoreQueryFilters()
                .Include(o => o.Tenant)
                .Where(o => o.Id == args.DataId)
                .SingleOrDefault();

            if (notice != null)
            {
                var baseurl = HostingEnvironment.GetAppConfiguration()["base:url"];
                var url = baseurl + "/MES/TenantNoticeView?id=" + args.DataId;

                var templateId = "r5azOIRD5TP2heIi-6TTF29h_0TvyM4gruh4wy4YXMQ";//模来模往
                //var templateId = "MPrQIbyjLaa-grTkEihsEPEWtCUvvguDk6mtmMO0hnc";//云报工
                var message = new
                {
                    first = new TemplateDataItem($"收到{notice.Tenant.TenancyName}公告消息"),
                    keyword1 = new TemplateDataItem(notice.Tenant.TenancyName),
                    keyword2 = new TemplateDataItem(notice.NoticeTitle),
                    //keyword3 = new TemplateDataItem(processQuote.PublishDate?.ToString("yyyy-MM-dd")),
                    remark = new TemplateDataItem("请及时查看")
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

    
}
