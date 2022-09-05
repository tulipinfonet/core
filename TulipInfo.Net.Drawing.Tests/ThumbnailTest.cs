using SkiaSharp;
using System.IO;

namespace TulipInfo.Net.Drawing.Tests
{
    [TestClass]
    public class ThumbnailTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string imageFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image/image1.jpg");
            byte[] imageBytes = File.ReadAllBytes(imageFile);
            byte[] thumBytes = Thumbnail.GetThumbnailBytes(imageBytes, 200, 200);
            SKImage thumImage = SKImage.FromEncodedData(thumBytes);
            Assert.AreEqual(200, thumImage.Width);
            Assert.AreEqual(200, thumImage.Height);

            thumBytes = Thumbnail.GetThumbnailBytes(imageBytes, 150, 200);
            thumImage = SKImage.FromEncodedData(thumBytes);
            Assert.AreEqual(150, thumImage.Width);
            Assert.AreEqual(200, thumImage.Height);

            thumBytes = Thumbnail.GetThumbnailBytes(imageBytes, 200, 150);
            thumImage = SKImage.FromEncodedData(thumBytes);
            Assert.AreEqual(200, thumImage.Width);
            Assert.AreEqual(150, thumImage.Height);


        }
    }
}