using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication354
{
    class Builder
    {
        public Bitmap Result;
        public int PixelsPerCell = 50;
        public int ShadowLength = 130;
        public float ShadowDepth = 0.35f;
        public float ShadowDirectionX = -0.5f;
        public float ShadowDirectionY = -0.9f;

        public void Build(Map map)
        {
            if (Result != null)
                Result.Dispose();
            Result = new Bitmap(map.ROI.Width * PixelsPerCell, map.ROI.Height * PixelsPerCell);
            map.ROI = map.ROI.Inflate(1, 1);
            //асфальт
            //new Asphalt(){SpriteName = "tiles.png"}.Apply(map.ROI);

            //Render(map, map.ROI);

            //building roof
            var roi = map.ROI.Inflate(1 + Tool.Rnd.Next(2), 1 + Tool.Rnd.Next(2));
            var set = Tool.GetAny(new string[] { "1_a.png", "5_a.png", "1_a.png", "5_a.png", "elem_2.jpg", "elem_4.jpg", "elem_5.jpg", "elem_6.jpg", "elem_7.jpg" });
            new Filler(){SpriteName = set}.Apply(roi);

            roi = map.ROI;

            //clear place for road
            new RoadTool().Apply(map.ROI);

            var roofCells = roi.Where(p => map[p].Type != CellType.Free && map[p].Type != CellType.OutsideElement).ToArray();
            //RenderShadow(roofCells);

            //border
            set = Tool.GetAny(new string[] { "10", "11", "13", "15" });
            new Border() { SpriteName = set + "_d.png", SpriteNameCorner = set + "_c.png", SpriteNameCorner2 = set + "_c1.png" }.Apply(roi);

            //decorator
            if (Tool.Rnd.Next(3) == 0) 
                new Decorator() { Type = 1, SpriteName = "stay.png" }.Apply(roi);
            new Decorator() { Type = 0, SpriteName = "trash.png" }.Apply(roi);
            new Decorator() { Type = 0, SpriteName = "pipe.png" }.Apply(roi);
            new Decorator() { Type = 0, SpriteName = "bench2.png" }.Apply(roi);
            new Decorator() { Type = 0, SpriteName = "stairs.png" }.Apply(roi);
            if (Tool.Rnd.Next(2) == 0)
                new Decorator() { Type = 0, SpriteName = Tool.GetAny(new[] { "8_b.png", "9_b.png" }) }.Apply(roi);
            

            //helicopter place
            if(Tool.Rnd.Next(5) == 0)
                new Elements() { Count = 1, SpriteName = "helic_part_1.png" }.Apply(roi);


            //random elements
            var arr = new string[] { "2_b.png", "2_b.png", "4_b.png", "6_b.png", "7_a.png", "12_b.png", "elem_1.jpg", "elem_3.jpg", "elem_10.jpg", "elem_12.jpg", "elem_13.jpg" };
            
            set = Tool.GetAny(arr);
            new Elements() { Count = 3, SpriteName = set }.Apply(roi);
            set = Tool.GetAny(arr);
            new Elements() { Count = 3, SpriteName = set }.Apply(roi);

            arr = new string[] { "roof (2).png", "roof (3).png", "roof (6).png" };
            set = Tool.GetAny(arr);
            new Elements() { Count = 1, SpriteName = set }.Apply(roi);

            set = Tool.GetAny(new string[] { "2_b.png" });
            new Elements() { Count = 3, SpriteName = set }.Apply(roi);

            //
            Render(map, roi.Where(p=>map[p].Type == CellType.OutsideElement));
            RenderShadow(roofCells, ShadowLength);
            Render(map, roi.Where(p => map[p].Type != CellType.OutsideElement && map[p].Type != CellType.Free));
            RenderShadow(roi.Where(p => map[p].Type == CellType.InsideElement), ShadowLength / 10);
            Render(map, roi.Where(p => map[p].Type == CellType.InsideElement));

            //Надстройка
            BuildSecondFloor(map);
        }

        private void BuildSecondFloor(Map map)
        {
            string set;
            var cells = map.ROI.Where(p => map[p].Type == CellType.Flat).ToArray();
            for (int i = 0; i < 6; i++)
            {
                var p = Tool.GetAny(cells);

                var roi = new TopologyHelper.ROI(3, 3, 3 + Tool.Rnd.Next(map.ROI.Width / 4), 3 + Tool.Rnd.Next(map.ROI.Height / 4));
                roi.X = p.X;
                roi.Y = p.Y;
                if (cells.IsInside(roi))
                {
                    set = Tool.GetAny(new string[] { "1_a.png", "5_a.png" });
                    new Filler() { SpriteName = set }.Apply(roi);

                    set = Tool.GetAny(new string[] { "10", "11", "13", "15" });
                    new ROIBorder() { SpriteName = set + "_d.png", SpriteNameCorner = set + "_c.png", SpriteNameCorner2 = set + "_c1.png" }.Apply(roi);

                    RenderShadow(roi, ShadowLength / 3);
                    Render(map, roi);
                    RenderBorder(roi);

                    return;
                }
            }
        }

        private void Render(Map map, IEnumerable<TopologyHelper.Point> cells)
        {
            using (var gr = Graphics.FromImage(Result))
            {
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.ScaleTransform(PixelsPerCell, PixelsPerCell);

                foreach (var p in cells)
                {
                    var cell = map[p];
                    if (string.IsNullOrEmpty(cell.SpriteName))
                    {
                        Brush brush = null;
                        switch (cell.Type)
                        {
                            case CellType.Free:
                                break;
                            case CellType.Flat:
                                brush = Brushes.Silver;
                                break;
                            case CellType.NotFlat:
                                brush = Brushes.Gray;
                                break;
                        }
                        if (brush != null)
                            gr.FillRectangle(brush, p.X, p.Y, 1, 1);
                    }
                    else
                    {
                        var img = BitmapCache.GetBitmap(cell.SpriteName, cell.Rotate);
                        gr.DrawImage(img, p.X, p.Y, 1, 1);
                    }
                }
            }
        }

        private void RenderShadow(IEnumerable<TopologyHelper.Point> cells, int length)
        {
            using(var shadow = new Bitmap(Result.Width, Result.Height))
            {
                using (var gr = Graphics.FromImage(shadow))
                {
                    gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    gr.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                    //var reg = new Region();
                    foreach (var p in cells)
                    for (int i = -2; i < length; i += 2)
                        gr.FillRectangle(Brushes.Black, p.X * PixelsPerCell + i * ShadowDirectionX, p.Y * PixelsPerCell + i * ShadowDirectionY, PixelsPerCell, PixelsPerCell);

                    //reg.Union(new Rectangle((int)(p.X*PixelsPerCell + i*ShadowDirectionX), (int)(p.Y*PixelsPerCell + i*ShadowDirectionY), PixelsPerCell, PixelsPerCell));
                    //gr.SetClip(reg, CombineMode.Replace);
                    //gr.FillRectangle(Brushes.Black, 0, 0, 1000, 1000);
                    //gr.FillRectangle(Brushes.Black, p.X * PixelsPerCell + i * ShadowDirectionX, p.Y * PixelsPerCell + i * ShadowDirectionY, PixelsPerCell, PixelsPerCell);
                }

                using (var gr = Graphics.FromImage(Result))
                {
                    GraphicsHelper.DrawImage(gr, shadow, Point.Empty, ShadowDepth);
                }
            }
        }

        private void RenderBorder(TopologyHelper.ROI roi, float width = 4)
        {
            using (var gr = Graphics.FromImage(Result))
            {
                using (var pen = new Pen(Color.FromArgb(100, Color.Black), width))
                gr.DrawRectangle(pen, roi.X * PixelsPerCell, roi.Y * PixelsPerCell, roi.Width * PixelsPerCell, roi.Height * PixelsPerCell);
            }
        }
    }
}
