
namespace Nmkoder.Forms
{
    partial class TrimForm
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
            this.confirmBtn = new System.Windows.Forms.Button();
            this.resetBtn = new System.Windows.Forms.Button();
            this.trimMode = new System.Windows.Forms.ComboBox();
            this.label61 = new System.Windows.Forms.Label();
            this.startBox = new System.Windows.Forms.TextBox();
            this.durationBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.endBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
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
            this.confirmBtn.Size = new System.Drawing.Size(320, 23);
            this.confirmBtn.TabIndex = 5;
            this.confirmBtn.Text = "OK";
            this.confirmBtn.UseVisualStyleBackColor = false;
            this.confirmBtn.Click += new System.EventHandler(this.confirmBtn_Click);
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
            this.resetBtn.Size = new System.Drawing.Size(320, 23);
            this.resetBtn.TabIndex = 4;
            this.resetBtn.Text = "Reset";
            this.resetBtn.UseVisualStyleBackColor = false;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
            // 
            // trimMode
            // 
            this.trimMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.trimMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.trimMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.trimMode.ForeColor = System.Drawing.Color.White;
            this.trimMode.FormattingEnabled = true;
            this.trimMode.Items.AddRange(new object[] {
            "Trim Using Keyframe Time (Fast)",
            "Trim Using Exact Time (Slow)",
            "Trim Using Frame Numbers (Slow)"});
            this.trimMode.Location = new System.Drawing.Point(12, 13);
            this.trimMode.Name = "trimMode";
            this.trimMode.Size = new System.Drawing.Size(320, 21);
            this.trimMode.TabIndex = 0;
            this.trimMode.SelectedIndexChanged += new System.EventHandler(this.trimMode_SelectedIndexChanged);
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.ForeColor = System.Drawing.Color.White;
            this.label61.Location = new System.Drawing.Point(13, 50);
            this.label61.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(55, 13);
            this.label61.TabIndex = 84;
            this.label61.Text = "Start Time";
            // 
            // startBox
            // 
            this.startBox.AllowDrop = true;
            this.startBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.startBox.ForeColor = System.Drawing.Color.White;
            this.startBox.Location = new System.Drawing.Point(192, 47);
            this.startBox.MinimumSize = new System.Drawing.Size(4, 21);
            this.startBox.Name = "startBox";
            this.startBox.Size = new System.Drawing.Size(140, 20);
            this.startBox.TabIndex = 1;
            this.startBox.TextChanged += new System.EventHandler(this.startBox_TextChanged);
            // 
            // durationBox
            // 
            this.durationBox.AllowDrop = true;
            this.durationBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.durationBox.ForeColor = System.Drawing.Color.Silver;
            this.durationBox.Location = new System.Drawing.Point(192, 103);
            this.durationBox.MinimumSize = new System.Drawing.Size(4, 21);
            this.durationBox.Name = "durationBox";
            this.durationBox.ReadOnly = true;
            this.durationBox.Size = new System.Drawing.Size(140, 20);
            this.durationBox.TabIndex = 88;
            this.durationBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(13, 106);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 87;
            this.label1.Text = "Duration";
            // 
            // endBox
            // 
            this.endBox.AllowDrop = true;
            this.endBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.endBox.ForeColor = System.Drawing.Color.White;
            this.endBox.Location = new System.Drawing.Point(192, 75);
            this.endBox.MinimumSize = new System.Drawing.Size(4, 21);
            this.endBox.Name = "endBox";
            this.endBox.Size = new System.Drawing.Size(140, 20);
            this.endBox.TabIndex = 2;
            this.endBox.TextChanged += new System.EventHandler(this.endBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(13, 78);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 89;
            this.label2.Text = "End Time";
            // 
            // TrimForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(344, 198);
            this.Controls.Add(this.endBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.durationBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.startBox);
            this.Controls.Add(this.label61);
            this.Controls.Add(this.trimMode);
            this.Controls.Add(this.resetBtn);
            this.Controls.Add(this.confirmBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TrimForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trim";
            this.Load += new System.EventHandler(this.TrimForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button confirmBtn;
        private System.Windows.Forms.Button resetBtn;
        private System.Windows.Forms.ComboBox trimMode;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.TextBox startBox;
        private System.Windows.Forms.TextBox durationBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox endBox;
        private System.Windows.Forms.Label label2;
    }
}