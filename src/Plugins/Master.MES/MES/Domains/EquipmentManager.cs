using Abp.Domain.Repositories;
using Abp.UI;
using Master.Authentication;
using Master.Domain;
using Master.Entity;
using Master.EntityFrameworkCore;
using Master.Module;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES
{
    public class EquipmentManager:ModuleServiceBase<Equipment,int>
    {
        //public ProcessTaskManager ProcessTaskManager { get; set; }
        //public EquipmentOperatorHistoryManager EquipmentOperatorHistoryManager { get; set; }
      
   
        public IRepository<EquipmentProcessType,int> EquipmentProcessTypeRepository { get; set; }

        /// <summary>
        /// 通过设备编号寻找设备，若不存在则新增
        /// </summary>
        /// <param name="equipmentSN"></param>
        /// <param name="processTypeId"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public virtual async Task<Equipment> GetByEquipmentSNOrInsert(string equipmentSN,int processTypeId,int? unitId=null)
        {            
            if (string.IsNullOrEmpty(equipmentSN)) { return null; }
            var equipment = await Repository.GetAll().Where(o => o.EquipmentSN == equipmentSN && o.UnitId==unitId).FirstOrDefaultAsync();
            if (equipment == null)
            {
                equipment = new Equipment()
                {
                    EquipmentSN=equipmentSN
                };
                var equipmentId=await InsertAndGetIdAsync(equipment);
                await EquipmentProcessTypeRepository.InsertAsync(new EquipmentProcessType()
                {
                    EquipmentId = equipmentId,
                    ProcessTypeId = processTypeId
                });
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            return equipment;
        }

        #region 重写增删改查

        /// <summary>
        /// 数据过滤
        /// </summary>
        /// <returns></returns>
        public override IQueryable<Equipment> GetAll()
        {
            var query = base.GetAll();
            //如果没有查看全部权限
            if (!PermissionChecker.IsGrantedAsync("Module.Equipment.Button.ShowAll").Result)
            {
                //只有设备的负责人和编程能看到设备
                query = query.Where(o=>o.ProgrammerId==AbpSession.UserId || o.ArrangerId==AbpSession.UserId);
            }

            return query;
        }

        public async override Task ValidateEntity(Equipment entity)
        {
            //todo:设备提交验证
            if (string.IsNullOrEmpty(entity.EquipmentSN))
            {
                throw new UserFriendlyException(L("设备编号不能为空"));
            }
            if(entity.Id>0 && await Repository.GetAll().CountAsync(o=>o.EquipmentSN==entity.EquipmentSN && o.Id != entity.Id) > 0)
            {
                throw new UserFriendlyException(L("相同设备编号已存在"));
            }

            if (entity.Id == 0 && await Repository.GetAll().CountAsync(o => o.EquipmentSN == entity.EquipmentSN ) > 0)
            {
                throw new UserFriendlyException(L("相同设备编号已存在"));
            }

            await base.ValidateEntity(entity);
        }
        public override async Task DeleteAsync(IEnumerable<int> ids)
        {
            if(await Resolve<ProcessTaskManager>().GetAll().CountAsync(o=>o.EquipmentId!=null && ids.Contains(o.EquipmentId.Value)) > 0)
            {
                throw new UserFriendlyException(L("此设备已被使用,无法删除"));
            }
        }
        public override async Task  DeleteAsync(Equipment entity)
        {
            if (await Resolve<ProcessTaskManager>().GetAll().CountAsync(o => o.EquipmentId.Value ==entity.Id)>0)
            {
                throw new UserFriendlyException(L("此设备已被使用,无法删除"));
            }
        }
        #endregion

        #region 设备交接
        /// <summary>
        /// 解绑设备
        /// </summary>
        /// <param name="equipmentIds"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task UnbindUser(int[] equipmentIds, long userId)
        {
            var equipmentOperatorHistoryManager = Resolve<EquipmentOperatorHistoryManager>();
            var equipments = await GetListByIdsAsync(equipmentIds);
            foreach (var equipment in equipments)
            {
                var history = new EquipmentOperatorHistory()
                {
                    OperatorId = equipment.OperatorId.Value,
                    EquipmentId = equipment.Id,
                    EquipmentTransitionType = EquipmentTransitionType.Out
                };
                await equipmentOperatorHistoryManager.InsertAsync(history);
                equipment.OperatorId = null;
            }
        }

        /// <summary>
        /// 绑定用户至设备
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task BindUser(int[] equipmentIds,long userId)
        {
            var equipmentOperatorHistoryManager = Resolve<EquipmentOperatorHistoryManager>();
            var equipments = await GetListByIdsAsync(equipmentIds);
            foreach(var equipment in equipments)
            {
                //如果设备绑定人员就是当前人员，不做处理
                if (equipment.OperatorId != null && equipment.OperatorId.Value == userId)
                {
                    continue;
                }
                //如果设备有绑定人员
                if (equipment.OperatorId != null)
                {
                    //产生原有绑定人员的交接出记录
                    var history = new EquipmentOperatorHistory()
                    {
                        OperatorId = equipment.OperatorId.Value,
                        EquipmentId = equipment.Id,
                        EquipmentTransitionType = EquipmentTransitionType.Out
                    };
                    await equipmentOperatorHistoryManager.InsertAsync(history);
                }
                //如果设备没有绑定人员或者设备绑定人员与当前人员不同
                if (equipment.OperatorId == null || equipment.OperatorId != userId)
                {
                    //产生交接记录
                    var history = new EquipmentOperatorHistory()
                    {
                        OperatorId = userId,
                        EquipmentId = equipment.Id,
                        EquipmentTransitionType = EquipmentTransitionType.In
                    };
                    await equipmentOperatorHistoryManager.InsertAsync(history);
                }

                equipment.OperatorId = userId;
                //设置当前用户的绑定设备
                //var user = await UserManager.GetByIdAsync(userId);
                //user.SetPropertyValue("EquipmentId", equipmentId);
            }
            
        }

        /// <summary>
        /// 获取操作工当前操作的设备列表
        /// </summary>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public virtual async Task<List<Equipment>> GetByOperatorId(long operatorId)
        {
            return await Repository.GetAll().Where(o => o.OperatorId == operatorId)
                .ToListAsync();
        }
        #endregion

        #region 表单相关

        /// <summary>
        /// 重写设备添加
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <param name="Datas"></param>
        /// <returns></returns>
        public async override Task<object> DoAdd(ModuleInfo moduleInfo, IDictionary<string, string> Datas)
        {
            var equipment=await base.DoAdd(moduleInfo, Datas) as Equipment;

            await ValidateEntity(equipment);

            if (string.IsNullOrEmpty(Datas["ProcessType"]))
            {
                throw new UserFriendlyException(L("请选择设备对应的工序"));
            }
            //附加操作设备工序信息
            var processTypeIds = Datas["ProcessType"].Split(',').Select(o=>int.Parse(o));
            foreach(var processTypeId in processTypeIds)
            {
                await EquipmentProcessTypeRepository.InsertAsync(new EquipmentProcessType()
                {
                    EquipmentId=equipment.Id,
                    ProcessTypeId=processTypeId
                });
            }
            return equipment;
        }

        public async override Task<object> DoEdit(ModuleInfo moduleInfo, IDictionary<string, string> Datas, object id)
        {
            var equipment=await base.DoEdit(moduleInfo, Datas, id) as Equipment;

            await ValidateEntity(equipment);
            //附加操作设备工序信息
            await EquipmentProcessTypeRepository.DeleteAsync(o => o.EquipmentId == equipment.Id);

            var processTypeIds = Datas["ProcessType"].Split(',').Select(o => int.Parse(o));
            foreach (var processTypeId in processTypeIds)
            {
                await EquipmentProcessTypeRepository.InsertAsync(new EquipmentProcessType()
                {
                    EquipmentId = equipment.Id,
                    ProcessTypeId = processTypeId
                });
            }
            return equipment;
        }

        public override async Task FillEntityDataAfter(IDictionary<string, object> data, ModuleInfo moduleInfo,object entity)
        {
            //添加工序信息
            var equipmentId = Convert.ToInt32(data["Id"]);
            var processTypes = (await EquipmentProcessTypeRepository.GetAllIncluding(o => o.ProcessType).Where(o => o.EquipmentId == equipmentId).Select(o => o.ProcessType).ToListAsync())
                .Select(o=>new { id=o.Id,processTypeName=o.ProcessTypeName});

            data["ProcessType"] = string.Join(',', processTypes.Select(o => o.id));
            data["ProcessType_data"] = processTypes;
            data["ProcessType_display"] = string.Join(',', processTypes.Select(o => o.processTypeName));

            await base.FillEntityDataAfter(data, moduleInfo,entity);
        }

        #endregion

        #region 设备相关任务
        /// <summary>
        /// 获取设备的加工中任务
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public virtual async Task<ProcessTask> GetProcessingTask(int equipmentId)
        {
            var task = await Resolve<ProcessTaskManager>().GetAll()
                .Include(o => o.Part).ThenInclude(o => o.Project)
                .Where(o => o.EquipmentId == equipmentId && o.ProcessTaskStatus == ProcessTaskStatus.Processing)
                .OrderBy(o => o.ArrangeDate)
                .FirstOrDefaultAsync();

            return task;
        }

        /// <summary>
        /// 获取设备上未完成的任务
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public virtual IQueryable<ProcessTask> GetUnFinishedTasks(int equipmentId, int? taskNumber = null)
        {
            var tasks = Resolve<ProcessTaskManager>().GetAll()
                .Include("Part.Project")
                .Where(o => o.EquipmentId == equipmentId && o.Progress < 1)
                .OrderBy(o => o.ArrangeDate);

            if (taskNumber.HasValue)
            {
                return tasks.Take(taskNumber.Value);
            }
            else
            {
                return tasks;
            }

            //return tasks.OrderBy(o => MESDbContext.GetJsonValueNumber(o.Property, "$.OrderInEquipment"));
        }
        /// <summary>
        /// 获取设备上已完成的任务
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="taskNumber"></param>
        /// <returns></returns>
        public virtual IQueryable<ProcessTask> GetFinishedTasks(int equipmentId, int? taskNumber = null)
        {
            var tasks = Resolve<ProcessTaskManager>().GetAll()
                .Include("Part.Project")
                .Where(o => o.EquipmentId == equipmentId && o.Progress == 1)
                .OrderByDescending(o => o.EndDate);

            if (taskNumber.HasValue)
            {
                return tasks.Take(taskNumber.Value);
            }

            return tasks;
        }
        
        #endregion


        /// <summary>
        /// 获取设备负荷
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public virtual async Task<EquipmentLoadInfo> GetLoadInfo(int equipmentId,DateTime startDate,DateTime endDate)
        {
            var equipment = await GetByIdFromCacheAsync(equipmentId);
            var equipmentDayCapacity = equipment.DayCapacity ?? 24;//设备日产能，默认24

            var tasks = await Resolve<ProcessTaskManager>().GetTimeLineTasksQuery(startDate, endDate)
                .Where(o => o.EquipmentId == equipmentId)
                .ToListAsync();
            //总时间段内的负荷情况 
            var loadInfo = new EquipmentLoadInfo()
            {
                EquipmentId = equipmentId,
                DateStart = startDate,
                DateEnd = endDate,
                TaskIds = tasks.Select(o => o.Id).ToArray(),
                TaskCount = tasks.Count
            };            

            //按每天计算负荷
            var daySpans = Convert.ToInt32(Math.Floor((endDate - startDate).TotalDays));
            for(var i = 0; i < daySpans; i++)
            {
                var dayStart = startDate.AddDays(i);
                var dayEnd = startDate.AddDays(i + 1);
                //一天内的任务
                var dayTasks = tasks.Where(o => o.IsInTimeLine(dayStart, dayEnd));
                var dayLoadInfo = new EquipmentLoadInfo()
                {
                    EquipmentId = equipmentId,
                    DateStart = dayStart,
                    DateEnd = dayEnd,
                    TotalCapacity = equipmentDayCapacity,
                    TaskCount=dayTasks.Count(),
                    TaskIds=dayTasks.Select(o=>o.Id).ToArray(),
                    OccupyHours=dayTasks.Sum(o=>o.CalcTimeLineOccupyTime(dayStart,dayEnd))//计算任务在这一天的占用时长
                };

                loadInfo.DayDetails.Add(dayLoadInfo);
            }
            //汇总
            loadInfo.TotalCapacity = loadInfo.DayDetails.Sum(o => o.TotalCapacity);
            loadInfo.OccupyHours = loadInfo.DayDetails.Sum(o => o.OccupyHours);

            return loadInfo;
        }

       
    }
}
