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
    public class UserRoleRemover :
        IEventHandler<EntityDeletedEventData<User>>,
        ITransientDependency
    {
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserRoleRemover(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<UserRole> userRoleRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _userRoleRepository = userRoleRepository;
        }

        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<User> eventData)
        {
            using (_unitOfWorkManager.Current.SetTenantId(eventData.Entity.TenantId))
            {
                _userRoleRepository.Delete(
                    ur => ur.UserId == eventData.Entity.Id
                );
            }
        }
    }
}
