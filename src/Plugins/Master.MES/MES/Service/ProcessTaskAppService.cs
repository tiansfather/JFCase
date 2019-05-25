using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.BackgroundJobs;
using Abp.Configuration;
using Abp.Domain.Entities;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using Master.Authentication;
using Master.Authentication.External;
using Master.Configuration;
using Master.Dto;
using Master.Entity;
using Master.EntityFrameworkCore;
using Master.EntityFrameworkCore.Extensions;
using Master.MES.Dtos;
using Master.MES.Jobs;
using Master.MES.Service;
using Master.MouldTry.Domains;
using Master.MultiTenancy;
using Master.Projects;
using Master.Units;
using Master.WeiXin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES
{    
    public class ProcessTaskAppService : MESAppServiceBase<ProcessTask, int>
    {
        //public IProjectManager ProjectManager { get; set; }
        //public MESUnitManager MESUnitManager { get; set; }
        //public EquipmentManager EquipmentManager { get; set; }
        //public ProcessTypeManager ProcessTypeManager { get; set; }
        //public PartManager PartManager { get; set; }
        //public PersonManager PersonManager { get; set; }
        //public ProcessTaskReportManager ProcessTaskReportManager { get; set; }
        //public ProcessTaskManager ProcessTaskManager { get; set; }
        //public IFileManager FileManager { get; set; }
        //public CloudEquipmentAppService CloudEquipmentAppService { get; set; }
        //public RemindLogManager RemindLogManager { get; set; }
        //public IBackgroundJobManager BackgroundJobManager { get; set; }
        //public WeiXinAppService WeiXinAppService { get; set; }

        public virtual string Get()
        {
            return "1";
        }
        /// <summary>
        /// 通过试模id查找对应的加工id
        /// </summary>
        /// <param name="tryid"></param>
        /// <returns></returns>
        public virtual async Task<string> GetIdByTryId(string tryid)
        {
            var mouldTryManager = Resolve<MouldTryManager>();
            var mouldTry = await mouldTryManager.GetByIdAsync(Convert.ToInt32(tryid));
            var processTask = Manager.GetAll().Where(o => o.ProcessSN == mouldTry.MouldTrySN).FirstOrDefault();
            if (processTask == null)
            {
                return "";
            }
            return processTask.Id.ToString();
        }
        #region 汇总数量
        /// <summary>
        /// 获取任务的汇总统计
        /// By Tiansfather 2019-03-29
        /// </summary>
        /// <returns></returns>
        public virtual async Task<object> GetSummaryCount()
        {
            var baseQuery = Manager.GetAll();
            //已查收数量
            var processorReadedCount = await baseQuery.Where(o => o.Status.Contains(ProcessTask.Status_ProcessorReaded)).CountAsync();
            //已回单数量
            var verifyCount = await baseQuery.Where(o => o.Status.Contains(ProcessTask.Status_Verify)).CountAsync();
            //已发送数量
            var sendCount = await baseQuery.Where(o => o.Status.Contains(ProcessTask.Status_SendProcessor)).CountAsync();
            //待上机数量
            var waitForProcessCount = await baseQuery.Where(o => o.ProcessTaskStatus == ProcessTaskStatus.WaitForProcess).CountAsync();
            //已到料数量
            var receivedCount = await baseQuery.Where(o => o.ProcessTaskStatus == ProcessTaskStatus.Received).CountAsync();
            //已上机数量
            var processingCount = await baseQuery.Where(o => o.ProcessTaskStatus == ProcessTaskStatus.Processing).CountAsync();
            //已完成数量
            var completeCount = await baseQuery.Where(o => o.ProcessTaskStatus == ProcessTaskStatus.Completed).CountAsync();
            //暂停中数量
            var suspendCount = await baseQuery.Where(o => o.ProcessTaskStatus == ProcessTaskStatus.Suspended).CountAsync();
            //已取消数量
            var canceledCount = await baseQuery.Where(o => o.ProcessTaskStatus == ProcessTaskStatus.Canceled).CountAsync();

            return new
            {
                verifyCount,
                sendCount,
                waitForProcessCount,
                receivedCount,
                processingCount,
                completeCount,
                suspendCount,
                canceledCount,
                processorReadedCount
            };
        }
        /// <summary>
        /// 获取所有回单审核金额总额和核算金额总额
        /// </summary>
        /// 
        /// <returns></returns>
        public virtual async Task<object> GetSummaryFee(int? unitId,string poster)
        {
            
            var baseQuery = Manager.GetAll();
            if (unitId.HasValue)
            {
                baseQuery = baseQuery.Where(o => o.SupplierId == unitId);
            }
            if (!string.IsNullOrEmpty(poster))
            {
                baseQuery = baseQuery.Where(o => o.Poster == poster);
            }
            var verifyFee = await baseQuery.Where(o => o.Status.Contains(ProcessTask.Status_Verify)).SumAsync(o=>o.Fee);
            var checkFee = await baseQuery.Where(o => o.Status.Contains(ProcessTask.Status_Checked)).SumAsync(o => o.CheckFee);
            return new
            {
                verifyFee,
                checkFee
            };
        }

        #endregion

        #region 删除重写
        /// <summary>
        /// Task任务删除之前筛选已经上机后的任务判断不能删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public override async  Task DeleteEntity(IEnumerable<int> ids)
        {
            var manager = Manager as ProcessTaskManager;
            var tasks = await manager.GetListByIdsAsync(ids);

            string rstring = "";

            //todo:此逻辑可以移至领域层,即ProcessTaskManager类中
            foreach(var task in tasks)
            {
                //已上机
                if(task.ProcessTaskStatus== ProcessTaskStatus.Processing)
                {
                    rstring = "已上机无法删除";
                    break;
                }
                //已完成
                if (task.ProcessTaskStatus == ProcessTaskStatus.Completed)
                {
                    rstring = "已完成无法删除";
                    break;
                }
                //暂停
                if (task.ProcessTaskStatus == ProcessTaskStatus.Suspended)
                {
                    rstring = "已暂停无法删除";
                    break;
                }
                //已对账
                if (task.HasStatus(ProcessTask.Status_Checked))
                {
                    rstring = "已对账无法删除";
                    break;
                }

            }
            if (rstring == "")
            {
                await manager.DeleteAsync(ids);
            }
            else
            {
                throw new UserFriendlyException(rstring);
            }
        }
        #endregion

        #region 分页

        //protected async override Task<IQueryable<ProcessTask>> GetQueryable(RequestPageDto request)
        //{
        //    var query= (await base.GetQueryable(request))
        //        .Include(o => o.Part).ThenInclude(o => o.Project)
        //        .Include(o => o.Equipment)
        //        .Include(o => o.Supplier)
        //        .Include(o => o.ProcessTaskReports)
        //        .Include(o => o.ProcessType);


        //    return query;
        //}
        protected override async Task<IQueryable<ProcessTask>> BuildKeywordQueryAsync(string keyword, IQueryable<ProcessTask> query)
        {
            var newQuery=await base.BuildKeywordQueryAsync(keyword, query);
            return newQuery.Where(o => o.Part.PartName.Contains(keyword) || o.ProcessType.ProcessTypeName.Contains(keyword) || o.Part.Project.ProjectSN.Contains(keyword));
        }
        protected async override Task<PagedResult<ProcessTask>> GetPageResultQueryable(RequestPageDto request)
        {
            var query = (await GetQueryable(request))
                .Include(o => o.Part).ThenInclude(o => o.Project)
                .Include(o => o.Equipment)
                .Include(o=>o.CreatorUser)
                .Include(o => o.Supplier)
                .Include(o => o.ProcessTaskReports)
                .Include(o => o.ProcessType);

            return query.PageResult(request.Page, request.Limit);
        }

        protected override async Task<IQueryable<ProcessTask>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<ProcessTask> query)
        {
            
            if (searchKeys.ContainsKey("from"))
            {
                if (searchKeys["from"] == "processor")
                {
                    using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant))
                    {
                        return query.Where(o => MESDbContext.GetJsonValueNumber(o.Supplier.Property, "$.TenantId") == AbpSession.TenantId.Value);
                    }
                    
                }
            }
            return await base.BuildSearchQueryAsync(searchKeys, query);
        }
        /// <summary>
        /// 分页返回
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [DontWrapResult]
        [AbpAuthorize]
        public override async Task<ResultPageDto> GetPageResult(RequestPageDto request)
        {
            //var queryable = await GetQueryable(request);

            var query =await GetPageResultQueryable(request);


            var tasks = await query.Queryable.ToListAsync();

            var data = tasks.Select(o => {
                //计算初始金额 
                decimal? jobFee = 0;
                if (o.FeeType == FeeType.承包)
                {
                    jobFee = o.JobFee;
                }else if (o.FeeType == FeeType.按时间)
                {
                    jobFee = o.Price * o.EstimateHours;
                }
                else
                {
                    jobFee = o.Price * o.FeeFactor;
                }
                return new {
                    o.Id,
                    o.PartId,
                    o.Part.ProjectId,
                    o.Part.Project.ProjectSN,
                    o.Part.Project.ProjectName,
                    T0Date = o.Part.Project.GetPropertyValue<DateTime?>("T0Date"),
                    o.Part.PartSN,
                    o.Part.PartName,
                    o.Part.PartSpecification,
                    o.Part.PartNum,
                    o.ProcessSN,
                    o.Supplier?.UnitName,
                    o.Equipment?.EquipmentSN,
                    o.ProcessType?.ProcessTypeName,
                    StartDate = o.StartDate?.ToString("yyyy-MM-dd HH:mm"),
                    EndDate = o.EndDate?.ToString("yyyy-MM-dd HH:mm"),
                    Progress = o.EndDate != null ? 1 : o.Progress,
                    o.EstimateHours,
                    JobFee = jobFee,
                    o.Fee,
                    o.FeeType,
                    o.FeeFactor,
                    o.ProjectCharger,
                    o.Emergency,
                    o.CheckFee,
                    o.Price,
                    o.TaskInfo,
                    AppointDate = o.AppointDate?.ToString("yyyy-MM-dd"),
                    RequireDate = o.RequireDate?.ToString("yyyy-MM-dd"),
                    Files = o.ProcessTaskReports.LastOrDefault()?.Files,
                    Inner = o.HasStatus(ProcessTask.Status_Inner),
                    Rate = o.GetPropertyValue<int>("Rate"),
                    QuanlityType = o.GetPropertyValue<int>("QuanlityType"),
                    RateInfo = o.GetPropertyValue<string>("RateInfo"),
                    RateInfoCount=o.RateFeeInfos.Count,//回单审核数量
                    IsVerified = o.HasStatus(ProcessTask.Status_Verify),
                    IsKaiPiao = o.HasStatus(ProcessTask.Status_MakeInvoice),
                    IsAccountingPass = o.HasStatus(ProcessTask.Status_AccountingPass),
                    IsAccountingDeny = o.HasStatus(ProcessTask.Status_AccountingDeny),
                    SendProcessor = o.HasStatus(ProcessTask.Status_SendProcessor),
                    ProcessorReaded=o.HasStatus(ProcessTask.Status_ProcessorReaded),
                    Printed = o.HasStatus(ProcessTask.Status_Print),
                    Quoted=o.HasStatus(ProcessTask.Status_Quoted),
                    o.Cha,
                    o.Xiu,
                    o.FromMobile,
                    o.Checked,
                    ActualHours = o.ActualHours == null ? null : Math.Round(Convert.ToDecimal(o.ActualHours), 2).ToString(),
                    CreationTime = o.CreationTime.ToString("yyyy-MM-dd HH:mm"),
                    o.ProcessTaskStatus,
                    FeeFromProcessor = o.GetPropertyValue<decimal?>("ProcessorFee"),
                    o.Poster,
                    o.Verifier,
                    KaiDate = o.KaiDate != null ? o.KaiDate.Value.ToString("yyyy-MM-dd HH:mm") : "",
                    CreatorName = o.CreatorUser?.Name,
                    o.SheetFile,
                    o.ArrangeDate,
                    o.ReceiveDate,
                    Reason= o.GetPropertyValue("reason")
                };
            });

            var result = new ResultPageDto()
            {
                code = 0,
                count = query.RowCount,
                data = data
            };

            return result;
        }
        #endregion

        #region 加工开单
        /// <summary>
        /// 加工开单提交
        /// modi 20190415 支持多零件开单
        /// </summary>
        /// <param name="processTaskDto"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public virtual async Task<object> SubmitProcessTask(ProcessTaskDto processTaskDto)
        {
            var projectManager = Resolve<IProjectManager>();
            var unitManager = Resolve<MESUnitManager>();
            var processTypeManager = Resolve<ProcessTypeManager>();
            var equipmentManager = Resolve<EquipmentManager>();
            var partManager = Resolve<PartManager>();
            var manager = Manager as ProcessTaskManager;
            //对于模具、加工点、零件、工艺，如果不存在的话需要新增
            var project = await projectManager.GetByProjectSNOrInsert(processTaskDto.ProjectSN);
            var supplier = await unitManager.GetByUnitNameOrInsert(processTaskDto.UnitName,UnitNature.供应商);            
            var processType = await processTypeManager.GetByNameOrInsert(processTaskDto.ProcessTypeName);
            var equipment = await equipmentManager.GetByEquipmentSNOrInsert(processTaskDto.EquipmentSN,processType.Id);
            if (supplier!=null && string.IsNullOrEmpty(supplier.SupplierType))
            {
                supplier.SupplierType = "采购,加工";
            }
            List<UploadFileInfo> files = null;
            UploadFileInfo sheetFile = null;

            #region 读取上传文件
            //modi 2019-03-29
            //如果是微信端开单，约定上传文件的filepath为空，filename中存储微信文件id
            if (processTaskDto.Files != null && processTaskDto.Files.Count(o => string.IsNullOrEmpty(o.FilePath) && !string.IsNullOrEmpty(o.FileName)) > 0)
            {
                var serverIds = processTaskDto.Files.Select(o => o.FileName);
                files = await Resolve<WeiXinAppService>().DownLoadMedia(serverIds.ToArray());
                sheetFile = files[0];
            }
            else
            {
                files = processTaskDto.Files;
                sheetFile = processTaskDto.SheetFile;
            }
            #endregion

            var parts = new List<Part>();

            #region 读取零件 
            //1.默认零件，即开单时在单据中选择的零件
            //如果有零件编号，则表明零件有存在，不然新增零件
            Part mainPart = null;
            if (!processTaskDto.PartSN.IsNullOrEmpty())
            {
                mainPart = await partManager.GetByPartSN(processTaskDto.PartSN);
            }
            else
            {
                //新增零件
                mainPart = await partManager.GenerateNewPart(project, processTaskDto.PartName, processTaskDto.PartSpecification, processTaskDto.PartNum);
            }
            mainPart.EnableProcess = true;//启用生产
            mainPart.PartSpecification = processTaskDto.PartSpecification;
            mainPart.PartNum = processTaskDto.PartNum;
            parts.Add(mainPart);
            //2.附加的批量开单的零件
            if(processTaskDto.ProcessTaskParts!=null && processTaskDto.ProcessTaskParts.Count > 0)
            {
                foreach(var addInPartDto in processTaskDto.ProcessTaskParts)
                {
                    Part addinPart = null;
                    if (!addInPartDto.PartSN.IsNullOrEmpty())
                    {
                        addinPart = await partManager.GetByPartSN(addInPartDto.PartSN);
                    }
                    else
                    {
                        //新增零件
                        addinPart = await partManager.GenerateNewPart(project, addInPartDto.PartName, addInPartDto.PartSpecification, addInPartDto.PartNum);
                    }
                    addinPart.EnableProcess = true;//启用生产
                    addinPart.PartSpecification = addInPartDto.PartSpecification;
                    addinPart.PartNum = addInPartDto.PartNum;                    
                    parts.Add(addinPart);
                }                

            }
            #endregion

            var processTasks = new List<ProcessTask>();

            #region 建立任务
            //打包标记
            var needWrap = processTaskDto.ProcessTaskParts != null && processTaskDto.ProcessTaskParts.Count > 0;
            var wrapperFlag = Guid.NewGuid().ToString();
            foreach (var part in parts)
            {
                var partIndex = parts.IndexOf(part);//索引
                ProcessTask processTask = null;
                if (processTaskDto.Id > 0)
                {
                    processTask = await Manager.GetByIdAsync(processTaskDto.Id);
                    processTaskDto.MapTo(processTask);
                    //if (!string.IsNullOrEmpty(processTask.ProcessSN))
                    //{
                    //    //已开单的需要判断开单权限
                    //    if(!await manager.CanKaiDan(processTask))
                    //    {
                    //        throw new UserFriendlyException(L("对不起,您无权进行此修改"));
                    //    }
                    //    //如果修改了加工点或者清空了加工点
                    //    //if(processTask.SupplierId!=null &&(supplier==null || supplier.Id != processTask.SupplierId))
                    //    //{
                    //    //    needReKaiDan = true;
                    //    //}
                    //}

                }
                else
                {
                    var maxSort = 0;
                    try
                    {
                        maxSort = await Repository.GetAll().Where(o => o.PartId == part.Id).MaxAsync(o => o.Sort);
                    }
                    catch
                    {

                    }
                    processTask = processTaskDto.MapTo<ProcessTask>();
                    processTask.Sort = maxSort++;
                }

                processTask.PartId = part.Id;
                processTask.ProcessTypeId = processType.Id;
                processTask.SupplierId = supplier?.Id;
                processTask.EquipmentId = equipment?.Id;

                processTask.Files = files;
                //如果多零件开单中提交了图片，则使用多零件中的图片,否则使用主零件的图片
                if(partIndex>0 && processTaskDto.ProcessTaskParts.ElementAt(partIndex - 1).SheetFile != null)
                {
                    processTask.SheetFile = processTaskDto.ProcessTaskParts.ElementAt(partIndex - 1).SheetFile;
                }
                else
                {
                    processTask.SheetFile = sheetFile;
                }
                


                //add 20190322如果设置了设备且未设置安排上机时间，则取预约日期为安排上机时间
                if (processTask.EquipmentId.HasValue && !processTask.ArrangeDate.HasValue && processTask.AppointDate.HasValue)
                {
                    processTask.ArrangeDate = processTask.AppointDate;
                    //add 20190427，增加安排下机时间的设定
                    processTask.ArrangeEndDate = processTask.ArrangeDate.Value.AddHours(Convert.ToDouble(processTask.EstimateHours ?? 0));
                }
                //modi 20181208 tiansfather 如果上传了加工示意图且零件没有图片，则将此加工示意图作为零件图片
                if (string.IsNullOrEmpty(part.PartImg) && processTask.SheetFile != null && !string.IsNullOrEmpty(processTask.SheetFile.FilePath))
                {
                    part.PartImg = processTask.SheetFile.FilePath;
                }
                //如果是已开单（单号不为空）并且不能开单
                if (!string.IsNullOrEmpty(processTask.ProcessSN) && !await manager.CanKaiDan(processTask))
                {
                    //已开单的需要判断开单权限
                    throw new UserFriendlyException(L("对不起,您无权进行此修改"));
                    //如果修改了加工点或者清空了加工点
                    //if(processTask.SupplierId!=null &&(supplier==null || supplier.Id != processTask.SupplierId))
                    //{
                    //    needReKaiDan = true;
                    //}
                }
                //加工开单
                if (await manager.CanKaiDan(processTask) && string.IsNullOrEmpty(processTask.ProcessSN))
                {
                    await manager.KaiDan(processTask);
                }
                //如果设置了模具组长，则更新项目列表中的模具组长
                if (!processTask.ProjectCharger.IsNullOrEmpty())
                {
                    project.SetPropertyValue("ProjectCharger", processTask.ProjectCharger);
                }

                //计算加工费用
                manager.CalcTaskFee(processTask);
                //if (needReKaiDan)
                //{
                //    //重新开单
                //    var newProcessTask = new ProcessTask();
                //    processTask.MapTo(newProcessTask);
                //    //删除原有任务
                //    await manager.DeleteAsync(processTask);
                //    //增加新任务
                //    newProcessTask.ProcessSN = "";                
                //    //await manager.InsertAsync(newProcessTask);
                //    //await CurrentUnitOfWork.SaveChangesAsync();
                //    await manager.KaiDan(newProcessTask);
                //    return new { newProcessTask.Id, PartId = part.Id, part.PartSN, newProcessTask.ProcessSN };
                //}
                //else
                //{

                //}
                processTask.SetPropertyValue("reason", processTaskDto.Reason);

                if (needWrap)
                {
                    //设置打包标记
                    processTask.SetPropertyValue("wrapperFlag", wrapperFlag);
                }

                processTasks.Add(processTask);
                await manager.SaveAsync(processTask);

                await CurrentUnitOfWork.SaveChangesAsync();
                //add 20190413 by tiansfather 开单审核提醒
                if (await manager.NeedConfirm(processTask))
                {
                    await manager.SendTaskConfirmRemind(processTask);
                }
            }
            #endregion
                                 
            


            //返回第一个任务数据
            return new { processTasks[0].Id,
                PartId = parts[0].Id,
                parts[0].PartSN,
                processTasks[0].ProcessSN,
                parts[0].PartName ,
                processTasks[0].ProcessTaskStatus,
                processTasks[0].SheetFile,
                processTasks[0].Files,
                processTasks[0].SupplierId
            };

        }
        
        /// <summary>
        /// 加工路线添加工序
        /// </summary>
        /// <param name="simpleSubmitTaskDto"></param>
        /// <returns></returns>
        public virtual async Task<int> SubmitSimpleProcessType(SimpleSubmitTaskDto simpleSubmitTaskDto)
        {
            var manager = Manager as ProcessTaskManager;
            var maxSort = 0;
            try
            {
                maxSort = await Repository.GetAll().Where(o => o.PartId == simpleSubmitTaskDto.PartId).MaxAsync(o => o.Sort);
            }
            catch
            {

            }
            
            var processTask = simpleSubmitTaskDto.MapTo<ProcessTask>();
            // 读取上一工序的开单人、组长、审核、工艺师、检验
            var lastProcess =await  Repository.FirstOrDefaultAsync(o => o.PartId == simpleSubmitTaskDto.PartId&& o.Sort == maxSort);

            if (lastProcess != null)
            {
                processTask.Poster = lastProcess.Poster;
                processTask.ProjectCharger = lastProcess.ProjectCharger;
                processTask.Verifier = lastProcess.Verifier;
                processTask.CraftsMan = lastProcess.CraftsMan;
                processTask.Checker = lastProcess.Checker;
                //if (!string.IsNullOrEmpty(lastProcess.Poster))
                //{
                //    processTask.Poster = lastProcess.Poster;
                //}
                //if (!string.IsNullOrEmpty(lastProcess.ProjectCharger))
                //{
                //    processTask.ProjectCharger = lastProcess.ProjectCharger;
                //}
            }
            
            processTask.Sort = maxSort++;
            //默认按时间
            processTask.FeeType = FeeType.按时间;

            //开单来源     //默认厂内
            var  inner =  await SettingManager.GetSettingValueAsync<bool>("MES.DefaultSourceInner");
            
            processTask.Inner = inner;

           
            await manager.SaveAsync(processTask);
            await CurrentUnitOfWork.SaveChangesAsync();

            //厂内自动开单
            if (inner)
            {
                await manager.KaiDan(processTask);
            
            }


            return processTask.Id;
        }
        #endregion

        #region 与云加工平台交互
        /// <summary>
        /// 将任务发送至MES
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public virtual async Task SendToMES(int taskId)
        {
            var companySN = await SettingManager.GetSettingValueAsync(MESSettingNames.MESCompanySN);
            var companyToken = await SettingManager.GetSettingValueAsync(MESSettingNames.MESCompanyToken);

            if (string.IsNullOrEmpty(companySN) || string.IsNullOrEmpty(companyToken))
            {
                throw new UserFriendlyException(L("请绑定企业编号与企业令牌"));
            }

            var task = await Repository.GetAll().Include(o => o.ProcessType)
                .Include(o=>o.Part)
                .Include(o => o.Supplier)
                .Where(o=>o.Id==taskId)
                .SingleOrDefaultAsync();

            if (task.SupplierId == null)
            {
                throw new UserFriendlyException(L("请先设置加工点再发送"));
            }
            if (!task.RequireDate.HasValue)
            {
                throw new UserFriendlyException(L("请先设置要求完成日期"));
            }
            //通过加工点名称云找云加工点编号
            var mesSupplier = await Resolve<CloudEquipmentAppService>().FindByCompanyName(task.Supplier.UnitName);
            if (mesSupplier == null)
            {
                throw new UserFriendlyException($"未找到名称为{task.Supplier.UnitName}的云加工点");
            }
            //接口地址
            string apiUrl = $"http://mes.imould.me/Ajax/ajaxapi.ashx?action=SetCloudtask";

            var formData = new Dictionary<string, string>()
            {
                {"fromCompanySN", companySN},
                {"fromCompanyToken", companyToken},
                {"toCompanySN", mesSupplier.CompanySN},
                {"partName",task.Part.PartName },
                {"spec",task.Part.PartSpecification },
                {"equipmentType",task.ProcessType.ProcessTypeName },
                {"requireDate", task.RequireDate?.ToString("yyyy-MM-dd")},
                {"sourceType","MLMW" },
                {"sourceId",taskId.ToString() }
            };

            var result=await Senparc.CO2NET.HttpUtility.RequestUtility.HttpPostAsync(apiUrl, formData:formData);
            if (!string.IsNullOrEmpty(result))
            {
                throw new UserFriendlyException(result);
            }
        }
        [AbpAllowAnonymous]
        public virtual async Task ReceiveMESTaskInfoForm(int taskId, int receiveMESTaskReportType,DateTime reportTime,string message)
        {
            var receiveMESTaskDto = new ReceiveMESTaskDto()
            {
                TaskId = taskId,
                ReceiveMESTaskReportType = (ReceiveMESTaskReportType)receiveMESTaskReportType,
                ReportTime = reportTime,
                Message = message
            };

            await ReceiveMESTaskInfo(receiveMESTaskDto);
        }
        /// <summary>
        /// 接收云加工平台的任务反馈
        /// </summary>
        /// <param name="receiveMESTaskDto"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public virtual async Task ReceiveMESTaskInfo(ReceiveMESTaskDto receiveMESTaskDto)
        {
            var manager = Manager as ProcessTaskManager;
            var personManager = Resolve<PersonManager>();
            var processTaskReportManager = Resolve<ProcessTaskReportManager>();
            try
            {
                var processTask = await manager.GetByIdAsync(receiveMESTaskDto.TaskId);

                if (receiveMESTaskDto.ReceiveMESTaskReportType == ReceiveMESTaskReportType.接收)
                {
                    //todo:接收任务另外处理
                    return;
                }

                //生成报工人
                var reporter = await personManager.GetDefaultPersonBySourceOrInsert(PersonSource.MES);
                var processTaskReport = new ProcessTaskReport()
                {
                    ProcessTaskId = receiveMESTaskDto.TaskId,
                    TenantId = processTask.TenantId,
                    Remarks = receiveMESTaskDto.Message,
                    ReportTime = receiveMESTaskDto.ReportTime
                };
                processTaskReport.ReporterId = reporter.Id;

                switch (receiveMESTaskDto.ReceiveMESTaskReportType)
                {
                    case ReceiveMESTaskReportType.上机:
                        processTaskReport.ReportType = ReportType.上机;
                        break;
                    case ReceiveMESTaskReportType.加工:
                        processTaskReport.ReportType = ReportType.加工;
                        break;
                    case ReceiveMESTaskReportType.下机:
                        processTaskReport.ReportType = ReportType.下机;
                        break;
                }

                await processTaskReportManager.InsertAsync(processTaskReport);
                await CurrentUnitOfWork.SaveChangesAsync();
                //更新任务状态
                await manager.UpdateTaskStatusByReport(processTaskReport);
            }catch(Exception ex)
            {
                Logger.Error(ex.Message + ex.StackTrace);
                throw new UserFriendlyException(ex.Message);
            }
            
        }
        #endregion

        #region 报工页获取报工任务接口
        /// <summary>
        /// 获取待报工任务相关数据,用于报工页
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public virtual async Task<object> GetReportTaskInfoById(int taskId)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant))
            {
                var task = await Repository.GetAllIncluding(o => o.ProcessType, o => o.Tenant).Include(o => o.Part).ThenInclude(o => o.Project)
                .Where(o => o.Id == taskId).SingleOrDefaultAsync();

                if (task == null)
                {
                    throw new UserFriendlyException(L("未找到任务"));
                }

                return new
                {
                    task.TenantId,
                    task.Part.PartName,
                    task.Part.PartSpecification,
                    task.Part.PartNum,
                    task.ProcessType.ProcessTypeName,
                    task.Tenant.TenancyName,
                    task.Part.Project.ProjectSN,
                    task.ProcessTaskStatus,
                    task.Progress,
                    FeeFromProcessor=task.GetPropertyValue<decimal?>("ProcessorFee"),
                    task.EstimateHours,
                    task.TaskInfo,
                    task.SheetFile,//加工示意图
                    task.Files,//附件
                    SubmitFeeFromProcessor = task.GetPropertyValue<SubmitFeeFromProcessorDto>("SubmitFeeFromProcessorDto"),
                };
            }
            
        }
        #endregion

        #region 报工提交
        /// <summary>
        /// 微信端报工提交
        /// </summary>
        /// <param name="processTaskReportDto"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public virtual async Task Report(ProcessTaskReportDto processTaskReportDto)
        {
            var personManager = Resolve<PersonManager>();
            var processTaskReportManager = Resolve<ProcessTaskReportManager>();
            var fileManager = Resolve<IFileManager>();
            var manager = Manager as ProcessTaskManager;
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant,AbpDataFilters.MayHaveTenant))
            {
                var processTaskReport = processTaskReportDto.MapTo<ProcessTaskReport>();
                var processTask = await manager.GetByIdAsync(processTaskReport.ProcessTaskId);
                //由于是微信提交，需要手动补充账套id
                processTaskReport.TenantId = processTask.TenantId;
                //报工人
                var weuser = WeiXinHelper.GetWeiXinUserInfo();
                var reporter = await personManager.GetPersonByWeUserOrInsert(weuser);
                processTaskReport.ReporterId = reporter.Id;
                var now = DateTime.Now;
                string upload_path = $"{Directory.GetCurrentDirectory()}\\wwwroot\\files\\{now.Year}\\{now.ToString("MM")}\\{now.ToString("dd")}\\";
                //报工文件
                var files = new List<UploadFileInfo>();
                foreach (var mediaId in processTaskReportDto.MediaIds)
                {
                    var path = await WeiXinHelper.DownloadMedia(mediaId, upload_path);
                    files.Add(new UploadFileInfo()
                    {
                        FilePath = fileManager.AbsolutePathToVirtualPath(path)
                    });
                }
                processTaskReport.Files = files;

                await processTaskReportManager.InsertAsync(processTaskReport);
                await CurrentUnitOfWork.SaveChangesAsync();
                //更新任务状态
                await manager.UpdateTaskStatusByReport(processTaskReport);
            }
            
            
        }
        #endregion

        #region 任务报工统计
        /// <summary>
        /// 获取任务的各种报工数量
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public virtual async Task<TaskReportInfoDto> GetTaskReportInfo(int taskId)
        {
            var task = await Manager.GetAll().Include(o => o.ProcessTaskReports)
                .Where(o => o.Id == taskId).SingleAsync();
            var result = new TaskReportInfoDto()
            {
                ReceiveDate=task.ReceiveDate,
                ReceiveDateFromReport=task.ProcessTaskReports.Count(o=>o.ReportType==ReportType.到料)>0,
                StartDate=task.StartDate,
                StartDateFromReport = task.ProcessTaskReports.Count(o => o.ReportType == ReportType.上机) > 0,
                EndDate =task.EndDate,
                EndDateFromReport = task.ProcessTaskReports.Count(o => o.ReportType == ReportType.下机) > 0,
            };

            return result;
        }
        #endregion

        #region 报工记录


        /// <summary>
        /// 获取某任务的所有报工记录
        /// </summary>
        /// <param name="processTaskId"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<object> GetProcessTaskReports(int processTaskId)
        {
            var processTaskReportManager = Resolve<ProcessTaskReportManager>();
            var reports = await processTaskReportManager.GetAll().Include(o=>o.Reporter).Where(o => o.ProcessTaskId == processTaskId).ToListAsync();

            return reports.Select(o =>
            new {
                o.Id,
                ReporterName=o.Reporter.Name,
                o.ReportType,
                ReportTime=o.ReportTime.ToString("yyyy-MM-dd HH:mm:ss"),
                CreationTime=o.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                o.Remarks,
                o.Files
            });
        }
        #endregion

        #region 任务详情
        /// <summary>
        /// 获取任务明细
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public virtual async Task<object> GetTaskInfoById(int taskId)
        {
            var task = await Repository.GetAllIncluding(o => o.ProcessType, o => o.Tenant,o=>o.Equipment).Include(o => o.Part).ThenInclude(o => o.Project)
                .Include(o=>o.Supplier)
                .Include(o=>o.Tenant)
                .Where(o => o.Id == taskId).SingleOrDefaultAsync();

            if (task == null)
            {
                throw new UserFriendlyException(L("未找到对应任务,或相关零件及项目信息已被删除"));
            }

            var uploadFiles = task.Files;
            //获取此加工单直接在模板中上传的文件
            var sheetFile = task.SheetFile;
            //如果加工图片未上传，则读取零件图片
            if (sheetFile == null)
            {
                sheetFile = new UploadFileInfo() { FilePath = task.Part.PartImg };
            }

            //如果是批量开单的，同时返回打包的零件数据
            object wrappedInfo = null;
            var wrapperFlag = task.GetPropertyValue<string>("wrapperFlag");
            if (!string.IsNullOrEmpty(wrapperFlag))
            {
                wrappedInfo = await Manager.GetAll().Include(o => o.Part)
                    .Where(o=>o.Id!=taskId)
                    .Where(o => MESDbContext.GetJsonValueString(o.Property, "$.wrapperFlag") == wrapperFlag)
                    .Select(o => new
                    {
                        TaskId = o.Id,
                        o.Part.PartSN,
                        o.Part.PartName,
                        o.Part.PartSpecification,
                        o.Part.PartNum,
                        o.SheetFile
                    })
                    .ToListAsync();
            }

            return new
            {
                task.ProcessTaskReports,
                task.TenantId,
                task.SupplierId,
                task.Id,
                PartId = task.Part.Id,
                task.ProcessSN,
                task.Part.PartSN,
                task.Part.PartName,
                task.Part.PartSpecification,
                task.Part.PartNum,
                task.ProcessType.ProcessTypeName,
                task.Tenant.TenancyName,
                task.Part.Project.ProjectSN,
                task.Supplier?.UnitName,
                task.Equipment?.EquipmentSN,
                task.Poster,
                task.ProjectCharger,
                task.Verifier,
                task.Checker,
                task.CraftsMan,
                task.TaskInfo,
                task.StartDate,
                task.EndDate,
                task.FeeType,
                task.Price,
                task.FeeFactor,
                task.EstimateHours,
                task.JobFee,
                task.Fee,
                task.Tenant.Logo,
                task.ProcessTaskStatus,
                Rate = task.GetPropertyValue<int>("Rate"),
                RateInfo = task.GetPropertyValue("RateInfo"),
                QuanlityType = task.GetPropertyValue<int>("QuanlityType"),
                task.RateFeeInfos,
                Emergency = task.HasStatus(ProcessTask.Status_Emergency),
                Verified=task.HasStatus(ProcessTask.Status_Verify),
                Printed=task.HasStatus(ProcessTask.Status_Print),
                Inner=task.HasStatus(ProcessTask.Status_Inner),
                task.Cha,
                task.Xiu,
                RequireDate = task.RequireDate?.ToString("yyyy-MM-dd"),
                AppointDate = task.AppointDate?.ToString("yyyy-MM-dd"),
                Files = uploadFiles,
                task.ProgramFiles,
                task.CheckFiles,
                task.ReturnFiles,
                task.ActualHours,
                sheetFile,
                FeeFromProcessor=task.GetPropertyValue<decimal?>("FeeFromProcessor"),
                task.ProcessTaskDetails,
                //SubmitFeeFromProcessor= task.GetPropertyValue("SubmitFeeFromProcessorDto"),
                //SubmitFeeFromProcessor2 = task.GetPropertyValue<dynamic>("SubmitFeeFromProcessorDto"),
                SubmitFeeFromProcessor = task.GetPropertyValue<SubmitFeeFromProcessorDto>("SubmitFeeFromProcessorDto"),
                task.Part.Project.ProjectName,
                KaiDate = task.KaiDate?.ToString("yyyy-MM-dd HH:mm"),
                Reason = task.GetPropertyValue("reason"),
                wrappedInfo
            };
        }
        #endregion

        #region 附件操作

        /// <summary>
        /// 给加工任务附加文件
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="uploadFileInfo"></param>
        /// <returns></returns>
        public virtual async Task AttachFile(int taskId,UploadFileInfo uploadFileInfo,string rel)
        {
            if (uploadFileInfo == null) { return; }

            var task = await Manager.GetByIdAsync(taskId);
            if (task == null) { return; }
            var files = task.GetData<List<UploadFileInfo>>(rel);
            if (files == null) { files = new List<UploadFileInfo>(); }
            if (!files.Exists(o => o.FilePath == uploadFileInfo.FilePath))
            {
                files.Add(uploadFileInfo);
                task.SetData(rel, files);
            }
            await Manager.UpdateAsync(task);
        }
        /// <summary>
        /// 给加工任务附加文件
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="uploadFileInfos"></param>
        /// <param name="rel"></param>
        /// <returns></returns>
        public virtual async Task AttachFiles(int taskId, IEnumerable<UploadFileInfo> uploadFileInfos, string rel)
        {
            if (uploadFileInfos == null) { return; }

            var task = await Manager.GetByIdAsync(taskId);
            if (task == null) { return; }
            var files = task.GetData<List<UploadFileInfo>>(rel);
            if (files == null) { files = new List<UploadFileInfo>(); }
            foreach(var uploadFileInfo in uploadFileInfos)
            {
                if (!files.Exists(o => o.FilePath == uploadFileInfo.FilePath))
                {
                    files.Add(uploadFileInfo);                    
                }
            }
            task.SetData(rel, files);
            await Manager.UpdateAsync(task);
        }
        /// <summary>
        /// 移除加工附件
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="filePath"></param>
        /// <param name="rel"></param>
        /// <returns></returns>
        public virtual async Task RemoveAttachFile(int taskId,string filePath,string rel)
        {
            var task = await Manager.GetByIdAsync(taskId);
            if (task == null) { return; }
            var files = task.GetData<List<UploadFileInfo>>(rel);
            if (files.Count==0) { return; }

            files.RemoveAll(o => o.FilePath == filePath);
            task.SetData(rel, files);

            await Manager.UpdateAsync(task);
        }

        /// <summary>
        /// 给加工任务附加加工图片
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public virtual async Task AttachSheetFile(int taskId,string filePath)
        {
            var partManager = Resolve<PartManager>();
            if (filePath.IsNullOrWhiteSpace())
            {
                return;
            }
            var task = await Manager.GetByIdAsync(taskId);
            if (task == null) { return; }
            var fileInfo = new UploadFileInfo() { FilePath = filePath };
            task.SheetFile = fileInfo;
            var part = await partManager.GetByIdAsync(task.PartId);
            //modi 20181208 tiansfather 如果上传了加工示意图且零件没有图片，则将此加工示意图作为零件图片
            if (string.IsNullOrEmpty(part.PartImg) )
            {
                part.PartImg = task.SheetFile.FilePath;
            }
            await Manager.UpdateAsync(task);
        }
        #endregion

        #region 加工开单时获取的人员信息
        /// <summary>
        /// 获取历史开单的相关人员
        /// poster,projectcharger,verifier,craftsMan,checker
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<List<dynamic>> GetHistoryPerson(string key)
        {
            var result = new List<dynamic>();
            if (key.ToLower() == "projectcharger")
            {
                //非常重要important!!!!!!不使用自带查询必须手动加上过滤器
                result.AddRange(await DynamicQuery.SelectAsync<string>($"select property->>'$.ProjectCharger' as projectcharger from {nameof(Project)} where property->>'$.ProjectCharger'!='' and tenantid={AbpSession.TenantId.Value} and isdeleted=0 group by projectcharger"));
                Logger.Info("Step1:" + DateTime.Now);
            }
            result.AddRange(await DynamicQuery.SelectAsync<string>($"select {key}  from {nameof(ProcessTask)} where {key}!='' and tenantid={AbpSession.TenantId.Value} and isdeleted=0 group by {key}"));
            //result.AddRange((await Manager.GetAll().Where($"!string.IsNullOrEmpty({key})").GroupBy(key).Select("Key").ToDynamicListAsync()).Distinct());

            return result;
        }


        /// <summary>
        /// 返回上一个任务的人员信息
        /// </summary>
        /// <returns></returns>
        public virtual async Task<object> GetLastTaskPersons()
        {
            var task = await Repository.GetAll().Where(o=>o.ProcessTaskStatus!=ProcessTaskStatus.Inputed).LastOrDefaultAsync();
            if (task == null)
            {
                return new object();
            }
            return new
            {
                task.Poster,
                task.ProjectCharger,
                task.Verifier,
                task.CraftsMan,
                task.Checker
            };
        }
        /// <summary>
        /// 通过项目Id返回模具组长
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public virtual async Task<string> GetProjectChargerByProjectId(int projectId)
        {
            var projectManager = Resolve<IProjectManager>();
            var project = await projectManager.GetByIdFromCacheAsync(projectId);
            return project?.GetPropertyValue<string>("ProjectCharger");
            //var task =await Repository.GetAll().Where(o => o.Part.Project.Id == projectId && !string.IsNullOrEmpty(o.ProjectCharger)).LastOrDefaultAsync();
            //return task?.ProjectCharger;
        }
        /// <summary>
        /// 通过零件Id返回工艺师
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        public virtual async Task<string> GetCraftsManByPartId(int partId)
        {
            var task = await Repository.GetAll().Where(o => o.Part.Id == partId && !string.IsNullOrEmpty(o.CraftsMan)).LastOrDefaultAsync();
            return task?.CraftsMan;
        }
        #endregion

        #region 直接提交报工时间
        /// <summary>
        /// 更新任务的报工时间
        /// </summary>
        /// <param name="updateTaskDateDto"></param>
        /// <returns></returns>
        public virtual async Task SubmitTaskDate(UpdateTaskDateDto updateTaskDateDto)
        {
            //不允许在有下机时间的情况下清空上机时间
            if(/*(updateTaskDateDto.ReceiveDate==null && (updateTaskDateDto.StartDate!=null || updateTaskDateDto.EndDate != null))||*/
                (updateTaskDateDto.StartDate==null && updateTaskDateDto.EndDate!=null))
            {
                throw new UserFriendlyException(L("提交数据错误,有下机时间时必须填写上机时间"));
            }

            var manager = Manager as ProcessTaskManager;
            var task = await Repository.GetAsync(updateTaskDateDto.Id);
            //清空手动报工记录的判断逻辑
            var reportInfo = await GetTaskReportInfo(updateTaskDateDto.Id);
            if(reportInfo.ReceiveDateFromReport && updateTaskDateDto.ReceiveDate == null)
            {
                throw new UserFriendlyException(L("已经有到料报工,无法清除报料时间,请直接撤消报工记录"));
            }
            if (reportInfo.StartDateFromReport && updateTaskDateDto.StartDate == null)
            {
                throw new UserFriendlyException(L("已经有上机报工,无法清除上机时间,请直接撤消报工记录"));
            }
            if (reportInfo.EndDateFromReport && updateTaskDateDto.EndDate == null)
            {
                throw new UserFriendlyException(L("已经有下机报工,无法清除下机时间,请直接撤消报工记录"));
            }
            //清空数据后的任务状态处理
            //删除下机报工时间
            if(updateTaskDateDto.EndDate==null && task.ProcessTaskStatus == ProcessTaskStatus.Completed)
            {
                task.ProcessTaskStatus = ProcessTaskStatus.Processing;
            }
            if(updateTaskDateDto.StartDate==null && task.ProcessTaskStatus == ProcessTaskStatus.Processing)
            {
                task.ProcessTaskStatus = ProcessTaskStatus.Received;
            }
            if(updateTaskDateDto.ReceiveDate==null && task.ProcessTaskStatus == ProcessTaskStatus.Received)
            {
                task.ProcessTaskStatus = ProcessTaskStatus.WaitForProcess;
            }
            updateTaskDateDto.MapTo(task);            
            //if (updateTaskDateDto.StartDate.IsNullOrEmpty())
            //{
            //    task.StartDate = null;
            //}
            //else
            //{
            //    task.StartDate = DateTime.Parse(updateTaskDateDto.StartDate);
            //}
            //if (updateTaskDateDto.EndDate.IsNullOrEmpty())
            //{
            //    task.EndDate = null;
            //}
            //else
            //{
            //    task.EndDate = DateTime.Parse(updateTaskDateDto.EndDate);
            //}
            await Manager.UpdateAsync(task);
            //设置实际工时及费用
            if(task.StartDate!=null && task.EndDate != null)
            {
                task.ActualHours = Math.Round(Convert.ToDecimal((task.EndDate.Value - task.StartDate.Value).TotalHours),2);
            }
            //manager.SetActualHours(task);
            manager.CalcTaskFee(task);
        }
        #endregion

        #region 批量获取任务信息
        /// <summary>
        /// 批量获取任务信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task<object> GetProcessTaskInfosByIds(int[] ids)
        {
            return await Manager.GetAll().Include(o => o.Part)
                .ThenInclude(o => o.Project)
                .Include(o => o.ProcessType)
                .Include(o => o.Supplier)
                .Where(o => ids.Contains(o.Id))
                .OrderByDescending(o=>o.Id)
                .Select(task=>new {
                    task.TenantId,
                    task.SupplierId,
                    task.Id,
                    PartId = task.Part.Id,
                    task.ProcessSN,
                    task.Part.PartSN,
                    task.Part.PartName,
                    task.Part.PartSpecification,
                    task.Part.PartNum,
                    task.ProcessType.ProcessTypeName,
                    task.Tenant.TenancyName,
                    task.Part.Project.ProjectSN,
                    UnitName=task.Supplier!=null?task.Supplier.UnitName:"",
                    task.Poster,
                    task.ProjectCharger,
                    task.Verifier,
                    task.Checker,
                    task.CraftsMan,
                    task.TaskInfo,
                    task.StartDate,
                    task.EndDate,
                    task.FeeType,
                    task.Price,
                    task.FeeFactor,
                    task.EstimateHours,
                    task.JobFee,
                    task.Fee,
                    task.Tenant.Logo,
                    task.ProcessTaskStatus,
                    task.SheetFile,
                    Rate = task.GetPropertyValue<int>("Rate"),
                    RateInfo = task.GetPropertyValue("RateInfo"),
                    QuanlityType = task.GetPropertyValue<int>("QuanlityType"),
                    task.RateFeeInfos,
                    Emergency = task.HasStatus(ProcessTask.Status_Emergency),
                    Verified = task.HasStatus(ProcessTask.Status_Verify),
                    Printed = task.HasStatus(ProcessTask.Status_Print),
                    Inner = task.HasStatus(ProcessTask.Status_Inner),
                    task.Cha,
                    task.Xiu,
                    RequireDate = task.RequireDate!=null?task.RequireDate.Value.ToString("yyyy-MM-dd"):"",
                    AppointDate = task.AppointDate!=null?task.AppointDate.Value.ToString("yyyy-MM-dd"):"",
                    task.ProgramFiles,
                    task.CheckFiles,
                    task.ReturnFiles,
                    task.ActualHours,
                    FeeFromProcessor = task.GetPropertyValue<decimal?>("FeeFromProcessor"),
                    task.ProcessTaskDetails,
                    SubmitFeeFromProcessor = task.GetPropertyValue("SubmitFeeFromProcessorDto"),
                    task.Part.Project.ProjectName,
                    KaiDate = task.KaiDate!=null?task.KaiDate.Value.ToString("yyyy-MM-dd HH:mm"):"",
                    Reason = task.GetPropertyValue("reason"),
                    MakeInvoice=task.HasStatus(ProcessTask.Status_MakeInvoice)?"1":"",
                    
                })
                .ToListAsync();

        }
        #endregion

        #region 对账
        /// <summary>
        /// 对账提交
        /// </summary>
        /// <param name="checkFeeDtos"></param>
        /// <returns></returns>
        public virtual async Task CheckFee(List<SimpleFeeDto> checkFeeDtos)
        {
            var tasks = await Manager.GetAll().Where(o => checkFeeDtos.Select(dto => dto.Id).Contains(o.Id)).ToListAsync();
            foreach(var task in tasks)
            {
                //task.CheckFee = checkFeeDtos.Single(o => o.Id == task.Id).CheckFee;
                //
                task.Fee = checkFeeDtos.Single(o => o.Id == task.Id).Fee;
                 await Manager.UpdateAsync(task);
            }

            
        }
        #endregion

        #region 核算
        /// <summary>
        /// 录入金额提交
        /// </summary>
        /// <param name="FeeDtos"></param>
        /// <returns></returns>
        public virtual async Task AddFee(List<FeeDto> feeDtos)
        {
            var tasks = await Manager.GetAll().Where(o => feeDtos.Select(dto => dto.Id).Contains(o.Id)).ToListAsync();
            
            foreach (var task in tasks)
            {
                //task.CheckFee = checkFeeDtos.Single(o => o.Id == task.Id).CheckFee;
                //
                var feeDto = feeDtos.Single(o => o.Id == task.Id);
                
                feeDto.MapTo(task);
                if (feeDto.MakeInvoice == "1")
                {
                    task.SetStatus(Master.MES.ProcessTask.Status_MakeInvoice);
                }
                else
                {
                    task.RemoveStatus(Master.MES.ProcessTask.Status_MakeInvoice);
                }
                await Manager.UpdateAsync(task);
            }
           

        }
        /// <summary>
        ///    核算状态
        /// </summary>
        /// <param name="accountingDto"></param>
        /// <returns></returns>
        public virtual async Task AddAccountingResult(AccountingDto accountingDto)
        {
            var ids = accountingDto.Ids.Split(',').ToList().ConvertAll(o=>int.Parse(o));
                var tasks = await Manager.GetAll().IgnoreQueryFilters().Where(o=>ids.Contains(o.Id))
                    .ToListAsync();
            var img = accountingDto.ImgPath;
            if (accountingDto.Flag)
            {
                //无视过滤
                foreach(var task in tasks)
                {
                    //var task = await Manager.GetByIdAsync(Convert.ToInt32(id));
                    task.SetStatus(Master.MES.ProcessTask.Status_AccountingPass);
                    Logger.Error("\r\n\r\nsign:"+img);
                    if (img.EndsWith("signed.png"))
                    {
                        task.SetPropertyValue("SignSrc", img);
                    }
                    
                }
            }
            else
            {
                foreach (var task in tasks)
                {                   
                    task.SetStatus(Master.MES.ProcessTask.Status_AccountingDeny);
                }
            }       

        }
        public virtual async Task<String> SaveBase64(AccountingDto accountingDto)

        {
            //Logger.Error("\r\n\r\nbase:"+ accountingDto.ImgPath);
            String base64 = accountingDto.Base64.Split(",")[1];
            string imgPath = accountingDto.ImgPath;
            System.Drawing.Bitmap bitmap = null;//定义一个Bitmap对象，接收转换完成的图片


            string imagesurl2="";
            try//会有异常抛出，try，catch一下
            {



                String inputStr = base64;//把纯净的Base64资源扔给inpuStr,这一步有点多余

                byte[] arr = Convert.FromBase64String(inputStr);//将纯净资源Base64转换成等效的8位无符号整形数组

                System.IO.MemoryStream ms = new System.IO.MemoryStream(arr);//转换成无法调整大小的MemoryStream对象
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms);//将MemoryStream对象转换成Bitmap对象
                ms.Close();//关闭当前流，并释放所有与之关联的资源
                bitmap = bmp;
                //filename = path + "/Knowledge_" + imgName + ".png";//所要保存的相对路径及名字
                //string tmpRootDir = Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString()); //获取程序根目录 
                //string tmpRootDir = System.AppDomain.CurrentDomain.BaseDirectory;
                string tmpRootDir = $"{Directory.GetCurrentDirectory()}\\wwwroot\\sheets\\signpic";
                if (!string.IsNullOrEmpty(imgPath))
                {
                    if (!imgPath.EndsWith(".png"))
                    {
                        tmpRootDir = $"{Directory.GetCurrentDirectory()}"+imgPath;
                     if (!System.IO.Directory.Exists(tmpRootDir))
                     {
                           System.IO.Directory.CreateDirectory(tmpRootDir);
                          }
                        imagesurl2 = tmpRootDir + "\\test" + DateTime.Today.ToString("yyyyMMdd") + ".png";
                    }
                    if (imgPath.EndsWith(".png")) {
                        imagesurl2 = $"{Directory.GetCurrentDirectory()}"+ imgPath;
                    }
                    //string imagesurl2 = tmpRootDir + filename.Replace(@"/", @"\"); //转换成绝对路径 

                }
                else
                {
                    tmpRootDir = $"{Directory.GetCurrentDirectory()}\\wwwroot\\sheets\\signpic";
                    imagesurl2 = tmpRootDir + "\\test" + DateTime.Today.ToString("yyyyMMdd") + ".png";
                }
                bitmap.Save(imagesurl2, System.Drawing.Imaging.ImageFormat.Png);//保存到服务器路径
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
                Console.WriteLine(f.StackTrace);
            }                                                                                                             
            return imagesurl2.Split("wwwroot")[1];//返回相对路径


        }
        /// <summary>
        /// 判断是否已核算确认
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task<bool> GetIsAccountingConfirmed(int[] ids)
        {
            var tasks = await Manager.GetAll().IgnoreQueryFilters().Where(o => ids.Contains(o.Id))
                     .ToListAsync();
            bool flag = false;
            foreach (var task in tasks)
            {
                //var task = await Manager.GetByIdAsync(Convert.ToInt32(id));
                flag= task.HasStatus(Master.MES.ProcessTask.Status_AccountingPass);
                if (flag)
                {
                    break;
                }
            }
            return flag;
        }
        /// <summary>
        /// 发送核算信息给供应商
        /// </summary>
        /// <param name="FeeDtos"></param>
        /// <returns></returns>
        public  async Task SendFee(List<FeeDto> feeDtos)
        {
            var unitManager = Resolve<MESUnitManager>();
            var remindLogManager = Resolve<RemindLogManager>();
            var tasks = await Manager.GetAll().Where(o => feeDtos.Select(dto => dto.Id).Contains(o.Id)).ToListAsync();
            string processSNs="";
            decimal? totalFee = 0;
            string taskIds = "";
            foreach (var task in tasks)
            {
                //task.CheckFee = checkFeeDtos.Single(o => o.Id == task.Id).CheckFee;
                //
                taskIds += task.Id+ ",";
                var feeDto = feeDtos.Single(o => o.Id == task.Id);
                feeDto.MapTo(task);
                totalFee += feeDto.CheckFee;
                processSNs += task.ProcessSN+",";
                if (feeDto.MakeInvoice == "1")
                {
                    task.SetStatus(Master.MES.ProcessTask.Status_MakeInvoice);
                }
                else
                {
                    
                    task.RemoveStatus(Master.MES.ProcessTask.Status_MakeInvoice);
                }
                await Manager.UpdateAsync(task);
                task.RemoveStatus(Master.MES.ProcessTask.Status_AccountingDeny);
                task.RemoveStatus(Master.MES.ProcessTask.Status_AccountingPass);
            }
            taskIds = taskIds.Substring(0, taskIds.Length - 1);
            processSNs = processSNs.Substring(0, processSNs.Length - 1) ;
            var tenantId = AbpSession.GetTenantId();
            //currentUser = AbpSession.GetTenantId();
            var tenantManager = Resolve<TenantManager>();
            var q = await tenantManager.GetByIdAsync(tenantId);
            var currentUser = q.TenancyName;
            var remindLog = new RemindLog()
            {
                RemindType = "核算提醒",          //外协加工发送提醒
                Name = tasks[0].Supplier.UnitName,
                TenantId = tasks[0].TenantId,
                Message = "核算"
            };
            var remindLogId = await remindLogManager.InsertAndGetIdAsync(remindLog);
            var openids = await unitManager.FindUnitOpenId(tasks[0].Supplier);
           
                var arg = new SendWeiXinMessageJobArgs()
                {
                    OpenId = openids[0],
                    DataId = tasks[0].Supplier.TenantId,
                    RemindLogId = remindLogId,
                    //ExtendInfo= "{\"ids\":\""+taskIds+ "\",\"currentTenantId\":\""+ tenantId + "\", \"totalFee\":\"" + totalFee + "\",\"processSNs\":\"" + processSNs + "\"}"
                     ExtendInfo = taskIds +"test123" + tenantId + "test123" + totalFee + "test123" + processSNs 
                };


              Resolve<IBackgroundJobManager>().Enqueue<SendCountingWeiXinMessageJob, SendWeiXinMessageJobArgs>(arg);
            


      
        }

        #endregion

        #region 审核
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="feeDtos"></param>
        /// <returns></returns>
        public virtual async Task VerifyFee(List<RateFeeDto> feeDtos)
        {
            var tasks = await Manager.GetAll().Where(o => feeDtos.Select(dto => dto.Id).Contains(o.Id)).ToListAsync();
            foreach (var task in tasks)
            {
                //task.CheckFee = checkFeeDtos.Single(o => o.Id == task.Id).CheckFee;
                var feeDto = feeDtos.Single(o => o.Id == task.Id);
                task.Fee = feeDto.Fee;
                task.SetPropertyValue("Rate", feeDto.Rate);
                task.SetPropertyValue("QuanlityType", feeDto.QuanlityType);
                task.SetPropertyValue("RateInfo", feeDto.RateInfo);
                task.SetStatus(Master.MES.ProcessTask.Status_Verify);
                //保存每次提交的审核金额至任务中
                var feeInfo = feeDto.MapTo<RateFeeInfo>();
                feeInfo.Verifier = AbpSession.Name();
                feeInfo.VerifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var feeInfos = task.RateFeeInfos;
                feeInfos.Add(feeInfo);
                task.RateFeeInfos = feeInfos;

                await Manager.UpdateAsync(task);
            }
        }
        #endregion

        #region 重新计算实际工时
        /// <summary>
        /// 强制重新计算实际工时
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task ReActualHours(IEnumerable<int> ids)
        {
            var manager = Manager as ProcessTaskManager;
            var tasks = await manager.GetListByIdsAsync(ids);
            foreach(var task in tasks)
            {
                manager.SetActualHours(task, "1");
            }
             
        }
        #endregion

        #region 调整排序顺序
        public virtual async Task ReSort(IEnumerable<int> ids)
        {
            for(var i=0;i<ids.Count();i++)
            {
                var task = await Manager.GetByIdAsync(ids.ElementAt(i));
                task.Sort = i;
            }

            

        }
        #endregion
       
        #region 高级查询
        

        #region 获取基础模板数据
        public override List<SearchData> GetBaseSearchData()
        {
            List<SearchData> list = new List<SearchData>();

            #region 构建查询内容

            //上机时间
            var StartDateSD = new SearchData()
            {
                Name = "上机时间",
                Keys = "StartDate",
                Model = "ProcessTask",
                SearchType = SearchData.Date,
                CanAnd = true,
            };
            list.Add(StartDateSD);

            //下机时间
            var EndDateSD = new SearchData()
            {
                Name = "下机时间",
                Keys = "EndDate",
                Model = "ProcessTask",
                SearchType = SearchData.Date,
                CanAnd = true,
            };
            list.Add(EndDateSD);

            //创建时间
            var CreationTimeSD = new SearchData()
            {
                Name = "创建时间",
                Keys = "CreationTime",
                Model = "ProcessTask",
                SearchType = SearchData.Date,
                CanAnd = true,
            };
            list.Add(CreationTimeSD);

            //开单时间
            var KaiTimeSD = new SearchData()
            {
                Name = "开单时间",
                Keys = "KaiDate",
                Model = "ProcessTask",
                SearchType = SearchData.Date,
                CanAnd = true,
            };
            list.Add(KaiTimeSD);

            //零件
            var partSD = new SearchData()
            {
                Name = "零件",
                Keys = "PartSN,PartName",
                Model = "Part",
                SearchType = SearchData.Like,
                CanAnd = false,
            };
            list.Add(partSD);

            //模具编号
            var mouldsnSD = new SearchData()
            {
                Name = "模具编号",
                Keys = "ProjectSN",
                Model = "Part.Project",
                SearchType = SearchData.Search,
                CanAnd = false,
            };
            list.Add(mouldsnSD);

            //加工点
            var UnitSD = new SearchData()
            {
                Name = "加工点",
                Keys = "UnitName",
                Model = "Supplier",
                SearchType = SearchData.Search,
                CanAnd = false,
            };
            list.Add(UnitSD);

            //工序
            var ProcessTypeSD = new SearchData()
            {
                Name = "工序",
                Keys = "ProcessTypeName",
                Model = "ProcessType",
                SearchType = SearchData.Search,
                CanAnd = false,
            };
            list.Add(ProcessTypeSD);

            //计价方式
            var FeeTypeSD = new SearchData()
            {
                Name = "计价方式",
                Keys = "FeeType",
                Model = "ProcessTask",
                SearchType = SearchData.Array,
                CanAnd = false,
                ArrayData = new List<string>() { "承包", "按时间", "按平方", "按长度", "按重量", "按数量" },
            };
            list.Add(FeeTypeSD);

            //工时
            //var ActualHoursSD = new SearchData()
            //{
            //    Name = "工时",
            //    Keys = "ActualHours",
            //    Model = "ProcessTask",
            //    SearchType = SearchData.Check,
            //    CanAnd = false,
            //    ArrayData = new List<string>() { "有", "无" },
            //};
            //list.Add(ActualHoursSD);

            //开单人
            var PosterSD = new SearchData()
            {
                Name = "开单人",
                Keys = "Poster",
                Model = "ProcessTask",
                SearchType = SearchData.Search,
                CanAnd = false,
            };
            list.Add(PosterSD);

            //模具组长
            var ProjectChargerSD = new SearchData()
            {
                Name = "模具组长",
                Keys = "ProjectCharger",
                Model = "ProcessTask",
                SearchType = SearchData.Search,
                CanAnd = false,
            };
            list.Add(ProjectChargerSD);

            //工艺师
            var CraftsManSD = new SearchData()
            {
                Name = "工艺师",
                Keys = "CraftsMan",
                Model = "ProcessTask",
                SearchType = SearchData.Search,
                CanAnd = false,
            };
            list.Add(CraftsManSD);

            //审核
            var VerifierSD = new SearchData()
            {
                Name = "审核人",
                Keys = "Verifier",
                Model = "ProcessTask",
                SearchType = SearchData.Search,
                CanAnd = false,
            };
            list.Add(VerifierSD);

            //检验
            var CheckerSD = new SearchData()
            {
                Name = "检验人",
                Keys = "Checker",
                Model = "ProcessTask",
                SearchType = SearchData.Search,
                CanAnd = false,
            };
            list.Add(CheckerSD);

            //开单状态
            var ProcessTaskStatusSD = new SearchData()
            {
                Name = "开单状态",
                Keys = "ProcessTaskStatus",
                Model = "ProcessTask",
                SearchType = SearchData.Array,
                CanAnd = false,
                ArrayData = new List<string>() { "待上机", "已到料", "加工中", "已完成", "暂停中", "已取消" },
            };
            list.Add(ProcessTaskStatusSD);

            //生产状态
            var AboutReportDateSD = new SearchData()
            {
                Name = "生产状态",
                Keys = "AboutReportDate",
                Model = "ProcessTask",
                SearchType = SearchData.Array,
                CanAnd = false,
                ArrayData = new List<string>() { "延期上机", "延期完工", "工期超预期"},
            };
            list.Add(AboutReportDateSD);
            //单据
            //var signSD = new SearchData()
            //{
            //    Name = "单据状态",
            //    Keys = "Sign",
            //    Model = "ProcessTask",
            //    SearchType = SearchData.Array,
            //    CanAnd = false,
            //    ArrayData = new List<string>() { "已回单", "未回单", "已发送", "未发送", "已打印", "未打印", "厂内", "厂外", "加急", "插单", "修模" },
            //};
            //list.Add(signSD);

            //单据
            var signSD = new SearchData()
            {
                Name = "单据状态",
                Keys = "State",
                Model = "ProcessTask",
                SearchType = SearchData.Check,
                CanAnd = false,
                ArrayData = new List<string>() { "已回单", "未回单", "已发送", "未发送", "已打印","未打印"},
            };
            list.Add(signSD);

            //标记
            var stateSD = new SearchData()
            {
                Name = "标记",
                Keys = "Sign",
                Model = "ProcessTask",
                SearchType = SearchData.Check,
                CanAnd = false,
                ArrayData = new List<string>() {"厂内", "厂外", "加急", "插单", "修模","移动端","非移动端" },
            };
            list.Add(stateSD);

            #endregion

            return list;
        }
        #endregion

        #region 获取模块名称（model|key）
        /// <summary>
        /// 得到当前模块标记
        /// </summary>
        /// <returns></returns>
        public virtual string GetModelName()
        {
            return "ProcessTask";
        }

        #endregion

        #endregion

        #region 任务排机
        /// <summary>
        /// 将任务安排至设备
        /// </summary>
        /// <param name="tasksInfo"></param>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public virtual async Task ArrangeTaskToEquipment(JArray tasksInfo,int equipmentId)
        {
            var taskIds=tasksInfo.Select(o => Convert.ToInt32(o["id"]));            
            var manager = Manager as ProcessTaskManager;
            var tasks = await Manager.GetListByIdsAsync(taskIds);

            //var equipmentTasks = await EquipmentManager.GetUnFinishedTasks(equipmentId).ToListAsync();
            //var maxOrder = 0M;
            //if (equipmentTasks.Count() > 0)
            //{
            //    maxOrder = equipmentTasks.Max(o => o.GetPropertyValue<decimal>("OrderInEquipment"));
            //}
            for(var i = 0; i < tasks.Count(); i++)
            {
                var task = tasks.ElementAt(i);
                task.EquipmentId = equipmentId;
                //设置任务的安排上下机时间和预约工时
                task.ArrangeDate = Convert.ToDateTime( tasksInfo.ElementAt(i)["ArrangeDate"]);
                task.EstimateHours = Convert.ToDecimal( tasksInfo.ElementAt(i)["EstimateHours"]);
                task.ArrangeEndDate = task.ArrangeDate.Value.AddHours(Convert.ToDouble(task.EstimateHours));
                //设置为厂内任务
                task.SetStatus(ProcessTask.Status_Inner);
                await manager.KaiDan(task);
            }

            //foreach(var task in tasks)
            //{
            //    task.EquipmentId = equipmentId;
            //    //设置任务的安排上机时间等于计划开始时间
            //    task.ArrangeDate = task.AppointDate??DateTime.Now;
            //    //设置为厂内任务
            //    task.SetStatus(ProcessTask.Status_Inner);
                
            //    //设置任务在设备上的序号
            //    //task.SetPropertyValue("OrderInEquipment", ++maxOrder);
            //    //加工开单
            //    await manager.KaiDan(task);
            //}
        }
        /// <summary>
        /// 取消任务排机
        /// </summary>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        public virtual async Task UnArrangeTasks(int[] taskIds)
        {
            var manager = Manager as ProcessTaskManager;
            var tasks = await manager.GetListByIdsAsync(taskIds);
            foreach(var task in tasks)
            {
                await manager.UnArrange(task);
            }
        }
        #endregion        

        #region 外协加工点提交预计工时和费用
        [AbpAuthorize]
        public virtual async Task SubmitFeeFromProcessor(SubmitFeeFromProcessorDto submitFeeFromProcessorDto)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant))
            {
                var task = await Manager.GetByIdAsync(submitFeeFromProcessorDto.TaskId);
                //if(task.EndDate!=null && (DateTime.Now - task.EndDate.Value).TotalDays > 1)
                //{
                //    throw new UserFriendlyException(L("此任务已下机超过24小时,无法再提交加工费用,请直接联系发包方"));
                //}
                //task.SetPropertyValue("SubmitFeeFromProcessorDto", submitFeeFromProcessorDto);
                task.SetPropertyValue("SubmitFeeFromProcessorDto", new { submitFeeFromProcessorDto.Fee,submitFeeFromProcessorDto.Info,submitFeeFromProcessorDto.Num, submitFeeFromProcessorDto.Price, submitFeeFromProcessorDto.Files });

                #region 20190506 加工点回单后进行提醒开单人
                //获取开单人
                if (!string.IsNullOrEmpty(task.Poster))
                {
                    var poster = await Resolve<UserManager>().GetAll()
                        .IgnoreQueryFilters().Where(o => o.IsActive && !o.IsDeleted && o.TenantId == task.TenantId && o.Name == task.Poster)
                        .FirstOrDefaultAsync();
                    if (poster != null )
                    {
                        Logger.Info("回单提醒2");
                        var openId = poster.GetWechatOpenId();
                        if (!string.IsNullOrEmpty(openId))
                        {
                            Logger.Info("回单提醒3");
                            //先产生一条提醒记录
                            var remindLog = new RemindLog()
                            {
                                RemindType = "外协加工回单提醒",
                                Name = task.Poster,
                                TenantId = task.TenantId,
                                Message = task.ProcessSN
                            };
                            var remindLogId = await Resolve<RemindLogManager>().InsertAndGetIdAsync(remindLog);
                            var arg = new SendWeiXinMessageJobArgs()
                            {
                                OpenId = openId,
                                DataId = task.Id,
                                RemindLogId = remindLogId
                            };


                            Resolve<IBackgroundJobManager>().Enqueue<OuterTaskReturnWeiXinMessageJob, SendWeiXinMessageJobArgs>(arg);
                        }
                    }
                } 
                #endregion

            }
        }
        #endregion

        #region 设置加工任务状态
        public virtual async Task SetTaskStatus(int taskId,string status,bool isSet)
        {
            await (Manager as ProcessTaskManager).SetTaskStatus(new int[] { taskId }, status, isSet);
        }
        public virtual async Task SetTasksStatus(int[] taskIds, string status, bool isSet)
        {
            await (Manager as ProcessTaskManager).SetTaskStatus(taskIds, status, isSet);
        }
        #endregion

        #region 模板
        /// <summary>
        /// 获取可用模板
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public virtual async Task<List<TemplateSelectDto>> GetAvailableTemplates(int taskId)
        {
            var templates = (await (Manager as ProcessTaskManager).GetAvailableTemplates(taskId))
                .Select(o=>new TemplateSelectDto() {
                    Id=o.Id,
                    TemplateName=o.TemplateName
                }).ToList();
            //如果账套模板中不存在加工单模板，则将默认加工单模板加入
            if (!templates.Exists(o => o.TemplateName == MESTemplateSetting.TemplateName_DefaultProcessTask))
            {
                templates=templates.Prepend(new TemplateSelectDto()
                {
                    Id=null,
                    TemplateName=MESTemplateSetting.TemplateName_DefaultProcessTask
                }).ToList();
            }

            return templates;
        }
        /// <summary>
        /// 设置任务的打印模板
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public virtual async Task SetTaskTemplate(int taskId,int? templateId)
        {
            await (Manager as ProcessTaskManager).SetTaskTemplate( taskId,  templateId);
        }
        /// <summary>
        /// 获取任务的打印额外添加数据
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public virtual async Task<Dictionary<string, string>> GetTaskTemplateData(int taskId)
        {
            var task = await Manager.GetByIdFromCacheAsync(taskId);
            return task.GetData<Dictionary<string, string>>("TemplateData");
        }
        /// <summary>
        /// 设置任务的打印额外添加数据
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="templateData"></param>
        /// <returns></returns>
        public virtual async Task SubmitTaskTemplateData(int taskId,Dictionary<string,string> templateData)
        {
            var task = await Manager.GetByIdFromCacheAsync(taskId);
            task.SetData("TemplateData", templateData);
        }
        #endregion

        #region 撤回开单
        public virtual async Task RevertCanDan(int[] taskIds)
        {
            await (Manager as ProcessTaskManager).RevertCanDan(taskIds);
        }
        #endregion

        #region 厂外任务发送
        /// <summary>
        /// 将加工单发送给厂外加工点对应的微信
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public virtual async Task SendToUnit(int taskId)
        {
            var manager = Manager as ProcessTaskManager;
            var unitManager = Resolve<MESUnitManager>();

            var task = await manager.GetAll()
                .Include(o => o.Part)
                .Include(o => o.Supplier).Where(o => o.Id == taskId)
                .SingleOrDefaultAsync();
            if (task == null)
            {
                throw new UserFriendlyException(L("未找到对应任务"));
            }
            if (task.SupplierId == null)
            {
                throw new UserFriendlyException(L("请先选择加工点再进行发送"));
            }

            //获取用户中有外协标记的用户
            var openids = await unitManager.FindUnitOpenId(task.Supplier, MESStatusNames.ReceiveOuterTask);

            ////将加工单生成为图片
            await manager.SaveTaskSheetToImage2(task);

            //设置外协发送标记
            task.SetStatus(ProcessTask.Status_SendProcessor);
            //如果任务是打包任务，则对应的所有打包任务也设置为发送状态
            var wrapperFlag = task.GetPropertyValue<string>("wrapperFlag");
            if (!string.IsNullOrEmpty(wrapperFlag))
            {
                var wrappedTasks = await manager.GetAll()
                    .Where(o => MESDbContext.GetJsonValueString(o.Property, "$.wrapperFlag") == wrapperFlag)
                    .ToListAsync();
                foreach(var wrappedTask in wrappedTasks)
                {
                    wrappedTask.SetStatus(ProcessTask.Status_SendProcessor);
                }
            }
            //add 20190429 发送后设置此任务的接收方账套Id
            task.ToTenantId = task.Supplier.GetPropertyValue<int>("TenantId");
            //先产生一条提醒记录
            var remindLog = new RemindLog()
            {
                RemindType = "外协加工发送提醒",
                Name = task.Supplier.UnitName,
                TenantId = task.TenantId,
                Message = task.ProcessSN
            };
            var remindLogId = await Resolve<RemindLogManager>().InsertAndGetIdAsync(remindLog);
            foreach (var openid in openids)
            {
                var arg = new SendWeiXinMessageJobArgs()
                {
                    OpenId = openid,
                    DataId = taskId,
                    RemindLogId = remindLogId
                };


                Resolve<IBackgroundJobManager>().Enqueue<SendOuterTaskWeiXinMessageJob, SendWeiXinMessageJobArgs>(arg);
            }

        }

        #endregion

        #region 新版发送生成加工单至静态文件
        /// <summary>
        /// 将加工单生成静态Html文件
        /// </summary>
        /// <param name="html"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public virtual async Task SaveTaskToHtmlAndSendUnit([FromBody]string html,int taskId)
        {
            var manager = Manager as ProcessTaskManager;
            var processTask = await manager.GetAll()
                .Include(o=>o.Part).Where(o=>o.Id==taskId)
                .SingleOrDefaultAsync();

            if (string.IsNullOrEmpty(processTask.ProcessSN))
            {
                throw new UserFriendlyException(L("不能对未开单任务进行此操作"));
            }

            string upload_path = $"{Directory.GetCurrentDirectory()}\\wwwroot\\sheets\\{processTask.TenantId}\\ProcessSheet\\{processTask.Part?.ProjectId ?? 0}\\";
            if (!System.IO.Directory.Exists(upload_path))
            {
                System.IO.Directory.CreateDirectory(upload_path);
            }
            var filename = upload_path + "\\" + processTask.ProcessSN + ".html";

            await System.IO.File.WriteAllTextAsync(filename, html);

            await SendToUnit(taskId);
        }
        #endregion
        public virtual async Task<String> SaveTaskToHtml([FromBody]string html, int taskId)
        {
            Logger.Error("\r\n\r\nstart");
            var manager = Manager as ProcessTaskManager;
            var processTask = await manager.GetAll()
                .Include(o => o.Part).Where(o => o.Id == taskId)
                .SingleOrDefaultAsync();
            Logger.Error("\r\n\r\nmiddel");
            string upload_path = $"{Directory.GetCurrentDirectory()}\\wwwroot\\sheets\\{processTask.TenantId}\\ProcessSheet\\{processTask.Part?.ProjectId ?? 0}\\";
            if (!System.IO.Directory.Exists(upload_path))
            {
                System.IO.Directory.CreateDirectory(upload_path);
            }
            var filename = upload_path + "\\" + processTask.ProcessSN + ".html";

            await System.IO.File.WriteAllTextAsync(filename, html);
            await manager.SaveTaskSheetToImage2(processTask);
            Logger.Error("\r\n\r\ntest"+ upload_path);
            return upload_path;
        }
        //public virtual async Task<String> SaveAccountingToImg([FromBody]string html,int taskId)
        //{
        //    Logger.Error("\r\n\r\nstart");
        //    var manager = Manager as ProcessTaskManager;
        //    var processTask = await manager.GetAll()
        //        .Include(o => o.Part).Where(o => o.Id == taskId)
        //       .SingleOrDefaultAsync();
        //    Logger.Error("\r\n\r\nmiddel");
        //    string upload_path = $"{Directory.GetCurrentDirectory()}\\wwwroot\\sheets\\{AbpSession.GetTenantId()}\\CountingPic\\{processTask.Supplier.TenantId}\\{DateTime.Today.ToString("yyyyMMdd")}";
        //    //string upload_path = $"{Directory.GetCurrentDirectory()}\\wwwroot\\sheets\\{processTask.TenantId}\\ProcessSheet\\{processTask.Part?.ProjectId ?? 0}\\";
        //    if (!System.IO.Directory.Exists(upload_path))
        //    {
        //        System.IO.Directory.CreateDirectory(upload_path);
        //    }
        //    var time = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        //    var filename = upload_path + "\\" + time + ".html";

        //    await System.IO.File.WriteAllTextAsync(filename, html);
        //    var imagename = upload_path + "\\" + time + ".png";       
        //    try
        //    {
        //        //Common.Fun.Html2Pdf(htmlUrl, filename, "Powerd By 模来模往");
        //        Common.Fun.Html2Image(filename, imagename);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex.Message, ex);
        //        throw new UserFriendlyException(L("生成图片失败"));
        //    }
        //    //await manager.SaveTaskSheetToImage2(processTask);
        //    Logger.Error("\r\n\r\ntest" + imagename);
        //    return imagename;
        //}
        public virtual async Task<String> GetTaskPicSrc(int taskId)
        {
            Logger.Error("\r\n\r\nstart"+taskId);
            var manager = Manager as ProcessTaskManager;
            var processTask = await manager.GetAll()
                .IgnoreQueryFilters()
                .Where(o=>!o.IsDeleted)
               .Include(o => o.Part).Where(o => o.Id == taskId)
               .SingleOrDefaultAsync();
            string upload_path = $"/sheets/{processTask.TenantId}/ProcessSheet/{processTask.Part?.ProjectId ?? 0}/{processTask.ProcessSN }.png";
            Logger.Error("\r\n\r\nend" + upload_path);
            return upload_path;
        }
        //public override async Task<object> GetFilterColumnPageResult(RequestPageDto request)
        //{
        //    var pageResult = await GetPageResultQueryable(request);
        //    return await pageResult.Queryable.Include("Part").GroupBy(request.FilterField).ToDynamicListAsync();
        //}
    }


}
