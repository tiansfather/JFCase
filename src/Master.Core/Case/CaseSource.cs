using Abp.Domain.Entities;
using Master.Authentication;
using Master.Entity;
using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Case
{
    /// <summary>
    /// 判例
    /// </summary>
    [InterModule("判例库")]
    public class CaseSource : BaseFullEntityWithTenant, IHaveStatus, IPassivable
    {
        [InterColumn(ColumnName ="案号",Sort =1, Templet = "<a onclick=\"showPdf('{{d.sourceFile}}','{{d.sourceSN}}')\" href=\"#_\" style=\"color:blue\">{{d.sourceSN}}</a>")]
        public string SourceSN { get; set; }
        
        [InterColumn(ColumnName = "城市", DisplayPath = "City.DisplayName", Templet = "{{d.cityId_display||''}}",Sort =2)]
        public int? CityId { get; set; }
        [ForeignKey("CityId")]
        public virtual BaseTree City { get; set; }
        [InterColumn(ColumnName = "一审法院", DisplayPath = "Court1.DisplayName", Templet = "{{d.court1Id_display||''}}",Sort =3)]
        public int? Court1Id { get; set; }
        [ForeignKey("Court1Id")]
        public virtual BaseTree Court1 { get; set; }
        [InterColumn(ColumnName = "二审法院", DisplayPath = "Court2.DisplayName", Templet = "{{d.court2Id_display||''}}",Sort =4)]
        public int? Court2Id { get; set; }
        [ForeignKey("Court2Id")]
        public virtual BaseTree Court2 { get; set; }
        [InterColumn(ColumnName = "案由", DisplayPath = "AnYou.DisplayName", Templet = "{{d.anYouId_display||''}}",Sort =5)]
        public int? AnYouId { get; set; }
        [ForeignKey("AnYouId")]
        public virtual BaseTree AnYou { get; set; }
        [InterColumn(ColumnName ="生效日期",ColumnType =Module.ColumnTypes.DateTime,Sort =6)]
        public DateTime ValidDate { get; set; }
        public string TrialPeopleField { get; set; }
        [NotMapped]
        public List<TrialPerson> TrialPeople
        {
            get
            {
                return this.GetPropertyValue<List<TrialPerson>>("TrialPeople");
            }
            set
            {
                this.SetPropertyValue("TrialPeople", value);
            }
        }
        public string LawyerFirmsField { get; set; }
        [NotMapped]
        public List<LawyerFirm> LawyerFirms
        {
            get
            {
                return this.GetPropertyValue<List<LawyerFirm>>("LawyerFirms");
            }
            set
            {
                this.SetPropertyValue("LawyerFirms", value);
            }
        }
        /// <summary>
        /// 判例文件
        /// </summary>
        //[InterColumn(ColumnName ="判例原件",Templet = "<button class=\"layui-btn layui-btn-xs\" onclick=\"showPdf('{{d.sourceFile}}','{{d.sourceSN}}')\">查看</button>",Sort =7)]        
        public string SourceFile { get; set; }
        public bool IsActive { get; set; } = true;
        public string Status { get; set; }
        [InterColumn(ColumnName ="状态",ColumnType =Module.ColumnTypes.Select,DictionaryName = "Master.Case.CaseSourceStatus",Templet = "{{d.caseSourceStatus_display}}",Sort =8)]
        public CaseSourceStatus CaseSourceStatus { get; set; }
        /// <summary>
        /// 当前所有人
        /// </summary>
        public long? OwerId { get; set; }
        public virtual User Ower { get; set; }
        public virtual ICollection<CaseSourceHistory> CaseSourceHistories { get; set; }
    }

    public enum CaseSourceStatus
    {
        下架=-1,
        待选,
        待加工,
        加工中,
        已加工
    }
}
