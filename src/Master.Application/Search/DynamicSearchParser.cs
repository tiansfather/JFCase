using Abp.AutoMapper;
using Castle.Core.Logging;
using Master.EntityFrameworkCore;
using Master.Module;
using Microsoft.CodeAnalysis.Scripting;
using SkyNet.Master.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;

namespace Master.Search
{
    public class DynamicSearchParser: IDynamicSearchParser
    {
        public ILogger Logger { get; set; }
        private readonly IModuleInfoManager _moduleInfoManager;
        public DynamicSearchParser(IModuleInfoManager moduleInfoManager)
        {
            _moduleInfoManager = moduleInfoManager;
        }
        #region 原有模块过滤
        public IQueryable Parse<TEntity>(string searchConditionStr, ModuleInfo moduleInfo, IQueryable query)
        {
            try
            {
                var searchConditions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SearchConditionDto>>(searchConditionStr);
                if (searchConditions.Count > 0)
                {
                    var predicate = GeneratePredicate(searchConditions);
                    var funcList = GenerateLamda<TEntity>(searchConditions, moduleInfo);

                    return query.Where(predicate, funcList);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }

            return query;
        }

        /// <summary>
        /// (@0(it) and @1(it) or @2(it))
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        private string GeneratePredicate(List<SearchConditionDto> conditions)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < conditions.Count; i++)
            {
                var condition = conditions[i];
                sb.Append($"{condition.LeftBracket} @{i}(it) {condition.RightBracket} {condition.Joiner}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 生成每个查询条件的lamda表达式
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="conditions"></param>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        private object[] GenerateLamda<TEntity>(List<SearchConditionDto> conditions, ModuleInfo moduleInfo)
        {
            //var op = ScriptOptions.Default;
            //op = op.WithImports(new string[] { "System", "System.Math", "Master.EntityFrameworkCore" });
            //op = op.WithReferences(typeof(ModuleInfo).Assembly, typeof(MasterDbContext).Assembly,typeof(TEntity).Assembly);

            List<object> funcList = new List<object>();
            foreach (var condition in conditions)
            {
                ColumnInfo column;
                //如果是模块查询，则使用模块定义中的列,否则直接使用前台传过来的列定义数据
                if (moduleInfo != null && !string.IsNullOrEmpty(condition.Column.ColumnKey))
                {
                    column = moduleInfo.ColumnInfos.Where(o => o.ColumnKey == condition.Column.ColumnKey).Single();
                }
                else
                {
                    column = condition.Column.MapTo<ColumnInfo>();
                }
                //string code = GenerateCode(condition, column);//动态编译构建
                string code2 = GenerateLamdaCode<TEntity>(condition, column, out var dbLamda);//动态lamda构建
                object func = null;
                //if (column.IsPropertyColumn)
                //{
                //    //todo:plugin下的实体不支持属性查询
                //    //目前动态lamda不支持调用dbcontext方法，但是动态编译也不支持plugin下的实体
                //    func = ScriptRunner.EvaluateScript<Expression<Func<TEntity, bool>>>(code, op).Result;
                //}
                //else
                //{
                //    func = DynamicExpressionParser.ParseLambda(typeof(TEntity), typeof(bool), code2, dbLamda);
                //}
                func = DynamicExpressionParser.ParseLambda(typeof(TEntity), typeof(bool), code2, dbLamda);
                //var func = ScriptRunner.EvaluateScript<Expression<Func<Staff, bool>>>("(m) =>MasterDbContext.GetJsonValueNumber(m.Property, \"$.Index\") > 5", op).Result;
                funcList.Add(func);
            }
            return funcList.ToArray();
        }
        /// <summary>
        /// 对属性列生成lamda表达式
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="columnType"></param>
        /// <param name="columnKey"></param>
        /// <returns></returns>
        public LambdaExpression GeneratePropertyLamda<TEntity>(ColumnTypes columnType, string columnKey)
        {
            var dbFuncName = GetDbFunctionName(columnType);
            var p = Expression.Parameter(typeof(TEntity), "x");
            var _pExp = Expression.Constant($"$.{columnKey}", typeof(string));
            MemberExpression member = Expression.PropertyOrField(p, "Property");
            var expRes = Expression.Call(typeof(TEntity).GetEntityDbContextType().GetMethod(dbFuncName), member, _pExp);
            return Expression.Lambda(expRes, p);
        }
        private string GenerateLamdaCode<TEntity>(SearchConditionDto condition, ColumnInfo column, out LambdaExpression dbLamda)
        {
            dbLamda = null;

            var value = condition.Value;
            var valuePath = $"{column.ValuePath}";

            if (column.IsPropertyColumn)
            {
                //自定义属性列的查询
                //var dbFuncName = GetDbFunctionName(column.ColumnType);
                //var p = Expression.Parameter(typeof(TEntity), "x");
                //var _pExp = Expression.Constant($"$.{column.ColumnKey}", typeof(string));
                //MemberExpression member = Expression.PropertyOrField(p, "Property");
                //var expRes = Expression.Call(typeof(TEntity).GetEntityDbContextType().GetMethod(dbFuncName), member, _pExp);
                //dbLamda = Expression.Lambda(expRes, p);

                dbLamda = GeneratePropertyLamda<TEntity>(column.ColumnType, column.ColumnKey);

                //valuePath = $"MasterDbContext.{dbFuncName}(Property,\"$.{column.ColumnKey}\")";
                //基于数据库json字段的查询需要动态构建lamda
                valuePath = "@0(it)";//@0之后会由dbLamda代入
            }


            if (column.ColumnType != ColumnTypes.Number)
            {
                value = $"\"{value}\"";
            }
            if (column.ColumnType == ColumnTypes.DateTime)
            {
                //日期类型需要转换
                value = $"DateTime.Parse({value})";
            }
            //以.开头的为字符串操作
            if (condition.Operator.StartsWith("."))
            {
                //加括号:a.StartsWith("b")
                value = $"({value})";
            }
            else if (string.IsNullOrEmpty(condition.Value))
            {
                //非字符串操作，如果未传值，作null处理
                value = "null";
            }
            return $"{valuePath}{condition.Operator}{value}";
        }
        #endregion

        #region SoulTable方式过滤
        private int _soulLamdaIndex = 0;//存储动态lamda索引
        public IQueryable ParseSoulTable<TEntity>(string filterSos, ModuleInfo moduleInfo, IQueryable query)
        {
            try
            {
                var searchConditions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SoulTableConditionDto>>(filterSos);
                if (searchConditions.Count > 0)
                {
                    var predicate = GeneratePredicate(searchConditions);
                    var funcList = GenerateLamda<TEntity>(searchConditions, moduleInfo);

                    return query.Where(predicate, funcList);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }

            return query;
        }
        /// <summary>
        /// (@0(it) and @1(it) or @2(it))
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        private string GeneratePredicate(List<SoulTableConditionDto> conditions)
        {
            var sb = new StringBuilder();
            foreach(var condition in conditions)
            {
                var currentIndex = conditions.IndexOf(condition);
                var prefix = currentIndex > 0 ? condition.Prefix : "";
                if (condition.Children!=null && condition.Children.Count > 0)
                {
                    sb.Append($" {prefix}({GeneratePredicate(condition.Children)})");
                }
                else
                {
                    sb.Append($" {prefix} @{_soulLamdaIndex}(it)");
                    _soulLamdaIndex++;
                }
            }            
            return sb.ToString();
        }
        /// <summary>
        /// 生成每个查询条件的lamda表达式
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="conditions"></param>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        private object[] GenerateLamda<TEntity>(List<SoulTableConditionDto> conditions, ModuleInfo moduleInfo)
        {
            //var op = ScriptOptions.Default;
            //op = op.WithImports(new string[] { "System", "System.Math", "Master.EntityFrameworkCore" });
            //op = op.WithReferences(typeof(ModuleInfo).Assembly, typeof(MasterDbContext).Assembly,typeof(TEntity).Assembly);

            List<object> funcList = new List<object>();
            foreach (var condition in conditions)
            {
                ColumnInfo column;
                column = moduleInfo.ColumnInfos.Where(o => o.ColumnKey.ToLower() == condition.Field?.ToLower()).Single();
                if (condition.Children!=null && condition.Children.Count > 0)
                {
                    funcList.AddRange(GenerateLamda<TEntity>(condition.Children, moduleInfo));
                }
                else
                {
                    string code2 = GenerateLamdaCode<TEntity>(condition, column, out var dbLamda);//动态lamda构建
                    funcList.Add(DynamicExpressionParser.ParseLambda(typeof(TEntity), typeof(bool), code2, dbLamda));
                }
                
            }
            return funcList.ToArray();
        }
        private string GenerateLamdaCode<TEntity>(SoulTableConditionDto condition, ColumnInfo column, out LambdaExpression dbLamda)
        {
            dbLamda = null;

            var value = condition.Value;
            var values = condition.Values;
            var valuePath = $"{column.ValuePath}";
            if (!string.IsNullOrEmpty(column.DisplayPath))
            {
                valuePath = column.DisplayPath;
            }
            if (column.IsPropertyColumn)
            {
                //自定义属性列的查询
                //var dbFuncName = GetDbFunctionName(column.ColumnType);
                //var p = Expression.Parameter(typeof(TEntity), "x");
                //var _pExp = Expression.Constant($"$.{column.ColumnKey}", typeof(string));
                //MemberExpression member = Expression.PropertyOrField(p, "Property");
                //var expRes = Expression.Call(typeof(TEntity).GetEntityDbContextType().GetMethod(dbFuncName), member, _pExp);
                //dbLamda = Expression.Lambda(expRes, p);

                dbLamda = GeneratePropertyLamda<TEntity>(column.ColumnType, column.ColumnKey);

                //valuePath = $"MasterDbContext.{dbFuncName}(Property,\"$.{column.ColumnKey}\")";
                //基于数据库json字段的查询需要动态构建lamda
                valuePath = "@0(it)";//@0之后会由dbLamda代入
            }

            Func<string, string> valueWrapp = inValue =>
            {
                string outValue=inValue;
                if (column.ColumnType != ColumnTypes.Number)
                {
                    outValue = $"\"{inValue}\"";
                }
                if (column.ColumnType == ColumnTypes.DateTime)
                {
                    //日期类型需要转换
                    outValue = $"DateTime.Parse({outValue})";
                }
                return outValue;
            };

            value = valueWrapp(value);
            
            string code = "";
            if (condition.Mode == "in")
            {
                //如果未传入筛选数据数组，直接返回
                code =values.Count==0?"1=1": string.Join(" Or ", values.Select(o => $"{valuePath}={valueWrapp(o)}"));
            }
            else if (condition.Mode == "date")
            {
                //日期的筛选
                if (condition.Type == "all")
                {
                    code = "1=1";
                }
                else if (condition.Type == "specific")
                {
                    code = $"{valuePath}={value}";
                }
                else
                {
                    var dateTimeValues = GetDateTimeValuesByIdentifier(condition.Type);
                    code = $"{valuePath}>={valueWrapp(dateTimeValues.startDate.ToString())} && {valuePath}<={valueWrapp(dateTimeValues.endDate.ToString())}";
                }
            }
            else
            {
                switch (condition.Type)
                {
                    case "eq":
                        code = $"{valuePath}={value}";
                        break;
                    case "ne":
                        code = $"{valuePath}!={value}";
                        break;
                    case "gt":
                        code = $"{valuePath}>{value}";
                        break;
                    case "ge":
                        code = $"{valuePath}>={value}";
                        break;
                    case "lt":
                        code = $"{valuePath}<{value}";
                        break;
                    case "le":
                        code = $"{valuePath}<={value}";
                        break;
                    case "contain":
                        code = $"{valuePath}.Contains({value})";
                        break;
                    case "notContain":
                        code = $"!{valuePath}.Contains({value})";
                        break;
                    case "start":
                        code = $"{valuePath}.StartWith({value})";
                        break;
                    case "end":
                        code = $"{valuePath}.EndWith({value})";
                        break;
                    case "null":
                        code = $"{valuePath}=null";
                        break;
                    case "notNull":
                        code = $"{valuePath}!=null";
                        break;
                }
            }
            
            return code;
        }
        /// <summary>
        /// yesterday,thisWeek,thisMonth==>2019-01-01---2019-01-08
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        private (DateTime startDate,DateTime endDate) GetDateTimeValuesByIdentifier(string identifier)
        {
            var nowZeroDate =DateTime.Parse( DateTime.Now.ToString("yyyy-MM-dd"));//当天0点
            int daydiff = (int)nowZeroDate.DayOfWeek - 1 < 0 ? 6 : (int)nowZeroDate.DayOfWeek - 1;//如果是0结果小于0表示周日 那最后要减6天:其他天数在dayOfWeek上减1，表示回到周一
            var weekFirstDay = nowZeroDate.AddDays(-daydiff);
            switch (identifier)
            {
                case "yesterday":
                    return (nowZeroDate.AddDays(-1), nowZeroDate);
                case "thisWeek":
                    var weekLastDay = weekFirstDay.AddDays(7);
                    return (weekFirstDay, weekLastDay);
                case "lastWeek":
                    var lastWeekFirstDay = weekFirstDay.AddDays(-7);
                    return (lastWeekFirstDay, weekFirstDay);
                case "thisMonth":
                    var monthFirstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var monthLastDay = monthFirstDay.AddMonths(1);
                    return (monthFirstDay, monthLastDay);
                case "thisYear":
                    var yearFirstDay= new DateTime(DateTime.Now.Year, 1, 1);
                    var yearLastDay = yearFirstDay.AddYears(1);
                    return (yearFirstDay, yearLastDay);
                default:
                    return (DateTime.Now.AddYears(-5), DateTime.Now);
            }
        }
        #endregion

        #region 已弃用
        /// <summary>
        /// 构建lamda表达式的语句
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private string GenerateCode(SearchConditionDto condition, ColumnInfo column)
        {
            var value = condition.Value;
            var valuePath = $"o.{column.ValuePath}";

            if (column.IsPropertyColumn)
            {
                //自定义属性列的查询
                var dbFuncName = GetDbFunctionName(column.ColumnType);
                valuePath = $"MasterDbContext.{dbFuncName}(o.Property,\"$.{column.ColumnKey}\")";
            }


            if (column.ColumnType != ColumnTypes.Number)
            {
                value = $"\"{value}\"";
            }
            if (column.ColumnType == ColumnTypes.DateTime)
            {
                //日期类型需要转换
                value = $"DateTime.Parse({value})";
            }
            //以.开头的为字符串操作
            if (condition.Operator.StartsWith("."))
            {
                //加括号:a.StartsWith("b")
                value = $"({value})";
            }
            else if (string.IsNullOrEmpty(condition.Value))
            {
                //非字符串操作，如果未传值，作null处理
                value = "null";
            }
            return $"(o)=>{valuePath}{condition.Operator}{value}";
        }
        #endregion


        /// <summary>
        /// 获取列类型对应的自定义属性数据库函数名称
        /// </summary>
        /// <param name="columnType"></param>
        /// <returns></returns>
        private string GetDbFunctionName(ColumnTypes columnType)
        {
            string result;
            switch (columnType)
            {
                case ColumnTypes.DateTime:
                    result = nameof(MasterDbContext.GetJsonValueDate);
                    break;
                case ColumnTypes.Number:
                    result = nameof(MasterDbContext.GetJsonValueNumber);
                    break;
                case ColumnTypes.Switch:
                    result = nameof(MasterDbContext.GetJsonValueBool);
                    break;
                default:
                    result = nameof(MasterDbContext.GetJsonValueString);
                    break;
            }
            return result;
        }
    }
}
