using Abp.Dependency;
using Abp.Extensions;
using Master.Configuration.Dictionaries;
using Master.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Module
{
    public class DefaultColumnReader : IColumnReader, ITransientDependency
    {
        private readonly IFileManager _fileManager;
        public DictionaryManager DictionaryManager { get; set; }

        public DefaultColumnReader(FileManager fileManager)
        {
            _fileManager = fileManager;
        }
        /// <summary>
        /// 从实体中读取对应列数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual Task Read(ColumnReadContext context)
        {
            return Task.Run(() => {
                try
                {
                    var columnInfo = context.ColumnInfo;
                    var entity = context.Entity;
                    var value = entity[columnInfo.ColumnKey];
                    //对于文件或图片类型字段，自动将文件信息加入
                    if (columnInfo.ColumnType == ColumnTypes.Images || columnInfo.ColumnType == ColumnTypes.Files)
                    {
                        var files = new List<File>();
                        var ids = value.ToString().Split(',');
                        foreach (var id in ids)
                        {
                            files.Add(_fileManager.GetByIdFromCacheAsync(int.Parse(id)).Result);
                        }
                        entity.SetValue(columnInfo.ColumnKey + "_Files", files.Select(o => new { o.FileName, o.FilePath, o.FileSize, o.Id }));
                    }
                    //对于日期类型自动设置格式化字符串
                    if (columnInfo.ColumnType == ColumnTypes.DateTime && columnInfo.DisplayFormat.IsNullOrWhiteSpace())
                    {
                        columnInfo.DisplayFormat = "yyyy-MM-dd";
                    }
                    //select类型的自动把显示值加入
                    if (columnInfo.ColumnType == ColumnTypes.Select && !string.IsNullOrEmpty(columnInfo.DictionaryName))
                    {
                        var allDictionary = DictionaryManager.GetAllDictionaries().Result;
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        if (allDictionary.ContainsKey(columnInfo.DictionaryName))
                        {
                            dic = allDictionary[columnInfo.DictionaryName];
                        }
                        else
                        {
                            try
                            {
                                dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(columnInfo.DictionaryName);
                            }
                            catch
                            {

                            }
                        }
                        string displayValue = dic.ContainsKey(value.ToString()) ? dic[value.ToString()] : value.ToString();
                        entity[columnInfo.ColumnKey + "_display"] = displayValue;
                    }
                    //进行值格式化
                    if (!columnInfo.DisplayFormat.IsNullOrWhiteSpace())
                    {

                        if (value != null)
                        {
                            //如果是日期类型
                            if (columnInfo.ColumnType == ColumnTypes.DateTime)
                            {

                                entity[columnInfo.ColumnKey] = Convert.ToDateTime(value).ToString(columnInfo.DisplayFormat);
                            }
                            //数值类型
                            if (columnInfo.ColumnType == ColumnTypes.Number)
                            {
                                entity[columnInfo.ColumnKey] = Convert.ToDecimal(value).ToString(columnInfo.DisplayFormat);
                            }
                        }


                    }
                }
                catch
                {

                }

            });


        }

    }
}
