using System;
using System.Collections.Generic;

namespace MainRoadModel.Model
{
    /// <summary>
    /// Road between nodes
    /// </summary>
    [Serializable]
    public class Road
    {
        public Node From;
        public Node To;

        /// <summary>
        /// Queue of cars on the road
        /// </summary>
        public LinkedList<Car> Cars { get; private set; }

        public Road(Node from, Node to)
        {
            this.From = from;
            this.To = to;

            Cars = new LinkedList<Car>();
        }
    }
}