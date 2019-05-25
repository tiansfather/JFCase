using Abp.Domain.Entities;
using Abp.Reflection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Abp;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Master.Entity
{
    public interface IHaveProperty
    {
        JsonObject<IDictionary<string, object>> Property { get; set; }
    }

    public static class HavePropertyObjectExtension
    {
        public static object GetPropertyValue(this IHaveProperty havePropertyObject, string name)
        {
            var obj = havePropertyObject.Property?.Object;
            if (obj != null && obj.ContainsKey(name))
            {
                return obj[name];
            }
            return null;
        }

        #region old
        //public static T GetPropertyValue<T>(this IHaveProperty havePropertyObject, string name)
        //{
        //    var obj = havePropertyObject.Property?.Object;
        //    if (obj != null && obj.ContainsKey(name))
        //    {
        //        try
        //        {
        //            Type t = typeof(T);
        //            if (t.IsGenericType)
        //            {
        //                Type genericTypeDefinition = t.GetGenericTypeDefinition();
        //                if (genericTypeDefinition == typeof(Nullable<>))
        //                {
        //                    return (T)Convert.ChangeType(obj[name], Nullable.GetUnderlyingType(t));
        //                }
        //            }
        //            return (T)Convert.ChangeType(obj[name], t);
        //            //return (T)obj[name];
        //        }
        //        catch (Exception ex)
        //        {
        //            return default(T);
        //        }

        //    }
        //    return default(T);
        //}

        //public static void SetPropertyValue(this IHaveProperty havePropertyObject, string name, object value)
        //{
        //    var obj = havePropertyObject.Property?.Object;
        //    if (obj == null)
        //    {
        //        obj = new Dictionary<string, object>();
        //    }
        //    obj[name] = value;
        //    havePropertyObject.Property = new JsonObject<IDictionary<string, object>>(obj);
        //}

        //public static void RemoveProperty(this IHaveProperty havePropertyObject, string name)
        //{
        //    var obj = havePropertyObject.Property?.Object;
        //    if (obj != null && obj.ContainsKey(name))
        //    {
        //        obj.Remove(name);
        //        havePropertyObject.Property.Json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        //    }
        //} 
        #endregion

        public static T GetPropertyValue<T>([NotNull]this IHaveProperty havePropertyObject, [NotNull] string name)
        {
            var handleType = false;
            return havePropertyObject.GetPropertyValue<T>(
                name,
                handleType
                    ? new JsonSerializer { TypeNameHandling = TypeNameHandling.All }
                    : JsonSerializer.CreateDefault()
            );
        }

        public static T GetPropertyValue<T>([NotNull]this IHaveProperty havePropertyObject, [NotNull]string name, [CanBeNull] JsonSerializer jsonSerializer)
        {
            Check.NotNull(havePropertyObject, nameof(havePropertyObject));
            Check.NotNull(name, nameof(name));

            if (havePropertyObject.Property?.Object == null)
            {
                return default(T);
            }

            var json = JObject.Parse(havePropertyObject.Property.Json);

            var prop = json[name];
            if (prop == null)
            {
                return default(T);
            }

            if (IsPrimitiveExtendedIncludingNullable(typeof(T)))
            {
                try
                {
                    return prop.Value<T>();
                }
                catch
                {
                    return default(T);
                }
                
            }
            else
            {
                return (T)prop.ToObject(typeof(T), jsonSerializer ?? JsonSerializer.CreateDefault());
            }
        }

        public static void SetPropertyValue<T>([NotNull] this IHaveProperty havePropertyObject, [NotNull] string name, [CanBeNull] T value)
        {
            var handleType = false;
            havePropertyObject.SetPropertyValue(
                name,
                value,
                handleType
                    ? new JsonSerializer { TypeNameHandling = TypeNameHandling.All }
                    : JsonSerializer.CreateDefault()
            );
        }

        public static void SetPropertyValue<T>([NotNull] this IHaveProperty havePropertyObject, [NotNull] string name, [CanBeNull] T value, [CanBeNull] JsonSerializer jsonSerializer)
        {
            Check.NotNull(havePropertyObject, nameof(havePropertyObject));
            Check.NotNull(name, nameof(name));

            if (jsonSerializer == null)
            {
                jsonSerializer = JsonSerializer.CreateDefault();
            }

            if (havePropertyObject.Property?.Object == null)
            {
                if (EqualityComparer<T>.Default.Equals(value, default(T)))
                {
                    return;
                }

            }

            var json = JObject.Parse(string.IsNullOrEmpty(havePropertyObject.Property?.Json)?"{}":havePropertyObject.Property.Json);

            if (value == null || EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                if (json[name] != null)
                {
                    json.Remove(name);
                }
            }
            else if (IsPrimitiveExtendedIncludingNullable(value.GetType()))
            {
                json[name] = new JValue(value);
            }
            else
            {
                json[name] = JToken.FromObject(value, jsonSerializer);
            }

            var data = json.ToString(Formatting.None);
            //if (data == "{}")
            //{
            //    data = null;
            //}
            havePropertyObject.Property = new JsonObject<IDictionary<string, object>>(data);
            //havePropertyObject.Property.Json = data;
        }

        public static bool RemoveProperty([NotNull] this IHaveProperty havePropertyObject, string name)
        {
            Check.NotNull(havePropertyObject, nameof(havePropertyObject));

            if (havePropertyObject.Property?.Object == null)
            {
                return false;
            }

            var json = JObject.Parse(havePropertyObject.Property.Json);

            var token = json[name];
            if (token == null)
            {
                return false;
            }

            json.Remove(name);

            var data = json.ToString(Formatting.None);
            //if (data == "{}")
            //{
            //    data = null;
            //}

            havePropertyObject.Property = new JsonObject<IDictionary<string, object>>(data);
            //havePropertyObject.Property.Json = data;

            return true;
        }

        #region Static
        public static bool IsPrimitiveExtendedIncludingNullable(Type type, bool includeEnums = false)
        {
            if (IsPrimitiveExtended(type, includeEnums))
            {
                return true;
            }

            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return IsPrimitiveExtended(type.GenericTypeArguments[0], includeEnums);
            }

            return false;
        }

        private static bool IsPrimitiveExtended(Type type, bool includeEnums)
        {
            if (type.GetTypeInfo().IsPrimitive)
            {
                return true;
            }

            if (includeEnums && type.GetTypeInfo().IsEnum)
            {
                return true;
            }

            return type == typeof(string) ||
                   type == typeof(decimal) ||
                   type == typeof(DateTime) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(Guid);
        }
        #endregion
    }
}
