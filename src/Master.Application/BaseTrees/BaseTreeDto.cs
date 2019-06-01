using Abp.AutoMapper;
using Master.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.BaseTrees
{
    [AutoMap(typeof(BaseTree))]
    public class BaseTreeDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int Sort { get; set; }
        public string Remarks { get; set; }
        public TreeNodeType TreeNodeType { get; set; }
        public bool EnableMultiSelect { get; set; }
        public int? RelativeNodeId { get; set; }
        public string Code { get; set; }
    }
}
