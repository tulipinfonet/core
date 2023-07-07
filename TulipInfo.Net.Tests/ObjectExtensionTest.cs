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
            var dt2=dt.ToDataType<DateTime?>();
            Assert.AreEqual(dt, dt2);

            dt = null;
            dt2 = dt.ToDataType<DateTime?>();
            Assert.IsNull(dt2);
        }
    }
}
