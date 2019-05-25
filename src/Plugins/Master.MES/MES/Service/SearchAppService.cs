using Abp.Dependency;
using Abp.Reflection;
using Abp.Runtime.Session;
using Abp.UI;
using Master.Configuration;
using Master.MES.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES.Service
{
    public class SearchAppService : MESAppServiceBase<ProcessTask, int>
    {
        public ITypeFinder TypeFinder { get; set; }
        #region 获取模板数据
        public override async Task<List<SearchData>> GetSearchData(string name, string key, string pagename)
        {
            List<SearchData> searchDatas = new List<SearchData>();
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(key))
            {
                string data = await SettingManager.GetSettingValueForUserAsync(MESSettingNames.SearchTemplate, AbpSession.ToUserIdentifier());
                if (string.IsNullOrEmpty(data))
                {
                    searchDatas = Getdata(key);
                }
                else
                {
                    var searchDataSavelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SearchDataSave>>(data);
                    bool flag = true;
                    foreach (var searchDataSave in searchDataSavelist)
                    {
                        if (searchDataSave.Name == name && searchDataSave.Key == key&& searchDataSave.PageName==pagename)
                        {
                            searchDatas = searchDataSave.searchDatas;
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        searchDatas = Getdata(key);
                    }
                }
            }
            else
            {
                searchDatas = Getdata(key);
            }


            return searchDatas;
        }

        /// <summary>
        /// 通过反射获取基础数据
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<SearchData> Getdata(string key)
        {
            List<SearchData> searchDatas = new List<SearchData>();
            try { 
            var type = TypeFinder.Find(o => o.Name == key + "AppService")[0];
            using (var serviceWrapper = IocManager.Instance.ResolveAsDisposable(type))
            {
                searchDatas = type.GetMethod("GetBaseSearchData").Invoke(serviceWrapper.Object, new object[] { }) as List<SearchData>;
            }
                //searchDatas =new ProcessTaskReportAppService().GetBaseSearchData();
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
            return searchDatas;
        }
        #endregion
    }
}
