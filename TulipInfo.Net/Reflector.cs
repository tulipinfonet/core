using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Collections;

namespace TulipInfo.Net
{
    public static class Reflector
    {
        public static object GetPropertyValue(object obj, string propertyName)
        {
            if (obj != null)
            {
                try
                {
                    var prop = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.IgnoreCase);
                    if (prop != null)
                    {
                        return prop.GetValue(obj, null);
                    }
                }
                catch
                {
                    //ignore
                }
            }
            return null;
        }

        public static DataType GetPropertyValue<DataType>(object obj, string propertyName)
        {
            object value = GetPropertyValue(obj, propertyName);
            return value.ToDataType<DataType>();
        }

        public static void SetPropertyValue(object obj, string propertyName, object propertyValue)
        {
            if (obj != null)
            {
                try
                {
                    var prop = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.IgnoreCase);
                    if (prop != null)
                    {
                        prop.SetValue(obj, propertyValue);
                    }
                }
                catch
                {
                    //ignore
                }
            }
        }

        public static bool IsNullableType(Type type)
        {
            return type.IsClass || type.Name.StartsWith("Nullable");
        }

        public static bool IsValueType(Type t)
        {
            return t.IsValueType || t.IsEnum || t.Equals(typeof(string));
        }

        public static IDictionary<string, object?> ConvertToDictionary(object obj)
        {
            IDictionary<string, object?> result = new Dictionary<string, object?>();
            if (obj != null)
            {
                if (obj is IDictionary<string, object?> dictionary)
                {
                    return dictionary;
                }
                else if (obj is IDictionary)
                {
                    foreach (var key in ((IDictionary)obj).Keys)
                    {
                        result.Add(key.ToString()!, ((IDictionary)obj)[key]);
                    }
                    return result;
                }
                else
                {
                    var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
                    foreach (var property in properties)
                    {
                        result.Add(property.Name, property.GetValue(obj, null));
                    }
                    return result;
                }
            }
            return result;
        }
    }
}
