using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.BackgroundJobs;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Master.Authentication;
using Master.Authentication.External;
using Master.Entity;
using Master.EntityFrameworkCore;
using Master.MES;
using Master.MES.Jobs;
using Master.MES.Service;
using Master.MultiTenancy;
using Master.Notices;
using Master.Projects;
using Master.WeiXin;
using Master.WeiXin.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Helpers;

namespace Master.Controllers
{
    public class MESController: WeiXinBaseController
    {
        private readonly IBackgroundJobManager _backgroundJobManager;
        public ProcessTaskReportManager ProcessTaskReportManager { get; set; }
        public ProcessTaskManager ProcessTaskManager { get; set; }
        public PersonManager PersonManager { get; set; }
        public UserManager UserManager { get; set; }
        public TenantManager TenantManager { get; set; }
        public MESUnitManager MESUnitManager { get; set; }
        public TacticManager TacticManager { get; set; }
        public EquipmentManager EquipmentManager { get; set; }
        public IFileManager FileManager { get; set; }
        public NoticeManager NoticeManager { get; set; }
        public MesTenancyAppService MesTenancyAppService { get; set; }
        public IRepository<Equipment,int> EquipmentRepository { get; set; }
        public MESController(IBackgroundJobManager backgroundJobManager)
        {
            _backgroundJobManager = backgroundJobManager;
        }
        public async Task<IActionResult> Test()
        {

            var url="http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=20zglA2I8TDi44JEq-_N2ClVfTqhyQ_1O90aRDT6-IRLLCA-XsZVCruo6LhxlVByY3-3zc4YAK_X_MNs6hACtH7EAZyXxFncAqMvhqe2zrLZRsLGwERBbAGAMZJ&media_id=jSjE-pnj_C-4e9sYcwuEHclptn5HVG4lmR5FJsq815Hcxz78cx-8RK3vRsrbZ7un";
            await Senparc.CO2NET.HttpUtility.Get.DownloadAsync(url, $"{Directory.GetCurrentDirectory()}\\wwwroot\\");
            //AppDomain.CurrentDomain.Load(typeof(Part).Assembly.GetName());
            //var op = ScriptOptions.Default;
            ////class WanTaiProject defined in plugin,this line failed
            //var a = CSharpScript.EvaluateAsync<int>("Id", op, new Part() { Id = 1 }, typeof(Part)).Result;
            ////class Project defined in main,this line works
            ////var b= CSharpScript.EvaluateAsync<int>("Id", op, new Project() { Id = 1 }, typeof(Project)).Result;

            return Content("ok");
        }
        public IActionResult Test2()
        {
            var url = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=20zglA2I8TDi44JEq-_N2ClVfTqhyQ_1O90aRDT6-IRLLCA-XsZVCruo6LhxlVByY3-3zc4YAK_X_MNs6hACtH7EAZyXxFncAqMvhqe2zrLZRsLGwERBbAGAMZJ&media_id=jSjE-pnj_C-4e9sYcwuEHclptn5HVG4lmR5FJsq815Hcxz78cx-8RK3vRsrbZ7un";
            Senparc.CO2NET.HttpUtility.Get.Download(url, $"{Directory.GetCurrentDirectory()}\\wwwroot\\");
           // new System.Net.WebClient().DownloadFile(url, $"{Directory.GetCurrentDirectory()}\\wwwroot\\1.jpg");
            return Content("ok");
        }
        public IActionResult Test3()
        {
            var url = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=20zglA2I8TDi44JEq-_N2ClVfTqhyQ_1O90aRDT6-IRLLCA-XsZVCruo6LhxlVByY3-3zc4YAK_X_MNs6hACtH7EAZyXxFncAqMvhqe2zrLZRsLGwERBbAGAMZJ&media_id=jSjE-pnj_C-4e9sYcwuEHclptn5HVG4lmR5FJsq815Hcxz78cx-8RK3vRsrbZ7un";
            new System.Net.WebClient().DownloadFile(url, $"{Directory.GetCurrentDirectory()}\\wwwroot\\1.jpg");
            return Content("ok");
        }
        public async Task<ActionResult> TestReg()
        {
            await MesTenancyAppService.Register(new MES.Dtos.RegisterDto()
            {
                CompanyName="Test",
                Mobile="111111",
                Name="Abc",
                Password="test"
            });
            return Content("OK");
        }
        /// <summary>
        /// 展示注册二维码
        /// </summary>
        /// <returns></returns>
        public ActionResult RegisterCode()
        {
            return View();
        }

        #region 微信端页面

        /// <summary>
        /// 微信端公告查看页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [WeUserFilter]
        [WeBindUserFilter]
        public async Task<IActionResult> TenantNoticeView(int id)
        {
            var notice = await NoticeManager.GetAll().IgnoreQueryFilters()
                .Where(o => o.Id == id)
                .SingleOrDefaultAsync();

            return View("WeChat/NoticeView",notice);
        }

        /// <summary>
        /// 微信端注册页
        /// </summary>
        /// <returns></returns>
        [WeUserFilter]
        [WeMustSubscribeFilter]
        public async Task<ActionResult> Register(int? inviter,string companyName)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                //验证用户是否已绑定了微信登录,已绑定的不能再注册
                var user = await UserManager.Repository.GetAll()
                    .Include(o => o.Tenant)
                    .Where(o => o.Logins.Count(l => l.ProviderKey == WeUser.openid) > 0)
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    if (inviter.HasValue && !companyName.IsNullOrEmpty())
                    {
                        //如果是被邀请注册的,且当前扫码人已经注册,则跳转至认领邀请人往来单位页
                        return Redirect($"/MES/ClaimUnit?inviter={inviter.Value}&companyName={companyName.UrlEncode()}");
                    }
                    return Redirect("/weixin/error?msg=" + $"您已绑定企业\"{user.Tenant.TenancyName}\"".UrlEncode());
                }

                return View();
            }
            
        }
        /// <summary>
        /// 认领其它账套的往来单位
        /// </summary>
        /// <param name="inviter"></param>
        /// <param name="companyName"></param>
        /// <returns></returns>
        [WeUserFilter]
        [WeBindUserFilter]
        public async Task<ActionResult> ClaimUnit(int inviter,string companyName)
        {
            var tenant = await TenantManager.GetByIdFromCacheAsync(inviter);
            ViewBag.TenantName = tenant.TenancyName;
            ViewBag.UnitName = companyName;
            return View("Wechat/ClaimUnit");
        }
        /// <summary>
        /// 微信端报工页
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [WeUserFilter]
        public async Task<ActionResult> Report(int taskId)
        {
            //return Redirect("/WeiXin/Error?msg=未找到可报工任务");
            if(!await ProcessTaskManager.CanReportAsync(taskId))
            {
                var task = await ProcessTaskManager.GetByIdAsync(taskId);
 
                if (task!=null && task.ProcessTaskStatus == ProcessTaskStatus.Completed) {
                  
                    return Redirect("/MES/PartReportView?id="  + taskId);
                }
                return Redirect("/WeiXin/Error?msg="+"任务不存在或已失效".UrlEncode());
            }
            //todo:临时使用，强制获取token
            //await AccessTokenContainer.GetAccessTokenAsync(appId, true);
            //JsApiTicketContainer.GetJsApiTicket(appId, true);


            var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(appId, appSecret, Request.AbsoluteUri().Split('#')[0]);
            Logger.Info("JSSDKPackage:" + Common.JSONConvert.SerializeCamelCase(jssdkUiPackage));
            ViewBag.ReporterName = WeUser.nickname;
            return View(jssdkUiPackage);
        }
        /// <summary>
        /// 绑定提醒者微信端页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [WeUserFilter]
       [WeMustSubscribeFilter]
        public async Task<ActionResult> BindReminder(int id)
        {
            var info = UserApi.Info(appId, WeUser.openid);
            if (info.subscribe == 0)
            {
                return Redirect("/WeiXin/GuanZhu");
            }
            var tactic = await TacticManager.Repository.GetAll().IgnoreQueryFilters().Where(o => o.Id == id).SingleOrDefaultAsync();
            if (tactic == null)
            {
                return Redirect("/WeiXin/Error?msg=" + "未找到对应提醒策略".UrlEncode());
            }
            var bindPersons = await TacticManager.GetTacticReminders(id);
            if (bindPersons.Exists(o => o.GetPropertyValue<string>("OpenId") == WeUser.openid)){
                return Redirect("/WeiXin/Error?msg=" + "您已经绑定此提醒策略".UrlEncode());
            }

            ViewBag.TenancyName = tactic.Tenant!=null?tactic.Tenant.TenancyName:"管理系统";
            ViewBag.TacticName = tactic.TacticName;
            ViewBag.TacticId = id;
            return View();
        }
        /// <summary>
        /// 绑定客户报工提醒页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[WeUserFilter]
        //[WeMustSubscribeFilter]
        //public async Task<ActionResult> BindCustomerReminder(int unitId)
        //{
        //    var person = await PersonManager.GetPersonByWeUserOrInsert(WeUser);
        //    var unit = await MESUnitManager.GetAll()
        //        .Include(o => o.Tenant)
        //        .Where(o => o.Id == unitId).SingleAsync();

        //    //是否已绑定
        //    var bindedPersonIds = unit.GetData<List<int>>("BindedPersonIds");
        //    var binded = bindedPersonIds!=null && bindedPersonIds.Contains(person.Id);
        //    ViewBag.UnitName = unit.UnitName;
        //    ViewBag.UnitId = unitId;
        //    ViewBag.TenancyName = unit.Tenant.TenancyName;
        //    ViewBag.Binded = binded;

        //    return View();
        //}
        /// <summary>
        /// 设备绑定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [WeUserFilter]
        [WeBindUserFilter]
        public async Task<IActionResult> BindEquipment(int id)
        {
            
            var obj = await EquipmentRepository.GetAll().Include(o => o.Operator).Where(o => o.Id == id).SingleOrDefaultAsync();
            if (obj == null)
            {
                return Redirect("/WeiXin/Error?msg=" + ("设备信息错误").UrlEncode());
            }
            //if (obj.OperatorId == CurrentUserId)
            //{
            //    return Redirect("/WeiXin/Error?msg=" + ("当前微信已经绑定了设备" + obj.EquipmentSN).UrlEncode());
            //}
            return View("WeChat/BindEquipment", obj);
            //using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant))
            //{
                
            //}
        }
        /// <summary>
        /// 操作工设备交接二维码页
        /// </summary>
        /// <returns></returns>
        [WeUserFilter]
        [WeBindUserFilter]
        public IActionResult EquipmentTranisitionCode()
        {
            return View("WeChat/EquipmentTransitionCode");
        }
        /// <summary>
        /// 操作工设备交接确认页
        /// </summary>
        /// <returns></returns>
        [WeUserFilter]
        [WeBindUserFilter]
        public IActionResult EquipmentTransition()
        {
            return View("WeChat/EquipmentTransition");
        }
        /// <summary>
        /// 微信端报工信息查看页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [WeUserFilter]
        public async Task<ActionResult> ReportView(int id)
        {
            //todo:需要验证当前微信用户是否可以属于报工信息对应企业的策略提醒人
            var remindPersons =(await TacticManager.GetRemindPersonsByReport(id)).Select(o=>o.Person);

            var currentPerson =await PersonManager.GetPersonByWeUserOrInsert(WeUser);
            if (!remindPersons.Contains(currentPerson))
            {
                return Redirect("/WeiXin/Error?msg=" + "无权查看此报工信息".UrlEncode());
            }

            var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(appId, appSecret, Request.AbsoluteUri());
            return View(jssdkUiPackage);
        }
        /// <summary>
        ///  微信端 零件报工信息查看页
        /// </summary>
        /// <param name="id"></param> 
        /// <returns></returns>
        public async Task<ActionResult> PartReportView(int id )
        {
            
            var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(appId, appSecret, Request.AbsoluteUri());
            return View("Wechat/PartReportView", jssdkUiPackage);
        }
        /// <summary>
        /// 微信公众号 网页主页面跳转
        /// </summary>
        /// <returns></returns>
        [WeUserFilter]
       [WeBindUserFilter]
     
        public ActionResult WeChatIndex()
        {

            return View("Wechat/WeChatIndex");
        }
        /// <summary>
        /// 主页
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [WeUserFilter]
        [WeBindUserFilter]
        [Route("[controller]/[action]/{page}")]
        [HttpGet]
        public ActionResult Index(string page)
        {
            
            return View("Wechat/"+ page + "/Index");
        }

        /// <summary>
        /// 邀请页面
        /// </summary>
        /// <returns></returns>
        [WeUserFilter]
        [WeBindUserFilter(false)]
        public ActionResult Invite()
        {
            return View("Invite");
        }

        #region 模具厂
        /// <summary>
        /// 加工开单
        /// </summary>
        [WeUserFilter]
        [WeBindUserFilter]
        public ActionResult JGKD()
        {


            var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(appId, appSecret, Request.AbsoluteUri().Split('#')[0]);
            Logger.Info("JSSDKPackage:" + Common.JSONConvert.SerializeCamelCase(jssdkUiPackage));
         
 
            return View("Wechat/factory/JGKD",jssdkUiPackage);
        }
        /// <summary>
        /// 回单审核
        /// </summary>
        [WeUserFilter]
        [WeBindUserFilter]
        [Route("[controller]/wechat/[action]")]
        [HttpGet]
        public ActionResult ReceiptAudit()
        {
            var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(appId, appSecret, Request.AbsoluteUri().Split('#')[0]);
            Logger.Info("JSSDKPackage:" + Common.JSONConvert.SerializeCamelCase(jssdkUiPackage));

            return View("Wechat/factory/ReceiptAudit", jssdkUiPackage);
        }
        /// <summary>
        /// 开单任务
        /// </summary>
        /// <returns></returns>
        [WeUserFilter]
         [WeBindUserFilter]
        public ActionResult Project()
        {

            return View("Wechat/Project");
        }
         [WeUserFilter]
         [WeBindUserFilter]
        public ActionResult ProcessTask(string data)
        {
           
            return View("Wechat/ProcessTask");
        }
        /// <summary>
        /// 开单审核
        /// </summary>
       [WeUserFilter]
       [WeBindUserFilter]
        public ActionResult ProcessTask_Verify()
        {

            return View("Wechat/factory/ProcessTask_Verify");
        }

        /// <summary>
        /// 报工记录
        /// </summary>
        [WeUserFilter]
         [WeBindUserFilter]
        public ActionResult ProcessTaskReport()
        {

            return View("Wechat/factory/ProcessTaskReport");
        }
        /// <summary>
        /// 生产任务
        /// </summary>
        [WeUserFilter]
        [WeBindUserFilter]
        public ActionResult ProcessTaskWechat()
        {

            return View("Wechat/ProcessTaskWechat");
        }
        /// <summary>
        /// 开单审核
        /// </summary>
        [WeUserFilter]
        [WeBindUserFilter]
        public ActionResult VerifyWechat()
        {

            return View("Wechat/factory/VerifyWechat");
        }
        /// <summary>
        /// 设备详情列表
        /// </summary>
        /// <returns></returns>
        [WeUserFilter]
         [WeBindUserFilter]
        public ActionResult Equipment() {

             
            return View("Wechat/Equipment");
        }
        /// <summary>
        /// 零件详情列表
        /// </summary>
        /// <returns></returns>
        [WeUserFilter]
        [WeBindUserFilter]
        public ActionResult PartTask()
        {
            return View("Wechat/PartTask");
        }
        #endregion

        #region 加工点


        /// <summary>
        /// 微信端加工点查看询价页
        /// </summary>
        /// <returns></returns>
        [WeUserFilter]
        [WeBindUserFilter]
        public async Task<ActionResult> ProcessQuoteProcessor()
        {           

                return View("Wechat/process/ProcessQuoteProcessor");
        }

        /// <summary>
        /// 加工点查看加工任务页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [WeUserFilter]
        [WeBindUserFilter]
        public async Task<ActionResult> OuterTaskView(int id)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant))
            {
                var task = await ProcessTaskManager.GetAll().Where(o => o.ToTenantId == AbpSession.TenantId).Where(o => o.Id == id).SingleOrDefaultAsync();
                if (task == null)
                {
                    return Redirect("/WeiXin/Error?msg=" + "无权查看此任务信息".UrlEncode());
                }
                //标记任务为加工点已查看状态
                task.SetStatus(MES.ProcessTask.Status_ProcessorReaded);
                var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(appId, appSecret, Request.AbsoluteUri().Split('#')[0]);
                var path = await ProcessTaskManager.GetTaskSheetImagePath(id);
                ViewBag.ImgPath = path;
                return View("WeChat/process/TaskView", jssdkUiPackage);
            }
            
        }
        /// <summary>
        /// 加工点查看加工核算页
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="tenantId"></param>
        ///     <param name="currentTenantId"></param>
        ///      <param name="partUrl"></param>
        /// <returns></returns>
        [WeUserFilter]
        [WeBindUserFilter]
        public async Task<ActionResult> CountingView(string ids,string partUrl,string signUrl)
        {
            //using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant))
            //{
                var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(appId, appSecret, Request.AbsoluteUri().Split('#')[0]);
                ViewBag.Ids = ids;
            String srcArray = "";
            foreach(var id in ids.Split(','))
            {
                Logger.Error("\r\n\r\nid" + id);
                var path = await ProcessTaskManager.GetTaskPicSrc(Convert.ToInt32(id));
                srcArray += path + ",";
            }
            srcArray.Substring(0, srcArray.Length-1);
            Logger.Error("\r\n\r\nSrcArr"+srcArray);
            //var path = await ProcessTaskManager.GetTaskSheetImagePath(id);
            ViewBag.ImgPath =partUrl;
                ViewBag.SignUrl = signUrl;
            ViewBag.TaskUrls = srcArray;
                return View("WeChat/process/CountingView", jssdkUiPackage);
            //}
        }

        [WeUserFilter]
        [WeBindUserFilter]
        public async Task<ActionResult> HandWrite(string id)
        {
            //using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant))
            //{
            var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(appId, appSecret, Request.AbsoluteUri().Split('#')[0]);
            ViewBag.Ids = id;
            //var path = await ProcessTaskManager.GetTaskSheetImagePath(id);
            //ViewBag.ImgPath = $"/sheets/{currentTenantId}/CountingPic/{tenantId}/{partUrl}.png";
            return View("WeChat/process/handWrite", jssdkUiPackage);
            //}

        }
        /// <summary>
        /// 加工点报工记录查看页
        /// </summary>
        /// <returns></returns>
        [WeUserFilter]
        [WeBindUserFilter]
        public ActionResult ProcessorReport()
        {
            var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(appId, appSecret, Request.AbsoluteUri().Split('#')[0]);
            return View("WeChat/process/ProcessorReport", jssdkUiPackage);

        }
        /// <summary>
        /// 加工点设备添加页
        /// </summary>
        /// <returns></returns>
        [WeUserFilter]
        [WeBindUserFilter]
        [HttpGet]
        [Route("[controller]/process/[action]")]
        public ActionResult ProfileEquipment()
        {
            var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(appId, appSecret, Request.AbsoluteUri().Split('#')[0]);
            return View("WeChat/process/ProfileEquipment", jssdkUiPackage);

        }
        /// <summary>
        /// 加工点信息填写页
        /// </summary>
        /// <returns></returns>
        [WeUserFilter]
        [WeBindUserFilter]
        [HttpGet]
        [Route("[controller]/process/[action]")]
        public ActionResult Profile()
        {
            var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(appId, appSecret, Request.AbsoluteUri().Split('#')[0]);
            return View("WeChat/process/Profile", jssdkUiPackage);

        }

        /// <summary>
        /// 加工点报工记录查看页
        /// </summary>
        /// <returns></returns>
        [WeUserFilter]
        [WeBindUserFilter]
        public ActionResult ProcessorTask()
        {
            var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(appId, appSecret, Request.AbsoluteUri().Split('#')[0]);
            return View("WeChat/process/ProcessorTask", jssdkUiPackage);

        }

        #endregion

        #region 客户
       
        #endregion
        /// <summary>
        /// 微信扫码设备任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [WeUserFilter]
        [WeBindUserFilter]        
        public ActionResult EquipmentTasks(int id)
        {            
            return View("WeChat/EquipmentTasks");
        }
        /// <summary>
        /// 微信扫码工件任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [WeUserFilter]
        //[WeBindUserFilter]
        public ActionResult PartTasks(int id)
        {
            return View("WeChat/PartTasks");
        }
   

        #endregion


        public ActionResult BindError()
        {
            return View();
        }
        public async Task<ActionResult> Do()
        {
            var c = ProcessTaskReportManager.Repository.GetAll().Where(o => o.Id == 1).Single();
            //_backgroundJobManager.Enqueue<SendReportWeiXinMessageJob, int>(1);

            return Content(c.CreationTime.ToString());
        }

    }
}
