using Abp.Domain.Repositories;
using Master.Domain;
using Master.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Caching;
using Abp.Domain.Entities;
using Master.Configuration;

namespace Master.MES
{
    /// <summary>
    /// 策略管理
    /// </summary>
    public class TacticManager : DomainServiceBase<Tactic, int>
    {
        //public ProcessTaskReportManager ProcessTaskReportManager { get; set; }
        public IRepository<TacticPerson,int> TacticPersonRepository { get; set; }
        //public PersonManager PersonManager { get; set; }
       
        /// <summary>
        /// 缓存方式获取所有有效策略
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<Tactic>> GetAllActiveTactics()
        {
            var tenantId = CurrentUnitOfWork.GetTenantId() ?? 0;
            return await CacheManager.GetCache<int, List<Tactic>>("Tactics").GetAsync(tenantId, async o => {
                return await GetAll().Where(t => t.IsActive && t.TenantId==CurrentUnitOfWork.GetTenantId()).ToListAsync();
            });
        }
        /// <summary>
        /// 将人员加入到策略的提醒人中
        /// </summary>
        /// <param name="tactic"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        public virtual async Task AddReminderToTactic(Tactic tactic,Person person)
        {
            var existReminders = await GetTacticReminders(tactic.Id);
            if (!existReminders.Contains(person))
            {
                var tacticPerson = new TacticPerson()
                {
                    TacticId=tactic.Id,
                    PersonId=person.Id
                };
                await TacticPersonRepository.InsertAsync(tacticPerson);
            }
        }
        #region 获取提醒人
        /// <summary>
        /// 获取策略的所有直接提醒人
        /// </summary>
        /// <param name="tactic"></param>
        /// <returns></returns>
        public virtual async Task<List<Person>> GetTacticReminders(int tacticId)
        {
            return await TacticPersonRepository.GetAll().Include(o => o.Person).Where(o => o.TacticId == tacticId)
                .Select(o => o.Person).ToListAsync();
        }
        /// <summary>
        /// 获取加工任务的动态提醒人
        /// </summary>
        /// <param name="tactic"></param>
        /// <param name="processTask"></param>
        /// <returns></returns>
        public virtual async Task<List<Person>> GetTacticDynamicReminders(Tactic tactic, ProcessTask processTask)
        {
            var personManager = Resolve<PersonManager>();
            var remindPersons = new List<Person>();
            var remindInfo = tactic.RemindTacticInfo;
            if (remindInfo.DynamicRemindPoster && !string.IsNullOrEmpty(processTask.Poster))
            {
                remindPersons.Add(await personManager.FindByName(processTask.Poster));
            }
            if (remindInfo.DynamicRemindProjectCharger && !string.IsNullOrEmpty(processTask.Part.Project.GetPropertyValue<string>("ProjectCharger")))
            {
                remindPersons.Add(await personManager.FindByName(processTask.Part.Project.GetPropertyValue<string>("ProjectCharger")));
            }
            if (remindInfo.DynamicRemindProjectTracker && !string.IsNullOrEmpty(processTask.Part.Project.GetPropertyValue<string>("ProjectTracker")))
            {
                remindPersons.Add(await personManager.FindByName(processTask.Part.Project.GetPropertyValue<string>("ProjectTracker")));
            }
            if (remindInfo.DynamicRemindCraftsMan && !string.IsNullOrEmpty(processTask.CraftsMan))
            {
                remindPersons.Add(await personManager.FindByName(processTask.CraftsMan));
            }
            if (remindInfo.DynamicRemindVerifier && !string.IsNullOrEmpty(processTask.Verifier))
            {
                remindPersons.Add(await personManager.FindByName(processTask.Verifier));
            }
            if (remindInfo.DynamicRemindChecker && !string.IsNullOrEmpty(processTask.Checker))
            {
                remindPersons.Add(await personManager.FindByName(processTask.Checker));
            }
            if (remindInfo.DynamicRemindCustomer && processTask.Part?.Project?.Unit!=null)
            {
                try
                {
                    //提醒客户
                    var openids = await Resolve<MESUnitManager>().FindUnitOpenId(processTask.Part.Project.Unit, MESStatusNames.ReceiveOuterTask);
                    foreach (var openid in openids)
                    {
                        var person = await personManager.GetPersonByWeUserOrInsert(new Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthUserInfo() { openid = openid });
                        remindPersons.Add(person);
                    }
                }
                catch(Exception ex)
                {

                }
                
                //var customerTenantId = processTask.Part.Project.Unit?.GetPropertyValue<int?>("TenantId");
                //if(remindPersonIds!=null && remindPersonIds.Count > 0)
                //{
                //    remindPersons.AddRange(await personManager.GetListByIdsAsync(remindPersonIds));
                //}
            }
            return remindPersons.Where(o=>o!=null).Distinct().ToList();
        }
        /// <summary>
        /// 获取报工的所有接收提醒人
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public virtual async Task<List<PersonTacticInfo>> GetRemindPersonsByReport(int reportId)
        {
            var remindPersons = new List<PersonTacticInfo>();//待提醒人
            var report = await Resolve<ProcessTaskReportManager>().GetAll().IgnoreQueryFilters()
                .Include(o => o.ProcessTask).ThenInclude(o => o.Part).ThenInclude(o => o.Project).ThenInclude(o=>o.Unit)
                .Where(o => o.Id == reportId).SingleOrDefaultAsync();
            if (report == null) { return new List<PersonTacticInfo>(); }
            using (CurrentUnitOfWork.SetTenantId(report.TenantId))
            {
                var tactics = await GetActiveTacticsByReportType(report.ReportType, report.TenantId, report.ProcessTask?.Part?.Project.ProjectSN);

                foreach (var tactic in tactics)
                {
                    remindPersons.AddRange((await GetTacticReminders(tactic.Id)).Select(o=>new PersonTacticInfo() {
                        Person=o,
                        Tactics=new List<Tactic>() { tactic}
                    }));
                    //添加动态提醒人,开单人,模具组长,工艺师,审核,检验
                    remindPersons.AddRange((await GetTacticDynamicReminders(tactic, report.ProcessTask)).Select(o => new PersonTacticInfo()
                    {
                        Person = o,
                        Tactics = new List<Tactic>() { tactic }
                    }));

                    //var remindInfo = tactic.RemindTacticInfo;
                    //if (remindInfo.DynamicRemindPoster && !string.IsNullOrEmpty(report.ProcessTask.Poster))
                    //{
                    //    remindPersons.Add(await PersonManager.FindByName(report.ProcessTask.Poster));
                    //}
                    //if (remindInfo.DynamicRemindProjectCharger && !string.IsNullOrEmpty(report.ProcessTask.Part.Project.GetPropertyValue<string>("ProjectCharger")))
                    //{
                    //    remindPersons.Add(await PersonManager.FindByName(report.ProcessTask.Part.Project.GetPropertyValue<string>("ProjectCharger")));
                    //}
                    //if (remindInfo.DynamicRemindProjectTracker && !string.IsNullOrEmpty(report.ProcessTask.Part.Project.GetPropertyValue<string>("ProjectTracker")))
                    //{
                    //    remindPersons.Add(await PersonManager.FindByName(report.ProcessTask.Part.Project.GetPropertyValue<string>("ProjectTracker")));
                    //}
                    //if (remindInfo.DynamicRemindCraftsMan && !string.IsNullOrEmpty(report.ProcessTask.CraftsMan))
                    //{
                    //    remindPersons.Add(await PersonManager.FindByName(report.ProcessTask.CraftsMan));
                    //}
                    //if (remindInfo.DynamicRemindVerifier && !string.IsNullOrEmpty(report.ProcessTask.Verifier))
                    //{
                    //    remindPersons.Add(await PersonManager.FindByName(report.ProcessTask.Verifier));
                    //}
                    //if (remindInfo.DynamicRemindChecker && !string.IsNullOrEmpty(report.ProcessTask.Checker))
                    //{
                    //    remindPersons.Add(await PersonManager.FindByName(report.ProcessTask.Checker));
                    //}
                }

                return MergePersonTacticInfo(remindPersons);

                //remindPersons = remindPersons.Distinct().Where(o => o != null).ToList();

                //return remindPersons;
            }

        }
        /// <summary>
        /// 获取延期任务的提醒人
        /// </summary>
        /// <param name="processTask"></param>
        /// <param name="delayType"></param>
        /// <returns></returns>
        public virtual async Task<List<PersonTacticInfo>> GetRemindPersonByDelayTask(ProcessTask processTask,DelayType delayType)
        {
            var remindPersons = new List<PersonTacticInfo>();//待提醒人
            using (CurrentUnitOfWork.SetTenantId(processTask.TenantId))
            {
                var tactics = await GetActiveTacticsByDelayProcessTask(processTask, delayType);
                foreach(var tactic in tactics)
                {
                    remindPersons.AddRange((await GetTacticReminders(tactic.Id)).Select(o => new PersonTacticInfo()
                    {
                        Person = o,
                        Tactics = new List<Tactic>() { tactic }
                    }));
                    //添加动态提醒人,客户,开单人,模具组长,工艺师,审核,检验
                    remindPersons.AddRange((await GetTacticDynamicReminders(tactic, processTask)).Select(o => new PersonTacticInfo()
                    {
                        Person = o,
                        Tactics = new List<Tactic>() { tactic }
                    }));
                }                
            }

            return MergePersonTacticInfo(remindPersons);

            //remindPersons = remindPersons.Distinct().Where(o => o != null).ToList();
            //return remindPersons;
        }
        /// <summary>
        /// 对被提醒人进行合并
        /// </summary>
        /// <param name="personTacticInfos"></param>
        /// <returns></returns>
        private List<PersonTacticInfo> MergePersonTacticInfo(List<PersonTacticInfo> personTacticInfos)
        {
            var result = new List<PersonTacticInfo>();
            foreach(var personTacticInfo in personTacticInfos)
            {
                var info = result.Where(o => o.Person == personTacticInfo.Person).FirstOrDefault();
                if (info==null)
                {
                    result.Add(personTacticInfo);
                }
                else
                {
                    info.Tactics.AddRange(personTacticInfo.Tactics);
                }
            }
            return result;
        }
        #endregion

        #region 获取提醒策略
        /// <summary>
        /// 通过报工类型获取对应的提醒策略
        /// </summary>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public virtual async Task<List<Tactic>> GetActiveTacticsByReportType(ReportType reportType, int tenantId, string projectSN)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                //使用缓存优化性能
                var allTactics = await GetAllActiveTactics();
                //限制模具编号
                if (!string.IsNullOrEmpty(projectSN))
                {
                    allTactics = allTactics.Where(o => o.RemindTacticInfo == null || o.RemindTacticInfo.RemindProjectSNs.Count == 0 || o.RemindTacticInfo.RemindProjectSNs.Contains(projectSN)).ToList();
                }
                switch (reportType)
                {
                    case ReportType.到料:
                        allTactics = allTactics.Where(o => o.RemindTacticInfo.EnableReceiveRemind).ToList();
                        break;
                    case ReportType.上机:
                        allTactics = allTactics.Where(o => o.RemindTacticInfo.EnableStartRemind).ToList();
                        break;
                    case ReportType.下机:
                        allTactics = allTactics.Where(o => o.RemindTacticInfo.EnableEndRemind).ToList();
                        break;
                    case ReportType.加工:
                        allTactics = allTactics.Where(o => o.RemindTacticInfo.EnableProcessingRemind).ToList();
                        break;
                    case ReportType.暂停:
                        allTactics = allTactics.Where(o => o.RemindTacticInfo.EnableSuspendRemind).ToList();
                        break;
                }

                return allTactics;
            }
        }
        /// <summary>
        /// 通过加工任务获取延期提醒策略
        /// </summary>
        /// <param name="processTask"></param>
        /// <param name="delayType"></param>
        /// <returns></returns>
        public virtual async Task<List<Tactic>> GetActiveTacticsByDelayProcessTask(ProcessTask processTask, DelayType delayType)
        {
            using (CurrentUnitOfWork.SetTenantId(processTask.TenantId))
            {
                var projectSN = processTask.Part.Project.ProjectSN;
                //使用缓存优化性能
                var allTactics = await GetAllActiveTactics();
                //限制模具编号
                if (!string.IsNullOrEmpty(projectSN))
                {
                    allTactics = allTactics.Where(o => o.RemindTacticInfo == null || o.RemindTacticInfo.RemindProjectSNs.Count == 0 || o.RemindTacticInfo.RemindProjectSNs.Contains(projectSN)).ToList();
                }
                switch (delayType)
                {
                    //延期上机
                    case DelayType.DelayStart:
                        allTactics = allTactics.Where(o => o.RemindTacticInfo.EnableStartDelayRemind && processTask.AppointDate != null && processTask.StartDate == null && (DateTime.Now - processTask.AppointDate.Value).TotalDays >= o.RemindTacticInfo.StartDelayOffsetDay).ToList();
                        break;
                    //延期下机
                    case DelayType.DelayEnd:
                        allTactics = allTactics.Where(o => o.RemindTacticInfo.EnableEndDelayRemind && processTask.RequireDate != null && processTask.EndDate == null && (DateTime.Now - processTask.RequireDate.Value).TotalDays >= o.RemindTacticInfo.EndDelayOffsetDay).ToList();
                        break;
                    //到料未上机
                    case DelayType.ReceiveNotStart:
                        allTactics = allTactics.Where(o => o.RemindTacticInfo.EnableReceiveNotStartRemind && processTask.ReceiveDate != null && processTask.StartDate == null && (DateTime.Now - processTask.ReceiveDate.Value).TotalDays >= o.RemindTacticInfo.ReceiveNotStartOffsetDay).ToList();
                        break;
                    //工时超预计
                    case DelayType.ExceedHour:
                        allTactics = allTactics.Where(o => o.RemindTacticInfo.EnableExceedHourRemind && processTask.StartDate != null && processTask.EndDate == null && processTask.EstimateHours != null && o.RemindTacticInfo.ExceedHourOffsetHour > 0 && (DateTime.Now - processTask.StartDate.Value).TotalHours >= o.RemindTacticInfo.ExceedHourOffsetHour).ToList();
                        break;
                }

                return allTactics;
            }
        }


        #endregion

    }
}
