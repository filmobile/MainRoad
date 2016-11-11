namespace WindowsFormsApplication354
{
    partial class Form1
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
            this.btGenerate = new System.Windows.Forms.Button();
            this.cbRotate = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btGenerate
            // 
            this.btGenerate.Location = new System.Drawing.Point(12, 12);
            this.btGenerate.Name = "btGenerate";
            this.btGenerate.Size = new System.Drawing.Size(75, 23);
            this.btGenerate.TabIndex = 0;
            this.btGenerate.Text = "Generate";
            this.btGenerate.UseVisualStyleBackColor = true;
            this.btGenerate.Click += new System.EventHandler(this.btGenerate_Click);
            // 
            // cbRotate
            // 
            this.cbRotate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRotate.FormattingEnabled = true;
            this.cbRotate.Items.AddRange(new object[] {
            "Shadow top",
            "Shadow right",
            "Shadow bottom",
            "Shadow left"});
            this.cbRotate.Location = new System.Drawing.Point(133, 14);
            this.cbRotate.Name = "cbRotate";
            this.cbRotate.Size = new System.Drawing.Size(121, 21);
            this.cbRotate.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(2000, 2000);
            this.BackgroundImage = global::WindowsFormsApplication354.Properties.Resources.Tiles;
            this.ClientSize = new System.Drawing.Size(839, 733);
            this.Controls.Add(this.cbRotate);
            this.Controls.Add(this.btGenerate);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btGenerate;
        private System.Windows.Forms.ComboBox cbRotate;

    }
}

