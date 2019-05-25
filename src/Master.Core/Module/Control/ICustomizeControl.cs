using System;
using System.Collections.Generic;
using System.Text;
using Abp.Dependency;

namespace Master.Module.Control
{
    /// <summary>
    /// 自定义控件接口
    /// </summary>
    public interface ICustomizeControl : IColumnReader, IColumnWriter, ITransientDependency
    {
        /// <summary>
        /// 获取对应的控件视图名称
        /// </summary>
        /// <returns></returns>
        string GetViewComponentName();
    }
}
