using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TulipInfo.Net.Tests
{
    [TestClass]
    public class DateTimeTest
    {
        [TestMethod]
        public void Test_MonthDiff()
        {
            DateTime from = new DateTime(2021, 1, 1);
            DateTime to = new DateTime(2021, 2, 1);
            Assert.AreEqual(1, from.MonthDiff(to));

            from = new DateTime(2021, 1, 31);
            to = new DateTime(2021, 2, 1);
            Assert.AreEqual(0, from.MonthDiff(to));
            Assert.AreEqual(1, from.MonthDiff(to,false));

            from = new DateTime(2021, 1, 31);
            to = new DateTime(2021, 2, 15);
            Assert.AreEqual(0, from.MonthDiff(to));

            from = new DateTime(2021, 1, 31);
            to = new DateTime(2021, 2, 28);
            Assert.AreEqual(1, from.MonthDiff(to));

            from = new DateTime(2021, 2, 28); 
            to = new DateTime(2021, 1, 31);
            Assert.AreEqual(-1, from.MonthDiff(to));

            from = new DateTime(2021, 1, 31);
            to = new DateTime(2021, 3, 15);
            Assert.AreEqual(1, from.MonthDiff(to));

            from = new DateTime(2021, 1, 31);
            to = new DateTime(2021, 3, 30);
            Assert.AreEqual(1, from.MonthDiff(to));

            from = new DateTime(2021, 1, 31);
            to = new DateTime(2021, 3, 31);
            Assert.AreEqual(2, from.MonthDiff(to));

            from = new DateTime(2021, 1, 31);
            to = new DateTime(2021, 4, 15);
            Assert.AreEqual(2, from.MonthDiff(to));

            from = new DateTime(2021, 1, 31);
            to = new DateTime(2021, 4, 30);
            Assert.AreEqual(3, from.MonthDiff(to));

            from = new DateTime(2020, 12, 31);
            to = new DateTime(2021, 1, 31);
            Assert.AreEqual(1, from.MonthDiff(to));

            from = new DateTime(2020, 12, 10);
            to = new DateTime(2021, 4, 10);
            Assert.AreEqual(4, from.MonthDiff(to));
        }
    }
}
