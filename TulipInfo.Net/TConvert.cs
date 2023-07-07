﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace TulipInfo.Net
{
    public static class TConvert
    {
        public static bool? ToNullableBoolean(object? input)
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
            return input!.ToString()!.ToNullableBoolean();
        }

        public static bool ToBoolean(object? input)
        {
            if (input == null || input == DBNull.Value)
            {
                return false;
            }
            Type type = input.GetType();
            if (type == typeof(bool) || type == typeof(bool?))
            {
                return Convert.ToBoolean(input);
            }
            return input!.ToString()!.ToBoolean();

        }

        public static byte? ToNullableByte(object? input)
        {
            if (input == null || input == DBNull.Value)
            {
                return null;
            }
            Type type = input.GetType();
            if (type == typeof(long) || type == typeof(long?)
                || type == typeof(int) || type == typeof(int?)
                || type == typeof(short) || type == typeof(short?)
                || type == typeof(byte) || type == typeof(byte?))
            {
                return Convert.ToByte(input);
            }
            else if (type.IsEnum)
            {
                return (byte)input;
            }
            return input!.ToString()!.ToByte();
        }

        public static byte ToByte(object? input)
        {
            if (input == null || input == DBNull.Value)
            {
                return 0;
            }
            Type type = input.GetType();
            if (type == typeof(long) || type == typeof(long?)
                || type == typeof(int) || type == typeof(int?)
                || type == typeof(short) || type == typeof(short?)
                || type == typeof(byte) || type == typeof(byte?))
            {
                return Convert.ToByte(input);
            }
            else if (type.IsEnum)
            {
                return (byte)input;
            }
            return input!.ToString()!.ToByte();
        }

        public static int? ToNullableInt(object? input)
        {
            if (input == null || input == DBNull.Value)
            {
                return null;
            }
            Type type = input.GetType();
            if (type == typeof(long) || type == typeof(long?)
                || type == typeof(int) || type == typeof(int?)
                || type == typeof(short) || type == typeof(short?)
                || type == typeof(byte) || type == typeof(byte?))
            {
                return Convert.ToInt32(input);
            }
            else if (type.IsEnum)
            {
                return (int)input;
            }
            return input!.ToString()!.ToNullableInt();
        }

        public static int ToInt(object? input)
        {
            if (input == null || input == DBNull.Value)
            {
                return 0;
            }
            Type type = input.GetType();
            if (type == typeof(long) || type == typeof(long?)
                || type == typeof(int) || type == typeof(int?)
                || type == typeof(short) || type == typeof(short?)
                || type == typeof(byte) || type == typeof(byte?))
            {
                return Convert.ToInt32(input);
            }
            else if (type.IsEnum)
            {
                return (int)input;
            }
            return input!.ToString()!.ToInt();
        }

        public static short? ToNullableInt16(object? input)
        {
            if (input == null || input == DBNull.Value)
            {
                return null;
            }
            Type type = input.GetType();
            if (type == typeof(long) || type == typeof(long?)
                || type == typeof(int) || type == typeof(int?)
                || type == typeof(short) || type == typeof(short?)
                || type == typeof(byte) || type == typeof(byte?))
            {
                return Convert.ToInt16(input);
            }
            else if (type.IsEnum)
            {
                return (short)input;
            }
            return input!.ToString()!.ToNullableInt16();
        }

        public static short ToInt16(object? input)
        {
            if (input == null || input == DBNull.Value)
            {
                return 0;
            }
            Type type = input.GetType();
            if (type == typeof(long) || type == typeof(long?)
                || type == typeof(int) || type == typeof(int?)
                || type == typeof(short) || type == typeof(short?)
                || type == typeof(byte) || type == typeof(byte?))
            {
                return Convert.ToInt16(input);
            }
            else if (type.IsEnum)
            {
                return (short)input;
            }
            return input!.ToString()!.ToInt16();
        }

        public static long? ToNullableInt64(object? input)
        {
            if (input == null || input == DBNull.Value)
            {
                return null;
            }
            Type type = input.GetType();
            if (type == typeof(long) || type == typeof(long?)
                || type == typeof(int) || type == typeof(int?)
                || type == typeof(short) || type == typeof(short?)
                || type == typeof(byte) || type == typeof(byte?))
            {
                return Convert.ToInt64(input);
            }
            else if (type.IsEnum)
            {
                return (long)input;
            }
            return input!.ToString()!.ToNullableInt64();
        }

        public static long ToInt64(object? input)
        {
            if (input == null || input == DBNull.Value)
            {
                return 0;
            }
            Type type = input.GetType();
            if (type == typeof(long) || type == typeof(long?)
                || type == typeof(int) || type == typeof(int?)
                || type == typeof(short) || type == typeof(short?)
                || type == typeof(byte) || type == typeof(byte?))
            {
                return Convert.ToInt64(input);
            }
            else if (type.IsEnum)
            {
                return (long)input;
            }
            return input!.ToString()!.ToInt64();
        }

        public static float? ToNullableFloat(object? input)
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
            return input!.ToString()!.ToNullableFloat();
        }

        public static float ToFloat(object? input)
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
            return input!.ToString()!.ToFloat();
        }

        public static double? ToNullableDouble(object? input)
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
            return input!.ToString()!.ToNullableDouble();
        }

        public static double ToDouble(object? input)
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
            return input!.ToString()!.ToDouble();
        }

        public static decimal? ToNullableDecimal(object? input)
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
            return input!.ToString()!.ToNullableDecimal();
        }

        public static decimal ToDecimal(object? input)
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
            return input!.ToString()!.ToDecimal();
        }

        public static DateTime? ToNullableDateTime(object? input)
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
            return input!.ToString()!.ToNullableDateTime();
        }

        public static DateTime? ToNullableDateTime(object? input, string format)
        {
            return ToNullableDateTime(input, format, CultureInfo.InvariantCulture);
        }

        public static DateTime? ToNullableDateTime(object? input, string format, IFormatProvider formatProvider)
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
            return input!.ToString()!.ToNullableDateTime(format, formatProvider);
        }

        public static DateTime ToDateTime(object? input)
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
            return input!.ToString()!.ToDateTime();
        }

        public static DateTime ToDateTime(object? input, string format)
        {
            return ToDateTime(input,format, CultureInfo.InvariantCulture);
        }

        public static DateTime ToDateTime(object? input, string format, IFormatProvider formatProvider)
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
            return input!.ToString()!.ToDateTime(format, formatProvider);
        }

        public static Guid ToGuid(object? input)
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
            return input!.ToString()!.ToGuid();
        }

        public static byte[] ToBytes(object? input)
        {
            if (input == null || input == DBNull.Value)
            {
                return new byte[0];
            }
            Type type = input.GetType();
            if (type == typeof(byte[]))
            {
                return (byte[])(input);
            }
            else if (type == typeof(string))
            {
                return Encoding.UTF8.GetBytes(input.ToString()!);
            }
            return new byte[0];
        }

        public static DataType? ToDataType<DataType>(object? input)
        {
            if (input == null || input == DBNull.Value)
            {
                return default(DataType);
            }

            Type t = typeof(DataType);
            object? newValue = ChangeType(input, t);

            if (newValue == null)
                return default(DataType);
            else
                return (DataType)newValue;
        }

        public static object? ChangeType(object? input, Type t)
        {
            object? newValue = null;
            if (t == typeof(string))
            {
                newValue = input == null ? null : input.ToString();
            }
            else if (t == typeof(bool))
            {
                newValue = ToBoolean(input);
            }
            else if (t == typeof(bool?))
            {
                newValue = ToNullableBoolean(input);
            }
            else if (t == typeof(byte))
            {
                newValue = ToByte(input);
            }
            else if (t == typeof(byte?))
            {
                newValue = ToNullableByte(input);
            }
            else if (t == typeof(int))
            {
                newValue = ToInt(input);
            }
            else if (t == typeof(int?))
            {
                newValue = ToNullableInt(input);
            }
            else if (t == typeof(short))
            {
                newValue = ToInt16(input);
            }
            else if (t == typeof(short?))
            {
                newValue = ToNullableInt16(input);
            }
            else if (t == typeof(long))
            {
                newValue = ToInt64(input);
            }
            else if (t == typeof(long?))
            {
                newValue = ToNullableInt64(input);
            }
            else if (t == typeof(float))
            {
                newValue = ToFloat(input);
            }
            else if (t == typeof(float?))
            {
                newValue = ToNullableFloat(input);
            }
            else if (t == typeof(double))
            {
                newValue = ToDouble(input);
            }
            else if (t == typeof(double?))
            {
                newValue = ToNullableDouble(input);
            }
            else if (t == typeof(decimal))
            {
                newValue = ToDecimal(input);
            }
            else if (t == typeof(decimal?))
            {
                newValue = ToNullableDecimal(input);
            }
            else if (t == typeof(DateTime))
            {
                newValue = ToDateTime(input);
            }
            else if (t == typeof(DateTime?))
            {
                newValue = ToNullableDateTime(input);
            }
            else if (t == typeof(Guid))
            {
                newValue = ToGuid(input);
            }
            else if (t == typeof(byte[]))
            {
                newValue = ToBytes(input);
            }
            else
            {
                if (input != null)
                {
                    if (Reflector.IsNullableType(t))
                    {
                        NullableConverter nullableConverter = new NullableConverter(t);
                        t = nullableConverter.UnderlyingType;
                    }
                    newValue = Convert.ChangeType(newValue, t);
                }
            }

            return newValue;
        }
    }
}