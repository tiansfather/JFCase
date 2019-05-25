using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 加工点页面提交费用的参数
    /// </summary>
    public class SubmitFeeFromProcessorDto
    {
        public int TaskId { get; set; }
        public decimal Fee { get; set; }
        public string Price { get; set; }
        public string Num { get; set; }
        public string Info { get; set; }
        public List<UploadFileInfo> Files { get; set; }
    }
}
