using SkiaSharp;
using System.IO;

namespace TulipInfo.Net.Drawing.Tests
{
    [TestClass]
    public class ThumbnailTest
    {
        [TestMethod]
        public void TestMethod()
        {
            string imageFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image/image1.jpg");
            byte[] imageBytes = File.ReadAllBytes(imageFile);
            byte[] thumBytes = Thumbnail.GetBytes(imageBytes, 200, 200);
            SKImage thumImage = SKImage.FromEncodedData(thumBytes);
            Assert.AreEqual(200, thumImage.Width);
            Assert.AreEqual(200, thumImage.Height);

            thumBytes = Thumbnail.GetBytes(imageBytes, 150, 200);
            thumImage = SKImage.FromEncodedData(thumBytes);
            Assert.AreEqual(150, thumImage.Width);
            Assert.AreEqual(200, thumImage.Height);

            thumBytes = Thumbnail.GetBytes(imageBytes, 200, 150);
            thumImage = SKImage.FromEncodedData(thumBytes);
            Assert.AreEqual(200, thumImage.Width);
            Assert.AreEqual(150, thumImage.Height);
        }

        [TestMethod]
        public void TestMethod1()
        {
            string imageFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image/image1.jpg");
            byte[] imageBytes = File.ReadAllBytes(imageFile);
            byte[] thumBytes = Thumbnail.GetBytes(imageBytes,new ThumbnailIOptions() { ImageFormat= ThumbnailImageFormat.Jpeg });

            string newFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"image/image1_{Guid.NewGuid()}.jpg");
            using (FileStream file = new FileStream(newFile, FileMode.Create, FileAccess.Write))
            {
                file.Write(thumBytes, 0, thumBytes.Length);
            }
        }

        [TestMethod]
        public void TestMethod1_200()
        {
            string imageFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image/image1.200.jpg");
            byte[] imageBytes = File.ReadAllBytes(imageFile);
            
            //jpg
            byte[] thumBytes = Thumbnail.GetBytes(imageBytes, new ThumbnailIOptions() { Width = 350, Height = 300, ImageFormat = ThumbnailImageFormat.Jpeg });
            string newFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"image/image1.200_{Guid.NewGuid()}.jpg");
            using (FileStream file = new FileStream(newFile, FileMode.Create, FileAccess.Write))
            {
                file.Write(thumBytes, 0, thumBytes.Length);
            }

            //png
            thumBytes = Thumbnail.GetBytes(imageBytes, new ThumbnailIOptions() { Width = 350, Height = 300, ImageFormat = ThumbnailImageFormat.Png });
            newFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"image/image1.200_{Guid.NewGuid()}.png");
            using (FileStream file = new FileStream(newFile, FileMode.Create, FileAccess.Write))
            {
                file.Write(thumBytes, 0, thumBytes.Length);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            string imageFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image/image2.png");
            byte[] imageBytes = File.ReadAllBytes(imageFile);
            byte[] thumBytes = Thumbnail.GetBytes(imageBytes, 400, 400);

            string newFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"image/image2_{Guid.NewGuid()}.png");
            using (FileStream file = new FileStream(newFile, FileMode.Create, FileAccess.Write))
            {
                file.Write(thumBytes, 0, thumBytes.Length);
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            string imageFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image/image3.jpg");
            byte[] imageBytes = File.ReadAllBytes(imageFile);
            byte[] thumBytes = Thumbnail.GetBytes(imageBytes, 200, 200);

            string newFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"image/image3_{Guid.NewGuid()}.png");
            using (FileStream file = new FileStream(newFile, FileMode.Create, FileAccess.Write))
            {
                file.Write(thumBytes, 0, thumBytes.Length);
            }
        }
    }
}