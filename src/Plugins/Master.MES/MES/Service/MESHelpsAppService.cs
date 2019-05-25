using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Uow;
using Master.Entity;
using Master.MES.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES.Service
{
    [AbpAuthorize]
    public class MESHelpsAppService:MasterAppServiceBase<MESHelps,int>
    {
        protected override async Task<IQueryable<MESHelps>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<MESHelps> query)
        {
            if (searchKeys.ContainsKey("menu"))
            {
                query = query.Where(o => o.MenuName == searchKeys["menu"]);
            }
            return query;
        }
        /// <summary>
        /// 添加帮助
        /// </summary>
        /// <param name="title"></param>
        /// <param name="key"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public virtual async Task AddHelps(string title,string key,string displayName)
        {
            var helps = new MESHelps()
            {
                HelpTitle = title,
                MenuName = key,
                MenuDisplayName=displayName,
                TenantId = AbpSession.TenantId
            };
            await Manager.InsertAsync(helps);
        }
        /// <summary>
        /// 获取帮助文档详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public virtual async Task<HelpsDto> GetHelpInfo(int id)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var help = await Manager.GetByIdFromCacheAsync(id);
                return help.MapTo<HelpsDto>();
            }
            
        }
        /// <summary>
        /// 修改帮助文档
        /// </summary>
        /// <param name="helpsDto"></param>
        /// <returns></returns>
        public virtual async Task EditHelps(HelpsDto helpsDto)
        {
            var help = await Manager.GetByIdAsync(helpsDto.Id);
            helpsDto.MapTo(help);
            
        }
        /// <summary>
        /// 通过菜单名获取帮助信息
        /// </summary>
        /// <param name="menuDisplayName"></param>
        /// <returns></returns>
        public virtual async Task<Dictionary<int,string>> GetHelpTitlesByMenuDisplayName(string menuDisplayName)
        {
            var dic = new Dictionary<int, string>();
            //先获取Host帮助
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var hostDatas=await Manager.GetAll().Where(o => o.MenuDisplayName == menuDisplayName).Select(o => new { o.Id, o.HelpTitle }).ToListAsync();
                foreach(var data in hostDatas)
                {
                    dic.Add(data.Id, data.HelpTitle);
                }
            }
            //如果当前是账套登录，获取账套帮助
            if (AbpSession.TenantId.HasValue)
            {
                var tenantDatas = await Manager.GetAll().Where(o => o.MenuDisplayName == menuDisplayName).Select(o => new { o.Id, o.HelpTitle }).ToListAsync();
                foreach (var data in tenantDatas)
                {
                    dic.Add(data.Id, data.HelpTitle);
                }
            }
            return dic;
        }
    }
}
