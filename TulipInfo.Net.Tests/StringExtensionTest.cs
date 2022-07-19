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
            Assert.AreEqual(null, input.ToNullableBoolean());
            input = "y";
            Assert.AreEqual(true, input.ToNullableBoolean());
            input = "Y";
            Assert.AreEqual(true, input.ToNullableBoolean());
            input = "true";
            Assert.AreEqual(true, input.ToNullableBoolean());
            input = "True";
            Assert.AreEqual(true, input.ToNullableBoolean());
            input = "1";
            Assert.AreEqual(true, input.ToNullableBoolean());
            input = "n";
            Assert.AreEqual(false, input.ToNullableBoolean());
            input = "N";
            Assert.AreEqual(false, input.ToNullableBoolean());
            input = "false";
            Assert.AreEqual(false, input.ToNullableBoolean());
            input = "False";
            Assert.AreEqual(false, input.ToNullableBoolean());
            input = "0";
            Assert.AreEqual(false, input.ToNullableBoolean());
            input = "a";
            Assert.AreEqual(null, input.ToNullableBoolean());
        }
    }
}
