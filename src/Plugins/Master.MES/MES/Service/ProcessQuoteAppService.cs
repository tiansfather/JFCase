using Abp.Authorization;
using Abp.AutoMapper;
using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Master.Authentication;
using Master.Authentication.External;
using Master.Domain;
using Master.Dto;
using Master.Entity;
using Master.MES.Dtos;
using Master.MES.Jobs;
using Master.MultiTenancy;
using Master.Units;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES.Service
{
    [AbpAuthorize]
    public class ProcessQuoteAppService:ModuleDataAppServiceBase<ProcessQuote,int>
    {
        public MesTenancyAppService MesTenancyAppService { get; set; }
        public MESTenantManager MESTenantManager { get; set; }
        public MESUnitManager MESUnitManager { get; set; }
        public RemindLogManager RemindLogManager { get; set; }
        public ProcessTaskManager ProcessTaskManager { get; set; }
        public IRepository<ProcessQuoteTask,int> ProcessQuoteTaskRepository { get; set; }
        public IRepository<ProcessQuoteBid, int> ProcessQuoteBidRepository { get; set; }
        public IRepository<User,long> UserRepository { get; set; }
        

        protected override string ModuleKey()
        {
            return nameof(ProcessQuote);
        }


        #region 添加编辑询价单
        /// <summary>
        /// 添加或编辑询价信息
        /// </summary>
        /// <param name="processQuoteSubmitDto"></param>
        /// <param name="isPublish"></param>
        /// <returns></returns>
        public virtual async Task SubmitProcessQuote(ProcessQuoteSubmitDto processQuoteSubmitDto, bool isPublish = false)
        {
            if (string.IsNullOrEmpty(processQuoteSubmitDto.QuoteName))
            {
                throw new UserFriendlyException(L("请输入询价名称"));
            }

            if (processQuoteSubmitDto.ProcessQuoteTasks.Count == 0)
            {
                throw new UserFriendlyException(L("询价明细数量不能为0"));
            }
            if (processQuoteSubmitDto.QuoteScope == QuoteScope.邀请投标 && processQuoteSubmitDto.UnitIds.Count == 0)
            {
                throw new UserFriendlyException(L("邀请投标的请至少选择一个加工点"));
            }


            ProcessQuote processQuote = null;
            var manager = Manager as ProcessQuoteManager;
            if (processQuoteSubmitDto.Id == 0)
            {
                #region 添加
                processQuote = processQuoteSubmitDto.MapTo<ProcessQuote>();
                processQuote.ProcessQuoteBids = new List<ProcessQuoteBid>();
                foreach (var addUnitId in processQuoteSubmitDto.UnitIds)
                {
                    var unit = await MESUnitManager.GetByIdFromCacheAsync(addUnitId);
                    var unitTenantBinded = unit.IsTenantBinded();
                    //发布状态下，往来单位必须绑定账号
                    if (isPublish && !unitTenantBinded)
                    {
                        throw new UserFriendlyException($"{unit.UnitName}尚未绑定模来模往账号");
                    }
                    //产生新的投标明细
                    var quoteBid = new ProcessQuoteBid()
                    {
                        UnitId = addUnitId,
                        ToTenantId = unit.GetTenantId(),
                        QuoteBidStatus = unitTenantBinded ? QuoteBidStatus.未发送 : QuoteBidStatus.未加入
                    };
                    processQuote.ProcessQuoteBids.Add(quoteBid);
                }
                await manager.InsertAsync(processQuote);
                #endregion
            }
            else
            {
                processQuote = await Manager.GetAll()
                    .Include(o => o.ProcessQuoteTasks)
                    .Include(o => o.ProcessQuoteBids)
                    .Where(o => o.Id == processQuoteSubmitDto.Id).SingleAsync();

                if (processQuote.QuoteStatus != QuoteStatus.草稿)
                {
                    throw new UserFriendlyException(L("只有草稿状态的询价单才能修改"));
                }

                processQuote.QuoteName = processQuoteSubmitDto.QuoteName;
                processQuote.QuoteScope = processQuoteSubmitDto.QuoteScope;
                processQuote.QuotePayType = processQuoteSubmitDto.QuotePayType;
                processQuote.ExpireDate = processQuoteSubmitDto.ExpireDate;
                processQuote.Files = processQuoteSubmitDto.Files;

                #region 询价明细的保存
                //新增
                var addedTasks = processQuoteSubmitDto.ProcessQuoteTasks.Where(o => o.Id == 0).MapTo<List<ProcessQuoteTask>>();
                foreach (var addTask in addedTasks)
                {
                    processQuote.ProcessQuoteTasks.Add(addTask);
                }
                //修改
                foreach (var oldTask in processQuote.ProcessQuoteTasks.Where(o => processQuoteSubmitDto.ProcessQuoteTasks.Exists(m => m.Id == o.Id)))
                {
                    var newTask = processQuoteSubmitDto.ProcessQuoteTasks.Single(o => o.Id == oldTask.Id);
                    newTask.MapTo(oldTask);
                }
                //删除
                var deletedTaskIds = processQuote.ProcessQuoteTasks.Where(o => !processQuoteSubmitDto.ProcessQuoteTasks.Exists(m => m.Id == o.Id))
                .Select(o => o.Id);

                await ProcessQuoteTaskRepository.DeleteAsync(o => deletedTaskIds.Contains(o.Id));
                #endregion

                #region 投标明细的保存
                //新增
                var addedUnitIds = processQuoteSubmitDto.UnitIds.Where(o => processQuote.ProcessQuoteBids.Count(b => b.UnitId == o) < 0);
                foreach (var addUnitId in addedUnitIds)
                {
                    var unit = await MESUnitManager.GetByIdFromCacheAsync(addUnitId);
                    var unitTenantBinded = unit.IsTenantBinded();
                    //发布状态下，往来单位必须绑定账号
                    if (isPublish && !unitTenantBinded)
                    {
                        throw new UserFriendlyException($"{unit.UnitName}尚未绑定模来模往账号");
                    }
                    //产生新的投标明细
                    var quoteBid = new ProcessQuoteBid()
                    {
                        UnitId = addUnitId,
                        ToTenantId = unit.GetTenantId(),
                        QuoteBidStatus = unitTenantBinded ? QuoteBidStatus.未发送 : QuoteBidStatus.未加入
                    };
                    processQuote.ProcessQuoteBids.Add(quoteBid);
                }
                //删除
                var deletedBidIds = processQuote.ProcessQuoteBids.Where(o => !processQuoteSubmitDto.UnitIds.Exists(m => m == o.UnitId))
                .Select(o => o.Id);
                await ProcessQuoteBidRepository.DeleteAsync(o => deletedBidIds.Contains(o.Id));
                #endregion

                await Manager.UpdateAsync(processQuote);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
            //设置询价中的所有询价任务为“已询价状态"
            var tasks = await ProcessTaskManager.GetAll()
                .Where(o => processQuote.ProcessQuoteTasks.Where(t => t.ProcessTaskId.HasValue).Select(t => t.ProcessTaskId).Contains(o.Id))
                .ToListAsync();
            foreach(var task in tasks)
            {
                task.SetStatus(ProcessTask.Status_Quoted);
            }
            //发布
            if (isPublish)
            {
                await manager.Publish(processQuote.Id);
            }

        }
        #endregion

        #region 询价详情
        /// <summary>
        /// 获取询价详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ProcessQuoteDto> GetQuoteInfo(int id)
        {
            var processQuote = await Manager.GetAll()
                .Include(o => o.ProcessQuoteTasks)
                .Include("ProcessQuoteBids.Unit").Where(o => o.Id == id).SingleOrDefaultAsync();

            if (processQuote == null)
            {
                throw new UserFriendlyException(L("此询价已被删除"));
            }

            //判断是否流标
            if (processQuote.QuoteStatus==QuoteStatus.询价中 &&  processQuote.ExpireDate<DateTime.Now )
            {
                if(processQuote.ProcessQuoteBids.Count(p => p.BidType != null) == 0)
                {
                    processQuote.QuoteStatus = QuoteStatus.已流标;
                }
                else
                {
                    processQuote.QuoteStatus = QuoteStatus.已截止;
                }
                
            }            
            foreach (var bid in processQuote.ProcessQuoteBids)
            {
                //如果之前是未加入的，现在对应加工点已加入，则调整显示状态
                if (bid.QuoteBidStatus == QuoteBidStatus.未加入)
                {
                    if (bid.Unit.IsTenantBinded())
                    {
                        bid.QuoteBidStatus = QuoteBidStatus.未发送;
                    }
                }
            }

            var processQuoteDto = processQuote.MapTo<ProcessQuoteDto>();
            //如果是已中标的,则读取询价明细任务对应的单号
            if (processQuote.QuoteStatus == QuoteStatus.已选标)
            {
                foreach (var task in processQuoteDto.ProcessQuoteTasks)
                {
                    if (task.ProcessTaskId.HasValue)
                    {
                        var processSN = await ProcessTaskManager.GetAll().Where(o => o.Id == task.ProcessTaskId.Value)
                            .Select(o => o.ProcessSN)
                            .SingleOrDefaultAsync();
                        task.ProcessSN = processSN;
                    }
                }
            }
            return processQuoteDto;
        }
        #endregion

        #region 选中标
        /// <summary>
        /// 选中投标
        /// </summary>
        /// <param name="quoteId"></param>
        /// <param name="bidId"></param>
        /// <returns></returns>
        public virtual async Task ChooseBid(int quoteId, int bidId)
        {
            var manager = Manager as ProcessQuoteManager;
            var quote = await manager.GetAll()
                .Include("ProcessQuoteBids.Unit")
                .Include("ProcessQuoteTasks.ProcessTask")
                .Where(o => o.Id == quoteId)
                .SingleOrDefaultAsync();

            if (quote == null)
            {
                throw new UserFriendlyException(L("数据错误"));
            }
            if (quote.ProcessQuoteBids.Count(o => o.Id == bidId) == 0)
            {
                throw new UserFriendlyException(L("数据错误"));
            }
            if (quote.QuoteStatus != QuoteStatus.询价中)
            {
                throw new UserFriendlyException(L("非询价中状态的询价不能进行选标"));
            }
            //
            await manager.Choose(quote, bidId);

        }
        #endregion

        /// <summary>
        /// 重写删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public override async Task DeleteEntity(IEnumerable<int> ids)
        {
            //当询价单处于有人投标状态时不能删除
            var canDelete = await Manager.GetAll()
                .Where(o => ids.Contains(o.Id))
                .Where(o => o.ProcessQuoteBids.Count(p => p.BidType != null) > 0)
                .CountAsync() == 0;
            if (!canDelete)
            {
                throw new UserFriendlyException(L("已有投标数据,不能删除"));
            }
            await base.DeleteEntity(ids);
        }

    }
}
