using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Master.Case;
using Master.Dto;
using Master.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.BaseTrees
{
    [AbpAuthorize]
    public class LabelAppService:MasterAppServiceBase<Label,int>
    {
        #region 后台管理
        protected override async Task<IQueryable<Label>> GetQueryable(RequestPageDto request)
        {
            return (await base.GetQueryable(request))
                .Include(o => o.CreatorUser);
        }
        protected override async Task<IQueryable<Label>> BuildKeywordQueryAsync(string keyword, IQueryable<Label> query)
        {
            return (await base.BuildKeywordQueryAsync(keyword, query))
                .Where(o => o.LabelName.Contains(keyword));
        }
        protected override async Task<IQueryable<Label>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<Label> query)
        {
            query = await base.BuildSearchQueryAsync(searchKeys, query);
            if (searchKeys.ContainsKey("anYou"))
            {
                var anYouId = int.Parse(searchKeys["anYou"]);
                //query = from label in query
                //        join treelabel in Resolve<IRepository<TreeLabel, int>>().GetAll() on label.Id equals treelabel.LabelId
                //        where treelabel.BaseTreeId == anYouId && false
                //        select label;
                query = query.Where(o => o.TreeLabels.Count(t => t.BaseTreeId == anYouId) > 0);
            }
            return query;

        }
        public virtual async Task Add(string name)
        {
            var label = new Label()
            {
                LabelName = name,
                LabelType = "标签"
            };
            await Manager.InsertAsync(label);
        }

        protected override object PageResultConverter(Label entity)
        {
            var baseTreeManager = Resolve<BaseTreeManager>();
            var nodes = (Manager as LabelManager).GetRelativeTreeNodes(entity.Id).Result;
            return new
            {
                entity.Id,
                entity.LabelName,
                entity.LabelType,
                entity.CreationTime,
                entity.Sort,
                Creator = entity.CreatorUser?.Name,
                RelativeNodeStrings = nodes.Select(o => baseTreeManager.GetNamesFromTopLevel(o).Result)
            };
        }

        public virtual async Task UpdateField(int id, string field, string value)
        {
            var entity = await Manager.GetByIdAsync(id);
            if (field == "labelName")
            {
                entity.LabelName = value;
            }
        }
        public virtual async Task SetLabelType(int id, string labelType)
        {
            var entity = await Manager.GetByIdAsync(id);
            entity.LabelType = labelType;
        } 
        /// <summary>
        /// 绑定标签至节点
        /// </summary>
        /// <param name="labelId"></param>
        /// <param name="nodeIds"></param>
        /// <returns></returns>
        public virtual async Task BindToNode(int labelId,IEnumerable<int> nodeIds)
        {
            var repository = Resolve<IRepository<TreeLabel, int>>();
            await repository.DeleteAsync(o => o.LabelId == labelId);
            foreach(var nodeId in nodeIds)
            {
                var treelabel = new TreeLabel()
                {
                    BaseTreeId = nodeId,
                    LabelId = labelId
                };
                await repository.InsertAsync(treelabel);
            }
        }

        /// <summary>
        /// 获取标签绑定的所有节点
        /// </summary>
        /// <param name="labelId"></param>
        /// <returns></returns>
        public virtual async Task<object> GetBindedNodes(int labelId)
        {
            var repository = Resolve<IRepository<TreeLabel, int>>();
            return await repository.GetAll().Where(o => o.LabelId == labelId)
                .Select(o => new
                {
                    Id=o.BaseTreeId
                }).ToListAsync();
        }

        public override async Task DeleteEntity(IEnumerable<int> ids)
        {
            //如果标签已被案例使用则不能删除
            if(await Resolve<IRepository<CaseLabel, int>>().CountAsync(o => ids.Contains(o.LabelId)) > 0){
                throw new UserFriendlyException("标签已被案例使用,无法删除");
            }
            await base.DeleteEntity(ids);
        }
        public virtual async Task SetSort(int id, string sortStr)
        {
            int sort = 0;
            if (int.TryParse(sortStr, out sort))
            {
                if (sort <= 0)
                {
                    throw new UserFriendlyException("排序值必须大于0");
                }
            }
            var label = await Manager.GetByIdAsync(id);
            label.Sort = sort;
        }
        #endregion
    }
}
