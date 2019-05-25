using Abp.AutoMapper;
using Master.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.BaseTypes
{
    public class BaseTypeAppService : MasterAppServiceBase<BaseType, int>
    {
        /// <summary>
        /// 获取对应类别的类型集合
        /// </summary>
        /// <param name="discriminator"></param>
        /// <returns></returns>
        public virtual async Task<List<BaseTypeDto>> GetTypesByDiscriminator(string discriminator)
        {
            var types=Manager.GetAll().Where(o => o.Discriminator == discriminator).ToListAsync();
            return types.MapTo<List<BaseTypeDto>>();
        }

        /// <summary>
        /// 获取分类实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<BaseTypeDto> GetBaseType(int id)
        {
            var entity = await Manager.GetByIdFromCacheAsync(id);
            return entity.MapTo<BaseTypeDto>();
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="baseTypeDto"></param>
        /// <returns></returns>
        public virtual async Task Submit(BaseTypeDto baseTypeDto)
        {
            BaseType baseType = null;
            if (baseTypeDto.Id == 0)
            {
                baseType = baseTypeDto.MapTo<BaseType>();
                baseType.TenantId = AbpSession.TenantId;
                await Manager.InsertAsync(baseType);
            }
            else
            {
                baseType = await Manager.GetByIdAsync(baseTypeDto.Id);                
                baseTypeDto.MapTo(baseType);
                await Manager.UpdateAsync(baseType);
            }
        }
    }
}
