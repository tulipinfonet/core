using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace TulipInfo.Net
{
    public static class DbDataReaderExtension
    {
        public static bool GetSafeBit(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToBoolean(sdr.GetSafeValue(fieldName));
        }

        public static bool? GetSafeBit2(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToBoolean2(sdr.GetSafeValue(fieldName));
        }

        public static byte GetSafeByte(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToByte(sdr.GetSafeValue(fieldName));
        }

        public static byte? GetSafeByte2(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToByte2(sdr.GetSafeValue(fieldName));
        }

        public static int GetSafeInt(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToInt(sdr.GetSafeValue(fieldName));
        }

        public static int? GetSafeInt2(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToInt2(sdr.GetSafeValue(fieldName));
        }

        public static short GetSafeInt16(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToInt16(sdr.GetSafeValue(fieldName));
        }

        public static short? GetSafeInt162(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToInt162(sdr.GetSafeValue(fieldName));
        }

        public static long GetSafeInt64(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToInt64(sdr.GetSafeValue(fieldName));
        }

        public static long? GetSafeInt642(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToInt642(sdr.GetSafeValue(fieldName));
        }

        public static float GetSafeFloat(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToFloat(sdr.GetSafeValue(fieldName));
        }

        public static float? GetSafeFloat2(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToFloat2(sdr.GetSafeValue(fieldName));
        }

        public static double GetSafeDouble(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToDouble(sdr.GetSafeValue(fieldName));
        }

        public static double? GetSafeDouble2(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToDouble2(sdr.GetSafeValue(fieldName));
        }

        public static decimal GetSafeDecimal(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToDecimal(sdr.GetSafeValue(fieldName));
        }

        public static decimal? GetSafeDecimal2(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToDecimal2(sdr.GetSafeValue(fieldName));
        }

        public static DateTime GetSafeDateTime(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToDateTime(sdr.GetSafeValue(fieldName));
        }

        public static DateTime? GetSafeDateTime2(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToDateTime2(sdr.GetSafeValue(fieldName));
        }

        public static Guid GetSafeGuid(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToGuid(sdr.GetSafeValue(fieldName));
        }

        public static Guid? GetSafeGuid2(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToGuid2(sdr.GetSafeValue(fieldName));
        }

        public static byte[] GetSafeBytes(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToBytes(sdr.GetSafeValue(fieldName));
        }

        public static string GetSafeString(this DbDataReader sdr, string fieldName)
        {
            object? value = sdr.GetSafeValue(fieldName);
            if (value != null)
            {
                return sdr[fieldName].ToString()!;
            }
            return "";
        }

        private static object? GetSafeValue(this DbDataReader sdr, string fieldName)
        {
            object? value = null;
            for (var i = 0; i < sdr.FieldCount; i++)
            {
                if (fieldName.Equals(sdr.GetName(i), StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!sdr.IsDBNull(i))
                    {
                        value = sdr[i];
                    }
                    break;
                }
            }

            return value;
        }

    }
}
