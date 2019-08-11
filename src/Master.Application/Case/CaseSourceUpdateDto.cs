using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.Case
{
    [AutoMap(typeof(CaseSource))]
    public class CaseSourceUpdateDto
    {
        public int? Id { get; set; }
        public string SourceSN { get; set; }
        public int? AnYouId { get; set; }
        public int? CityId { get; set; }
        public int? Court1Id { get; set; }
        public int? Court2Id { get; set; }
        public DateTime ValidDate { get; set; }
        public string TrialPeopleField { get; set; }
        public string LawyerFirmsField { get; set; }
        public List<TrialPerson> TrialPeople
        {
            get;set;
        }
        public List<LawyerFirm> LawyerFirms
        {
            get;set;
        }
        /// <summary>
        /// 判例文件
        /// </summary>
        public string SourceFile { get; set; }
        public CaseSourceStatus CaseSourceStatus { get; set; }

        public void Normalize()
        {
            TrialPeopleField = string.Join(',', TrialPeople.Select(o => o.Name));
            LawyerFirmsField = string.Join(',', LawyerFirms.Select(o => o.FirmName + ":" + o.lawyer));
        }
    }
}
