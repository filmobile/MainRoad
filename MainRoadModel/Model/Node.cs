using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainRoadModel.Model
{
    /// <summary>
    /// Graph's node
    /// </summary>
    [Serializable]
    class Node
    {
        //coordinates in grid
        public int CellX;
        public int CellY;

        /// <summary>
        /// Nodes linked to this node
        /// </summary>
        public LinkedList<Node> Nodes;
    }
}
