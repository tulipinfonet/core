using System;
using System.Collections.Generic;
using System.Text;

namespace TulipInfo.Net
{
    public static class DateTimeExtension
    {
        public static string ToShortDateString(this DateTime? dtm)
        {
            if(dtm.HasValue)
            {
                return dtm.Value.ToShortDateString();
            }
            else
            {
                return "";
            }
        }

        public static string ToShortTimeString(this DateTime? dtm)
        {
            if (dtm.HasValue)
            {
                return dtm.Value.ToShortTimeString();
            }
            else
            {
                return "";
            }
        }

        public static string ToLongDateString(this DateTime? dtm)
        {
            if (dtm.HasValue)
            {
                return dtm.Value.ToLongDateString();
            }
            else
            {
                return "";
            }
        }

        public static string ToLongTimeString(this DateTime? dtm)
        {
            if (dtm.HasValue)
            {
                return dtm.Value.ToLongTimeString();
            }
            else
            {
                return "";
            }
        }

        public static string ToFormattedString(this DateTime? dtm, string format)
        {
            if (dtm.HasValue)
            {
                return dtm.Value.ToFormattedString(format);
            }
            else
            {
                return "";
            }
        }

        public static string ToFormattedString(this DateTime dtm, string format)
        {
            return dtm.ToString(format, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static DateTime ToStartDateTime(this DateTime dtm)
        {
            return dtm.Date;
        }

        public static DateTime ToEndDateTime(this DateTime dtm)
        {
            return dtm.Date.AddDays(1).AddTicks(-1);
        }

        public static DateTime ToStartDateTime(this DateTime? dtm)
        {
            if (dtm.HasValue)
            {
                return dtm.Value.ToStartDateTime();
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime ToEndDateTime(this DateTime? dtm)
        {
            if(dtm.HasValue)
            {
                return dtm.Value.ToEndDateTime();
            }
            else
            {
                return DateTime.MaxValue;
            }
        }

        public static int ToAge(this DateTime dtm)
        {
            return dtm.ToAge(DateTime.Now);
        }

        public static int ToAge(this DateTime dtm, DateTime now)
        {
            int age = now.Year - dtm.Year;
            if (now.Month < dtm.Month || (now.Month == dtm.Month && now.Day < dtm.Day))
            {
                age--;
            }

            return age < 0 ? 0 : age;
        }

        public static int ToAge(this DateTime? dtm)
        {
            return dtm.HasValue? dtm.Value.ToAge(DateTime.Now):0;
        }

        public static int ToAge(this DateTime? dtm, DateTime now)
        {
            return dtm.HasValue ? dtm.Value.ToAge(now) : 0;
        }

        public static int MonthDiff(this DateTime from, DateTime to, bool includeDay=true)
        {
            int monthDiff = Math.Abs((to.Year * 12 + to.Month) - (from.Year * 12 + from.Month));
            if (includeDay)
            {
                int fromDay = from.Day;
                int toDay = to.Day;
                int toDaysInMonth = DateTime.DaysInMonth(to.Year, to.Month);
                if (fromDay > toDay && toDay < toDaysInMonth)
                {
                    monthDiff -= 1;
                }
            }

            return from < to ? monthDiff : -monthDiff;
        }
    }
}
