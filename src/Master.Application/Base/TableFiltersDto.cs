using Abp.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Base
{
    public class FilterColumnDto
    {
        public string ColumnName { get; set; }
        public List<FilterColumnConditionDto> Conditions { get; set; }
        public string GetWhereStr()
        {
            var resultWhere = "";
            var index = 0;
            foreach (var conditonDto in Conditions)
            {
                var where = "";
                switch (conditonDto.Comparer)
                {
                    case "=":
                        where = $"{ColumnName}=\"{conditonDto.Value}\"";
                        break;
                    case "!=":
                        where = $"{ColumnName}!=\"{conditonDto.Value}\"";
                        break;
                    case ">":
                        where = $"{ColumnName}>\"{conditonDto.Value}\"";
                        break;
                    case "<":
                        where = $"{ColumnName}><\"{conditonDto.Value}\"";
                        break;
                    case ">=":
                        where = $"{ColumnName}>=\"{conditonDto.Value}\"";
                        break;
                    case "<=":
                        where = $"{ColumnName}<=\"{conditonDto.Value}\"";
                        break;
                    case "%":
                        where = $"{ColumnName}.Contains(\"{conditonDto.Value}\")";
                        break;
                    case "_%":
                        where = $"{ColumnName}.StartWith(\"{conditonDto.Value}\")";
                        break;
                    case "%_":
                        where = $"{ColumnName}.EndWith(\"{conditonDto.Value}\")";
                        break;
                }
                //多个连接条件如果前台未传入连接符,则以or 连接
                if(index>0 && conditonDto.AndOr.IsNullOrWhiteSpace())
                {
                    conditonDto.AndOr = "Or";
                }
                resultWhere += $" {conditonDto.AndOr} {conditonDto.LeftBracket} {where} {conditonDto.RightBracket} ";
                index++;
            }

            return resultWhere;
        }
    }

    public class FilterColumnConditionDto
    {
        public string AndOr { get; set; }
        public string LeftBracket { get; set; }
        public string Comparer { get; set; } = "=";
        public string Value { get; set; }
        public string RightBracket { get; set; }
    }
}
