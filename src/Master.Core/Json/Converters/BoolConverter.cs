using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.Json.Converters
{
    public class BoolConvert : JsonConverter
    {
        private string[] arrFalseString { get; set; }

        public BoolConvert()
        {
            arrFalseString="否,false,0".Split(',');
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BooleanString">将bool值转换成的字符串值</param>
        public BoolConvert(string falseString)
        {
            arrFalseString= falseString.Split(',');
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            bool isNullable = IsNullableType(objectType);
            Type t = isNullable ? Nullable.GetUnderlyingType(objectType) : objectType;

            if (reader.TokenType == JsonToken.Null)
            {
                if (!IsNullableType(objectType))
                {
                    return false;
                    //throw new Exception(string.Format("不能转换null value to {0}.", objectType));
                }

                return null;
            }

            try
            {
                if (reader.TokenType == JsonToken.String)
                {
                    string boolText = reader.Value.ToString().ToLower();
                    if (string.IsNullOrEmpty(boolText) || arrFalseString.Contains(boolText))
                    //else if (boolText.Equals(arrBString[1], StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                    return true;
                }
                if (reader.TokenType == JsonToken.Integer)
                {
                    //数值
                    return Convert.ToInt32(reader.Value) >0;
                }
                if (reader.TokenType == JsonToken.Boolean)
                {
                    return Convert.ToBoolean(reader.Value);
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error converting value {0} to type '{1}'", reader.Value, objectType));
            }
            throw new Exception(string.Format("Unexpected token {0} when parsing bool", reader.TokenType));
        }

        /// <summary>
        /// 判断是否为Bool类型
        /// </summary>
        /// <param name="objectType">类型</param>
        /// <returns>为bool类型则可以进行转换</returns>
        public override bool CanConvert(Type objectType)
        {
            //return true;
            return objectType == typeof(bool) || objectType == typeof(bool?);
        }


        public bool IsNullableType(Type t)
        {
            if (t == null)
            {
                throw new ArgumentNullException("t");
            }
            return (t.IsGenericType && t.BaseType.FullName == "System.ValueType" && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            bool bValue = (bool)value;

            if (bValue)
            {
                writer.WriteValue(true);
            }
            else
            {
                writer.WriteValue(false);
            }
        }
    }
}
