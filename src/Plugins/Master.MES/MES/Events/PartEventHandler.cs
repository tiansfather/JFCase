using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.UI;
using Castle.Core.Logging;
using Master.Domain;
using Master.Scheduler.Domains;
using Master.Storage.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Master.MES.Events
{
    /// <summary>
    /// 零件事件
    /// </summary>
    public class PartEventHandler : 
        IEventHandler<EntityDeletingEventData<Part>>,
        IEventHandler<EntityDeletedEventData<Part>>,
        IEventHandler<EntityCreatedEventData<Part>>,
        IEventHandler<EntityUpdatedEventData<Part>>, 
        IEventHandler<EntityChangingEventData<Part>>,
        ITransientDependency
    {
        public ProcessTaskManager ProcessTaskManager { get; set; }
        public MaterialManager MaterialManager { get; set; }
        public MaterialRequireManager MaterialRequireManager { get; set; }
        public MESProjectManager MESProjectManager { get; set; }
        public ILogger Logger { get; set; }
        public IDynamicQuery Query { get; set; }
        /// <summary>
        /// 零件删除后删除对应的任务
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<Part> eventData)
        { 
            ProcessTaskManager.Repository.Delete(o => o.PartId == eventData.Entity.Id);
        }
        [UnitOfWork]
        public virtual void HandleEvent(EntityCreatedEventData<Part> eventData)
        {
            try
            {
                //创建零件后进行进度表的同步
                if (eventData.Entity.EnableProcess)
                {
                    MESProjectManager.SyncProcessSchedule(eventData.Entity.ProjectId).GetAwaiter().GetResult();
                }
            }
            catch(Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }
           

        }
     
        /// <summary>
        /// 进入流程的bom物料 不能修改
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public void HandleEvent(EntityChangingEventData<Part> eventData)
        {
            var entity = eventData.Entity;
          
            var materialRequire = MaterialRequireManager.GetAll().Where(o => o.RequireSource == "BOM" && o.RequireSourceId == entity.Id && o.ProjectId == entity.ProjectId).FirstOrDefault();
            if (materialRequire != null)
            {
                var oldentity = Query.FirstOrDefault<Part>("select * from Part where id=@0", entity.Id);
                if (oldentity == null)
                {
                    return;
                    //throw new UserFriendlyException("不存在记录[Part] Id=" + entity.Id);
                }
                var isdo = "";
                isdo += materialRequire.IsBuyed ? "已采购," : "";
                isdo += materialRequire.IsReceived ? "已入库," : "";
                isdo += materialRequire.IsUsed ? "已领用," : "";
             //   isdo += materialRequire.IsQuoted ? "已询价," : "";
                if (!string.IsNullOrEmpty(isdo))
                {
                    throw new UserFriendlyException("该BOM基础物资已进入流程，无法修改：" + isdo);
                }
                //验证通过的情况下判断物资内容是否改变
                if (oldentity.MaterialCode != entity.MaterialCode || oldentity.PartNum!=entity.PartNum || oldentity.MeasureMentUnit != entity.MeasureMentUnit || oldentity.RequireDate != entity.RequireDate) {
                    //物资编码   数量 单位改变  改变需求表的内容
                    var msg = "";
                    if (oldentity.MaterialCode != entity.MaterialCode)
                    {
                        var codematerial = MaterialManager.GetAll().Where(o => o.Code == entity.MaterialCode).FirstOrDefault();
                        materialRequire.CustomName = codematerial.Name;
                        materialRequire.CustomSpecification =   string.IsNullOrEmpty(codematerial.Specification) ? entity.PartSpecification : codematerial.Specification;
                        materialRequire.CustomBrand = codematerial.Brand;
                        msg += "物料编码改变[" + oldentity.MaterialCode+"-->"+ entity.MaterialCode+"] ,";
                    }
                    if (oldentity.PartNum != entity.PartNum)
                    {
                        materialRequire.Number = entity.PartNum;
                        materialRequire.ToOutNumber = entity.PartNum;
                        msg += "数量改变[" + oldentity.PartNum + "-->" + entity.PartNum + "] ,";
                    }
                    if (oldentity.MeasureMentUnit != entity.MeasureMentUnit)
                    {
                        msg += "单位改变[" + oldentity.MeasureMentUnit + "-->" + entity.MeasureMentUnit + "] ,";
                    }
                    if (oldentity.RequireDate != entity.RequireDate)
                    {
                        materialRequire.RequireDate = entity.RequireDate;
                        msg += "物料应到料时间改变[" + oldentity.RequireDate + "-->" + entity.RequireDate + "] ,";
                    }  
                     
                    MaterialRequireManager.AddOperationHistory(materialRequire.Id, "BOM修改:"+msg).GetAwaiter().GetResult();

                }

            }
            else {
                if (entity.EnableBuy || entity.EnableStorage) {
                    SetMaterialRequire(entity);
                }

            }


        }
        /// <summary>
        /// 添加 采购/出库  需求
        /// </summary>
        /// <param name="entity"></param>
        protected void SetMaterialRequire(Part entity) {
            var materialCode = entity.MaterialCode;
            var material = MaterialManager.GetAll().Where(o => o.Code == materialCode).Include(o=>o.MeasureMent.MeasureMentUnits).FirstOrDefault();
            if (material == null) {
                return;
            }
            var measureMentUnit = material.MeasureMent.MeasureMentUnits.Where(o => o.Name == entity.MeasureMentUnit).FirstOrDefault();

            var materialRequire = new MaterialRequire();
            materialRequire.RequireSource = "BOM";
            materialRequire.RequireSourceId = entity.Id;
             materialRequire.MaterialId = material.Id ;
            materialRequire.CustomSpecification = string.IsNullOrEmpty(material.Specification)? entity.PartSpecification: material.Specification;
            materialRequire.CustomBrand = material.Brand;

            materialRequire.Number = entity.PartNum;
            materialRequire.MeasureMentUnitId = measureMentUnit.Id ;
            materialRequire.RequireDate = entity.RequireDate;
            materialRequire.ProjectId = entity.ProjectId;

            var codematerial = MaterialManager.GetAll().Where(o => o.Code == entity.MaterialCode).FirstOrDefault();
            materialRequire.CustomName = codematerial.Name;


            if (entity.EnableBuy)
            {
                materialRequire.RequireType = RequireType.Buy;
                MaterialRequireManager.InsertAsync(materialRequire).GetAwaiter().GetResult();
            }
            else if (entity.EnableStorage)
            {
                materialRequire.RequireType = RequireType.Storage;
                materialRequire.ToOutNumber =  entity.PartNum;
                MaterialRequireManager.InsertAsync(materialRequire).GetAwaiter().GetResult();

            }
        }
        /// <summary>
        /// //修改零件后进行进度表的同步
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public virtual void HandleEvent(EntityUpdatedEventData<Part> eventData)
        {
            
            try
            {
                //修改零件后进行进度表的同步
                if (eventData.Entity.EnableProcess)
                {
                    MESProjectManager.SyncProcessSchedule(eventData.Entity.ProjectId).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }



        }
        /// <summary>
        /// 删除bom时，判断 采购/出库  需求是否已进入流程
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public void HandleEvent(EntityDeletingEventData<Part> eventData)
        {
            var entity = eventData.Entity;
            var materialRequire = MaterialRequireManager.GetAll().Where(o => o.RequireSource == "BOM" && o.RequireSourceId == entity.Id &&  o.ProjectId == entity.ProjectId).FirstOrDefault();
            if(materialRequire != null){

                var isdo = "";
                isdo += materialRequire.IsBuyed?"已采购,":"";
                isdo += materialRequire.IsReceived ? "已入库,":"";
                isdo += materialRequire.IsUsed ? "已领用,":"";
               // isdo += materialRequire.IsQuoted ? "已询价,":"";
                if (string.IsNullOrEmpty(isdo)) {
                    throw new UserFriendlyException("该BOM基础物资已进入流程，无法删除："+isdo);
                }
            }
            

        }

    }
}
