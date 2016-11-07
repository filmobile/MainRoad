using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainRoadModel.Model
{
    /// <summary>
    /// Game state
    /// </summary>
    [Serializable]
    class GameState
    {
        /// <summary>
        /// Grid of tiles
        /// </summary>
        public Cell[,] Cells { get; private set; }

        public GameState()
        {
            Cells = new Cell[Game.GRIDSIZE, Game.GRIDSIZE];
        }
    }

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

    /// <summary>
    /// Tile
    /// </summary>
    [Serializable]
    class Tile
    {
        public Cell ParentCell { get; set; }
    }
}
