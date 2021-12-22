
namespace Nmkoder.Forms
{
    partial class CropForm
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
            this.label61 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.confirmBtn = new System.Windows.Forms.Button();
            this.cropTop = new System.Windows.Forms.NumericUpDown();
            this.cropBot = new System.Windows.Forms.NumericUpDown();
            this.cropRight = new System.Windows.Forms.NumericUpDown();
            this.cropLeft = new System.Windows.Forms.NumericUpDown();
            this.cropAreaH = new System.Windows.Forms.NumericUpDown();
            this.cropAreaW = new System.Windows.Forms.NumericUpDown();
            this.cropAreaY = new System.Windows.Forms.NumericUpDown();
            this.cropAreaX = new System.Windows.Forms.NumericUpDown();
            this.resetBtn = new System.Windows.Forms.Button();
            this.noVidPanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cropTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropBot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropAreaH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropAreaW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropAreaY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropAreaX)).BeginInit();
            this.noVidPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.ForeColor = System.Drawing.Color.White;
            this.label61.Location = new System.Drawing.Point(13, 15);
            this.label61.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(89, 13);
            this.label61.TabIndex = 61;
            this.label61.Text = "Crop Top/Bottom";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(13, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 64;
            this.label1.Text = "Crop Left/Right";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(13, 75);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(206, 13);
            this.label2.TabIndex = 67;
            this.label2.Text = "Resulting Area Dimensions (Width/Height)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(13, 105);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(218, 13);
            this.label3.TabIndex = 70;
            this.label3.Text = "Resulting Area Coordinates (Top Left Corner)";
            // 
            // confirmBtn
            // 
            this.confirmBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.confirmBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.confirmBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.confirmBtn.ForeColor = System.Drawing.Color.White;
            this.confirmBtn.Location = new System.Drawing.Point(12, 163);
            this.confirmBtn.Name = "confirmBtn";
            this.confirmBtn.Size = new System.Drawing.Size(462, 23);
            this.confirmBtn.TabIndex = 73;
            this.confirmBtn.Text = "OK";
            this.confirmBtn.UseVisualStyleBackColor = false;
            this.confirmBtn.Click += new System.EventHandler(this.confirmBtn_Click);
            // 
            // cropTop
            // 
            this.cropTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cropTop.ForeColor = System.Drawing.Color.White;
            this.cropTop.Location = new System.Drawing.Point(250, 13);
            this.cropTop.Name = "cropTop";
            this.cropTop.Size = new System.Drawing.Size(110, 20);
            this.cropTop.TabIndex = 74;
            this.cropTop.ValueChanged += new System.EventHandler(this.CropBotTopChanged);
            // 
            // cropBot
            // 
            this.cropBot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cropBot.ForeColor = System.Drawing.Color.White;
            this.cropBot.Location = new System.Drawing.Point(366, 13);
            this.cropBot.Name = "cropBot";
            this.cropBot.Size = new System.Drawing.Size(110, 20);
            this.cropBot.TabIndex = 75;
            this.cropBot.ValueChanged += new System.EventHandler(this.CropBotTopChanged);
            // 
            // cropRight
            // 
            this.cropRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cropRight.ForeColor = System.Drawing.Color.White;
            this.cropRight.Location = new System.Drawing.Point(366, 43);
            this.cropRight.Name = "cropRight";
            this.cropRight.Size = new System.Drawing.Size(110, 20);
            this.cropRight.TabIndex = 77;
            this.cropRight.ValueChanged += new System.EventHandler(this.CropLeftRightChanged);
            // 
            // cropLeft
            // 
            this.cropLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cropLeft.ForeColor = System.Drawing.Color.White;
            this.cropLeft.Location = new System.Drawing.Point(250, 43);
            this.cropLeft.Name = "cropLeft";
            this.cropLeft.Size = new System.Drawing.Size(110, 20);
            this.cropLeft.TabIndex = 76;
            this.cropLeft.ValueChanged += new System.EventHandler(this.CropLeftRightChanged);
            // 
            // cropAreaH
            // 
            this.cropAreaH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.cropAreaH.Enabled = false;
            this.cropAreaH.ForeColor = System.Drawing.Color.White;
            this.cropAreaH.Location = new System.Drawing.Point(366, 73);
            this.cropAreaH.Name = "cropAreaH";
            this.cropAreaH.Size = new System.Drawing.Size(110, 20);
            this.cropAreaH.TabIndex = 79;
            // 
            // cropAreaW
            // 
            this.cropAreaW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.cropAreaW.Enabled = false;
            this.cropAreaW.ForeColor = System.Drawing.Color.White;
            this.cropAreaW.Location = new System.Drawing.Point(250, 73);
            this.cropAreaW.Name = "cropAreaW";
            this.cropAreaW.Size = new System.Drawing.Size(110, 20);
            this.cropAreaW.TabIndex = 78;
            // 
            // cropAreaY
            // 
            this.cropAreaY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.cropAreaY.Enabled = false;
            this.cropAreaY.ForeColor = System.Drawing.Color.White;
            this.cropAreaY.Location = new System.Drawing.Point(366, 103);
            this.cropAreaY.Name = "cropAreaY";
            this.cropAreaY.Size = new System.Drawing.Size(110, 20);
            this.cropAreaY.TabIndex = 81;
            // 
            // cropAreaX
            // 
            this.cropAreaX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.cropAreaX.Enabled = false;
            this.cropAreaX.ForeColor = System.Drawing.Color.White;
            this.cropAreaX.Location = new System.Drawing.Point(250, 103);
            this.cropAreaX.Name = "cropAreaX";
            this.cropAreaX.Size = new System.Drawing.Size(110, 20);
            this.cropAreaX.TabIndex = 80;
            // 
            // resetBtn
            // 
            this.resetBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resetBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.resetBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.resetBtn.ForeColor = System.Drawing.Color.White;
            this.resetBtn.Location = new System.Drawing.Point(12, 134);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(462, 23);
            this.resetBtn.TabIndex = 82;
            this.resetBtn.Text = "Reset";
            this.resetBtn.UseVisualStyleBackColor = false;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
            // 
            // noVidPanel
            // 
            this.noVidPanel.Controls.Add(this.label4);
            this.noVidPanel.Location = new System.Drawing.Point(250, 73);
            this.noVidPanel.Name = "noVidPanel";
            this.noVidPanel.Size = new System.Drawing.Size(226, 50);
            this.noVidPanel.TabIndex = 83;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(226, 50);
            this.label4.TabIndex = 62;
            this.label4.Text = "Not available without a loaded video.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CropForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(486, 198);
            this.Controls.Add(this.noVidPanel);
            this.Controls.Add(this.resetBtn);
            this.Controls.Add(this.cropAreaY);
            this.Controls.Add(this.cropAreaX);
            this.Controls.Add(this.cropAreaH);
            this.Controls.Add(this.cropAreaW);
            this.Controls.Add(this.cropRight);
            this.Controls.Add(this.cropLeft);
            this.Controls.Add(this.cropBot);
            this.Controls.Add(this.cropTop);
            this.Controls.Add(this.confirmBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label61);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CropForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CropForm";
            this.Load += new System.EventHandler(this.CropForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cropTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropBot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropAreaH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropAreaW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropAreaY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropAreaX)).EndInit();
            this.noVidPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button confirmBtn;
        private System.Windows.Forms.NumericUpDown cropTop;
        private System.Windows.Forms.NumericUpDown cropBot;
        private System.Windows.Forms.NumericUpDown cropRight;
        private System.Windows.Forms.NumericUpDown cropLeft;
        private System.Windows.Forms.NumericUpDown cropAreaH;
        private System.Windows.Forms.NumericUpDown cropAreaW;
        private System.Windows.Forms.NumericUpDown cropAreaY;
        private System.Windows.Forms.NumericUpDown cropAreaX;
        private System.Windows.Forms.Button resetBtn;
        private System.Windows.Forms.Panel noVidPanel;
        private System.Windows.Forms.Label label4;
    }
}