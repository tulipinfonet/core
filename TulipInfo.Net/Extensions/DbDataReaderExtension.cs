using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace TulipInfo.Net
{
    public static class DbDataReaderExtension
    {
        public static string GetSafeString(this DbDataReader sdr, string fieldName)
        {
            if (sdr[fieldName] != DBNull.Value)
            {
                try
                {
                    return sdr[fieldName].ToString();
                }
                catch
                {
                    //ignore
                }
            }
            return "";
        }

        public static Guid GetSafeGuid(this DbDataReader sdr, string fieldName)
        {
            string strValue = sdr.GetSafeString(fieldName);
            Guid result = Guid.Empty;
            Guid.TryParse(strValue, out result);
            return result;
        }

        public static DateTime GetSafeDateTime(this DbDataReader sdr, string fieldName)
        {
            if (sdr[fieldName] != DBNull.Value)
            {
                try
                {
                    return sdr.GetDateTime(fieldName);
                }
                catch
                {
                    //ignore
                }
            }
            return DateTime.MinValue;
        }

        public static DateTime? GetSafeDateTime2(this DbDataReader sdr, string fieldName)
        {
            if (sdr[fieldName] != DBNull.Value)
            {
                try
                {
                    return sdr.GetDateTime(fieldName);
                }
                catch
                {
                    //ignore
                }
            }
            return null;
        }

        public static int GetSafeInt(this DbDataReader sdr, string fieldName)
        {
            if (sdr[fieldName] != DBNull.Value)
            {
                try
                {
                    return sdr.GetInt32(fieldName);
                }
                catch
                {
                    //ignore
                }
            }
            return 0;
        }

        public static int? GetSafeInt2(this DbDataReader sdr, string fieldName)
        {
            if (sdr[fieldName] != DBNull.Value)
            {
                try
                {
                    return sdr.GetInt32(fieldName);
                }
                catch
                {
                    //ignore
                }
            }
            return null;
        }

        public static bool GetSafeBit(this DbDataReader sdr, string fieldName)
        {
            if (sdr[fieldName] != DBNull.Value)
            {
                try
                {
                    string dbValue = sdr[fieldName].ToString().ToLower();
                    return dbValue == "1" || dbValue == "true" || dbValue == "y";
                }
                catch
                {
                    //ignore
                }
            }
            return false;
        }

        public static bool? GetSafeBit2(this DbDataReader sdr, string fieldName)
        {
            if (sdr[fieldName] != DBNull.Value)
            {
                try
                {
                    string dbValue = sdr[fieldName].ToString().ToLower();
                    if (dbValue == "1" || dbValue == "true" || dbValue == "y")
                    {
                        return true;
                    }
                    else if (dbValue == "0" || dbValue == "false" || dbValue == "n")
                    {
                        return false;
                    }
                }
                catch
                {
                    //ignore
                }
            }
            return null;
        }
    }
}
