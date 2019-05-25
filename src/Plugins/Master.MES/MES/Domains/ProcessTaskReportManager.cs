using Abp.UI;
using Master.Authentication;
using Master.Configuration;
using Master.Domain;
using Master.MES.Jobs;
using Microsoft.AspNetCore.Hosting;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES
{
    public class ProcessTaskReportManager : DomainServiceBase<ProcessTaskReport, int>
    {
        private IHostingEnvironment _hostingEnvironment;

        //public ProcessTaskManager ProcessTaskManager { get; set; }
        //public PersonManager PersonManager { get; set; }
        public ProcessTaskReportManager(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 重写过滤
        /// </summary>
        /// <returns></returns>
        public override IQueryable<ProcessTaskReport> GetAll()
        {
            //add 20181210增加过滤,如果当前用户是独立用户
            if (AbpSession.IsSeparateUser() )
            {
                return Repository.GetAll().Where(o =>o.ProcessTask.CreatorUserId==AbpSession.UserId.Value);
            }

            return Repository.GetAll();
        }
        #region 重写删除
        public async override Task DeleteAsync(IEnumerable<int> ids)
        {
            var reports = await GetListByIdsAsync(ids);
            foreach (var report in reports)
            {
                await DeleteAsync(report);
            }
        }
        /// <summary>
        /// 删除时需要进行判断
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task DeleteAsync(ProcessTaskReport entity)
        {
            var processTask = entity.ProcessTask;
            switch (processTask.ProcessTaskStatus)
            {
                //已完成任务只允许删除下机报工和加工中报工
                case ProcessTaskStatus.Completed:
                    if (entity.ReportType == ReportType.下机)
                    {
                        //删除下机报工后设置任务状态为加工中
                        processTask.ProcessTaskStatus = ProcessTaskStatus.Processing;
                        //清除任务的下机时间\实际工时\进度
                        processTask.ActualHours = null;
                        processTask.EndDate = null;
                        processTask.Progress = 0.5M;
                    }else if (entity.ReportType == ReportType.加工)
                    {
                        //删除加工中报工不做任何处理
                    }
                    else
                    {
                        throw new UserFriendlyException(L("该任务为已完成状态，只能对加工和下机的报工记录标记失效"));
                    }
                    break;
                //暂停中任务只允许删除暂停报工和加工中报工
                case ProcessTaskStatus.Suspended:
                    if (entity.ReportType == ReportType.暂停)
                    {
                        //删除暂停报工后设置任务状态为加工中
                        processTask.ProcessTaskStatus = ProcessTaskStatus.Processing;                        
                    }
                    else if (entity.ReportType == ReportType.加工)
                    {
                        //删除加工中报工不做任何处理
                    }
                    else
                    {
                        throw new UserFriendlyException(L("该任务为已暂停状态，只能对加工和暂停的报工记录标记失效"));
                    }
                    break;
                //加工中任务只允许删除上机报工和重新开始报工和加工中报工
                case ProcessTaskStatus.Processing:
                    if (entity.ReportType == ReportType.上机)
                    {
                        //删除报工后设置任务状态为待上机
                        processTask.ProcessTaskStatus = ProcessTaskStatus.WaitForProcess;
                        //清除任务的上机时间,设置进度为0
                        processTask.StartDate = null;
                        processTask.Progress = 0;
                    }
                    else if (entity.ReportType==ReportType.重新开始)                    {
                        //删除重新开始后设置任务状态为暂停
                        processTask.ProcessTaskStatus = ProcessTaskStatus.Suspended;
                        
                    }
                    else if (entity.ReportType == ReportType.加工)
                    {
                        //删除加工中报工不做任何处理
                    }
                    else
                    {
                        throw new UserFriendlyException(L("该任务为加工中状态，只能对加工和上机、重新开始的报工记录标记失效"));
                    }
                    break;
                //已到料任务只允许删除到料报工
                case ProcessTaskStatus.Received:
                    if (entity.ReportType == ReportType.到料)
                    {
                        //删除到料报工后设置任务状态为待上机
                        processTask.ProcessTaskStatus = ProcessTaskStatus.WaitForProcess;
                        //清空任务的到料时间
                        processTask.ReceiveDate = null;
                    }
                    else
                    {
                        throw new UserFriendlyException(L("该任务为到料状态,只能对到料报工标记失效"));
                    }
                    break;
            }

            await base.DeleteAsync(entity);
        }
        #endregion

    }
}
