using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Module
{
    /// <summary>
    /// 关联数据解析接口
    /// </summary>
    public interface IRelativeDataParser : ITransientDependency
    {
        Task Parse(ModuleDataContext context);
    }
}
