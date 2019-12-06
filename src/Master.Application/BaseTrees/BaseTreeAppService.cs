using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using Master.Case;
using Master.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.BaseTrees
{
    public class BaseTreeAppService:MasterAppServiceBase<BaseTree,int>
    {
        public BaseTreeManager BaseTreeManager { get; set; }
        /// <summary>
        /// 获取知识树
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public virtual async Task<object> GetKnowledgeTreeJsonByParentId(int? parentId)
        {
            var manager = Manager as BaseTreeManager;
            var nodes = (await manager.GetAllList()).Where(o=>o.TreeNodeType==TreeNodeType.Knowledge || o.ParentId==null);
            if (parentId != null)
            {
                var parentNode = nodes.Where(o => o.Id == parentId.Value).SingleOrDefault();
                if (parentNode != null)
                {
                    nodes = nodes.Where(o => o.Code.StartsWith(parentNode.Code)).ToList();
                }
            }
            
            return nodes.OrderBy(o=>o.Sort).Select(o =>
            {
                var dto = o.MapTo<BaseTreeDto>();

                return dto;
            }
            );
        }
        /// <summary>
        /// 获取分类树
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public virtual async Task<object> GetTypeTreeJsonByParentId(int? parentId)
        {
            var manager = Manager as BaseTreeManager;
            var nodes = (await manager.GetAllList()).Where(o => o.TreeNodeType == TreeNodeType.Type || o.ParentId == null);
            if (parentId != null)
            {
                var parentNode = nodes.Where(o => o.Id == parentId.Value).SingleOrDefault();
                if (parentNode != null)
                {
                    nodes = nodes.Where(o => o.Code.StartsWith(parentNode.Code)).ToList();
                }
            }

            return nodes.OrderBy(o=>o.Sort).Select(o =>
            {
                var dto = o.MapTo<BaseTreeDto>();

                return dto;
            }
            );
        }

        /// <summary>
        /// 获取分类树实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<BaseTreeDto> GetNodeById(int id)
        {
            var entity =await BaseTreeManager.GetByIdFromCacheAsync(id);
            return entity.MapTo<BaseTreeDto>();
        }

        #region 提交
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="baseTreeDto"></param>
        /// <returns></returns>
        public virtual async Task<object> Submit(BaseTreeDto baseTreeDto)
        {
            BaseTree baseTree = null;
            if (string.IsNullOrWhiteSpace(baseTreeDto.DisplayName) || baseTreeDto.TreeNodeType == TreeNodeType.Knowledge)
            {
                baseTreeDto.DisplayName = baseTreeDto.Name;
            }
            if (baseTreeDto.Id == 0)
            {
                baseTree = baseTreeDto.MapTo<BaseTree>();
                await BaseTreeManager.CreateAsync(baseTree);
            }
            else
            {
                baseTree = await BaseTreeManager.GetByIdAsync(baseTreeDto.Id);
                //仅当父级变动
                if (baseTreeDto.ParentId != baseTree.ParentId)
                {
                    var childIds = (await BaseTreeManager.FindChildrenAsync(baseTree.Id,  true)).Select(o => o.Id).ToList();
                    if (baseTree.Id == baseTreeDto.ParentId)
                    {
                        throw new UserFriendlyException("不允许设置父级为自己");
                    }
                    else if (baseTreeDto.ParentId != null && childIds.Contains(baseTreeDto.ParentId.Value))
                    {
                        throw new UserFriendlyException("不允许设置父级为子级");
                    }
                    if (baseTreeDto.ParentId == null)
                    {
                        await BaseTreeManager.MoveAsync(baseTree.Id, null);
                    }
                    else
                    {
                        await BaseTreeManager.MoveAsync(baseTree.Id, baseTreeDto.ParentId.Value);
                    }
                }
                baseTreeDto.MapTo(baseTree);
                await BaseTreeManager.UpdateAsync(baseTree);                
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            return new
            {
                baseTree.Id,
                baseTree.ParentId,
                baseTree.Name,
                baseTree.DisplayName,
                baseTree.Code,
                baseTree.RelativeNodeId,
                baseTree.Sort,
                baseTree.TreeNodeType
            };
        } 
        #endregion

        /// <summary>
        /// 获取树节点的关联标签，并且获取标签关联的其它节点
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public virtual async Task<object> GetRelativeLabelsWithOtherReference(int nodeId)
        {
            var result = new List<object>();
            var labelManager = Resolve<LabelManager>();
            var manager = Manager as BaseTreeManager;
            var labels = await manager.GetRelativeLabels(nodeId);

            foreach(var label in labels)
            {
                var relativeNodes = (await labelManager.GetRelativeTreeNodes(label.Id)).Where(n => n.Id != nodeId);
                var relativeNodeStrings = new List<IEnumerable<string>>();
                foreach(var node in relativeNodes)
                {
                    var nodeNames = await manager.GetNamesFromTopLevel(node);
                    relativeNodeStrings.Add(nodeNames);
                }
                result.Add(new
                {
                    label.LabelName,
                    relativeNodeStrings
                });
            }

            return result;
        }

        /// <summary>
        /// 获取节点关联的标签
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public virtual async Task<object> GetRelativeLabels(int nodeId)
        {
            var labels = await (Manager as BaseTreeManager).GetRelativeLabels(nodeId);
            return labels.Select(o => new
            {
                o.Id,
                o.LabelName,
                o.LabelType
            });
        }

    }
}
