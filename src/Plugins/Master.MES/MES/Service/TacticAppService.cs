using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using Master.Dto;
using Master.MES.Dtos;
using Master.WeiXin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES.Service
{
    public class TacticAppService : MasterAppServiceBase<Tactic, int>
    {
        public PersonManager PersonManager { get; set; }
        public IRepository<TacticPerson,int> TacticPersonRepository { get; set; }
        /// <summary>
        /// 提醒策略分页返回
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [DontWrapResult]
        [AbpAuthorize]
        public virtual async Task<ResultPageDto> GetRemindTacticPageResult(RequestPageDto request)
        {
            var manager = Manager as TacticManager;
            var query = await GetPageResultQueryable(request);

            query.Queryable = query.Queryable.Where(o => o.TacticType == TacticType.RemindTactic);

            var tactics = await query.Queryable.ToListAsync();
            var data = tactics.Select(o => {
                var tacticPersons = manager.GetTacticReminders(o.Id).Result;
                var tacticInfo = o.RemindTacticInfo;
                return new
                {
                    o.Id,
                    o.IsActive,
                    o.TacticName,
                    tacticInfo.DynamicRemindPoster,
                    tacticInfo.DynamicRemindCraftsMan,
                    tacticInfo.DynamicRemindProjectCharger,
                    tacticInfo.DynamicRemindChecker,
                    tacticInfo.DynamicRemindVerifier,
                    tacticInfo.DynamicRemindCustomer,
                    Persons = tacticPersons.Select(p=>new {p.Id,p.Name })
                };
            });

            var result = new ResultPageDto()
            {
                code = 0,
                count = query.RowCount,
                data = data
            };

            return result;
        }

        /// <summary>
        /// 管理端提醒策略分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [DontWrapResult]
        [AbpAuthorize]
        public virtual async Task<ResultPageDto> GetRemindTacticHostPageResult(RequestPageDto request)
        {
            var manager = Manager as TacticManager;
            var query = await GetPageResultQueryable(request);

            query.Queryable = query.Queryable.Where(o => o.TacticType == TacticType.Host);

            var tactics = await query.Queryable.ToListAsync();
            var data = tactics.Select(o => {
                var tacticPersons = manager.GetTacticReminders(o.Id).Result;
                return new
                {
                    o.Id,
                    o.IsActive,
                    o.TacticName,
                    Persons = tacticPersons.Select(p => new { p.Id, p.Name })
                };
            });

            var result = new ResultPageDto()
            {
                code = 0,
                count = query.RowCount,
                data = data
            };

            return result;
        }

        /// <summary>
        /// 提交提醒策略
        /// </summary>
        /// <param name="remindTacticDto"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public virtual async Task SubmitRemindTactic(RemindTacticDto remindTacticDto)
        {
            Tactic tactic = null;
            RemindTacticInfo remindTacticInfo = remindTacticDto.MapTo<RemindTacticInfo>();

            if (remindTacticDto.Id > 0)
            {
                tactic = await Manager.GetByIdAsync(remindTacticDto.Id);
                tactic.TacticName = remindTacticDto.TacticName;
            }
            else
            {
                tactic = new Tactic()
                {
                    TacticName = remindTacticDto.TacticName,
                    TacticType = TacticType.RemindTactic
                };
            }
            tactic.RemindTacticInfo = remindTacticInfo;

            tactic.TenantId = AbpSession.TenantId;
            await Manager.SaveAsync(tactic);
        }

        /// <summary>
        /// 获取提醒策略信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public virtual async Task<RemindTacticDto> GetRemindTacticInfoById(int id)
        {
            var tactic = await Manager.GetByIdAsync(id);
            var remindTacticInfoDto = tactic.RemindTacticInfo.MapTo<RemindTacticDto>();
            remindTacticInfoDto.Id = id;
            remindTacticInfoDto.TacticName = tactic.TacticName;

            if (remindTacticInfoDto.RemindEndHour == 0)
            {
                remindTacticInfoDto.RemindEndHour = 24;
            }

            return remindTacticInfoDto;
        }

        /// <summary>
        /// 绑定提醒人至策略
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task BindReminder(int id)
        {
            var manager = Manager as TacticManager;
            var weuser = WeiXinHelper.GetWeiXinUserInfo();
            var person = await PersonManager.GetPersonByWeUserOrInsert(weuser);

            var tactic = await manager.Repository.GetAll().IgnoreQueryFilters().Where(o => o.Id == id).SingleOrDefaultAsync();

            await manager.AddReminderToTactic(tactic, person);
        }
        /// <summary>
        /// 绑定往来单位报工提醒
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public virtual async Task BindCustomerReminder(int unitId)
        {
            var unit = await Resolve<MESUnitManager>().GetAll().IgnoreQueryFilters().Where(o=>o.Id==unitId).SingleAsync();
            var weuser = WeiXinHelper.GetWeiXinUserInfo();
            var person = await PersonManager.GetPersonByWeUserOrInsert(weuser);

            var bindedPersonIds = unit.GetData<List<int>>("BindedPersonIds");
            if (bindedPersonIds == null)
            {
                bindedPersonIds = new List<int>();
            }
            if (!bindedPersonIds.Contains(person.Id))
            {
                bindedPersonIds.Add(person.Id);
            }
            unit.SetData("BindedPersonIds", bindedPersonIds);

        }
        /// <summary>
        /// 从策略移除被提醒人
        /// </summary>
        /// <param name="tacticId"></param>
        /// <param name="personId"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public virtual async Task RemoveReminder(int tacticId,int personId)
        {
            await TacticPersonRepository.DeleteAsync(o => o.TacticId == tacticId && o.PersonId == personId);
        }
        /// <summary>
        /// 设置策略的有效性
        /// </summary>
        /// <param name="tacticId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public virtual async Task SetActive(int tacticId,bool isActive)
        {
            var tactic = await Manager.GetByIdAsync(tacticId);
            tactic.IsActive = isActive;
        }
    }
}
