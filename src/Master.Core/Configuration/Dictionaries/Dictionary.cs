using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Master.Authentication;
using Master.Entity;
using Master.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Configuration.Dictionaries
{
    /// <summary>
    /// 字典
    /// </summary>
    public class Dictionary: CreationAuditedEntity<int>,IMustHaveTenant
    {
        public virtual string DictionaryName { get; set; }
        public virtual string DictionaryContent { get; set; }

        public virtual Dictionary<string,string> GetDictionary()
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(DictionaryContent);
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }
        [ForeignKey("CreatorUserId")]
        public virtual User CreatorUser { get; set; }
        [ForeignKey("LastModifierUserId")]
        public virtual User LastModifierUser { get; set; }
        [ForeignKey("DeleterUserId")]
        public virtual User DeleterUser { get; set; }
        public int TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}
