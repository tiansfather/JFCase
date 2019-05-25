using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Abp.Auditing;
using Abp.Domain.Uow;

namespace Master.Notices
{
    [AbpAuthorize]
    public class NoticeAppService:MasterAppServiceBase<Notice,int>
    {
        /// <summary>
        /// 添加公告
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public virtual async Task AddNotice(string title)
        {
            var notice = new Notice()
            {
                NoticeTitle = title
            };
            await Manager.InsertAsync(notice);
        }

        public virtual async Task SetActive(int id,bool isActive)
        {
            var notice = await Manager.GetByIdAsync(id);
            notice.IsActive = isActive;
        }

        public virtual async Task UpdateField(int noticeId, string field, string value)
        {
            var notice = await Manager.GetByIdAsync(noticeId);
            switch (field)
            {
                case "noticeTitle":
                    notice.NoticeTitle = value;
                    break;
            }
            await Manager.UpdateAsync(notice);
        }

        /// <summary>
        /// 获取激活状态的公告
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public virtual async Task<IEnumerable<NoticeDto>> GetActiveNotices()
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                return (await Manager.GetAll().Where(o => o.IsActive && o.TenantId==null)
                .OrderByDescending(o => o.Id)
                .ToListAsync())
                .MapTo<List<NoticeDto>>();
            }
            
        }
    }
}
