using System.Collections.Generic;
using System.Drawing;

namespace WindowsFormsApplication354
{
    class Map
    {
        public Cell[,] Cells;
        public TopologyHelper.ROI ROI;

        public Map(int width, int height)
        {
            Cells = new Cell[width,height];
            ROI = new TopologyHelper.ROI(0, 0, width, height);
            foreach (var p in ROI)
                Cells[p.X, p.Y] = new Cell();
        }

        public Cell this[TopologyHelper.Point p]
        {
            get { return this[p.X, p.Y]; }
        }

        public Cell this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= Cells.GetLength(0)) return new Cell();
                if (y < 0 || y >= Cells.GetLength(1)) return new Cell();
                return Cells[x, y];
            }
        }

        public bool IsFilled(TopologyHelper.Point point)
        {
            return this[point.X, point.Y].Type != CellType.Free && this[point.X, point.Y].Type != CellType.OutsideElement;
        }

        public Map Clone()
        {
            return new Map(ROI.Width, ROI.Height) {Cells = (Cell[,]) Cells.Clone(), ROI = ROI};
        }
    }

    class Cell
    {
        public CellType Type;
        public string SpriteName;
        public RotateFlipType Rotate = RotateFlipType.RotateNoneFlipNone;
    }

    enum CellType
    {
        Free, Flat, NotFlat, OutsideElement, InsideElement
    }

    class MapLayers : List<Map>
    {
    }
}