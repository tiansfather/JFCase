using Abp.Application.Services;
using Master.Menu;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Menu
{
    public interface IMenuAppService : IApplicationService
    {
        Task<List<CustomUserMenuItem>> GetUserMenu(Abp.UserIdentifier userIdentifier);
    }
}
