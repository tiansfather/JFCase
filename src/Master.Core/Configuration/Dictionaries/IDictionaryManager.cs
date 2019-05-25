using Master.Configuration.Dictionaries;
using Master.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Configuration.Dictionaries
{
    public interface IDictionaryManager : IData<Dictionary, int>
    {
        /// <summary>
        /// 获取所有字典，包括内置字典及用户自定义字典
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, Dictionary<string, string>>> GetAllDictionaries();
        /// <summary>
        /// 获取所有用户自定义字典
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, Dictionary<string, string>>> GetUserDictionaries();
        /// <summary>
        /// 是否在内置字典键中
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> IsInInnerDic(string key);
        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task AddDictionary(string name);

        Task<Dictionary> GetUserDicByNameAsync(string name);
    }
}
