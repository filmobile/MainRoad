using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MainRoadModel;
using MainRoadModel.Helpers;

namespace MainRoad.Controls
{
    public partial class GamePanel : UserControl
    {
        PointF MapPosition = new PointF(Game.GRID_SIZE  / 2, Game.GRID_SIZE / 2);
        float Zoom = 1;
        Point oldMousePos;

        public GamePanel()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint|ControlStyles.OptimizedDoubleBuffer|ControlStyles.UserPaint|ControlStyles.ResizeRedraw,true);


        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Game.State == null)
                return;

            var gr = e.Graphics;
            //gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            //gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            //gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;

            //gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            //gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            //gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            var scale = Game.TILE_SIZE*Zoom;

            gr.TranslateTransform(Width / 2, Height / 2);
            gr.ScaleTransform(scale, scale);
            gr.TranslateTransform(-MapPosition.X, -MapPosition.Y);



            // draw tails
            var fromX =(int)(MapPosition.X - Width / (2 * scale));
            var toX = (int)Math.Min(Game.GRID_SIZE - 1, MapPosition.X + Width / ( 2 * scale) + 2);
            var fromY = (int)(MapPosition.Y - Height / (2 * scale));
            var toY = (int)Math.Min(Game.GRID_SIZE - 1, MapPosition.Y + Height / ( 2 * scale) + 2);
            using (var pen = new Pen(Color.Black,1/Game.TILE_SIZE)  )
                for (int x = fromX; x < toX; x++)
                    for (int y = fromY; y < toY; y++)
                    {
                        //gr.DrawRectangle(pen,x,y,1,1);
                        var cell = Game.State[x, y];
                        foreach (var tile in cell.Tiles)
                        {
                            var bmp = BitmapCache.GetBitmap(tile.Name, tile.Rotate);
                            if(bmp !=null)
                                //gr.DrawImage(bmp, x,y,1.05f,1.05f);
                                gr.DrawImage(bmp, x - 0.5f, y - 0.5f, 1.01f, 1.01f);
                        }
                        //e.Graphics.FillEllipse(Brushes.Silver, x - 0.05f, y - 0.05f, 0.1f, 0.1f);
                    }

            //draw cars
            foreach (var car in Game.State.Cars)
            {
                //e.Graphics.FillEllipse(Brushes.Red, car.Position.X - 0.1f, car.Position.Y - 0.1f, 0.2f, 0.2f);
                var state = gr.Save();
                gr.TranslateTransform(car.Position.X, car.Position.Y);
                gr.RotateTransform(car.Direction.Angle() * 180 / (float)Math.PI);
                e.Graphics.FillRectangle(Brushes.Red, - 0.15f, - 0.1f, 0.3f, 0.2f);
                gr.Restore(state);
                //foreach(var p in car.Path)
                //e.Graphics.FillEllipse(Brushes.Magenta, p.X - 0.05f, p.Y - 0.05f, 0.1f, 0.1f);
            }
        }

        public PointF ClientToImagePoint(PointF point)
        {
            var dx = (point.X - ClientSize.Width / 2f) / Zoom + MapPosition.X;
            var dy = (point.Y - ClientSize.Height / 2f) / Zoom + MapPosition.Y;
            return new PointF(dx, dy);
        }

        public PointF ImagePointToClient(PointF point)
        {
            var dx = (point.X - MapPosition.X) * Zoom + ClientSize.Width / 2f;
            var dy = (point.Y - MapPosition.Y) * Zoom + ClientSize.Height / 2f;
            return new PointF(dx, dy);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var dx = e.X - oldMousePos.X;
                var dy = e.Y - oldMousePos.Y;
                MapPosition = new PointF(MapPosition.X - dx/(Game.TILE_SIZE * Zoom), MapPosition.Y - dy/(Game.TILE_SIZE * Zoom));
                Invalidate();
            }

            oldMousePos = e.Location;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            Zoom *= e.Delta > 0 ? 1.1f : 0.9f;
            Invalidate();
        }
    }
}
