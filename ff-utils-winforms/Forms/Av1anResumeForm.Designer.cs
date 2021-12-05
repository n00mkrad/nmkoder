
namespace Nmkoder.Forms
{
    partial class Av1anResumeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Av1anResumeForm));
            this.folderList = new System.Windows.Forms.ListBox();
            this.resumeWithSavedSettings = new System.Windows.Forms.Button();
            this.resumeWithNewSettings = new System.Windows.Forms.Button();
            this.DeleteFull = new System.Windows.Forms.Button();
            this.DeleteChunks = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // folderList
            // 
            this.folderList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.folderList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.folderList.ForeColor = System.Drawing.Color.White;
            this.folderList.FormattingEnabled = true;
            this.folderList.IntegralHeight = false;
            this.folderList.ItemHeight = 16;
            this.folderList.Location = new System.Drawing.Point(12, 12);
            this.folderList.Name = "folderList";
            this.folderList.Size = new System.Drawing.Size(674, 178);
            this.folderList.TabIndex = 57;
            // 
            // resumeWithSavedSettings
            // 
            this.resumeWithSavedSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resumeWithSavedSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.resumeWithSavedSettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.resumeWithSavedSettings.Location = new System.Drawing.Point(692, 12);
            this.resumeWithSavedSettings.Name = "resumeWithSavedSettings";
            this.resumeWithSavedSettings.Size = new System.Drawing.Size(180, 40);
            this.resumeWithSavedSettings.TabIndex = 58;
            this.resumeWithSavedSettings.Text = "Resume\r\n(Use Saved Original Settings)";
            this.resumeWithSavedSettings.UseVisualStyleBackColor = false;
            this.resumeWithSavedSettings.Click += new System.EventHandler(this.resumeWithSavedSettings_Click);
            // 
            // resumeWithNewSettings
            // 
            this.resumeWithNewSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resumeWithNewSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.resumeWithNewSettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.resumeWithNewSettings.Location = new System.Drawing.Point(692, 58);
            this.resumeWithNewSettings.Name = "resumeWithNewSettings";
            this.resumeWithNewSettings.Size = new System.Drawing.Size(180, 40);
            this.resumeWithNewSettings.TabIndex = 59;
            this.resumeWithNewSettings.Text = "Resume\r\n(Use Current GUI Settings)";
            this.resumeWithNewSettings.UseVisualStyleBackColor = false;
            this.resumeWithNewSettings.Click += new System.EventHandler(this.resumeWithNewSettings_Click);
            // 
            // DeleteFull
            // 
            this.DeleteFull.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteFull.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.DeleteFull.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.DeleteFull.Location = new System.Drawing.Point(692, 104);
            this.DeleteFull.Name = "DeleteFull";
            this.DeleteFull.Size = new System.Drawing.Size(180, 40);
            this.DeleteFull.TabIndex = 60;
            this.DeleteFull.Text = "Delete\r\n(Entire Folder)";
            this.DeleteFull.UseVisualStyleBackColor = false;
            this.DeleteFull.Click += new System.EventHandler(this.DeleteFull_Click);
            // 
            // DeleteChunks
            // 
            this.DeleteChunks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteChunks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.DeleteChunks.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.DeleteChunks.Location = new System.Drawing.Point(692, 150);
            this.DeleteChunks.Name = "DeleteChunks";
            this.DeleteChunks.Size = new System.Drawing.Size(180, 40);
            this.DeleteChunks.TabIndex = 61;
            this.DeleteChunks.Text = "Delete\r\n(Keep Scene Detection Data)";
            this.DeleteChunks.UseVisualStyleBackColor = false;
            this.DeleteChunks.Click += new System.EventHandler(this.DeleteChunks_Click);
            // 
            // Av1anResumeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(884, 202);
            this.Controls.Add(this.DeleteChunks);
            this.Controls.Add(this.DeleteFull);
            this.Controls.Add(this.resumeWithNewSettings);
            this.Controls.Add(this.resumeWithSavedSettings);
            this.Controls.Add(this.folderList);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Av1anResumeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select AV1AN Encode To Resume";
            this.Load += new System.EventHandler(this.Av1anResumeForm_Load);
            this.Shown += new System.EventHandler(this.Av1anResumeForm_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox folderList;
        private System.Windows.Forms.Button resumeWithSavedSettings;
        private System.Windows.Forms.Button resumeWithNewSettings;
        private System.Windows.Forms.Button DeleteFull;
        private System.Windows.Forms.Button DeleteChunks;
    }
}