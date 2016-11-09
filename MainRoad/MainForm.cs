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
            CityBuilder.Create();
            TileBuilder.Build();
        }
    }
}
