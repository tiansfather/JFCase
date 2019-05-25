using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Module
{
    public interface IRelativeDataProvider<TEntity> : IRelativeDataProvider
    {

    }
    public interface IRelativeDataProvider : ITransientDependency
    {
        Task FillRelativeData(ModuleDataContext context);
    }
}
