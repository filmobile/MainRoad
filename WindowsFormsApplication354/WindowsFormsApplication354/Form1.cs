using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication354
{
    public partial class Form1 : Form
    {
        private Map map;
        private Bitmap result;

        public Form1()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            cbRotate.SelectedIndex = 0;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (result == null)
                return;

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            e.Graphics.TranslateTransform(10 - HorizontalScroll.Value, 10 - VerticalScroll.Value);
            e.Graphics.DrawImage(result, Point.Empty);
        }

        private void btGenerate_Click(object sender, EventArgs e)
        {
            map = new Map(18, 20);
            Tool.InstanceMap = map;
            TopologyHelper.IsFilled = (p) => map.IsFilled(p);
            var builder = new Builder();
            switch(cbRotate.SelectedIndex)
            {
                case 0:
                    builder.ShadowDirectionX = -0.5f;
                    builder.ShadowDirectionY = -0.9f;
                    break;
                case 1:
                    builder.ShadowDirectionX = 0.9f;
                    builder.ShadowDirectionY = -0.5f;
                    break;
                case 2:
                    builder.ShadowDirectionX = 0.5f;
                    builder.ShadowDirectionY = 0.9f;
                    break;
                case 3:
                    builder.ShadowDirectionX = -0.9f;
                    builder.ShadowDirectionY = 0.5f;
                    break;
            }
            
            builder.Build(map);
            result = builder.Result;
            Invalidate();
        }
    }
}
