using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
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
                .Include("CaseSourceHistories.CreatorUser")
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
            var histories = entity.CaseSourceHistories.ToList()
                .Select(o => new
                {
                    o.Reason,
                    CreationTime=o.CreationTime.ToString("MM-dd HH:mm"),
                    Creator=o.CreatorUser.Name
                });

            return new
            {
                entity.Id,
                AnYou = entity.AnYou != null ? entity.AnYou.DisplayName : "",
                City = entity.City != null ? entity.City.DisplayName : "",
                Court1 = entity.Court1 != null ? entity.Court1.DisplayName : "",
                Court2 = entity.Court2 != null ? entity.Court2.DisplayName : "",
                ValidDate=entity.ValidDate.ToString("yyyy/MM/dd"),
                entity.SourceSN,
                entity.SourceFile,
                entity.LawyerFirms,
                //History = entity.CaseSourceHistories.MapTo<List<CaseSourceHistoryDto>>()
                History=histories
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
                    o.SourceSN,
                    o.SourceFile
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
                //移除未发布的案例卡及精加工
                caseInitialDto.CaseFines.RemoveAll(o => o.CaseStatus != CaseStatus.展示中);
                caseInitialDto.CaseCards.RemoveAll(o => o.CaseStatus != CaseStatus.展示中);
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
                    ValidDate=caseSource.ValidDate.ToString("yyyy-MM-dd"),
                    TrialPeople=caseSource.TrialPeople.Select(o=>new { o.Name, TrialRole=o.TrialRole.ToString()}),
                    caseSource.SourceFile
                },
                Initial = caseInitialDto
            };
        }
        #endregion


        #region 加工
        /// <summary>
        /// 加工信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<object> GetCaseProcessInfo(int id)
        {
            var caseInitialManager = Resolve<CaseInitialManager>();
            var caseSource = await Manager.GetAll()
                .Include(o => o.AnYou)
                .Include(o => o.City)
                .Include(o => o.Court1)
                .Include(o => o.Court2)
                .Where(o => o.Id == id)
                .SingleOrDefaultAsync();

            var caseInitial = await caseInitialManager.GetAll()
                .Include(o => o.CaseNodes)
                .Include(o => o.CaseLabels)
                .Include(o => o.CaseFines)
                .Include(o => o.CaseCards)
                .Where(o => o.CaseSourceId == id && o.CreatorUserId == caseSource.OwerId)
                .FirstOrDefaultAsync();

            #region 初加工数据
            CaseInitialUpdateDto caseInitialUpdateDto = new CaseInitialUpdateDto();
            if (caseInitial == null)
            {
                //如果没有初加工，直接产生初加工记录
                caseInitial = new CaseInitial()
                {
                    CaseSourceId = id,
                    CaseStatus = CaseStatus.加工中
                };
                await caseInitialManager.InsertAndGetIdAsync(caseInitial);
            }
            caseInitial.MapTo(caseInitialUpdateDto);
            #endregion

            #region 精加工数据
            CaseFineUpdateDto caseFineUpdateDto = new CaseFineUpdateDto()
            {
                CaseInitialId = caseInitial.Id
            };
            caseFineUpdateDto.CaseFines = caseInitial.CaseFines.MapTo<List<CaseFineDto>>();
            #endregion

            #region 案例卡数据
            CaseCardUpdateDto caseCardUpdateDto = new CaseCardUpdateDto()
            {
                CaseInitialId= caseInitial.Id
            };
            caseCardUpdateDto.CaseCards = caseInitial.CaseCards.MapTo<List<CaseCardDto>>(); 
            #endregion

            return new
            {
                Source = new
                {
                    caseSource.SourceSN,
                    caseSource.AnYouId,
                    AnYou = caseSource.AnYou?.DisplayName,
                    caseSource.CityId,
                    City = caseSource.City?.DisplayName,
                    caseSource.Court1Id,
                    Court1 = caseSource.Court1?.DisplayName,
                    caseSource.Court2Id,
                    Court2 = caseSource.Court2?.DisplayName,
                    caseSource.LawyerFirms,
                    TrialPeople = caseSource.TrialPeople.Select(o => new { o.Name, TrialRole = o.TrialRole.ToString() }),
                    caseSource.SourceFile,
                    caseSource.ValidDate
                },
                caseInitialUpdateDto,
                caseFineUpdateDto,
                caseCardUpdateDto

            };
        }

        #region 初加工提交发布
        /// <summary>
        /// 提交初加工内容
        /// </summary>
        /// <param name="caseInitialUpdateDto"></param>
        /// <returns></returns>
        public virtual async Task UpdateInitial(CaseInitialUpdateDto caseInitialUpdateDto)
        {
            var caseInitialManager = Resolve<CaseInitialManager>();

            CaseInitial caseInitial = null;
            if (caseInitialUpdateDto.Id > 0)
            {
                //先删除对应初加工的分类绑定和标签绑定
                await Resolve<IRepository<CaseNode, int>>().DeleteAsync(o => o.RelType == "初加工" && o.CaseInitialId == caseInitialUpdateDto.Id);
                await Resolve<IRepository<CaseLabel, int>>().DeleteAsync(o => o.RelType == "初加工" && o.CaseInitialId == caseInitialUpdateDto.Id);

                caseInitial = await caseInitialManager.GetAll().Include(o => o.CaseNodes)
                    .Include(o => o.CaseLabels)
                    .Where(o => o.Id == caseInitialUpdateDto.Id).SingleOrDefaultAsync();
                if (caseInitial == null)
                {
                    throw new UserFriendlyException("不存在对应加工信息");
                }
                //数据设置
                caseInitial.Title = caseInitialUpdateDto.Title;
                caseInitial.SubjectId = caseInitialUpdateDto.SubjectId;
                caseInitial.Introduction = caseInitialUpdateDto.Introduction;
                caseInitial.Law = caseInitialUpdateDto.Law;
                caseInitial.LawyerOpinion = caseInitialUpdateDto.LawyerOpinion;
                caseInitial.Experience = caseInitialUpdateDto.Experience;
                caseInitial.Remarks = caseInitialUpdateDto.Remarks;
                caseInitial.JudgeInfo = caseInitialUpdateDto.JudgeInfo;
                //树节点和标签
                foreach (var caseNodeDto in caseInitialUpdateDto.CaseNodes)
                {
                    var caseNode = caseNodeDto.MapTo<CaseNode>();
                    caseNode.RelType = "初加工";
                    caseInitial.CaseNodes.Add(caseNode);
                }
                foreach (var caseLabelDto in caseInitialUpdateDto.CaseLabels)
                {
                    var caseLabel = caseLabelDto.MapTo<CaseLabel>();
                    caseLabel.RelType = "初加工";
                    caseInitial.CaseLabels.Add(caseLabel);
                }
            }
            var caseSource = await Manager.GetByIdAsync(caseInitial.CaseSourceId);
            //设置案源为加工中状态
            caseSource.CaseSourceStatus = CaseSourceStatus.加工中;
            caseInitial.CaseStatus = CaseStatus.加工中;
            await CurrentUnitOfWork.SaveChangesAsync();
        }
        /// <summary>
        /// 发布初加工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task PublishInitial(CaseInitialUpdateDto caseInitialUpdateDto)
        {
            await UpdateInitial(caseInitialUpdateDto);
            var caseInitial = await Resolve<CaseInitialManager>().GetAll().Include(o => o.CaseSource)
                .Where(o => o.Id == caseInitialUpdateDto.Id).SingleAsync();
            caseInitial.PublishDate = DateTime.Now;
            caseInitial.CaseStatus = CaseStatus.展示中;
            caseInitial.CaseSource.CaseSourceStatus = CaseSourceStatus.已加工;
        }
        #endregion

        #region 精加工提交发布
        /// <summary>
        /// 提交精加工信息
        /// </summary>
        /// <param name="caseFineUpdateDto"></param>
        /// <returns></returns>
        public virtual async Task UpdateFine(CaseFineUpdateDto caseFineUpdateDto)
        {
            var caseFineManager = Resolve<CaseFineManager>();
            var caseFineDtos = caseFineUpdateDto.CaseFines;
            var caseInitial = await Resolve<CaseInitialManager>().GetAll().Include(o => o.CaseFines)
                .Where(o => o.Id == caseFineUpdateDto.CaseInitialId).SingleOrDefaultAsync();
            if (caseInitial == null)
            {
                throw new UserFriendlyException("不存在加工信息");
            }
            //删除
            var delIds = caseInitial.CaseFines.Where(o => !caseFineDtos.Select(c => c.Id).Contains(o.Id)).Select(o => o.Id);
            await caseFineManager.DeleteAsync(delIds);
            //增加
            foreach (var caseFineDto in caseFineDtos.Where(o => o.Id == 0))
            {
                var newCaseFine = caseFineDto.MapTo<CaseFine>();
                newCaseFine.CaseInitialId = caseInitial.Id;
                newCaseFine.CaseStatus = CaseStatus.加工中;
                await caseFineManager.InsertAsync(newCaseFine);
            }
            //修改
            foreach (var caseFineDto in caseFineDtos.Where(o => o.Id > 0))
            {
                var oriCaseFine = await caseFineManager.GetByIdAsync(caseFineDto.Id);
                caseFineDto.MapTo(oriCaseFine);
                oriCaseFine.CaseStatus = CaseStatus.加工中;
            }
        }
        /// <summary>
        /// 发布精加工
        /// </summary>
        /// <param name="caseFineUpdateDto"></param>
        /// <returns></returns>
        public virtual async Task PublishFine(CaseFineUpdateDto caseFineUpdateDto)
        {
            var caseInitial = await Resolve<CaseInitialManager>().GetAll().Include(o => o.CaseCards)
                .Where(o => o.Id == caseFineUpdateDto.CaseInitialId).SingleOrDefaultAsync();
            if (caseInitial == null)
            {
                throw new UserFriendlyException("不存在加工信息");
            }
            if (caseInitial.CaseStatus != CaseStatus.展示中 && caseInitial.CaseStatus != CaseStatus.下架)
            {
                throw new UserFriendlyException("请先完成本判例的初加工并发布后再进行精加工发布，谢谢！您可点击保存按钮保留已做的成果");
            }

            await UpdateFine(caseFineUpdateDto);
            await CurrentUnitOfWork.SaveChangesAsync();
            var caseFines = await Resolve<CaseFineManager>().GetAll().Where(o => o.CaseInitialId == caseFineUpdateDto.CaseInitialId).ToListAsync();
            foreach (var caseFine in caseFines)
            {
                caseFine.PublishDate = DateTime.Now;
                caseFine.CaseStatus = CaseStatus.展示中;
            }
        } 
        #endregion

        #region 案例卡提交发布
        /// <summary>
        /// 提交案例卡信息
        /// </summary>
        /// <param name="caseFineDtos"></param>
        /// <returns></returns>
        public virtual async Task UpdateCard(CaseCardUpdateDto caseCardUpdateDto)
        {
            var caseCardManager = Resolve<CaseCardManager>();
            var caseCardDtos = caseCardUpdateDto.CaseCards;
            var caseInitial = await Resolve<CaseInitialManager>().GetAll().Include(o => o.CaseCards)
                .Where(o => o.Id == caseCardUpdateDto.CaseInitialId).SingleOrDefaultAsync();
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
                newCaseCard.CaseStatus = CaseStatus.加工中;
                await caseCardManager.InsertAsync(newCaseCard);
            }
            //修改
            foreach (var caseCardDto in caseCardDtos.Where(o => o.Id > 0))
            {
                var oriCaseCard = await caseCardManager.GetByIdAsync(caseCardDto.Id);
                caseCardDto.MapTo(oriCaseCard);
                oriCaseCard.CaseStatus = CaseStatus.加工中;
            }
        }
        /// <summary>
        /// 发布案例卡
        /// </summary>
        /// <param name="caseInitialId"></param>
        /// <returns></returns>
        public virtual async Task PublishCard(CaseCardUpdateDto caseCardUpdateDto)
        {
            var caseInitial = await Resolve<CaseInitialManager>().GetAll().Include(o => o.CaseCards)
                .Where(o => o.Id == caseCardUpdateDto.CaseInitialId).SingleOrDefaultAsync();
            if (caseInitial == null)
            {
                throw new UserFriendlyException("不存在加工信息");
            }
            if (caseInitial.CaseStatus != CaseStatus.展示中 && caseInitial.CaseStatus != CaseStatus.下架)
            {
                throw new UserFriendlyException("请先完成本判例的初加工并发布后再进行案例卡发布，谢谢！您可点击保存按钮保留已做的成果");
            }

            await UpdateCard(caseCardUpdateDto);
            await CurrentUnitOfWork.SaveChangesAsync();
            var caseCards = await Resolve<CaseCardManager>().GetAll().Where(o => o.CaseInitialId == caseCardUpdateDto.CaseInitialId).ToListAsync();
            foreach (var caseCard in caseCards)
            {
                caseCard.CaseStatus = CaseStatus.展示中;
            }
        } 
        /// <summary>
        /// 编辑提交单个案例卡
        /// </summary>
        /// <param name="caseCardDto"></param>
        /// <returns></returns>
        public virtual async Task UpdateSingleCard(CaseCardDto caseCardDto)
        {
            var card = await Resolve<CaseCardManager>().GetByIdAsync(caseCardDto.Id);
            caseCardDto.MapTo(card);
        }
        #endregion

        #endregion
    }
}
