using MainRoadModel;
using MainRoadModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainRoad
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Game.State = new GameState();
            Game.CarsController = new CarsController();
            CityBuilder.Create();
            TileBuilder.Build();
        }

        int drawCounter = 0;
        DateTime lastFPStime;

        private void tmUpdate_Tick(object sender, EventArgs e)
        {
            Game.CarsController.Update();
            pnGame.Invalidate();
            drawCounter++;
        }

        private void tmFPS_Tick(object sender, EventArgs e)
        {
            var dt = DateTime.Now - lastFPStime;
            Text = string.Format("Draw: {0:00.0} fps", drawCounter / dt.TotalSeconds);
            drawCounter = 0;
            lastFPStime = DateTime.Now;
        }
    }
}
