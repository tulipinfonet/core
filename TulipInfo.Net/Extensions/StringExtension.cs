using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TulipInfo.Net
{
    public static class StringExtension
    {
        public static bool? ToNullableBoolean(this string input)
        {
            bool? result = null;
            if (input != null)
            {
                string up = input.ToUpper();
                if (up == "Y"
                    || up == "YES"
                    || up == "TRUE"
                    || input == "1")
                {
                    result = true;
                }
                else if (up == "N"
                    || up == "NO"
                  || up == "FALSE"
                  || input == "0")
                {
                    result = false;
                }
            }
            return result;
        }

        public static bool ToBoolean(this string input)
        {
            if (input == null)
                return false;
            string up = input.ToUpper();
            return up == "Y"
                || up == "YES"
                || up == "TRUE"
                || input == "1";
        }

        public static byte? ToNullableByte(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            byte value = 0;
            if (byte.TryParse(input, out value))
                return value;
            else
                return null;
        }

        public static byte ToByte(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            byte value = 0;
            if (byte.TryParse(input, out value))
                return value;
            else
                return 0;
        }

        public static int? ToNullableInt(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            int value = 0;
            if (int.TryParse(input, out value))
                return value;
            else
                return null;
        }

        public static int ToInt(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            int value = 0;
            if (int.TryParse(input, out value))
                return value;
            else
                return 0;
        }

        public static short? ToNullableInt16(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            short value = 0;
            if (short.TryParse(input, out value))
                return value;
            else
                return null;
        }

        public static short ToInt16(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            short value = 0;
            if (short.TryParse(input, out value))
                return value;
            else
                return 0;
        }

        public static long? ToNullableInt64(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            long value = 0;
            if (long.TryParse(input, out value))
                return value;
            else
                return null;
        }

        public static long ToInt64(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            long value = 0;
            if (long.TryParse(input, out value))
                return value;
            else
                return 0;
        }

        public static float? ToNullableFloat(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            float value = 0;
            if (float.TryParse(input, out value))
                return value;
            else
                return null;
        }

        public static float ToFloat(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            float value = 0;
            if (float.TryParse(input, out value))
                return value;
            else
                return 0;
        }

        public static double? ToNullableDouble(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            double value = 0;
            if (double.TryParse(input, out value))
                return value;
            else
                return null;
        }

        public static double ToDouble(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            double value = 0;
            if (double.TryParse(input, out value))
                return value;
            else
                return 0;
        }

        public static decimal? ToNullableDecimal(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            decimal value = 0;
            if (decimal.TryParse(input, out value))
                return value;
            else
                return null;
        }

        public static decimal ToDecimal(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            decimal value = 0;
            if (decimal.TryParse(input, out value))
                return value;
            else
                return 0;
        }

        public static DateTime? ToNullableDateTime(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            DateTime result = DateTime.MinValue;
            if (DateTime.TryParse(input, out result))
            {
                return result;
            }
            else
            {
                string newValue = (input + "00000000000000000").Substring(0, 17);
                if (DateTime.TryParseExact(newValue, "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                    return result;
            }

            return null;
        }

        public static DateTime? ToNullableDateTime(this string input, string format)
        {
            return input.ToNullableDateTime(format, CultureInfo.InvariantCulture);
        }

        public static DateTime? ToNullableDateTime(this string input, string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            string newValue = input;
            if (input.Length > format.Length)
            {
                newValue = input.Substring(0, format.Length);
            }
            else if (input.Length < format.Length)
            {
                newValue = input.PadRight(format.Length, '0');
            }
            DateTime result = DateTime.MinValue;
            if (DateTime.TryParseExact(newValue, format, formatProvider, DateTimeStyles.None, out result))
                return result;
            else
                return null;
        }

        public static DateTime ToDateTime(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return DateTime.MinValue;

            DateTime result = DateTime.MinValue;
            if (!DateTime.TryParse(input, out result))
            {
                string newValue = (input + "00000000000000000").Substring(0, 17);
                DateTime.TryParseExact(newValue, "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
            }
            return result;
        }

        public static DateTime ToDateTime(this string input, string format)
        {
            return input.ToDateTime(format, CultureInfo.InvariantCulture);
        }

        public static DateTime ToDateTime(this string input, string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrWhiteSpace(input))
                return DateTime.MinValue;

            string newValue = input;
            if (input.Length > format.Length)
            {
                newValue = input.Substring(0, format.Length);
            }
            else if (input.Length < format.Length)
            {
                newValue = input.PadRight(format.Length, '0');
            }
            DateTime result = DateTime.MinValue;
            DateTime.TryParseExact(newValue, format, formatProvider, System.Globalization.DateTimeStyles.None, out result);
            return result;
        }

        public static Guid ToGuid(this string input)
        {
            Guid result = Guid.Empty;
            Guid.TryParse(input, out result);
            return result;
        }

        public static byte[] ToBytes(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return new byte[0];
            return Encoding.UTF8.GetBytes(input);
        }

        public static DataType ToDataType<DataType>(this string input)
        {
            Type t = typeof(DataType);
            object newValue = null;
            if (t == typeof(bool))
            {
                newValue = input.ToBoolean();
            }
            else if (t == typeof(bool?))
            {
                newValue = input.ToNullableBoolean();
            }
            else if (t == typeof(int))
            {
                newValue = input.ToInt();
            }
            else if (t == typeof(int?))
            {
                newValue = input.ToNullableInt();
            }
            else if (t == typeof(short))
            {
                newValue = input.ToInt16();
            }
            else if (t == typeof(short?))
            {
                newValue = input.ToNullableInt16();
            }
            else if (t == typeof(long))
            {
                newValue = input.ToInt64();
            }
            else if (t == typeof(long?))
            {
                newValue = input.ToNullableInt64();
            }
            else if (t == typeof(float))
            {
                newValue = input.ToFloat();
            }
            else if (t == typeof(float?))
            {
                newValue = input.ToNullableFloat();
            }
            else if (t == typeof(double))
            {
                newValue = input.ToDouble();
            }
            else if (t == typeof(double?))
            {
                newValue = input.ToNullableDouble();
            }
            else if (t == typeof(decimal))
            {
                newValue = input.ToDecimal();
            }
            else if (t == typeof(decimal?))
            {
                newValue = input.ToNullableDecimal();
            }
            else if (t == typeof(DateTime))
            {
                newValue = input.ToDateTime();
            }
            else if (t == typeof(DateTime?))
            {
                newValue = input.ToNullableDateTime();
            }
            else if (t == typeof(Guid))
            {
                newValue = input.ToGuid();
            }
            else if (t == typeof(byte[]))
            {
                newValue = input.ToBytes();
            }
            else
            {
                throw new NotImplementedException("Un Supported Data Type:"+ t.ToString());
            }

            if (newValue == null)
                return default(DataType);
            else
                return (DataType)Convert.ChangeType(newValue, t);
        }

        public static string ToUpperSafe(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }
            return input.ToUpperInvariant();
        }

        public static string ToLowerSafe(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }
            return input.ToLowerInvariant();
        }

        static readonly char[] _emptySpaces = new char[] { '﻿'/*half space*/, ' '/*half width*/, '　'/*full width*/,' '/*chinese space*/};
        public static string TrimSafe(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            return input.Trim(_emptySpaces);
        }

        public static string TrimSafe(this string input,char trimChar)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            return input.Trim(trimChar);
        }

        public static string TrimSafe(this string input, params char[] trimChars)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            return input.Trim(trimChars);
        }

        public static string TrimStartSafe(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            return input.TrimStart(_emptySpaces);
        }

        public static string TrimStartSafe(this string input, char trimChar)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            return input.TrimStart(trimChar);
        }

        public static string TrimStartSafe(this string input, params char[] trimChars)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            return input.TrimStart(trimChars);
        }

        public static string TrimEndSafe(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            return input.TrimStart(_emptySpaces);
        }

        public static string TrimEndSafe(this string input, char trimChar)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            return input.TrimStart(trimChar);
        }

        public static string TrimEndSafe(this string input, params char[] trimChars)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            return input.TrimStart(trimChars);
        }

        public static bool EqualsTo(this string input, string compare, bool nullAsEmpty = true, bool trim = true)
        {
            if(input==null && compare==null)
            {
                return true;
            }

            if(input==null || compare == null)
            {
                if (nullAsEmpty)
                {
                    string strInput = input == null ? "" : (trim ? input.TrimSafe() : input);
                    string strCompare = compare == null ? "" : (trim ? compare.TrimSafe() : compare);
                    return strInput == strCompare;
                }
                else
                {
                    return false;
                }
            }

            return (trim ? input.TrimSafe() : input) == (trim ? compare.TrimSafe() : compare);
        }
        public static string Left(this string input, int length)
        {
            if (input == null)
            {
                return string.Empty;
            }

            if (length < 0 || input.Length <= length)
            {
                return input;
            }

            return input.Substring(0, length);
        }

        public static string Right(this string input, int length)
        {
            if (input == null)
            {
                return string.Empty;
            }

            if (length < 0 || input.Length <= length)
            {
                return input;
            }

            return input.Substring(input.Length - length);
        }
    }
}
