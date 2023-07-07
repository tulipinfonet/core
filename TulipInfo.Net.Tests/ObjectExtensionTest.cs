using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TulipInfo.Net.Tests
{
    [TestClass]
    public class ObjectExtensionTest
    {
        [TestMethod]
        public void DateToDate2()
        {
            object dt = DateTime.Now;

            var dt2=TConvert.ToDataType<DateTime?>(dt);
            Assert.AreEqual(dt, dt2);

            dt = null;
            dt2 = TConvert.ToDataType<DateTime?>(dt);
            Assert.IsNull(dt2);
        }
    }
}
