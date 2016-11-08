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

        /// <summary>
        /// Rotate angle when drawing
        /// </summary>
        public RotateFlipType Rotate;

        /// <summary>
        /// Offset in pixels relative to cell centre
        /// </summary>
        public PointF Offset;
    }
}