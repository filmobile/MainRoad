using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainRoadModel
{
    /// <summary>
    /// Loads and Stores bitmaps
    /// </summary>
    public static class BitmapCache
    {
        //cache of bitmaps
        static Dictionary<Id, Bitmap> cache = new Dictionary<Id, Bitmap>();

        /// <summary>
        /// Get bitmap from file or cache
        /// </summary>
        public static Bitmap GetBitmap(string shortFileName, RotateFlipType rotate)
        {
            Bitmap bmp;
            var id = new Id { Name = shortFileName, Rotate = rotate };
            if (!cache.TryGetValue(id, out bmp))
            {
                using (var temp = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sprites\\" + shortFileName)))
                {
                    temp.SetResolution(96, 96);
                    //change format to PARGB32
                    bmp = new Bitmap(temp.Width, temp.Height, PixelFormat.Format32bppPArgb);
                    using (Graphics gr = Graphics.FromImage(bmp))
                        gr.DrawImageUnscaled(temp, new Rectangle(0, 0, temp.Width, temp.Height));
                    //rotate and flip
                    bmp.RotateFlip(rotate);
                }

                cache[id] = bmp;
            }

            return bmp;
        }

        struct Id
        {
            public string Name;
            public RotateFlipType Rotate;

            public override int GetHashCode()
            {
                return Name.GetHashCode() ^ Rotate.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var other = (Id)obj;
                return other.Rotate == Rotate && other.Name == Name;
            }
        }
    }
}
