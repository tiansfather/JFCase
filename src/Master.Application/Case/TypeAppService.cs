using Abp.Authorization;
using Abp.AutoMapper;
using Master.BaseTrees;
using Master.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Case
{
    [AbpAuthorize]
    public class TypeAppService:MasterAppServiceBase<BaseTree,int>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<List<BaseTreeDto>> GetTypesByParentId(int id)
        {
            var types=await Resolve<BaseTreeManager>().FindChildrenAsync(id, "BaseType");
            
            return types.MapTo<List<BaseTreeDto>>();
        }

        public virtual async Task<List<BaseTreeDto>> GetTypesByParentName(string name)
        {
            var baseTree = await Resolve<BaseTreeManager>().GetByName(name, "BaseType");
            if (baseTree == null)
            {
                return new List<BaseTreeDto>();
            }
            return await GetTypesByParentId(baseTree.Id);
        }

        /// <summary>
        /// 获取案由
        /// </summary>
        /// <returns></returns>
        public virtual async Task<object> GetAnYous()
        {
            var nodes = await GetTypesByParentName("纠纷属性");
            return nodes.Select(o => new
            {
                o.Id,
                o.DisplayName
            });
        }
        /// <summary>
        /// 获取所有城市
        /// </summary>
        /// <returns></returns>
        public virtual async Task<object> GetCities()
        {
            var nodes = await GetTypesByParentName("审判组织");
            return nodes.Select(o => new
            {
                o.Id,
                o.DisplayName
            });
        }
        /// <summary>
        /// 获取城市对应的法院
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<object> GetCityCourts(int id)
        {
            var nodes = await GetTypesByParentId(id);
            return nodes.Select(o => new
            {
                o.Id,
                o.DisplayName
            });
        }
    }
}
