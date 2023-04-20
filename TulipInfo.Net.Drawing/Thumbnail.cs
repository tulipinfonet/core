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

            SKImage srcImage = SKImage.FromEncodedData(imageBytes);
            SKRect srcRect = new SKRect(0, 0, srcImage.Width, srcImage.Height);
            SKRect targetRect = new SKRect(0, 0, width, height);

            if (srcImage.Width != width || srcImage.Height != height)
            {
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
                    float srcRate = (float)srcImage.Width / (float)srcImage.Height;
                    float targetRate = (float)width / (float)height;
                    if (srcRate != targetRate)
                    {
                        if (srcRate < targetRate)
                        {
                            //target is wider than source
                            float zoom = (float)srcImage.Height / (float)height;
                            float newSrcWidth = (float)srcImage.Width / zoom;
                            float centerTargetX = (float)width / 2;


                            targetRect.Left = centerTargetX - (newSrcWidth / 2);
                            targetRect.Top = 0;
                            targetRect.Right = centerTargetX + (newSrcWidth / 2); ;
                            targetRect.Bottom = height;
                        }
                        else if (srcRate > targetRate)
                        {
                            //target is wider than thumbnail
                            float zoom = (float)srcImage.Width / (float)width;
                            float newSrcHeight = (float)srcImage.Height / zoom;
                            float centerTargetY = (float)height / 2;

                            targetRect.Left = 0;
                            targetRect.Top = centerTargetY - (newSrcHeight / 2);
                            targetRect.Right = width;
                            targetRect.Bottom = centerTargetY + (newSrcHeight / 2);
                        }

                    }

                }
            }
            using var surface = SKSurface.Create(new SKImageInfo(width, height));
            SKCanvas canvas = surface.Canvas;
            canvas.DrawImage(srcImage, srcRect, targetRect);

            var data = surface.Snapshot().Encode((SKEncodedImageFormat)((byte)options.ImageFormat), options.Quantity);
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
