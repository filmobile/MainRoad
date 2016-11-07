using System;

namespace MainRoadModel.Model
{
    /// <summary>
    /// Road between nodes
    /// </summary>
    [Serializable]
    class Road
    {
        public Node From;
        public Node To;
    }
}