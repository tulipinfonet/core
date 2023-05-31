using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TulipInfo.Net
{
    public static class TRegex
    {
        //https://blog.csdn.net/to_study/article/details/106832942
        public const string ChineseMobilePattern = "^1(3([0-35-9]\\d|4[1-8])|4[14-9]\\d|5([0-35689]\\d|7[1-79])|66\\d|7[2-35-8]\\d|8\\d{2}|9[13589]\\d)\\d{7}$";
        public const string ChineseMobileSimplePattern = "^1[0-9]{10}$";
        //https://www.oreilly.com/library/view/regular-expressions-cookbook/9781449327453/ch04s01.html
        public const string EmailAddressPattern = @"^[\w!#$%&'*+/=?`{|}~^-]+(?:\.[\w!#$%&'*+/=?`{|}~^-]+)*@(?:[A-Z0-9-]+\.)+[A-Z]{2,6}$";
        /// <summary>
        /// he password length must be greater than or equal to 8
        //The password must contain one or more uppercase characters
        //The password must contain one or more lowercase characters
        //The password must contain one or more numeric values
        //The password must contain one or more special characters
        /// </summary>
        public const string StrongPasswordPattern = @"(?=^.{8,}$)(?=.*\d)(?=.*[^\w\s]+)(?=.*[A-Z])(?=.*[a-z]).*$";
        /// <summary>
        /// he password length must be greater than or equal to 8
        //The password must contain one or more uppercase characters
        //The password must contain one or more lowercase characters
        //The password must contain one or more numeric values
        /// </summary>
        public const string StrongPasswordPatternWithOutSymbol = @"(?=^.{8,}$)(?=.*\d)(?=.*[A-Z])(?=.*[a-z]).*$";
        private const string UrlComponentPattern= "(([0-9a-z_!~*'().&=+$%-]+:)?[0-9a-z_!~*'().&=+$%-]+@)?(([0-9]{1,3}\\.){3}[0-9]{1,3}|([0-9a-z_!~*'()-]+\\.)*([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\\.[a-z]{2,6})(:[0-9]{1,4})?((\\/?)|(\\/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+\\/?)";
        public const string UrlWithSchemeOptionalPattern = "^((http[s]?):\\/\\/)?"+UrlComponentPattern;
        public const string UrlWithSchemeRequiredPattern = "^((http[s]?):\\/\\/){1}" + UrlComponentPattern;
        public static bool IsChineseMobile(string input, bool simpleCheck = false)
        {
            if (simpleCheck)
            {
                return Regex.IsMatch(input, ChineseMobileSimplePattern);
            }
            else
            {
                return Regex.IsMatch(input, ChineseMobilePattern);
            }
        }

        public static bool IsEmail(string input)
        {
            return Regex.IsMatch(input, EmailAddressPattern, RegexOptions.IgnoreCase);
        }

        public static bool IsMatch(string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }
        
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern,options);
        }
    }
}
