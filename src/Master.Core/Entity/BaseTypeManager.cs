using Abp.UI;
using Master.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Entity
{
    public class BaseTypeManager : DomainServiceBase<BaseType, int>
    {
        public override async Task<int> InsertAndGetIdAsync(BaseType entity)
        {
            await ValidateBaseTypeAsync(entity);
            return await base.InsertAndGetIdAsync(entity);
        }
        public override async Task InsertAsync(BaseType entity)
        {
            await ValidateBaseTypeAsync(entity);
            await base.InsertAsync(entity);
        }

        public override async Task UpdateAsync(BaseType entity)
        {
            await ValidateBaseTypeAsync(entity);
            await base.UpdateAsync(entity);
        }

        /// <summary>
        /// 验证数据有效性
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        protected virtual async Task ValidateBaseTypeAsync(BaseType baseType)
        {            
            
            if (baseType.Id==0 && GetAll().Any(o => o.Discriminator==baseType.Discriminator && (o.Code==baseType.Code || o.Name==baseType.Name)))
            {
                throw new UserFriendlyException(L("相同编码或名称已存在"));
            }else if(baseType.Id!=0 && GetAll().Any(o=>o.Discriminator==baseType.Discriminator && o.Id!=baseType.Id && (o.Code == baseType.Code || o.Name == baseType.Name)))
            {
                throw new UserFriendlyException(L("相同编码或名称已存在"));
            }
        }
    }
}
