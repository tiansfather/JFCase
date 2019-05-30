using Abp.AutoMapper;
using Abp.UI;
using Master.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.BaseTrees
{
    public class BaseTreeAppService:MasterAppServiceBase<BaseTree,int>
    {
        public BaseTreeManager BaseTreeManager { get; set; }
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
            
            return nodes.Select(o =>
            {
                var dto = o.MapTo<BaseTreeDto>();

                return dto;
            }
            );
        }


        public virtual async Task<object> GetTreeJson(string discriminator,int? parentId, int maxLevel = 0)
        {
            var manager = Manager as BaseTreeManager;
            var ous = await manager.FindChildrenAsync(parentId, discriminator, true);
            if (maxLevel > 0)
            {
                ous = ous.Where(o => o.Code.ToCharArray().Count(c => c == '.') < maxLevel).ToList();
            }
            return ous.Select(o =>
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
        public virtual async Task<BaseTreeDto> GetBaseTree(int id)
        {
            var entity =await BaseTreeManager.GetByIdFromCacheAsync(id);
            return entity.MapTo<BaseTreeDto>();
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="baseTreeDto"></param>
        /// <returns></returns>
        public virtual async Task Submit(BaseTreeDto baseTreeDto)
        {
            BaseTree baseTree = null;
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
                    var childIds = (await BaseTreeManager.FindChildrenAsync(baseTree.Id,baseTree.Discriminator, true)).Select(o => o.Id).ToList();
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
        }

        
    }
}
