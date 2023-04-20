using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TulipInfo.Net.Drawing
{
    public class ThumbnailIOptions
    {
        public int Width { get; set; } = 200;
        public int Height { get; set; } = 200;
        public ThumbnailImageFormat ImageFormat { get; set; } = ThumbnailImageFormat.Png;
        public int Quantity { get; set; } = 80;
    }
}
