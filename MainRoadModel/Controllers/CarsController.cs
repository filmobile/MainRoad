using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainRoadModel.Helpers;
using MainRoadModel.Model;

namespace MainRoadModel
{
    public class CarsController
    {
        CollisionMap map = new CollisionMap();


        public void Update()
        {
            //create cars
            if (Game.State.Cars.Count < Game.CarCount)
                CreateCars(Game.CarCount - Game.State.Cars.Count);

            map.Clear();

            //prepare
            foreach (var car in Game.State.Cars)
            {
                UpdateTarget(car);
                //calc dir to target node
                var target = car.Path.First.Value;
                var dir = target.Sub(car.Position);
                var dist = dir.Length();
                dir = dist < 0.0001f ? new PointF(1, 0) : dir.Mul(1 / dist);
                car.Direction = car.Direction.Lerp(dir, 0.2f);
                //adjust speed
                var k = car.Direction.DotScalar(dir);
                var targetSpeed = car.MaxSpeed;
                if (car.Path.Count > 1)
                {
                    if (k > 0.7f)
                        targetSpeed = car.MaxSpeed;
                    else
                        targetSpeed = car.MaxSpeed * 0.5f;
                }
                car.Speed = (car.Speed * 2 + targetSpeed) / 3;
                //create collision map
                map.AddCarToMap(car);
            }

            var newCarPositions = new List<Tuple<Car, PointF>>();
            //call new car coordinates
            foreach (var car in Game.State.Cars)
            {
                var newPos = CalcNewCarPosition(car);
                newCarPositions.Add(new Tuple<Car, PointF>(car, newPos));
            }

            //update car coordinates
            foreach (var pair in newCarPositions)
                pair.Item1.Position = pair.Item2;
        }

        private static void UpdateTarget(Car car)
        {
            var target = car.Path.First.Value;
            //update direction of moving
            //we are reach target point ?
            if (target.DistanceTo(car.Position) < 0.1f)
            {
                car.Path.RemoveFirst();
                if (car.Path.Count == 0)
                {
                    //выбираем случайным образом следующую дорогу
                    var otherNode = Game.State.Nodes.GetRnd();
                    var node = FindNearestNode(car.Position);
                    var nodePath = AStar.FindPath(node, otherNode, Distance, last => Distance(last, otherNode)).Reverse();
                    car.Path = new LinkedList<PointF>(CreatePathPoints(nodePath.Select(n => n.Position).ToList(), car));
                }
            }
        }

        private static IEnumerable<PointF> CreatePathPoints(IList<PointF> nodes, Car car)
        {
            var prevDir = RoundDir(car.Direction);
            var pos = car.Position;

            //var p = nodes[0].Add(prevDir.Rotate90().Mul(0.5f));
            yield return nodes[0];

            for (int i = 0; i < nodes.Count - 1; i++)
            {
                var p1 = nodes[i];
                var p2 = nodes[i + 1];
                var dir = p2.Sub(p1).Normalized();
                yield return p1.Add(dir.Rotate((float)(Math.PI / 6)).Mul(0.4f));
                yield return p2.Add(dir.Rotate((float)(5 * Math.PI / 6)).Mul(0.4f));
            }
        }

        static PointF RoundDir(PointF dir)
        {
            dir = new PointF((int)Math.Round(dir.X), (int)Math.Round(dir.Y));
            if (dir.Length() < 0.001f) return new PointF(1, 0);
            return dir;
        }

        private static Node FindNearestNode(PointF pos)
        {
            var x = (int)pos.X;
            var y = (int)pos.Y;
            if (Game.State[x, y].Node != null)
                return Game.State[x, y].Node;

            foreach (var p in TopologyHelper.GetNeighbors(new Point(x, y)))
            {
                if (Game.State[p.X, p.Y].Node != null)
                    return Game.State[p.X, p.Y].Node;
            }

            return Game.State.Nodes.GetRnd();
        }

        static double Distance(Node n1, Node n2)
        {
            var dx = Math.Abs(n1.CellX - n2.CellX);
            var dy = Math.Abs(n1.CellY - n2.CellY);
            return dx + dy;
        }

        private PointF CalcNewCarPosition(Car car)
        {
            var p = car.Position.Add(car.Direction.Mul(car.Speed));

            foreach (var other in map.GetNeighbors(car))
            {
                var dirToOther = other.Position.Sub(car.Position);
                //если впереди
                if (car.Direction.DotScalar(dirToOther) > 0.3f)
                {
                    var dist = dirToOther.Length();
                    if (dist < car.Length + other.Length)
                    {
                        //если движения в разные стороны, то немного сдвигаемся вправо
                        if (car.Direction.DotScalar(other.Direction) < 0)
                        {
                            
                        }else
                            //столконовение, стоим на месте
                            return car.Position; //возвращаем старую позицию
                    }
                }
            }
            //возвращаем новую позицию
            return p;
        }

        private void CreateCars(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var car = new Car();
                var node = Game.State.Nodes.GetRnd();
                car.Path.AddLast(node.Position);
                car.Position = node.Position;
                Game.State.Cars.AddLast(car);
            }
        }
    }

    class CollisionMap
    {
        List<Car>[,] map;

        public CollisionMap()
        {
            map = new List<Car>[Game.GRID_SIZE + 2, Game.GRID_SIZE + 2];
            for (int i = 0; i < map.GetLength(0); i++)
            for (int j = 0; j < map.GetLength(1); j++)
                map[i, j] = new List<Car>();
        }

        public void Clear()
        {
            var l1 = map.GetLength(0);
            var l2 = map.GetLength(1);
            for (int i = 0; i < l1; i++)
            for (int j = 0; j < l2; j++)
                map[i, j].Clear();
        }

        public void AddCarToMap(Car car)
        {
            var x = (int) car.Position.X + 1;
            var y = (int) car.Position.Y + 1;
            map[x, y].Add(car);
        }


        public IEnumerable<Car> GetNeighbors(Car car)
        {
            var x = (int)car.Position.X + 1;
            var y = (int)car.Position.Y + 1;

            if (x < 1 || x >= Game.GRID_SIZE) yield break;
            if (y < 1 || y >= Game.GRID_SIZE) yield break;

            for (int i = x - 1; i <= x + 1; i++)
            for (int j = y - 1; j <= y + 1; j++)
            foreach (var c in map[i, j])
            if (c != car)
                yield return c;
        }
    }

    public class CarsController_old
    {
        public int NeededCarCount = 500;
        static Random rnd = new Random();

        public void Update()
        {
            //create cars
            if (Game.State.Cars.Count < NeededCarCount)
                CreateCars(NeededCarCount - Game.State.Cars.Count);

            //update cars coordinates
            foreach (var node in Game.State.Nodes)
            foreach (var road in node.RoadsOut)
                UpdateCarsOnRoad(road);
        }

        private void UpdateCarsOnRoad(Road road)
        {
            var n = road.Cars.First;
            while (n != null)
            {
                UpdateCarPositionOnRoad(road, n);
                n = n.Next;
            }
        }

        private void UpdateCarPositionOnRoad(Road road, LinkedListNode<Car> n)
        {

            var prev = n.Previous;
            //pos of prev car
            var prevPos = prev == null ? 1000 : prev.Value.PosAlongRoad;
            var prevCarLength = prev == null ? 0 : prev.Value.Length;
            var car = n.Value;
            var pos = car.PosAlongRoad;
            if (pos + car.Speed > prevPos - prevCarLength) //пробка, стоим
                return;

            //если мы уже доехали до перекрестка...
            if (car.PosAlongRoad + car.Speed >= 1)
            {
                //выбираем случайным образом следующую дорогу
                road = road.To.RoadsOut.Count <= 1
                    ? road.To.RoadsOut.First.Value
                    : road.To.RoadsOut.Where(r => r.To != road.From).GetRnd();
                //если дорога не заполнена - выезжаем на нее
                var prevCar = road.Cars.Last;
                if (prevCar == null || prevCar.Value.PosAlongRoad - prevCar.Value.Length > 0)
                {
                    n.List.Remove(n);
                    road.Cars.AddLast(car);
                    car.PosAlongRoad = 0;
                }
            }
            else
            {
                //едем вдоль текущей дороги
                car.PosAlongRoad = car.PosAlongRoad + car.Speed;
            }

            var p1 = new PointF(road.From.CellX, road.From.CellY);
            var p2 = new PointF(road.To.CellX, road.To.CellY);

            car.Position = p1.Lerp(p2, car.PosAlongRoad);
        }

        private void CreateCars(int count)
        {
            var roads = Game.State.Nodes.Select(n=>n.RoadsOut.FirstOrDefault()).ToArray();
            var rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                var car = new Car();
                roads[rnd.Next(roads.Length)].Cars.AddLast(car);
                Game.State.Cars.AddLast(car);
            }
        }
    }
}
