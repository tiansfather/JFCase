using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Reflection;
using Castle.Core.Logging;
using Master.Configuration;
using Master.Domain;
using Master.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Master.Module
{
    public class RelativeDataParser : IRelativeDataParser
    {
        public ITypeFinder TypeFinder { get; set; }
        public ILogger Logger { get; set; }
        private IDynamicQuery _dynamicQuery;
        private MasterConfiguration _masterConfiguration;
        public RelativeDataParser(IDynamicQuery dynamicQuery,MasterConfiguration masterConfiguration)
        {
            _masterConfiguration = masterConfiguration;
            _dynamicQuery = dynamicQuery;
        }
        [UnitOfWork]
        public virtual async Task Parse(ModuleDataContext context)
        {
            try
            {
                //内置关联数据
                await ParseInnerData(context);

                //解析列自定义SQL数据
                await ParseSqlData(context);

                //解析模块关联数据
                await ParseModuleRelativeData(context);

                //遍历所有对应的关联数据列对实体进行数据赋值
                foreach (var column in context.ModuleInfo.ColumnInfos.Where(o => o.IsRelativeColumn && !string.IsNullOrEmpty(o.ValuePath) && o.ValuePath.StartsWith($"@")))
                {
                    var propertyName = column.ValuePath.Replace($"@", "");
                    context.Entity.TryGetValue(propertyName, out var val);
                    context.Entity.SetValue(column.ColumnKey, val);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }

        }
        /// <summary>
        /// 解析内置关联数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task ParseInnerData(ModuleDataContext context)
        {
            var entityType = TypeFinder.Find(o => o.FullName == context.ModuleInfo.EntityFullName)[0];
            //var entityType = Type.GetType(context.ModuleInfo.EntityFullName);

            if (_masterConfiguration.RelativeDataProviders.ContainsKey(entityType))
            {
                foreach (var providerType in _masterConfiguration.RelativeDataProviders[entityType])
                {
                    using (var wrapper = IocManager.Instance.ResolveAsDisposable(providerType))
                    {
                        var provider = wrapper.Object as IRelativeDataProvider;
                        await provider.FillRelativeData(context);
                    }

                }
            }
            //await Task.Run(() => {
                
            //});
        }
        /// <summary>
        /// 解析SQL关联数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task ParseSqlData(ModuleDataContext context)
        {
            await Task.Run(() => {
                foreach (var sqlColumn in context.ModuleInfo.ColumnInfos.Where(o => o.RelativeDataType == RelativeDataType.CustomSql))
                {
                    var dataResult = _dynamicQuery.Select(ParseDataString(sqlColumn.RelativeDataString, context.Entity));

                    //entity[relativeData.DataKey] = dataResult;
                    if (dataResult.Count() > 0)
                    {
                        var firstDataRow = dataResult.First() as IDictionary<string, object>;
                        foreach (var key in firstDataRow.Keys)
                        {
                            context.Entity.SetValue(key, firstDataRow[key]);
                        }

                    }
                }
            });
        }

        /// <summary>
        /// 解析模块关联数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task ParseModuleRelativeData(ModuleDataContext context)
        {
            var moduleInfoManager = context.ModuleInfoManager;

            foreach (var relativeColumn in context.ModuleInfo.ColumnInfos.Where(o => o.RelativeDataType == RelativeDataType.Module))
            {
                var moduleKey = relativeColumn.RelativeDataString;
                var moduleInfo = await moduleInfoManager.GetModuleInfo(moduleKey);
                if (moduleInfo != null)
                {
                    var ids = context.Entity[relativeColumn.ColumnKey]?.ToString();//关联的模块ids
                    if (!string.IsNullOrEmpty(ids))
                    {
                        //Id=1 or Id=2 or Id=3
                        var whereCondition = string.Join(" or ", ids.Split(',').Select(o => $"Id={o}"));
                        var relativeDatas = await moduleInfoManager.GetModuleDataListAsync(moduleInfo, whereCondition);

                        context.Entity.SetValue($"{relativeColumn.ColumnKey}_data", relativeDatas);
                    }
                    else
                    {
                        //没有关联数据则设置默认空数组
                        context.Entity.SetValue($"{relativeColumn.ColumnKey}_data", new List<object>());
                    }

                }

            }
        }

        /// <summary>
        /// 替换数据语句中的引用数据为当前实体中的数据
        /// select count(0) as SubNum from core_staffs where id>@id
        /// </summary>
        /// <param name="dataString"></param>
        /// <returns></returns>
        private string ParseDataString(string dataString, IDictionary<string, object> entity)
        {

            //select count(0) as SubNum from core_Staffs where id> entity["id"]
            dataString = dataString + " ";//兼容末尾无空格
            Regex r = new Regex("(?<=@).*?(?= )", RegexOptions.IgnoreCase);
            string result = "";
            var datearr = dataString.Split('@');
            string[] resulArr = new string[datearr.Length];
            resulArr[0] = datearr[0];
            for (int i = 0; i < datearr.Length - 1; i++)
            {
                resulArr[i + 1] = r.Replace("@" + datearr[i + 1], entity[r.Matches(dataString)[i].ToString()].ToString());
            }

            for (int i = 0; i < resulArr.Length; i++)
            {
                result += resulArr[i];
            }
            result = result.Replace("@", "");
            return result;
        }
    }
}
