using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    /// <summary>
    /// 插件中实现用于提供实体标记
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IStatusProvider<TEntity> : IStatusProvider
    {
    }

    public interface IStatusProvider : ITransientDependency
    {
        IEnumerable<StatusDefinition> GetStatusDefinitions();
    }
}
