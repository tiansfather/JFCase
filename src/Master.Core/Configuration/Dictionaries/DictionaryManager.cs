using Abp.Runtime.Caching;
using Abp.UI;
using Master.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Configuration.Dictionaries
{
    public class DictionaryManager : DomainServiceBase<Dictionary, int>, IDictionaryManager
    {
        public MasterConfiguration MasterConfiguration { get; set; }
        private readonly ICacheManager _cacheManager;

        public DictionaryManager(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }
        public async Task<Dictionary<string, Dictionary<string, string>>> GetAllDictionaries()
        {
            var allDics = await GetUserDictionaries();
            //内置字典
            var innerDics = MasterConfiguration.Dictionaries;
            foreach (var dic in innerDics)
            {
                //如果用户字典中已包含了内置字典的键，则不加入内置字典
                if (!allDics.ContainsKey(dic.Key))
                {
                    allDics.Add(dic.Key, dic.Value);
                }
                
            }

            return allDics;

            
        }

        public async Task<Dictionary<string, Dictionary<string, string>>> GetUserDictionaries()
        {
            var key = AbpSession.TenantId ?? 0;
            return await _cacheManager.GetCache("UserDictionary").GetAsync(key, async () => {
                var allDics = new Dictionary<string, Dictionary<string, string>>();                

                //用户字典
                var userDics = await Repository.GetAllListAsync();
                foreach (var dic in userDics)
                {
                    
                    allDics.Add(dic.DictionaryName, dic.GetDictionary());
                }

                return allDics;
            });
        }

        public Task<bool> IsInInnerDic(string key)
        {
            return Task.FromResult(MasterConfiguration.Dictionaries.ContainsKey(key));
        }

        public async Task AddDictionary(string name)
        {
            if((await Repository.CountAsync(o => o.DictionaryName == name)) > 0)
            {
                throw new UserFriendlyException(L("相同字典名称已存在"));
            }
            var dic = new Dictionary()
            {
                DictionaryName = name,
                DictionaryContent="{}"
            };
            await Repository.InsertAsync(dic);
        }

        public async Task<Dictionary> GetUserDicByNameAsync(string name)
        {
            var dic = await Repository.GetAll().Where(o => o.DictionaryName == name).FirstOrDefaultAsync();
            return dic;
        }

    }
}
