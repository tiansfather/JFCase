using Abp.AutoMapper;
using Master.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.BaseTypes
{
    [AutoMap(typeof(BaseType))]
    public class BaseTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Sort { get; set; }
        public string Remarks { get; set; }
        public string Discriminator { get; set; }
    }
}
