using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainRoadModel.Model;

namespace MainRoadModel
{
    public static class CityBuilder
    {
        static Random rnd;

        static int DecimationCount = Game.GRID_SIZE * Game.GRID_SIZE / 70;
        static int BuildingsCount = Game.GRID_SIZE * Game.GRID_SIZE / 100;

        public static void Create(int randomSeed = 0)
        {
            rnd = new Random(randomSeed);

            //new game
            Game.State = new Model.GameState();
            var state = Game.State;

            //Create cells and regular graph
            CreateCellsAndRegularGraph(state);

            //Graph decimation
            MakeDecimation(DecimationCount);
            MakeBuildings(BuildingsCount);

            //remove nodes w/o roads
            foreach(var n in Game.State.Nodes.ToArray())
                if (n.RoadsIn.Count == 0)
                {
                    RemoveNode(n);
                }
        }

        static void MakeDecimation(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var x = rnd.Next(Game.GRID_SIZE / Game.NODE_STEP) * Game.NODE_STEP;
                var y = rnd.Next(Game.GRID_SIZE / Game.NODE_STEP) * Game.NODE_STEP;
                var n = Game.State[x, y].Node;
                if (n == null)
                    continue;

                if(n.CellX == 0 || n.CellY == 0 || n.CellX == Game.GRID_SIZE - Game.NODE_STEP || n.CellY == Game.GRID_SIZE - Game.NODE_STEP)
                    continue; // do not remove on bounds of map

                RemoveNode(n);
            }
        }

        static void RemoveNode(Node n)
        {
            //remove all roads
            foreach (var r in n.RoadsIn.ToArray())
                RemoveRoad(r);
            foreach (var r in n.RoadsOut.ToArray())
                RemoveRoad(r);

            //remove node
            Game.State.Nodes.Remove(n);
            Game.State.Cells[n.CellX, n.CellY].Node = null;
        }

        static void RemoveRoad(Road r)
        {
            r.From.RoadsOut.Remove(r);
            r.To.RoadsIn.Remove(r);
        }

        static void MakeBuildings(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var x = rnd.Next(Game.GRID_SIZE / Game.NODE_STEP) * Game.NODE_STEP;
                var y = rnd.Next(Game.GRID_SIZE / Game.NODE_STEP) * Game.NODE_STEP;
                var n = Game.State[x, y].Node;

                if (n == null)
                    continue;

                if (n.CellX == 0 || n.CellY == 0 || n.CellX == Game.GRID_SIZE - Game.NODE_STEP || n.CellY == Game.GRID_SIZE - Game.NODE_STEP)
                    continue; // do not remove on bounds of map

                var roads = n.RoadsIn.ToArray();
                if (roads.Length == 0)
                    continue;

                n.Name = "Building";
                var stillRoad = roads[rnd.Next(roads.Length)];//still one random road
                var otherNode = stillRoad.From == n ? stillRoad.To : stillRoad.From;

                //remove all roads
                foreach (var r in n.RoadsIn.ToArray())
                    RemoveRoad(r);
                foreach (var r in n.RoadsOut.ToArray())
                    RemoveRoad(r);

                //add one road
                CityBuilder.AddTwoWayRoad(n, otherNode);
            }
        }

        private static void CreateCellsAndRegularGraph(GameState state)
        {
    //create regular graph
            for (int y = 0; y < Game.GRID_SIZE; y += Game.NODE_STEP)
            for (int x = 0; x < Game.GRID_SIZE; x += Game.NODE_STEP)
            {
                var n = new Node() {CellX = x, CellY = y};
                state.Cells[x, y].Node = n;
                state.Nodes.AddLast(n);
            }

            //create regular roads (edges between nodes)
            for (int y = 0; y < Game.GRID_SIZE - Game.NODE_STEP; y += Game.NODE_STEP)
            for (int x = 0; x < Game.GRID_SIZE - Game.NODE_STEP; x += Game.NODE_STEP)
                {
                    var n1 = state.Cells[x, y].Node;
                    var n2 = state.Cells[x + Game.NODE_STEP, y].Node;
                    var n3 = state.Cells[x, y + Game.NODE_STEP].Node;
                    AddTwoWayRoad(n1, n2);
                    AddTwoWayRoad(n1, n3);
                }

            //last roads
           for (int x = 0; x < Game.GRID_SIZE - Game.NODE_STEP; x += Game.NODE_STEP)
            {
                var n1 = state.Cells[x, Game.GRID_SIZE - Game.NODE_STEP].Node;
                var n2 = state.Cells[x + Game.NODE_STEP, Game.GRID_SIZE - Game.NODE_STEP].Node;
                AddTwoWayRoad(n1, n2);
            }

            for (int y = 0; y < Game.GRID_SIZE - Game.NODE_STEP; y += Game.NODE_STEP)
            {
                var n1 = state.Cells[Game.GRID_SIZE - Game.NODE_STEP, y].Node;
                var n2 = state.Cells[Game.GRID_SIZE - Game.NODE_STEP, y + Game.NODE_STEP].Node;
                AddTwoWayRoad(n1, n2);
            }
        }

        public static void AddTwoWayRoad(Node n1, Node n2)
        {
            var r1 = new Road(n1, n2);
            n1.RoadsOut.AddLast(r1);
            n2.RoadsIn.AddLast(r1);

            //two - way traffic
            var r2 = new Road(n2, n1);
            n1.RoadsIn.AddLast(r2);
            n2.RoadsOut.AddLast(r2);
        }
    }
}
