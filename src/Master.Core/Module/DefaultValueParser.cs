using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Module
{
    public class DefaultValueParser : IDefaultValueParser
    {
        public ILogger Logger { get; set; }

        public async Task<object> Parse(ColumnReadContext context)
        {
            var defaultValueString = context.ColumnInfo.DefaultValue;

            object result = "";
            //如果没有默认值设置
            if (!string.IsNullOrEmpty(defaultValueString) )
            {
                try
                {
                    result = await ScriptRunner.EvaluateScript(context.ColumnInfo.DefaultValue);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message,ex);
                    //出错后直接显示字符串
                    result = defaultValueString;
                }
                
            }

            return result;
            
        }
    }
}
