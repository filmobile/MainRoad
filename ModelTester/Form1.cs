using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MainRoadModel;

namespace ModelTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            CityBuilder.Create();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            AutoScrollMinSize = new Size(5000, 5000);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(-HorizontalScroll.Value, -VerticalScroll.Value);
            e.Graphics.ScaleTransform(5, 5);

            //draw graph
            foreach (var n in Game.State.Nodes)
            {
                e.Graphics.FillEllipse(Brushes.Blue, n.CellX -0.5f, n.CellY - 0.5f, 1, 1);

                using(var pen = new Pen(Color.Red, 0.1f))
                foreach (var road in n.Roads)
                    e.Graphics.DrawLine(pen, road.From.CellX, road.From.CellY, road.To.CellX, road.To.CellY);
            }
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            Invalidate();
        }
    }
}
