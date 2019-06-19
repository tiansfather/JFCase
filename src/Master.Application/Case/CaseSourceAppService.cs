using Abp.Authorization;
using Abp.AutoMapper;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Case
{
    [AbpAuthorize]
    public class CaseSourceAppService : ModuleDataAppServiceBase<CaseSource, int>
    {
        protected override string ModuleKey()
        {
            return nameof(CaseSource);
        }

        #region 添加修改
        public virtual async Task Update(CaseSourceUpdateDto caseSourceDto)
        {
            caseSourceDto.Normalize();
            var manager = Manager as CaseSourceManager;
            CaseSource caseSource = null;
            if (!caseSourceDto.Id.HasValue || caseSourceDto.Id.Value == 0)
            {
                caseSource = caseSourceDto.MapTo<CaseSource>();
            }
            else
            {
                caseSource = await manager.GetByIdAsync(caseSourceDto.Id.Value);
                caseSourceDto.MapTo(caseSource);
                
            }

            await manager.SaveAsync(caseSource);
        } 
        #endregion

        public override async Task<object> GetById(int primary)
        {
            var entity=await base.GetById(primary);
            return entity.MapTo<CaseSourceUpdateDto>();
        }
        #region 上架下架删除
        public virtual async Task Freeze(IEnumerable<int> ids)
        {
            if (await Manager.GetAll().CountAsync(o => ids.Contains(o.Id) && o.CaseSourceStatus != CaseSourceStatus.待选) > 0)
            {
                throw new UserFriendlyException("只有待选状态的判例可以下架");
            }
            else
            {
                var caseSources = await Manager.GetAll().Where(o => ids.Contains(o.Id)).ToListAsync();
                foreach(var caseSource in caseSources)
                {
                    caseSource.CaseSourceStatus = CaseSourceStatus.下架;
                }
            }
        }
        public virtual async Task UnFreeze(IEnumerable<int> ids)
        {
            if (await Manager.GetAll().CountAsync(o => ids.Contains(o.Id) && o.CaseSourceStatus != CaseSourceStatus.下架) > 0)
            {
                throw new UserFriendlyException("只有下架状态的判例可以上架");
            }
            else
            {
                var caseSources = await Manager.GetAll().Where(o => ids.Contains(o.Id)).ToListAsync();
                foreach (var caseSource in caseSources)
                {
                    caseSource.CaseSourceStatus = CaseSourceStatus.待选;
                }
            }
        }
        public override async Task DeleteEntity(IEnumerable<int> ids)
        {
            if (await Manager.GetAll().CountAsync(o => ids.Contains(o.Id) && o.CaseSourceStatus != CaseSourceStatus.下架) > 0)
            {
                throw new UserFriendlyException("只有下架状态的判例可以删除");
            }
            await base.DeleteEntity(ids);
        }
        #endregion

        #region 导入
        public virtual async Task<object> DoImport(string excelFilePath,string zipFilePath,int importType)
        {
            var manager = Manager as CaseSourceManager;
            var caseSourceImportResults = await ReadExcel(excelFilePath);
            if (caseSourceImportResults.Exists(o => !o.Valid))
            {
                throw new UserFriendlyException("数据验证失败,请检查后重新提交");
            }
            //如果是增量的，忽略已存在的数据
            if (importType == 0)
            {
                caseSourceImportResults = caseSourceImportResults.Where(o => !o.Exist).ToList();
            }
            //之前解压出来的文件路径
            DateTime now = DateTime.Now;
            var tempDirectory = System.IO.Path.GetDirectoryName(Common.PathHelper.VirtualPathToAbsolutePath(zipFilePath));
            var virtualDirectory = $"/files/{now.Year}/{now.ToString("MM")}/{now.ToString("dd")}";
            System.IO.Directory.CreateDirectory(Common.PathHelper.VirtualPathToAbsolutePath(virtualDirectory));
            foreach (var importResult in caseSourceImportResults)
            {
                var caseSource =await manager.GetAll().Where(o => o.SourceSN == importResult.SourceSN).FirstOrDefaultAsync();
                if (caseSource == null)
                {
                    caseSource = new CaseSource();
                }
                importResult.CaseSourceUpdateDto.MapTo(caseSource);
                //将pdf文件从临时文件夹移至正式文件夹
                caseSource.SourceFile = $"{virtualDirectory}/{caseSource.SourceSN}.pdf";
                System.IO.File.Copy($"{tempDirectory}\\{caseSource.SourceSN}.pdf", Common.PathHelper.VirtualPathToAbsolutePath(caseSource.SourceFile),true);
                
                
                await manager.SaveAsync(caseSource);
            }
            //新增数量和覆盖数量
            return new
            {
                newCount = caseSourceImportResults.Count(o => !o.Exist),
                overrideCount = caseSourceImportResults.Count(o => o.Exist)
            };
        }
        public virtual IEnumerable<string> ReadZip(string filePath)
        {
            var fileNames = Common.ZipHelper.GetFileNames(Common.PathHelper.VirtualPathToAbsolutePath(filePath));
            var fileDirectory = System.IO.Path.GetDirectoryName(Common.PathHelper.VirtualPathToAbsolutePath(filePath));
            Common.ZipHelper.Decompression(Common.PathHelper.VirtualPathToAbsolutePath(filePath), fileDirectory);
            return fileNames;
        }
        public virtual async Task<List<CaseSourceImportResult>> ReadExcel(string filePath)
        {
            var dataTable = Common.ExcelHelper.ReadExcelToDataTable(Common.PathHelper.VirtualPathToAbsolutePath(filePath), out var _);

            var result= ReadFromDataTable(dataTable);
            await ValidImportResult(result);
            return result;
        }
        /// <summary>
        /// 验证导入数据
        /// </summary>
        /// <param name="caseSourceImportResults"></param>
        /// <returns></returns>
        private async Task ValidImportResult(List<CaseSourceImportResult> caseSourceImportResults)
        {
            foreach(var importDto in caseSourceImportResults)
            {
                try
                {
                    await importDto.GenerateCaseSource();
                }catch(Exception ex)
                {
                    importDto.ErrMsg = ex.Message;
                    importDto.Valid = false;
                }
            }
        }
        private List<CaseSourceImportResult> ReadFromDataTable(DataTable dataTable)
        {
            //验证表头
            var needColumNames = new string[] { "案号", "城市", "一审法院", "二审法院", "案由", "代理律师", "审判人员", "裁判时间" };
            foreach(var needColumnName in needColumNames)
            {
                if (!dataTable.Columns.Contains(needColumnName))
                {
                    throw new UserFriendlyException($"列\"{needColumnName}\"未在表格中存在");
                }
            }
            var result = new List<CaseSourceImportResult>();
            foreach(DataRow row in dataTable.Rows)
            {
                if (!string.IsNullOrEmpty(row["案号"].ToString()))
                {
                    result.Add(ReadFromRow(row));
                }
                
            }
            return result;
        }
        private CaseSourceImportResult ReadFromRow(DataRow dataRow)
        {
            var result = new CaseSourceImportResult()
            {
                SourceSN = dataRow["案号"].ToString(),
                City = dataRow["城市"].ToString(),
                Court1 = dataRow["一审法院"].ToString(),
                Court2 = dataRow["二审法院"].ToString(),
                AnYou = dataRow["案由"].ToString(),
                Lawyer = dataRow["代理律师"].ToString(),
                TrialPeople = dataRow["审判人员"].ToString(),
                ValidDate = dataRow["裁判时间"].ToString(),
                //SourceFile = dataRow["裁判文件"].ToString()
            };
            return result;
        }
        #endregion

       

    }
}
