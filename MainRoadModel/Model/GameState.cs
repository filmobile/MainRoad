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
    public class GameState
    {
        /// <summary>
        /// Grid of tiles
        /// </summary>
        public Cell[,] Cells { get; private set; }

        /// <summary>
        /// Buildings and roads graph
        /// </summary>
        public LinkedList<Node> Nodes { get; private set; }

        public GameState()
        {
            Cells = new Cell[Game.GRID_SIZE, Game.GRID_SIZE];
            Nodes = new LinkedList<Node>();
        }

        /// <summary>
        /// Returns cell by coordinates (with boundaries control)
        /// </summary>
        public Cell this[int cellX, int cellY]
        {
            get
            {
                if (cellX< 0) cellX = 0;
                if (cellY< 0) cellY = 0;
                if (cellX >= Game.GRID_SIZE) cellX = Game.GRID_SIZE - 1;
                if (cellY >= Game.GRID_SIZE) cellY = Game.GRID_SIZE - 1;
                return Cells[cellX, cellY];
            }
        }
    }
}
