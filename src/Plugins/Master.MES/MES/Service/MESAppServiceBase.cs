using Abp.Domain.Entities;
using Abp.Runtime.Session;
using Master.Configuration;
using Master.MES.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master
{
    public class MESAppServiceBase<TEntity, TPrimary> : MasterAppServiceBase<TEntity, TPrimary>
        where TEntity : class, IEntity<TPrimary>, new()
    {
        #region 高级查询
        #region 保存模板数据
        public virtual async Task SaveSearchData(SearchDataSave searchDataSave)
        {
            List<SearchDataSave> searchDataSaves = new List<SearchDataSave>();
            string data = await SettingManager.GetSettingValueForUserAsync(MESSettingNames.SearchTemplate, AbpSession.ToUserIdentifier());
            if(string.IsNullOrEmpty(data))
            {
                searchDataSaves.Add(searchDataSave);
            }
            else
            {
                searchDataSaves = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SearchDataSave>>(data);

                var flag = true;

                for(var i=0;i< searchDataSaves.Count;i++)
                {
                    if (searchDataSaves[i].Name == searchDataSave.Name&& searchDataSaves[i].Key== searchDataSave.Key&& searchDataSaves[i].PageName== searchDataSave.PageName)
                    {
                        searchDataSaves[i] = searchDataSave;
                        flag = false;
                    }
                }
                if(flag)
                {
                    searchDataSaves.Add(searchDataSave);
                }
            }
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), MESSettingNames.SearchTemplate, Newtonsoft.Json.JsonConvert.SerializeObject(searchDataSaves));

        }
        #endregion

        #region 删除模板数据
        public virtual async Task DelSearchData(SearchDataSave searchDataSave)
        {
            List<SearchDataSave> searchDataSaves = new List<SearchDataSave>();

            List<SearchDataSave> usearchDataSaves= new List<SearchDataSave>();

            string data = await SettingManager.GetSettingValueForUserAsync(MESSettingNames.SearchTemplate, AbpSession.ToUserIdentifier());

            if (string.IsNullOrEmpty(data))
            {
                //searchDataSaves.Add(searchDataSave);
            }
            else
            {
                searchDataSaves = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SearchDataSave>>(data);
                

                for (var i = 0; i < searchDataSaves.Count; i++)
                {
                    if (searchDataSaves[i].Name == searchDataSave.Name && searchDataSaves[i].Key == searchDataSave.Key && searchDataSaves[i].PageName == searchDataSave.PageName)
                    {
                        
                    }
                    else
                    {
                        usearchDataSaves.Add(searchDataSaves[i]);
                    }
                }
            }
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), MESSettingNames.SearchTemplate, Newtonsoft.Json.JsonConvert.SerializeObject(usearchDataSaves));

        }
        #endregion

        #region 重命名

        public virtual async Task RNameSearchData(SearchDataSave searchDataSave)
        {
            List<SearchDataSave> searchDataSaves = new List<SearchDataSave>();

            string data = await SettingManager.GetSettingValueForUserAsync(MESSettingNames.SearchTemplate, AbpSession.ToUserIdentifier());

            if (string.IsNullOrEmpty(data))
            {
            }
            else
            {
                searchDataSaves = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SearchDataSave>>(data);


                for (var i = 0; i < searchDataSaves.Count; i++)
                {
                    if (searchDataSaves[i].Name == searchDataSave.OldName && searchDataSaves[i].Key == searchDataSave.Key && searchDataSaves[i].PageName == searchDataSave.PageName)
                    {
                        searchDataSaves[i].Name = searchDataSave.Name;
                    }
                }
            }
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), MESSettingNames.SearchTemplate, Newtonsoft.Json.JsonConvert.SerializeObject(searchDataSaves));

        }

        #endregion

        #region 获取基础模板数据
        public virtual List<SearchData> GetBaseSearchData()
        {
            List<SearchData> list = new List<SearchData>();
            return list;
        }
        #endregion

        #region 获取模板数据
        public virtual async Task<List<SearchData>> GetSearchData(string name,string key,string pagename)
        {
            List<SearchData> searchDatas = new List<SearchData>();
            if (!string.IsNullOrEmpty(name)&& !string.IsNullOrEmpty(key)&& !string.IsNullOrEmpty(pagename))
            {
                string data = await SettingManager.GetSettingValueForUserAsync(MESSettingNames.SearchTemplate, AbpSession.ToUserIdentifier());
                if (string.IsNullOrEmpty(data))
                {
                    searchDatas = GetBaseSearchData();
                }
                else
                {
                    var searchDataSavelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SearchDataSave>>(data);
                    bool flag = true;
                    foreach(var searchDataSave in searchDataSavelist)
                    {
                        if(searchDataSave.Name==name&& searchDataSave.Key==key&&searchDataSave.PageName== pagename)
                        {
                            searchDatas = searchDataSave.searchDatas;
                            flag = false;
                            break;
                        }
                    }
                    if(flag)
                    {
                        searchDatas = GetBaseSearchData();
                    }
                }
            }
            else
            {
                searchDatas = GetBaseSearchData();
            }


            return searchDatas;
        }
        #endregion

        #region 获取模板列表信息
        public virtual async Task<List<string>> GetSearchDataSave(string Key,string Pagename)
        {
            List<string> searchDatas = new List<string>();
            string data = await SettingManager.GetSettingValueForUserAsync(MESSettingNames.SearchTemplate, AbpSession.ToUserIdentifier());

            if(!string.IsNullOrEmpty(data))
            {
                var searchDataSavelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SearchDataSave>>(data);
                foreach (var searchDataSave in searchDataSavelist)
                {
                    if(searchDataSave.Key== Key&&searchDataSave.PageName== Pagename)
                    {
                        searchDatas.Add(searchDataSave.Name);
                    }
                    
                }
            }

            return searchDatas;
        }
        #endregion

        #endregion
    }
}
