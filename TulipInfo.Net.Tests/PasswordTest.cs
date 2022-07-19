using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TulipInfo.Net.Tests
{
    [TestClass]
    public class PasswordTest
    {
        [TestMethod]
        public void Test_Password()
        {
            for (int i = 0; i < 1000; i++)
            {
                string password = Password.Generate();
                Assert.AreEqual(8, password.Length);
                Assert.IsTrue(TRegex.IsMatch(password, TRegex.StrongPasswordPattern));
            }
        }
    }
}
