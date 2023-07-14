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
            return TConvert.ToBoolean(sdr[fieldName]);
        }

        public static bool? GetSafeBit2(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToBoolean2(sdr[fieldName]);
        }

        public static byte GetSafeByte(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToByte(sdr[fieldName]);
        }

        public static byte? GetSafeByte2(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToByte2(sdr[fieldName]);
        }

        public static int GetSafeInt(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToInt(sdr[fieldName]);
        }

        public static int? GetSafeInt2(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToInt2(sdr[fieldName]);
        }

        public static short GetSafeInt16(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToInt16(sdr[fieldName]);
        }

        public static short? GetSafeInt162(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToInt162(sdr[fieldName]);
        }

        public static long GetSafeInt64(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToInt64(sdr[fieldName]);
        }

        public static long? GetSafeInt642(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToInt642(sdr[fieldName]);
        }

        public static float GetSafeFloat(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToFloat(sdr[fieldName]);
        }

        public static float? GetSafeFloat2(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToFloat2(sdr[fieldName]);
        }

        public static double GetSafeDouble(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToDouble(sdr[fieldName]);
        }

        public static double? GetSafeDouble2(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToDouble2(sdr[fieldName]);
        }

        public static decimal GetSafeDecimal(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToDecimal(sdr[fieldName]);
        }

        public static decimal? GetSafeDecimal2(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToDecimal2(sdr[fieldName]);
        }

        public static DateTime GetSafeDateTime(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToDateTime(sdr[fieldName]);
        }

        public static DateTime? GetSafeDateTime2(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToDateTime2(sdr[fieldName]);
        }

        public static Guid GetSafeGuid(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToGuid(sdr[fieldName]);
        }

        public static Guid? GetSafeGuid2(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToGuid2(sdr[fieldName]);
        }

        public static byte[] GetSafeBytes(this DbDataReader sdr, string fieldName)
        {
            return TConvert.ToBytes(sdr[fieldName]);
        }

        public static string GetSafeString(this DbDataReader sdr, string fieldName)
        {
            if (sdr[fieldName] != DBNull.Value)
            {
                try
                {
                    return sdr[fieldName].ToString()!;
                }
                catch
                {
                    //ignore
                }
            }
            return "";
        }

    }
}
