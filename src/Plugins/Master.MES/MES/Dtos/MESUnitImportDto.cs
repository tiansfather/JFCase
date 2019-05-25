using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.UI;
using Master.Imports;
using Master.Units;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES.Dtos
{
    [AutoMap(typeof(Unit))]
    public class MESUnitImportDto:IImport
    {
        [DisplayName("名称")]
        public virtual string UnitName { get; set; }
        [DisplayName("编号")]
        public virtual string UnitSN { get; set; }
        [DisplayName("性质(1:客户,2:供应商,3:客户及供应商)")]
        public virtual UnitNature UnitNature { get; set; }
        [DisplayName("供应类别")]
        [ImportField(DictionaryName =StaticDictionaryNames.SupplierType)]
        public virtual string SupplierType { get; set; }
        [DisplayName("简称")]
        public virtual string BriefName { get; set; }
        [DisplayName("国家")]
        public string Country { get; set; }
        [DisplayName("省份")]
        public string Province { get; set; }
        [DisplayName("城市")]
        public string District { get; set; }
        [DisplayName("地址")]
        public string Address { get; set; }
        [DisplayName("备注")]
        public string Remarks { get; set; }
        public async Task Import(IDictionary<string, object> parameter)
        {
            //if (string.IsNullOrEmpty(UnitName) || string.IsNullOrEmpty(UnitSN))
            //{
            //    throw new UserFriendlyException("单位名称及单位简称不能为空");
            //}
            using (var scope = IocManager.Instance.CreateScope())
            {
                var unit = this.MapTo<Unit>();
                var unitManager = scope.Resolve<IUnitManager>();
                var currentUnitOfWork = scope.Resolve<IUnitOfWorkManager>();
                await unitManager.InsertAsync(unit);
                await currentUnitOfWork.Current.SaveChangesAsync();
            }
        }
    }
}
