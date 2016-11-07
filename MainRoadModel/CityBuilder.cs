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
        public static void Create()
        {
            //new game
            Game.State = new Model.GameState();
            var state = Game.State;

            //Create cells and regular graph
            CreateCellsAndRegularGraph(state);
        }

        private static void CreateCellsAndRegularGraph(GameState state)
        {
            //create cells
            for (int y = 0; y < Game.GRID_SIZE; y += 1)
            for (int x = 0; x < Game.GRID_SIZE; x += 1)
            {
                state.Cells[x, y] = new Cell();
            }

            //create regular graph
            for (int y = 0; y < Game.GRID_SIZE; y += 2)
            for (int x = 0; x < Game.GRID_SIZE; x += 2)
            {
                var n = new Node() {CellX = x, CellY = y};
                state.Cells[x, y].Node = n;
                state.Nodes.AddLast(n);
            }

            //create regular roads (edges between nodes)
            for (int y = 0; y < Game.GRID_SIZE - 2; y += 2)
                for (int x = 0; x < Game.GRID_SIZE - 2; x += 2)
                {
                    var n1 = state.Cells[x, y].Node;
                    var n2 = state.Cells[x + 2, y].Node;
                    var n3 = state.Cells[x, y + 2].Node;
                    n1.Roads.AddLast(new Road(n1, n2));
                    n2.Roads.AddLast(new Road(n2, n1)); //two - way traffic
                    n1.Roads.AddLast(new Road(n1, n3));
                    n3.Roads.AddLast(new Road(n3, n1)); //two - way traffic
                }

            //last roads
            for (int x = 0; x < Game.GRID_SIZE - 2; x += 2)
            {
                var n1 = state.Cells[x, Game.GRID_SIZE - 2].Node;
                var n2 = state.Cells[x + 2, Game.GRID_SIZE - 2].Node;
                n1.Roads.AddLast(new Road(n1, n2));
                n2.Roads.AddLast(new Road(n2, n1)); //two - way traffic
            }

            for (int y = 0; y < Game.GRID_SIZE - 2; y += 2)
            {
                var n1 = state.Cells[Game.GRID_SIZE - 2, y].Node;
                var n2 = state.Cells[Game.GRID_SIZE - 2, y + 2].Node;
                n1.Roads.AddLast(new Road(n1, n2));
                n2.Roads.AddLast(new Road(n2, n1)); //two - way traffic
            }
        }
    }
}
