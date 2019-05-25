using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Imports
{
    public class ImportDto
    {
        public string Type { get; set; }
        public IDictionary<string, object> Parameter { get; set; }
        public List<IDictionary<string, object>> Data { get; set; }
    }
}
