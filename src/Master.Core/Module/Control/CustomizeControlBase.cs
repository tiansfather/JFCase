
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Module.Control
{
    /// <summary>
    /// 所有自定义控件基类
    /// </summary>
    public abstract class CustomizeControlBase : ICustomizeControl
    {
        /// <summary>
        /// 获取控件的视图名
        /// </summary>
        /// <returns></returns>
        public virtual string GetViewComponentName()
        {
            //默认取类名
            return this.GetType().Name.Replace("Control", "");
        }

        public abstract Task Read(ColumnReadContext context);

        public abstract Task Write(ColumnWriteContext context);
    }
}
