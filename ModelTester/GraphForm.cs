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
using MainRoadModel.Helpers;
using MainRoadModel.Model;

namespace ModelTester
{
    public partial class GraphForm : Form
    {
        public GraphForm()
        {
            InitializeComponent();

            CityBuilder.Create();
            Game.CarsController = new CarsController();

            Application.Idle += Application_Idle;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            AutoScrollMinSize = new Size(5000, 5000);
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            Game.CarsController.Update();
            Invalidate();
        }

        Path<Node> AStarPath;

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(5 - HorizontalScroll.Value,  55 - VerticalScroll.Value);
            e.Graphics.ScaleTransform(5, 5);

            //draw grid
            //using (var pen = new Pen(Color.Silver, 0.1f))
            //    for (int x = 0; x < Game.GRID_SIZE; x++)
            //        e.Graphics.DrawLine(pen, x - 0.5f, -0.5f, x - 0.5f, Game.GRID_SIZE - 0.5f);
            //using (var pen = new Pen(Color.Silver, 0.1f))
            //    for (int y = 0; y < Game.GRID_SIZE; y++)
            //        e.Graphics.DrawLine(pen, 0 - 0.5f, y - 0.5f, Game.GRID_SIZE - 0.5f, y - 0.5f);

            //draw graph
            foreach (var n in Game.State.Nodes)
            {
                if (n.Name != null)
                {
                    e.Graphics.FillEllipse(Brushes.Blue, n.CellX - 0.5f, n.CellY - 0.5f, 1, 1);
                    if (n.RoadsOut.Count == 0)
                        Console.WriteLine(n);
                }

                using(var pen = new Pen(Color.Red, 0.1f))
                foreach (var road in n.RoadsOut)
                    e.Graphics.DrawLine(pen, road.From.CellX, road.From.CellY, road.To.CellX, road.To.CellY);
            }

            foreach (var car in Game.State.Cars)
            {
                e.Graphics.FillEllipse(Brushes.Black, car.Position.X - 0.3f, car.Position.Y - 0.3f, 0.6f, 0.6f);
            }

            if (AStarPath != null)
            foreach (var n in AStarPath)
            {
                e.Graphics.FillEllipse(Brushes.Green, n.CellX - 0.5f, n.CellY - 0.5f, 1, 1);
            }
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            Invalidate();
        }

        private void aStarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var nodes = Game.State.Nodes.Where(n => n.Name != null).ToArray();
            var rnd = new Random();
            var i1 = rnd.Next(nodes.Length);
            var i2 = rnd.Next(nodes.Length);
            var n1 = nodes[i1];
            var n2 = nodes[i2];
            AStarPath = AStar.FindPath(n1, n2, Distance, last=> Distance(last, n2));
            Invalidate();
        }

        double Distance(Node n1, Node n2)
        {
            var dx = Math.Abs(n1.CellX - n2.CellX);
            var dy = Math.Abs(n1.CellY - n2.CellY);
            return dx + dy;
        }

        private void stopOneCarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Game.State.Cars.GetRnd().Speed = 0.0001f;
        }
    }
}
