using System.Collections.Generic;
using System.Drawing;

namespace MainRoadModel.Model
{
    public class Car
    {
        public PointF Position;
        public float MaxSpeed = 0.1f;
        public float Speed;
        public PointF Direction;
        public float PosAlongRoad;

        public LinkedList<PointF> Path;

        //public Road CurrentRoad;
        //public Node CurrentNode;
        public float Length = 0.2f;

        public Car()
        {
            MaxSpeed = FastRnd.Gauss(0.05f, 0.03f);
            Path = new LinkedList<PointF>();
        }
    }
}