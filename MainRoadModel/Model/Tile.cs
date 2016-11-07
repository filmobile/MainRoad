using System;
using System.Drawing;

namespace MainRoadModel.Model
{
    /// <summary>
    /// Tile
    /// </summary>
    [Serializable]
    public class Tile
    {
        public string Name;

        [NonSerialized]
        private readonly Bitmap _bitmap;

        /// <summary>
        /// Bitmap
        /// </summary>
        public Bitmap Bitmap
        {
            get { return _bitmap ?? BitmapCache.GetBitmap(Name); }
        }

        /// <summary>
        /// Rotate angle when drawing
        /// </summary>
        public float Angle;

        /// <summary>
        /// Offset in pixels relative to cell centre
        /// </summary>
        public PointF Offset;
    }
}