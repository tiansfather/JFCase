using Abp.Authorization;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using Master.Configuration;
using Master.Dto;
using Master.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Master.Case
{
    [AbpAuthorize]
    public class CaseInitialAppService : ModuleDataAppServiceBase<CaseInitial, int>
    {
        public IHostingEnvironment HostingEnvironment { get; set; }

        protected override string ModuleKey()
        {
            return nameof(CaseInitial);
        }

        protected override async Task<IQueryable<CaseInitial>> BuildOrderQueryAsync(RequestPageDto request, IQueryable<CaseInitial> query)
        {
            //默认按推荐排序
            if (request.OrderField == "Id")
            {
                return query.OrderBy(o => o.Sort).ThenByDescending(o => o.Id);
                //return query.OrderBy(o => o.Sort).ThenByDescending(o => o.Id);
            }
            else
            {
            }
            return await base.BuildOrderQueryAsync(request, query);
        }

        /// <summary>
        /// 退回
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task Back(IEnumerable<int> ids)
        {
            var manager = Manager as CaseInitialManager;
            if (await manager.GetAll().CountAsync(o => ids.Contains(o.Id) && o.CaseStatus != CaseStatus.下架) > 0)
            {
                throw new UserFriendlyException("只有下架的案例可以退回");
            }
            var caseInitials = await Manager.GetAll().Include(o => o.CaseSource)
                .Where(o => ids.Contains(o.Id)).ToListAsync();
            foreach (var caseInitial in caseInitials)
            {
                caseInitial.CaseStatus = CaseStatus.退回;
                caseInitial.CaseSource.CaseSourceStatus = CaseSourceStatus.加工中;
            }
        }

        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task Down(IEnumerable<int> ids)
        {
            var caseInitials = await Manager.GetAll().Include(o => o.CaseSource)
                .Where(o => ids.Contains(o.Id)).ToListAsync();
            foreach (var caseInitial in caseInitials)
            {
                caseInitial.CaseStatus = CaseStatus.下架;
            }
        }

        /// <summary>
        /// 上架
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task Up(IEnumerable<int> ids)
        {
            var manager = Manager as CaseInitialManager;
            if (await manager.GetAll().CountAsync(o => ids.Contains(o.Id) && o.CaseStatus != CaseStatus.下架 && o.PublishDate != null) > 0)
            {
                throw new UserFriendlyException("只有下架中的已发布案例可以上架");
            }
            var caseInitials = await Manager.GetAll().Include(o => o.CaseSource)
                .Where(o => ids.Contains(o.Id)).ToListAsync();
            foreach (var caseInitial in caseInitials)
            {
                caseInitial.CaseStatus = CaseStatus.展示中;
            }
        }

        /// <summary>
        /// 推荐
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task Recommand(IEnumerable<int> ids)
        {
            var caseInitials = await Manager.GetAll().Include(o => o.CaseSource)
                .Where(o => ids.Contains(o.Id)).ToListAsync();
            foreach (var caseInitial in caseInitials)
            {
                caseInitial.IsActive = true;
            }
        }

        /// <summary>
        /// 取消推荐
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task UnRecommand(IEnumerable<int> ids)
        {
            var caseInitials = await Manager.GetAll().Include(o => o.CaseSource)
                .Where(o => ids.Contains(o.Id)).ToListAsync();
            foreach (var caseInitial in caseInitials)
            {
                caseInitial.IsActive = false;
            }
        }

        public virtual async Task SetSort(int id, string sortStr)
        {
            int sort = 999999;
            if (int.TryParse(sortStr, out sort))
            {
                if (sort <= 0)
                {
                    throw new UserFriendlyException("排序值必须大于0");
                }
            }
            else
            {
                sort = 999999;
            }
            var caseInitial = await Manager.GetByIdAsync(id);
            caseInitial.Sort = sort;
        }

        /// <summary>
        /// 管理方清空判例的加工内容
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task ClearContent(int[] ids)
        {
            var caseSourceIds = await Manager.GetAll().Where(o => ids.Contains(o.Id)).Select(o => o.CaseSourceId).ToListAsync();
            foreach (var id in caseSourceIds)
            {
                await Resolve<CaseSourceManager>().ClearCaseContent(id, true);
            }
        }

        /// <summary>
        /// 传送数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task TransferRemote(IEnumerable<int> ids)
        {
            var caseInitials = await Manager.GetAll()
                .Include(o => o.CaseSource.AnYou)
                .Include(o => o.CaseCards)
                .Include(o => o.CreatorUser)
                .Where(o => ids.Contains(o.Id)).ToListAsync();

            var result = new List<RemoteResult>();

            foreach (var caseInitial in caseInitials)
            {
                var judgeInfo = caseInitial.GetPropertyValue<JudgeInfo>("JudgeInfo");
                var data = new
                {
                    anYou = caseInitial.CaseSource.AnYou.DisplayName,
                    content = caseInitial.Introduction,
                    courtOpinion = caseInitial.CaseCards.Select(o => new
                    {
                        title = o.Title,
                        content = o.Content
                    }),
                    experience = caseInitial.Experience,
                    lawyerOpinion = caseInitial.LawyerOpinion,
                    lawyerPhone = caseInitial.CreatorUser.PhoneNumber,
                    sn = caseInitial.CaseSource.SourceSN,
                    title = caseInitial.Title,
                    property = judgeInfo
                };

                var transferResult = await TransferRemoteInner(data);
                transferResult.SN = data.sn;
                if (transferResult.Success)
                {
                    caseInitial.TransferNum++;//增加传送数据数量
                }
                result.Add(transferResult);
            }

            if (!result.All(o => o.Success))
            {
                var successNum = result.Where(o => o.Success).Count();
                var failNum = result.Where(o => !o.Success).Count();
                var errMsg = $"共{successNum}条成功,{failNum}条失败，失败原因分别为:<br/>{string.Join("<br/>", result.Where(o => !o.Success).Select(o => $"{o.SN}:{o.Msg}"))}";
                throw new UserFriendlyException(errMsg);
            }
        }

        public async Task<RemoteResult> TransferRemoteInner(object data)
        {
            var remoteUrl = AppConfigurations.Get(HostingEnvironment.ContentRootPath, HostingEnvironment.EnvironmentName)["base:remoteurl"];
            var uploadData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            try
            {
                var wc = new WebClient();
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                var uploadResult = wc.UploadString(remoteUrl, uploadData);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<RemoteResult>(uploadResult);
            }
            catch (WebException ex)
            {
                var bytes = ex.Response.GetResponseStream().GetAllBytes();
                var content = System.Text.Encoding.UTF8.GetString(bytes);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<RemoteResult>(content) ?? new RemoteResult() { Msg = ex.Message };
            }
            catch (Exception ex)
            {
                return new RemoteResult() { Msg = ex.Message };
            }
        }
    }

    public class RemoteResult
    {
        public string SN { get; set; }
        public int Code { get; set; }
        public bool Success { get; set; }
        public string Msg { get; set; } = "未知错误";
    }
}