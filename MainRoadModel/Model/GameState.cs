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
            for (int i = 0; i < Game.GRID_SIZE; i++)
                for (int j = 0; j < Game.GRID_SIZE; j++)
                {
                    Cells[i, j] = new Cell() { CellX =i,CellY = j};
                }
        }

        static Cell EmptyCell = new Cell();
        
        /// <summary>
        /// Returns cell by coordinates (with boundaries control)
        /// </summary>
        public Cell this[int cellX, int cellY]
        {
            get
            {
                if (cellX< 0) return EmptyCell;
                if (cellY< 0) return EmptyCell;
                if (cellX >= Game.GRID_SIZE) return EmptyCell;
                if (cellY >= Game.GRID_SIZE) return EmptyCell;
                return Cells[cellX, cellY];
            }
        }
    }
}
