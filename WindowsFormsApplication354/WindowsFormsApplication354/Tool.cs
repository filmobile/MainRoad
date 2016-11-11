using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication354
{
    using ROI = TopologyHelper.ROI;
    using Point = TopologyHelper.Point;

    abstract class Tool
    {
        public static Map InstanceMap;
        public static Random Rnd = new Random(0);

        public string SpriteName;
        public Map Map{ get { return InstanceMap; }}
        public abstract bool Apply(ROI roi);

        public static T GetAny<T>(IList<T> list)
        {
            var i = Rnd.Next(list.Count);
            return list[i];
        }
    }


    /// <summary>
    /// Выгрызает пустые ячейки вдоль границы области (ставит либо травку, либо внешние объекты - бочки, мусор и т.д.)
    /// </summary>
    class Cutter : Tool
    {
        public override bool Apply(ROI roi)
        {
            foreach (var p in roi.GetBorder().ToArray())
                Map.Cells[p.X, p.Y] = new Cell() { Type = CellType.Free, SpriteName = SpriteName };
            return true;
        }
    }

    /// <summary>
    /// Создает тайлы вдоль границы области (внутри , без учета вырезанных)
    /// </summary>
    class Border : Tool
    {
        public string SpriteNameCorner;
        public string SpriteNameCorner2;

        public override bool Apply(ROI roi)
        {
            foreach (var p in roi.GetBorder().ToArray())
            {
                var c = new Cell() {Type = CellType.NotFlat};
                var code = p.GetFilledNeighborsCode(topologyType: TopType.N4);
                switch(code)
                {
                    case 1101: c.SpriteName = SpriteName; c.Rotate = RotateFlipType.RotateNoneFlipNone; break;
                    case 0111: c.SpriteName = SpriteName; c.Rotate = RotateFlipType.Rotate180FlipNone; break;
                    case 1011: c.SpriteName = SpriteName; c.Rotate = RotateFlipType.Rotate90FlipX; break;
                    case 1110: c.SpriteName = SpriteName; c.Rotate = RotateFlipType.Rotate90FlipNone; break;
                    case 1100: c.SpriteName = SpriteNameCorner; c.Rotate = RotateFlipType.RotateNoneFlipNone; break;
                    case 0110: c.SpriteName = SpriteNameCorner; c.Rotate = RotateFlipType.Rotate90FlipNone; break;
                    case 0011: c.SpriteName = SpriteNameCorner; c.Rotate = RotateFlipType.Rotate180FlipNone; break;
                    case 1001: c.SpriteName = SpriteNameCorner; c.Rotate = RotateFlipType.Rotate270FlipNone; break;
                    case 1111:
                        switch(p.GetFilledNeighborsCode(topologyType: TopType.N8))
                        {
                            case 01111111: c.SpriteName = SpriteNameCorner2; c.Rotate = RotateFlipType.Rotate90FlipNone; break;
                            case 11011111: c.SpriteName = SpriteNameCorner2; c.Rotate = RotateFlipType.Rotate180FlipNone; break;
                            case 11110111: c.SpriteName = SpriteNameCorner2; c.Rotate = RotateFlipType.Rotate270FlipNone; break;
                            case 11111101: c.SpriteName = SpriteNameCorner2; c.Rotate = RotateFlipType.RotateNoneFlipNone; break;
                        }
                        break;
                }

                Map.Cells[p.X, p.Y] = c;
            }
            return true;
        }
    }

    /// <summary>
    /// Создает границу вунтри ROI
    /// </summary>
    class ROIBorder : Tool
    {
        public string SpriteNameCorner;
        public string SpriteNameCorner2;

        public override bool Apply(ROI roi)
        {
            foreach (var p in roi.Border())
            {
                var c = new Cell() { Type = CellType.NotFlat };
                var code = p.GetFilledNeighborsCode(pp => roi.Contains(pp), topologyType: TopType.N4);
                switch (code)
                {
                    case 1101: c.SpriteName = SpriteName; c.Rotate = RotateFlipType.RotateNoneFlipNone; break;
                    case 0111: c.SpriteName = SpriteName; c.Rotate = RotateFlipType.Rotate180FlipNone; break;
                    case 1011: c.SpriteName = SpriteName; c.Rotate = RotateFlipType.Rotate90FlipX; break;
                    case 1110: c.SpriteName = SpriteName; c.Rotate = RotateFlipType.Rotate90FlipNone; break;
                    case 1100: c.SpriteName = SpriteNameCorner; c.Rotate = RotateFlipType.RotateNoneFlipNone; break;
                    case 0110: c.SpriteName = SpriteNameCorner; c.Rotate = RotateFlipType.Rotate90FlipNone; break;
                    case 0011: c.SpriteName = SpriteNameCorner; c.Rotate = RotateFlipType.Rotate180FlipNone; break;
                    case 1001: c.SpriteName = SpriteNameCorner; c.Rotate = RotateFlipType.Rotate270FlipNone; break;
                    case 1111:
                        switch (p.GetFilledNeighborsCode(topologyType: TopType.N8))
                        {
                            case 01111111: c.SpriteName = SpriteNameCorner2; c.Rotate = RotateFlipType.Rotate90FlipNone; break;
                            case 11011111: c.SpriteName = SpriteNameCorner2; c.Rotate = RotateFlipType.Rotate180FlipNone; break;
                            case 11110111: c.SpriteName = SpriteNameCorner2; c.Rotate = RotateFlipType.Rotate270FlipNone; break;
                            case 11111101: c.SpriteName = SpriteNameCorner2; c.Rotate = RotateFlipType.RotateNoneFlipNone; break;
                        }
                        break;
                }

                Map.Cells[p.X, p.Y] = c;
            }
            return true;
        }
    }

    /// <summary>
    /// Создает асфальт вокруг здания
    /// </summary>
    class Asphalt : Tool
    {
        public override bool Apply(ROI roi)
        {
            foreach (var p in roi.ToArray())
            {
                var c = new Cell() { Type = CellType.Free, SpriteName = SpriteName };
                Map.Cells[p.X, p.Y] = c;
            }
            return true;
        }
    }

    /// <summary>
    /// Заливает область плитками
    /// </summary>
    class Filler : Tool
    {
        public override bool Apply(ROI roi)
        {
            foreach (var p in roi)
                Map.Cells[p.X, p.Y] = new Cell() { Type = CellType.Flat, SpriteName = SpriteName };
            return true;
        }
    }

    /// <summary>
    /// Делит область на отдельные области и обрабатывает их отдельно
    /// </summary>
    class Divider : Tool
    {
        public override bool Apply(ROI roi)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Навешивает вешние объекты снаружи границы области
    /// </summary>
    class Decorator : Tool
    {
        //0 - random
        //1 - regular
        public int Type;

        public int Step = 4;

        public override bool Apply(ROI roi)
        {
            int counter = 0;
            var dict = new Dictionary<Point, Cell>();
            var cells = roi.GetOutsideBorder().ToArray();
            foreach (var p in cells)
            {
                counter++;

                if (Map.Cells[p.X, p.Y].Type != CellType.Free) continue;
                //
                switch(Type)
                {
                    case 0:
                        {
                            if(Rnd.Next(cells.Length) > 2) continue;
                            break;
                        }

                    case 1:
                        {
                            if ((p.X + p.Y) % Step != 0) continue;
                            break;
                        }
                    default:
                        continue;
                }

                //create cells
                var c = new Cell() { Type = CellType.OutsideElement };
                switch (p.GetFilledNeighborsCode(topologyType: TopType.N8))
                {
                    case 00001110: c.SpriteName = SpriteName; c.Rotate = RotateFlipType.RotateNoneFlipNone; break;
                    case 10000011: c.SpriteName = SpriteName; c.Rotate = RotateFlipType.Rotate90FlipNone; break;
                    case 11100000: c.SpriteName = SpriteName; c.Rotate = RotateFlipType.Rotate180FlipNone; break;
                    case 00111000: c.SpriteName = SpriteName; c.Rotate = RotateFlipType.Rotate270FlipNone; break;
                    default: c = null; break;
                }

                if(c!= null)
                    dict[p] = c;
            }

            foreach(var pair in dict)
                Map.Cells[pair.Key.X, pair.Key.Y] = pair.Value;

            return true;
        }
    }

    class RoadTool : Tool
    {
        public override bool Apply(ROI roi)
        {
            var fromY = roi.Height / 3 + 1;
            var toY = 2 * roi.Height / 3;

            foreach (var p in roi.GetOutsideBorder())
                if (p.X < roi.X + roi.Width / 2 && p.Y >= fromY && p.Y <= toY)
                    Map.Cells[p.X, p.Y] = new Cell { Type = CellType.OutsideElement };//место под дорогу

            var dx = Rnd.Next(roi.Width / 2);
            if(Rnd.Next(2) == 1)
                foreach (var p in roi.GetBorder())
                    if (p.X < roi.X + dx && p.Y >= fromY && p.Y <= toY)
                        Map.Cells[p.X, p.Y] = new Cell { Type = CellType.OutsideElement };//место под дорогу
            return true;
        }
    }

    class Elements : Tool
    {
        public int Count = 2;

        public override bool Apply(ROI roi)
        {
            var list = new List<Point>();
            foreach(var point in roi.Where(t=>Map.Cells[t.X, t.Y].Type == CellType.Flat))
            {
                if(!point.IsBorder())
                    list.Add(point);
            }

            for (int i = 0; i < Count; i++)
            {
                var p = list[Rnd.Next(list.Count)];

                
                if(SpriteName.Contains("_part_1"))
                {
                    // 2 x 2 tile
                    if(Map.Cells[p.X + 1, p.Y].Type == CellType.Flat &&
                        Map.Cells[p.X + 1, p.Y + 1].Type == CellType.Flat &&
                        Map.Cells[p.X, p.Y + 1].Type == CellType.Flat)
                    {
                        Map.Cells[p.X, p.Y] = new Cell { SpriteName = SpriteName, Type = CellType.InsideElement };
                        Map.Cells[p.X + 1, p.Y] = new Cell { SpriteName = SpriteName.Replace("_part_1", "_part_2"), Type = CellType.InsideElement };
                        Map.Cells[p.X, p.Y + 1] = new Cell { SpriteName = SpriteName.Replace("_part_1", "_part_3"), Type = CellType.InsideElement };
                        Map.Cells[p.X + 1, p.Y + 1] = new Cell { SpriteName = SpriteName.Replace("_part_1", "_part_4"), Type = CellType.InsideElement };
                    }
                    continue;
                }

                var cx = Rnd.Next(3);
                var cy = Rnd.Next(3);
                for (int x = 0; x < cx; x++ )
                for (int y = 0; y < cy; y++)
                if(Map.Cells[p.X + x, p.Y + y].Type == CellType.Flat)
                    Map.Cells[p.X + x, p.Y + y] = new Cell { SpriteName = SpriteName, Type = CellType.InsideElement };
            }

            return true;
        }
    }
}
