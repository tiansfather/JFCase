using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 对账提交Dto
    /// </summary>
    public class SimpleFeeDto
    {
        public int Id { get; set; }
        public decimal Fee { get; set; }
    }
}
