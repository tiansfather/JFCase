using System;
using System.Collections.Generic;
using System.Text;

namespace Master
{
    public class UploadResult
    {
        public bool Success { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public string Msg { get; set; }
        public int FileId { get; set; }
    }
}
