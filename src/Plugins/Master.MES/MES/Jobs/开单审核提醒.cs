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
    /// 开单审核提醒
    /// </summary>
    public class TaskConfirmMessageJob : BackgroundJob<SendWeiXinMessageJobArgs>, ITransientDependency
    {
        private IHostingEnvironment _hostingEnvironment;
        public RemindLogManager RemindLogManager { get; set; }
        public ProcessTaskManager ProcessTaskManager { get; set; }
        public TaskConfirmMessageJob(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [UnitOfWork]
        public override void Execute(SendWeiXinMessageJobArgs args)
        {
            var task = ProcessTaskManager.GetAll().IgnoreQueryFilters()
                .Include(o => o.CreatorUser)
                .Where(o => o.Id == args.DataId)
                .SingleOrDefault();

            if (task != null)
            {
                var config = _hostingEnvironment.GetAppConfiguration();
                var baseurl = config["base:url"];
                var url = baseurl + "/mes/JGKD?taskid=" + task.Id;

                var templateId = "u99i9f5VikTcDUJw4OL1neDcI3Lt2oYl7y5wmIqHQjA";//模来模往
                //var templateId = "8j4xzh_ke3-ikaLx3iNnoY4yn8A-yEBLllrNxTiv7bY";//云报工
                if (!string.IsNullOrEmpty(config["TemplateId:TaskConfirm"]))
                {
                    templateId = config["TemplateId:TaskConfirm"];
                }
                var message = new
                {
                    first = new TemplateDataItem("你好，收到新的加工开单申请"),
                    keyword1 = new TemplateDataItem(task.Poster),
                    keyword2=new TemplateDataItem(task.ProjectCharger),
                    keyword3 = new TemplateDataItem(task.CreationTime.ToString("yyyy-MM-dd HH:mm:ss")),
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
