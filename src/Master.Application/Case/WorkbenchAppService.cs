using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Runtime.Security;
using Abp.UI;
using Master.Authentication;
using Master.Configuration;
using Master.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Case
{
    /// <summary>
    /// 工作台及加工接口
    /// </summary>
    [AbpAuthorize]
    public class WorkbenchAppService : MasterAppServiceBase<CaseSource, int>
    {
        #region 分页
        protected override async Task<IQueryable<CaseSource>> GetQueryable(RequestPageDto request)
        {
            var query = await base.GetQueryable(request);
            return query
                .Include(o=>o.AnYou)
                .Include(o=>o.City)
                .Include(o=>o.Court1)
                .Include(o=>o.Court2)
                .Include(o=>o.CaseSourceHistories)
                .Where(o => o.CaseSourceStatus == CaseSourceStatus.待选 && o.OwerId == null);
        }
        protected override async Task<IQueryable<CaseSource>> BuildKeywordQueryAsync(string keyword, IQueryable<CaseSource> query)
        {
            return (await base.BuildKeywordQueryAsync(keyword, query))
                .Where(o=>o.SourceSN.Contains(keyword) 
                || o.AnYou.DisplayName.Contains(keyword)
                || o.City.DisplayName.Contains(keyword)
                || o.Court1.DisplayName.Contains(keyword)
                || o.Court2.DisplayName.Contains(keyword)
                || o.TrialPeopleField.Contains(keyword)
                || o.LawyerFirmsField.Contains(keyword)
                );
        }
        protected override object PageResultConverter(CaseSource entity)
        {
            return new
            {
                entity.Id,
                AnYou = entity.AnYou != null ? entity.AnYou.DisplayName : "",
                City = entity.City != null ? entity.City.DisplayName : "",
                Court1 = entity.Court1 != null ? entity.Court1.DisplayName : "",
                Court2 = entity.Court2 != null ? entity.Court2.DisplayName : "",
                ValidDate=entity.ValidDate.ToString("yyyy/MM/dd"),
                entity.SourceSN,
                entity.LawyerFirms,
                History = entity.CaseSourceHistories.MapTo<List<CaseSourceHistoryDto>>()
            };
        }
        #endregion

        #region 工作台
        /// <summary>
        /// 获取当前用户工作台案例数量
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> GetMyProcessingCount()
        {
            return await Manager.GetAll()
                .Where(o => o.OwerId == AbpSession.UserId)
                .Where(o => o.CaseSourceStatus == CaseSourceStatus.加工中 || o.CaseSourceStatus == CaseSourceStatus.待加工)
                .CountAsync();
        }
        /// <summary>
        /// 获取当前用户所有工作台案例
        /// </summary>
        /// <returns></returns>
        public virtual async Task<object> GetMyProcessingCaseSource()
        {
            var caseSources = await Manager.GetAll()
                .Include(o => o.AnYou)
                .Include(o => o.Court1)
                .Include(o => o.Court2)
                .Where(o => o.OwerId == AbpSession.UserId)
                .Where(o => o.CaseSourceStatus == CaseSourceStatus.加工中 || o.CaseSourceStatus == CaseSourceStatus.待加工)
                .Select(o => new {
                    o.Id,
                    EncrypedId=SimpleStringCipher.Instance.Encrypt(o.Id.ToString(),null,null),
                    AnYou = o.AnYou.DisplayName,
                    Court1 = o.Court1 != null ? o.Court1.DisplayName : "",
                    Court2 = o.Court2 != null ? o.Court2.DisplayName : "",
                    ValidDate = o.ValidDate.ToString("yyyy-MM-dd"),
                    o.CaseSourceStatus,
                    o.SourceSN
                })
                .ToListAsync();

            return caseSources;
        }
        /// <summary>
        /// 选中判例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task Choose(int id)
        {
            var user = await GetCurrentUserAsync();
            if (!user.IsActive)
            {
                throw new UserFriendlyException("您的账号被冻结，目前不能加工案例，请联系管理员");
            }
            var caseSource = await Manager.GetByIdAsync(id);
            if(caseSource==null || caseSource.CaseSourceStatus == CaseSourceStatus.下架)
            {
                throw new UserFriendlyException("十分抱歉！该判例存在问题，正在勘误，请先挑选其他的看看哦！");
            }
            if (caseSource.OwerId != null)
            {
                throw new UserFriendlyException("十分抱歉！该判例已被别人捷足先登了，下次动作要快哦！");
            }
            var processingCount = await GetMyProcessingCount();
            var maxCount = int.Parse(await SettingManager.GetSettingValueAsync(SettingNames.maxWorkbenchCaseCount));
            if (processingCount + 1 > maxCount)
            {
                throw new UserFriendlyException("您的工作台中已经有多个案例待加工，请完成后并发布，然后再添加新的判例。");
            }
            caseSource.OwerId = AbpSession.UserId;
            caseSource.CaseSourceStatus = CaseSourceStatus.待加工;
        }
        /// <summary>
        /// 退还判例
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public virtual async Task GiveBack(int id, string reason)
        {
            var manager = Manager as CaseSourceManager;
            var caseSource = await manager.GetByIdAsync(id);
            //设置案源状态
            caseSource.OwerId = null;
            caseSource.CaseSourceStatus = CaseSourceStatus.待选;
            //清空当前用户针对此案源加工的所有数据
            await manager.ClearCaseContent(id);
            //增加判例记录
            var caseHistory = new CaseSourceHistory()
            {
                CaseSourceId = id,
                Reason = reason
            };
            await Resolve<CaseSourceHistoryManager>().InsertAsync(caseHistory);
        }
        #endregion

        #region 获取案例信息
        /// <summary>
        /// 获取案例信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<object> GetCaseInfo(int id)
        {
            var caseSource = await Manager.GetAll()
                .Include(o => o.AnYou)
                .Include(o => o.City)
                .Include(o => o.Court1)
                .Include(o => o.Court2)
                .Where(o => o.Id == id)
                .SingleOrDefaultAsync();

            var caseInitial = await Resolve<CaseInitialManager>().GetAll()
                .Include(o => o.CaseNodes)
                .Include(o=>o.CaseLabels)
                .Include(o => o.CaseFines)
                .Include(o => o.CaseCards)
                .Where(o => o.CaseSourceId == id && o.CreatorUserId == caseSource.OwerId)
                .FirstOrDefaultAsync();

            CaseInitialDto caseInitialDto = new CaseInitialDto();
            if (caseInitial != null)
            {
                caseInitial.MapTo(caseInitialDto);
            }

            return new
            {
                Source = new
                {
                    caseSource.SourceSN,
                    caseSource.AnYouId,
                    AnYou=caseSource.AnYou?.DisplayName,
                    caseSource.CityId,
                    City=caseSource.City?.DisplayName,
                    caseSource.Court1Id,
                    Court1=caseSource.Court1?.DisplayName,
                    caseSource.Court2Id,
                    Court2 = caseSource.Court2?.DisplayName,
                    caseSource.LawyerFirms,
                    caseSource.TrialPeople,
                    caseSource.SourceFile
                },
                Initial = caseInitialDto
            };
        }
        #endregion


        #region 加工
        /// <summary>
        /// 提交初加工内容
        /// </summary>
        /// <param name="caseInitialUpdateDto"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateInitial(CaseInitialUpdateDto caseInitialUpdateDto)
        {
            var caseInitialManager = Resolve<CaseInitialManager>();
            CaseInitial caseInitial = null;
            if (caseInitialUpdateDto.Id > 0)
            {
                caseInitial = await caseInitialManager.GetAll().Include(o => o.CaseNodes)
                    .Where(o => o.Id == caseInitialUpdateDto.Id).SingleOrDefaultAsync();
                if (caseInitial == null)
                {
                    throw new UserFriendlyException("不存在对应加工信息");
                }
                //删除
                caseInitial.CaseNodes.Clear();
                caseInitial.CaseLabels.Clear();
            }
            else
            {
                caseInitial = new CaseInitial();
            }

            caseInitialUpdateDto.MapTo(caseInitial);
            var caseSource = await Manager.GetByIdAsync(caseInitial.CaseSourceId);
            //设置案源为加工中状态
            caseSource.CaseSourceStatus = CaseSourceStatus.加工中;
            caseInitial.CaseStatus = CaseStatus.加工中;
            await CurrentUnitOfWork.SaveChangesAsync();
            return caseInitial.Id;
        }
        /// <summary>
        /// 发布初加工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task PublishInitial(int id)
        {
            var caseInitial = await Resolve<CaseInitialManager>().GetAll().Include(o=>o.CaseSource)
                .Where(o=>o.Id==id).SingleAsync();
            caseInitial.PublisDate = DateTime.Now;
            caseInitial.CaseStatus = CaseStatus.展示中;
            caseInitial.CaseSource.CaseSourceStatus = CaseSourceStatus.已加工;
        }
        /// <summary>
        /// 提交精加工信息
        /// </summary>
        /// <param name="caseFineDtos"></param>
        /// <returns></returns>
        public virtual async Task UpdateFine(int caseInitialId,IEnumerable<CaseFineDto> caseFineDtos)
        {
            var caseFineManager = Resolve<CaseFineManager>();
            var caseInitial =await Resolve<CaseInitialManager>().GetAll().Include(o => o.CaseFines)
                .Where(o => o.Id == caseInitialId).SingleOrDefaultAsync();
            if (caseInitial == null)
            {
                throw new UserFriendlyException("不存在加工信息");
            }
            //删除
            var delIds = caseInitial.CaseFines.Where(o => !caseFineDtos.Select(c => c.Id).Contains(o.Id)).Select(o => o.Id);
            await caseFineManager.DeleteAsync(delIds);
            //增加
            foreach(var caseFineDto in caseFineDtos.Where(o => o.Id == 0))
            {
                var newCaseFine = caseFineDto.MapTo<CaseFine>();
                newCaseFine.CaseInitialId = caseInitial.Id;
                await caseFineManager.InsertAsync(newCaseFine);
            }
            //修改
            foreach (var caseFineDto in caseFineDtos.Where(o => o.Id > 0))
            {
                var oriCaseFine = await caseFineManager.GetByIdAsync(caseFineDto.Id);
                caseFineDto.MapTo(oriCaseFine);
                oriCaseFine.UserModifyTime = DateTime.Now;
            }
        }
        /// <summary>
        /// 发布精加工
        /// </summary>
        /// <param name="caseInitialId"></param>
        /// <returns></returns>
        public virtual async Task PublishFine(int caseInitialId)
        {
            var caseFines = await Resolve<CaseFineManager>().GetAll().Where(o => o.CaseInitialId == caseInitialId).ToListAsync();
            foreach(var caseFine in caseFines)
            {
                caseFine.IsActive = true;
            }
        }
        /// <summary>
        /// 提交案例卡信息
        /// </summary>
        /// <param name="caseFineDtos"></param>
        /// <returns></returns>
        public virtual async Task UpdateCard(int caseInitialId, IEnumerable<CaseCardDto> caseCardDtos)
        {
            var caseCardManager = Resolve<CaseCardManager>();
            var caseInitial = await Resolve<CaseInitialManager>().GetAll().Include(o => o.CaseCards)
                .Where(o => o.Id == caseInitialId).SingleOrDefaultAsync();
            if (caseInitial == null)
            {
                throw new UserFriendlyException("不存在加工信息");
            }
            //删除
            var delIds = caseInitial.CaseCards.Where(o => !caseCardDtos.Select(c => c.Id).Contains(o.Id)).Select(o => o.Id);
            await caseCardManager.DeleteAsync(delIds);
            //增加
            foreach (var caseCardDto in caseCardDtos.Where(o => o.Id == 0))
            {
                var newCaseCard = caseCardDto.MapTo<CaseCard>();
                newCaseCard.CaseInitialId = caseInitial.Id;
                await caseCardManager.InsertAsync(newCaseCard);
            }
            //修改
            foreach (var caseCardDto in caseCardDtos.Where(o => o.Id > 0))
            {
                var oriCaseCard = await caseCardManager.GetByIdAsync(caseCardDto.Id);
                caseCardDto.MapTo(oriCaseCard);
            }
        }
        /// <summary>
        /// 发布案例卡
        /// </summary>
        /// <param name="caseInitialId"></param>
        /// <returns></returns>
        public virtual async Task PublishCard(int caseInitialId)
        {
            var caseCards = await Resolve<CaseCardManager>().GetAll().Where(o => o.CaseInitialId == caseInitialId).ToListAsync();
            foreach (var caseCard in caseCards)
            {
                caseCard.IsActive = true;
            }
        }
        #endregion
    }
}
