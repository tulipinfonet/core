using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;

namespace TulipInfo.Net.Tests
{
    [TestClass]
    public class StringTemplateTest
    {
        [TestMethod]
        public void Test_Format()
        {
            string text = @"hello ${name}, your auth code is ${code} and will expired after ${exp} minutes <br/>, thank you ${name} <br/> ${now.yyyy-MM-dd}";
            string replaced = StringTemplate.Format(text, new
            {
                name="lwu",
                code=123,
                exp=5,
                now=new DateTime(2021,5,1)
            });
            Assert.AreEqual("hello lwu, your auth code is 123 and will expired after 5 minutes <br/>, thank you lwu <br/> 2021-05-01", replaced);
        }
    }
}
