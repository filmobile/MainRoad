using System;
using System.Collections.Generic;

namespace MainRoadModel.Model
{
    /// <summary>
    /// Cell of tile's grid
    /// </summary>
    [Serializable]
    class Cell
    {
        public int CellX { get; set; }
        public int CellY { get; set; }
        public LinkedList<Tile> Tiles { get; set; }
    }
}