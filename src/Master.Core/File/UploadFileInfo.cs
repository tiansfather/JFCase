using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master
{
    public class UploadFileInfo: IExtendableObject
    {
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public string FilePath { get; set; }
        public string ExtensionData { get;set; }
    }
}
