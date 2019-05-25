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
    public class SendTacticBindWeiXinMessageJob : BackgroundJob<SendTacticBindWeiXinMessageJobArgs>, ITransientDependency
    {
        public TacticManager TacticManager { get; set; }
        public RemindLogManager RemindLogManager { get; set; }
        private IHostingEnvironment _hostingEnvironment;

        private readonly IRepository<Person, int> _personRepository;
        public SendTacticBindWeiXinMessageJob(IRepository<Person, int> personRepository, IHostingEnvironment hostingEnvironment)
        {
            _personRepository = personRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        [UnitOfWork]
        public override void Execute(SendTacticBindWeiXinMessageJobArgs args)
        {
            var config = _hostingEnvironment.GetAppConfiguration();
            var tactic = TacticManager.Repository.GetAll().IgnoreQueryFilters().Where(o => o.Id == args.TacticId && o.IsActive && !o.IsDeleted).SingleOrDefault();
            var person = _personRepository.Single(o => o.Id == args.PersonId);
            if (tactic != null && person != null)
            {
                string templateId;
                object message;
                if (args.BindType == 1)
                {
                    templateId = "h3S7JP_4Okm52Kwn-9aOgkeoKdXKh0cNKuorU2vR_QI";
                    if (!string.IsNullOrEmpty(config["TemplateId:SendTacticBindWeiXinBind"]))
                    {
                        templateId = config["TemplateId:SendTacticBindWeiXinBind"];
                    }
                    message = new
                    {
                        first = new TemplateDataItem("提醒策略绑定成功"),
                        keyword1 = new TemplateDataItem(person.Name),
                        keyword2 = new TemplateDataItem($"您已成功绑定策略\"{tactic.TacticName}\""),
                        remark = new TemplateDataItem("欢迎使用模来模往")
                    };
                }
                else
                {
                    templateId = "uOBfBNq_dDCW99glamDGqcdekP3qeJxJGyCJu5Ipnko";
                    if (!string.IsNullOrEmpty(config["TemplateId:SendTacticBindWeiXinUnBind"]))
                    {
                        templateId = config["TemplateId:SendTacticBindWeiXinUnBind"];
                    }
                    message = new
                    {
                        first = new TemplateDataItem($"您已成功解除策略\"{tactic.TacticName}\"的绑定"),
                        keyword1 = new TemplateDataItem(person.Name),
                        keyword2 = new TemplateDataItem(""),
                        remark = new TemplateDataItem("欢迎使用模来模往")
                    };
                }
                SendTemplateMessageResult sendResult;
                try
                {
                    sendResult = WeiXin.WeiXinHelper.SendTemplateMessage(person.GetPropertyValue<string>("OpenId"), templateId, "", message);
                }
                catch(Exception ex)
                {
                    sendResult = new SendTemplateMessageResult()
                    {
                        errcode=Senparc.Weixin.ReturnCode.系统繁忙此时请开发者稍候再试,
                        errmsg=ex.Message
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

    [Serializable]
    public class SendTacticBindWeiXinMessageJobArgs
    {
        /// <summary>
        /// 类型，1为绑定，0为解绑
        /// </summary>
        public int BindType { get; set; }
        public int PersonId { get; set; }
        public int TacticId { get; set; }
        public int RemindLogId { get; set; }
    }
}
