using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Module
{
    public interface IColumnWriter
    {
        Task Write(ColumnWriteContext context);
    }
}
