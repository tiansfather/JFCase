using Abp.Dependency;
using Master.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Authentication
{
    public class UserLoginAttemptManager:DomainServiceBase<UserLoginAttempt, int>, ITransientDependency
    {
    }
}
