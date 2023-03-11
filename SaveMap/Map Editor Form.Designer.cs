namespace SaveMap
{
    partial class SaveMapForm
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
            this.SaveMapButton = new System.Windows.Forms.Button();
            this.ForestPath = new System.Windows.Forms.RadioButton();
            this.SnowPath = new System.Windows.Forms.RadioButton();
            this.MountainPathUpper = new System.Windows.Forms.RadioButton();
            this.MountainPathLower = new System.Windows.Forms.RadioButton();
            this.ShrubPathRight = new System.Windows.Forms.RadioButton();
            this.ShrubPathLeft = new System.Windows.Forms.RadioButton();
            this.LoadButton = new System.Windows.Forms.Button();
            this.ForestMapButton = new System.Windows.Forms.RadioButton();
            this.SnowMapButton = new System.Windows.Forms.RadioButton();
            this.MountainMapButton = new System.Windows.Forms.RadioButton();
            this.ShrubMapButton = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // SaveMapButton
            // 
            this.SaveMapButton.Location = new System.Drawing.Point(138, 231);
            this.SaveMapButton.Name = "SaveMapButton";
            this.SaveMapButton.Size = new System.Drawing.Size(75, 23);
            this.SaveMapButton.TabIndex = 0;
            this.SaveMapButton.Text = "Save";
            this.SaveMapButton.UseVisualStyleBackColor = true;
            this.SaveMapButton.Click += new System.EventHandler(this.SaveMapButton_Click);
            // 
            // ForestPath
            // 
            this.ForestPath.AutoSize = true;
            this.ForestPath.Location = new System.Drawing.Point(12, 8);
            this.ForestPath.Name = "ForestPath";
            this.ForestPath.Size = new System.Drawing.Size(76, 17);
            this.ForestPath.TabIndex = 1;
            this.ForestPath.TabStop = true;
            this.ForestPath.Text = "ForestPath";
            this.ForestPath.UseVisualStyleBackColor = true;
            this.ForestPath.Click += new System.EventHandler(this.ForestPath_Click);
            // 
            // SnowPath
            // 
            this.SnowPath.AutoSize = true;
            this.SnowPath.Location = new System.Drawing.Point(12, 31);
            this.SnowPath.Name = "SnowPath";
            this.SnowPath.Size = new System.Drawing.Size(74, 17);
            this.SnowPath.TabIndex = 2;
            this.SnowPath.TabStop = true;
            this.SnowPath.Text = "SnowPath";
            this.SnowPath.UseVisualStyleBackColor = true;
            this.SnowPath.Click += new System.EventHandler(this.SnowPath_Click);
            // 
            // MountainPathUpper
            // 
            this.MountainPathUpper.AutoSize = true;
            this.MountainPathUpper.Location = new System.Drawing.Point(12, 54);
            this.MountainPathUpper.Name = "MountainPathUpper";
            this.MountainPathUpper.Size = new System.Drawing.Size(120, 17);
            this.MountainPathUpper.TabIndex = 3;
            this.MountainPathUpper.TabStop = true;
            this.MountainPathUpper.Text = "MountainPathUpper";
            this.MountainPathUpper.UseVisualStyleBackColor = true;
            this.MountainPathUpper.Click += new System.EventHandler(this.MountainPathUpper_Click);
            // 
            // MountainPathLower
            // 
            this.MountainPathLower.AutoSize = true;
            this.MountainPathLower.Location = new System.Drawing.Point(12, 77);
            this.MountainPathLower.Name = "MountainPathLower";
            this.MountainPathLower.Size = new System.Drawing.Size(120, 17);
            this.MountainPathLower.TabIndex = 4;
            this.MountainPathLower.TabStop = true;
            this.MountainPathLower.Text = "MountainPathLower";
            this.MountainPathLower.UseVisualStyleBackColor = true;
            this.MountainPathLower.Click += new System.EventHandler(this.MountainPathLower_Click);
            // 
            // ShrubPathRight
            // 
            this.ShrubPathRight.AutoSize = true;
            this.ShrubPathRight.Location = new System.Drawing.Point(12, 100);
            this.ShrubPathRight.Name = "ShrubPathRight";
            this.ShrubPathRight.Size = new System.Drawing.Size(100, 17);
            this.ShrubPathRight.TabIndex = 5;
            this.ShrubPathRight.TabStop = true;
            this.ShrubPathRight.Text = "ShrubPathRight";
            this.ShrubPathRight.UseVisualStyleBackColor = true;
            this.ShrubPathRight.Click += new System.EventHandler(this.ShrubPathRight_Click);
            // 
            // ShrubPathLeft
            // 
            this.ShrubPathLeft.AutoSize = true;
            this.ShrubPathLeft.Location = new System.Drawing.Point(12, 123);
            this.ShrubPathLeft.Name = "ShrubPathLeft";
            this.ShrubPathLeft.Size = new System.Drawing.Size(93, 17);
            this.ShrubPathLeft.TabIndex = 6;
            this.ShrubPathLeft.TabStop = true;
            this.ShrubPathLeft.Text = "ShrubPathLeft";
            this.ShrubPathLeft.UseVisualStyleBackColor = true;
            this.ShrubPathLeft.Click += new System.EventHandler(this.ShrubPathLeft_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(12, 231);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(75, 23);
            this.LoadButton.TabIndex = 7;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // ForestMapButton
            // 
            this.ForestMapButton.AutoSize = true;
            this.ForestMapButton.Location = new System.Drawing.Point(134, 8);
            this.ForestMapButton.Name = "ForestMapButton";
            this.ForestMapButton.Size = new System.Drawing.Size(75, 17);
            this.ForestMapButton.TabIndex = 8;
            this.ForestMapButton.TabStop = true;
            this.ForestMapButton.Text = "ForestMap";
            this.ForestMapButton.UseVisualStyleBackColor = true;
            this.ForestMapButton.CheckedChanged += new System.EventHandler(this.ForestMapButton_CheckedChanged);
            // 
            // SnowMapButton
            // 
            this.SnowMapButton.AutoSize = true;
            this.SnowMapButton.Location = new System.Drawing.Point(134, 31);
            this.SnowMapButton.Name = "SnowMapButton";
            this.SnowMapButton.Size = new System.Drawing.Size(73, 17);
            this.SnowMapButton.TabIndex = 9;
            this.SnowMapButton.TabStop = true;
            this.SnowMapButton.Text = "SnowMap";
            this.SnowMapButton.UseVisualStyleBackColor = true;
            this.SnowMapButton.CheckedChanged += new System.EventHandler(this.SnowMapButton_CheckedChanged);
            // 
            // MountainMapButton
            // 
            this.MountainMapButton.AutoSize = true;
            this.MountainMapButton.Location = new System.Drawing.Point(134, 77);
            this.MountainMapButton.Name = "MountainMapButton";
            this.MountainMapButton.Size = new System.Drawing.Size(90, 17);
            this.MountainMapButton.TabIndex = 10;
            this.MountainMapButton.TabStop = true;
            this.MountainMapButton.Text = "MountainMap";
            this.MountainMapButton.UseVisualStyleBackColor = true;
            this.MountainMapButton.CheckedChanged += new System.EventHandler(this.MountainMapButton_CheckedChanged);
            // 
            // ShrubMapButton
            // 
            this.ShrubMapButton.AutoSize = true;
            this.ShrubMapButton.Location = new System.Drawing.Point(134, 123);
            this.ShrubMapButton.Name = "ShrubMapButton";
            this.ShrubMapButton.Size = new System.Drawing.Size(74, 17);
            this.ShrubMapButton.TabIndex = 11;
            this.ShrubMapButton.TabStop = true;
            this.ShrubMapButton.Text = "ShrubMap";
            this.ShrubMapButton.UseVisualStyleBackColor = true;
            this.ShrubMapButton.CheckedChanged += new System.EventHandler(this.ShrubMapButton_CheckedChanged);
            // 
            // SaveMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(225, 268);
            this.Controls.Add(this.ShrubMapButton);
            this.Controls.Add(this.MountainMapButton);
            this.Controls.Add(this.SnowMapButton);
            this.Controls.Add(this.ForestMapButton);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.ShrubPathLeft);
            this.Controls.Add(this.ShrubPathRight);
            this.Controls.Add(this.MountainPathLower);
            this.Controls.Add(this.MountainPathUpper);
            this.Controls.Add(this.SnowPath);
            this.Controls.Add(this.ForestPath);
            this.Controls.Add(this.SaveMapButton);
            this.Name = "SaveMapForm";
            this.RightToLeftLayout = true;
            this.Text = "Save Map";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SaveMapButton;
        private System.Windows.Forms.RadioButton ForestPath;
        private System.Windows.Forms.RadioButton SnowPath;
        private System.Windows.Forms.RadioButton MountainPathUpper;
        private System.Windows.Forms.RadioButton MountainPathLower;
        private System.Windows.Forms.RadioButton ShrubPathRight;
        private System.Windows.Forms.RadioButton ShrubPathLeft;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.RadioButton ForestMapButton;
        private System.Windows.Forms.RadioButton SnowMapButton;
        private System.Windows.Forms.RadioButton MountainMapButton;
        private System.Windows.Forms.RadioButton ShrubMapButton;
    }
}

