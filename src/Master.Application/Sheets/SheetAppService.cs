using Abp.UI;
using Master.Entity;
using Master.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Sheets
{
    public class SheetAppService:MasterAppServiceBase<Sheet,int>
    {
        /// <summary>
        /// 单据提交
        /// </summary>
        /// <param name="sheetSubmitDto"></param>
        /// <returns></returns>
        public virtual async Task Submit(SheetSubmitDto sheetSubmitDto)
        {
            var sheet= GenerateSheet(sheetSubmitDto);

            await Manager.InsertAsync(sheet);
        }

        /// <summary>
        /// 产生新的单据
        /// </summary>
        /// <param name="sheetSubmitDto"></param>
        private Sheet GenerateSheet(SheetSubmitDto sheetSubmitDto)
        {
            var sheet = new Sheet();
            sheet.SheetName = sheetSubmitDto.SheetHeader.GetDataOrEmpty("sheetName");
            if (string.IsNullOrEmpty(sheet.SheetName))
            {
                throw new UserFriendlyException(L("单据标志sheetName不能为空"));
            }
            sheet.SheetSN = sheetSubmitDto.SheetHeader.GetDataOrEmpty("sheetSN");
            if (string.IsNullOrEmpty(sheet.SheetSN))
            {
                throw new UserFriendlyException(L("单据编号不能为空"));
            }

            sheet.SheetDate = sheetSubmitDto.SheetHeader.GetDataOrDefault<DateTime?>("sheetDate")??DateTime.Now;

            sheet.BusinessType = sheetSubmitDto.SheetHeader.GetDataOrEmpty("businessType");

            sheet.HandlerId = sheetSubmitDto.SheetHeader.GetDataOrDefault<long?>("handlerId");

            sheet.ProjectId = sheetSubmitDto.SheetHeader.GetDataOrDefault<int?>("projectId");

            sheet.UnitId = sheetSubmitDto.SheetHeader.GetDataOrDefault<int?>("unitId");

            //将表头数据保存至属性中
            foreach(var key in sheetSubmitDto.SheetHeader.Keys)
            {
                sheet.SetPropertyValue(key, sheetSubmitDto.SheetHeader[key]);
            }

            //将表格内容保存
            foreach(var rowValue in sheetSubmitDto.SheetValues)
            {
                var sheetContent = new SheetContent();

                foreach(var key in rowValue.Keys)
                {
                    sheetContent.SetPropertyValue(key, rowValue[key]);
                }

                sheet.SheetContents.Add(sheetContent);
            }


            return sheet;
        }
    }
}
