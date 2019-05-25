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
    /// 将加工单发给加工点的提醒
    /// </summary>
    public class SendOuterTaskWeiXinMessageJob : BackgroundJob<SendWeiXinMessageJobArgs>, ITransientDependency
    {
        private IHostingEnvironment _hostingEnvironment;
        public RemindLogManager RemindLogManager { get; set; }
        public ProcessTaskManager ProcessTaskManager { get; set; }
        public SendOuterTaskWeiXinMessageJob( IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [UnitOfWork]
        public override void Execute(SendWeiXinMessageJobArgs args)
        {
            var task = ProcessTaskManager.GetAll()
                .Include(o=>o.Tenant)
                .Include(o=>o.ProcessType)
                .Include(o => o.Part).ThenInclude(o => o.Project)
                .Where(o => o.Id == args.DataId)
                .SingleOrDefault();

            if (task != null)
            {
                //图片之前已生成,不进行重新生成
                //ProcessTaskManager.SaveTaskSheetToImage2(task).GetAwaiter().GetResult();
                var config = _hostingEnvironment.GetAppConfiguration();
                var baseurl = config["base:url"];
                var url = baseurl + "/MES/OuterTaskView?id=" + task.Id;

                var templateId = "wVpgBt0ziOXX3DwWvqosHb7K2G43G61Xj40aS7iY3R4";//模来模往
                //var templateId = "dltvm7BNLgK9d6KxxMn_Sl6uVuaPkV1ywMdZTiudHkk";//云报工
                if (!string.IsNullOrEmpty(config["TemplateId:SendOuterTask"]))
                {
                    templateId = config["TemplateId:SendOuterTask"];
                }
                var message = new
                {
                    first = new TemplateDataItem("你好，收到"+task.Tenant.TenancyName+"新的加工任务"),
                    keyword1 = new TemplateDataItem(task.Part.Project.ProjectSN),
                    keyword2 = new TemplateDataItem(task.Part.PartName + task.ProcessType.ProcessTypeName ),
                    keyword3 = new TemplateDataItem(task.Part.Project.GetPropertyValue<string>("ProjectCharger")),
                    keyword4 = new TemplateDataItem(task.RequireDate?.ToString("yyyy-MM-dd HH:mm:ss")),
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
