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
using Master.Authentication;

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
        /// 检测是否有新矿工审核
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public virtual async Task<bool> GetActiveNotices()
        {
            if(!await PermissionChecker.IsGrantedAsync("Menu.Admin.Tenancy.NewMiner"))
            {
                return false;
            }
            var normalCount = await Resolve<NewMinerManager>().GetAll().IgnoreQueryFilters().Where(o => !o.IsDeleted).CountAsync();
            return normalCount > 0;
        }
    }
}
