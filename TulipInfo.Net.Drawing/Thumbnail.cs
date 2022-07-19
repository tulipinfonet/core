using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

//ref: http://www.cnblogs.com/wu-jian/archive/2011/02/21/1959382.html
namespace TulipInfo.Net.Drawing
{
    public static class Thumbnail
    {
        public static Stream GetThumbnail(byte[] image, ThumbnailOptions options)
        {
            MemoryStream imageStream = new MemoryStream();
            imageStream.Write(image, 0, image.Length);
            return GetThumbnail(imageStream, options);
        }

        public static Stream GetThumbnail(Stream image, ThumbnailOptions options)
        {
            return GetThumbnail(Image.FromStream(image, true), options);
        }

        public static Stream GetThumbnail(Image image, ThumbnailOptions options)
        {
            int maxWidth = options.Width;
            int maxHeight = options.Height;
            if (image.Width <= maxWidth && image.Height <= maxHeight)
            {
                MemoryStream resultStream = new MemoryStream();
                if (options.ImageFormat == ThumbnailImageFormat.SameAsSource)
                {
                    image.Save(resultStream, image.RawFormat);
                }
                else
                {
                    image.Save(resultStream, GetImageFormat(options.ImageFormat));
                }
                resultStream.Seek(0, SeekOrigin.Begin);
                return resultStream;
            }
            else
            {
                double templateRate = (double)maxWidth / maxHeight;
                double initRate = (double)image.Width / image.Height;                
                if (templateRate == initRate)
                {
                    return Zoom(image, options);
                }                
                else
                {
                    Rectangle fromR = new Rectangle(0, 0, 0, 0);
                    Rectangle toR = new Rectangle(0, 0, 0, 0);
                                                              
                    if (templateRate > initRate)
                    {
                       
                        using Image pickedImage = new Bitmap(image.Width, (int)Math.Floor(image.Width / templateRate));
                        using Graphics pickedG = Graphics.FromImage(pickedImage);
                       
                        fromR.X = 0;
                        fromR.Y = (int)Math.Floor((image.Height - image.Width / templateRate) / 2);
                        fromR.Width = image.Width;
                        fromR.Height = (int)Math.Floor(image.Width / templateRate);
                        
                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = image.Width;
                        toR.Height = (int)Math.Floor(image.Width / templateRate);

                        
                        pickedG.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = SmoothingMode.HighQuality;
                       
                        pickedG.DrawImage(image, toR, fromR, GraphicsUnit.Pixel);

                        return Zoom(pickedImage, options);
                    }
                    
                    else
                    {
                        using Image pickedImage = new Bitmap((int)Math.Floor(image.Height * templateRate), image.Height);
                        using Graphics pickedG = Graphics.FromImage(pickedImage);
                        fromR.X = (int)Math.Floor((image.Width - image.Height * templateRate) / 2);
                        fromR.Y = 0;
                        fromR.Width = (int)Math.Floor(image.Height * templateRate);
                        fromR.Height = image.Height;
                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = (int)Math.Floor(image.Height * templateRate);
                        toR.Height = image.Height;

                        
                        pickedG.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = SmoothingMode.HighQuality;
                        
                        pickedG.DrawImage(image, toR, fromR, GraphicsUnit.Pixel);

                        return Zoom(pickedImage, options);
                    }
                }
            }
        }


        static Stream Zoom(Image image, ThumbnailOptions options)
        {
            MemoryStream resultStream = new MemoryStream();
           
            using Image templateImage = new Bitmap(options.Width, options.Height);
            using Graphics templateG = Graphics.FromImage(templateImage);
            templateG.InterpolationMode = InterpolationMode.High;
            templateG.SmoothingMode = SmoothingMode.HighQuality;
            templateG.Clear(Color.White);
            templateG.DrawImage(image, new Rectangle(0, 0, options.Width, options.Height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

            if (options.ImageFormat == ThumbnailImageFormat.SameAsSource)
            {
                templateImage.Save(resultStream, image.RawFormat);
            }
            else
            {
                templateImage.Save(resultStream, GetImageFormat(options.ImageFormat));
            }

            resultStream.Seek(0, SeekOrigin.Begin);

            return resultStream;
        }

        static ImageFormat GetImageFormat(ThumbnailImageFormat format)
        {
            ImageFormat imgF = ImageFormat.Jpeg;

            switch (format)
            {
                case ThumbnailImageFormat.Bmp:
                    imgF = ImageFormat.Bmp;
                    break;
                case ThumbnailImageFormat.Emf:
                    imgF = ImageFormat.Emf;
                    break;
                case ThumbnailImageFormat.Exif:
                    imgF = ImageFormat.Exif;
                    break;
                case ThumbnailImageFormat.Gif:
                    imgF = ImageFormat.Gif;
                    break;
                case ThumbnailImageFormat.Icon:
                    imgF = ImageFormat.Icon;
                    break;
                case ThumbnailImageFormat.Jpeg:
                    imgF = ImageFormat.Jpeg;
                    break;
                case ThumbnailImageFormat.MemoryBmp:
                    imgF = ImageFormat.MemoryBmp;
                    break;
                case ThumbnailImageFormat.Png:
                    imgF = ImageFormat.Png;
                    break;
                case ThumbnailImageFormat.Tiff:
                    imgF = ImageFormat.Tiff;
                    break;
                case ThumbnailImageFormat.Wmf:
                    imgF = ImageFormat.Wmf;
                    break;
                default:
                    break;
            }

            return imgF;
        }
    }
}
