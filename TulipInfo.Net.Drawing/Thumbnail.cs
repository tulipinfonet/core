using System.IO;
using SkiaSharp;

namespace TulipInfo.Net.Drawing
{
    public static class Thumbnail
    {
        public static byte[] GetBytes(byte[] imageBytes, int width, int height)
        {
            SKImage srcImage= SKImage.FromEncodedData(imageBytes);
            SKRect srcRect = new SKRect(0, 0, srcImage.Width, srcImage.Height);
            SKRect targetRect = new SKRect(0, 0, width, height);


            if (srcImage.Width <= width && srcImage.Height <= height)
            {
                targetRect.Right = srcImage.Width;
                targetRect.Bottom = srcImage.Height;
            }
            else
            {
                float srcRate = (float)srcImage.Width / (float)srcImage.Height;
                float targetRate = (float)width / (float)height;
                if (srcRate != targetRate)
                {
                    //cut by rate
                    float srcCenterX = (float)srcImage.Width / 2;
                    float srcCenterY = (float)srcImage.Height / 2;
                    float left = srcCenterX;
                    float top = srcCenterY;
                    float right = srcCenterX;
                    float bottom = srcCenterY;
                    bool continueCut = true;
                    while(continueCut)
                    {
                        float xSpan = 1;
                        float ySpan = xSpan/targetRate;
                        left-= xSpan;
                        top-= ySpan;
                        right += xSpan;
                        bottom += ySpan;

                        continueCut = (left > 0 && top > 0 && right < width && bottom < height);
                        if(continueCut)
                        {
                            srcRect.Left = left;
                            srcRect.Top = top;
                            srcRect.Right = right;
                            srcRect.Bottom = bottom;
                        }
                    }
                }
            }

            using var surface = SKSurface.Create(new SKImageInfo(width, height));
            SKCanvas canvas = surface.Canvas;
            canvas.DrawImage(srcImage, srcRect, targetRect);

            var data = surface.Snapshot().Encode(SKEncodedImageFormat.Jpeg, 80);
            return  data.ToArray();
        }
    }
}
