using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    public class CaseFineUpdateDto
    {
        public int CaseInitialId { get; set; }
        public List<CaseFineDto> CaseFines { get; set; } = new List<CaseFineDto>();
    }
}
