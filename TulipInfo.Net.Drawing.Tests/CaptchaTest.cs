using SkiaSharp;

namespace TulipInfo.Net.Drawing.Tests
{
    [TestClass]
    public class CaptchaTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string randomString = Captcha.RandomString(6);
            var imageData = Captcha.GetBytes(randomString);
            //string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp");
            //if(!Directory.Exists(folder))
            //{
            //    Directory.CreateDirectory(folder);
            //}
            //string file = Path.Combine(folder,$"{randomString}_{Guid.NewGuid()}.jpeg");
            //using (var fileStream = File.Create(file))
            //{
            //    fileStream.Write(imageData,0,imageData.Length);
            //}
            var bitmap = SKBitmap.Decode(imageData);
            Assert.AreEqual(380, bitmap.Width);
            Assert.AreEqual(100, bitmap.Height);
        }
    }
}