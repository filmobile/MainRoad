using System;
using System.Collections.Generic;

namespace MainRoadModel.Model
{
    /// <summary>
    /// Cell of tile's grid
    /// </summary>
    [Serializable]
    public class Cell
    {
        public int CellX { get; set; }
        public int CellY { get; set; }
        public LinkedList<Tile> Tiles { get; set; }
        public Node Node { get; set; }

        public Cell()
        {
            Tiles = new LinkedList<Tile>();
        }
    }
}