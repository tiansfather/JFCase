using Abp.Authorization;
using Abp.AutoMapper;
using Abp.UI;
using Master.Authentication;
using Master.Configuration;
using Master.Dto;
using Master.Entity;
using Master.MES.Domains;
using Master.MES.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES.Service
{
    /// <summary>
    /// 加工点方的询价相关接口
    /// </summary>
    [AbpAuthorize]
    public class ProcessQuoteProcessorAppService : MasterAppServiceBase<ProcessQuote, int>
    {
        public EquipmentManager EquipmentManager { get; set; }
        public FileManager FileManager { get; set; }
        
        #region 分页数据
        protected async override Task<IQueryable<ProcessQuote>> GetQueryable(RequestPageDto request)
        {
            var query = await base.GetQueryable(request);
            return query
                .IgnoreQueryFilters()
                .Include(o=>o.Tenant)
                .Where(o => !o.IsDeleted)
                .Include(o => o.ProcessQuoteBids);
        }

        protected override async Task<IQueryable<ProcessQuote>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<ProcessQuote> query)
        {
            var newQuery = query;
            if (searchKeys.ContainsKey("bidStatus"))
            {
                var status = (QuoteBidStatus)int.Parse(searchKeys["bidStatus"]);
                newQuery = newQuery.Where(o => o.ProcessQuoteBids.Count(b => b.ToTenantId == AbpSession.TenantId && b.QuoteBidStatus == status) > 0);
            }

            return newQuery;
        }
        protected override object PageResultConverter(ProcessQuote entity)
        {
            return new
            {
                entity.Id,
                entity.QuoteSN,
                entity.QuoteName,
                QuoteScope=entity.QuoteScope.ToString(),
                QuotePayType=entity.QuotePayType.ToString(),
                entity.PublishDate,
                ExpireDate=entity.ExpireDate.ToString("yyyy-MM-dd"),
                QuoteStatus=entity.QuoteStatus.ToString(),
                entity.Tenant.TenancyName,
                Cost=entity.ProcessQuoteBids.Where(o=>o.ToTenantId==AbpSession.TenantId).FirstOrDefault()?.BidData?.Cost
            };
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
                .IgnoreQueryFilters()
                .Where(o=>!o.IsDeleted)
                .Include(o => o.ProcessQuoteTasks)
                .Include(o=>o.Tenant)
                .Include("ProcessQuoteBids.Unit").Where(o => o.Id == id).SingleOrDefaultAsync();

            if (processQuote == null)
            {
                throw new UserFriendlyException(L("此询价已被删除"));
            }
            if (processQuote.QuoteStatus == QuoteStatus.已截止 || processQuote.QuoteStatus==QuoteStatus.已流标 || processQuote.QuoteStatus==QuoteStatus.已选标)
            {
                if (processQuote.ProcessQuoteBids.Count(o => o.ToTenantId == AbpSession.TenantId && o.QuoteBidStatus == QuoteBidStatus.已中标) == 0)
                {
                    throw new UserFriendlyException(L("此询价已结束,只有中标方能够查看信息"));
                }
                    
            }
            
            var processQuoteDto = processQuote.MapTo<ProcessQuoteDto>();

            //移除当前账套之外的所有投标明细
            processQuoteDto.ProcessQuoteBids.RemoveAll(o => o.ToTenantId != AbpSession.TenantId);
            processQuoteDto.TenancyName = processQuote.Tenant.TenancyName;
            return processQuoteDto;
        }

        #endregion

        #region 投标
        /// <summary>
        /// 加工点投标提交
        /// </summary>
        /// <param name="processorBidDto"></param>
        /// <returns></returns>
        public virtual async Task Bid(ProcessorBidDto processorBidDto)
        {
            var processQuote = await Manager.GetAll()
                .IgnoreQueryFilters()
                .Where(o=>!o.IsDeleted)
                .Include(o=>o.ProcessQuoteBids)
                .Where(o=>o.Id== processorBidDto.ProcessQuoteId)
                .SingleOrDefaultAsync();
            if (processQuote.QuoteStatus != QuoteStatus.询价中)
            {
                throw new UserFriendlyException(L("非询价中状态的询价不能进行投标"));
            }
            if (processQuote.ExpireDate < DateTime.Now)
            {
                throw new UserFriendlyException(L("此询价已超过截止日期,不能投标"));
            }

            //todo:需要区分是公开投标还是邀请投标
            if (processQuote.QuoteScope == QuoteScope.邀请投标)
            {
                var bidInfo = processQuote.ProcessQuoteBids.Where(o => o.ToTenantId == AbpSession.TenantId).SingleOrDefault();
                bidInfo.BidDate = DateTime.Now;
                bidInfo.BidType = processorBidDto.BidType;
                bidInfo.Remarks = processorBidDto.Remarks;
                var bidData = bidInfo.BidData;
                bidData.Cost = processorBidDto.Cost;
                bidInfo.BidData = bidData;
                bidInfo.QuoteBidStatus = bidInfo.BidType == 0 ? QuoteBidStatus.已放弃 : QuoteBidStatus.已投标;
            }
            else
            {
                //公开投标的不存在放弃投标
                var bidInfo = new ProcessQuoteBid()
                {
                    ProcessQuoteId = processQuote.Id,
                    ToTenantId = AbpSession.TenantId,
                    BidDate = DateTime.Now,
                    BidType = 1,
                    Remarks = processorBidDto.Remarks,
                    BidData = new BidData() { Cost = processorBidDto.Cost },
                    QuoteBidStatus = QuoteBidStatus.已投标

                };
                processQuote.ProcessQuoteBids.Add(bidInfo);
            }
        }
        #endregion
        #region
        /// <summary>
        /// 加工点 设备添加或修改
        /// </summary>
        /// <param name="Equipment"></param>
        /// <returns></returns>
        public virtual async Task ProcessEquipmentSave(Equipment Equipment)
        {
            await EquipmentManager.SaveAsync(Equipment);
        }
        /// <summary>
        /// 加工点 设备批量添加或修改
        /// </summary>
        /// <param name="Equipmentlist"></param>
        /// <returns></returns>
        public virtual async Task ProcessEquipmentSaves(List<EquipmentDto> Equipmentlist)
        {
            var equipments = await EquipmentManager.GetAll().Include(o => o.EquipmentProcessTypes).ToListAsync();
            foreach (var EquipmentDto in Equipmentlist)
            {
                var Equipment = equipments.Where(o => o.Id == EquipmentDto.Id).SingleOrDefault();
                if(Equipment!=null)
                {
                    EquipmentDto.MapTo(Equipment);
                }
                else
                {
                    Equipment = EquipmentDto.MapTo<Equipment>();
                }
                var imgpath = EquipmentDto.EquipmentPicPath;
                if(!string.IsNullOrEmpty(imgpath))
                {
                    var file = FileManager.GetAll().Where(o => o.FilePath == imgpath).FirstOrDefault();
                    if (file == null)
                    {
                        file = new File
                        {
                            TenantId = AbpSession.TenantId,
                            FileName = Equipment.EquipmentSN,
                            FilePath = imgpath,
                            FileSize = 1
                        };
                        await FileManager.InsertAsync(file);
                    }
                    else
                    {
                        Equipment.EquipmentPic = file.Id;
                    }
                }
                await EquipmentManager.SaveAsync(Equipment);
                //添加设备工艺类型
                var typeids = EquipmentDto.TypeID;
                foreach (var typeid in typeids)
                {
                    var eqType = Equipment.EquipmentProcessTypes.Where(o => o.ProcessTypeId == typeid).FirstOrDefault();
                    if (eqType == null)
                    {
                        eqType = new EquipmentProcessType
                        {
                            TenantId = Convert.ToInt32(AbpSession.TenantId),
                            EquipmentId = Equipment.Id,
                            ProcessTypeId = typeid
                        };
                        Equipment.EquipmentProcessTypes.Add(eqType);
                    }
                }
                //删除设备工艺类型
                foreach (var type in Equipment.EquipmentProcessTypes)
                {
                    int? eqType = typeids.Where(o => o == type.Id).FirstOrDefault();
                    if (eqType == null)
                    {
                        Equipment.EquipmentProcessTypes.Remove(type);
                    }
                }
            }
        }
        /// <summary>
        /// 获取设备工艺类型
        /// </summary>
        /// <returns></returns>
        public List<string> GetProcessTypes()
        {
            return Const.StandardProessTypes;
        }

        #endregion
    }
}
