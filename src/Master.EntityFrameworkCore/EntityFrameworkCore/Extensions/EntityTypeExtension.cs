using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.EntityFrameworkCore
{
    public static class EntityTypeExtension
    {
        /// <summary>
        /// 获取一个类型对应的DbContext类型
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static Type GetEntityDbContextType(this Type entityType)
        {
            var dbContextType= entityType.Assembly.GetTypes()
                .Where(o => typeof(DbContext).IsAssignableFrom(o))
                .FirstOrDefault();

            if (dbContextType == null)
            {
                dbContextType = typeof(MasterDbContext);
            }

            return dbContextType;
        }
    }
}
