using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Master.Extension
{
    public static class IDictionaryExtension
    {
        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            var dic = new Dictionary<string, object>();
            foreach (var property in obj.GetType().GetProperties())
            {
                dic[property.Name] = property.GetValue(obj);
            }
            return dic;
        }

        #region 获取数据
        public static string GetDataOrEmpty(this IDictionary<string, string> datas, string key)
        {
            if (string.IsNullOrEmpty(key) || !datas.ContainsKey(key))
            {
                return string.Empty;
            }
            else
            {
                return datas[key];
            }
        }
        /// <summary>
        /// 根据键获了数据，若键不存在则抛出异常
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetDataOrException(this IDictionary<string, string> datas, string key)
        {
            if (!datas.ContainsKey(key))
            {
                throw new UserFriendlyException($"Key:{key} not exist");
            }
            else
            {
                return datas[key];
            }
        }
        /// <summary>
        /// 获取强类型数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetDataOrDefault<T>(this IDictionary<string, string> datas, string key)
        {
            try
            {
                return GetDataOrException<T>(datas, key);
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// 获取强类型数据
        /// GetDataOrException<DateTime?>("date")
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetDataOrException<T>(this IDictionary<string, string> datas, string key)
        {
            if (!datas.ContainsKey(key))
            {
                throw new UserFriendlyException($"Key:{key} not exist");
            }
            else
            {
                //获取表单提交的强类型数据 20180607 lijianbo
                var value = datas[key];
                if (value == null)
                    return default(T);
                else
                {
                    Type t = typeof(T);
                    if (t.IsGenericType)
                    {
                        Type genericTypeDefinition = t.GetGenericTypeDefinition();
                        if (genericTypeDefinition == typeof(Nullable<>))
                        {
                            return (T)Convert.ChangeType(value, Nullable.GetUnderlyingType(t));
                        }
                    }
                    return (T)Convert.ChangeType(value, t);
                }

            }
        }
        #endregion

        #region 设置数据
        public static void SetValue<TKey, TValue>(this IDictionary<TKey, TValue> datas, TKey key, TValue value)
        {
            if (!datas.ContainsKey(key))
            {
                datas.Add(key, value);
            }
            else
            {
                datas[key] = value;
            }
        }
        #endregion
    }

    public static class DynamicClassExtension
    {
        public static IDictionary<string, object> ToDictionary(this DynamicClass entity)
        {
            var dic = new Dictionary<string, object>();
            foreach (var key in entity.GetDynamicMemberNames())
            {
                dic.Add(key, entity.GetDynamicPropertyValue(key));
            }

            return dic;
        }
    }
}
