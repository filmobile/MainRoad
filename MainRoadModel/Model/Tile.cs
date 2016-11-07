using System;

namespace MainRoadModel.Model
{
    /// <summary>
    /// Tile
    /// </summary>
    [Serializable]
    class Tile
    {
        /// <summary>
        /// Parent cell
        /// </summary>
        public Cell ParentCell { get; set; }

        /// <summary>
        /// Visual image represents the tile
        /// </summary>
        [NonSerialized]
        public Sprite Sprite;

        /// <summary>
        /// Order drawing relative to other tiles in this cell
        /// </summary>
        public float ZOrder = 0;

        /// <summary>
        /// Type of the tile
        /// </summary>
        public string Type;
    }
}