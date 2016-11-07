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
        /// Roads from other nodes (in)
        /// </summary>
        public LinkedList<Road> RoadsIn = new LinkedList<Road>();

        /// <summary>
        /// Roads to other nodes (out)
        /// </summary>
        public LinkedList<Road> RoadsOut = new LinkedList<Road>();
        /// <summary>
        /// Name
        /// </summary>
        public string Name;

        public override string ToString()
        {
            return string.Format("x={0} y={1}", CellX, CellY);
        }
    }
}
