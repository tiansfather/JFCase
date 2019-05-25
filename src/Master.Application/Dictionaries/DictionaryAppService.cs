using System;
using System.Collections.Generic;
using System.Text;
using Abp.UI;
using Abp.Web.Models;
using Master.Configuration.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Dto;

namespace Master.Dictionaries
{
    

    public class DictionaryAppService : MasterAppServiceBase<Dictionary, int>
    {
        private IDictionaryManager _dictionaryManager;
        public DictionaryAppService(IDictionaryManager dictionaryManager)
        {
            _dictionaryManager = dictionaryManager;
        }
        /// <summary>
        /// 获取所有字典键集合
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<string>> GetAllKeysAsync()
        {
            return (await _dictionaryManager.GetAllDictionaries()).Keys.OrderBy(o => o);
        }
        /// <summary>
        /// 判断键是否是内置字典键
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> IsInInnerDics(string key)
        {
            return await _dictionaryManager.IsInInnerDic(key);
        }
        public virtual async Task AddDictionary(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new UserFriendlyException(L("字典名不能为空"));
            }
            await _dictionaryManager.AddDictionary(name);
        }
        public virtual async Task UpdateDictionary(int id, string dicContent)
        {
            var dic = await _dictionaryManager.GetByIdAsync(id);
            dic.DictionaryContent = dicContent;
        }
        /// <summary>
        /// 获取某用户字典内容
        /// </summary>
        /// <param name="dictionaryName"></param>
        /// <returns></returns>
        public virtual async Task<object> GetDictionary(string dictionaryName)
        {
            var dictionary = await _dictionaryManager.GetUserDicByNameAsync(dictionaryName);
            if (dictionary != null)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dictionary.DictionaryContent);
            }
            else
            {
                return new Dictionary<string, string>();
            }
            
        }
        [DontWrapResult]
        public override async Task<ResultPageDto> GetPageResult(RequestPageDto request)
        {
            var pageResult = await GetPageResultQueryable(request);

            var data = (await pageResult.Queryable.Include(o => o.CreatorUser).OrderByDescending(o => o.CreationTime).ToListAsync())
                .Select(o => new { o.Id, o.DictionaryName, Creator = o.CreatorUser.Name, CreationTime = o.CreationTime.ToString("yyyy-MM-dd HH:mm"), FieldCount = o.GetDictionary().Count });

            var result = new ResultPageDto()
            {
                code = 0,
                count = pageResult.RowCount,
                data = data
            };

            return result;
        }
    }

}
