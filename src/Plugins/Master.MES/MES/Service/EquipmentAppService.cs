using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Master.Configuration;
using Master.Entity;
using Master.EntityFrameworkCore;
using Master.MES.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES.Service
{

    [AbpAuthorize]
    public class EquipmentAppService : ModuleDataAppServiceBase<Equipment, int>
    {
        

        public IFileManager FileManager { get; set; }
        public ProcessTypeManager ProcessTypeManager { get; set; }
        public ProcessTaskManager ProcessTaskManager { get; set; }
        public IRepository<EquipmentProcessType,int> EquipmentProcessTypeRepository { get; set; }
        protected override string ModuleKey()
        {
            return nameof(Equipment);
        }

        protected async override Task<IQueryable<Equipment>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<Equipment> query)
        {
            var queryable=await base.BuildSearchQueryAsync(searchKeys, query);
            if (searchKeys.ContainsKey("processTypeId"))
            {
                var processTypeId = int.Parse(searchKeys["processTypeId"]);
                queryable = from equipment in Manager.GetAll()
                            join equipmentProcessType in EquipmentProcessTypeRepository.GetAll() on equipment.Id equals equipmentProcessType.EquipmentId
                            where equipmentProcessType.ProcessTypeId == processTypeId
                            select equipment;

            }

            return queryable;
        }

        #region 绑定及解绑用户至设备
        /// <summary>
        /// 绑定当前用户至某设备
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public virtual async Task BindUser(int[] equipmentIds)
        {
            var manager = Manager as EquipmentManager;
                                                        
            await manager.BindUser(equipmentIds, AbpSession.UserId.Value);
            //var equipment = await manager.GetByIdAsync(equipmentId);
            //equipment.OperatorId = AbpSession.UserId;
            ////设置当前用户的绑定设备
            //var user = await UserManager.GetByIdAsync(AbpSession.UserId.Value);
            //user.SetPropertyValue("EquipmentId", equipmentId);
        }
        /// <summary>
        /// 解绑设备
        /// </summary>
        /// <param name="equipmentIds"></param>
        /// <returns></returns>
        public virtual async Task UnbindUser(int[] equipmentIds)
        {
            var manager = Manager as EquipmentManager;

            await manager.UnbindUser(equipmentIds, AbpSession.UserId.Value);

            //var equipment = await manager.GetByIdAsync(equipmentId);
            //equipment.OperatorId = AbpSession.UserId;
            ////设置当前用户的绑定设备
            //var user = await UserManager.GetByIdAsync(AbpSession.UserId.Value);
            //user.SetPropertyValue("EquipmentId", equipmentId);
        }
        #endregion

        #region 获取操作工的操作设备
        public virtual async Task<List<EquipmentInfoDto>> GetByOperatorId(long operatorId)
        {
            var manager = Manager as EquipmentManager;
            return (await manager.GetByOperatorId(operatorId)).MapTo<List<EquipmentInfoDto>>();
        }
        #endregion


        #region 获取设备信息
        /// <summary>
        /// 设备信息
        /// </summary>
        /// <param name="processTypeName"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public virtual async Task<List<EquipmentInfoDto>> GetEquipmentInfosByProcessTypeName(string processTypeName, int? unitId)
        {
            //返回所有设备
            if (string.IsNullOrEmpty(processTypeName))
            {
                return await GetEquipmentInfos(null, unitId);
            }

            var processType = await ProcessTypeManager.GetAll().Where(o => o.ProcessTypeName == processTypeName).FirstOrDefaultAsync();
            if (processType != null)
            {
                return await GetEquipmentInfos(processType.Id, unitId);
            }
            else
            {
                return new List<EquipmentInfoDto>();
            }
        }

        /// <summary>
        /// 设备信息
        /// </summary>
        /// <param name="processTypeId"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public virtual async Task<List<EquipmentInfoDto>> GetEquipmentInfos(int? processTypeId,int? unitId)
        {
            var manager = Manager as EquipmentManager;
            var processTypes = await ProcessTypeManager.GetAllList();

            var query = Manager.GetAll().Where(o => o.UnitId == unitId);
            if (processTypeId.HasValue)
            {
                query = from equipment in query
                        join equipmentProcessType in EquipmentProcessTypeRepository.GetAll() on equipment.Id equals equipmentProcessType.EquipmentId
                        where equipmentProcessType.ProcessTypeId == processTypeId
                        select equipment;
            }

            var equipments = await query.Include(o => o.EquipmentProcessTypes).ToListAsync();
            var result = new List<EquipmentInfoDto>();

            foreach(var equipment in equipments)
            {
                var dto = equipment.MapTo<EquipmentInfoDto>();
                dto.ProcessTypeName = string.Join(',', equipment.EquipmentProcessTypes.Select(o => processTypes.FirstOrDefault(p => p.Id == o.ProcessTypeId)?.ProcessTypeName));
                result.Add(dto);
            }

            return result;
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public virtual async Task<EquipmentInfoDto> GetEquipmentInfo(int equipmentId)
        {
            var processTypes = await ProcessTypeManager.GetAllList();
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant))
            {
                //不直接调用Manager.GetAll，以避免使用底层权限过滤
                var equipment = await Manager.Repository.GetAll().Include(o => o.EquipmentProcessTypes).Where(o => o.Id == equipmentId)
                    .SingleOrDefaultAsync();

                var dto = equipment.MapTo<EquipmentInfoDto>();
                dto.ProcessTypeName = string.Join(',', equipment.EquipmentProcessTypes.Select(o => processTypes.FirstOrDefault(p => p.Id == o.ProcessTypeId)?.ProcessTypeName));
                return dto;
            }
        }


        /// <summary>
        /// 设备现场信息
        /// </summary>
        /// <param name="equipmentProcessInfoSearchDto"></param>
        /// <returns></returns>
        public virtual async Task<List<EquipmentProcessInfoDto>> GetEquipmentProcessInfos(EquipmentProcessInfoSearchDto equipmentProcessInfoSearchDto)
        {
            var manager = Manager as EquipmentManager;
            var processTypes = await ProcessTypeManager.GetAllList();

            var query = Manager.GetAll()
                .Include(o=>o.Operator)
                .Where(o=>o.UnitId== equipmentProcessInfoSearchDto.UnitId)
                .WhereIf(!equipmentProcessInfoSearchDto.Keyword.IsNullOrEmpty(),o=>o.EquipmentSN.Contains(equipmentProcessInfoSearchDto.Keyword));
            if (equipmentProcessInfoSearchDto.ProcessTypeId.HasValue)
            {
                query = from equipment in query
                        join equipmentProcessType in EquipmentProcessTypeRepository.GetAll() on equipment.Id equals equipmentProcessType.EquipmentId
                            where equipmentProcessType.ProcessTypeId == equipmentProcessInfoSearchDto.ProcessTypeId
                        select equipment;
            }

            var equipments = await query.Include(o=>o.EquipmentProcessTypes).ToListAsync();
            var result=new List<EquipmentProcessInfoDto>();

            var startDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

            foreach(var equipment in equipments)
            {
                var dto = equipment.MapTo<EquipmentProcessInfoDto>();
                dto.Operator = equipment.Operator?.Name;
                dto.ProcessTypeName = string.Join(',',equipment.EquipmentProcessTypes.Select(o => processTypes.FirstOrDefault(p => p.Id == o.ProcessTypeId)?.ProcessTypeName));
                //获取设备正在加工中的任务
                var task = await manager.GetProcessingTask(equipment.Id);
                //设备上安排的未完成任务
                //var unFinishedTasks =await  GetUnFinishedTasks(equipment.Id);
                dto.TaskId = task?.Id;
                dto.PartSN = task?.Part?.PartSN;
                dto.PartName = task?.Part?.PartName;
                dto.ProjectSN = task?.Part?.Project?.ProjectSN;
                dto.ProcessTaskProgressInfo = task?.ProcessTaskProgressInfo ;
                //dto.Tasks = unFinishedTasks;
                dto.TaskNumber = await manager.GetUnFinishedTasks(equipment.Id).CountAsync();
                dto.EquipmentLoadInfo = await manager.GetLoadInfo(equipment.Id, startDate, startDate.AddDays(7));
                result.Add(dto);
            }

            return result;
        }

        /// <summary>
        /// 获取设备上可以排机的最早时间
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public virtual async Task<string> GetAvailableArrangeDate(int equipmentId)
        {
            var manager = Manager as EquipmentManager;
            var task=await manager.GetUnFinishedTasks(equipmentId)
                .Where(o => o.ArrangeDate != null)
                .OrderByDescending(o => o.ArrangeDate)
                .FirstOrDefaultAsync();

            return (task == null ? DateTime.Now : task.ArrangeDate.Value.AddHours(Convert.ToDouble(task.EstimateHours ?? 0))).ToString("yyyy-MM-dd HH:mm:ss");
        }
        #endregion

        #region 云平台同步
        /// <summary>
        /// 从云平台同步设备信息
        /// </summary>
        /// <returns></returns>
        public virtual async Task SyncEquipmentFromMES()
        {
            var companySN = await SettingManager.GetSettingValueAsync(MESSettingNames.MESCompanySN);
            var companyToken = await SettingManager.GetSettingValueAsync(MESSettingNames.MESCompanyToken);

            if (string.IsNullOrEmpty(companySN) || string.IsNullOrEmpty(companyToken))
            {
                throw new UserFriendlyException(L("请绑定企业编号与企业令牌"));
            }

            string apiUrl = $"http://mes.imould.me/Ajax/ajaxapi.ashx?action=GetYunEquipmentList&page=1&pagesize=1000&companysn={companySN}&companytoken={companyToken}";

            var pageResult = await Senparc.CO2NET.HttpUtility.Get.GetJsonAsync<CloudPageResultDto<CloudEquipmentDto>>(apiUrl);

            foreach (var cloudEquipment in pageResult.Data.ObjList)
            {
                if (await Manager.GetAll().CountAsync(o => o.EquipmentSN == cloudEquipment.ResSN) == 0)
                {
                    var processType = await ProcessTypeManager.GetByNameOrInsert(cloudEquipment.EquipmentType);
                    var equipment = new Equipment()
                    {
                        EquipmentSN = cloudEquipment.ResSN,
                        Brand = cloudEquipment.Brand,
                        Price = cloudEquipment.ProcessPrice,
                        Range = cloudEquipment.Range,
                        BuyCost = cloudEquipment.Cost,
                    };
                    int buyYear;
                    int.TryParse(cloudEquipment.BuyYear, out buyYear);
                    if (buyYear > 0)
                    {
                        equipment.BuyYear = buyYear;
                    }
                    //下载设备图片
                    var imgPath = $"http://mes.imould.me/thumb.ashx?fileid={cloudEquipment.EquipmentPic}";
                    var file = await FileManager.DownLoadFile(imgPath);
                    equipment.EquipmentPic = file.Id;
                    var equipmentId = await Manager.InsertAndGetIdAsync(equipment);
                    await EquipmentProcessTypeRepository.InsertAsync(new EquipmentProcessType()
                    {
                        EquipmentId = equipmentId,
                        ProcessTypeId = processType.Id
                    });
                }

            }
        }
        #endregion

        #region 设备相关任务
        /// <summary>
        /// 获取安排在设备上的任务
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<ProcessTaskViewDto>> GetUnFinishedTasks(int equipmentId,int? taskNumber)
        {
            var manager = Manager as EquipmentManager;
            var tasks = await manager.GetUnFinishedTasks(equipmentId, taskNumber).ToListAsync();

            return tasks.MapTo<List<ProcessTaskViewDto>>();
        }
        /// <summary>
        /// 获取一批设备的未完成任务
        /// </summary>
        /// <param name="equipmentIds"></param>
        /// <param name="taskNumber"></param>
        /// <returns></returns>
        public virtual async Task<Dictionary<int, List<ProcessTaskViewDto>>> GetEquipmentsUnFinishedTasks(int[] equipmentIds, int? taskNumber)
        {
            var manager = Manager as EquipmentManager;
            var result = new Dictionary<int, List<ProcessTaskViewDto>>();
            foreach (var equipmentId in equipmentIds)
            {
                var tasks = ((await manager.GetUnFinishedTasks(equipmentId, taskNumber).ToListAsync()).MapTo<List<ProcessTaskViewDto>>());
                result.Add(equipmentId, tasks);
            }

            return result;
        }
        /// <summary>
        /// 获取设备上最近已完成的任务
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="taskNumber"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<ProcessTaskViewDto>> GetFinishedTasks(int equipmentId, int? taskNumber)
        {
            var manager = Manager as EquipmentManager;
            var tasks = await manager.GetFinishedTasks(equipmentId, taskNumber).ToListAsync();

            return tasks.MapTo<List<ProcessTaskViewDto>>();
        }
        /// <summary>
        /// 获取设备上异常报工任务
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<ProcessTaskViewDto>> GetAbnormalReportTasks(int equipmentId)
        {            
            var tasks = await ProcessTaskManager.GetAbnormalReportTasks()
                .Include("Part.Project")
                .Where(o=>o.EquipmentId==equipmentId).ToListAsync();

            return tasks.MapTo<List<ProcessTaskViewDto>>();
        }
        /// <summary>
        /// 对设备上的未完成任务进行重排
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        public virtual async Task ReorderUnFinishedTasks(int[] taskIds)
        {
            throw new UserFriendlyException("此方法未启用");
            var tasks = await ProcessTaskManager.GetListByIdsAsync(taskIds);
            foreach(var task in tasks)
            {
                task.SetPropertyValue("OrderInEquipment", taskIds.ToList().IndexOf(task.Id) + 1);
                await ProcessTaskManager.UpdateAsync(task);
            }
        }
        #endregion

        #region 设备时间轴相关接口
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="processTypeId"></param>
        /// <returns></returns>
        [AbpAuthorize]
        [DontWrapResult]
        public virtual async Task<object> GetEquipmentTimeLineTasks(DateTime from,DateTime to,int? processTypeId)
        {
            var manager = Manager as EquipmentManager;

            var tasks = await ProcessTaskManager
                .GetTimeLineTasksQuery(from,to)
                .Include("Part.Project")
                .Include(o => o.ProcessType)
                .WhereIf(processTypeId.HasValue,o=>o.ProcessTypeId==processTypeId.Value)
                .Select(o=>
                new 
                {
                    o.Id,
                    o.StartDate,
                    o.EndDate,
                    o.Part.Project.ProjectSN,
                    o.Part.PartName,
                    o.ProcessType.ProcessTypeName,
                    o.ArrangeDate,
                    o.ArrangeEndDate,
                    o.AppointDate,
                    o.EquipmentId,
                    o.Progress,
                    o.EstimateHours,
                    o.ProcessTaskStatus,
                    o.ActualHours
                    //id=o.Id,
                    //text=$"{o.Part.Project.ProjectSN}{o.Part.PartName}{o.ProcessType.ProcessTypeName}",
                    //
                    //start_date =o.StartDate.HasValue?o.StartDate.Value.ToString("yyyy-MM-dd HH:mm"):(o.AppointDate.Value).ToString("yyyy-MM-dd HH:mm"),
                    //end_date = (o.ArrangeDate ?? o.AppointDate.Value).AddHours(Convert.ToDouble(o.EstimateHours??0)).ToString("yyyy-MM-dd HH:mm"),
                    //equipmentId=o.EquipmentId,
                    //progress=o.Progress
                })
                .ToListAsync();
            //如果已上机取实际上机时间，不然取安排上机时间
            return tasks.Select(o=> {
                var startDate = o.StartDate ?? o.ArrangeDate??o.AppointDate??DateTime.Now;
                var endDate = o.EndDate ??o.ArrangeEndDate?? startDate.AddHours(Convert.ToDouble(o.EstimateHours ?? 0));
                return new
                {
                    id = o.Id,
                    text = $"{o.ProjectSN}{o.PartName}{o.ProcessTypeName}",
                    progress = o.Progress,
                    equipmentId=o.EquipmentId??-1,
                    o.ProcessTaskStatus,
                    ActualHours=Math.Round(o.ActualHours??0,2),
                    o.EstimateHours,
                    start_date = startDate.ToString("yyyy-MM-dd HH:mm"),
                    end_date= endDate.ToString("yyyy-MM-dd HH:mm")
                };
            });
        }
        /// <summary>
        /// 设备时间轴修改提交
        /// </summary>
        /// <param name="equipmentTimeLineSubmitDto"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public virtual async Task SubmitTimeLineData(EquipmentTimeLineSubmitDto equipmentTimeLineSubmitDto)
        {
            //删除任务
            await ProcessTaskManager.DeleteAsync(equipmentTimeLineSubmitDto.deleteIds);
            foreach(var taskDto in equipmentTimeLineSubmitDto.events)
            {
                var task = await ProcessTaskManager.GetByIdAsync(taskDto.id);
                task.ArrangeDate = taskDto.start_date;
                task.ArrangeEndDate = taskDto.end_date;
                //task.EstimateHours = Math.Round(Convert.ToDecimal((taskDto.end_date - taskDto.start_date).TotalHours),2);
                task.EquipmentId = taskDto.equipmentId;
                //
                if (task.EquipmentId == -1)
                {
                    task.EquipmentId = null;
                }
            }
        }
        #endregion

        #region 设备负荷
        /// <summary>
        /// 获取某一时间段内设备的负荷状态
        /// </summary>
        /// <param name="equipmentIds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public virtual async Task<List<EquipmentLoadInfo>> GetEquipmentLoads(IEnumerable<int> equipmentIds,DateTime startDate,DateTime endDate)
        {
            var manager = Manager as EquipmentManager;
            var result = new List<EquipmentLoadInfo>();
            foreach(var equipmentId in equipmentIds)
            {
                result.Add(await manager.GetLoadInfo(equipmentId, startDate, endDate));
            }

            return result;
        }
        #endregion
    }

    public class EquipmentTimeLineSubmitDto
    {
        public int[] deleteIds { get; set; }
        public List<TimeLineEventDto> events { get; set; }
    }

    public class TimeLineEventDto
    {
        public int id { get; set; }
        public string text { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public int? equipmentId { get; set; }
    }
    public class EquipmentProcessInfoSearchDto
    {
        /// <summary>
        /// 工序id
        /// </summary>
        public int? ProcessTypeId { get; set; }
        public int? UnitId { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
    }
}
