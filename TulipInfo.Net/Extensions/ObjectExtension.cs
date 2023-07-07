using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TulipInfo.Net
{
    public static class ObjectExtension
    {
        public static bool? ToNullableBoolean(this object input)
        {
            if (input == null || input == DBNull.Value)
            {
                return null;
            }
            Type type = input.GetType();
            if (type == typeof(bool) || type == typeof(bool?))
            {
                return Convert.ToBoolean(input);
            }
            return input.ToString().ToNullableBoolean();
        }

        public static bool ToBoolean(this object input)
        {
            if (input == null || input == DBNull.Value)
            {
                return false;
            }
            Type type = input.GetType();
            if (type == typeof(bool) || type==typeof(bool?))
            {
                return Convert.ToBoolean(input);
            }
            return input.ToString().ToBoolean();

        }

        public static int? ToNullableInt(this object input)
        {
            if (input == null || input == DBNull.Value)
            {
                return null;
            }
            Type type = input.GetType();
            if (type == typeof(int) || type==typeof(int?) || type==typeof(short) || type==typeof(short?) )
            {
                return Convert.ToInt32(input);
            }
            return input.ToString().ToNullableInt();
        }

        public static int ToInt(this object input)
        {
            if (input == null || input == DBNull.Value)
            {
                return 0;
            }
            Type type = input.GetType();
            if (type == typeof(int) || type == typeof(int?) || type == typeof(short) || type == typeof(short?))
            {
                return Convert.ToInt32(input);
            }
            return input.ToString().ToInt();
        }

        public static short? ToNullableInt16(this object input)
        {
            if (input == null || input == DBNull.Value)
            {
                return null;
            }
            Type type = input.GetType();
            if (type == typeof(short) || type == typeof(short?))
            {
                return Convert.ToInt16(input);
            }
            return input.ToString().ToNullableInt16();
        }

        public static short ToInt16(this object input)
        {
            if (input == null || input == DBNull.Value)
            {
                return 0;
            }
            Type type = input.GetType();
            if (type == typeof(short) || type == typeof(short?))
            {
                return Convert.ToInt16(input);
            }
            return input.ToString().ToInt16();
        }

        public static long? ToNullableInt64(this object input)
        {
            if (input == null || input == DBNull.Value)
            {
                return null;
            }
            Type type = input.GetType();
            if (type == typeof(long) || type == typeof(long?)
                || type == typeof(int) || type == typeof(int?)
                || type == typeof(short) || type == typeof(short?))
            {
                return Convert.ToInt64(input);
            }
            return input.ToString().ToNullableInt64();
        }

        public static long ToInt64(this object input)
        {
            if (input == null || input == DBNull.Value)
            {
                return 0;
            }
            Type type = input.GetType();
            if (type == typeof(long) || type == typeof(long?)
                || type == typeof(int) || type == typeof(int?)
                || type == typeof(short) || type == typeof(short?))
            {
                return Convert.ToInt64(input);
            }
            return input.ToString().ToInt64();
        }

        public static float? ToNullableFloat(this object input)
        {
            if (input == null || input == DBNull.Value)
            {
                return null;
            }
            Type type = input.GetType();
            if (type == typeof(float) || type == typeof(float?))
            {
                return Convert.ToSingle(input);
            }
            return input.ToString().ToNullableFloat();
        }

        public static float ToFloat(this object input)
        {
            if (input == null || input == DBNull.Value)
            {
                return 0;
            }
            Type type = input.GetType();
            if (type == typeof(float) || type == typeof(float?))
            {
                return Convert.ToSingle(input);
            }
            return input.ToString().ToFloat();
        }

        public static double? ToNullableDouble(this object input)
        {
            if (input == null || input == DBNull.Value)
            {
                return null;
            }
            Type type = input.GetType();
            if (type == typeof(double) || type == typeof(double?))
            {
                return Convert.ToDouble(input);
            }
            return input.ToString().ToNullableDouble();
        }

        public static double ToDouble(this object input)
        {
            if (input == null || input == DBNull.Value)
            {
                return 0;
            }
            Type type = input.GetType();
            if (type == typeof(double) || type == typeof(double?))
            {
                return Convert.ToDouble(input);
            }
            return input.ToString().ToDouble();
        }

        public static decimal? ToNullableDecimal(this object input)
        {
            if (input == null || input == DBNull.Value)
            {
                return null;
            }
            Type type = input.GetType();
            if (type == typeof(decimal) || type == typeof(decimal?))
            {
                return Convert.ToDecimal(input);
            }
            return input.ToString().ToNullableDecimal();
        }

        public static decimal ToDecimal(this object input)
        {
            if (input == null || input == DBNull.Value)
            {
                return 0;
            }
            Type type = input.GetType();
            if (type == typeof(decimal) || type == typeof(decimal?))
            {
                return Convert.ToDecimal(input);
            }
            return input.ToString().ToDecimal();
        }

        public static DateTime? ToNullableDateTime(this object input)
        {
            if (input == null || input == DBNull.Value)
            {
                return null;
            }
            Type type = input.GetType();
            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return Convert.ToDateTime(input);
            }
            return input.ToString().ToNullableDateTime();
        }

        public static DateTime? ToNullableDateTime(this object input, string format)
        {
            return input.ToNullableDateTime(format, CultureInfo.InvariantCulture);
        }

        public static DateTime? ToNullableDateTime(this object input, string format, IFormatProvider formatProvider)
        {
            if (input == null || input == DBNull.Value)
            {
                return null;
            }
            Type type = input.GetType();
            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return Convert.ToDateTime(input);
            }
            return input.ToString().ToNullableDateTime(format,formatProvider);
        }

        public static DateTime ToDateTime(this object input)
        {
            if (input == null || input == DBNull.Value)
            {
                return DateTime.MinValue;
            }
            Type type = input.GetType();
            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return Convert.ToDateTime(input);
            }
            return input.ToString().ToDateTime();
        }

        public static DateTime ToDateTime(this object input, string format)
        {
            return input.ToDateTime(format, CultureInfo.InvariantCulture);
        }

        public static DateTime ToDateTime(this object input, string format, IFormatProvider formatProvider)
        {
            if (input == null || input == DBNull.Value)
            {
                return DateTime.MinValue;
            }
            Type type = input.GetType();
            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return Convert.ToDateTime(input);
            }
            return input.ToString().ToDateTime(format, formatProvider);
        }

        public static Guid ToGuid(this object input)
        {
            if (input == null || input == DBNull.Value)
            {
                return Guid.Empty;
            }
            Type type = input.GetType();
            if (type == typeof(Guid) || type == typeof(Guid?))
            {
                return (Guid)(input);
            }
            return input.ToString().ToGuid();
        }

        public static byte[] ToBytes(this object input)
        {
            if (input == null || input == DBNull.Value)
            {
                return new byte[0];
            }
            if (input.GetType() == typeof(byte[]))
            {
                return (byte[])(input);
            }
            return new byte[0];
        }

        public static DataType ToDataType<DataType>(this object input)
        {
            Type t = typeof(DataType);
            object? newValue = null;
            if (t == typeof(string))
            {
                newValue = input == null ? null : input.ToString();
            }
            else if (t == typeof(bool))
            {
                newValue = input.ToBoolean();
            }
            else if (t == typeof(bool?))
            {
                newValue = input.ToNullableBoolean();
                if (newValue != null)
                {
                    return (DataType)newValue;
                }
            }
            else if (t == typeof(int))
            {
                newValue = input.ToInt();
            }
            else if (t == typeof(int?))
            {
                newValue = input.ToNullableInt();
                if (newValue != null)
                {
                    return (DataType)newValue;
                }
            }
            else if (t == typeof(short))
            {
                newValue = input.ToInt16();
            }
            else if (t == typeof(short?))
            {
                newValue = input.ToNullableInt16();
                if (newValue != null)
                {
                    return (DataType)newValue;
                }
            }
            else if (t == typeof(long))
            {
                newValue = input.ToInt64();
            }
            else if (t == typeof(long?))
            {
                newValue = input.ToNullableInt64();
                if (newValue != null)
                {
                    return (DataType)newValue;
                }
            }
            else if (t == typeof(float))
            {
                newValue = input.ToFloat();
            }
            else if (t == typeof(float?))
            {
                newValue = input.ToNullableFloat();
                if (newValue != null)
                {
                    return (DataType)newValue;
                }
            }
            else if (t == typeof(double))
            {
                newValue = input.ToDouble();
            }
            else if (t == typeof(double?))
            {
                newValue = input.ToNullableDouble();
                if (newValue != null)
                {
                    return (DataType)newValue;
                }
            }
            else if (t == typeof(decimal))
            {
                newValue = input.ToDecimal();
            }
            else if (t == typeof(decimal?))
            {
                newValue = input.ToNullableDecimal();
                if (newValue != null)
                {
                    return (DataType)newValue;
                }
            }
            else if (t == typeof(DateTime))
            {
                newValue = input.ToDateTime();
            }
            else if (t == typeof(DateTime?))
            {
                newValue = input.ToNullableDateTime();
                if (newValue != null)
                {
                    return (DataType)newValue;
                }
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
                throw new NotImplementedException("Un Supported Data Type:" + t.ToString());
            }

            if (newValue == null)
                return default(DataType);
            else
                return (DataType)Convert.ChangeType(newValue, t);
        }

        public static object GetPropertyValue(this object input, string propertyName)
        {
            return Reflector.GetPropertyValue(input, propertyName);
        }

        public static DataType GetPropertyValue<DataType>(this object input, string propertyName)
        {
            return Reflector.GetPropertyValue<DataType>(input, propertyName);
        }
    }
}
