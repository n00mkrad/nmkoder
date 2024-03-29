﻿
namespace Nmkoder.Forms.Utils
{
    partial class UtilsMetricsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UtilsMetricsForm));
            this.confirmBtn = new System.Windows.Forms.Button();
            this.encodedVideo = new System.Windows.Forms.ComboBox();
            this.referenceVideo = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.vmaf = new System.Windows.Forms.CheckBox();
            this.ssim = new System.Windows.Forms.CheckBox();
            this.psnr = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.align = new System.Windows.Forms.ComboBox();
            this.vmafMdl = new System.Windows.Forms.ComboBox();
            this.subsample = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // confirmBtn
            // 
            this.confirmBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.confirmBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.confirmBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.confirmBtn.Location = new System.Drawing.Point(12, 222);
            this.confirmBtn.Name = "confirmBtn";
            this.confirmBtn.Size = new System.Drawing.Size(320, 23);
            this.confirmBtn.TabIndex = 57;
            this.confirmBtn.Text = "OK";
            this.confirmBtn.UseVisualStyleBackColor = false;
            this.confirmBtn.Click += new System.EventHandler(this.confirmBtn_Click);
            // 
            // encodedVideo
            // 
            this.encodedVideo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encodedVideo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encodedVideo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.encodedVideo.ForeColor = System.Drawing.Color.White;
            this.encodedVideo.FormattingEnabled = true;
            this.encodedVideo.Location = new System.Drawing.Point(110, 12);
            this.encodedVideo.Name = "encodedVideo";
            this.encodedVideo.Size = new System.Drawing.Size(222, 21);
            this.encodedVideo.TabIndex = 58;
            // 
            // referenceVideo
            // 
            this.referenceVideo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.referenceVideo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.referenceVideo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.referenceVideo.ForeColor = System.Drawing.Color.White;
            this.referenceVideo.FormattingEnabled = true;
            this.referenceVideo.Location = new System.Drawing.Point(110, 42);
            this.referenceVideo.Name = "referenceVideo";
            this.referenceVideo.Size = new System.Drawing.Size(222, 21);
            this.referenceVideo.TabIndex = 59;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.ForeColor = System.Drawing.Color.White;
            this.label23.Location = new System.Drawing.Point(13, 15);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(83, 13);
            this.label23.TabIndex = 60;
            this.label23.Text = "Encoded Video:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(13, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 61;
            this.label1.Text = "Reference Video:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(13, 135);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "Calculate VMAF:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(13, 165);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 63;
            this.label3.Text = "Calculate SSIM:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(13, 195);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 64;
            this.label4.Text = "Calculate PSNR:";
            // 
            // vmaf
            // 
            this.vmaf.AutoSize = true;
            this.vmaf.ForeColor = System.Drawing.Color.White;
            this.vmaf.Location = new System.Drawing.Point(110, 134);
            this.vmaf.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.vmaf.Name = "vmaf";
            this.vmaf.Size = new System.Drawing.Size(15, 14);
            this.vmaf.TabIndex = 73;
            this.vmaf.UseVisualStyleBackColor = true;
            // 
            // ssim
            // 
            this.ssim.AutoSize = true;
            this.ssim.ForeColor = System.Drawing.Color.White;
            this.ssim.Location = new System.Drawing.Point(110, 165);
            this.ssim.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.ssim.Name = "ssim";
            this.ssim.Size = new System.Drawing.Size(15, 14);
            this.ssim.TabIndex = 74;
            this.ssim.UseVisualStyleBackColor = true;
            // 
            // psnr
            // 
            this.psnr.AutoSize = true;
            this.psnr.ForeColor = System.Drawing.Color.White;
            this.psnr.Location = new System.Drawing.Point(110, 195);
            this.psnr.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.psnr.Name = "psnr";
            this.psnr.Size = new System.Drawing.Size(15, 14);
            this.psnr.TabIndex = 75;
            this.psnr.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(13, 105);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 76;
            this.label5.Text = "Align:";
            // 
            // align
            // 
            this.align.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.align.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.align.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.align.ForeColor = System.Drawing.Color.White;
            this.align.FormattingEnabled = true;
            this.align.Items.AddRange(new object[] {
            "Disabled (Resolutions must match!)",
            "Auto-Crop",
            "Resize To Match",
            "Auto-Crop Then Resize To Match"});
            this.align.Location = new System.Drawing.Point(110, 102);
            this.align.Name = "align";
            this.align.Size = new System.Drawing.Size(222, 21);
            this.align.TabIndex = 77;
            // 
            // vmafMdl
            // 
            this.vmafMdl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.vmafMdl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.vmafMdl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.vmafMdl.ForeColor = System.Drawing.Color.White;
            this.vmafMdl.FormattingEnabled = true;
            this.vmafMdl.Items.AddRange(new object[] {
            "VMAF v0.6.1 (1080p)",
            "VMAF v0.6.1 NEG (1080p)",
            "VMAF v0.6.1 (4K)"});
            this.vmafMdl.Location = new System.Drawing.Point(131, 131);
            this.vmafMdl.Name = "vmafMdl";
            this.vmafMdl.Size = new System.Drawing.Size(201, 21);
            this.vmafMdl.TabIndex = 78;
            // 
            // subsample
            // 
            this.subsample.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.subsample.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.subsample.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.subsample.ForeColor = System.Drawing.Color.White;
            this.subsample.FormattingEnabled = true;
            this.subsample.Items.AddRange(new object[] {
            "Check Every Frame",
            "Check Every 2nd Frame",
            "Check Every 3rd Frame",
            "Check Every 4th Frame"});
            this.subsample.Location = new System.Drawing.Point(110, 72);
            this.subsample.Name = "subsample";
            this.subsample.Size = new System.Drawing.Size(222, 21);
            this.subsample.TabIndex = 79;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(13, 75);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 80;
            this.label6.Text = "Accuracy";
            // 
            // UtilsMetricsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(344, 257);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.subsample);
            this.Controls.Add(this.vmafMdl);
            this.Controls.Add(this.align);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.psnr);
            this.Controls.Add(this.ssim);
            this.Controls.Add(this.vmaf);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.referenceVideo);
            this.Controls.Add(this.encodedVideo);
            this.Controls.Add(this.confirmBtn);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UtilsMetricsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Metrics";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UtilsMetricsForm_FormClosing);
            this.Load += new System.EventHandler(this.UtilsMetricsForm_Load);
            this.Shown += new System.EventHandler(this.UtilsMetricsForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button confirmBtn;
        private System.Windows.Forms.ComboBox encodedVideo;
        private System.Windows.Forms.ComboBox referenceVideo;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox vmaf;
        private System.Windows.Forms.CheckBox ssim;
        private System.Windows.Forms.CheckBox psnr;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox align;
        private System.Windows.Forms.ComboBox vmafMdl;
        private System.Windows.Forms.ComboBox subsample;
        private System.Windows.Forms.Label label6;
    }
}