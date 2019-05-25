using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.UI;
using Master.Entity;
using Master.Imports;
using Master.Projects;
using Master.Units;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 项目导入Dto
    /// </summary>
    [AutoMap(typeof(Project))]
    public class MESProjectImportDto:IImport
    {
        [DisplayName("图片")]
        [ImportField(ColumnTypes =Module.ColumnTypes.Images)]
        public string ProjectPicString { get; set; }
        [DisplayName("客户")]
        public string CustomerName { get; set; }
        [DisplayName("模具编号")]
        [ImportField(Required = true)]
        public string ProjectSN { get; set; }
        [DisplayName("模具名称")]
        [ImportField(Required = true)]
        public string ProjectName { get; set; }
        [DisplayName("客户模具编号")]
        public string CustomerProjectSN { get; set; }
        [DisplayName("模具类别")]
        [ImportField(DictionaryName = StaticDictionaryNames.ProjectType)]
        public string ProjectType { get; set; }
        [DisplayName("数量")]
        [ImportField(Required = true)]
        public int Number { get; set; }
        [DisplayName( "下单日期")]
        public DateTime? OrderDate { get; set; }
        [DisplayName( "要求完成日期")]
        public DateTime? RequireDate { get; set; }
        [DisplayName("T0日期")]
        public DateTime? T0Date { get; set; }
        [DisplayName("模具组长")]
        public string ProjectCharger { get; set; } = "";
        [DisplayName("项目负责")]
        public string ProjectTracker { get; set; } = "";
        [DisplayName("产品设计 ")]
        public string ProductDesign { get; set; } = "";
        [DisplayName("模具设计")]
        public string MouldDesign { get; set; } = "";
        public async Task Import(IDictionary<string, object> parameter)
        {
            if (string.IsNullOrEmpty(ProjectSN))
            {
                throw new UserFriendlyException("模具编号不能为空");
            }
            using (var scope = IocManager.Instance.CreateScope())
            {
                var project = this.MapTo<Project>();
                var projectManager = scope.Resolve<IProjectManager>();
                var unitManager = scope.Resolve<IUnitManager>();
                var fileManager = scope.Resolve<IFileManager>();
                var currentUnitOfWork = scope.Resolve<IUnitOfWorkManager>();
                //验证模具编号
                //if(await projectManager.GetAll().CountAsync(o => o.ProjectSN == ProjectSN) > 0)
                //{
                //    throw new UserFriendlyException("相同模具编号已存在");
                //}
                //验证客户
                if (!string.IsNullOrEmpty(CustomerName))
                {
                    var customer = await unitManager.GetAll().Where(o => o.BriefName == CustomerName || o.UnitName == CustomerName).FirstOrDefaultAsync();
                    if (customer == null)
                    {
                        throw new UserFriendlyException("对应客户不存在");
                    }
                    project.UnitId = customer.Id;
                }
                //导入图片
                if(!string.IsNullOrEmpty(ProjectPicString) && ProjectPicString.StartsWith("data:image/png;"))
                {
                    var file = fileManager.UploadFile(ProjectPicString.Replace("data:image/png;base64,", "")).Result;
                    project.ProjectPic = file.Id;
                }
                project.SetPropertyValue("T0Date", this.T0Date);
                project.SetPropertyValue("ProjectCharger", this.ProjectCharger);
                project.SetPropertyValue("ProjectTracker", this.ProjectTracker);
                project.SetPropertyValue("ProductDesign", this.ProductDesign);
                project.SetPropertyValue("MouldDesign", this.MouldDesign);
                await projectManager.InsertAsync(project);
                await currentUnitOfWork.Current.SaveChangesAsync();
            }
        }
    }
}
