using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Master.Dto;
using Master.Entity;
using Master.MES.Dtos;
using Master.Projects;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace Master.MES.Service
{
    public class PartAppService : MasterAppServiceBase<Part, int>
    {
        public IRepository<ProcessTask,int> ProcessTaskRepository { get; set; }
        public IProjectManager ProjectManager { get; set; }
        public ProcessTaskReportManager ProcessTaskReportManager { get; set; }

        public ProcessTaskManager ProcessTaskManager { get; set; }


        //protected async override Task<IQueryable<Part>> GetQueryable(RequestPageDto request)
        //{
        //    var query=await base.GetQueryable(request);
        //    //仅显示启用生产
        //    return query.Where(o => o.EnableProcess).Include("ProcessTasks.ProcessType");
        //}
        protected override async Task<PagedResult<Part>> GetPageResultQueryable(RequestPageDto request)
        {
            var query=await GetQueryable(request);
            return query.Where(o => o.EnableProcess).Include("ProcessTasks.ProcessType").PageResult(request.Page, request.Limit);
        }
        protected override async Task<IQueryable<Part>> BuildKeywordQueryAsync(string keyword, IQueryable<Part> query)
        {
            return query.Where(o => o.PartSN.Contains(keyword) || o.PartName.Contains(keyword));
        }
        /// <summary>
        /// 加工路线分页返回
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [DontWrapResult]
        [AbpAuthorize]
        public virtual async Task<ResultPageDto> GetTaskPageResult(RequestPageDto request)
        {
            var query = await GetPageResultQueryable(request);

            var parts = await query.Queryable.ToListAsync();
            //foreach(var part in parts)
            //{
            //    foreach(var task in part.ProcessTasks)
            //    {
            //        await ProcessTaskRepository.EnsurePropertyLoadedAsync(task, p => p.ProcessType);
            //    }
            //}
            var data = parts.Select(o => {
                //获取零件的所有任务
                var partTasks = o.ProcessTasks.OrderBy(s=>s.Sort).Select(t=> {
                    return new {t.Id,
                        t.ProcessType.ProcessTypeName,
                        t.Progress,
                        t.ProcessTaskStatus,
                        PlanStartDate =t.AppointDate?.ToString("MM-dd"),
                        PlanEndDate =t.RequireDate?.ToString("MM-dd"),
                        StartDate = t.StartDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                        EndDate = t.EndDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                        t.Sort,t.Inner,t.Supplier?.UnitName,
                        t.ArrangeDate,t.EstimateHours,
                        SendProcessor =t.HasStatus(ProcessTask.Status_SendProcessor),
                        Printed =t.HasStatus(ProcessTask.Status_Print),
                        ProcessorReaded = o.HasStatus(ProcessTask.Status_ProcessorReaded),
                        RateInfo = t.GetPropertyValue<string>("RateInfo"),
                        SubmitFeeFromProcessor = t.GetPropertyValue<SubmitFeeFromProcessorDto>("SubmitFeeFromProcessorDto"),
                        t.TaskInfo,
                        t.Emergency,
                        t.ProcessTaskProgressInfo,
                        t.Quoted
                    };
                });
                //优化为缓存方式
                var project = ProjectManager.GetByIdFromCacheAsync(o.ProjectId).Result;
                return new
                {
                    o.Id,
                    project?.ProjectSN,
                    o.PartSN,
                    o.PartName,
                    o.PartImg,
                    o.PartSpecification,
                    o.PartNum,
                    Tasks=partTasks

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
        /// 返回项目编号下面的所有零件
        /// </summary>
        /// <param name="projectSN"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public virtual async Task<List<PartDto>> GetAll(string projectSN,string key,bool enableProcess=true)
        {
            //todo:当没有项目编号时返回所有零件所造成的性能影响
            var query = Manager.GetAll();
            if (enableProcess)
            {
                query = query.Where(o => o.EnableProcess);
            }
            if (!projectSN.IsNullOrEmpty())
            {
                query = query.Where(o => o.Project.ProjectSN.Contains(projectSN));
            }
            if (!key.IsNullOrEmpty())
            {
                query = query.Where(o => o.PartName.Contains(key));
            }

            return (await query.OrderByDescending(o=>o.Id).ToListAsync()).MapTo<List<PartDto>>();
        }

        /// <summary>
        /// 设置零件的图片
        /// </summary>
        /// <param name="partId"></param>
        /// <param name="partImg"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public virtual async Task SetPartImg(int partId,string partImg)
        {
            var part = await Repository.GetAsync(partId);
            part.PartImg = partImg;
        }

        /// <summary>
        /// 获取某零件的所有报工记录
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public virtual async Task<object> GetPartReports(int partId)
        {
            var reports = await ProcessTaskReportManager.GetAll()
                .Include(o => o.Reporter)
                .Include(o=>o.ProcessTask).ThenInclude(o=>o.ProcessType)
                .Where(o => o.ProcessTask.PartId == partId).ToListAsync();

            return reports.Select(o =>
            new {
                o.Id,
                ReporterName = o.Reporter.Name,
                o.ReportType,
                o.ProcessTask.ProcessType.ProcessTypeName,
                ReportTime = o.ReportTime.ToString("yyyy-MM-dd HH:mm:ss"),
                CreationTime = o.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                o.Remarks,
                o.Files
            });
        }

        /// <summary>
        /// 获取零件相关加工信息
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public virtual async Task<object> GetPartTaskInfo(int partId)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant))
            {
                var part = await Manager.GetAll()
                    .Include(o => o.Project)
                    .Include("ProcessTasks.ProcessType")
                    .Where(o => o.Id == partId)
                    .SingleOrDefaultAsync();

                if (part == null)
                {
                    throw new UserFriendlyException(L("未找到对应零件信息"));
                }

                //获取零件的所有任务
                var partTasks = part.ProcessTasks.OrderBy(s => s.Sort).Select(t => {
                    return new {
                        t.Id,
                        t.ProcessSN,
                        t.ProcessTypeId,
                        t.ProcessType.ProcessTypeName,
                        t.Progress,
                        t.ProcessTaskStatus,
                        PlanStartDate = t.AppointDate?.ToString("yyyy-MM-dd"),
                        PlanEndDate = t.RequireDate?.ToString("yyyy-MM-dd"),
                        ArrangeDate=t.ArrangeDate?.ToString("yyyy-MM-dd HH:mm"),
                        StartDate = t.StartDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                        EndDate = t.EndDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                        t.Sort,
                        t.EstimateHours,
                        t.TaskInfo,
                        t.Inner,
                        t.Emergency,
                        t.Xiu,
                        t.Cha
                    };
                });
                return new
                {
                    part.Id,
                    part.Project?.ProjectSN,
                    part.PartSN,
                    part.PartName,
                    part.PartImg,
                    part.PartSpecification,
                    part.PartNum,
                    Tasks = partTasks

                };
            }
                
        }

        #region 工艺设定
        /// <summary>
        /// 保存零件信息的方法
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize]
        public virtual async Task SavePartTaskInfo(SubmitPartTaskInfoDto data)
        {
            var part = await Manager.GetAll()
                    .Include(o => o.Project)
                    .Include(o => o.ProcessTasks)
                    .Include("ProcessTasks.ProcessType")
                    .Where(o => o.Id == data.Id)
                    .SingleOrDefaultAsync();


            //删除
            var deltemp = part.ProcessTasks.Where(o => !data.Tasks.Exists(t => t.Id == o.Id)).ToList();
            foreach (var temp in deltemp)
            {
                ProcessTaskRepository.Delete(temp);
            }
            var lastTask = part.ProcessTasks.LastOrDefault();//最后一道工序
            //编辑
            foreach (var temp in data.Tasks)
            {
                var t = part.ProcessTasks.Where(o => o.Id == temp.Id).SingleOrDefault();

                string type = "0";
                //增加
                if (t == null)
                {
                    type = "1";
                    //新建任务，读取最后一个任务的开单人和模具组长
                    t = new ProcessTask()
                    {
                        Poster = lastTask?.Poster,
                        ProjectCharger = lastTask?.ProjectCharger,
                        Verifier = lastTask?.Verifier,
                        CraftsMan = lastTask?.CraftsMan,
                        Checker = lastTask?.Checker,
                        FeeType = FeeType.按时间
                    };
                }
                //修改
                t.Sort = temp.Sort;
                t.ProcessTypeId = temp.ProcessTypeId;
                if (temp.PlanStartDate == null)
                {
                    t.AppointDate = null;
                }
                else
                {
                    t.AppointDate = Convert.ToDateTime(temp.PlanStartDate);
                }

                if (temp.PlanStartDate == null)
                {
                    t.AppointDate = null;
                }
                else
                {
                    t.AppointDate = Convert.ToDateTime(temp.PlanStartDate);
                }

                if (temp.PlanEndDate == null)
                {
                    t.RequireDate = null;
                }
                else
                {
                    t.RequireDate = Convert.ToDateTime(temp.PlanEndDate);
                }

                t.EstimateHours = temp.EstimateHours;

                t.TaskInfo = temp.TaskInfo;

                t.Inner = temp.Inner;
                t.Emergency = temp.Emergency;
                t.Cha = temp.Cha;
                t.Xiu = temp.Xiu;

                //增加
                if (type == "1")
                {
                    t.PartId = data.Id;
                    await ProcessTaskManager.SaveAsync(t);
                    await CurrentUnitOfWork.SaveChangesAsync();
                }

                //开单操作
                if (await ProcessTaskManager.CanKaiDan(t) && string.IsNullOrEmpty(t.ProcessSN))
                {
                    await ProcessTaskManager.KaiDan(t);
                }

            } 
            


        }
        #endregion

        #region 导出多个零件的工艺单
        /// <summary>
        /// 导出加工工艺表
        /// </summary>
        /// <param name="partIds"></param>
        /// <returns></returns>
        public virtual async Task<string> ExportPartProcessInfo(int[] partIds)
        {
            var parts = await Manager.GetAll()
                .Include(o => o.Project).ThenInclude(o=>o.Unit)
                .Include("ProcessTasks.ProcessType")
                .Where(o => partIds.Contains(o.Id))
                .Select(o => new {
                    o.Project.ProjectSN,
                    o.Project.ProjectName,
                    ProjectCharger=o.Project.GetPropertyValue<string>("ProjectCharger"),
                    UnitName=o.Project.Unit!=null?o.Project.Unit.UnitName:"",
                    o.PartName,
                    o.PartImg,
                    o.PartSpecification,
                    Tasks=o.ProcessTasks.Select(t=>new {
                        t.ProcessType.ProcessTypeName,
                        t.AppointDate,
                        t.RequireDate,
                        t.StartDate,
                        t.EndDate
                    })
                })
                .ToListAsync();

            #region 导出方法
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("模具制作工艺单");
            sheet.DefaultColumnWidth = 300 * 256;//貌似没有作用
            var rowIndex = 0;
            var cellIndex = 0;
            //模具信息行
            IRow row = sheet.CreateRow(rowIndex++);
            parts.ForEach(o => {
                row.CreateCell(cellIndex++).SetCellValue(o.ProjectSN);
                row.CreateCell(cellIndex++).SetCellValue(o.ProjectName);
                row.CreateCell(cellIndex++).SetCellValue(o.ProjectCharger);
                row.CreateCell(cellIndex++).SetCellValue(o.UnitName);
            });
            //零件信息行
            row = sheet.CreateRow(rowIndex++);
            cellIndex = 0;
            parts.ForEach(o => {
                row.CreateCell(cellIndex++).SetCellValue(o.PartName);
                row.CreateCell(cellIndex++).SetCellValue(o.PartSpecification);
                row.CreateCell(cellIndex++).SetCellValue("");
                row.CreateCell(cellIndex++).SetCellValue("");
            });
            //图片行
            row = sheet.CreateRow(rowIndex++);
            row.Height = 80 * 20;
            cellIndex = 0;
            HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
            parts.ForEach(o => {
                sheet.AddMergedRegion(new CellRangeAddress(rowIndex-1, rowIndex - 1, cellIndex, cellIndex+3));
                if (!string.IsNullOrEmpty(o.PartImg))
                {
                    try
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(Common.PathHelper.VirtualPathToAbsolutePath(o.PartImg));
                        int pictureIdx = workbook.AddPicture(bytes, PictureType.JPEG);
                        // 插图片的位置  HSSFClientAnchor（dx1,dy1,dx2,dy2,col1,row1,col2,row2) 后面再作解释
                        HSSFClientAnchor anchor = new HSSFClientAnchor(70, 10, 0, 0, cellIndex, rowIndex - 1, cellIndex + 4, rowIndex);
                        //把图片插到相应的位置
                        HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);
                    }
                    catch(Exception ex)
                    {

                    }
                }
                
                cellIndex += 4;
            });
            //工序表头
            row = sheet.CreateRow(rowIndex++);
            cellIndex = 0;
            parts.ForEach(o =>
            {
                row.CreateCell(cellIndex++).SetCellValue("序号");
                row.CreateCell(cellIndex++).SetCellValue("加工流程");
                row.CreateCell(cellIndex++).SetCellValue("计划时间");
                row.CreateCell(cellIndex++).SetCellValue("实际时间");
            });
            //工序内容
            var maxProcessTypeCount = parts.Max(o => o.Tasks.Count());
            for(var i = 0; i < maxProcessTypeCount; i++)
            {
                //先构建下面所有行
                sheet.CreateRow(rowIndex + i);
            }
            cellIndex = 0;
            for(var i = 0; i < parts.Count; i++)
            {
                var part = parts[i];
                for(var j = 0; j < part.Tasks.Count(); j++)
                {
                    var task = part.Tasks.ElementAt(j);
                    row = sheet.GetRow(rowIndex + j);
                    row.CreateCell(i * 4).SetCellValue(j+1);
                    row.CreateCell(i * 4 + 1).SetCellValue(task.ProcessTypeName);
                    row.CreateCell(i * 4 + 2).SetCellValue(task.AppointDate?.ToString("MM-dd")+"-"+task.RequireDate?.ToString("MM-dd"));
                    row.CreateCell(i * 4 + 3).SetCellValue(task.StartDate?.ToString("MM-dd") + "-" + task.EndDate?.ToString("MM-dd"));
                }
            }
            //边框
            ICellStyle style = workbook.CreateCellStyle();
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BottomBorderColor = HSSFColor.Black.Index;
            style.LeftBorderColor = HSSFColor.Black.Index;
            style.RightBorderColor = HSSFColor.Black.Index;
            style.TopBorderColor = HSSFColor.Black.Index;
            for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
            {
                row = sheet.GetRow(i);
                for (int j = 0; j <= row.LastCellNum; j++)
                {
                    var cell = row.GetCell(j);
                    if (cell != null)
                    {
                        cell.CellStyle = style;
                    }
                    
                }
            }
            #endregion
            var filePath = $"/Temp/{Guid.NewGuid()}.xls";
            System.IO.Directory.CreateDirectory(Common.PathHelper.VirtualPathToAbsolutePath("/Temp/"));
            using (var fs = new FileStream(Common.PathHelper.VirtualPathToAbsolutePath(filePath), FileMode.Create, FileAccess.ReadWrite))
            {
                workbook.Write(fs); //写入到excel
            }
            workbook = null;
            return filePath;
        }
        #endregion

        #region 时间轴相关接口
        /// <summary>
        /// 获取零件信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="partId"></param>
        /// <returns></returns>
        public virtual async Task<object> GetPartInfos(int? projectId, int? partId)
        {
            if(projectId==null && partId == null)
            {
                throw new UserFriendlyException(L("参数错误"));
            }
            return await Manager.GetAll()
                .Where(o=>o.EnableProcess)
                .WhereIf(projectId != null, o => o.ProjectId == projectId.Value)
                .WhereIf(partId != null, o => o.Id == partId.Value)
                .Select(o => new
                {
                    o.Id,
                    o.PartName
                })
                .ToListAsync();
        }
        [DontWrapResult]
        public virtual async Task<object> GetPartTimeLineTasks(DateTime from, DateTime to, int? projectId,int? partId)
        {
            

            var tasks = await ProcessTaskManager
                .GetTimeLineTasksQuery(from, to)
                .Include("Part.Project")
                .Include(o => o.ProcessType)
                .WhereIf(projectId.HasValue, o => o.Part.ProjectId==projectId)
                .WhereIf(partId.HasValue,o=>o.PartId==partId)
                .Select(o =>
                new
                {
                    o.Id,
                    o.StartDate,
                    o.EndDate,
                    o.Part.Project.ProjectSN,
                    o.Part.PartName,
                    o.ProcessType.ProcessTypeName,
                    o.ArrangeDate,
                    o.ArrangeEndDate,
                    o.AppointDate,
                    o.EquipmentId,
                    o.Progress,
                    o.EstimateHours,
                    o.ProcessTaskStatus,
                    o.ActualHours,
                    o.PartId
                    //id=o.Id,
                    //text=$"{o.Part.Project.ProjectSN}{o.Part.PartName}{o.ProcessType.ProcessTypeName}",
                    //
                    //start_date =o.StartDate.HasValue?o.StartDate.Value.ToString("yyyy-MM-dd HH:mm"):(o.AppointDate.Value).ToString("yyyy-MM-dd HH:mm"),
                    //end_date = (o.ArrangeDate ?? o.AppointDate.Value).AddHours(Convert.ToDouble(o.EstimateHours??0)).ToString("yyyy-MM-dd HH:mm"),
                    //equipmentId=o.EquipmentId,
                    //progress=o.Progress
                })
                .ToListAsync();
            //如果已上机取实际上机时间，不然取安排上机时间
            return tasks.Select(o => {
                var startDate = o.StartDate ?? o.ArrangeDate ?? o.AppointDate.Value;
                var endDate = o.EndDate ??o.ArrangeEndDate?? startDate.AddHours(Convert.ToDouble(o.EstimateHours ?? 0));
                return new
                {
                    id = o.Id,
                    o.PartId,
                    text = $"{o.ProjectSN}{o.PartName}{o.ProcessTypeName}",
                    progress = o.Progress,
                    equipmentId = o.EquipmentId ?? -1,
                    o.ProcessTaskStatus,
                    ActualHours = Math.Round(o.ActualHours ?? 0, 2),
                    o.EstimateHours,
                    start_date = startDate.ToString("yyyy-MM-dd HH:mm"),
                    end_date = endDate.ToString("yyyy-MM-dd HH:mm")
                };
            });
        }
        #endregion

    }
}
