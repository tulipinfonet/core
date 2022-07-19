using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TulipInfo.Net.Tests
{
    [TestClass]
    public class EnumerationTest
    {
        public enum EnumTestData
        {
            Unknow=0,
            First=1,
            [System.ComponentModel.Description("Second Element")]
            Second=2
        }

        [TestMethod]
        public void Test_GetList()
        {
            var list = Enumeration.GetKeyValueList<EnumTestData>();
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("0", list[0].Key);
            Assert.AreEqual("Unknow", list[0].Value);
            Assert.AreEqual("2", list[2].Key);
            Assert.AreEqual("Second Element", list[2].Value);
        }

        [TestMethod]
        public void Test_Parse()
        {
            EnumTestData v = Enumeration.Parse<EnumTestData>("");
            Assert.AreEqual(EnumTestData.Unknow, v);

            v = Enumeration.Parse<EnumTestData>("Unknow");
            Assert.AreEqual(EnumTestData.Unknow, v);

            v = Enumeration.Parse<EnumTestData>("First");
            Assert.AreEqual(EnumTestData.First, v);

            v = Enumeration.Parse<EnumTestData>("Second");
            Assert.AreEqual(EnumTestData.Second, v);

            v = Enumeration.Parse<EnumTestData>(0);
            Assert.AreEqual(EnumTestData.Unknow, v);

            v = Enumeration.Parse<EnumTestData>("0");
            Assert.AreEqual(EnumTestData.Unknow, v);

            v = Enumeration.Parse<EnumTestData>(1);
            Assert.AreEqual(EnumTestData.First, v);

            v = Enumeration.Parse<EnumTestData>("1");
            Assert.AreEqual(EnumTestData.First, v);

            v = Enumeration.Parse<EnumTestData>(2);
            Assert.AreEqual(EnumTestData.Second, v);

            v = Enumeration.Parse<EnumTestData>("2");
            Assert.AreEqual(EnumTestData.Second, v);

            v = Enumeration.Parse<EnumTestData>("abc");
            Assert.AreEqual(EnumTestData.Unknow, v);
        }

    }
}
