using Master.Scheduler.Domains;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES
{
    public class MESSchedulerExporter: DefaultSchedulerExporter
    {
        public ProcessTaskReportManager ProcessTaskReportManager { get; set; }
        public override async  Task<IWorkbook> DoExport(int projectId)
        {
            var workbook=await base.DoExport(projectId);
            var sheet = workbook.GetSheetAt(0);
            
            //获取最后一条报工记录
            var lastReport = await ProcessTaskReportManager.GetAll().Where(o => o.ProcessTask.Part.ProjectId == projectId).LastOrDefaultAsync();
            if (lastReport != null && lastReport.Files.Count>0)
            {
                var maxRowNum = sheet.LastRowNum;
                sheet.CreateRow(maxRowNum + 10);
                var file = lastReport.Files[0];
                var patriarch = sheet.CreateDrawingPatriarch();
                sheet.AddMergedRegion(new CellRangeAddress(maxRowNum+1, maxRowNum+10, 0,  3));
                if (!string.IsNullOrEmpty(file.FilePath))
                {
                    try
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(Common.PathHelper.VirtualPathToAbsolutePath(file.FilePath));
                        int pictureIdx = workbook.AddPicture(bytes, PictureType.JPEG);
                        // 插图片的位置  HSSFClientAnchor（dx1,dy1,dx2,dy2,col1,row1,col2,row2) 后面再作解释
                        IClientAnchor anchor = patriarch.CreateAnchor(70, 10, 0, 0, 0, maxRowNum + 1, 3, maxRowNum + 10);
                        //把图片插到相应的位置
                        patriarch.CreatePicture(anchor, pictureIdx);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            
            return workbook;
        }
    }
}
