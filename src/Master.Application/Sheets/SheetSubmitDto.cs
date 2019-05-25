using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Sheets
{
    public class SheetSubmitDto
    {
        public IDictionary<string,string> SheetHeader { get; set; }
        public List<IDictionary<string, string>> SheetValues { get; set; }
    }
}
