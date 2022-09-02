using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;

namespace TulipInfo.Net.Tests
{
    [TestClass]
    public class ReflectorTest
    {
        [TestMethod]
        public void Test_Convert_Obj_To_Dictionary()
        {
            var obj = new
            {
                name = "lwu",
                code = 123,
                exp = 5,
                now = new DateTime(2021, 5, 1)
            };

            var dic = Reflector.ConvertToDictionary(obj);
            Assert.AreEqual("lwu", dic["name"]);
            Assert.AreEqual(123, dic["code"]);
            Assert.AreEqual(5, dic["exp"]);
            Assert.AreEqual(new DateTime(2021, 5, 1), dic["now"]);
        }

        [TestMethod]
        public void Test_Convert_Dic_To_Dictionary()
        {
            var obj = new System.Collections.Generic.Dictionary<string, string>()
            {
                {"name", "lwu"}
            };

            var dic = Reflector.ConvertToDictionary(obj);
            Assert.AreEqual("lwu", dic["name"]);
        }
    }
}
