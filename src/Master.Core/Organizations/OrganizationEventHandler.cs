using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Master.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.Organizations
{
    public class OrganizationEventHandler : IEventHandler<EntityDeletedEventData<Organization>>,
        ITransientDependency
    {
        private readonly UserManager _userManager;
        public OrganizationEventHandler(UserManager userManager)
        {
            _userManager = userManager;
        }
        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<Organization> eventData)
        {
            //删除组织后设置用户组织为空
            var organizationId = eventData.Entity.Id;
            var users = _userManager.Repository.GetAll().Where(o => o.OrganizationId == organizationId).ToList();
            foreach(var user in users)
            {
                user.OrganizationId = null;
            }
        }
    }
}
