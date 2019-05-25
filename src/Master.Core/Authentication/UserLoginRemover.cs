using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Authentication
{
    public class UserLoginRemover : IEventHandler<EntityDeletedEventData<User>>,
        ITransientDependency
    {
        public IRepository<UserLogin,int> UserLoginRepository { get; set; }
        [UnitOfWork]
        public void HandleEvent(EntityDeletedEventData<User> eventData)
        {
            UserLoginRepository.Delete(o => o.UserId == eventData.Entity.Id && o.TenantId == eventData.Entity.TenantId);
        }
    }
}
