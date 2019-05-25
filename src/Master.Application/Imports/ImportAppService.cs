using Abp.Domain.Uow;
using Abp.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Imports
{
    public class ImportAppService:MasterAppServiceBase
    {
        public ImportManager ImportManager { get; set; }
        public IFileManager FileManager { get; set; }
        /// <summary>
        /// 导入的提交方法，此方法禁用事务提交，改为内部代码手动控制
        /// </summary>
        /// <param name="importDto"></param>
        /// <returns></returns>
        [UnitOfWork(IsDisabled =true)]
        public virtual async Task<ImportResult> DoImport(ImportDto importDto)
        {
            using(var unitOfWork = UnitOfWorkManager.Begin())
            {
                var importType = ImportManager.GetImportType(importDto.Type);
                var result = new ImportResult();

                var rowIndex = 0;
                foreach (var rowData in importDto.Data)
                {
                    var resultDetail = new ImportResultDetail() { Row = rowIndex };
                    try
                    {
                        //先序列化再反序列化来获取对象
                        var serializedString = JsonConvert.SerializeObject(rowData);
                        var obj = JsonConvert.DeserializeObject(serializedString, importType) as IImport;
                        //调用实体的Import方法进行额外处理
                        await obj.Import(importDto.Parameter);
                        resultDetail.Success = true;
                    }
                    catch (ImportMsgException ex)
                    {
                        //自定义异常用来返回验证成功但是需要返回信息的情况
                        resultDetail.Success = true;
                        resultDetail.Message = ex.Message;
                    }
                    catch (JsonSerializationException ex)
                    {
                        //接收json错误，将错误字段名返回前台用于友好提示
                        resultDetail.Success = false;
                        resultDetail.FieldName = ex.Path;
                        resultDetail.Message = ex.Message;
                    }
                    catch (JsonReaderException ex)
                    {
                        //接收json错误，将错误字段名返回前台用于友好提示
                        resultDetail.Success = false;
                        resultDetail.FieldName = ex.Path;
                        resultDetail.Message = ex.Message;
                    }
                    catch (Exception ex)
                    {
                        resultDetail.Success = false;
                        resultDetail.Message = ex.Message;
                    }
                    rowIndex++;

                    result.ImportResultDetails.Add(resultDetail);
                }
                result.Success = !result.ImportResultDetails.Exists(o => !o.Success);
                //如果全部成功的则提交
                if (result.Success)
                {
                    unitOfWork.Complete();
                }

                return result;
            }
           
        }

        /// <summary>
        /// 从excel文件中读取数据供前台展示，其中图片以base64字符串形式返回
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public virtual async Task<List<string>> ReadFromExcel(int fileId)
        {
            try
            {
                var result = new List<string>();

                var file = await FileManager.GetByIdFromCacheAsync(fileId);
                var absolutePath = Common.PathHelper.VirtualPathToAbsolutePath(file.FilePath);
                var dataTable = Common.ExcelHelper.ReadExcelToDataTable(absolutePath, out var picturesInfos);

                //表头信息
                var columnName_arr = new List<string>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    columnName_arr.Add(column.ColumnName);

                }
                result.Add(string.Join('\t', columnName_arr));
                //数据信息
                for (var i = 0; i < dataTable.Rows.Count; i++)
                {
                    var row = dataTable.Rows[i];
                    var items = row.ItemArray.ToList();
                    //由于这里的索引是内容区的索引，而图片中的row对应的是excel表格中的行索引，故在比对时需要+1
                    foreach (var picture in picturesInfos.Where(o => o.MinRow == i + 1))
                    {
                        items[picture.MinCol] = "data:image/png;base64," + Convert.ToBase64String(picture.PictureData);
                    }
                    result.Add(string.Join('\t', items));
                }
                return result;
            }catch(Exception ex)
            {
                Logger.Error(ex.Message, ex);
                if(ex.Message== "Your stream was neither an OLE2 stream, nor an OOXML stream.")
                {
                    throw new UserFriendlyException("本系统只支持微软EXCEL格式，不支持WPS格式，请转存为Excel格式");
                }
                else
                {
                    throw new UserFriendlyException(ex.Message);
                }
                
            }
            
        }
    }
}
