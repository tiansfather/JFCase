using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Master.Configuration;
using Master.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Master.MultiTenancy;

namespace Master.MES.Jobs
{
    /// <summary>
    /// 将核算单发给加工点的提醒
    /// </summary>
    public class SendCountingWeiXinMessageJob : BackgroundJob<SendWeiXinMessageJobArgs>, ITransientDependency
    {
        private IHostingEnvironment _hostingEnvironment;
        public RemindLogManager RemindLogManager { get; set; }
        public ProcessTaskManager ProcessTaskManager { get; set; }
        public TenantManager TenantManager { get; set; }
        public SendCountingWeiXinMessageJob( IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [UnitOfWork]
        public override void Execute(SendWeiXinMessageJobArgs args)
        {
            Logger.Error("\r\n\r\nstart");
            //JObject jo = (JObject)JsonConvert.DeserializeObject(args.ExtendInfo);
            var tenantId = args.DataId; //Supplier. tenantId
            var arr = args.ExtendInfo.Split(new string[] { "test123" }, StringSplitOptions.None);
            var taskIds = arr[0];
            var currentTenantId = arr[1];
            var totalFee = arr[2];
            var processSNs = arr[3];
     
            var q =  TenantManager.GetAll().Where(o => o.Id == Convert.ToInt32( currentTenantId)).SingleOrDefault(); 
            var currentUser = q.TenancyName;
           var partUrl= ProcessTaskManager.SaveAccountingSheetToImage(taskIds, currentTenantId, tenantId.ToString()).GetAwaiter().GetResult(); 
        partUrl= $"/sheets/{currentTenantId}/CountingPic/{tenantId}/{partUrl}.png";
            var config = _hostingEnvironment.GetAppConfiguration();
            var baseurl = $"{config["base:url"]}";
            //var url = $"{_hostingEnvironment.GetAppConfiguration()["base:url"]}/MES/CountingView?ids={taskIds}&tenantId={tenantId}&currentTenantId={currentTenantId}&partUrl={partUrl}";
            var url = $"{_hostingEnvironment.GetAppConfiguration()["base:url"]}/MES/CountingView?ids={taskIds}&partUrl={partUrl}&signUrl=";
            //var openids = await MESUnitManager.FindUnitOpenId(tenantId);
            var templateId = "dE83qStBxfAAAFMAnn_0t6EknJ8B97L_A7A03LdiYkk";           //master: B2CWkuQKyZDqE4EreiQHqitAYbfU_Njxelremw546lk
            if (!string.IsNullOrEmpty(config["TemplateId:SendCounting"]))
            {
                templateId = config["TemplateId:SendCounting"];
            }
            //var tenantId = AbpSession.GetTenantId();                             //demo:dE83qStBxfAAAFMAnn_0t6EknJ8B97L_A7A03LdiYkk
            //currentUser = AbpSession.GetTenantId();
            //var q = await TenantManager.GetByIdAsync(AbpSession.GetTenantId());
            //var currentUser = q.TenancyName;
            //var url = "";
            var message = new
            {
                first = new TemplateDataItem("您好，收到" + currentUser + "发来的核算信息"),
                keyword1 = new TemplateDataItem("核算单号:" + processSNs),
                keyword2 = new TemplateDataItem(DateTime.Now.ToString()),
                keyword3 = new TemplateDataItem(totalFee.ToString()),
                keyword4 = new TemplateDataItem(currentUser),
                remark = new TemplateDataItem("总核算金额为:" + totalFee.ToString() + "元")
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

