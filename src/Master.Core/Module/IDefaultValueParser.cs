using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;

namespace Master.Module
{
    public interface IDefaultValueParser:ITransientDependency
    {
        /// <summary>
        /// 解析列默认值
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<object> Parse(ColumnReadContext context);
    }
}
