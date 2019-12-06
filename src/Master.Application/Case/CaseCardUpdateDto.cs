using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    public class CaseCardUpdateDto
    {
        public int CaseInitialId { get; set; }
        public List<CaseCardDto> CaseCards { get; set; } = new List<CaseCardDto>();
    }
}
