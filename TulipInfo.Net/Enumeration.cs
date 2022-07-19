using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TulipInfo.Net
{
    public static class Enumeration
    {
        public static IList<KeyValuePair<string, string>> GetKeyValueList<T>(bool includeEmpty=false)
            where T: Enum
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            if (includeEmpty)
            {
                result.Add(new KeyValuePair<string, string>("", ""));
            }
            foreach (T v in Enum.GetValues(typeof(T)).Cast<T>())
            {
                object val = Convert.ChangeType(v, v.GetTypeCode());

                result.Add(new KeyValuePair<string, string>((val.ToString()), GetDisplayValue(v)));
            }
            return result;
        }
       

        private static string GetDisplayValue<T>(T value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static T Parse<T>(object value)
            where T: struct
        {
            T result = default(T);
            if (value != null)
            {
                Enum.TryParse<T>(value.ToString(), true, out result);
            }
            return result;
        }
    }
}
