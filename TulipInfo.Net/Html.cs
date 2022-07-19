using System;
using System.Collections.Generic;
using System.Text;

namespace TulipInfo.Net
{
    public static class Html
    {
        public static string RemoveTags(string input)
        {
            string str = input;
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("<[^>]*>");
            str = rx.Replace(str, "");
            return str;
        }
    }
}
