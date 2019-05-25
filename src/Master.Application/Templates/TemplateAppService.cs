using Abp.Authorization;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Templates
{
    [AbpAuthorize]
    public class TemplateAppService : MasterAppServiceBase<Template, int>
    {
        /// <summary>
        /// 获取企业账套的所有加工单模板
        /// </summary>
        /// <param name="templateType"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        
        public virtual async Task<IEnumerable<TemplateDto>> GetTemplates(string templateType,int? tenantId)
        {
            var manager = Manager as TemplateManager;
            using (CurrentUnitOfWork.SetTenantId(tenantId??AbpSession.TenantId))
            {
                var templates = await manager.GetTemplatesByTemplateType(templateType);
                return templates.MapTo<List<TemplateDto>>();
            }
            //var tenant = await Manager.GetByIdAsync(tenantId);
            //return tenant.GetPropertyValue<string>("TemplateContent");
        }
        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="templateDto"></param>
        /// <returns></returns>
        public virtual async Task AddTemplate(TemplateDto templateDto,int? tenantId)
        {
            var template = templateDto.MapTo<Template>();
            template.TenantId = tenantId ?? AbpSession.TenantId;
            await Manager.InsertAsync(template);
        }
        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public virtual async Task DelTemplate(int templateId, int? tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId??AbpSession.TenantId))
            {
                await Manager.DeleteAsync(new int[] { templateId });
            }

        }
        /// <summary>
        /// 设置模板
        /// </summary>
        /// <param name="templateDto"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public virtual async Task UpdateTemplate(TemplateDto templateDto, int? tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId ?? AbpSession.TenantId))
            {
                var template = await Manager.GetByIdAsync(templateDto.Id);
                templateDto.MapTo(template);
                await Manager.UpdateAsync(template);
            }
                
        }
    }
}
