using Abp.Authorization;
using Master.Application.Editions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES.Service
{
    [AbpAuthorize("Menu.MES.Host.Edition")]
    public class MESEditionAppService:MasterAppServiceBase<Edition,int>
    {
        public virtual async Task<object> GetAll()
        {
            var editions = await Manager.GetAll().Select(o => new { o.Id, o.DisplayName }).ToListAsync();
            return editions;
        }
        public async virtual Task AddEdition(string text,string key)
        {
            var edition = new Edition()
            {
                Name = key,
                DisplayName = text
            };
            await Manager.InsertAsync(edition);
        }

        public virtual async Task UpdateField(int editionId, string field, string value)
        {
            var edition = await Repository.GetAsync(editionId);
            switch (field)
            {
                case "name":
                    edition.Name = value;
                    break;
                case "displayName":
                    edition.DisplayName = value;
                    break;
            }
            await Manager.UpdateAsync(edition);
        }
    }
}
