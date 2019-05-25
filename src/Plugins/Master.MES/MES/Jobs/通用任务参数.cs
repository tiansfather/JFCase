using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Jobs
{
    [Serializable]
    public class SendWeiXinMessageJobArgs
    {
        public string OpenId { get; set; }
        public int DataId { get; set; }
        public int RemindLogId { get; set; }
        public string ExtendInfo { get; set; }
    }
}
