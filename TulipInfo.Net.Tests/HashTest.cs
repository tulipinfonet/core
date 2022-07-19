using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TulipInfo.Net.Tests
{
    [TestClass]
    public class HashTest
    {
        [TestMethod]
        public void TestCRC32()
        {
            string valueToHash = "51A03F65-6DBE-435D-97B7-207259C6C27D";
            uint crc32 = Hash.CRC32(valueToHash);
            Assert.AreEqual((uint)657598919, crc32);

            valueToHash = "02A71FB2-935E-46FF-BA70-3B81FABD1D3E";
            crc32 = Hash.CRC32(valueToHash);
            Assert.AreEqual((uint)3510452979, crc32);

            valueToHash = "02a71fb2-935e-46ff-ba70-3b81fabd1d3e";
            crc32 = Hash.CRC32(valueToHash);
            Assert.AreEqual((uint)4063108149, crc32);

        }
    }
}
