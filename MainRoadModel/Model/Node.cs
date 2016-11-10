using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainRoadModel.Helpers;

namespace MainRoadModel.Model
{
    /// <summary>
    /// Graph's node
    /// </summary>
    [Serializable]
    public class Node : IHasNeighbours<Node>
    {
        //coordinates in grid
        public int CellX;
        public int CellY;

        public PointF Position {
            get { return new PointF(CellX, CellY); }
        }

        /// <summary>
        /// Cars on the node
        /// </summary>
        public LinkedList<Car> Cars { get; private set; }

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

        public IEnumerable<Node> Neighbours
        {
            get
            {
                foreach (var r in RoadsOut)
                    yield return r.To;
            }
        }

        public override string ToString()
        {
            return string.Format("x={0} y={1}", CellX, CellY);
        }
    }
}
