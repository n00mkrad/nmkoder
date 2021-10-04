
namespace Nmkoder.Forms
{
    partial class FileImportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileImportForm));
            this.fileList = new System.Windows.Forms.CheckedListBox();
            this.formatInfo = new System.Windows.Forms.Label();
            this.importAppendBtn = new System.Windows.Forms.Button();
            this.importClearBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // fileList
            // 
            this.fileList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.fileList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileList.ForeColor = System.Drawing.Color.White;
            this.fileList.FormattingEnabled = true;
            this.fileList.IntegralHeight = false;
            this.fileList.Location = new System.Drawing.Point(12, 36);
            this.fileList.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.fileList.Name = "fileList";
            this.fileList.ScrollAlwaysVisible = true;
            this.fileList.Size = new System.Drawing.Size(440, 162);
            this.fileList.TabIndex = 28;
            // 
            // formatInfo
            // 
            this.formatInfo.AutoSize = true;
            this.formatInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.formatInfo.ForeColor = System.Drawing.Color.White;
            this.formatInfo.Location = new System.Drawing.Point(13, 13);
            this.formatInfo.Margin = new System.Windows.Forms.Padding(4);
            this.formatInfo.Name = "formatInfo";
            this.formatInfo.Size = new System.Drawing.Size(155, 16);
            this.formatInfo.TabIndex = 29;
            this.formatInfo.Text = "Import All Checked Files:";
            // 
            // importAppendBtn
            // 
            this.importAppendBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.importAppendBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.importAppendBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.importAppendBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importAppendBtn.Location = new System.Drawing.Point(12, 241);
            this.importAppendBtn.Name = "importAppendBtn";
            this.importAppendBtn.Size = new System.Drawing.Size(440, 28);
            this.importAppendBtn.TabIndex = 57;
            this.importAppendBtn.Text = "Import (Add To Existing Files)";
            this.importAppendBtn.UseVisualStyleBackColor = false;
            this.importAppendBtn.Click += new System.EventHandler(this.importAppendBtn_Click);
            // 
            // importClearBtn
            // 
            this.importClearBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.importClearBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.importClearBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.importClearBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importClearBtn.Location = new System.Drawing.Point(12, 207);
            this.importClearBtn.Name = "importClearBtn";
            this.importClearBtn.Size = new System.Drawing.Size(440, 28);
            this.importClearBtn.TabIndex = 58;
            this.importClearBtn.Text = "Import (Clear Existing Files First)";
            this.importClearBtn.UseVisualStyleBackColor = false;
            this.importClearBtn.Click += new System.EventHandler(this.importClearBtn_Click);
            // 
            // FileImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(464, 281);
            this.Controls.Add(this.importClearBtn);
            this.Controls.Add(this.importAppendBtn);
            this.Controls.Add(this.formatInfo);
            this.Controls.Add(this.fileList);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FileImportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Files";
            this.Load += new System.EventHandler(this.FileImportForm_Load);
            this.Shown += new System.EventHandler(this.FileImportForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox fileList;
        private System.Windows.Forms.Label formatInfo;
        private System.Windows.Forms.Button importAppendBtn;
        private System.Windows.Forms.Button importClearBtn;
    }
}