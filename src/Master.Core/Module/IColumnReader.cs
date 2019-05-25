using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Module
{
    public interface IColumnReader 
    {
        /// <summary>
        /// 读取列数据
        /// </summary>
        /// <param name="context"></param>
        /// <param name="columnInfo"></param>
        Task Read(ColumnReadContext context);
    }


}
