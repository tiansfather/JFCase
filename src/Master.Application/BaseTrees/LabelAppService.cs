using Abp.Authorization;
using Master.Case;
using Master.Dto;
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
        protected override async Task<IQueryable<Label>> GetQueryable(RequestPageDto request)
        {
            return (await base.GetQueryable(request))
                .Include(o=>o.CreatorUser);
        }
        protected override async Task<IQueryable<Label>> BuildKeywordQueryAsync(string keyword, IQueryable<Label> query)
        {
            return (await base.BuildKeywordQueryAsync(keyword, query))
                .Where(o=>o.LabelName.Contains(keyword));
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
            return new
            {
                entity.Id,
                entity.LabelName,
                entity.LabelType,
                entity.CreationTime,
                Creator=entity.CreatorUser?.Name
            };
        }

        public virtual async Task UpdateField(int id,string field,string value)
        {
            var entity = await Manager.GetByIdAsync(id);
            if (field == "labelName")
            {
                entity.LabelName = value;
            }
        }
        public virtual async Task SetLabelType(int id,string labelType)
        {
            var entity = await Manager.GetByIdAsync(id);
            entity.LabelType = labelType;
        }
    }
}
