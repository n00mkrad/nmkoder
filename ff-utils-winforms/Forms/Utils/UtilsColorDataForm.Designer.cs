
namespace Nmkoder.Forms.Utils
{
    partial class UtilsColorDataForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UtilsColorDataForm));
            this.confirmBtn = new System.Windows.Forms.Button();
            this.sourceVideo = new System.Windows.Forms.ComboBox();
            this.targetVideo = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.copyColorSpace = new System.Windows.Forms.CheckBox();
            this.copyHdrData = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // confirmBtn
            // 
            this.confirmBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.confirmBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.confirmBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.confirmBtn.Location = new System.Drawing.Point(12, 132);
            this.confirmBtn.Name = "confirmBtn";
            this.confirmBtn.Size = new System.Drawing.Size(360, 23);
            this.confirmBtn.TabIndex = 57;
            this.confirmBtn.Text = "OK";
            this.confirmBtn.UseVisualStyleBackColor = false;
            this.confirmBtn.Click += new System.EventHandler(this.confirmBtn_Click);
            // 
            // encodedVideo
            // 
            this.sourceVideo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.sourceVideo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceVideo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sourceVideo.ForeColor = System.Drawing.Color.White;
            this.sourceVideo.FormattingEnabled = true;
            this.sourceVideo.Location = new System.Drawing.Point(150, 12);
            this.sourceVideo.Name = "encodedVideo";
            this.sourceVideo.Size = new System.Drawing.Size(222, 21);
            this.sourceVideo.TabIndex = 58;
            // 
            // referenceVideo
            // 
            this.targetVideo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.targetVideo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.targetVideo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.targetVideo.ForeColor = System.Drawing.Color.White;
            this.targetVideo.FormattingEnabled = true;
            this.targetVideo.Location = new System.Drawing.Point(150, 42);
            this.targetVideo.Name = "referenceVideo";
            this.targetVideo.Size = new System.Drawing.Size(222, 21);
            this.targetVideo.TabIndex = 59;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.ForeColor = System.Drawing.Color.White;
            this.label23.Location = new System.Drawing.Point(13, 15);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(63, 13);
            this.label23.TabIndex = 60;
            this.label23.Text = "From Video:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(13, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 61;
            this.label1.Text = "To Video:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(13, 75);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "Transfer Color Space";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(13, 105);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 63;
            this.label3.Text = "Transfer HDR Data";
            // 
            // vmaf
            // 
            this.copyColorSpace.AutoSize = true;
            this.copyColorSpace.ForeColor = System.Drawing.Color.White;
            this.copyColorSpace.Location = new System.Drawing.Point(150, 74);
            this.copyColorSpace.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.copyColorSpace.Name = "vmaf";
            this.copyColorSpace.Size = new System.Drawing.Size(15, 14);
            this.copyColorSpace.TabIndex = 73;
            this.copyColorSpace.UseVisualStyleBackColor = true;
            // 
            // ssim
            // 
            this.copyHdrData.AutoSize = true;
            this.copyHdrData.ForeColor = System.Drawing.Color.White;
            this.copyHdrData.Location = new System.Drawing.Point(150, 104);
            this.copyHdrData.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.copyHdrData.Name = "ssim";
            this.copyHdrData.Size = new System.Drawing.Size(15, 14);
            this.copyHdrData.TabIndex = 74;
            this.copyHdrData.UseVisualStyleBackColor = true;
            // 
            // UtilsColorDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(384, 167);
            this.Controls.Add(this.copyHdrData);
            this.Controls.Add(this.copyColorSpace);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.targetVideo);
            this.Controls.Add(this.sourceVideo);
            this.Controls.Add(this.confirmBtn);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UtilsColorDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transfer Color Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UtilsColorDataForm_FormClosing);
            this.Load += new System.EventHandler(this.UtilsColorDataForm_Load);
            this.Shown += new System.EventHandler(this.UtilsColorDataForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button confirmBtn;
        private System.Windows.Forms.ComboBox sourceVideo;
        private System.Windows.Forms.ComboBox targetVideo;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox copyColorSpace;
        private System.Windows.Forms.CheckBox copyHdrData;
    }
}