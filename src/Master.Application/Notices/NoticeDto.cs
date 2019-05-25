using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Notices
{
    [AutoMap(typeof(Notice))]
    public class NoticeDto
    {
        public string NoticeTitle { get; set; }
        public string NoticeContent { get; set; }
        public bool IsActive { get; set; }
    }
}
