﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace TulipInfo.Net
{
    public static class StringTemplate
    {
        public static string Format(string input,object data, bool htmlEncoding = false)
        {
            return Format(input, "${", "}", data,htmlEncoding);
        }

        public static string Format(string input, string searchStart, string searchEnd, object data,bool htmlEncoding=false)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            string formattedValue = input;

            string start = searchStart.Replace("$", @"\$").Replace("^", @"\^");
            string end = searchEnd.Replace("$", @"\$").Replace("^", @"\^");
            Regex rg = new Regex("(?<=(" + start + "))[.\\s\\S]*?(?=(" + end + "))", RegexOptions.Multiline | RegexOptions.Singleline| RegexOptions.IgnoreCase);
            var matchedValues = rg.Matches(input).Select(m => m.Value).Distinct();

            IDictionary<string, object?> dataDic = Reflector.ConvertToDictionary(data);

            foreach (string searchValue in matchedValues)
            {
                string replacedValue = "";
                string propName = searchValue;
                string format = "";
                if (searchValue.Contains("."))
                {
                    string[] pf = searchValue.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    if (pf.Length > 0)
                    {
                        propName = pf[0];
                    }
                    if (pf.Length > 1)
                    {
                        format = pf[1];
                    }
                }

                var kv = dataDic!.FirstOrDefault(d => d.Key.Equals(propName, StringComparison.OrdinalIgnoreCase));
                object? propValue = kv.Value;

                if (propValue != null)
                {
                    if (string.IsNullOrWhiteSpace(format))
                    {
                        replacedValue = propValue.ToString()!;
                    }

                    if (format == "html")
                    {
                        replacedValue = HtmlEncoder.Default.Encode(propValue.ToString()!);
                    }
                    else
                    {
                        //need format
                        Type propType = propValue.GetType();
                        if (propType == typeof(DateTime))
                        {
                            DateTime realValue = Convert.ToDateTime(propValue);
                            replacedValue = realValue.ToString(format);
                        }
                        else if (propType == typeof(decimal))
                        {
                            decimal realValue = Convert.ToDecimal(propValue);
                            replacedValue = realValue.ToString(format);
                        }
                        else if (propType == typeof(float))
                        {
                            float realValue = Convert.ToSingle(propValue);
                            replacedValue = realValue.ToString(format);
                        }
                        else if (propType == typeof(double))
                        {
                            double realValue = Convert.ToDouble(propValue);
                            replacedValue = realValue.ToString(format);
                        }
                        else if (propType == typeof(int))
                        {
                            int realValue = Convert.ToInt32(propValue);
                            replacedValue = realValue.ToString(format);
                        }
                        else if (propType == typeof(long))
                        {
                            long realValue = Convert.ToInt64(propValue);
                            replacedValue = realValue.ToString(format);
                        }
                        else
                        {
                            //not support format currently
                            replacedValue = propValue.ToString()!;
                        }

                        if (htmlEncoding)
                        {
                            replacedValue = HtmlEncoder.Default.Encode(replacedValue);
                        }
                    }
                }

                formattedValue = formattedValue.Replace(searchStart + searchValue + searchEnd, replacedValue, StringComparison.InvariantCultureIgnoreCase);
            }

            return formattedValue;
        }
    }
}
