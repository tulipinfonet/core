using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TulipInfo.Net.Tests
{
    [TestClass]
    public class HtmlTest
    {
        [TestMethod]
        public void Test_RemoveTags()
        {
            string input = @"<html><head>abc def</head><body>ghi <div style='display:none'>bababa</div></body></html>";

            string str = Html.RemoveTags(input);
            Assert.AreEqual("abc defghi bababa", str);
        }
    }
}
