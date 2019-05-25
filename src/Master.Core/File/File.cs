using Abp.Domain.Entities;
using Master.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master
{
    public class File:BaseFullEntity,IMayHaveTenant
    {
        public virtual string FileName { get; set; }
        public virtual decimal FileSize { get; set; }
        public virtual string FilePath { get; set; }
        public int? TenantId { get;set; }
    }
}
