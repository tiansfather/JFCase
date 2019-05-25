using Abp.BackgroundJobs;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.UI;
using Master.Configuration;
using Master.Domain;
using Master.Entity;
using Master.Projects;
using Master.Web.Jobs;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Authorization;
using Abp.Runtime.Caching;
using Master.Templates;
using Abp.AutoMapper;
using System.Diagnostics;
using Master.Authentication;
using Abp.Domain.Repositories;
using Master.Authentication.External;
using Master.MES.Jobs;
using Master.MES.Specifications;

namespace Master.MES
{
    public class ProcessTaskManager : DomainServiceBase<ProcessTask, int>
    {
        //public UserManager UserManager { get; set; }
        //public IProjectManager ProjectManager { get; set; }
        public IBackgroundJobManager BackgroundJobManager { get; set; }
        public IRepository<UserLogin,int> UserLoginRepository { get; set; }
        //public RemindLogManager RemindLogManager { get; set; }
        //public TemplateManager TemplateManager { get; set; }
        private static object _lockObj = new object();

        /// <summary>
        /// 添加加工任务时自动加上序号
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task InsertAsync(ProcessTask entity)
        {
            var lastPart=await Repository.GetAll().Where(o => o.PartId == entity.PartId).OrderBy(o => o.Sort).LastOrDefaultAsync();
            entity.Sort = lastPart == null ? 1 : lastPart.Sort + 1;
            await base.InsertAsync(entity);
        }
        /// <summary>
        /// 重写更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task UpdateAsync(ProcessTask entity)
        {           


            //根据实体属性更新状态
            if (entity.CheckFee != null)
            {
                //有对账金额的设置任务状态为已对账
                //entity.ProcessTaskStatus = ProcessTaskStatus.Checked;
                entity.SetStatus(ProcessTask.Status_Checked);
            }
            else if (entity.EndDate != null )
            {
                //有完成时间的自动设置进度
                entity.ProcessTaskStatus = ProcessTaskStatus.Completed;
                entity.Progress = 1;
            }else if (entity.StartDate != null)
            {
                entity.ProcessTaskStatus = ProcessTaskStatus.Processing;
            }


            await base.UpdateAsync(entity);
        }

        /// <summary>
        /// 对任务进行开单
        /// </summary>
        /// <param name="processTask"></param>
        /// <returns></returns>
        public virtual async Task KaiDan(ProcessTask processTask)
        {
            //更改状态至待上机
            if (processTask.ProcessTaskStatus == ProcessTaskStatus.Inputed)
            {
                processTask.ProcessTaskStatus = ProcessTaskStatus.WaitForProcess;
            }
            //add 20190429 设置审核人为当前人员
            if (string.IsNullOrEmpty(processTask.Verifier))
            {
                processTask.Verifier = AbpSession.Name();
            }
            await SaveAsync(processTask);
            await CurrentUnitOfWork.SaveChangesAsync();
            processTask.KaiDate= DateTime.Now;
            //生成加工单号
            GenerateProcessSN(processTask);
            #region 20190506 Tiansfather 如果是外协的单，则开单后提醒发送人
            if(!processTask.Inner && processTask.SupplierId != null)
            {
                //获取所有有发送权限的人
                var users = await Resolve<UserManager>().FindByPermission("Module.JGKD.Button.SendToUnit");
                //提醒接收发送提醒的人
                foreach(var user in users.Where(o => o.Id != AbpSession.UserId && o.HasStatus("ReceiveSendRemind")))
                {
                    var openId = user.GetWechatOpenId();
                    if (!string.IsNullOrEmpty(openId))
                    {
                        //先产生一条提醒记录
                        var remindLog = new RemindLog()
                        {
                            RemindType = "开单待发送提醒",
                            Name = user.Name,
                            TenantId = processTask.TenantId,
                            Message = processTask.ProcessSN
                        };
                        var remindLogId = await Resolve<RemindLogManager>().InsertAndGetIdAsync(remindLog);
                        var arg = new SendWeiXinMessageJobArgs()
                        {
                            OpenId = openId,
                            DataId = processTask.Id,
                            RemindLogId = remindLogId
                        };
                        Resolve<IBackgroundJobManager>().Enqueue<TaskToSendMessageJob, SendWeiXinMessageJobArgs>(arg);
                    }
                }
            }
            #endregion
        }
        /// <summary>
        /// 判断任务是否可以开单
        /// </summary>
        /// <param name="processTask"></param>
        /// <returns></returns>
        public async Task<bool> CanKaiDan(ProcessTask processTask)
        {
            //厂外并且未选择加工点的不能开单
            if (!processTask.SupplierId.HasValue && !processTask.Inner)
            {
                return false;
            }
            //是否强制开单
            var mustConfirmProcess=await SettingManager.GetSettingValueAsync<bool>(MESSettingNames.MustConfirmProcess);
            if (mustConfirmProcess)
            {
                //是否是手机开单
                //if(processTask.FromMobile)
                //{
                //    return false;
                //}
                //判断有无审核权限
                return await PermissionChecker.IsGrantedAsync("Module.JGKD.Button.ConfirmProcess");
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 判断加工任务是否需要审核
        /// </summary>
        /// <param name="processTask"></param>
        /// <returns></returns>
        public async Task<bool> NeedConfirm(ProcessTask processTask)
        {
            var needConfirm = false;
            var mustConfirmProcess = await SettingManager.GetSettingValueAsync<bool>(MESSettingNames.MustConfirmProcess);
            //如果强制审核,且没有加工单号,且已选择加工点或厂内
            if (string.IsNullOrEmpty(processTask.ProcessSN) && mustConfirmProcess && (processTask.SupplierId.HasValue || processTask.Inner))
            {
                needConfirm = true;
            }
            return needConfirm;
        }

        /// <summary>
        /// 加工任务是否可以报工
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<bool> CanReportAsync(int taskId)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant))
            {
                var task = await GetByIdAsync(taskId);
                if (task == null)
                {
                    return false;
                }
                return task.ProcessTaskStatus == ProcessTaskStatus.Processing ||
                    task.ProcessTaskStatus == ProcessTaskStatus.Received ||
                    task.ProcessTaskStatus == ProcessTaskStatus.Suspended ||
                    task.ProcessTaskStatus == ProcessTaskStatus.WaitForProcess;
            }
            
        }

        /// <summary>
        /// 切换加工任务状态
        /// </summary>
        /// <param name="processTask"></param>
        /// <param name="processTaskStatus"></param>
        /// <returns></returns>
        public virtual async Task ChangeTaskStatus(ProcessTask processTask,ProcessTaskStatus processTaskStatus)
        {
            //从待开单到待加工时需要生成加工单号
            if(processTask.ProcessTaskStatus==ProcessTaskStatus.Inputed && processTaskStatus == ProcessTaskStatus.WaitForProcess)
            {
                //processTask.ProcessSN = GenerateProcessSN(processTask);
            }
            processTask.ProcessTaskStatus = ProcessTaskStatus.WaitForProcess;
            //todo:任务流转记录
        }

        /// <summary>
        /// 生成加工单号
        /// </summary>
        /// <param name="processTask"></param>
        /// <returns></returns>
        public virtual void GenerateProcessSN(ProcessTask processTask)
        {
            if (string.IsNullOrEmpty(processTask.ProcessSN))
            {                
                    lock (_lockObj)
                    {
                        //获取当前账套当天所开加工单最后一个单号
                        var dayPrefix = DateTime.Now.ToString("yyMMdd");
                        var prefix = DateTime.Now.ToString("yyMMddHH");
                        var codeNo = "001";
                        var lastTask=GetAll().Where(o => o.ProcessSN.StartsWith(dayPrefix)).OrderBy(o => o.ProcessSN).LastOrDefault();
                        if (lastTask != null)
                        {
                            codeNo = (int.Parse(lastTask.ProcessSN.Substring(lastTask.ProcessSN.Length - 3))+1).ToString().PadLeft(3,'0');
                        }
                        //todo:生成加工单号  181130140001
                        processTask.ProcessSN = $"{prefix}{codeNo}";
                    CurrentUnitOfWork.SaveChanges();
                    }
            }
            
        }
        /// <summary>
        /// 根据报工更新任务状态
        /// </summary>
        /// <param name="processTaskReport"></param>
        /// <returns></returns>
        public virtual async Task UpdateTaskStatusByReport(ProcessTaskReport processTaskReport)
        {
            var task = processTaskReport.ProcessTask;
            task.Progress = processTaskReport.Progress;
            switch (processTaskReport.ReportType)
            {
                //上机
                case ReportType.上机:
                    if (task.StartDate == null)
                    {
                        task.StartDate = processTaskReport.ReportTime;
                        task.ProcessTaskStatus = ProcessTaskStatus.Processing;
                    }
                    break;
                //到料
                case ReportType.到料:
                    if (task.ReceiveDate == null)
                    {
                        task.ReceiveDate = processTaskReport.ReportTime;
                        task.ProcessTaskStatus = ProcessTaskStatus.Received;
                    }
                    break;
                //下机
                case ReportType.下机:
                    if (task.EndDate == null)
                    {
                        task.EndDate = processTaskReport.ReportTime;
                        task.ProcessTaskStatus = ProcessTaskStatus.Completed;
                        task.Progress = 1;
                        SetActualHours(task);
                        CalcTaskFee(task);
                    }
                    break;
                //暂停
                case ReportType.暂停:
                    task.ProcessTaskStatus = ProcessTaskStatus.Suspended;
                    break;
                case ReportType.重新开始:
                    task.ProcessTaskStatus= ProcessTaskStatus.Processing;
                    break;
            }
            await UpdateAsync(task);
        }
        /// <summary>
        /// 计算任务的费用
        /// </summary>
        /// <param name="processTask"></param>
        /// <returns></returns>
        public virtual  void CalcTaskFee(ProcessTask processTask)
        {
            if (processTask.FeeType == FeeType.按时间)
            {
                processTask.JobFee = (processTask.Price ?? 0) * (processTask.ActualHours ?? 0);
            }
            //如果是厂内，直接设置实际金额=初始金额
            if (processTask.Inner)
            {
                processTask.Fee = processTask.JobFee;
            }
            else
            {
                //厂外若未开启回单审核，则设置实际金额=初始金额
                if (!SettingManager.GetSettingValue<bool>(MESSettingNames.MustReturnFileBeforeCheck))
                {
                    processTask.Fee = processTask.JobFee;
                }
            }
        }
        /// <summary>
        /// 通过报工记录计算实际时长 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="type">是否强制计算 1强制 0不强制</param>
        public virtual void SetActualHours(ProcessTask task,string type="0")
        {
            //判断任务是否已经完成
            if(task.ProcessTaskStatus== ProcessTaskStatus.Completed)
            {
                if((type=="0"&& task.ActualHours==null)||type=="1")
                {
                    double totalH = 0;

                    DateTime? strtime =null;

                    //System.IO.File.AppendAllText("D:/MESRelease/log1.txt", "   报工任务数="+ task.ProcessTaskReports.LongCount());

                    //Logger.Error("   报工任务数=" + task.ProcessTaskReports.LongCount());

                    foreach (var processTaskReport in task.ProcessTaskReports.OrderBy(o=>o.CreationTime))
                    {
                       // Logger.Error("   任务类型=" + processTaskReport.ReportType + "  totalH=" + totalH);

                       // System.IO.File.AppendAllText("D:/MESRelease/log1.txt", "   任务类型=" + processTaskReport.ReportType+ "  totalH="+ totalH);
                        if (processTaskReport.ReportType== ReportType.上机)
                        {
                            strtime = processTaskReport.ReportTime;
                        }
                        if (processTaskReport.ReportType == ReportType.暂停)
                        {
                            if (strtime != null)
                            {
                                totalH = totalH + (processTaskReport.ReportTime - Convert.ToDateTime(strtime)).TotalHours;
                            }
                            strtime = null;
                        }
                        if(processTaskReport.ReportType == ReportType.重新开始)
                        {
                            strtime = processTaskReport.ReportTime;
                        }
                        if(processTaskReport.ReportType == ReportType.下机)
                        {
                            if (strtime != null)
                            {
                                totalH = totalH + (processTaskReport.ReportTime - Convert.ToDateTime(strtime)).TotalHours;
                                strtime = null;
                            }
                        }
                    }

                    task.ActualHours =Math.Round( Convert.ToDecimal(totalH),2);
                }
            }
        }

        /// <summary>
        /// 将加工单生成为图片
        /// </summary>
        /// <param name="processTask"></param>
        public virtual async Task SaveTaskSheetToImage(ProcessTask processTask)
        {
            if (string.IsNullOrEmpty(processTask.ProcessSN))
            {
                throw new UserFriendlyException(L("未开单任务无法生成图片"));
            }
            var projectId = processTask.Part?.Project?.Id;
            var targetUrl = $"{HostingEnvironment.GetAppConfiguration()["base:url"]}/Home/Show?name=../MES/SheetView&taskid={processTask.Id}";
            string upload_path = $"{Directory.GetCurrentDirectory()}\\wwwroot\\sheets\\{processTask.TenantId}\\ProcessSheet\\{projectId ?? 0}\\";
            if (!System.IO.Directory.Exists(upload_path))
            {
                System.IO.Directory.CreateDirectory(upload_path);
            }
            var filename = upload_path + "\\" + processTask.ProcessSN + ".png";
            //若文件已存在则不生成
            if (System.IO.File.Exists(filename))
            {
                return;
            }
            //var arg = new Html2ImageJobArgs()
            //{
            //    Url=targetUrl,
            //    FilePath=filename,
            //    WaitForId="app"
            //};
            ////产生图片的后台任务
            //await BackgroundJobManager.EnqueueAsync<Html2ImageJob, Html2ImageJobArgs>(arg);
            ChromeOptions op = new ChromeOptions();
            op.BinaryLocation = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            op.AddArguments("--headless");//开启无gui模式
            op.AddArguments("--no-sandbox");//停用沙箱以在Linux中正常运行
            ChromeDriver driver = new ChromeDriver(Environment.CurrentDirectory, op, TimeSpan.FromSeconds(180));

            driver.Navigate().GoToUrl(targetUrl);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(o => o.FindElement(By.Id("app")).Displayed);
            //System.Threading.Thread.Sleep(3000);
            //截图保存
            Screenshot screenshot = driver.GetScreenshot();

            screenshot.SaveAsFile(filename, ScreenshotImageFormat.Png);
            //退出
            driver.Quit();
        }

        /// <summary>
        /// 将加工单保存为图片新版本
        /// </summary>
        /// <param name="processTask"></param>
        /// <returns></returns>
        public virtual async Task SaveTaskSheetToImage2(ProcessTask processTask)
        {
            var htmlUrl = $"{HostingEnvironment.GetAppConfiguration()["base:url"]}/sheets/{processTask.TenantId}/ProcessSheet/{processTask.Part?.ProjectId ?? 0}/{processTask.ProcessSN}.html";
            string upload_path = $"{Directory.GetCurrentDirectory()}\\wwwroot\\sheets\\{processTask.TenantId}\\ProcessSheet\\{processTask.Part?.ProjectId ?? 0}\\";
            if (!System.IO.Directory.Exists(upload_path))
            {
                System.IO.Directory.CreateDirectory(upload_path);
            }
            var imagename= upload_path + "\\" + processTask.ProcessSN + ".png";
            //若文件已存在则不生成
            //if (System.IO.File.Exists(imagename))
            //{
            //    return;
            //}
            try
            {
                //Common.Fun.Html2Pdf(htmlUrl, filename, "Powerd By 模来模往");
                Common.Fun.Html2Image(htmlUrl, imagename);
            }catch(Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw new UserFriendlyException(L("生成图片失败"));
            }

            //string exeFilePath = Directory.GetCurrentDirectory() + @"\wkhtmltopdf.exe";
            ////MoveFolderTo(fileName, Application.StartupPath + @"\PDFLIB\");
            ////生成ProcessStartInfo
            //ProcessStartInfo pinfo = new ProcessStartInfo(exeFilePath);
            ////设置参数
            //StringBuilder sb = new StringBuilder();
            //sb.Append("--footer-line ");
            //sb.Append("--footer-center \"powered by 模来模往(https://master.imould.me)\" ");
            //sb.Append("\"" +htmlUrl +"\"");

            //sb.Append(" \"" + filename + "\"");

            //pinfo.Arguments = sb.ToString();
            ////隐藏窗口
            //pinfo.WindowStyle = ProcessWindowStyle.Hidden;
            ////启动程序

            //Process p = Process.Start(pinfo);
            //p.WaitForExit();
            ////DeleteFiles(Application.StartupPath + @"\PDFLIB\");
            //if (p.ExitCode == 0)
            //{
                
            //}
            //else
            //{
            //    Logger.Error("PDF生成失败:" + sb.ToString());
            //    throw new UserFriendlyException(L("生成PDF文件失败"));
            //}
        }
        public virtual async Task<string> SaveAccountingSheetToImage(string taskIds,string currentTenantId,string tenantId)
        {
            Logger.Error("\r\n\r\nScreenshotstart");
            var targetUrl = $"{HostingEnvironment.GetAppConfiguration()["base:url"]}/Home/Show?name=../MES/ProcessCost_Duii&modulekey=ProcessTask&data={taskIds}";
            string upload_path = $"{Directory.GetCurrentDirectory()}\\wwwroot\\sheets\\{currentTenantId}\\CountingPic\\{tenantId}\\{DateTime.Today.ToString("yyyyMMdd")}";
            if (!System.IO.Directory.Exists(upload_path))
            {
                System.IO.Directory.CreateDirectory(upload_path);
            }
            var time = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            var filename = upload_path + "\\" + time + ".png";
            //若文件已存在则不生成
            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
            }
            //try
            //{

            //    Common.Fun.Html2Image(targetUrl, filename);
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error(ex.Message, ex);
            //    throw new UserFriendlyException(L("生成图片失败"));
            //}

            ChromeOptions op = new ChromeOptions();
            op.BinaryLocation = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            op.AddArguments("--headless");//开启无gui模式
            op.AddArguments("--no-sandbox");//停用沙箱以在Linux中正常运行
            ChromeDriver driver = new ChromeDriver(Environment.CurrentDirectory, op, TimeSpan.FromSeconds(180));
                             //targetUrl = $"http://demo.imould.me/";
            Logger.Error("\r\n" + filename + "\r\n" + targetUrl);
            driver.Navigate().GoToUrl(targetUrl);
                           
                    //driver.Manage().Timeouts().ImplicitWait(10, TimeUnit.SECONDS);      //识别元素时的超时时间
                 //driver.Manage().Timeouts().pageLoadTimeout(10, TimeUnit.SECONDS);  //页面加载时的超时时间
                 //driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);  //异步脚本的超时时间     
                 //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    //driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(10);
                    //Logger.Error("\r\n" + driver.PageSource + "\r\n\r\n\r\n\r\n\r\n\r\n" );
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    //wait.Until(o => o.FindElement(By.Id("app")).Displayed);
                     //wait.Until(o => o.FindElement(By.Name("accounting")).Displayed);
                    //wait.Until(o => o.FindElement(By.Id("searchTable")).Displayed);
            System.Threading.Thread.Sleep(5000);
            //Logger.Error("\r\n\r\nScreenshotstep");
            ////截图保存
            Screenshot screenshot = driver.GetScreenshot();
            screenshot.SaveAsFile(filename, ScreenshotImageFormat.Png);
            //var tasks = taskIds.Split(',');
            //var taskFilename = "";
            //foreach(var taskId in tasks)
            //{
            //    taskFilename = filename.Replace(".png", taskId + ".png");
            //    //// targetUrl = $"{HostingEnvironment.GetAppConfiguration()["base:url"]}/Home/Show?name=../MES/ShowTask&data={taskId}";
            //    targetUrl = $"{HostingEnvironment.GetAppConfiguration()["base:url"]}/Home/Show?name=../MES/SheetView&taskid={taskId}";
            //    //try
            //    //{

            //    //    Common.Fun.Html2Image(targetUrl, taskFilename);
            //    //}
            //    //catch (Exception ex)
            //    //{
            //    //    Logger.Error(ex.Message, ex);
            //    //    throw new UserFriendlyException(L("生成图片失败"));
            //    //}

            //    driver.Navigate().GoToUrl(targetUrl);
            //    //wait.Until(o => o.FindElement(By.Id("app")).);
            //    System.Threading.Thread.Sleep(3000);
            //    screenshot = driver.GetScreenshot();
            //    screenshot.SaveAsFile(taskFilename, ScreenshotImageFormat.Png);
            //}

           
            //退出
            driver.Quit();


            
            return DateTime.Today.ToString("yyyyMMdd")+"/"+ time;
        }
        public virtual async Task<String> GetTaskPicSrc(int taskId)
        {
            Logger.Error("\r\n\r\ngetSrc" + taskId);
            //var manager = Manager as ProcessTaskManager;
            var processTask = await GetAll()
                .IgnoreQueryFilters()
                .Where(o => !o.IsDeleted)
               .Include(o => o.Part).Where(o => o.Id == taskId)
               .SingleOrDefaultAsync();
            if (processTask == null)
            {
                throw new UserFriendlyException(L("未找到加工任务"));
            }
            string upload_path = $"/sheets/{processTask.TenantId}/ProcessSheet/{processTask.Part?.ProjectId ?? 0}/{processTask.ProcessSN }.png";
            Logger.Error("\r\n\r\nend" + upload_path);
            return upload_path;
        }
        /// <summary>
        /// 获取任务的加工单图片路径
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public virtual async Task<string> GetTaskSheetImagePath(int taskId)
        {
            var task = await GetAll().Include(o => o.Part).ThenInclude(o => o.Project).Where(o => o.Id == taskId).SingleOrDefaultAsync();
            if (task == null)
            {
                throw new UserFriendlyException(L("未找到加工任务"));
            }
            var projectId = task.Part?.Project?.Id;
            var host = $"{HostingEnvironment.GetAppConfiguration()["base:url"]}";
            return $"/sheets/{task.TenantId}/ProcessSheet/{projectId??0}/{task.ProcessSN}.png";
        }
        /// <summary>
        /// 获取核算图片路径
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        //public virtual async Task<string> GetCountingSheetImagePath(int tenantId)
        //{
          
         
        //    var host = $"{HostingEnvironment.GetAppConfiguration()["base:url"]}";
        //    return $"/sheets/{task.TenantId}/ProcessSheet/{projectId ?? 0}/{task.ProcessSN}.png";
        //}
        /// <summary>
        /// 设置任务状态
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="status"></param>
        /// <param name="isset"></param>
        /// <returns></returns>
        public virtual async Task SetTaskStatus(int[] taskIds,string status,bool isset)
        {
            var tasks = await GetListByIdsAsync(taskIds);
            foreach(var task in tasks)
            {
                if (isset)
                {
                    task.SetStatus(status);
                }
                else
                {
                    task.RemoveStatus(status);
                }
            }
            
            
        }

        #region 异常任务查询
        /// <summary>
        /// 获取未完成的异常报工任务
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<ProcessTask> GetAbnormalReportTasks()
        {
            return GetAll()
                .Where(o => o.Progress < 1)
                .Where(o => (o.StartDate == null && o.ArrangeDate != null && DateTime.Now > o.ArrangeDate.Value) ||//已超过安排上机时间未上机的
                (o.StartDate != null && o.EndDate == null && o.EstimateHours != null && DateTime.Now > o.StartDate.Value.AddHours(Convert.ToDouble(o.EstimateHours.Value))))//已上机，但超过预计上机时间未下机的
                ;
        }
        #endregion

        #region 时间段任务查询
        /// <summary>
        /// 通过安排上机时间检索出已排机任务某时间段任务,时间区间为闭区间
        /// 需注意，结束日期当天任务是不被包含的，如结束日期为2019-08-08,则查询出的任务只到2019-08-08 00：00：00
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public virtual IQueryable<ProcessTask> GetTimeLineTasksQuery(DateTime startDate, DateTime endDate)
        {
            return GetAll()                
                //.Where(o => o.EquipmentId != null)允许查询未排机设备
                //.Where(o => o.ArrangeDate != null)
                .Where(o=>o.Status.Contains(ProcessTask.Status_Inner))//只显示厂内任务
                .Where(new TimeLineTaskSpecification(startDate,endDate));
        }
        #endregion

        #region 排机
        /// <summary>
        /// 取消排机
        /// </summary>
        /// <param name="processTask"></param>
        /// <returns></returns>
        public virtual async Task UnArrange(ProcessTask task)
        {
            task.ProcessSN = "";
            task.SupplierId = null;
            task.EquipmentId = null;
            task.ProcessTaskStatus = ProcessTaskStatus.Inputed;
            task.ArrangeDate = null;
            task.ArrangeEndDate = null;
        }

        #endregion

        #region 加工模板
        /// <summary>
        /// 获取加工任务的加工模板
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public virtual async Task<string> GetTaskTemplate(int taskId)
        {
            var templateManager = Resolve<TemplateManager>();
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant))
            {
                var templateContent = "";
                var task = await GetByIdFromCacheAsync(taskId);
                Template template = null;
                var templateId = task.GetPropertyValue<int?>("TemplateId");
                //先寻找加工任务对应的模板id
                if (templateId != null)
                {
                    try
                    {
                        template = await templateManager.GetByIdAsync(templateId.Value);
                    }
                    catch
                    {

                    }
                }
                //寻找对应的名称为"加工单"的模板
                if (template == null)
                {
                    template = await templateManager.GetAll().Where(o => o.TenantId == task.TenantId && o.TemplateType == MESTemplateSetting.TemplateType_ProcessTask && o.TemplateName == MESTemplateSetting.TemplateName_DefaultProcessTask).FirstOrDefaultAsync();
                }
                //如果找不到模板则使用默认模板
                if (template==null)
                {
                    templateContent = GetDefaultTaskTemplate();
                }
                else
                {
                    templateContent = template.TemplateContent;
                }

                return templateContent;
            }
        }
        /// <summary>
        /// 设置加工任务模板
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public virtual async Task SetTaskTemplate(int taskId,int? templateId)
        {
            var task = await GetByIdAsync(taskId);
            task.SetPropertyValue("TemplateId", templateId);
        }
        /// <summary>
        /// 获取任务的可用模板
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<Template>> GetAvailableTemplates(int taskId)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant,AbpDataFilters.MayHaveTenant))
            {
                var task = await GetByIdFromCacheAsync(taskId);
                var templates = await Resolve<TemplateManager>().GetAll().Where(o => o.TenantId == task.TenantId && o.TemplateType == MESTemplateSetting.TemplateType_ProcessTask).ToListAsync();
                return templates;
            }
        }
        /// <summary>
        /// 获取默认加工模板
        /// </summary>
        /// <returns></returns>
        public virtual string GetDefaultTaskTemplate()
        {
            var templateContent = CacheManager.GetCache<string, string>("TaskTemplate").Get("", o =>
                      Common.Fun.ReadEmbedString(typeof(ProcessTaskManager).Assembly, "Template.ProcessSheet.defaultTemplate.html")
                );
            return templateContent;
        }
        #endregion

        #region 撤回开单
        /// <summary>
        /// 对任务撤回开单，注意，此操作会删除原任务新产生任务
        /// </summary>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        public virtual async Task RevertCanDan(int[] taskIds)
        {
            var tasks = await GetListByIdsAsync(taskIds);
            foreach(var task in tasks)
            {
                var newTask = new ProcessTask();
                task.MapTo(newTask);
                newTask.ProcessSN = "";
                newTask.ProcessTaskStatus = ProcessTaskStatus.Inputed;
                await DeleteAsync(task);
                await InsertAsync(newTask);
            }
        }
        #endregion

        #region 发送任务审核提醒
        /// <summary>
        /// 开单审核提醒
        /// </summary>
        /// <param name="processTask"></param>
        /// <returns></returns>
        public virtual async Task SendTaskConfirmRemind(ProcessTask processTask)
        {
            var remindLogManager = Resolve<RemindLogManager>();
            //所有有审核权限的用户
            var users = await Resolve<UserManager>().FindByPermission("Module.JGKD.Button.ConfirmProcess");
            //如果任务设定了审核人，只发送给此审核人员
            if (!string.IsNullOrEmpty(processTask.Verifier) && users.Exists(o=>o.Name==processTask.Verifier))
            {
                users = users.Where(o => o.Name == processTask.Verifier).ToList();
            }
            //获取对应用户的微信登录信息
            var userLogins = await UserLoginRepository.GetAll()
                .Where(o => users.Select(u => u.Id).Contains(o.UserId))
                .Where(o => o.LoginProvider == WeChatAuthProviderApi.Name)
                .Select(o => new { o.ProviderKey, o.UserId })
                .ToListAsync();

            foreach (var userLogin in userLogins)
            {
                var openid = userLogin.ProviderKey;
                var name = users.Where(o => o.Id == userLogin.UserId).Single().Name;
                //先产生一条提醒记录
                var remindLog = new RemindLog()
                {
                    RemindType = "开单审核提醒",
                    Name = name,
                    TenantId = AbpSession.TenantId,
                    Message = processTask.Part?.Project?.ProjectSN + processTask.Part?.PartName + processTask.ProcessType?.ProcessTypeName,
                };
                var remindLogId = await remindLogManager.InsertAndGetIdAsync(remindLog);

                var arg = new SendWeiXinMessageJobArgs()
                {
                    OpenId = openid,
                    DataId = processTask.Id,
                    RemindLogId = remindLogId,

                };

                BackgroundJobManager.Enqueue<TaskConfirmMessageJob, SendWeiXinMessageJobArgs>(arg);
            }
        }
        #endregion
    }
}
