using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
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
    /// 任务延期发送微信消息后台任务
    /// </summary>
    public class SendDelayTaskWeiXinMessageJob : BackgroundJob<SendWeiXinMessageJobArgs>, ITransientDependency
    {
        private IHostingEnvironment _hostingEnvironment;
        public TacticManager TacticManager { get; set; }
        public RemindLogManager RemindLogManager { get; set; }
        public ProcessTaskManager ProcessTaskManager { get; set; }
        private readonly IRepository<Person, int> _personRepository;
        public SendDelayTaskWeiXinMessageJob(IRepository<Person, int> personRepository, IHostingEnvironment hostingEnvironment)
        {
            _personRepository = personRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        [UnitOfWork]
        public override void Execute(SendWeiXinMessageJobArgs args)
        {
            
            var task = ProcessTaskManager.GetAll()
                .Include(o=>o.Supplier)
                .Include(o=>o.ProcessType)
                .Include(o => o.Part)
                .ThenInclude(o => o.Project)
                .Where(o => o.Id == args.DataId)
                .SingleOrDefault();

            if (task != null)
            {
                var remark = "";
                var delayType = Enum.Parse<DelayType>(args.ExtendInfo);
                switch (delayType)
                {
                    case DelayType.DelayStart:
                        remark = $"预约日期:{task.AppointDate.Value.ToString("yyyy-MM-dd")},任务已延期{(DateTime.Now-task.AppointDate.Value).Days}天未上机";
                        break;
                    case DelayType.DelayEnd:
                        remark = $"要求完成日期:{task.RequireDate.Value.ToString("yyyy-MM-dd")},任务已延期{(DateTime.Now - task.RequireDate.Value).Days}天未下机";
                        break;
                    case DelayType.ReceiveNotStart:
                        remark = $"到料日期:{task.ReceiveDate.Value.ToString("yyyy-MM-dd")},任务已到料{(DateTime.Now - task.ReceiveDate.Value).Days}天尚未上机";
                        break;
                }
                var templateId = "wVpgBt0ziOXX3DwWvqosHb7K2G43G61Xj40aS7iY3R4";
                var message = new
                {
                    first = new TemplateDataItem("延期提醒"),
                    keyword1 = new TemplateDataItem(task.Part.Project.ProjectSN),
                    keyword2 = new TemplateDataItem(task.Part.PartName + task.ProcessType.ProcessTypeName ),
                    keyword3 = new TemplateDataItem(task.Supplier?.UnitName),
                    //keyword4 = new TemplateDataItem(report.ReportTime.ToString("yyyy-MM-dd HH:mm:ss")),
                    remark = remark
                };
                SendTemplateMessageResult sendResult;
                try
                {
                    sendResult = WeiXin.WeiXinHelper.SendTemplateMessage(args.OpenId, templateId, "", message);
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
