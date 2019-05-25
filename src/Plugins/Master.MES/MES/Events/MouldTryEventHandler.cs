using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Master.MouldTry.Domains;
using Master.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Master.MES.Events
{
    /// <summary>
    /// 项目事件
    /// </summary>
    public class MouldTryEventHandler : IEventHandler<EntityDeletedEventData<Master.MouldTry.Domains.MouldTry>>,
        IEventHandler<EntityCreatedEventData<Master.MouldTry.Domains.MouldTry>>,
        ITransientDependency
    {
        public ProcessTypeManager ProcessTypeManager { get; set; }
        public PartManager PartManager { get; set; }
        public ProjectManager projectManager { get; set; }
        public ProcessTaskManager ProcessTaskManager { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<MouldTry.Domains.MouldTry> eventData)
        {
            //PartManager.Repository.Delete(o => o.ProjectId == eventData.Entity.Id);
            throw new NotImplementedException();
        }
        /// <summary>
        /// 试模=》加工任务
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public virtual void HandleEvent(EntityCreatedEventData<MouldTry.Domains.MouldTry> eventData)
        {
            var entity = eventData.Entity;
            if (entity.ProjectId != null) { }
            var ProjectId= Convert.ToInt32(entity.ProjectId);
           var processType= ProcessTypeManager.GetByNameOrInsert("试模").Result;
            var project = projectManager.GetByIdAsync(ProjectId).Result;
           var part= PartManager.GetAll().Where(o => o.ProjectId == ProjectId && o.PartName == "整副").FirstOrDefault();
            if (part == null) { 
            part = PartManager.GenerateNewPart(project, "整副","",1).Result;
                part.EnableProcess = true;
            }
            //var part = PartManager.GetByNameOrInsert("整副", Convert.ToInt32(entity.ProjectId));
            var processTask = new ProcessTask();
            //var project =  projectManager.GetByProjectSNOrInsert(entity.Project.ProjectSN);
            processTask.ProcessSN = entity.MouldTrySN;
            processTask.PartId = part.Id;
            processTask.SupplierId = entity.UnitId;
            processTask.ProcessTypeId = processType.Id;
            var maxSort = 0;
            try
            {
                maxSort = ProcessTaskManager.GetAll().Where(o => o.PartId == part.Id).Max(o => o.Sort);
            }
            catch
            {

            }
            processTask.Sort = maxSort++;
            processTask.ArrangeDate = entity.ArrangeDate;
            processTask.ArrangeEndDate = entity.ArrangeDate;
            processTask.ProjectCharger = entity.TryPerson;
            processTask.AppointDate = entity.PlanDate;
            processTask.RequireDate= entity.PlanDate;
            processTask.ProcessTaskStatus = ProcessTaskStatus.WaitForProcess;
            //throw new NotImplementedException();
            ProcessTaskManager.SaveAsync(processTask).GetAwaiter().GetResult();

            //CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}
