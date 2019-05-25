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
    /// 报工后发送微信消息后台任务
    /// </summary>
    public class SendReportWeiXinMessageJob : BackgroundJob<SendWeiXinMessageJobArgs>, ITransientDependency
    {
        private IHostingEnvironment _hostingEnvironment;
        public TacticManager TacticManager { get; set; }
        public RemindLogManager RemindLogManager { get; set; }
        public IRepository<ProcessTaskReport,int> ProcessTaskReportRepository { get; set; }
        private readonly IRepository<Person, int> _personRepository;
        public SendReportWeiXinMessageJob(IRepository<Person, int> personRepository, IHostingEnvironment hostingEnvironment)
        {
            _personRepository = personRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        [UnitOfWork(false)]
        public override void Execute(SendWeiXinMessageJobArgs args)
        {
            var report = ProcessTaskReportRepository.GetAll().Include(o => o.ProcessTask)
                .ThenInclude(o => o.Part)
                .ThenInclude(o => o.Project)
                .Where(o => o.Id == args.DataId)
                .SingleOrDefault();

            if (report != null)
            {
                var config = _hostingEnvironment.GetAppConfiguration();
                var baseurl = config["base:url"];
                var url = baseurl + "/MES/ReportView?id=" + report.Id;
                var reporter = _personRepository.Get(report.ReporterId);
                
                var templateId = "wVpgBt0ziOXX3DwWvqosHb7K2G43G61Xj40aS7iY3R4";//master
                //var templateId= "dltvm7BNLgK9d6KxxMn_Sl6uVuaPkV1ywMdZTiudHkk"//demo
                if (!string.IsNullOrEmpty(config["TemplateId:ReportRemind"]))
                {
                    templateId = config["TemplateId:ReportRemind"];
                }
                var message = new
                {
                    first = new TemplateDataItem("加工进度提醒"),
                    keyword1 = new TemplateDataItem(report.ProcessTask.Part.Project.ProjectSN),
                    keyword2 = new TemplateDataItem(report.ProcessTask.Part.PartName+report.ProcessTask.ProcessType.ProcessTypeName+report.ReportType.ToString()),
                    keyword3 = new TemplateDataItem(reporter.Name),
                    keyword4 = new TemplateDataItem(report.ReportTime.ToString("yyyy-MM-dd HH:mm:ss")),
                    remark = new TemplateDataItem(report.Remarks)
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
                    Logger.Error(ex.Message, ex);
                }
                //更新提醒记录状态
                var remindLog = RemindLogManager.GetAll().IgnoreQueryFilters().Where(o => o.Id == args.RemindLogId).SingleOrDefault();
                if (remindLog != null)
                {
                    remindLog.Success = sendResult.ErrorCodeValue == 0;
                    remindLog.SetPropertyValue("errCode", sendResult.errcode);
                    remindLog.SetPropertyValue("errMsg", sendResult.errmsg);
                    RemindLogManager.UpdateAsync(remindLog).GetAwaiter().GetResult();
                    CurrentUnitOfWork.SaveChanges();
                }
                //如果是失败状态，抛出异常，使后台作业自动重试
                if (sendResult.ErrorCodeValue != 0)
                {
                    throw new Exception("SendError:" + sendResult.ErrorCodeValue);
                }
            }

        }
    }

    //[Serializable]
    //public class SendReportWeiXinMessageJobArgs
    //{
    //    public string OpenId { get; set; }
    //    public int ReportId { get; set; }
    //    public int RemindLogId { get; set; }
    //}
}
