using System;
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
            Cells = new Cell[Game.GRID_SIZE, Game.GRID_SIZE];
        }
    }
}
