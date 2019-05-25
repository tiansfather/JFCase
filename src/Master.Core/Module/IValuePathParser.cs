using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Module
{
    /// <summary>
    /// 值路径解析接口
    /// </summary>
    public interface IValuePathParser : ITransientDependency
    {
        string Parse(string valuePath);
    }
}
