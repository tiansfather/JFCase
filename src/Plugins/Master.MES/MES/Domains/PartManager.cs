using Abp.UI;
using Master.Configuration;
using Master.Domain;
using Master.Projects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Runtime.Caching;
using Abp.Domain.Uow;
using Master.Templates;

namespace Master.MES
{
    public class PartManager: DomainServiceBase<Part, int>
    {
        //public TemplateManager TemplateManager { get; set; }
        private static object _obj = new object();

        /// <summary>
        /// 实体数据验证
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task ValidateEntity(Part entity)
        {
            if (string.IsNullOrEmpty(entity.PartName))
            {
                throw new UserFriendlyException(L("零件名称不能为空"));
            }

            if (entity.EnableBuy && entity.EnableStorage)
            {
                throw new UserFriendlyException("无法将物料同时设为采购及仓库");
            }
            //读取配置
            var enableDuplicatePartName = await SettingManager.GetSettingValueAsync<bool>(MESSettingNames.EnableDuplicatePartName);
            //不允许相同零件名称存在
            if (!enableDuplicatePartName && entity.Id>0 && await Repository.CountAsync(o => o.ProjectId == entity.ProjectId && o.PartName == entity.PartName && o.Id != entity.Id) > 0)
            {
                throw new UserFriendlyException(L("项目下相同零件名称已存在"));
            }
            if (!enableDuplicatePartName && entity.Id == 0 && await Repository.CountAsync(o => o.ProjectId == entity.ProjectId && o.PartName == entity.PartName ) > 0)
            {
                throw new UserFriendlyException(L("项目下相同零件名称已存在"));
            }
            await base.ValidateEntity(entity);
        }

        /// <summary>
        /// 通过零件名称、项目id获取零件，若不存在则新增
        /// </summary>        
        /// <returns></returns>
        [Obsolete]
        public virtual async Task<Part> GetByNameOrInsert(string partName,int projectId)
        {
            var part = await Repository.GetAll().Where(o => o.PartName == partName  && o.ProjectId==projectId).FirstOrDefaultAsync();
            if (part == null)
            {
                part = new Part()
                {
                    PartName=partName,
                    ProjectId=projectId
                };
                await Repository.InsertAsync(part);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            return part;
        }
        /// <summary>
        /// 生成新的零件
        /// </summary>
        /// <param name="project"></param>
        /// <param name="partName"></param>
        /// <param name="partSpecification"></param>
        /// <param name="partNum"></param>
        /// <returns></returns>
        public virtual async Task<Part> GenerateNewPart(Project project,string partName,string partSpecification,int partNum)
        {
            
            var part = new Part()
            {
                PartName = partName,
                PartSpecification=partSpecification,
                PartNum=partNum,
                ProjectId = project.Id
            };
            part.PartSN = GenerateNewPartSN(project);
            await InsertAsync(part);
            await CurrentUnitOfWork.SaveChangesAsync();

            return part;
        }
        /// <summary>
        /// 生成新的零件编号 
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public string GenerateNewPartSN(Project project)
        {
            var lastPart = Repository.GetAll().Where(o => o.ProjectId == project.Id).OrderBy(o => o.Id).LastOrDefault();
            if (lastPart == null)
            {
                return project.ProjectSN + "-001";
            }
            else
            {
                var lastPartNO =int.Parse( lastPart.PartSN.Split('-').Reverse().First());

                return project.ProjectSN + "-" + (lastPartNO + 1).ToString().PadLeft(3, '0');
            }
        }

        /// <summary>
        /// 通过零件编号获取零件
        /// </summary>
        /// <param name="partSN"></param>
        /// <returns></returns>
        public virtual async Task<Part> GetByPartSN(string partSN)
        {
            return await Repository.GetAll().Where(o => o.PartSN==partSN).FirstOrDefaultAsync();
        }

        public override IQueryable<Part> GetAll()
        {
            return base.GetAll().Include(o => o.ProcessTasks);
        }

        public virtual async Task<string> GetPartTemplate()
        {
            string templateContent;
            var template = await Resolve<TemplateManager>().GetAll().Where(o =>  o.TemplateType == MESTemplateSetting.TemplateType_PartSheet).FirstOrDefaultAsync();
            //如果找不到模板则使用默认模板
            if (template == null)
            {
                templateContent = GetDefaultTemplate();
            }
            else
            {
                templateContent = template.TemplateContent;
            }

            return templateContent;
        }

        /// <summary>
        /// 获取默认过程卡模板
        /// </summary>
        /// <returns></returns>
        public virtual string GetDefaultTemplate()
        {
            var templateContent = CacheManager.GetCache<string, string>("PartTemplate").Get("", o =>
                      Common.Fun.ReadEmbedString(typeof(ProcessTaskManager).Assembly, "Template.PartSheet.defaultTemplate.html")
                );
            return templateContent;
        }

        #region BOM树操作相关
        /// <summary>
        /// 移至目标节点内部
        /// </summary>
        /// <param name="fromNodeId"></param>
        /// <param name="toNodeId"></param>
        /// <returns></returns>
        public virtual async Task MoveInner(int fromNodeId,int toNodeId)
        {
            var fromNode = await GetByIdAsync(fromNodeId);
            var toNode = await GetByIdAsync(toNodeId);
            if (fromNode.ProjectId != toNode.ProjectId)
            {
                throw new UserFriendlyException(L("无法在不同项目间移动节点"));
            }
            fromNode.ParentId = toNodeId;
            fromNode.Sort = 10000;//默认加入后在最后一位
            await CurrentUnitOfWork.SaveChangesAsync();
            await ReSortChilds(toNode.ProjectId,toNode.Id);
        }
        /// <summary>
        /// 移至目标节点上面
        /// </summary>
        /// <param name="fromNodeId"></param>
        /// <param name="toNodeId"></param>
        /// <returns></returns>
        public virtual async Task MovePrev(int fromNodeId, int toNodeId)
        {
            var fromNode = await GetByIdAsync(fromNodeId);
            var toNode = await GetByIdAsync(toNodeId);
            if (fromNode.ProjectId != toNode.ProjectId)
            {
                throw new UserFriendlyException(L("无法在不同项目间移动节点"));
            }
            fromNode.ParentId = toNode.ParentId;
            //todo:此处排序还有bug
            fromNode.Sort = toNode.Sort - 1;
            await CurrentUnitOfWork.SaveChangesAsync();
            await ReSortChilds(toNode.ProjectId,toNode.ParentId);
        }
        /// <summary>
        /// 移至目标节点下面
        /// </summary>
        /// <param name="fromNodeId"></param>
        /// <param name="toNodeId"></param>
        /// <returns></returns>
        public virtual async Task MoveNext(int fromNodeId,int toNodeId)
        {
            var fromNode = await GetByIdAsync(fromNodeId);
            var toNode = await GetByIdAsync(toNodeId);
            if (fromNode.ProjectId != toNode.ProjectId)
            {
                throw new UserFriendlyException(L("无法在不同项目间移动节点"));
            }
            fromNode.ParentId = toNode.ParentId;
            //todo:此处排序还有bug
            fromNode.Sort = toNode.Sort + 1;
            await CurrentUnitOfWork.SaveChangesAsync();
            await ReSortChilds(toNode.ProjectId, toNode.ParentId);
        }
        /// <summary>
        /// 对某节点下所有子节点进行重排序
        /// </summary>
        /// <param name="parentNodeId"></param>
        /// <returns></returns>
        public virtual async Task ReSortChilds(int projectId,int? partId)
        {
            var nodes = await GetAll().Where(o => o.ParentId == partId && o.ProjectId==projectId).OrderBy(o => o.Sort).ToListAsync();
            for(var i = 0; i < nodes.Count; i++)
            {
                nodes[i].Sort = i + 1;
            }
        }
        #endregion
    }
}
