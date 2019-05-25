using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Imports
{
    /// <summary>
    /// 支持导入的类的基类
    /// </summary>
    public interface IImport
    {
        Task Import(IDictionary<string, object> parameter);
    }
}
