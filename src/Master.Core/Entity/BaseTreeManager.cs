using Abp.Domain.Repositories;
using Abp.UI;
using Master.Case;
using Master.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Entity
{
    public class BaseTreeManager : DomainServiceBase<BaseTree, int>
    {
        /// <summary>
        /// 获取知识树节点对应的所有分类
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<BaseTree>> GetTypeNodesByKnowledgeName(string name)
        {
            var nodes = await GetAllList();
            var baseTree = nodes
                .Where(o => o.TreeNodeType == TreeNodeType.Knowledge)
                .Where(o => o.Name == name)
                .FirstOrDefault();

            if (baseTree == null)
            {
                throw new UserFriendlyException($"未找到{name}对应知识树节点");
            }

            return await GetTypeNodesByKnowledgeNode(baseTree);
        }
        public virtual async Task<IEnumerable<BaseTree>> GetTypeNodesByParentKnowledgeName(string name)
        {            

            var parentKnowledgeNode = await GetByName(name);
            if (parentKnowledgeNode == null)
            {
                throw new UserFriendlyException($"未找到{name}对应知识树节点");
            }
            var childNodes = await FindChildrenAsync(parentKnowledgeNode.Id);//初级法院二级法院三级法院

            var nodes = new List<BaseTree>();
            foreach (var childNode in childNodes)
            {
                nodes.AddRange(await GetTypeNodesByKnowledgeNode(childNode));
            }
            return nodes;
        }
        public virtual async Task<IEnumerable<BaseTree>> GetTypeNodesByKnowledgeNode(BaseTree knowledgeNode)
        {
            var nodes = await GetAllList();

            return nodes.Where(o => o.RelativeNodeId == knowledgeNode.Id);
        }
        public virtual async Task<IEnumerable<string>> GetNamesFromTopLevel(BaseTree node)
        {            
            var nodes = await GetNodesFromTopLevel(node);
            return nodes.Select(o => o.Name);
        }
        /// <summary>
        /// 获取从顶级到节点的所有层级节点
        /// </summary>
        public virtual async Task<IEnumerable<BaseTree>> GetNodesFromTopLevel(BaseTree node)
        {
            var result = new List<BaseTree>();
            var nodes = await GetAllList();
            var code = node.Code;
            result.Add(node);
            while (code.IndexOf('.') > 0)
            {
                code = code.Substring(0, code.LastIndexOf('.'));
                result.Add(nodes.Where(o => o.Code == code).FirstOrDefault());
            }            
            result.Reverse();
            return result;
        }
        /// <summary>
        /// 获取树节点关联的所有标签
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<Label>> GetRelativeLabels(int nodeId)
        {
            return await Resolve<IRepository<TreeLabel, int>>()
                .GetAll().Include(o => o.Label)
                .Where(o => o.BaseTreeId == nodeId)
                .Select(o => o.Label)
                .ToListAsync();
        }
        /// <summary>
        /// 通过树类型和节点名称获取树节点
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual async Task<BaseTree> GetByName(string name)
        {
            return await GetAll().Where(o => o.Name == name)
                .FirstOrDefaultAsync();
        }
        /// <summary>
        /// 新建树节点
        /// </summary>
        /// <param name="BaseTree"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(BaseTree BaseTree)
        {
            BaseTree.TenantId = AbpSession.TenantId;
            BaseTree.Code = await GetNextChildCodeAsync(BaseTree.ParentId,BaseTree.Discriminator);
            await ValidateBaseTreeAsync(BaseTree);
            await Repository.InsertAsync(BaseTree);
        }

        public override async Task UpdateAsync(BaseTree BaseTree)
        {
            await ValidateBaseTreeAsync(BaseTree);
            await Repository.UpdateAsync(BaseTree);
        }

        public virtual async Task<string> GetNextChildCodeAsync(int? parentId,string discriminator)
        {
            var lastChild = await GetLastChildOrNullAsync(parentId,discriminator);
            if (lastChild == null)
            {
                var parentCode = parentId != null ? await GetCodeAsync(parentId.Value) : null;
                return BaseTree.AppendCode(parentCode, BaseTree.CreateCode(1));
            }

            return BaseTree.CalculateNextCode(lastChild.Code);
        }

        public virtual async Task<BaseTree> GetLastChildOrNullAsync(int? parentId,string discriminator)
        {
            var children = await Repository.GetAllListAsync(ou => ou.ParentId == parentId && ou.Discriminator==discriminator);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }

        public virtual async Task<string> GetCodeAsync(int id)
        {
            return (await Repository.GetAsync(id)).Code;
        }

        public override async Task DeleteAsync(IEnumerable<int> ids)
        {
            var ous = await GetListByIdsAsync(ids);
            foreach (var ou in ous)
            {
                await DeleteAsync(ou);
            }
        }

        public override async Task DeleteAsync(BaseTree entity)
        {
            var children = await FindChildrenAsync(entity.Id, true);

            foreach (var child in children)
            {
                await DeleteAsync(child);
            }
            //验证是否可以删除
            var nodes = await GetAllList();
            if (nodes.Exists(o => o.RelativeNodeId == entity.Id))
            {
                throw new UserFriendlyException("节点已被分类树关联,无法删除");
            }
            if(await Resolve<IRepository<TreeLabel, int>>().CountAsync(o => o.BaseTreeId == entity.Id) > 0)
            {
                throw new UserFriendlyException("节点已被标签关联,无法删除");
            }
            if(await Resolve<CaseSourceManager>().GetAll().CountAsync(o=>o.AnYouId==entity.Id || o.CityId==entity.Id || o.Court1Id==entity.Id || o.Court2Id == entity.Id) > 0)
            {
                throw new UserFriendlyException("节点已被判例关联,无法删除");
            }
            await Resolve<IRepository<CaseNode, int>>().DeleteAsync(o => o.BaseTreeId == entity.Id);
            //if (await Resolve<IRepository<CaseNode, int>>().CountAsync(o => o.BaseTreeId == entity.Id) > 0)
            //{
            //    throw new UserFriendlyException("节点已被案例关联,无法删除");
            //}
            await Repository.DeleteAsync(entity.Id);
        }

        public virtual async Task MoveAsync(int id, int? parentId)
        {
            var BaseTree = await Repository.GetAsync(id);
            if (BaseTree.ParentId == parentId)
            {
                return;
            }

            //Should find children before Code change
            var children = await FindChildrenAsync(id, true);

            //Store old code of OU
            var oldCode = BaseTree.Code;

            //Move OU
            BaseTree.Code = await GetNextChildCodeAsync(parentId,BaseTree.Discriminator);
            BaseTree.ParentId = parentId;

            await ValidateBaseTreeAsync(BaseTree);

            //Update Children Codes
            foreach (var child in children)
            {
                child.Code = BaseTree.AppendCode(BaseTree.Code, BaseTree.GetRelativeCode(child.Code, oldCode));
            }
        }

        public async Task<List<BaseTree>> FindChildrenAsync(int? parentId,bool recursive = false)
        {
            if (!recursive)
            {
                return await Repository.GetAll().IgnoreQueryFilters().Where(ou =>!ou.IsDeleted && ou.ParentId == parentId && ou.TenantId==AbpSession.TenantId).OrderBy(ou => ou.Sort).ToListAsync();
            }

            if (!parentId.HasValue)
            {
                return await Repository.GetAll().IgnoreQueryFilters().Where(o=>!o.IsDeleted  && o.TenantId == AbpSession.TenantId).OrderBy(ou => ou.Sort).ToListAsync();
            }

            var code = await GetCodeAsync(parentId.Value);

            return await Repository.GetAll().Where(
                ou => ou.Code.StartsWith(code) && ou.Id != parentId.Value
            ).OrderBy(ou => ou.Sort).ToListAsync();
        }

        protected virtual async Task ValidateBaseTreeAsync(BaseTree BaseTree)
        {
            //if (BaseTree.Id == 0 && (await Repository.GetAll().IgnoreQueryFilters().Where(o => !o.IsDeleted && o.Discriminator == BaseTree.Discriminator && o.BriefCode == BaseTree.BriefCode && o.TenantId == AbpSession.TenantId).CountAsync()) > 0)
            //{
            //    throw new UserFriendlyException("编码重复");
            //}
            //if (BaseTree.Id > 0 && (await Repository.GetAll().IgnoreQueryFilters().Where(o => !o.IsDeleted && o.Discriminator == BaseTree.Discriminator && o.Id!=BaseTree.Id && o.BriefCode == BaseTree.BriefCode && o.TenantId == AbpSession.TenantId).CountAsync()) > 0)
            //{
            //    throw new UserFriendlyException("编码重复");
            //}
            var siblings = (await FindChildrenAsync(BaseTree.ParentId))
                .Where(ou => ou.Id != BaseTree.Id)
                .ToList();

            if (siblings.Any(ou => ou.Name == BaseTree.Name))
            {
                throw new UserFriendlyException("名称重复");
            }
        }
    }
}
