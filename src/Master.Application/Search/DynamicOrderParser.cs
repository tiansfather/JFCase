using Master.Entity;
using Master.EntityFrameworkCore;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Master.Search
{
    public class DynamicOrderParser : IDynamicOrderParser
    {
        public IQueryable Parse<TEntity>(string orderField, SortType sortType, ModuleInfo moduleInfo, IQueryable query)
            where TEntity : IHaveProperty
        {
            //提交过来的排序
            var column = moduleInfo.ColumnInfos.SingleOrDefault(o => o.ColumnKey.ToLower() == orderField.ToLower());
            if (column == null)
            {
                query = query.OrderBy($"{orderField} {sortType}");
                return query;
            }
            if (column.IsPropertyColumn)
            {
                if (sortType == SortType.Asc)
                {
                    query = (query as IQueryable<TEntity>).OrderBy(o => MasterDbContext.GetJsonValueString(o.Property, $"$.{column.ColumnKey}"));
                }
                else
                {
                    query = (query as IQueryable<TEntity>).OrderByDescending(o => MasterDbContext.GetJsonValueString(o.Property, $"$.{column.ColumnKey}"));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(column.DisplayPath))
                {
                    query = query.OrderBy($"{column.DisplayPath} {sortType}");
                }
                else
                {
                    query = query.OrderBy($"{column.ValuePath} {sortType}");
                }

            }

            return query;
        }


    }
}
