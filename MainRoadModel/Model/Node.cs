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
    public class Node
    {
        //coordinates in grid
        public int CellX;
        public int CellY;

        /// <summary>
        /// Roads to other nodes
        /// </summary>
        public LinkedList<Road> Roads = new LinkedList<Road>();
    }
}
