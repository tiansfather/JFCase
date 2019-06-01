using Master.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    public class Label:BaseFullEntityWithTenant
    {
        public string LabelName { get; set; }
        public string LabelType { get; set; }
        public virtual ICollection<TreeLabel> TreeLabels { get; set; }
    }
}
