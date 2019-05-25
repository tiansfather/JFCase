using Master.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Templates
{
    public class TemplateManager : DomainServiceBase<Template, int>
    {
        public virtual async Task<IEnumerable<Template>> GetTemplatesByTemplateType(string templateType)
        {
            return await GetAll().Where(o => o.TemplateType == templateType).ToListAsync();
        }
    }
}
