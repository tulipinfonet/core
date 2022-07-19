using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TulipInfo.Net.Tests
{
    [TestClass]
    public class Base64ExtensionTest
    {
        [TestMethod]
        public void Test()
        {
            string input = "http://www.tulipinfo.net?t=abc&f=aaa";
            string output =Base64.UrlEncode(input);
            string decode =Base64.UrlDecode(output);
            Assert.AreEqual(input, decode);
        }
    }
}
