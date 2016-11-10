using MainRoadModel.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainRoadModel
{
    public static class TileBuilder
    {
        public static void Build()
        {
            // build road tiles
            CreateTiles();
            // adjust tiles
            AdjustTiles();
        }

        private static void AdjustTiles()
        {
            TopologyHelper.IsFilled = HasRoad; 

            foreach (var cell in Game.State.Cells)
            {
                var tile = cell.Tiles.FirstOrDefault(t=> t is RoadTile);
                if (tile != null)
                {
                    var p = new Point(cell.CellX, cell.CellY);
                    var code = p.GetFilledNeighborsCode(topologyType : TopType.N4);
                    switch (code)
                    {
                        case 0101: tile.Name = "mainroad.png"; break;
                        case 1111: tile.Name = "secondcrossroad.png"; break;
                        case 1010: tile.Name = "mainroad.png"; tile.Rotate = RotateFlipType.Rotate270FlipNone; break;
                        case 0110: tile.Name = "nineradius.png"; break;
                        case 1100: tile.Name = "nineradius.png"; tile.Rotate = RotateFlipType.Rotate270FlipNone; break;
                        case 1101: tile.Name = "thirdTroad.png"; tile.Rotate = RotateFlipType.Rotate180FlipNone; break;
                        case 1110: tile.Name = "thirdTroad.png"; tile.Rotate = RotateFlipType.Rotate270FlipNone; break;
                        case 1011: tile.Name = "thirdTroad.png"; tile.Rotate = RotateFlipType.Rotate90FlipNone; break;
                        case 1001: tile.Name = "nineradius.png"; tile.Rotate = RotateFlipType.Rotate180FlipNone; break;
                        case 0111: tile.Name = "thirdTroad.png"; break;
                        case 0011: tile.Name = "nineradius.png"; tile.Rotate = RotateFlipType.Rotate90FlipNone; break;
                        case 1000: tile.Name = "endofroad.png"; tile.Rotate = RotateFlipType.Rotate90FlipNone; break;
                        case 0010: tile.Name = "endofroad.png"; tile.Rotate = RotateFlipType.Rotate270FlipNone; break;
                        case 0100: tile.Name = "endofroad.png"; tile.Rotate = RotateFlipType.Rotate180FlipNone; break;
                        case 0001: tile.Name = "endofroad.png"; break;


                    }      
                }
            }
        }

        static bool HasRoad(Point p)
        {
            return Game.State[p.X, p.Y].Tiles.Any(t=> t is RoadTile);  
        }

        private static void CreateTiles()
        {
            foreach (var node in Game.State.Nodes)
            {
                if (node.RoadsOut.Count > 0)
                    Game.State.Cells[node.CellX, node.CellY].Tiles.AddLast(new RoadTile());

                foreach (var road in node.RoadsOut)
                {
                    var other = road.To;
                    if (node.CellX == other.CellX)
                    {
                        //vertical road 
                        var dy = Math.Sign(other.CellY - node.CellY);
                        for (var y = node.CellY + dy; y < other.CellY; y += dy)
                            Game.State.Cells[node.CellX, y].Tiles.AddLast(new RoadTile());
                    }
                    else
                    {
                        // horizontal road
                        var dx = Math.Sign(other.CellX - node.CellX);
                        for (var x = node.CellX + dx; x < other.CellX; x += dx)
                            Game.State.Cells[x, node.CellY].Tiles.AddLast(new RoadTile());
                    }

                }
            }
        }
    }
}
