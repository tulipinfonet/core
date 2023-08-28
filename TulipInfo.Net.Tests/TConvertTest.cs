using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TulipInfo.Net.Tests
{
    [TestClass]
    public class TConvertTest
    {
        [TestMethod]
        public void Test_ToInt()
        {
            double input = 12.34;
            Assert.AreEqual(12, TConvert.ToInt(input));

            double? input2 = 12.34;
            Assert.AreEqual(12, TConvert.ToInt(input2));

            double? input3 = null;
            Assert.AreEqual(0, TConvert.ToInt(input3));
        }
    }
}
