using Abp.BackgroundJobs;
using Abp.Dependency;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Jobs
{
    /// <summary>
    /// 有新用户注册后的提醒
    /// </summary>
    public class SendRegisteredMessageJob : BackgroundJob<SendRegisteredMessageJobArgs>, ITransientDependency
    {
        public override void Execute(SendRegisteredMessageJobArgs args)
        {
            var templateId = "8YRKA8IAzlqaOjgLuuZSOhkVbGRgAsK79MA47txODgo";
            var message = new
            {
                first = new TemplateDataItem("新用户注册提醒"),
                keyword1= new TemplateDataItem(args.PersonName),
                keyword2= new TemplateDataItem(args.RegisterDate.ToString("yyyy-MM-dd HH:mm")),
                keyword3= new TemplateDataItem(args.Mobile),
                remark= new TemplateDataItem(args.TenancyName),
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
            //var sendResult = WeiXin.WeiXinHelper.SendTemplateMessage(args.OpenId, templateId, "", message);
        }
    }
    [Serializable]
    public class SendRegisteredMessageJobArgs
    {
        public string OpenId { get; set; }
        public string Mobile { get; set; }
        public string TenancyName { get; set; }
        public string PersonName { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
