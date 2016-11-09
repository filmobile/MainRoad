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
            this.pnGame = new MainRoad.Controls.GamePanel();
            this.SuspendLayout();
            // 
            // pnGame
            // 
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
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.GamePanel pnGame;
    }
}

