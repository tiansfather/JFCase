using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.UI;
using Master.Imports;
using Master.Json.Converters;
using Master.Projects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES.Dtos
{
    [AutoMap(typeof(Part))]
    public class PartDto: IImport
    {
        public int Id { get; set; }
        [DisplayName("物料编码")]
        public string MaterialCode { get; set; }
        [DisplayName("物料应到料时间")]
        public DateTime? RequireDate { get; set; }
        public string PartSN { get; set; }
        [DisplayName("零件名称")]
        public string PartName { get; set; }
        [DisplayName("规格")]
        public string PartSpecification { get; set; }
        [DisplayName("数量")]
        public int PartNum { get; set; }
        [DisplayName("材质")]
        public string Material { get; set; }
        [DisplayName("单位")]
        public string MeasureMentUnit { get; set; }
        [DisplayName("启用生产")]
        [JsonConverter(typeof(BoolConvert))]
        public bool EnableProcess { get; set; }
        public bool EnableBuy { get; set; }
        public bool EnableStorage { get; set; }
        public string PartImg { get; set; }
        public string MaterialStatus { get; set; }
        public int ProjectId { get; set; }

        public async Task Import(IDictionary<string, object> parameter)
        {
            var part = this.MapTo<Part>();
            //项目Id是直接传递过来的参数
            part.ProjectId = Convert.ToInt32(parameter["projectId"]);
            using (var scope = IocManager.Instance.CreateScope())
            {
                var partManager = scope.Resolve<PartManager>();
                var projectManager = scope.Resolve<ProjectManager>();
                var currentUnitOfWork = scope.Resolve<IUnitOfWorkManager>();
                var project = await projectManager.GetByIdFromCacheAsync(part.ProjectId);

                part.PartSN = partManager.GenerateNewPartSN(project);
                await partManager.InsertAsync(part);
                await currentUnitOfWork.Current.SaveChangesAsync();
            }
        }
    }
}
