using System;
using System.Collections.Generic;
using System.Text;

namespace TulipInfo.Net
{
    public static class NumberExtension
    {
        /// <summary>
        /// Remove the Trailing Zeros
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal Normalize(this decimal value)
        {
            //http://blog.zerosharp.com/how-to-remove-the-trailing-zeros-of-precision-from-a-c-number-decimal/
            return value / 1.000000000000000000000000000000000m;
        }

        public static decimal Normalize(this decimal? value)
        {
            if (value.HasValue)
            {
                return value.Value.Normalize();
            }
            return 0;
        }

        public static decimal FormatDecimal(this decimal value, string format)
        {
            if (value != 0)
            {
                string strInput = value.ToString(format);
                if (!string.IsNullOrWhiteSpace(strInput))
                {
                    return Convert.ToDecimal(strInput);
                }
            }
            return 0;
        }

        public static decimal FormatDecimal(this decimal? value, string format)
        {
            if (value.HasValue)
            {
                value.Value.FormatDecimal(format);
            }
            return 0;
        }
    }
}
