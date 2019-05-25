using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Common
{
    public static class EnumHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<int,string> ToDictionary<T>()
        {
            return ToDictionary(typeof(T));
        }

        public static Dictionary<int,string> ToDictionary(Type type)
        {
            var list = new Dictionary<int, string>();

            foreach (var item in type.GetFields())
            {
                try
                {
                    var key = (int)Enum.Parse(type, item.Name);
                    var name = item.Name;
                    // 获取描述
                    var attr = item.GetCustomAttribute(typeof(DisplayNameAttribute), true) as DisplayNameAttribute;
                    if (attr != null && !string.IsNullOrEmpty(attr.DisplayName))
                    {
                        name = attr.DisplayName;
                    }
                    // 添加

                    list.Add(key, name);
                }catch(Exception ex)
                {

                }
                
            }
            return list;
        }
    }
}
