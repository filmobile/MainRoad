using System;

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

        public Road(Node from, Node to)
        {
            this.From = from;
            this.To = to;
        }
    }
}