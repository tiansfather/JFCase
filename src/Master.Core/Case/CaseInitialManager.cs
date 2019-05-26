using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.EntityFramework.Plus;

namespace Master.Case
{
    public class CaseInitialManager : ModuleServiceBase<CaseInitial, int>
    {
        //public virtual void HandleEvent(EntityDeletedEventData<CaseInitial> eventData)
        //{
        //    Resolve<CaseFineManager>().GetAll().Where(o => o.CaseInitialId == eventData.Entity.Id).Delete();
        //    Resolve<CaseCardManager>().GetAll().Where(o => o.CaseInitialId == eventData.Entity.Id).Delete();
        //    Resolve<CaseKeyManager>().GetAll().Where(o => o.CaseInitialId == eventData.Entity.Id).Delete();
        //}
    }
}
