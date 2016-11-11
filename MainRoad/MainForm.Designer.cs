namespace MainRoad
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tmUpdate = new System.Windows.Forms.Timer(this.components);
            this.tmFPS = new System.Windows.Forms.Timer(this.components);
            this.pnGame = new MainRoad.Controls.GamePanel();
            this.SuspendLayout();
            // 
            // tmUpdate
            // 
            this.tmUpdate.Enabled = true;
            this.tmUpdate.Interval = 20;
            this.tmUpdate.Tick += new System.EventHandler(this.tmUpdate_Tick);
            // 
            // tmFPS
            // 
            this.tmFPS.Enabled = true;
            this.tmFPS.Interval = 1000;
            this.tmFPS.Tick += new System.EventHandler(this.tmFPS_Tick);
            // 
            // pnGame
            // 
            this.pnGame.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnGame.BackgroundImage")));
            this.pnGame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnGame.Location = new System.Drawing.Point(0, 0);
            this.pnGame.Name = "pnGame";
            this.pnGame.Size = new System.Drawing.Size(551, 303);
            this.pnGame.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 303);
            this.Controls.Add(this.pnGame);
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.GamePanel pnGame;
        private System.Windows.Forms.Timer tmUpdate;
        private System.Windows.Forms.Timer tmFPS;
    }
}

