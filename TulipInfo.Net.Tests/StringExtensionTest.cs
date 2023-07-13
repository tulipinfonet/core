using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TulipInfo.Net.Tests
{
    [TestClass]
    public class StringExtensionTest
    {
        [TestMethod]
        public void Test_ToBoolean()
        {
            string input = null;
            Assert.AreEqual(false, input.ToBoolean());
            input = "y";
            Assert.AreEqual(true, input.ToBoolean());
            input = "Y";
            Assert.AreEqual(true, input.ToBoolean());
            input = "true";
            Assert.AreEqual(true, input.ToBoolean());
            input = "True";
            Assert.AreEqual(true, input.ToBoolean());
            input = "1";
            Assert.AreEqual(true, input.ToBoolean());
            input = "n";
            Assert.AreEqual(false, input.ToBoolean());
            input = "N";
            Assert.AreEqual(false, input.ToBoolean());
            input = "false";
            Assert.AreEqual(false, input.ToBoolean());
            input = "False";
            Assert.AreEqual(false, input.ToBoolean());
            input = "0";
            Assert.AreEqual(false, input.ToBoolean());
            input = "a";
            Assert.AreEqual(false, input.ToBoolean());
        }

        [TestMethod]
        public void Test_ToNullableBoolean()
        {
            string input = null;
            Assert.AreEqual(null, input.ToBoolean2());
            input = "y";
            Assert.AreEqual(true, input.ToBoolean2());
            input = "Y";
            Assert.AreEqual(true, input.ToBoolean2());
            input = "true";
            Assert.AreEqual(true, input.ToBoolean2());
            input = "True";
            Assert.AreEqual(true, input.ToBoolean2());
            input = "1";
            Assert.AreEqual(true, input.ToBoolean2());
            input = "n";
            Assert.AreEqual(false, input.ToBoolean2());
            input = "N";
            Assert.AreEqual(false, input.ToBoolean2());
            input = "false";
            Assert.AreEqual(false, input.ToBoolean2());
            input = "False";
            Assert.AreEqual(false, input.ToBoolean2());
            input = "0";
            Assert.AreEqual(false, input.ToBoolean2());
            input = "a";
            Assert.AreEqual(null, input.ToBoolean2());
        }
    }
}
