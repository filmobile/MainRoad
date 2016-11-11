using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication354
{
    public static class GraphicsHelper
    {
        /// <summary>
        /// Прямоугольник с закругленными углами
        /// </summary>
        public static GraphicsPath GetRoundedRectangle(Rectangle rect, int d)
        {
            var gp = new GraphicsPath();

            gp.AddArc(rect.X, rect.Y, d, d, 180, 90);
            gp.AddArc(rect.X + rect.Width - d, rect.Y, d, d, 270, 90);
            gp.AddArc(rect.X + rect.Width - d, rect.Y + rect.Height - d, d, d, 0, 90);
            gp.AddArc(rect.X, rect.Y + rect.Height - d, d, d, 90, 90);
            gp.AddLine(rect.X, rect.Y + rect.Height - d, rect.X, rect.Y + d / 2);

            return gp;
        }
        /// <summary>
        /// Отрисовка полупрозрачного изображения
        /// </summary>
        /// <param name="alpha">От 0 до 1</param>
        public static void DrawImage(this Graphics gr, Image img, Point location, float alpha)
        {
            var attr = new ImageAttributes();
            attr.SetColorMatrix(new ColorMatrix { Matrix33 = alpha });
            gr.DrawImage(img, new Rectangle(location, img.Size), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, attr);
        }

        public static Bitmap ResizeIfNeed(Bitmap bmp, int maxWidth, int maxHeight, InterpolationMode mode = InterpolationMode.Bicubic)
        {
            var kx = 1f * maxWidth / bmp.Width;
            var ky = 1f * maxHeight / bmp.Height;
            var k = Math.Min(1, Math.Min(kx, ky));
            var w = (int)(bmp.Width * k);
            var h = (int)(bmp.Height * k);

            var res = new Bitmap(w, h);
            using (var gr = Graphics.FromImage(res))
            {
                gr.InterpolationMode = mode;
                gr.DrawImage(bmp, 0, 0, w, h);
            }

            return res;
        }

        public static Bitmap Resize(Bitmap bmp, int width, int height, InterpolationMode mode = InterpolationMode.Bicubic)
        {
            var res = new Bitmap(width, height);
            using (var gr = Graphics.FromImage(res))
            {
                gr.InterpolationMode = mode;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(bmp, 0, 0, width, height);
            }

            return res;
        }

        public static Bitmap Resize(Bitmap bmp, float scale, InterpolationMode mode = InterpolationMode.Bicubic)
        {
            var w = (int)(bmp.Width * scale);
            var h = (int)(bmp.Height * scale);

            return Resize(bmp, w, h, mode);
        }

        public static Bitmap ResizeIfNeed(Bitmap bmp, int minWidth, int maxWidth, int minHeight, int maxHeight, InterpolationMode mode = InterpolationMode.Bicubic)
        {
            var kx = 1f * maxWidth / bmp.Width;
            var ky = 1f * maxHeight / bmp.Height;
            var k = Math.Min(kx, ky);

            if (k > 1)
            {
                kx = 1f * minWidth / bmp.Width;
                ky = 1f * minHeight / bmp.Height;
                k = Math.Max(kx, ky);

                if (k < 1) k = 1f;
            }

            var w = (int)(bmp.Width * k);
            var h = (int)(bmp.Height * k);

            var res = new Bitmap(w, h);
            using (var gr = Graphics.FromImage(res))
            {
                gr.InterpolationMode = mode;
                gr.DrawImage(bmp, 0, 0, w, h);
            }

            return res;
        }


        /// <summary>
        /// Changes brightness, contrast and saturation
        /// </summary>
        /// <param name="brightness">from -1 to 1</param>
        /// <param name="contrast">from 0 to 1 and more </param>
        /// <param name="saturation">from 0 to 1 and more</param>
        public static Bitmap AdjustColor(Image img, float brightness = 0, float contrast = 1, float saturation = 1)
        {
            var imageAttributes = new ImageAttributes();

            var b = brightness;
            var c = contrast;
            var t = (1f - c) / 2f;
            var s = saturation;
            var sr = (1 - s) * 0.3086f;
            var sg = (1 - s) * 0.6094f;
            var sb = (1 - s) * 0.0820f;

            float[][] colorMatrixElements = { 
               new float[] {c*(sr+s), c*sr,     c*(sr),    0, 0},
               new float[] {c*sg,     c*(sg+s), c*(sg),    0, 0},
               new float[] {c*sb,     c*sb,     c*(sb+s),  0, 0},
               new float[] {0,        0,        0,        1f, 0},
               new float[] {t+b,      t+b,      t+b,       0, 1}};

            var colorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            var result = new Bitmap(img.Width, img.Height);
            using (var gr = Graphics.FromImage(result))
                gr.DrawImage(img,
                           new Rectangle(0, 0, img.Width, img.Height), 0, 0,
                           img.Width, img.Height,
                           GraphicsUnit.Pixel, imageAttributes);

            return result;
        }

        /// <summary>
        /// Устанавливает пикселы изображения (RAW)
        /// </summary>
        public static void SetPixels(this Bitmap bmp, byte[] pixels)
        {
            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, bmpData.Scan0, pixels.Length);
            bmp.UnlockBits(bmpData);
        }

        public static Color HsvToRgb(double h, double S, double V)
        {
            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {
                    case 0: R = V; G = tv; B = pv; break;
                    case 1: R = qv; G = V; B = pv; break;
                    case 2: R = pv; G = V; B = tv; break;
                    case 3: R = pv; G = qv; B = V; break;
                    case 4: R = tv; G = pv; B = V; break;
                    case 5: R = V; G = pv; B = qv; break;
                    case 6: R = V; G = tv; B = pv; break;
                    case -1: R = V; G = pv; B = qv; break;
                    default: R = G = B = V; break;
                }
            }

            return Color.FromArgb(Clamp(R * 255.0), Clamp(G * 255.0), Clamp(B * 255.0));
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        static byte Clamp(double i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return (byte)i;
        }

    }
}
