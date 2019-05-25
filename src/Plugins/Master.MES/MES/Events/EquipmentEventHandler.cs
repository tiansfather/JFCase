using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.UI;
using Master.Configuration;
using Master.Entity;
using Master.MES.Domains;
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
    public class EquipmentEventHandler : IEventHandler<EntityChangedEventData<Equipment>>, ITransientDependency
    {
        public EquipmentManager EquipmentManager { get; set; }
         

        public void HandleEvent(EntityChangedEventData<Equipment> eventData)
        {
            //var entity = eventData.Entity;
            //var isMESSupplier =  EquipmentManager.FeatureChecker.IsEnabledAsync(MESFeatureNames.MESSupplier).GetAwaiter().GetResult();
            //if (isMESSupplier)
            //{

            //    var tenent = EquipmentManager.GetCurrentTenantAsync().GetAwaiter().GetResult();
            //    var processorInfo = tenent.GetPropertyValue<ProcessorInfo>("ProcessorInfo");
            //    var processTypes = processorInfo.ProcessTypes;
            //    foreach (var eqTypes in entity.EquipmentProcessTypes)
            //    {
            //        var typeName = eqTypes.ProcessType.ProcessTypeName;
            //        var falg = processTypes.Where(o => o == typeName).FirstOrDefault();
            //        if (falg != null) { }
            //        else
            //        {
            //            processTypes.Add(typeName);
            //        }
            //        tenent.SetPropertyValue("ProcessorInfo", processorInfo);
            //    }
            //}
            //else
            //{
            //    throw new UserFriendlyException("你不是加工点");
            //}
        }
    }
}
