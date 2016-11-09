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

namespace MainRoad.Controls
{
    public partial class GamePanel : UserControl
    {
        PointF MapPosition;
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
            gr.ScaleTransform(Game.TILE_SIZE*Zoom, Game.TILE_SIZE*Zoom);
            gr.TranslateTransform(-MapPosition.X, -MapPosition.Y);

            // draw tails
            var fromX =(int)MapPosition.X;
            var toX = (int)(MapPosition.X + Width / (Game.TILE_SIZE * Zoom)) + 1;
            var fromY = (int)MapPosition.Y;
            var toY = (int)(MapPosition.Y + Height /(Game.TILE_SIZE * Zoom)) + 1;
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
                                gr.DrawImage(bmp, x, y, 1, 1);
                        }
                    }   
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
