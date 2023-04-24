using System;
using System.IO;
using SkiaSharp;

namespace TulipInfo.Net.Drawing
{
    public static class Thumbnail
    {
        public static byte[] GetBytes(byte[] imageBytes, int width, int height, int quality = 80)
        {
            return GetBytes(imageBytes, new ThumbnailIOptions()
            {
                Width = width,
                Height = height,
                Quantity = quality
            });
        }

        public static byte[] GetBytes(byte[] imageBytes, ThumbnailIOptions options)
        {
            int width = options.Width;
            int height = options.Height;

            SKBitmap srcImage = SKBitmap.Decode(imageBytes);

            if (srcImage.Width == width && srcImage.Height == height)
            {
                return imageBytes;
            }

            float srcRate = (float)srcImage.Width / (float)srcImage.Height;
            float targetRate = (float)width / (float)height;
            if (srcRate == targetRate)
            {
                //resize
                return Resize(srcImage, options);
            }
            else
            {
                //adjust rate and draw again
                int tempWidth = width;
                int tempHeight = height;

                SKRect srcRect = new SKRect(0, 0, srcImage.Width, srcImage.Height);
                SKRect targetRect = new SKRect(0, 0, width, height);

                if (srcImage.Width <= width && srcImage.Height <= height)
                {
                    //keep center
                    float centerTargetX = (float)width / 2;
                    float centerTargetY = (float)height / 2;

                    targetRect.Left = centerTargetX - ((float)srcImage.Width / 2);
                    targetRect.Top = centerTargetY - ((float)srcImage.Height / 2);
                    targetRect.Right = centerTargetX + ((float)srcImage.Width / 2); ;
                    targetRect.Bottom = centerTargetY + ((float)srcImage.Height / 2);
                }
                else
                {
                    if (srcRate < targetRate)
                    {
                        //target is wider than source
                        //leave empty in both left and right
                        tempWidth= Convert.ToInt32(srcImage.Height*targetRate);
                        tempHeight = srcImage.Height;
                        float centerSrcX = (float)srcImage.Width / 2;
                        float centerTargetX = (float)tempWidth / 2;

                        targetRect.Left = centerTargetX - centerSrcX;
                        targetRect.Top = 0;
                        targetRect.Right = centerTargetX + centerSrcX;
                        targetRect.Bottom = tempHeight;
                    }
                    else if (srcRate > targetRate)
                    {
                        //source is wider than target
                        //leave empty in both top and bottom
                        tempWidth = srcImage.Width;
                        tempHeight = Convert.ToInt32(srcImage.Width / targetRate);

                        float centerSourceY = (float)srcImage.Height / 2;
                        float centerTargetY = (float)tempHeight / 2;

                        targetRect.Left = 0;
                        targetRect.Top = centerTargetY - centerSourceY;
                        targetRect.Right = tempWidth;
                        targetRect.Bottom = centerTargetY + centerSourceY;
                    }
                }

                using var surface = SKSurface.Create(new SKImageInfo(tempWidth, tempHeight, SKImageInfo.PlatformColorType,  SKAlphaType.Premul));
                SKCanvas canvas = surface.Canvas;
                canvas.Clear(SKColors.Transparent);
                canvas.DrawBitmap(srcImage, srcRect, targetRect, new SKPaint()
                {
                    IsAntialias = true
                });

                var data =  surface.Snapshot().Encode(SKEncodedImageFormat.Png, options.Quantity);
                
                //resize
                SKBitmap thumbImage = SKBitmap.Decode(data.ToArray());
                return Resize(thumbImage, options);
            }
        }

        private static byte[] Resize(SKBitmap srcImage, ThumbnailIOptions options)
        {
            int width = options.Width;
            int height = options.Height;
            var resizedImageInfo = new SKImageInfo(width, height, SKImageInfo.PlatformColorType, srcImage.AlphaType);
            var resizedBitMap = srcImage.Resize(resizedImageInfo, SKFilterQuality.High);
            var data = resizedBitMap.Encode((SKEncodedImageFormat)((byte)options.ImageFormat), options.Quantity);
            return data.ToArray();
        }

        private static SKRect CutByRate(int srcWidth,int srcHeight,int targetWidth,int targetHeight)
        {
            SKRect srcRect = new SKRect(0, 0, srcWidth, srcHeight);
            float targetRate = (float)targetWidth / (float)targetHeight;

            float srcCenterX = (float)srcWidth / 2;
            float srcCenterY = (float)srcHeight / 2;
            float left = srcCenterX;
            float top = srcCenterY;
            float right = srcCenterX;
            float bottom = srcCenterY;
            bool continueCut = true;
            while (continueCut)
            {
                float xSpan = 1;
                float ySpan = xSpan / targetRate;
                left -= xSpan;
                top -= ySpan;
                right += xSpan;
                bottom += ySpan;

                continueCut = (left > 0 && top > 0 && right < targetWidth && bottom < targetHeight);
                if (continueCut)
                {
                    srcRect.Left = left;
                    srcRect.Top = top;
                    srcRect.Right = right;
                    srcRect.Bottom = bottom;
                }
            }
            return srcRect;
        }
    }
}
