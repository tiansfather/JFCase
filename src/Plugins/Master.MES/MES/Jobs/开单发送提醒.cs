using Abp.BackgroundJobs;
using Abp.Dependency;
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
    /// 开单发送提醒
    /// </summary>
    public class TaskToSendMessageJob : BackgroundJob<SendWeiXinMessageJobArgs>, ITransientDependency
    {
        private IHostingEnvironment _hostingEnvironment;
        public RemindLogManager RemindLogManager { get; set; }
        public ProcessTaskManager ProcessTaskManager { get; set; }
        public TaskToSendMessageJob(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [UnitOfWork]
        public override void Execute(SendWeiXinMessageJobArgs args)
        {
            var task = ProcessTaskManager.GetAll().IgnoreQueryFilters()
                .Include(o=>o.Supplier)
                .Include(o=>o.ProcessType)
                .Include(o => o.CreatorUser)
                .Where(o => o.Id == args.DataId)
                .SingleOrDefault();

            if (task != null)
            {
                var config = _hostingEnvironment.GetAppConfiguration();
                var baseurl = config["base:url"];
                var url = baseurl + "/mes/JGKD?taskid=" + task.Id;

                var templateId = "wVpgBt0ziOXX3DwWvqosHb7K2G43G61Xj40aS7iY3R4";//master
                //var templateId= "dltvm7BNLgK9d6KxxMn_Sl6uVuaPkV1ywMdZTiudHkk"//demo
                if (!string.IsNullOrEmpty(config["TemplateId:ReportRemind"]))
                {
                    templateId = config["TemplateId:ReportRemind"];
                }
                var message = new
                {
                    first = new TemplateDataItem("你好，有新的加工单需要发送"),
                    keyword1 = new TemplateDataItem(task.ProcessSN),
                    keyword2 = new TemplateDataItem(task.ProcessType.ProcessTypeName),
                    keyword3 =new TemplateDataItem(task.Supplier?.UnitName),
                    keyword4 = new TemplateDataItem(task.KaiDate?.ToString()),
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
}
