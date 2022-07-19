using System;
using System.Collections.Generic;
using System.Text;

namespace TulipInfo.Net
{
    public static class Validator
    {
        #region CheckRequired
        public static bool CheckRequired(string input, bool allowEmptySpace = false)
        {
            if (allowEmptySpace)
                return !string.IsNullOrEmpty(input);
            else
                return !string.IsNullOrWhiteSpace(input);
        }

        public static bool CheckRequired(bool? input)
        {
            return input.HasValue;
        }

        public static bool CheckRequired(int input)
        {
            return input != 0;
        }

        public static bool CheckRequired(int? input, bool allowZero = false)
        {
            return input.HasValue && (allowZero || input != 0);
        }

        public static bool CheckRequired(short input)
        {
            return input != 0;
        }

        public static bool CheckRequired(short? input, bool allowZero = false)
        {
            return input.HasValue && (allowZero || input != 0);
        }

        public static bool CheckRequired(long input)
        {
            return input != 0;
        }

        public static bool CheckRequired(long? input, bool allowZero = false)
        {
            return input.HasValue && (allowZero || input != 0);
        }

        public static bool CheckRequired(float input)
        {
            return input != 0;
        }

        public static bool CheckRequired(float? input, bool allowZero = false)
        {
            return input.HasValue && (allowZero || input != 0);
        }

        public static bool CheckRequired(double input)
        {
            return input != 0;
        }

        public static bool CheckRequired(double? input, bool allowZero = false)
        {
            return input.HasValue && (allowZero || input != 0);
        }

        public static bool CheckRequired(decimal input)
        {
            return input != 0;
        }

        public static bool CheckRequired(decimal? input, bool allowZero = false)
        {
            return input.HasValue && (allowZero || input != 0);
        }

        public static bool CheckRequired(DateTime input)
        {
            return input != DateTime.MinValue;
        }

        public static bool CheckRequired(DateTime? input, bool allowMinDate = false)
        {
            return input.HasValue && (allowMinDate || input != DateTime.MinValue);
        }

        public static bool CheckRequired(Guid input)
        {
            return input != Guid.Empty;
        }

        public static bool CheckRequired(byte[] input)
        {
            return input != null && input.Length > 0;
        }

        public static bool CheckRequired(object input)
        {
            return input != null && input != DBNull.Value;
        }
        #endregion

        #region CheckMinValue
        public static bool CheckMinValue(int input,int minValue)
        {
            return input >= minValue;
        }
        public static bool CheckMinValue(int? input,int minValue, int defaultValue=0)
        {
            return (input.HasValue ? input.Value : defaultValue) >= minValue;
        }

        public static bool CheckMinValue(short input, short minValue)
        {
            return input >= minValue;
        }
        public static bool CheckMinValue(short? input, short minValue, short defaultValue = 0)
        {
            return (input.HasValue ? input.Value : defaultValue) >= minValue;
        }

        public static bool CheckMinValue(long input, long minValue)
        {
            return input >= minValue;
        }
        public static bool CheckMinValue(long? input, long minValue, long defaultValue = 0)
        {
            return (input.HasValue ? input.Value : defaultValue) >= minValue;
        }

        public static bool CheckMinValue(float input, float minValue)
        {
            return input >= minValue;
        }
        public static bool CheckMinValue(float? input, float minValue, float defaultValue = 0)
        {
            return (input.HasValue ? input.Value : defaultValue) >= minValue;
        }

        public static bool CheckMinValue(double input, double minValue)
        {
            return input >= minValue;
        }
        public static bool CheckMinValue(double? input, double minValue, double defaultValue = 0)
        {
            return (input.HasValue ? input.Value : defaultValue) >= minValue;
        }

        public static bool CheckMinValue(decimal input, decimal minValue)
        {
            return input >= minValue;
        }
        public static bool CheckMinValue(decimal? input, decimal minValue, decimal defaultValue = 0)
        {
            return (input.HasValue ? input.Value : defaultValue) >= minValue;
        }

        public static bool CheckMinValue(DateTime input, DateTime minValue)
        {
            return input >= minValue;
        }
        public static bool CheckMinValue(DateTime? input, DateTime minValue)
        {
            return (input.HasValue ? input.Value : DateTime.MinValue) >= minValue;
        }

        public static bool CheckMinValue(DateTime? input, DateTime minValue,DateTime defaultValue)
        {
            return (input.HasValue ? input.Value : defaultValue) >= minValue;
        }
        #endregion

        #region CheckMaxValue
        public static bool CheckMaxValue(int input, int maxValue)
        {
            return input <= maxValue;
        }
        public static bool CheckMaxValue(int? input, int maxValue, int defaultValue = 0)
        {
            return (input.HasValue ? input.Value : defaultValue) <= maxValue;
        }

        public static bool CheckMaxValue(short input, short maxValue)
        {
            return input <= maxValue;
        }
        public static bool CheckMaxValue(short? input, short maxValue, short defaultValue = 0)
        {
            return (input.HasValue ? input.Value : defaultValue) <= maxValue;
        }

        public static bool CheckMaxValue(long input, long maxValue)
        {
            return input <= maxValue;
        }
        public static bool CheckMaxValue(long? input, long maxValue, long defaultValue = 0)
        {
            return (input.HasValue ? input.Value : defaultValue) <= maxValue;
        }

        public static bool CheckMaxValue(float input, float maxValue)
        {
            return input <= maxValue;
        }
        public static bool CheckMaxValue(float? input, float maxValue, float defaultValue = 0)
        {
            return (input.HasValue ? input.Value : defaultValue) <= maxValue;
        }

        public static bool CheckMaxValue(double input, double maxValue)
        {
            return input <= maxValue;
        }
        public static bool CheckMaxValue(double? input, double maxValue, double defaultValue = 0)
        {
            return (input.HasValue ? input.Value : defaultValue) <= maxValue;
        }

        public static bool CheckMaxValue(decimal input, decimal maxValue)
        {
            return input <= maxValue;
        }
        public static bool CheckMaxValue(decimal? input, decimal maxValue, decimal defaultValue = 0)
        {
            return (input.HasValue ? input.Value : defaultValue) <= maxValue;
        }

        public static bool CheckMaxValue(DateTime input, DateTime maxValue)
        {
            return input <= maxValue;
        }
        public static bool CheckMaxValue(DateTime? input, DateTime maxValue)
        {
            return (input.HasValue ? input.Value : DateTime.MaxValue) <= maxValue;
        }

        public static bool CheckMaxValue(DateTime? input, DateTime maxValue, DateTime defaultValue)
        {
            return (input.HasValue ? input.Value : defaultValue) <= maxValue;
        }
        #endregion

        #region CheckRange
        public static bool CheckRange(int input, int minValue, int maxValue)
        {
            return CheckMinValue(input,minValue) && CheckMaxValue(input,maxValue);
        }
        public static bool CheckRange(int? input, int minValue, int maxValue, int defaultValue =0)
        {
            return CheckMinValue(input, minValue,defaultValue) && CheckMaxValue(input, maxValue, defaultValue);
        }
        public static bool CheckRange(short input, short minValue, short maxValue)
        {
            return CheckMinValue(input, minValue) && CheckMaxValue(input, maxValue);
        }
        public static bool CheckRange(short? input, short minValue, short maxValue, short defaultValue = 0)
        {
            return CheckMinValue(input, minValue, defaultValue) && CheckMaxValue(input, maxValue, defaultValue);
        }
        public static bool CheckRange(long input, long minValue, long maxValue)
        {
            return CheckMinValue(input, minValue) && CheckMaxValue(input, maxValue);
        }
        public static bool CheckRange(long? input, long minValue, long maxValue, long defaultValue = 0)
        {
            return CheckMinValue(input, minValue, defaultValue) && CheckMaxValue(input, maxValue, defaultValue);
        }
        public static bool CheckRange(float input, float minValue, float maxValue)
        {
            return CheckMinValue(input, minValue) && CheckMaxValue(input, maxValue);
        }
        public static bool CheckRange(float? input, float minValue, float maxValue, float defaultValue = 0)
        {
            return CheckMinValue(input, minValue, defaultValue) && CheckMaxValue(input, maxValue, defaultValue);
        }
        public static bool CheckRange(double input, double minValue, double maxValue)
        {
            return CheckMinValue(input, minValue) && CheckMaxValue(input, maxValue);
        }
        public static bool CheckRange(double? input, double minValue, double maxValue, double defaultValue = 0)
        {
            return CheckMinValue(input, minValue, defaultValue) && CheckMaxValue(input, maxValue, defaultValue);
        }
        public static bool CheckRange(decimal input, decimal minValue, decimal maxValue)
        {
            return CheckMinValue(input, minValue) && CheckMaxValue(input, maxValue);
        }
        public static bool CheckRange(decimal? input, decimal minValue, decimal maxValue, decimal defaultValue = 0)
        {
            return CheckMinValue(input, minValue, defaultValue) && CheckMaxValue(input, maxValue, defaultValue);
        }
        public static bool CheckRange(DateTime input, DateTime minValue, DateTime maxValue)
        {
            return CheckMinValue(input, minValue) && CheckMaxValue(input, maxValue);
        }
        public static bool CheckRange(DateTime? input, DateTime minValue, DateTime maxValue)
        {
            return CheckMinValue(input, minValue) && CheckMaxValue(input, maxValue);
        }
        public static bool CheckRange(DateTime? input, DateTime minValue, DateTime maxValue,DateTime defaultValue)
        {
            return CheckMinValue(input, minValue, defaultValue) && CheckMaxValue(input, maxValue, defaultValue);
        }
        #endregion
    }
}
