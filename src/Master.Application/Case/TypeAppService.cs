using Abp.Authorization;
using Abp.AutoMapper;
using Abp.UI;
using Master.BaseTrees;
using Master.Entity;
using Microsoft.EntityFrameworkCore;
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
            var types=await Resolve<BaseTreeManager>().FindChildrenAsync(id);
            
            return types.MapTo<List<BaseTreeDto>>().OrderBy(o=>o.Sort).ToList();
        }
        public virtual async Task<List<BaseTreeDto>> GetTypesByKnowledgeName(string name)
        {
            var nodes = await (Manager as BaseTreeManager).GetTypeNodesByKnowledgeName(name);

            return nodes.MapTo<List<BaseTreeDto>>();
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

        #region 案由
        /// <summary>
        /// 获取案由
        /// </summary>
        /// <returns></returns>
        public virtual async Task<object> GetAnYous()
        {
            var nodes = await GetTypesByKnowledgeName("案由");
            return nodes.Select(o => new
            {
                o.Id,
                o.Name,
                o.DisplayName
            });
        }
        #endregion

        #region 城市
        /// <summary>
        /// 获取所有城市
        /// </summary>
        /// <returns></returns>
        public virtual async Task<object> GetCities()
        {
            var nodes = await GetTypesByKnowledgeName("城市");
            return nodes.Select(o => new
            {
                o.Id,
                o.Name,
                o.DisplayName
            });
        }
        #endregion

        #region 法院
        public virtual async Task<object> GetCourts()
        {
            var nodes = await GetTypesByKnowledgeName("法院");
            return nodes.Select(o => new
            {
                o.Id,
                o.ParentId,
                o.Name,
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
                o.Name,
                o.DisplayName
            });
        }
        #endregion

        #region 专题        
        public virtual async Task<object> GetSubjects()
        {
            var nodes = await GetTypesByKnowledgeName("专题");
            return nodes.Select(o => new
            {
                o.Id,
                o.Name,
                o.DisplayName
            });
        }
        #endregion

        #region 纠纷类型
        /// <summary>
        /// 获取案由下的纠纷类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<object> GetAnYouTypes(int id)
        {
            var nodes = await GetTypesByParentId(id);
            return nodes.Select(o => new
            {
                o.Id,
                o.Name,
                o.DisplayName
            });
        }

        #endregion


        public virtual async Task<object> GetTypeKeys(int id, string key)
        {
            var result = new List<object>();
            var nodes = await GetTypesByParentId(id);
            var currentNode = nodes.FirstOrDefault(o => o.DisplayName == key);
            if (currentNode != null)
            {
                var subNodes = await GetTypesByParentId(currentNode.Id);
                result.AddRange(subNodes.Select(o => new
                {
                    o.Id,
                    o.DisplayName
                }));
            }
            return result;
        }
    }
}
