namespace ModelTester
{
    partial class GraphForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.aStarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopOneCarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aStarToolStripMenuItem,
            this.stopOneCarToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(463, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // aStarToolStripMenuItem
            // 
            this.aStarToolStripMenuItem.Name = "aStarToolStripMenuItem";
            this.aStarToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.aStarToolStripMenuItem.Text = "AStar";
            this.aStarToolStripMenuItem.Click += new System.EventHandler(this.aStarToolStripMenuItem_Click);
            // 
            // stopOneCarToolStripMenuItem
            // 
            this.stopOneCarToolStripMenuItem.Name = "stopOneCarToolStripMenuItem";
            this.stopOneCarToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
            this.stopOneCarToolStripMenuItem.Text = "Stop one car";
            this.stopOneCarToolStripMenuItem.Click += new System.EventHandler(this.stopOneCarToolStripMenuItem_Click);
            // 
            // GraphForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 369);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "GraphForm";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aStarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopOneCarToolStripMenuItem;
    }
}

