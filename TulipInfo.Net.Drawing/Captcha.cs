using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TulipInfo.Net.Drawing
{
    public static class Captcha
    {
        const int CHAR_WIDTH = 60;
        const int IMAGE_HEIGHT = 100;
        static readonly SKColor[] COLORS = { SKColors.Black, SKColors.Blue,SKColors.Brown,SKColors.Chocolate,SKColors.Crimson,SKColors.Green,SKColors.Orange,SKColors.Purple, SKColors.Red };
        static readonly string[] Fonts = { "sans-serif","Arial", "Verdana", "Times New Roman" };
        static readonly SKFontStyle[] FontStyles = { SKFontStyle.Normal,SKFontStyle.Bold, SKFontStyle.BoldItalic };

        public static byte[] GetBytes(string code)
        {
            int imageWidth = CHAR_WIDTH * code.Length+20;
            using var surface = SKSurface.Create(new SKImageInfo(imageWidth,IMAGE_HEIGHT));
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.White);

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            

            //draw line
            rand = new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < 30; i++)
            {
                int colorIdx = rand.Next(COLORS.Length - 1);

                int x1 = rand.Next(imageWidth);
                int x2 = rand.Next(imageWidth);
                int y1 = rand.Next(IMAGE_HEIGHT);
                int y2 = rand.Next(IMAGE_HEIGHT);


                canvas.DrawLine(x1, y1, x2, y2, new SKPaint()
                {
                    IsStroke = true,
                    StrokeWidth = i%3+1,
                    Color = COLORS[colorIdx]
                }) ;
            }

            //draw point
            for (int i = 0; i < 120; i++)
            {
                int colorIdx = rand.Next(COLORS.Length - 1);

                int x1 = rand.Next(imageWidth);
                int y1 = rand.Next(IMAGE_HEIGHT);

                canvas.DrawPoint(x1, y1, new SKPaint()
                {
                    IsStroke = true,
                    StrokeWidth = i % 5+1,
                    Color = COLORS[colorIdx]
                });
            }

            //draw code
            for (int i = 0; i < code.Length; i++)
            {
                int fontSize = rand.Next(Convert.ToInt32(IMAGE_HEIGHT * Convert.ToSingle(3.0 / 4)), Convert.ToInt32(IMAGE_HEIGHT * Convert.ToSingle(4.0 / 5)));
                float x = i * CHAR_WIDTH;
                float y = rand.Next(fontSize, IMAGE_HEIGHT);

                float drawX = x;
                float drawy = y;

                int degrees = rand.Next(-40, 40);

                if (degrees > 0)
                {
                    var radian = Math.PI * degrees / 180;
                    float yHeight = fontSize * Convert.ToSingle(Math.Sin(radian));

                    var radian2 = Math.PI * (90 - degrees) / 180;
                    float yDiff = yHeight * Convert.ToSingle(Math.Sin(radian2));
                    float xDiff = yHeight * Convert.ToSingle(Math.Cos(radian2));

                    drawX -= xDiff;
                    drawy -= yDiff;
                }
                else
                {
                    var radian = Math.PI * (90 + degrees) / 180;
                    float xWidth = fontSize * Convert.ToSingle(Math.Cos(radian));
                    float yDiff = xWidth * Convert.ToSingle(Math.Cos(radian));
                    float xDiff = xWidth * Convert.ToSingle(Math.Sin(radian));

                    drawX += xDiff;
                    drawy += yDiff;
                }

                canvas.RotateDegrees(degrees, x, y);

                int colorIdx = rand.Next(COLORS.Length - 1);
                int fontIdx = rand.Next(Fonts.Length - 1);
                int fontStyleIdx = rand.Next(FontStyles.Length - 1);
                canvas.DrawText(code[i].ToString(), drawX, drawy, new SKPaint(new SKFont(SKTypeface.FromFamilyName(Fonts[fontIdx], FontStyles[fontStyleIdx]), fontSize))
                {
                    Color = COLORS[colorIdx]
                });

                canvas.RotateDegrees(-degrees, x, y);
            }


            var data = surface.Snapshot().Encode(SKEncodedImageFormat.Jpeg, 80);

            return data.ToArray();
        }

        public static string RandomString(int length=6)
        {
            string str = string.Empty;
            string Vchar = "2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,j,k,l,m,n,p" +
            ",q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q" +
            ",R,S,T,U,V,W,X,Y,Z";

            string[] VcArray = Vchar.Split(new Char[] { ',' });
            string[] num = new string[length];

            int temp = -1;

            Random rand = new Random();
            for (int i = 1; i <= length; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(VcArray.Length-1);

                temp = t;
                str += VcArray[t];
            }
            return str;
        }
    }
}
