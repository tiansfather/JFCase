using Abp.Domain.Repositories;
using Master.Domain;
using Master.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Case
{
    public class LabelManager:DomainServiceBase<Label,int>
    {
        /// <summary>
        /// 获取标签对应的树节点集合
        /// </summary>
        /// <param name="labelId"></param>
        /// <returns></returns>
        public virtual async Task<List<BaseTree>> GetRelativeTreeNodes(int labelId)
        {
            return await Resolve<IRepository<TreeLabel, int>>().GetAll().Include(o => o.BaseTree)
                .Where(o => o.LabelId == labelId)
                .Select(o=>o.BaseTree)
                .ToListAsync();
        }
    }
}
