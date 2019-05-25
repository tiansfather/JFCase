using Abp.Authorization;
using Master.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Service
{
    [AbpAuthorize]
    public class RemindLogAppService : MasterAppServiceBase<RemindLog, int>
    {
        protected override object PageResultConverter(RemindLog entity)
        {
            return new
            {
                entity.Id,
                entity.Success,
                entity.Message,
                entity.Name,
                entity.RemindType,
                CreationTime=entity.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                ErrCode=entity.GetPropertyValue<int>("errCode"),
                ErrMsg=entity.GetPropertyValue<string>("errMsg")
            };
        }
    }
}
