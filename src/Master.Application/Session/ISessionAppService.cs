using Abp.Application.Services;
using Master.Session.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Session
{
    public interface ISessionAppService : IApplicationService
    {
        Task<LoginInformationDto> GetCurrentLoginInformations();

    }
}
