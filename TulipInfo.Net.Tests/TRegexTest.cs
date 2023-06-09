using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TulipInfo.Net.Tests
{
    [TestClass]
    public class TRegexTest
    {
        [TestMethod]
        public void Test_Email()
        {
            Assert.AreEqual(false, TRegex.IsEmail("123"));
            Assert.AreEqual(false, TRegex.IsEmail("abc"));
            Assert.AreEqual(false, TRegex.IsEmail("abc@cc"));
            Assert.AreEqual(false, TRegex.IsEmail("abc.cc@dd"));

            Assert.AreEqual(true, TRegex.IsEmail("123@google.com"));
            Assert.AreEqual(true, TRegex.IsEmail("123@google.us"));
            Assert.AreEqual(true, TRegex.IsEmail("123@google.com.us"));

            Assert.AreEqual(true, TRegex.IsEmail("abc@google.com"));
            Assert.AreEqual(true, TRegex.IsEmail("abc@google.us"));
            Assert.AreEqual(true, TRegex.IsEmail("abc@google.com.us"));

            Assert.AreEqual(true, TRegex.IsEmail("abc.aab@google.com"));
            Assert.AreEqual(true, TRegex.IsEmail("123.dd@google.us"));
            Assert.AreEqual(true, TRegex.IsEmail("123.123@google.com.us"));
        }

        [TestMethod]
        public void Test_ChineseMobile()
        {
            Assert.AreEqual(false, TRegex.IsChineseMobile("aaaaa"));
            Assert.AreEqual(false, TRegex.IsChineseMobile("343232"));
            Assert.AreEqual(false, TRegex.IsChineseMobile("111231"));
            Assert.AreEqual(false, TRegex.IsChineseMobile("32234234231"));
            Assert.AreEqual(false, TRegex.IsChineseMobile("abdredetd34"));

            Assert.AreEqual(true, TRegex.IsChineseMobile("13454152545"));
            Assert.AreEqual(true, TRegex.IsChineseMobile("13354152545"));
            Assert.AreEqual(true, TRegex.IsChineseMobile("15854152545"));

            Assert.AreEqual(true, TRegex.IsChineseMobile("11111111111",true));
        }

        [TestMethod]
        public void Test_StrongPassword()
        {
            Assert.AreEqual(true, TRegex.IsMatch("Password1!", TRegex.StrongPasswordPattern));
            Assert.AreEqual(true, TRegex.IsMatch("Password1(", TRegex.StrongPasswordPattern));
            Assert.AreEqual(true, TRegex.IsMatch("Passwo{d1", TRegex.StrongPasswordPattern));
            Assert.AreEqual(true, TRegex.IsMatch("Pa0dss1!", TRegex.StrongPasswordPattern));
            Assert.AreEqual(true, TRegex.IsMatch("1@h_(uNt", TRegex.StrongPasswordPattern));
            Assert.AreEqual(false, TRegex.IsMatch("Pa9-s1!", TRegex.StrongPasswordPattern));
            Assert.AreEqual(false, TRegex.IsMatch("password", TRegex.StrongPasswordPattern));
            Assert.AreEqual(false, TRegex.IsMatch("PASSWORD", TRegex.StrongPasswordPattern));
            Assert.AreEqual(false, TRegex.IsMatch("Password", TRegex.StrongPasswordPattern));
            Assert.AreEqual(false, TRegex.IsMatch("Password!", TRegex.StrongPasswordPattern));
            Assert.AreEqual(false, TRegex.IsMatch("password1", TRegex.StrongPasswordPattern));
            Assert.AreEqual(false, TRegex.IsMatch("1234!", TRegex.StrongPasswordPattern));
            Assert.AreEqual(false, TRegex.IsMatch("12345!", TRegex.StrongPasswordPattern));
            Assert.AreEqual(false, TRegex.IsMatch("12345", TRegex.StrongPasswordPattern));
            Assert.AreEqual(false, TRegex.IsMatch("$%$@#$*(&)<>:*$%$%", TRegex.StrongPasswordPattern));
        }

        [TestMethod]
        public void Test_StrongPasswordWithOutSymbol()
        {
            Assert.AreEqual(true, TRegex.IsMatch("Password1!", TRegex.StrongPasswordPatternWithOutSymbol));
            Assert.AreEqual(true, TRegex.IsMatch("Password1", TRegex.StrongPasswordPatternWithOutSymbol));
            Assert.AreEqual(true, TRegex.IsMatch("Password1(", TRegex.StrongPasswordPatternWithOutSymbol));
            Assert.AreEqual(true, TRegex.IsMatch("Pa0dss1{d1", TRegex.StrongPasswordPatternWithOutSymbol));
            Assert.AreEqual(true, TRegex.IsMatch("1@h_(uNt", TRegex.StrongPasswordPatternWithOutSymbol));
            Assert.AreEqual(true, TRegex.IsMatch("Pa9-ss1!", TRegex.StrongPasswordPatternWithOutSymbol));
            Assert.AreEqual(false, TRegex.IsMatch("Pas1!", TRegex.StrongPasswordPatternWithOutSymbol));
            Assert.AreEqual(false, TRegex.IsMatch("password", TRegex.StrongPasswordPatternWithOutSymbol));
            Assert.AreEqual(false, TRegex.IsMatch("PASSWORD", TRegex.StrongPasswordPatternWithOutSymbol));
            Assert.AreEqual(false, TRegex.IsMatch("Password", TRegex.StrongPasswordPatternWithOutSymbol));
            Assert.AreEqual(false, TRegex.IsMatch("Password!", TRegex.StrongPasswordPatternWithOutSymbol));
            Assert.AreEqual(false, TRegex.IsMatch("password1", TRegex.StrongPasswordPatternWithOutSymbol));
            Assert.AreEqual(false, TRegex.IsMatch("1234!", TRegex.StrongPasswordPatternWithOutSymbol));
            Assert.AreEqual(false, TRegex.IsMatch("12345!", TRegex.StrongPasswordPatternWithOutSymbol));
            Assert.AreEqual(false, TRegex.IsMatch("12345", TRegex.StrongPasswordPatternWithOutSymbol));
            Assert.AreEqual(false, TRegex.IsMatch("$%$@#$*(&)<>:*$%$%", TRegex.StrongPasswordPatternWithOutSymbol));
        }

        [TestMethod]
        public void Test_Url_Scheme_Optional()
        {
            Assert.AreEqual(true, TRegex.IsMatch("http://www.bing.com", TRegex.UrlPattern.FullWithSchemeOptional));
            Assert.AreEqual(true, TRegex.IsMatch("http://bing.com", TRegex.UrlPattern.FullWithSchemeOptional));
            Assert.AreEqual(true, TRegex.IsMatch("https://www.bing.com", TRegex.UrlPattern.FullWithSchemeOptional));
            Assert.AreEqual(true, TRegex.IsMatch("https://bing.com", TRegex.UrlPattern.FullWithSchemeOptional));
            Assert.AreEqual(true, TRegex.IsMatch("https://bing.com?q=abc", TRegex.UrlPattern.FullWithSchemeOptional));
            Assert.AreEqual(true, TRegex.IsMatch("bing.com", TRegex.UrlPattern.FullWithSchemeOptional));
            Assert.AreEqual(true, TRegex.IsMatch("www.bing.com", TRegex.UrlPattern.FullWithSchemeOptional));
            Assert.AreEqual(true, TRegex.IsMatch("www.bing.com/search", TRegex.UrlPattern.FullWithSchemeOptional));
            Assert.AreEqual(true, TRegex.IsMatch("www.bing.com/search?q=abc", TRegex.UrlPattern.FullWithSchemeOptional));
//            Assert.AreEqual(false, TRegex.IsMatch("bing", TRegex.UrlPattern.FullWithSchemeOptional));
           // Assert.AreEqual(false, TRegex.IsMatch("bing/search?q=abc", TRegex.UrlPattern.FullWithSchemeOptional));
            Assert.AreEqual(false, TRegex.IsMatch("ahttps://bing.com", TRegex.UrlPattern.FullWithSchemeOptional));
        }

        [TestMethod]
        public void Test_Url_Scheme_Required()
        {
            Assert.AreEqual(true, TRegex.IsMatch("http://www.bing.com", TRegex.UrlPattern.FullWithSchemeRequired));
            Assert.AreEqual(true, TRegex.IsMatch("http://bing.com", TRegex.UrlPattern.FullWithSchemeRequired));
            Assert.AreEqual(true, TRegex.IsMatch("https://www.bing.com", TRegex.UrlPattern.FullWithSchemeRequired));
            Assert.AreEqual(true, TRegex.IsMatch("https://bing.com", TRegex.UrlPattern.FullWithSchemeRequired));
            Assert.AreEqual(true, TRegex.IsMatch("https://bing.com?q=abc", TRegex.UrlPattern.FullWithSchemeRequired));
            Assert.AreEqual(true, TRegex.IsMatch("https://bing.com/serach", TRegex.UrlPattern.FullWithSchemeRequired));
            Assert.AreEqual(true, TRegex.IsMatch("https://bing.com/search?q=abc", TRegex.UrlPattern.FullWithSchemeRequired));
            Assert.AreEqual(true, TRegex.IsMatch("https://localhost/test", TRegex.UrlPattern.FullWithSchemeRequired));
            Assert.AreEqual(true, TRegex.IsMatch("https://localhost:8080/test/search?q=abc", TRegex.UrlPattern.FullWithSchemeRequired));
            Assert.AreEqual(true, TRegex.IsMatch("https://192.168.1.1/test", TRegex.UrlPattern.FullWithSchemeRequired));
            Assert.AreEqual(true, TRegex.IsMatch("https://192.168.1.1:8080/test/search?q=abc", TRegex.UrlPattern.FullWithSchemeRequired));
            Assert.AreEqual(false, TRegex.IsMatch("https://ahttps://bing.com", TRegex.UrlPattern.FullWithSchemeRequired));
            Assert.AreEqual(false, TRegex.IsMatch("bing.com", TRegex.UrlPattern.FullWithSchemeRequired));
            Assert.AreEqual(false, TRegex.IsMatch("www.bing.com", TRegex.UrlPattern.FullWithSchemeRequired));
            Assert.AreEqual(false, TRegex.IsMatch("www.bing.com/search", TRegex.UrlPattern.FullWithSchemeRequired));
            Assert.AreEqual(false, TRegex.IsMatch("www.bing.com/search?q=abc", TRegex.UrlPattern.FullWithSchemeRequired));
            Assert.AreEqual(false, TRegex.IsMatch("bing", TRegex.UrlPattern.FullWithSchemeRequired));
            Assert.AreEqual(false, TRegex.IsMatch("bing/search?q=abc", TRegex.UrlPattern.FullWithSchemeRequired));
            Assert.AreEqual(false, TRegex.IsMatch("ahttps://bing.com", TRegex.UrlPattern.FullWithSchemeRequired));
        }

        [TestMethod]
        public void Test_Url_InternalDomain()
        {
            Assert.AreEqual(true, TRegex.IsMatch("localhost", TRegex.UrlPattern.InternalDomain));
            Assert.AreEqual(true, TRegex.IsMatch("a.b", TRegex.UrlPattern.InternalDomain));
        }

        [TestMethod]
        public void Test_Url_PublicDomain()
        {
            Assert.AreEqual(true, TRegex.IsMatch("a.me", TRegex.UrlPattern.PublicDomain));
            Assert.AreEqual(true, TRegex.IsMatch("a.b.me", TRegex.UrlPattern.PublicDomain));
            Assert.AreEqual(true, TRegex.IsMatch("test.com", TRegex.UrlPattern.PublicDomain));
            Assert.AreEqual(true, TRegex.IsMatch("www.test.com", TRegex.UrlPattern.PublicDomain));
            Assert.AreEqual(true, TRegex.IsMatch("123.com", TRegex.UrlPattern.PublicDomain));
            Assert.AreEqual(false, TRegex.IsMatch("a.b", TRegex.UrlPattern.PublicDomain));
            Assert.AreEqual(false, TRegex.IsMatch("localhost", TRegex.UrlPattern.PublicDomain));
            Assert.AreEqual(false, TRegex.IsMatch("192.168.1.1", TRegex.UrlPattern.PublicDomain));
        }

        [TestMethod]
        public void Test_Url_IPV4()
        {
            Assert.AreEqual(true, TRegex.IsMatch("192.168.1.1", TRegex.UrlPattern.IPV4));
            Assert.AreEqual(true, TRegex.IsMatch("127.0.0.1", TRegex.UrlPattern.IPV4));
            Assert.AreEqual(false, TRegex.IsMatch("192", TRegex.UrlPattern.IPV4));
            Assert.AreEqual(false, TRegex.IsMatch("192.168", TRegex.UrlPattern.IPV4));
            Assert.AreEqual(false, TRegex.IsMatch("localhost", TRegex.UrlPattern.IPV4));
        }

        [TestMethod]
        public void Test_Url_Port()
        {
            Assert.AreEqual(true, TRegex.IsMatch(":80", TRegex.UrlPattern.Port));
            Assert.AreEqual(true, TRegex.IsMatch(":8080", TRegex.UrlPattern.Port));
            Assert.AreEqual(true, TRegex.IsMatch(":65535", TRegex.UrlPattern.Port));
            Assert.AreEqual(false, TRegex.IsMatch("80", TRegex.UrlPattern.Port));
            Assert.AreEqual(false, TRegex.IsMatch(":08", TRegex.UrlPattern.Port));
        }

        [TestMethod]
        public void Test_Url_Path()
        {
            Assert.AreEqual(true, TRegex.IsMatch("/test", TRegex.UrlPattern.Path));
            Assert.AreEqual(true, TRegex.IsMatch("/test?q=abc&f=123", TRegex.UrlPattern.Path));
            Assert.AreEqual(true, TRegex.IsMatch("/test/sub", TRegex.UrlPattern.Path));
            Assert.AreEqual(true, TRegex.IsMatch("/test/sub?q=abc", TRegex.UrlPattern.Path));
            Assert.AreEqual(false, TRegex.IsMatch("test", TRegex.UrlPattern.Path));
        }
    }
}
