using Abp.Application.Services;
using Master.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Logs
{
    public interface ISystemLogAppService : IApplicationService
    {
        Task<ResultPageDto> GetLogs(string type);
        Task<string> GetTipContent(string url, string type);
        Task<List<dynamic>> AddClass(string str, string type);
    }
}
