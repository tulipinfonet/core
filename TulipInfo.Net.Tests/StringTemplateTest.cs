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

        [TestMethod]
        public void Test_Format_Html()
        {
            string text = @"hello ${name}, your one-time password is ${code}";
            string replaced = StringTemplate.Format(text, new
            {
                name = "lwu",
                code = "ab<>"
            },true);
            Assert.AreEqual("hello lwu, your one-time password is ab&lt;&gt;", replaced);

            text = @"hello ${name}, your code is ${code}, after encoding is ${code.html}";
            replaced = StringTemplate.Format(text, new
            {
                name = "lwu",
                code = "ab<>"
            });
            Assert.AreEqual("hello lwu, your code is ab<>, after encoding is ab&lt;&gt;", replaced);
        }

        [TestMethod]
        public void Test_Format_Dictionary()
        {
            string text = @"hello ${name}, your auth code is ${code} and will expired after ${exp} minutes <br/>, thank you ${name} <br/> ${now.yyyy-MM-dd}";
            string replaced = StringTemplate.Format(text, new System.Collections.Generic.Dictionary<string, object>()
            {
                {"name", "lwu"},
                {"code", 123},
                {"exp", 5 },
                {"now", new DateTime(2021, 5, 1) }
            });
            Assert.AreEqual("hello lwu, your auth code is 123 and will expired after 5 minutes <br/>, thank you lwu <br/> 2021-05-01", replaced);
        }

        [TestMethod]
        public void Test_Format_Dynamic()
        {
            string text = @"hello ${name}, your auth code is ${code} and will expired after ${exp} minutes <br/>, thank you ${name} <br/> ${now.yyyy-MM-dd}";
            dynamic formatData = new System.Dynamic.ExpandoObject();
            formatData.name = "lwu";
            formatData.code = 123;
            formatData.exp = 5;
            formatData.now = new DateTime(2021, 5, 1);
            string replaced = StringTemplate.Format(text, formatData);
            Assert.AreEqual("hello lwu, your auth code is 123 and will expired after 5 minutes <br/>, thank you lwu <br/> 2021-05-01", replaced);
        }
    }
}
