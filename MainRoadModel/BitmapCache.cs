using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainRoadModel
{
    public static class BitmapCache
    {
        //cache of bitmaps
        static Dictionary<string, Bitmap> cache = new Dictionary<string, Bitmap>();

        public static Bitmap GetBitmap(string name)
        {
            Bitmap bmp;
            if (!cache.TryGetValue(name, out bmp))
                bmp = cache[name] = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "name"));

            return bmp;
        }
    }
}
