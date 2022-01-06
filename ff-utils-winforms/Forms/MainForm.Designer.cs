using Nmkoder.UI;

namespace Nmkoder.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.logTbox = new System.Windows.Forms.TextBox();
            this.formatInfo = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.inputPanel = new System.Windows.Forms.Panel();
            this.thumbInfo = new System.Windows.Forms.Label();
            this.thumbnail = new System.Windows.Forms.PictureBox();
            this.taskMode = new System.Windows.Forms.ComboBox();
            this.inputDropPanel = new System.Windows.Forms.Panel();
            this.label33 = new System.Windows.Forms.Label();
            this.progBar = new HTAlt.WinForms.HTProgressBar();
            this.tabList = new Cyotek.Windows.Forms.TabList();
            this.fileListPage = new Cyotek.Windows.Forms.TabListPage();
            this.fileCountLabel = new System.Windows.Forms.Label();
            this.fileList = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileListSortBtn = new System.Windows.Forms.Button();
            this.fileListMode = new System.Windows.Forms.ComboBox();
            this.fileListMoveUpBtn = new System.Windows.Forms.Button();
            this.fileListRemoveBtn = new System.Windows.Forms.Button();
            this.fileListMoveDownBtn = new System.Windows.Forms.Button();
            this.addTracksFromFileBtn = new System.Windows.Forms.Button();
            this.streamListPage = new Cyotek.Windows.Forms.TabListPage();
            this.trackListCheckTracksBtn = new System.Windows.Forms.Button();
            this.trackListMoveUpBtn = new System.Windows.Forms.Button();
            this.trackListMoveDownBtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.trackListDefaultSubs = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.trackListDefaultAudio = new System.Windows.Forms.ComboBox();
            this.streamDetails = new System.Windows.Forms.TextBox();
            this.streamListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.quickConvertPage = new Cyotek.Windows.Forms.TabListPage();
            this.encCustomArgsOut = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.outputPath = new System.Windows.Forms.TextBox();
            this.encCustomArgsIn = new System.Windows.Forms.TextBox();
            this.containers = new System.Windows.Forms.ComboBox();
            this.quickEncTabControl = new HTAlt.WinForms.HTTabControl();
            this.encVid = new System.Windows.Forms.TabPage();
            this.encCropConfBtn = new HTAlt.WinForms.HTButton();
            this.encQualMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.presetInfo = new System.Windows.Forms.Label();
            this.qInfo = new System.Windows.Forms.Label();
            this.encScaleH = new System.Windows.Forms.TextBox();
            this.encScaleW = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.encCropMode = new System.Windows.Forms.ComboBox();
            this.label27 = new System.Windows.Forms.Label();
            this.encVidQuality = new System.Windows.Forms.NumericUpDown();
            this.label61 = new System.Windows.Forms.Label();
            this.encVidFps = new System.Windows.Forms.TextBox();
            this.encVidColors = new System.Windows.Forms.ComboBox();
            this.encVidPreset = new System.Windows.Forms.ComboBox();
            this.encVidCodec = new System.Windows.Forms.ComboBox();
            this.label51 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.encAudPerTrackPanel = new System.Windows.Forms.Panel();
            this.encAudConfigureBtn = new HTAlt.WinForms.HTButton();
            this.encAudConfMode = new System.Windows.Forms.ComboBox();
            this.label39 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.encAudChannels = new System.Windows.Forms.ComboBox();
            this.encAudQuality = new System.Windows.Forms.NumericUpDown();
            this.encAudCodec = new System.Windows.Forms.ComboBox();
            this.label53 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.encSubBurn = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.encSubCodec = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.encMetaTab = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.metadataGrid = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.metaMode = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.av1anPage = new Cyotek.Windows.Forms.TabListPage();
            this.av1anOutputPath = new System.Windows.Forms.TextBox();
            this.av1anContainer = new System.Windows.Forms.ComboBox();
            this.av1anTabControl = new HTAlt.WinForms.HTTabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.av1anCropConfBtn = new HTAlt.WinForms.HTButton();
            this.av1anThreads = new System.Windows.Forms.NumericUpDown();
            this.label46 = new System.Windows.Forms.Label();
            this.av1anGrainSynthDenoise = new System.Windows.Forms.CheckBox();
            this.av1anGrainSynthStrength = new System.Windows.Forms.NumericUpDown();
            this.label37 = new System.Windows.Forms.Label();
            this.av1anCustomEncArgs = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.av1anFps = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.av1anQualityMode = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.av1anScaleH = new System.Windows.Forms.TextBox();
            this.av1anScaleW = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.av1anCrop = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.av1anQuality = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.av1anColorSpace = new System.Windows.Forms.ComboBox();
            this.av1anPreset = new System.Windows.Forms.ComboBox();
            this.av1anCodec = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label26 = new System.Windows.Forms.Label();
            this.av1anAudChannels = new System.Windows.Forms.ComboBox();
            this.av1anAudQuality = new System.Windows.Forms.NumericUpDown();
            this.av1anAudCodec = new System.Windows.Forms.ComboBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.av1anResumeBtn = new HTAlt.WinForms.HTButton();
            this.label42 = new System.Windows.Forms.Label();
            this.av1anOptsConcatMode = new System.Windows.Forms.ComboBox();
            this.label41 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.av1anOptsWorkerCount = new System.Windows.Forms.NumericUpDown();
            this.av1anCustomArgs = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.av1anOptsChunkMode = new System.Windows.Forms.ComboBox();
            this.label34 = new System.Windows.Forms.Label();
            this.av1anOptsSplitMode = new System.Windows.Forms.ComboBox();
            this.label32 = new System.Windows.Forms.Label();
            this.utilsPage = new Cyotek.Windows.Forms.TabListPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.utilsBitratesPanel = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.utilsBitratesSelBtn = new HTAlt.WinForms.HTButton();
            this.utilsMetricsPanel = new System.Windows.Forms.Panel();
            this.utilsMetricsConfBtn = new HTAlt.WinForms.HTButton();
            this.label1 = new System.Windows.Forms.Label();
            this.utilsMetricsSelBtn = new HTAlt.WinForms.HTButton();
            this.utilsOcrPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.htButton2 = new HTAlt.WinForms.HTButton();
            this.utilsColorDataPanel = new System.Windows.Forms.Panel();
            this.utilsColorDataConfBtn = new HTAlt.WinForms.HTButton();
            this.label40 = new System.Windows.Forms.Label();
            this.htButton3 = new HTAlt.WinForms.HTButton();
            this.utilsConcatPanel = new System.Windows.Forms.Panel();
            this.label44 = new System.Windows.Forms.Label();
            this.htButton1 = new HTAlt.WinForms.HTButton();
            this.utilsBitratePlotPanel = new System.Windows.Forms.Panel();
            this.label45 = new System.Windows.Forms.Label();
            this.htButton4 = new HTAlt.WinForms.HTButton();
            this.settingsPage = new Cyotek.Windows.Forms.TabListPage();
            this.htTabControl1 = new HTAlt.WinForms.HTTabControl();
            this.settingsGeneralTab = new System.Windows.Forms.TabPage();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.label43 = new System.Windows.Forms.Label();
            this.settingsContainersTab = new System.Windows.Forms.TabPage();
            this.mp4Faststart = new System.Windows.Forms.CheckBox();
            this.label64 = new System.Windows.Forms.Label();
            this.progressCircle = new CircularProgressBar.CircularProgressBar();
            this.busyControlsPanel = new System.Windows.Forms.Panel();
            this.pauseBtn = new System.Windows.Forms.Button();
            this.stopBtn = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.currentActionLabel = new System.Windows.Forms.Label();
            this.checkItemsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.checkAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invertSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkAllVideoTracksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkAllAudioTracksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkAllSubtitleTracksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortFileListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sortMenuAbcDesc = new System.Windows.Forms.ToolStripMenuItem();
            this.sortMenuAbcAsc = new System.Windows.Forms.ToolStripMenuItem();
            this.sortMenuSizeDesc = new System.Windows.Forms.ToolStripMenuItem();
            this.sortMenuSizeAsc = new System.Windows.Forms.ToolStripMenuItem();
            this.sortMenuRecentDesc = new System.Windows.Forms.ToolStripMenuItem();
            this.sortMenuRecentAsc = new System.Windows.Forms.ToolStripMenuItem();
            this.runBtn = new System.Windows.Forms.Button();
            this.label47 = new System.Windows.Forms.Label();
            this.av1anOptsChunkOrder = new System.Windows.Forms.ComboBox();
            this.inputPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thumbnail)).BeginInit();
            this.tabList.SuspendLayout();
            this.fileListPage.SuspendLayout();
            this.streamListPage.SuspendLayout();
            this.quickConvertPage.SuspendLayout();
            this.quickEncTabControl.SuspendLayout();
            this.encVid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.encVidQuality)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.encAudPerTrackPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.encAudQuality)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.encMetaTab.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metadataGrid)).BeginInit();
            this.panel2.SuspendLayout();
            this.av1anPage.SuspendLayout();
            this.av1anTabControl.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.av1anThreads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.av1anGrainSynthStrength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.av1anQuality)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.av1anAudQuality)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.av1anOptsWorkerCount)).BeginInit();
            this.utilsPage.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.utilsBitratesPanel.SuspendLayout();
            this.utilsMetricsPanel.SuspendLayout();
            this.utilsOcrPanel.SuspendLayout();
            this.utilsColorDataPanel.SuspendLayout();
            this.utilsConcatPanel.SuspendLayout();
            this.utilsBitratePlotPanel.SuspendLayout();
            this.settingsPage.SuspendLayout();
            this.htTabControl1.SuspendLayout();
            this.settingsGeneralTab.SuspendLayout();
            this.settingsContainersTab.SuspendLayout();
            this.busyControlsPanel.SuspendLayout();
            this.checkItemsContextMenu.SuspendLayout();
            this.sortFileListContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // logTbox
            // 
            this.logTbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.logTbox.ForeColor = System.Drawing.Color.White;
            this.logTbox.Location = new System.Drawing.Point(332, 458);
            this.logTbox.Multiline = true;
            this.logTbox.Name = "logTbox";
            this.logTbox.ReadOnly = true;
            this.logTbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logTbox.Size = new System.Drawing.Size(840, 110);
            this.logTbox.TabIndex = 8;
            this.logTbox.Text = "Ready...";
            // 
            // formatInfo
            // 
            this.formatInfo.AutoSize = true;
            this.formatInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.formatInfo.ForeColor = System.Drawing.Color.White;
            this.formatInfo.Location = new System.Drawing.Point(4, 4);
            this.formatInfo.Margin = new System.Windows.Forms.Padding(4);
            this.formatInfo.Name = "formatInfo";
            this.formatInfo.Size = new System.Drawing.Size(0, 16);
            this.formatInfo.TabIndex = 28;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Yu Gothic UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(12, 9);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(156, 40);
            this.titleLabel.TabIndex = 10;
            this.titleLabel.Text = "NMKODER";
            // 
            // inputPanel
            // 
            this.inputPanel.AllowDrop = true;
            this.inputPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.inputPanel.Controls.Add(this.thumbInfo);
            this.inputPanel.Controls.Add(this.thumbnail);
            this.inputPanel.Controls.Add(this.taskMode);
            this.inputPanel.Controls.Add(this.inputDropPanel);
            this.inputPanel.Controls.Add(this.label33);
            this.inputPanel.Location = new System.Drawing.Point(12, 62);
            this.inputPanel.Name = "inputPanel";
            this.inputPanel.Size = new System.Drawing.Size(314, 390);
            this.inputPanel.TabIndex = 12;
            this.inputPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.inputPanel_DragDrop);
            this.inputPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.inputPanel_DragEnter);
            // 
            // thumbInfo
            // 
            this.thumbInfo.ForeColor = System.Drawing.Color.DarkGray;
            this.thumbInfo.Location = new System.Drawing.Point(3, 342);
            this.thumbInfo.Margin = new System.Windows.Forms.Padding(4);
            this.thumbInfo.Name = "thumbInfo";
            this.thumbInfo.Size = new System.Drawing.Size(304, 13);
            this.thumbInfo.TabIndex = 29;
            this.thumbInfo.Text = "No Thumbnails Loaded.";
            // 
            // thumbnail
            // 
            this.thumbnail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.thumbnail.Enabled = false;
            this.thumbnail.Image = global::Nmkoder.Properties.Resources.baseline_image_white_48dp_4x_25pcAlphaPad;
            this.thumbnail.Location = new System.Drawing.Point(7, 133);
            this.thumbnail.Margin = new System.Windows.Forms.Padding(0);
            this.thumbnail.Name = "thumbnail";
            this.thumbnail.Size = new System.Drawing.Size(301, 205);
            this.thumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.thumbnail.TabIndex = 27;
            this.thumbnail.TabStop = false;
            this.thumbnail.Click += new System.EventHandler(this.thumbnail_Click);
            // 
            // taskMode
            // 
            this.taskMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.taskMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.taskMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.taskMode.ForeColor = System.Drawing.Color.White;
            this.taskMode.FormattingEnabled = true;
            this.taskMode.Items.AddRange(new object[] {
            "Load File On Drop, Start Manually",
            "Run Selected Action Right After Dropping File"});
            this.taskMode.Location = new System.Drawing.Point(6, 362);
            this.taskMode.Name = "taskMode";
            this.taskMode.Size = new System.Drawing.Size(302, 21);
            this.taskMode.TabIndex = 26;
            this.taskMode.SelectedIndexChanged += new System.EventHandler(this.SaveUiConfig);
            // 
            // inputDropPanel
            // 
            this.inputDropPanel.AllowDrop = true;
            this.inputDropPanel.BackColor = System.Drawing.Color.Transparent;
            this.inputDropPanel.BackgroundImage = global::Nmkoder.Properties.Resources.dragdrop2_white;
            this.inputDropPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.inputDropPanel.Enabled = false;
            this.inputDropPanel.Location = new System.Drawing.Point(6, 27);
            this.inputDropPanel.Margin = new System.Windows.Forms.Padding(6);
            this.inputDropPanel.Name = "inputDropPanel";
            this.inputDropPanel.Size = new System.Drawing.Size(302, 100);
            this.inputDropPanel.TabIndex = 1;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.ForeColor = System.Drawing.Color.White;
            this.label33.Location = new System.Drawing.Point(4, 4);
            this.label33.Margin = new System.Windows.Forms.Padding(4);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(209, 13);
            this.label33.TabIndex = 16;
            this.label33.Text = "Drag and drop a file into this area to load it:";
            // 
            // progBar
            // 
            this.progBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.progBar.BorderThickness = 0;
            this.progBar.Location = new System.Drawing.Point(332, 574);
            this.progBar.Name = "progBar";
            this.progBar.Size = new System.Drawing.Size(840, 15);
            this.progBar.TabIndex = 34;
            // 
            // tabList
            // 
            this.tabList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.tabList.Controls.Add(this.fileListPage);
            this.tabList.Controls.Add(this.streamListPage);
            this.tabList.Controls.Add(this.quickConvertPage);
            this.tabList.Controls.Add(this.av1anPage);
            this.tabList.Controls.Add(this.utilsPage);
            this.tabList.Controls.Add(this.settingsPage);
            this.tabList.ForeColor = System.Drawing.Color.DodgerBlue;
            this.tabList.Location = new System.Drawing.Point(332, 62);
            this.tabList.Name = "tabList";
            this.tabList.Size = new System.Drawing.Size(840, 390);
            this.tabList.TabIndex = 35;
            this.tabList.SelectedIndexChanged += new System.EventHandler(this.tabList_SelectedIndexChanged);
            this.tabList.Leave += new System.EventHandler(this.streamList_Leave);
            // 
            // fileListPage
            // 
            this.fileListPage.Controls.Add(this.fileCountLabel);
            this.fileListPage.Controls.Add(this.fileList);
            this.fileListPage.Controls.Add(this.fileListSortBtn);
            this.fileListPage.Controls.Add(this.fileListMode);
            this.fileListPage.Controls.Add(this.fileListMoveUpBtn);
            this.fileListPage.Controls.Add(this.fileListRemoveBtn);
            this.fileListPage.Controls.Add(this.fileListMoveDownBtn);
            this.fileListPage.Controls.Add(this.addTracksFromFileBtn);
            this.fileListPage.Name = "fileListPage";
            this.fileListPage.Size = new System.Drawing.Size(682, 382);
            this.fileListPage.Text = "File List";
            // 
            // fileCountLabel
            // 
            this.fileCountLabel.AutoSize = true;
            this.fileCountLabel.ForeColor = System.Drawing.Color.White;
            this.fileCountLabel.Location = new System.Drawing.Point(0, 31);
            this.fileCountLabel.Margin = new System.Windows.Forms.Padding(4);
            this.fileCountLabel.Name = "fileCountLabel";
            this.fileCountLabel.Size = new System.Drawing.Size(80, 13);
            this.fileCountLabel.TabIndex = 54;
            this.fileCountLabel.Text = "No files loaded.";
            // 
            // fileList
            // 
            this.fileList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.fileList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.fileList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.fileList.ForeColor = System.Drawing.Color.White;
            this.fileList.FullRowSelect = true;
            this.fileList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.fileList.HideSelection = false;
            this.fileList.LabelWrap = false;
            this.fileList.Location = new System.Drawing.Point(3, 49);
            this.fileList.Name = "fileList";
            this.fileList.Size = new System.Drawing.Size(676, 326);
            this.fileList.TabIndex = 53;
            this.fileList.UseCompatibleStateImageBehavior = false;
            this.fileList.View = System.Windows.Forms.View.Details;
            this.fileList.SelectedIndexChanged += new System.EventHandler(this.fileList_SelectedIndexChanged);
            this.fileList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.fileList_MouseDoubleClick);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 650;
            // 
            // fileListSortBtn
            // 
            this.fileListSortBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.fileListSortBtn.BackgroundImage = global::Nmkoder.Properties.Resources.icon_sort;
            this.fileListSortBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.fileListSortBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fileListSortBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileListSortBtn.ForeColor = System.Drawing.Color.White;
            this.fileListSortBtn.Location = new System.Drawing.Point(642, 3);
            this.fileListSortBtn.Name = "fileListSortBtn";
            this.fileListSortBtn.Size = new System.Drawing.Size(40, 40);
            this.fileListSortBtn.TabIndex = 43;
            this.toolTip.SetToolTip(this.fileListSortBtn, "Sort...");
            this.fileListSortBtn.UseVisualStyleBackColor = false;
            this.fileListSortBtn.Click += new System.EventHandler(this.fileListSortBtn_Click);
            // 
            // fileListMode
            // 
            this.fileListMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.fileListMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileListMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fileListMode.ForeColor = System.Drawing.Color.White;
            this.fileListMode.FormattingEnabled = true;
            this.fileListMode.Items.AddRange(new object[] {
            "Muxing Mode - Create One Output File From One Or More Input File(s)",
            "Batch Processing Mode - Run The Same Action On All Input Files"});
            this.fileListMode.Location = new System.Drawing.Point(3, 3);
            this.fileListMode.Name = "fileListMode";
            this.fileListMode.Size = new System.Drawing.Size(449, 21);
            this.fileListMode.TabIndex = 42;
            this.fileListMode.SelectedIndexChanged += new System.EventHandler(this.fileListMode_SelectedIndexChanged);
            // 
            // fileListMoveUpBtn
            // 
            this.fileListMoveUpBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.fileListMoveUpBtn.BackgroundImage = global::Nmkoder.Properties.Resources.icon_arrow_up;
            this.fileListMoveUpBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.fileListMoveUpBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fileListMoveUpBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileListMoveUpBtn.ForeColor = System.Drawing.Color.White;
            this.fileListMoveUpBtn.Location = new System.Drawing.Point(458, 3);
            this.fileListMoveUpBtn.Name = "fileListMoveUpBtn";
            this.fileListMoveUpBtn.Size = new System.Drawing.Size(40, 40);
            this.fileListMoveUpBtn.TabIndex = 41;
            this.toolTip.SetToolTip(this.fileListMoveUpBtn, "Move Up");
            this.fileListMoveUpBtn.UseVisualStyleBackColor = false;
            this.fileListMoveUpBtn.Click += new System.EventHandler(this.fileListMoveUpBtn_Click);
            // 
            // fileListRemoveBtn
            // 
            this.fileListRemoveBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.fileListRemoveBtn.BackgroundImage = global::Nmkoder.Properties.Resources.icon_clear;
            this.fileListRemoveBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.fileListRemoveBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fileListRemoveBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileListRemoveBtn.ForeColor = System.Drawing.Color.White;
            this.fileListRemoveBtn.Location = new System.Drawing.Point(550, 3);
            this.fileListRemoveBtn.Name = "fileListRemoveBtn";
            this.fileListRemoveBtn.Size = new System.Drawing.Size(40, 40);
            this.fileListRemoveBtn.TabIndex = 40;
            this.toolTip.SetToolTip(this.fileListRemoveBtn, "Remove this file from the list. Also removes all loaded tracks.");
            this.fileListRemoveBtn.UseVisualStyleBackColor = false;
            this.fileListRemoveBtn.Click += new System.EventHandler(this.fileListCleanBtn_Click);
            // 
            // fileListMoveDownBtn
            // 
            this.fileListMoveDownBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.fileListMoveDownBtn.BackgroundImage = global::Nmkoder.Properties.Resources.icon_arrow_down;
            this.fileListMoveDownBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.fileListMoveDownBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fileListMoveDownBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileListMoveDownBtn.ForeColor = System.Drawing.Color.White;
            this.fileListMoveDownBtn.Location = new System.Drawing.Point(504, 3);
            this.fileListMoveDownBtn.Name = "fileListMoveDownBtn";
            this.fileListMoveDownBtn.Size = new System.Drawing.Size(40, 40);
            this.fileListMoveDownBtn.TabIndex = 39;
            this.toolTip.SetToolTip(this.fileListMoveDownBtn, "Move Down");
            this.fileListMoveDownBtn.UseVisualStyleBackColor = false;
            this.fileListMoveDownBtn.Click += new System.EventHandler(this.fileListMoveDownBtn_Click);
            // 
            // addTracksFromFileBtn
            // 
            this.addTracksFromFileBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.addTracksFromFileBtn.BackgroundImage = global::Nmkoder.Properties.Resources.icon_extract;
            this.addTracksFromFileBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.addTracksFromFileBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addTracksFromFileBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addTracksFromFileBtn.ForeColor = System.Drawing.Color.White;
            this.addTracksFromFileBtn.Location = new System.Drawing.Point(596, 3);
            this.addTracksFromFileBtn.Name = "addTracksFromFileBtn";
            this.addTracksFromFileBtn.Size = new System.Drawing.Size(40, 40);
            this.addTracksFromFileBtn.TabIndex = 38;
            this.toolTip.SetToolTip(this.addTracksFromFileBtn, "Load all tracks from the selected file into the track list.");
            this.addTracksFromFileBtn.UseVisualStyleBackColor = false;
            this.addTracksFromFileBtn.Click += new System.EventHandler(this.addTracksFromFileBtn_Click);
            // 
            // streamListPage
            // 
            this.streamListPage.Controls.Add(this.trackListCheckTracksBtn);
            this.streamListPage.Controls.Add(this.trackListMoveUpBtn);
            this.streamListPage.Controls.Add(this.trackListMoveDownBtn);
            this.streamListPage.Controls.Add(this.label7);
            this.streamListPage.Controls.Add(this.trackListDefaultSubs);
            this.streamListPage.Controls.Add(this.label6);
            this.streamListPage.Controls.Add(this.trackListDefaultAudio);
            this.streamListPage.Controls.Add(this.streamDetails);
            this.streamListPage.Controls.Add(this.formatInfo);
            this.streamListPage.Controls.Add(this.streamListView);
            this.streamListPage.Name = "streamListPage";
            this.streamListPage.Size = new System.Drawing.Size(682, 382);
            this.streamListPage.Text = "Track List";
            // 
            // trackListCheckTracksBtn
            // 
            this.trackListCheckTracksBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.trackListCheckTracksBtn.BackgroundImage = global::Nmkoder.Properties.Resources.icon_checklist;
            this.trackListCheckTracksBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.trackListCheckTracksBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.trackListCheckTracksBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackListCheckTracksBtn.ForeColor = System.Drawing.Color.White;
            this.trackListCheckTracksBtn.Location = new System.Drawing.Point(626, 62);
            this.trackListCheckTracksBtn.Name = "trackListCheckTracksBtn";
            this.trackListCheckTracksBtn.Size = new System.Drawing.Size(30, 30);
            this.trackListCheckTracksBtn.TabIndex = 51;
            this.toolTip.SetToolTip(this.trackListCheckTracksBtn, "Move Down");
            this.trackListCheckTracksBtn.UseVisualStyleBackColor = false;
            this.trackListCheckTracksBtn.Click += new System.EventHandler(this.trackListCheckTracksBtn_Click);
            // 
            // trackListMoveUpBtn
            // 
            this.trackListMoveUpBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.trackListMoveUpBtn.BackgroundImage = global::Nmkoder.Properties.Resources.icon_arrow_up;
            this.trackListMoveUpBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.trackListMoveUpBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.trackListMoveUpBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackListMoveUpBtn.ForeColor = System.Drawing.Color.White;
            this.trackListMoveUpBtn.Location = new System.Drawing.Point(591, 233);
            this.trackListMoveUpBtn.Name = "trackListMoveUpBtn";
            this.trackListMoveUpBtn.Size = new System.Drawing.Size(30, 30);
            this.trackListMoveUpBtn.TabIndex = 50;
            this.toolTip.SetToolTip(this.trackListMoveUpBtn, "Move Up");
            this.trackListMoveUpBtn.UseVisualStyleBackColor = false;
            this.trackListMoveUpBtn.Visible = false;
            this.trackListMoveUpBtn.Click += new System.EventHandler(this.trackListMoveUpBtn_Click);
            // 
            // trackListMoveDownBtn
            // 
            this.trackListMoveDownBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.trackListMoveDownBtn.BackgroundImage = global::Nmkoder.Properties.Resources.icon_arrow_down;
            this.trackListMoveDownBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.trackListMoveDownBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.trackListMoveDownBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackListMoveDownBtn.ForeColor = System.Drawing.Color.White;
            this.trackListMoveDownBtn.Location = new System.Drawing.Point(626, 233);
            this.trackListMoveDownBtn.Name = "trackListMoveDownBtn";
            this.trackListMoveDownBtn.Size = new System.Drawing.Size(30, 30);
            this.trackListMoveDownBtn.TabIndex = 49;
            this.toolTip.SetToolTip(this.trackListMoveDownBtn, "Move Down");
            this.trackListMoveDownBtn.UseVisualStyleBackColor = false;
            this.trackListMoveDownBtn.Visible = false;
            this.trackListMoveDownBtn.Click += new System.EventHandler(this.trackListMoveDownBtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(349, 34);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 13);
            this.label7.TabIndex = 48;
            this.label7.Text = "Default Subtitle Track:";
            // 
            // trackListDefaultSubs
            // 
            this.trackListDefaultSubs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.trackListDefaultSubs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.trackListDefaultSubs.Enabled = false;
            this.trackListDefaultSubs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.trackListDefaultSubs.ForeColor = System.Drawing.Color.White;
            this.trackListDefaultSubs.FormattingEnabled = true;
            this.trackListDefaultSubs.Location = new System.Drawing.Point(469, 30);
            this.trackListDefaultSubs.Name = "trackListDefaultSubs";
            this.trackListDefaultSubs.Size = new System.Drawing.Size(210, 21);
            this.trackListDefaultSubs.TabIndex = 47;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(4, 34);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 13);
            this.label6.TabIndex = 46;
            this.label6.Text = "Default Audio Track:";
            // 
            // trackListDefaultAudio
            // 
            this.trackListDefaultAudio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.trackListDefaultAudio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.trackListDefaultAudio.Enabled = false;
            this.trackListDefaultAudio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.trackListDefaultAudio.ForeColor = System.Drawing.Color.White;
            this.trackListDefaultAudio.FormattingEnabled = true;
            this.trackListDefaultAudio.Location = new System.Drawing.Point(116, 30);
            this.trackListDefaultAudio.Name = "trackListDefaultAudio";
            this.trackListDefaultAudio.Size = new System.Drawing.Size(210, 21);
            this.trackListDefaultAudio.TabIndex = 45;
            // 
            // streamDetails
            // 
            this.streamDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.streamDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.streamDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.streamDetails.ForeColor = System.Drawing.Color.White;
            this.streamDetails.Location = new System.Drawing.Point(3, 279);
            this.streamDetails.Margin = new System.Windows.Forms.Padding(0);
            this.streamDetails.Multiline = true;
            this.streamDetails.Name = "streamDetails";
            this.streamDetails.ReadOnly = true;
            this.streamDetails.Size = new System.Drawing.Size(679, 100);
            this.streamDetails.TabIndex = 29;
            // 
            // streamListView
            // 
            this.streamListView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.streamListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.streamListView.CheckBoxes = true;
            this.streamListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.streamListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.streamListView.ForeColor = System.Drawing.Color.White;
            this.streamListView.FullRowSelect = true;
            this.streamListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.streamListView.HideSelection = false;
            this.streamListView.LabelWrap = false;
            this.streamListView.Location = new System.Drawing.Point(3, 57);
            this.streamListView.Name = "streamListView";
            this.streamListView.Size = new System.Drawing.Size(676, 211);
            this.streamListView.TabIndex = 52;
            this.streamListView.UseCompatibleStateImageBehavior = false;
            this.streamListView.View = System.Windows.Forms.View.Details;
            this.streamListView.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.streamList_ItemCheck);
            this.streamListView.SelectedIndexChanged += new System.EventHandler(this.streamList_SelectedIndexChanged);
            this.streamListView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.streamList_MouseDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 650;
            // 
            // quickConvertPage
            // 
            this.quickConvertPage.Controls.Add(this.encCustomArgsOut);
            this.quickConvertPage.Controls.Add(this.label30);
            this.quickConvertPage.Controls.Add(this.outputPath);
            this.quickConvertPage.Controls.Add(this.encCustomArgsIn);
            this.quickConvertPage.Controls.Add(this.containers);
            this.quickConvertPage.Controls.Add(this.quickEncTabControl);
            this.quickConvertPage.Name = "quickConvertPage";
            this.quickConvertPage.Size = new System.Drawing.Size(682, 382);
            this.quickConvertPage.Text = "Quick Convert";
            // 
            // encCustomArgsOut
            // 
            this.encCustomArgsOut.AllowDrop = true;
            this.encCustomArgsOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encCustomArgsOut.ForeColor = System.Drawing.Color.White;
            this.encCustomArgsOut.Location = new System.Drawing.Point(425, 328);
            this.encCustomArgsOut.MinimumSize = new System.Drawing.Size(4, 21);
            this.encCustomArgsOut.Name = "encCustomArgsOut";
            this.encCustomArgsOut.Size = new System.Drawing.Size(250, 20);
            this.encCustomArgsOut.TabIndex = 54;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.ForeColor = System.Drawing.Color.White;
            this.label30.Location = new System.Drawing.Point(14, 331);
            this.label30.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(165, 13);
            this.label30.TabIndex = 52;
            this.label30.Text = "Custom Arguments (Input/Output)";
            // 
            // outputPath
            // 
            this.outputPath.AllowDrop = true;
            this.outputPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.outputPath.ForeColor = System.Drawing.Color.White;
            this.outputPath.Location = new System.Drawing.Point(7, 358);
            this.outputPath.MinimumSize = new System.Drawing.Size(4, 21);
            this.outputPath.Name = "outputPath";
            this.outputPath.Size = new System.Drawing.Size(555, 20);
            this.outputPath.TabIndex = 45;
            // 
            // encCustomArgsIn
            // 
            this.encCustomArgsIn.AllowDrop = true;
            this.encCustomArgsIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encCustomArgsIn.ForeColor = System.Drawing.Color.White;
            this.encCustomArgsIn.Location = new System.Drawing.Point(189, 328);
            this.encCustomArgsIn.MinimumSize = new System.Drawing.Size(4, 21);
            this.encCustomArgsIn.Name = "encCustomArgsIn";
            this.encCustomArgsIn.Size = new System.Drawing.Size(230, 20);
            this.encCustomArgsIn.TabIndex = 50;
            // 
            // containers
            // 
            this.containers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.containers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.containers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.containers.ForeColor = System.Drawing.Color.White;
            this.containers.FormattingEnabled = true;
            this.containers.Location = new System.Drawing.Point(568, 358);
            this.containers.Name = "containers";
            this.containers.Size = new System.Drawing.Size(107, 21);
            this.containers.TabIndex = 44;
            this.containers.SelectedIndexChanged += new System.EventHandler(this.containers_SelectedIndexChanged);
            // 
            // quickEncTabControl
            // 
            this.quickEncTabControl.AllowDrop = true;
            this.quickEncTabControl.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.quickEncTabControl.BorderTabLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.quickEncTabControl.Controls.Add(this.encVid);
            this.quickEncTabControl.Controls.Add(this.tabPage2);
            this.quickEncTabControl.Controls.Add(this.tabPage1);
            this.quickEncTabControl.Controls.Add(this.encMetaTab);
            this.quickEncTabControl.DisableClose = true;
            this.quickEncTabControl.DisableDragging = true;
            this.quickEncTabControl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.quickEncTabControl.HoverTabButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(176)))), ((int)(((byte)(239)))));
            this.quickEncTabControl.HoverTabColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(151)))), ((int)(((byte)(234)))));
            this.quickEncTabControl.HoverUnselectedTabButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.quickEncTabControl.Location = new System.Drawing.Point(3, 3);
            this.quickEncTabControl.Name = "quickEncTabControl";
            this.quickEncTabControl.Padding = new System.Drawing.Point(14, 4);
            this.quickEncTabControl.SelectedIndex = 0;
            this.quickEncTabControl.SelectedTabButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(151)))), ((int)(((byte)(234)))));
            this.quickEncTabControl.SelectedTabColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.quickEncTabControl.Size = new System.Drawing.Size(676, 319);
            this.quickEncTabControl.TabIndex = 0;
            this.quickEncTabControl.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.quickEncTabControl.UnderBorderTabLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(70)))));
            this.quickEncTabControl.UnselectedBorderTabLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.quickEncTabControl.UnselectedTabColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.quickEncTabControl.UpDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.quickEncTabControl.UpDownTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(112)))));
            // 
            // encVid
            // 
            this.encVid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encVid.Controls.Add(this.encCropConfBtn);
            this.encVid.Controls.Add(this.encQualMode);
            this.encVid.Controls.Add(this.label4);
            this.encVid.Controls.Add(this.label3);
            this.encVid.Controls.Add(this.presetInfo);
            this.encVid.Controls.Add(this.qInfo);
            this.encVid.Controls.Add(this.encScaleH);
            this.encVid.Controls.Add(this.encScaleW);
            this.encVid.Controls.Add(this.label29);
            this.encVid.Controls.Add(this.encCropMode);
            this.encVid.Controls.Add(this.label27);
            this.encVid.Controls.Add(this.encVidQuality);
            this.encVid.Controls.Add(this.label61);
            this.encVid.Controls.Add(this.encVidFps);
            this.encVid.Controls.Add(this.encVidColors);
            this.encVid.Controls.Add(this.encVidPreset);
            this.encVid.Controls.Add(this.encVidCodec);
            this.encVid.Controls.Add(this.label51);
            this.encVid.Controls.Add(this.label50);
            this.encVid.Controls.Add(this.label49);
            this.encVid.Controls.Add(this.label48);
            this.encVid.Controls.Add(this.label38);
            this.encVid.Location = new System.Drawing.Point(4, 27);
            this.encVid.Name = "encVid";
            this.encVid.Padding = new System.Windows.Forms.Padding(3);
            this.encVid.Size = new System.Drawing.Size(668, 288);
            this.encVid.TabIndex = 0;
            this.encVid.Text = "Video";
            // 
            // encCropConfBtn
            // 
            this.encCropConfBtn.AutoColor = true;
            this.encCropConfBtn.ButtonImage = null;
            this.encCropConfBtn.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.encCropConfBtn.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(124)))), ((int)(((byte)(124)))));
            this.encCropConfBtn.DrawImage = false;
            this.encCropConfBtn.ForeColor = System.Drawing.Color.White;
            this.encCropConfBtn.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(104)))), ((int)(((byte)(104)))));
            this.encCropConfBtn.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.encCropConfBtn.Location = new System.Drawing.Point(480, 187);
            this.encCropConfBtn.Name = "encCropConfBtn";
            this.encCropConfBtn.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.encCropConfBtn.Size = new System.Drawing.Size(150, 23);
            this.encCropConfBtn.TabIndex = 66;
            this.encCropConfBtn.Text = "Configure...";
            this.encCropConfBtn.Click += new System.EventHandler(this.encCropConfBtn_Click);
            // 
            // encQualMode
            // 
            this.encQualMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encQualMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encQualMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.encQualMode.ForeColor = System.Drawing.Color.White;
            this.encQualMode.FormattingEnabled = true;
            this.encQualMode.Location = new System.Drawing.Point(336, 38);
            this.encQualMode.Name = "encQualMode";
            this.encQualMode.Size = new System.Drawing.Size(134, 23);
            this.encQualMode.TabIndex = 65;
            this.toolTip.SetToolTip(this.encQualMode, "Select how the quality/filesize will be controlled");
            this.encQualMode.SelectedIndexChanged += new System.EventHandler(this.encQualityMode_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Silver;
            this.label4.Location = new System.Drawing.Point(477, 130);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 15);
            this.label4.TabIndex = 64;
            this.label4.Text = "Unchanged if left empty.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Silver;
            this.label3.Location = new System.Drawing.Point(477, 160);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 15);
            this.label3.TabIndex = 63;
            this.label3.Text = "Unchanged if left empty.";
            // 
            // presetInfo
            // 
            this.presetInfo.AutoSize = true;
            this.presetInfo.ForeColor = System.Drawing.Color.Silver;
            this.presetInfo.Location = new System.Drawing.Point(477, 70);
            this.presetInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.presetInfo.Name = "presetInfo";
            this.presetInfo.Size = new System.Drawing.Size(0, 15);
            this.presetInfo.TabIndex = 62;
            // 
            // qInfo
            // 
            this.qInfo.AutoSize = true;
            this.qInfo.ForeColor = System.Drawing.Color.Silver;
            this.qInfo.Location = new System.Drawing.Point(477, 40);
            this.qInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.qInfo.Name = "qInfo";
            this.qInfo.Size = new System.Drawing.Size(0, 15);
            this.qInfo.TabIndex = 61;
            // 
            // encScaleH
            // 
            this.encScaleH.AllowDrop = true;
            this.encScaleH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encScaleH.ForeColor = System.Drawing.Color.White;
            this.encScaleH.Location = new System.Drawing.Point(360, 157);
            this.encScaleH.MinimumSize = new System.Drawing.Size(4, 21);
            this.encScaleH.Name = "encScaleH";
            this.encScaleH.Size = new System.Drawing.Size(110, 23);
            this.encScaleH.TabIndex = 60;
            this.toolTip.SetToolTip(this.encScaleH, "Examples:\r\n\"720\"\r\n\"50%\"\r\nLeave empty to automatically scale based on the width.\r\n" +
        "");
            // 
            // encScaleW
            // 
            this.encScaleW.AllowDrop = true;
            this.encScaleW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encScaleW.ForeColor = System.Drawing.Color.White;
            this.encScaleW.Location = new System.Drawing.Point(220, 157);
            this.encScaleW.MinimumSize = new System.Drawing.Size(4, 21);
            this.encScaleW.Name = "encScaleW";
            this.encScaleW.Size = new System.Drawing.Size(110, 23);
            this.encScaleW.TabIndex = 59;
            this.toolTip.SetToolTip(this.encScaleW, "Examples:\r\n\"1280\"\r\n\"50%\"\r\nLeave empty to automatically scale based on the height." +
        "");
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.ForeColor = System.Drawing.Color.White;
            this.label29.Location = new System.Drawing.Point(339, 160);
            this.label29.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(13, 15);
            this.label29.TabIndex = 58;
            this.label29.Text = "x";
            // 
            // encCropMode
            // 
            this.encCropMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encCropMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encCropMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.encCropMode.ForeColor = System.Drawing.Color.White;
            this.encCropMode.FormattingEnabled = true;
            this.encCropMode.Items.AddRange(new object[] {
            "Disable",
            "Manual",
            "Automatic"});
            this.encCropMode.Location = new System.Drawing.Point(220, 187);
            this.encCropMode.Name = "encCropMode";
            this.encCropMode.Size = new System.Drawing.Size(250, 23);
            this.encCropMode.TabIndex = 54;
            this.encCropMode.SelectedIndexChanged += new System.EventHandler(this.encCropMode_SelectedIndexChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.ForeColor = System.Drawing.Color.White;
            this.label27.Location = new System.Drawing.Point(7, 190);
            this.label27.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(33, 15);
            this.label27.TabIndex = 53;
            this.label27.Text = "Crop";
            // 
            // encVidQuality
            // 
            this.encVidQuality.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encVidQuality.ForeColor = System.Drawing.Color.White;
            this.encVidQuality.Location = new System.Drawing.Point(220, 38);
            this.encVidQuality.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.encVidQuality.Name = "encVidQuality";
            this.encVidQuality.Size = new System.Drawing.Size(110, 23);
            this.encVidQuality.TabIndex = 52;
            this.toolTip.SetToolTip(this.encVidQuality, "Set the video quality/filesize");
            this.encVidQuality.ValueChanged += new System.EventHandler(this.SaveUiConfig);
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.ForeColor = System.Drawing.Color.White;
            this.label61.Location = new System.Drawing.Point(7, 160);
            this.label61.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(39, 15);
            this.label61.TabIndex = 51;
            this.label61.Text = "Resize";
            // 
            // encVidFps
            // 
            this.encVidFps.AllowDrop = true;
            this.encVidFps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encVidFps.ForeColor = System.Drawing.Color.White;
            this.encVidFps.Location = new System.Drawing.Point(220, 127);
            this.encVidFps.MinimumSize = new System.Drawing.Size(4, 21);
            this.encVidFps.Name = "encVidFps";
            this.encVidFps.Size = new System.Drawing.Size(250, 23);
            this.encVidFps.TabIndex = 49;
            this.toolTip.SetToolTip(this.encVidFps, "This allows you to resample the frame rate without changing the video speed or lo" +
        "sing audio sync.");
            // 
            // encVidColors
            // 
            this.encVidColors.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encVidColors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encVidColors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.encVidColors.ForeColor = System.Drawing.Color.White;
            this.encVidColors.FormattingEnabled = true;
            this.encVidColors.Location = new System.Drawing.Point(220, 97);
            this.encVidColors.Name = "encVidColors";
            this.encVidColors.Size = new System.Drawing.Size(250, 23);
            this.encVidColors.TabIndex = 48;
            this.toolTip.SetToolTip(this.encVidColors, "Allows encoding without color subsampling (YUV444) or 10-bit encoding.\r\nKeep in m" +
        "ind that anything that\'s not YUV420P will not work on every media player!");
            this.encVidColors.SelectedIndexChanged += new System.EventHandler(this.SaveUiConfig);
            // 
            // encVidPreset
            // 
            this.encVidPreset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encVidPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encVidPreset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.encVidPreset.ForeColor = System.Drawing.Color.White;
            this.encVidPreset.FormattingEnabled = true;
            this.encVidPreset.Location = new System.Drawing.Point(220, 67);
            this.encVidPreset.Name = "encVidPreset";
            this.encVidPreset.Size = new System.Drawing.Size(250, 23);
            this.encVidPreset.TabIndex = 47;
            this.toolTip.SetToolTip(this.encVidPreset, "This determines how fast or slow the encoding will be.\r\nThe slower it is, the bet" +
        "ter the video compression (quality per filesize/bitrate) is.");
            this.encVidPreset.SelectedIndexChanged += new System.EventHandler(this.SaveUiConfig);
            // 
            // encVidCodec
            // 
            this.encVidCodec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encVidCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encVidCodec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.encVidCodec.ForeColor = System.Drawing.Color.White;
            this.encVidCodec.FormattingEnabled = true;
            this.encVidCodec.Location = new System.Drawing.Point(220, 7);
            this.encVidCodec.Name = "encVidCodec";
            this.encVidCodec.Size = new System.Drawing.Size(250, 23);
            this.encVidCodec.TabIndex = 45;
            this.toolTip.SetToolTip(this.encVidCodec, "Select which video codec and encoder to use.");
            this.encVidCodec.SelectedIndexChanged += new System.EventHandler(this.encVidCodec_SelectedIndexChanged);
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.ForeColor = System.Drawing.Color.White;
            this.label51.Location = new System.Drawing.Point(7, 130);
            this.label51.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(66, 15);
            this.label51.TabIndex = 21;
            this.label51.Text = "Frame Rate";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.ForeColor = System.Drawing.Color.White;
            this.label50.Location = new System.Drawing.Point(7, 100);
            this.label50.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(77, 15);
            this.label50.TabIndex = 20;
            this.label50.Text = "Color Format";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.ForeColor = System.Drawing.Color.White;
            this.label49.Location = new System.Drawing.Point(7, 70);
            this.label49.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(82, 15);
            this.label49.TabIndex = 19;
            this.label49.Text = "Speed (Preset)";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.ForeColor = System.Drawing.Color.White;
            this.label48.Location = new System.Drawing.Point(7, 40);
            this.label48.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(88, 15);
            this.label48.TabIndex = 18;
            this.label48.Text = "Quality Control";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.ForeColor = System.Drawing.Color.White;
            this.label38.Location = new System.Drawing.Point(5, 10);
            this.label38.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(95, 15);
            this.label38.TabIndex = 17;
            this.label38.Text = "Codec (Encoder)";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabPage2.Controls.Add(this.encAudPerTrackPanel);
            this.tabPage2.Controls.Add(this.encAudConfMode);
            this.tabPage2.Controls.Add(this.label39);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.encAudChannels);
            this.tabPage2.Controls.Add(this.encAudQuality);
            this.tabPage2.Controls.Add(this.encAudCodec);
            this.tabPage2.Controls.Add(this.label53);
            this.tabPage2.Controls.Add(this.label58);
            this.tabPage2.Controls.Add(this.pictureBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 27);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(668, 288);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Audio";
            // 
            // encAudPerTrackPanel
            // 
            this.encAudPerTrackPanel.Controls.Add(this.encAudConfigureBtn);
            this.encAudPerTrackPanel.Location = new System.Drawing.Point(220, 68);
            this.encAudPerTrackPanel.Margin = new System.Windows.Forms.Padding(0);
            this.encAudPerTrackPanel.Name = "encAudPerTrackPanel";
            this.encAudPerTrackPanel.Size = new System.Drawing.Size(250, 52);
            this.encAudPerTrackPanel.TabIndex = 58;
            // 
            // encAudConfigureBtn
            // 
            this.encAudConfigureBtn.AutoColor = true;
            this.encAudConfigureBtn.ButtonImage = null;
            this.encAudConfigureBtn.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.encAudConfigureBtn.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(124)))), ((int)(((byte)(124)))));
            this.encAudConfigureBtn.DrawImage = false;
            this.encAudConfigureBtn.ForeColor = System.Drawing.Color.White;
            this.encAudConfigureBtn.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(104)))), ((int)(((byte)(104)))));
            this.encAudConfigureBtn.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.encAudConfigureBtn.Location = new System.Drawing.Point(0, 15);
            this.encAudConfigureBtn.Name = "encAudConfigureBtn";
            this.encAudConfigureBtn.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.encAudConfigureBtn.Size = new System.Drawing.Size(250, 23);
            this.encAudConfigureBtn.TabIndex = 55;
            this.encAudConfigureBtn.Text = "Configure Per Track...";
            this.encAudConfigureBtn.Click += new System.EventHandler(this.encAudConfigureBtn_Click);
            // 
            // encAudConfMode
            // 
            this.encAudConfMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encAudConfMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encAudConfMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.encAudConfMode.ForeColor = System.Drawing.Color.White;
            this.encAudConfMode.FormattingEnabled = true;
            this.encAudConfMode.Items.AddRange(new object[] {
            "Basic - Apply Settings To All Tracks",
            "Advanced - Specify For Each Track"});
            this.encAudConfMode.Location = new System.Drawing.Point(220, 37);
            this.encAudConfMode.Name = "encAudConfMode";
            this.encAudConfMode.Size = new System.Drawing.Size(250, 23);
            this.encAudConfMode.TabIndex = 57;
            this.encAudConfMode.SelectedIndexChanged += new System.EventHandler(this.encAudConfMode_SelectedIndexChanged);
            this.encAudConfMode.VisibleChanged += new System.EventHandler(this.encAudConfMode_VisibleChanged);
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.ForeColor = System.Drawing.Color.White;
            this.label39.Location = new System.Drawing.Point(7, 40);
            this.label39.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(150, 15);
            this.label39.TabIndex = 56;
            this.label39.Text = "Audio Configuration Mode";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(7, 100);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 15);
            this.label11.TabIndex = 53;
            this.label11.Text = "Channels";
            // 
            // encAudChannels
            // 
            this.encAudChannels.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encAudChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encAudChannels.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.encAudChannels.ForeColor = System.Drawing.Color.White;
            this.encAudChannels.FormattingEnabled = true;
            this.encAudChannels.Items.AddRange(new object[] {
            "Unchanged",
            "1 (Mono 1.0)",
            "2 (Stereo 2.0)",
            "6 (Surround 5.1)",
            "8 (Surround 7.1)"});
            this.encAudChannels.Location = new System.Drawing.Point(220, 97);
            this.encAudChannels.Name = "encAudChannels";
            this.encAudChannels.Size = new System.Drawing.Size(250, 23);
            this.encAudChannels.TabIndex = 52;
            this.encAudChannels.SelectedIndexChanged += new System.EventHandler(this.encAudChannels_SelectedIndexChanged);
            // 
            // encAudQuality
            // 
            this.encAudQuality.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encAudQuality.ForeColor = System.Drawing.Color.White;
            this.encAudQuality.Location = new System.Drawing.Point(220, 68);
            this.encAudQuality.Maximum = new decimal(new int[] {
            6400,
            0,
            0,
            0});
            this.encAudQuality.Name = "encAudQuality";
            this.encAudQuality.Size = new System.Drawing.Size(221, 23);
            this.encAudQuality.TabIndex = 51;
            this.encAudQuality.ValueChanged += new System.EventHandler(this.SaveUiConfig);
            // 
            // encAudCodec
            // 
            this.encAudCodec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encAudCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encAudCodec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.encAudCodec.ForeColor = System.Drawing.Color.White;
            this.encAudCodec.FormattingEnabled = true;
            this.encAudCodec.Location = new System.Drawing.Point(220, 7);
            this.encAudCodec.Name = "encAudCodec";
            this.encAudCodec.Size = new System.Drawing.Size(250, 23);
            this.encAudCodec.TabIndex = 49;
            this.encAudCodec.SelectedIndexChanged += new System.EventHandler(this.encAudioCodec_SelectedIndexChanged);
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.ForeColor = System.Drawing.Color.White;
            this.label53.Location = new System.Drawing.Point(7, 70);
            this.label53.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(118, 15);
            this.label53.TabIndex = 48;
            this.label53.Text = "Quality (Stereo Kbps)";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.ForeColor = System.Drawing.Color.White;
            this.label58.Location = new System.Drawing.Point(5, 10);
            this.label58.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(95, 15);
            this.label58.TabIndex = 47;
            this.label58.Text = "Codec (Encoder)";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Nmkoder.Properties.Resources.icon_info;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Help;
            this.pictureBox1.Location = new System.Drawing.Point(447, 68);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(23, 23);
            this.pictureBox1.TabIndex = 54;
            this.pictureBox1.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBox1, "Base bitrate for Stereo (2-Channel).\r\nFor any other channel count (like 5.1), a m" +
        "ultiplier will be used on this value.");
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabPage1.Controls.Add(this.encSubBurn);
            this.tabPage1.Controls.Add(this.label25);
            this.tabPage1.Controls.Add(this.encSubCodec);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(668, 288);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Subtitles";
            // 
            // encSubBurn
            // 
            this.encSubBurn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encSubBurn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encSubBurn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.encSubBurn.ForeColor = System.Drawing.Color.White;
            this.encSubBurn.FormattingEnabled = true;
            this.encSubBurn.Location = new System.Drawing.Point(220, 37);
            this.encSubBurn.Name = "encSubBurn";
            this.encSubBurn.Size = new System.Drawing.Size(250, 23);
            this.encSubBurn.TabIndex = 49;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.ForeColor = System.Drawing.Color.White;
            this.label25.Location = new System.Drawing.Point(7, 40);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(45, 15);
            this.label25.TabIndex = 48;
            this.label25.Text = "Burn In";
            // 
            // encSubCodec
            // 
            this.encSubCodec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encSubCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encSubCodec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.encSubCodec.ForeColor = System.Drawing.Color.White;
            this.encSubCodec.FormattingEnabled = true;
            this.encSubCodec.Location = new System.Drawing.Point(220, 7);
            this.encSubCodec.Name = "encSubCodec";
            this.encSubCodec.Size = new System.Drawing.Size(250, 23);
            this.encSubCodec.TabIndex = 47;
            this.encSubCodec.SelectedIndexChanged += new System.EventHandler(this.encSubCodec_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(5, 10);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 15);
            this.label10.TabIndex = 46;
            this.label10.Text = "Codec (Encoder)";
            // 
            // encMetaTab
            // 
            this.encMetaTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.encMetaTab.Controls.Add(this.panel3);
            this.encMetaTab.Controls.Add(this.panel2);
            this.encMetaTab.ForeColor = System.Drawing.Color.White;
            this.encMetaTab.Location = new System.Drawing.Point(4, 27);
            this.encMetaTab.Name = "encMetaTab";
            this.encMetaTab.Padding = new System.Windows.Forms.Padding(3);
            this.encMetaTab.Size = new System.Drawing.Size(668, 288);
            this.encMetaTab.TabIndex = 3;
            this.encMetaTab.Text = "Metadata";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.metadataGrid);
            this.panel3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel3.Location = new System.Drawing.Point(6, 77);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(656, 205);
            this.panel3.TabIndex = 7;
            // 
            // metadataGrid
            // 
            this.metadataGrid.AllowUserToAddRows = false;
            this.metadataGrid.AllowUserToDeleteRows = false;
            this.metadataGrid.AllowUserToResizeColumns = false;
            this.metadataGrid.AllowUserToResizeRows = false;
            this.metadataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.metadataGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.metadataGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.metadataGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.metadataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.metadataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metadataGrid.Location = new System.Drawing.Point(0, 0);
            this.metadataGrid.MultiSelect = false;
            this.metadataGrid.Name = "metadataGrid";
            this.metadataGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.metadataGrid.RowHeadersWidth = 51;
            this.metadataGrid.Size = new System.Drawing.Size(656, 205);
            this.metadataGrid.TabIndex = 1;
            this.metadataGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.metadataGrid_CellEndEdit);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.metaMode);
            this.panel2.Controls.Add(this.label21);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Location = new System.Drawing.Point(6, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(656, 65);
            this.panel2.TabIndex = 6;
            // 
            // metaMode
            // 
            this.metaMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.metaMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.metaMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.metaMode.ForeColor = System.Drawing.Color.White;
            this.metaMode.FormattingEnabled = true;
            this.metaMode.Items.AddRange(new object[] {
            "Copy All From Input, Edit Titles/Languages",
            "Apply Titles/Languages, Strip Rest",
            "Strip All Metadata Including Titles/Languages"});
            this.metaMode.Location = new System.Drawing.Point(220, 37);
            this.metaMode.Name = "metaMode";
            this.metaMode.Size = new System.Drawing.Size(433, 23);
            this.metaMode.TabIndex = 49;
            this.metaMode.SelectedIndexChanged += new System.EventHandler(this.metaMode_SelectedIndexChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(4, 40);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(143, 15);
            this.label21.TabIndex = 18;
            this.label21.Text = "Metadata Handling Mode";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.SystemColors.Control;
            this.label14.Location = new System.Drawing.Point(3, 3);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(616, 15);
            this.label14.TabIndex = 0;
            this.label14.Text = "Here you can edit the title and language tag (ISO 639-2) of each track. Double cl" +
    "ick or press F2 to edit a selected cell.";
            // 
            // av1anPage
            // 
            this.av1anPage.Controls.Add(this.av1anOutputPath);
            this.av1anPage.Controls.Add(this.av1anContainer);
            this.av1anPage.Controls.Add(this.av1anTabControl);
            this.av1anPage.Name = "av1anPage";
            this.av1anPage.Size = new System.Drawing.Size(682, 382);
            this.av1anPage.Text = "AV1AN";
            // 
            // av1anOutputPath
            // 
            this.av1anOutputPath.AllowDrop = true;
            this.av1anOutputPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anOutputPath.ForeColor = System.Drawing.Color.White;
            this.av1anOutputPath.Location = new System.Drawing.Point(7, 358);
            this.av1anOutputPath.MinimumSize = new System.Drawing.Size(4, 21);
            this.av1anOutputPath.Name = "av1anOutputPath";
            this.av1anOutputPath.Size = new System.Drawing.Size(555, 20);
            this.av1anOutputPath.TabIndex = 55;
            // 
            // av1anContainer
            // 
            this.av1anContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anContainer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.av1anContainer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.av1anContainer.ForeColor = System.Drawing.Color.White;
            this.av1anContainer.FormattingEnabled = true;
            this.av1anContainer.Location = new System.Drawing.Point(568, 358);
            this.av1anContainer.Name = "av1anContainer";
            this.av1anContainer.Size = new System.Drawing.Size(107, 21);
            this.av1anContainer.TabIndex = 54;
            this.av1anContainer.SelectedIndexChanged += new System.EventHandler(this.av1anContainer_SelectedIndexChanged);
            // 
            // av1anTabControl
            // 
            this.av1anTabControl.AllowDrop = true;
            this.av1anTabControl.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.av1anTabControl.BorderTabLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.av1anTabControl.Controls.Add(this.tabPage3);
            this.av1anTabControl.Controls.Add(this.tabPage4);
            this.av1anTabControl.Controls.Add(this.tabPage5);
            this.av1anTabControl.DisableClose = true;
            this.av1anTabControl.DisableDragging = true;
            this.av1anTabControl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.av1anTabControl.HoverTabButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(176)))), ((int)(((byte)(239)))));
            this.av1anTabControl.HoverTabColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(151)))), ((int)(((byte)(234)))));
            this.av1anTabControl.HoverUnselectedTabButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.av1anTabControl.Location = new System.Drawing.Point(3, 3);
            this.av1anTabControl.Name = "av1anTabControl";
            this.av1anTabControl.Padding = new System.Drawing.Point(14, 4);
            this.av1anTabControl.SelectedIndex = 0;
            this.av1anTabControl.SelectedTabButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(151)))), ((int)(((byte)(234)))));
            this.av1anTabControl.SelectedTabColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.av1anTabControl.Size = new System.Drawing.Size(676, 349);
            this.av1anTabControl.TabIndex = 53;
            this.av1anTabControl.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.av1anTabControl.UnderBorderTabLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(70)))));
            this.av1anTabControl.UnselectedBorderTabLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.av1anTabControl.UnselectedTabColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.av1anTabControl.UpDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.av1anTabControl.UpDownTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(112)))));
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabPage3.Controls.Add(this.av1anCropConfBtn);
            this.tabPage3.Controls.Add(this.av1anThreads);
            this.tabPage3.Controls.Add(this.label46);
            this.tabPage3.Controls.Add(this.av1anGrainSynthDenoise);
            this.tabPage3.Controls.Add(this.av1anGrainSynthStrength);
            this.tabPage3.Controls.Add(this.label37);
            this.tabPage3.Controls.Add(this.av1anCustomEncArgs);
            this.tabPage3.Controls.Add(this.label36);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.av1anFps);
            this.tabPage3.Controls.Add(this.label19);
            this.tabPage3.Controls.Add(this.av1anQualityMode);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Controls.Add(this.label15);
            this.tabPage3.Controls.Add(this.av1anScaleH);
            this.tabPage3.Controls.Add(this.av1anScaleW);
            this.tabPage3.Controls.Add(this.label16);
            this.tabPage3.Controls.Add(this.av1anCrop);
            this.tabPage3.Controls.Add(this.label17);
            this.tabPage3.Controls.Add(this.av1anQuality);
            this.tabPage3.Controls.Add(this.label18);
            this.tabPage3.Controls.Add(this.av1anColorSpace);
            this.tabPage3.Controls.Add(this.av1anPreset);
            this.tabPage3.Controls.Add(this.av1anCodec);
            this.tabPage3.Controls.Add(this.label20);
            this.tabPage3.Controls.Add(this.label22);
            this.tabPage3.Controls.Add(this.label23);
            this.tabPage3.Controls.Add(this.label24);
            this.tabPage3.Location = new System.Drawing.Point(4, 27);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(668, 318);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Video";
            // 
            // av1anCropConfBtn
            // 
            this.av1anCropConfBtn.AutoColor = true;
            this.av1anCropConfBtn.ButtonImage = null;
            this.av1anCropConfBtn.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.av1anCropConfBtn.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(124)))), ((int)(((byte)(124)))));
            this.av1anCropConfBtn.DrawImage = false;
            this.av1anCropConfBtn.ForeColor = System.Drawing.Color.White;
            this.av1anCropConfBtn.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(104)))), ((int)(((byte)(104)))));
            this.av1anCropConfBtn.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.av1anCropConfBtn.Location = new System.Drawing.Point(480, 217);
            this.av1anCropConfBtn.Name = "av1anCropConfBtn";
            this.av1anCropConfBtn.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.av1anCropConfBtn.Size = new System.Drawing.Size(150, 23);
            this.av1anCropConfBtn.TabIndex = 75;
            this.av1anCropConfBtn.Text = "Configure...";
            this.av1anCropConfBtn.Click += new System.EventHandler(this.av1anCropConfBtn_Click);
            // 
            // av1anThreads
            // 
            this.av1anThreads.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anThreads.ForeColor = System.Drawing.Color.White;
            this.av1anThreads.Location = new System.Drawing.Point(220, 248);
            this.av1anThreads.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.av1anThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.av1anThreads.Name = "av1anThreads";
            this.av1anThreads.Size = new System.Drawing.Size(250, 23);
            this.av1anThreads.TabIndex = 74;
            this.toolTip.SetToolTip(this.av1anThreads, "Amount of threads that each encoder instance (worker) can use");
            this.av1anThreads.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.ForeColor = System.Drawing.Color.White;
            this.label46.Location = new System.Drawing.Point(7, 250);
            this.label46.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(109, 15);
            this.label46.TabIndex = 73;
            this.label46.Text = "Threads Per Worker";
            // 
            // av1anGrainSynthDenoise
            // 
            this.av1anGrainSynthDenoise.AutoSize = true;
            this.av1anGrainSynthDenoise.ForeColor = System.Drawing.Color.White;
            this.av1anGrainSynthDenoise.Location = new System.Drawing.Point(402, 131);
            this.av1anGrainSynthDenoise.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.av1anGrainSynthDenoise.Name = "av1anGrainSynthDenoise";
            this.av1anGrainSynthDenoise.Size = new System.Drawing.Size(68, 19);
            this.av1anGrainSynthDenoise.TabIndex = 72;
            this.av1anGrainSynthDenoise.Text = "Denoise";
            this.av1anGrainSynthDenoise.UseVisualStyleBackColor = true;
            // 
            // av1anGrainSynthStrength
            // 
            this.av1anGrainSynthStrength.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anGrainSynthStrength.ForeColor = System.Drawing.Color.White;
            this.av1anGrainSynthStrength.Location = new System.Drawing.Point(220, 128);
            this.av1anGrainSynthStrength.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.av1anGrainSynthStrength.Name = "av1anGrainSynthStrength";
            this.av1anGrainSynthStrength.Size = new System.Drawing.Size(173, 23);
            this.av1anGrainSynthStrength.TabIndex = 71;
            this.toolTip.SetToolTip(this.av1anGrainSynthStrength, "Improves encodes of noisy videos by synthesizing grain");
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.ForeColor = System.Drawing.Color.White;
            this.label37.Location = new System.Drawing.Point(7, 130);
            this.label37.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(135, 15);
            this.label37.TabIndex = 70;
            this.label37.Text = "Grain Synthesis Strength";
            // 
            // av1anCustomEncArgs
            // 
            this.av1anCustomEncArgs.AllowDrop = true;
            this.av1anCustomEncArgs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anCustomEncArgs.ForeColor = System.Drawing.Color.White;
            this.av1anCustomEncArgs.Location = new System.Drawing.Point(220, 277);
            this.av1anCustomEncArgs.MinimumSize = new System.Drawing.Size(4, 21);
            this.av1anCustomEncArgs.Name = "av1anCustomEncArgs";
            this.av1anCustomEncArgs.Size = new System.Drawing.Size(250, 23);
            this.av1anCustomEncArgs.TabIndex = 69;
            this.toolTip.SetToolTip(this.av1anCustomEncArgs, "Enter custom arguments for the selected encoder.");
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.ForeColor = System.Drawing.Color.White;
            this.label36.Location = new System.Drawing.Point(7, 280);
            this.label36.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(157, 15);
            this.label36.TabIndex = 68;
            this.label36.Text = "Custom Encoder Arguments";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Silver;
            this.label9.Location = new System.Drawing.Point(477, 160);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(138, 15);
            this.label9.TabIndex = 67;
            this.label9.Text = "Unchanged if left empty.";
            // 
            // av1anFps
            // 
            this.av1anFps.AllowDrop = true;
            this.av1anFps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anFps.ForeColor = System.Drawing.Color.White;
            this.av1anFps.Location = new System.Drawing.Point(220, 157);
            this.av1anFps.MinimumSize = new System.Drawing.Size(4, 21);
            this.av1anFps.Name = "av1anFps";
            this.av1anFps.Size = new System.Drawing.Size(250, 23);
            this.av1anFps.TabIndex = 66;
            this.toolTip.SetToolTip(this.av1anFps, "This allows you to resample the frame rate without changing the video speed or lo" +
        "sing audio sync.");
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Location = new System.Drawing.Point(7, 160);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(66, 15);
            this.label19.TabIndex = 65;
            this.label19.Text = "Frame Rate";
            // 
            // av1anQualityMode
            // 
            this.av1anQualityMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anQualityMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.av1anQualityMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.av1anQualityMode.ForeColor = System.Drawing.Color.White;
            this.av1anQualityMode.FormattingEnabled = true;
            this.av1anQualityMode.Location = new System.Drawing.Point(336, 38);
            this.av1anQualityMode.Name = "av1anQualityMode";
            this.av1anQualityMode.Size = new System.Drawing.Size(134, 23);
            this.av1anQualityMode.TabIndex = 64;
            this.toolTip.SetToolTip(this.av1anQualityMode, "Use either a CRF value or a target VMAF quality level");
            this.av1anQualityMode.SelectedIndexChanged += new System.EventHandler(this.av1anQualityMode_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.Silver;
            this.label12.Location = new System.Drawing.Point(477, 190);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(138, 15);
            this.label12.TabIndex = 63;
            this.label12.Text = "Unchanged if left empty.";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Silver;
            this.label13.Location = new System.Drawing.Point(477, 70);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(0, 15);
            this.label13.TabIndex = 62;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.Silver;
            this.label15.Location = new System.Drawing.Point(477, 40);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(0, 15);
            this.label15.TabIndex = 61;
            // 
            // av1anScaleH
            // 
            this.av1anScaleH.AllowDrop = true;
            this.av1anScaleH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anScaleH.ForeColor = System.Drawing.Color.White;
            this.av1anScaleH.Location = new System.Drawing.Point(360, 187);
            this.av1anScaleH.MinimumSize = new System.Drawing.Size(4, 21);
            this.av1anScaleH.Name = "av1anScaleH";
            this.av1anScaleH.Size = new System.Drawing.Size(110, 23);
            this.av1anScaleH.TabIndex = 60;
            this.toolTip.SetToolTip(this.av1anScaleH, "Examples:\r\n\"720\"\r\n\"50%\"\r\nLeave empty to automatically scale based on the width.\r\n" +
        "");
            // 
            // av1anScaleW
            // 
            this.av1anScaleW.AllowDrop = true;
            this.av1anScaleW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anScaleW.ForeColor = System.Drawing.Color.White;
            this.av1anScaleW.Location = new System.Drawing.Point(220, 187);
            this.av1anScaleW.MinimumSize = new System.Drawing.Size(4, 21);
            this.av1anScaleW.Name = "av1anScaleW";
            this.av1anScaleW.Size = new System.Drawing.Size(110, 23);
            this.av1anScaleW.TabIndex = 59;
            this.toolTip.SetToolTip(this.av1anScaleW, "Examples:\r\n\"1280\"\r\n\"50%\"\r\nLeave empty to automatically scale based on the height." +
        "");
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(339, 190);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(13, 15);
            this.label16.TabIndex = 58;
            this.label16.Text = "x";
            // 
            // av1anCrop
            // 
            this.av1anCrop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anCrop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.av1anCrop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.av1anCrop.ForeColor = System.Drawing.Color.White;
            this.av1anCrop.FormattingEnabled = true;
            this.av1anCrop.Items.AddRange(new object[] {
            "Disable",
            "Manual",
            "Automatic"});
            this.av1anCrop.Location = new System.Drawing.Point(220, 217);
            this.av1anCrop.Name = "av1anCrop";
            this.av1anCrop.Size = new System.Drawing.Size(250, 23);
            this.av1anCrop.TabIndex = 54;
            this.av1anCrop.SelectedIndexChanged += new System.EventHandler(this.av1anCrop_SelectedIndexChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(7, 220);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(33, 15);
            this.label17.TabIndex = 53;
            this.label17.Text = "Crop";
            // 
            // av1anQuality
            // 
            this.av1anQuality.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anQuality.ForeColor = System.Drawing.Color.White;
            this.av1anQuality.Location = new System.Drawing.Point(220, 38);
            this.av1anQuality.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.av1anQuality.Name = "av1anQuality";
            this.av1anQuality.Size = new System.Drawing.Size(110, 23);
            this.av1anQuality.TabIndex = 52;
            this.toolTip.SetToolTip(this.av1anQuality, "Set the video quality level (CRF/CQ)");
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ForeColor = System.Drawing.Color.White;
            this.label18.Location = new System.Drawing.Point(7, 190);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(39, 15);
            this.label18.TabIndex = 51;
            this.label18.Text = "Resize";
            // 
            // av1anColorSpace
            // 
            this.av1anColorSpace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anColorSpace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.av1anColorSpace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.av1anColorSpace.ForeColor = System.Drawing.Color.White;
            this.av1anColorSpace.FormattingEnabled = true;
            this.av1anColorSpace.Location = new System.Drawing.Point(220, 97);
            this.av1anColorSpace.Name = "av1anColorSpace";
            this.av1anColorSpace.Size = new System.Drawing.Size(250, 23);
            this.av1anColorSpace.TabIndex = 48;
            this.toolTip.SetToolTip(this.av1anColorSpace, "Allows encoding without color subsampling (YUV444) or 10-bit encoding.\r\nKeep in m" +
        "ind that anything that\'s not YUV420P will not work on every media player!");
            // 
            // av1anPreset
            // 
            this.av1anPreset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.av1anPreset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.av1anPreset.ForeColor = System.Drawing.Color.White;
            this.av1anPreset.FormattingEnabled = true;
            this.av1anPreset.Location = new System.Drawing.Point(220, 67);
            this.av1anPreset.Name = "av1anPreset";
            this.av1anPreset.Size = new System.Drawing.Size(250, 23);
            this.av1anPreset.TabIndex = 47;
            this.toolTip.SetToolTip(this.av1anPreset, "This determines how fast or slow the encoding will be.\r\nThe slower it is, the bet" +
        "ter the video compression (quality per filesize/bitrate) is.");
            // 
            // av1anCodec
            // 
            this.av1anCodec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.av1anCodec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.av1anCodec.ForeColor = System.Drawing.Color.White;
            this.av1anCodec.FormattingEnabled = true;
            this.av1anCodec.Location = new System.Drawing.Point(220, 7);
            this.av1anCodec.Name = "av1anCodec";
            this.av1anCodec.Size = new System.Drawing.Size(250, 23);
            this.av1anCodec.TabIndex = 45;
            this.toolTip.SetToolTip(this.av1anCodec, "Select which video codec and encoder to use.");
            this.av1anCodec.SelectedIndexChanged += new System.EventHandler(this.av1anCodec_SelectedIndexChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(7, 100);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(77, 15);
            this.label20.TabIndex = 20;
            this.label20.Text = "Color Format";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(7, 70);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(82, 15);
            this.label22.TabIndex = 19;
            this.label22.Text = "Speed (Preset)";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.ForeColor = System.Drawing.Color.White;
            this.label23.Location = new System.Drawing.Point(7, 40);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(88, 15);
            this.label23.TabIndex = 18;
            this.label23.Text = "Quality Control";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.ForeColor = System.Drawing.Color.White;
            this.label24.Location = new System.Drawing.Point(5, 10);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(95, 15);
            this.label24.TabIndex = 17;
            this.label24.Text = "Codec (Encoder)";
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabPage4.Controls.Add(this.label26);
            this.tabPage4.Controls.Add(this.av1anAudChannels);
            this.tabPage4.Controls.Add(this.av1anAudQuality);
            this.tabPage4.Controls.Add(this.av1anAudCodec);
            this.tabPage4.Controls.Add(this.label28);
            this.tabPage4.Controls.Add(this.label31);
            this.tabPage4.Location = new System.Drawing.Point(4, 27);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(668, 318);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Audio";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.ForeColor = System.Drawing.Color.White;
            this.label26.Location = new System.Drawing.Point(7, 70);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(56, 15);
            this.label26.TabIndex = 53;
            this.label26.Text = "Channels";
            // 
            // av1anAudChannels
            // 
            this.av1anAudChannels.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anAudChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.av1anAudChannels.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.av1anAudChannels.ForeColor = System.Drawing.Color.White;
            this.av1anAudChannels.FormattingEnabled = true;
            this.av1anAudChannels.Items.AddRange(new object[] {
            "1 (Mono)",
            "2 (Stereo)",
            "6 (5.1)",
            "8 (7.1)"});
            this.av1anAudChannels.Location = new System.Drawing.Point(220, 67);
            this.av1anAudChannels.Name = "av1anAudChannels";
            this.av1anAudChannels.Size = new System.Drawing.Size(250, 23);
            this.av1anAudChannels.TabIndex = 52;
            // 
            // av1anAudQuality
            // 
            this.av1anAudQuality.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anAudQuality.ForeColor = System.Drawing.Color.White;
            this.av1anAudQuality.Location = new System.Drawing.Point(220, 38);
            this.av1anAudQuality.Maximum = new decimal(new int[] {
            6400,
            0,
            0,
            0});
            this.av1anAudQuality.Name = "av1anAudQuality";
            this.av1anAudQuality.Size = new System.Drawing.Size(250, 23);
            this.av1anAudQuality.TabIndex = 51;
            // 
            // av1anAudCodec
            // 
            this.av1anAudCodec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anAudCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.av1anAudCodec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.av1anAudCodec.ForeColor = System.Drawing.Color.White;
            this.av1anAudCodec.FormattingEnabled = true;
            this.av1anAudCodec.Location = new System.Drawing.Point(220, 7);
            this.av1anAudCodec.Name = "av1anAudCodec";
            this.av1anAudCodec.Size = new System.Drawing.Size(250, 23);
            this.av1anAudCodec.TabIndex = 49;
            this.av1anAudCodec.SelectedIndexChanged += new System.EventHandler(this.av1anAudCodec_SelectedIndexChanged);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.ForeColor = System.Drawing.Color.White;
            this.label28.Location = new System.Drawing.Point(7, 40);
            this.label28.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(132, 15);
            this.label28.TabIndex = 48;
            this.label28.Text = "Quality (Bitrate in Kbps)";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.ForeColor = System.Drawing.Color.White;
            this.label31.Location = new System.Drawing.Point(5, 10);
            this.label31.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(95, 15);
            this.label31.TabIndex = 47;
            this.label31.Text = "Codec (Encoder)";
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabPage5.Controls.Add(this.av1anOptsChunkOrder);
            this.tabPage5.Controls.Add(this.label47);
            this.tabPage5.Controls.Add(this.av1anResumeBtn);
            this.tabPage5.Controls.Add(this.label42);
            this.tabPage5.Controls.Add(this.av1anOptsConcatMode);
            this.tabPage5.Controls.Add(this.label41);
            this.tabPage5.Controls.Add(this.label8);
            this.tabPage5.Controls.Add(this.av1anOptsWorkerCount);
            this.tabPage5.Controls.Add(this.av1anCustomArgs);
            this.tabPage5.Controls.Add(this.label35);
            this.tabPage5.Controls.Add(this.av1anOptsChunkMode);
            this.tabPage5.Controls.Add(this.label34);
            this.tabPage5.Controls.Add(this.av1anOptsSplitMode);
            this.tabPage5.Controls.Add(this.label32);
            this.tabPage5.Location = new System.Drawing.Point(4, 27);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(668, 318);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "Av1an Options";
            // 
            // av1anResumeBtn
            // 
            this.av1anResumeBtn.AutoColor = true;
            this.av1anResumeBtn.ButtonImage = null;
            this.av1anResumeBtn.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.av1anResumeBtn.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(124)))), ((int)(((byte)(124)))));
            this.av1anResumeBtn.DrawImage = false;
            this.av1anResumeBtn.ForeColor = System.Drawing.Color.White;
            this.av1anResumeBtn.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(104)))), ((int)(((byte)(104)))));
            this.av1anResumeBtn.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.av1anResumeBtn.Location = new System.Drawing.Point(220, 186);
            this.av1anResumeBtn.Name = "av1anResumeBtn";
            this.av1anResumeBtn.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.av1anResumeBtn.Size = new System.Drawing.Size(250, 23);
            this.av1anResumeBtn.TabIndex = 61;
            this.av1anResumeBtn.Text = "Select Encode To Resume";
            this.av1anResumeBtn.Click += new System.EventHandler(this.av1anResumeBtn_Click);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.ForeColor = System.Drawing.Color.White;
            this.label42.Location = new System.Drawing.Point(5, 190);
            this.label42.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(91, 15);
            this.label42.TabIndex = 60;
            this.label42.Text = "Resume Encode";
            // 
            // av1anOptsConcatMode
            // 
            this.av1anOptsConcatMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anOptsConcatMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.av1anOptsConcatMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.av1anOptsConcatMode.ForeColor = System.Drawing.Color.White;
            this.av1anOptsConcatMode.FormattingEnabled = true;
            this.av1anOptsConcatMode.Items.AddRange(new object[] {
            "ffmpeg",
            "mkvmerge"});
            this.av1anOptsConcatMode.Location = new System.Drawing.Point(220, 67);
            this.av1anOptsConcatMode.Name = "av1anOptsConcatMode";
            this.av1anOptsConcatMode.Size = new System.Drawing.Size(250, 23);
            this.av1anOptsConcatMode.TabIndex = 59;
            this.toolTip.SetToolTip(this.av1anOptsConcatMode, "Select the chunk merging method. Use mkvmerge if ffmpeg doesn\'t work correctly.");
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.ForeColor = System.Drawing.Color.White;
            this.label41.Location = new System.Drawing.Point(5, 70);
            this.label41.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(135, 15);
            this.label41.TabIndex = 58;
            this.label41.Text = "Chunk Merging Method";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(5, 160);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 15);
            this.label8.TabIndex = 57;
            this.label8.Text = "Custom Arguments";
            // 
            // av1anOptsWorkerCount
            // 
            this.av1anOptsWorkerCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anOptsWorkerCount.ForeColor = System.Drawing.Color.White;
            this.av1anOptsWorkerCount.Location = new System.Drawing.Point(220, 130);
            this.av1anOptsWorkerCount.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.av1anOptsWorkerCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.av1anOptsWorkerCount.Name = "av1anOptsWorkerCount";
            this.av1anOptsWorkerCount.Size = new System.Drawing.Size(250, 23);
            this.av1anOptsWorkerCount.TabIndex = 53;
            this.toolTip.SetToolTip(this.av1anOptsWorkerCount, "Set the amount of av1an workers (encoder instances)");
            this.av1anOptsWorkerCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // av1anCustomArgs
            // 
            this.av1anCustomArgs.AllowDrop = true;
            this.av1anCustomArgs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anCustomArgs.ForeColor = System.Drawing.Color.White;
            this.av1anCustomArgs.Location = new System.Drawing.Point(220, 157);
            this.av1anCustomArgs.MinimumSize = new System.Drawing.Size(4, 21);
            this.av1anCustomArgs.Name = "av1anCustomArgs";
            this.av1anCustomArgs.Size = new System.Drawing.Size(442, 23);
            this.av1anCustomArgs.TabIndex = 56;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.ForeColor = System.Drawing.Color.White;
            this.label35.Location = new System.Drawing.Point(7, 130);
            this.label35.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(113, 15);
            this.label35.TabIndex = 50;
            this.label35.Text = "Amount Of Workers";
            // 
            // av1anOptsChunkMode
            // 
            this.av1anOptsChunkMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anOptsChunkMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.av1anOptsChunkMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.av1anOptsChunkMode.ForeColor = System.Drawing.Color.White;
            this.av1anOptsChunkMode.FormattingEnabled = true;
            this.av1anOptsChunkMode.Location = new System.Drawing.Point(220, 37);
            this.av1anOptsChunkMode.Name = "av1anOptsChunkMode";
            this.av1anOptsChunkMode.Size = new System.Drawing.Size(250, 23);
            this.av1anOptsChunkMode.TabIndex = 49;
            this.toolTip.SetToolTip(this.av1anOptsChunkMode, "Select the chunk generation method. If you are unsure, use lsmash.");
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.ForeColor = System.Drawing.Color.White;
            this.label34.Location = new System.Drawing.Point(5, 40);
            this.label34.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(148, 15);
            this.label34.TabIndex = 48;
            this.label34.Text = "Chunk Generation Method";
            // 
            // av1anOptsSplitMode
            // 
            this.av1anOptsSplitMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anOptsSplitMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.av1anOptsSplitMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.av1anOptsSplitMode.ForeColor = System.Drawing.Color.White;
            this.av1anOptsSplitMode.FormattingEnabled = true;
            this.av1anOptsSplitMode.Items.AddRange(new object[] {
            "None",
            "Scene Detection"});
            this.av1anOptsSplitMode.Location = new System.Drawing.Point(220, 7);
            this.av1anOptsSplitMode.Name = "av1anOptsSplitMode";
            this.av1anOptsSplitMode.Size = new System.Drawing.Size(250, 23);
            this.av1anOptsSplitMode.TabIndex = 47;
            this.toolTip.SetToolTip(this.av1anOptsSplitMode, "Set how to split the video into chunks");
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.ForeColor = System.Drawing.Color.White;
            this.label32.Location = new System.Drawing.Point(5, 10);
            this.label32.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(96, 15);
            this.label32.TabIndex = 46;
            this.label32.Text = "Splitting Method";
            // 
            // utilsPage
            // 
            this.utilsPage.Controls.Add(this.tableLayoutPanel2);
            this.utilsPage.Name = "utilsPage";
            this.utilsPage.Size = new System.Drawing.Size(682, 382);
            this.utilsPage.Text = "Utilities";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.utilsBitratesPanel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.utilsMetricsPanel, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.utilsOcrPanel, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.utilsColorDataPanel, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.utilsConcatPanel, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.utilsBitratePlotPanel, 2, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(682, 382);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // utilsBitratesPanel
            // 
            this.utilsBitratesPanel.Controls.Add(this.label5);
            this.utilsBitratesPanel.Controls.Add(this.utilsBitratesSelBtn);
            this.utilsBitratesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.utilsBitratesPanel.Location = new System.Drawing.Point(0, 0);
            this.utilsBitratesPanel.Margin = new System.Windows.Forms.Padding(0);
            this.utilsBitratesPanel.Name = "utilsBitratesPanel";
            this.utilsBitratesPanel.Size = new System.Drawing.Size(227, 127);
            this.utilsBitratesPanel.TabIndex = 3;
            this.utilsBitratesPanel.Click += new System.EventHandler(this.SelectReadBitrates);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(4, 4);
            this.label5.Margin = new System.Windows.Forms.Padding(4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(219, 20);
            this.label5.TabIndex = 17;
            this.label5.Text = "Read Bitrates And Stream Sizes";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // utilsBitratesSelBtn
            // 
            this.utilsBitratesSelBtn.AutoColor = false;
            this.utilsBitratesSelBtn.ButtonImage = global::Nmkoder.Properties.Resources.icon_analyze;
            this.utilsBitratesSelBtn.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.utilsBitratesSelBtn.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.utilsBitratesSelBtn.DrawImage = true;
            this.utilsBitratesSelBtn.Enabled = false;
            this.utilsBitratesSelBtn.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.utilsBitratesSelBtn.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.Zoom;
            this.utilsBitratesSelBtn.Location = new System.Drawing.Point(70, 31);
            this.utilsBitratesSelBtn.Margin = new System.Windows.Forms.Padding(70, 3, 70, 3);
            this.utilsBitratesSelBtn.Name = "utilsBitratesSelBtn";
            this.utilsBitratesSelBtn.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.utilsBitratesSelBtn.Size = new System.Drawing.Size(87, 65);
            this.utilsBitratesSelBtn.TabIndex = 0;
            // 
            // utilsMetricsPanel
            // 
            this.utilsMetricsPanel.Controls.Add(this.utilsMetricsConfBtn);
            this.utilsMetricsPanel.Controls.Add(this.label1);
            this.utilsMetricsPanel.Controls.Add(this.utilsMetricsSelBtn);
            this.utilsMetricsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.utilsMetricsPanel.Location = new System.Drawing.Point(227, 0);
            this.utilsMetricsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.utilsMetricsPanel.Name = "utilsMetricsPanel";
            this.utilsMetricsPanel.Size = new System.Drawing.Size(227, 127);
            this.utilsMetricsPanel.TabIndex = 4;
            this.utilsMetricsPanel.Click += new System.EventHandler(this.SelectGetMetrics);
            // 
            // utilsMetricsConfBtn
            // 
            this.utilsMetricsConfBtn.AutoColor = true;
            this.utilsMetricsConfBtn.ButtonImage = null;
            this.utilsMetricsConfBtn.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.utilsMetricsConfBtn.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.utilsMetricsConfBtn.DrawImage = false;
            this.utilsMetricsConfBtn.ForeColor = System.Drawing.Color.White;
            this.utilsMetricsConfBtn.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.utilsMetricsConfBtn.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.utilsMetricsConfBtn.Location = new System.Drawing.Point(50, 100);
            this.utilsMetricsConfBtn.Margin = new System.Windows.Forms.Padding(50, 3, 50, 3);
            this.utilsMetricsConfBtn.Name = "utilsMetricsConfBtn";
            this.utilsMetricsConfBtn.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.utilsMetricsConfBtn.Size = new System.Drawing.Size(127, 23);
            this.utilsMetricsConfBtn.TabIndex = 18;
            this.utilsMetricsConfBtn.Text = "Configure";
            this.utilsMetricsConfBtn.Click += new System.EventHandler(this.utilsMetricsConfBtn_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 20);
            this.label1.TabIndex = 17;
            this.label1.Text = "Get Metrics (VMAF, SSIM, PSNR)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // utilsMetricsSelBtn
            // 
            this.utilsMetricsSelBtn.AutoColor = false;
            this.utilsMetricsSelBtn.ButtonImage = global::Nmkoder.Properties.Resources.icon_metrics;
            this.utilsMetricsSelBtn.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.utilsMetricsSelBtn.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.utilsMetricsSelBtn.DrawImage = true;
            this.utilsMetricsSelBtn.Enabled = false;
            this.utilsMetricsSelBtn.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.utilsMetricsSelBtn.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.Zoom;
            this.utilsMetricsSelBtn.Location = new System.Drawing.Point(70, 31);
            this.utilsMetricsSelBtn.Margin = new System.Windows.Forms.Padding(70, 3, 70, 3);
            this.utilsMetricsSelBtn.Name = "utilsMetricsSelBtn";
            this.utilsMetricsSelBtn.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.utilsMetricsSelBtn.Size = new System.Drawing.Size(87, 65);
            this.utilsMetricsSelBtn.TabIndex = 0;
            // 
            // utilsOcrPanel
            // 
            this.utilsOcrPanel.Controls.Add(this.label2);
            this.utilsOcrPanel.Controls.Add(this.htButton2);
            this.utilsOcrPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.utilsOcrPanel.Location = new System.Drawing.Point(454, 0);
            this.utilsOcrPanel.Margin = new System.Windows.Forms.Padding(0);
            this.utilsOcrPanel.Name = "utilsOcrPanel";
            this.utilsOcrPanel.Size = new System.Drawing.Size(228, 127);
            this.utilsOcrPanel.TabIndex = 5;
            this.utilsOcrPanel.Click += new System.EventHandler(this.SelectOcr);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(4, 4);
            this.label2.Margin = new System.Windows.Forms.Padding(4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(219, 20);
            this.label2.TabIndex = 17;
            this.label2.Text = "Convert Bitmap Subtitles To Text";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // htButton2
            // 
            this.htButton2.AutoColor = false;
            this.htButton2.ButtonImage = global::Nmkoder.Properties.Resources.icon_subs;
            this.htButton2.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.htButton2.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.htButton2.DrawImage = true;
            this.htButton2.Enabled = false;
            this.htButton2.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.htButton2.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.Zoom;
            this.htButton2.Location = new System.Drawing.Point(70, 31);
            this.htButton2.Margin = new System.Windows.Forms.Padding(70, 3, 70, 3);
            this.htButton2.Name = "htButton2";
            this.htButton2.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.htButton2.Size = new System.Drawing.Size(87, 65);
            this.htButton2.TabIndex = 0;
            // 
            // utilsColorDataPanel
            // 
            this.utilsColorDataPanel.Controls.Add(this.utilsColorDataConfBtn);
            this.utilsColorDataPanel.Controls.Add(this.label40);
            this.utilsColorDataPanel.Controls.Add(this.htButton3);
            this.utilsColorDataPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.utilsColorDataPanel.Location = new System.Drawing.Point(0, 127);
            this.utilsColorDataPanel.Margin = new System.Windows.Forms.Padding(0);
            this.utilsColorDataPanel.Name = "utilsColorDataPanel";
            this.utilsColorDataPanel.Size = new System.Drawing.Size(227, 127);
            this.utilsColorDataPanel.TabIndex = 6;
            this.utilsColorDataPanel.Click += new System.EventHandler(this.SelectColorData);
            // 
            // utilsColorDataConfBtn
            // 
            this.utilsColorDataConfBtn.AutoColor = true;
            this.utilsColorDataConfBtn.ButtonImage = null;
            this.utilsColorDataConfBtn.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.utilsColorDataConfBtn.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.utilsColorDataConfBtn.DrawImage = false;
            this.utilsColorDataConfBtn.ForeColor = System.Drawing.Color.White;
            this.utilsColorDataConfBtn.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.utilsColorDataConfBtn.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.utilsColorDataConfBtn.Location = new System.Drawing.Point(50, 100);
            this.utilsColorDataConfBtn.Margin = new System.Windows.Forms.Padding(50, 3, 50, 3);
            this.utilsColorDataConfBtn.Name = "utilsColorDataConfBtn";
            this.utilsColorDataConfBtn.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.utilsColorDataConfBtn.Size = new System.Drawing.Size(127, 23);
            this.utilsColorDataConfBtn.TabIndex = 18;
            this.utilsColorDataConfBtn.Text = "Configure";
            this.utilsColorDataConfBtn.Click += new System.EventHandler(this.utilsColorDataConfBtn_Click);
            // 
            // label40
            // 
            this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label40.ForeColor = System.Drawing.Color.White;
            this.label40.Location = new System.Drawing.Point(4, 4);
            this.label40.Margin = new System.Windows.Forms.Padding(4);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(219, 20);
            this.label40.TabIndex = 17;
            this.label40.Text = "Transfer Color/HDR Metadata";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // htButton3
            // 
            this.htButton3.AutoColor = false;
            this.htButton3.ButtonImage = global::Nmkoder.Properties.Resources.icon_videocolor;
            this.htButton3.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.htButton3.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.htButton3.DrawImage = true;
            this.htButton3.Enabled = false;
            this.htButton3.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.htButton3.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.Zoom;
            this.htButton3.Location = new System.Drawing.Point(70, 31);
            this.htButton3.Margin = new System.Windows.Forms.Padding(70, 3, 70, 3);
            this.htButton3.Name = "htButton3";
            this.htButton3.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.htButton3.Size = new System.Drawing.Size(87, 65);
            this.htButton3.TabIndex = 0;
            // 
            // utilsConcatPanel
            // 
            this.utilsConcatPanel.Controls.Add(this.label44);
            this.utilsConcatPanel.Controls.Add(this.htButton1);
            this.utilsConcatPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.utilsConcatPanel.Location = new System.Drawing.Point(227, 127);
            this.utilsConcatPanel.Margin = new System.Windows.Forms.Padding(0);
            this.utilsConcatPanel.Name = "utilsConcatPanel";
            this.utilsConcatPanel.Size = new System.Drawing.Size(227, 127);
            this.utilsConcatPanel.TabIndex = 7;
            this.utilsConcatPanel.Click += new System.EventHandler(this.SelectConcat);
            // 
            // label44
            // 
            this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label44.ForeColor = System.Drawing.Color.White;
            this.label44.Location = new System.Drawing.Point(4, 4);
            this.label44.Margin = new System.Windows.Forms.Padding(4);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(219, 20);
            this.label44.TabIndex = 17;
            this.label44.Text = "Concatenate Into Single MKV";
            this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // htButton1
            // 
            this.htButton1.AutoColor = false;
            this.htButton1.ButtonImage = global::Nmkoder.Properties.Resources.icon_concat;
            this.htButton1.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.htButton1.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.htButton1.DrawImage = true;
            this.htButton1.Enabled = false;
            this.htButton1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.htButton1.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.Zoom;
            this.htButton1.Location = new System.Drawing.Point(70, 31);
            this.htButton1.Margin = new System.Windows.Forms.Padding(70, 3, 70, 3);
            this.htButton1.Name = "htButton1";
            this.htButton1.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.htButton1.Size = new System.Drawing.Size(87, 65);
            this.htButton1.TabIndex = 0;
            // 
            // utilsBitratePlotPanel
            // 
            this.utilsBitratePlotPanel.Controls.Add(this.label45);
            this.utilsBitratePlotPanel.Controls.Add(this.htButton4);
            this.utilsBitratePlotPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.utilsBitratePlotPanel.Location = new System.Drawing.Point(454, 127);
            this.utilsBitratePlotPanel.Margin = new System.Windows.Forms.Padding(0);
            this.utilsBitratePlotPanel.Name = "utilsBitratePlotPanel";
            this.utilsBitratePlotPanel.Size = new System.Drawing.Size(228, 127);
            this.utilsBitratePlotPanel.TabIndex = 8;
            this.utilsBitratePlotPanel.Click += new System.EventHandler(this.SelectBitratePlotUtil);
            // 
            // label45
            // 
            this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label45.ForeColor = System.Drawing.Color.White;
            this.label45.Location = new System.Drawing.Point(4, 4);
            this.label45.Margin = new System.Windows.Forms.Padding(4);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(219, 20);
            this.label45.TabIndex = 17;
            this.label45.Text = "Show Bitrate Chart For Video";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // htButton4
            // 
            this.htButton4.AutoColor = false;
            this.htButton4.ButtonImage = global::Nmkoder.Properties.Resources.icon_plot;
            this.htButton4.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.htButton4.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.htButton4.DrawImage = true;
            this.htButton4.Enabled = false;
            this.htButton4.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.htButton4.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.Zoom;
            this.htButton4.Location = new System.Drawing.Point(70, 31);
            this.htButton4.Margin = new System.Windows.Forms.Padding(70, 3, 70, 3);
            this.htButton4.Name = "htButton4";
            this.htButton4.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.htButton4.Size = new System.Drawing.Size(87, 65);
            this.htButton4.TabIndex = 0;
            // 
            // settingsPage
            // 
            this.settingsPage.Controls.Add(this.htTabControl1);
            this.settingsPage.Name = "settingsPage";
            this.settingsPage.Size = new System.Drawing.Size(682, 382);
            this.settingsPage.Text = "Settings";
            // 
            // htTabControl1
            // 
            this.htTabControl1.AllowDrop = true;
            this.htTabControl1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.htTabControl1.BorderTabLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.htTabControl1.Controls.Add(this.settingsGeneralTab);
            this.htTabControl1.Controls.Add(this.settingsContainersTab);
            this.htTabControl1.DisableClose = true;
            this.htTabControl1.DisableDragging = true;
            this.htTabControl1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.htTabControl1.HoverTabButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(176)))), ((int)(((byte)(239)))));
            this.htTabControl1.HoverTabColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(151)))), ((int)(((byte)(234)))));
            this.htTabControl1.HoverUnselectedTabButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.htTabControl1.Location = new System.Drawing.Point(3, 3);
            this.htTabControl1.Name = "htTabControl1";
            this.htTabControl1.Padding = new System.Drawing.Point(14, 4);
            this.htTabControl1.SelectedIndex = 0;
            this.htTabControl1.SelectedTabButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(151)))), ((int)(((byte)(234)))));
            this.htTabControl1.SelectedTabColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.htTabControl1.Size = new System.Drawing.Size(676, 319);
            this.htTabControl1.TabIndex = 1;
            this.htTabControl1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.htTabControl1.UnderBorderTabLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(70)))));
            this.htTabControl1.UnselectedBorderTabLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.htTabControl1.UnselectedTabColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.htTabControl1.UpDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.htTabControl1.UpDownTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(112)))));
            // 
            // settingsGeneralTab
            // 
            this.settingsGeneralTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.settingsGeneralTab.Controls.Add(this.comboBox4);
            this.settingsGeneralTab.Controls.Add(this.label43);
            this.settingsGeneralTab.Location = new System.Drawing.Point(4, 27);
            this.settingsGeneralTab.Name = "settingsGeneralTab";
            this.settingsGeneralTab.Padding = new System.Windows.Forms.Padding(3);
            this.settingsGeneralTab.Size = new System.Drawing.Size(668, 288);
            this.settingsGeneralTab.TabIndex = 0;
            this.settingsGeneralTab.Text = "General";
            // 
            // comboBox4
            // 
            this.comboBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox4.ForeColor = System.Drawing.Color.White;
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(220, 7);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(250, 23);
            this.comboBox4.TabIndex = 45;
            this.comboBox4.Visible = false;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.ForeColor = System.Drawing.Color.White;
            this.label43.Location = new System.Drawing.Point(5, 10);
            this.label43.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(95, 15);
            this.label43.TabIndex = 17;
            this.label43.Text = "Codec (Encoder)";
            this.label43.Visible = false;
            // 
            // settingsContainersTab
            // 
            this.settingsContainersTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.settingsContainersTab.Controls.Add(this.mp4Faststart);
            this.settingsContainersTab.Controls.Add(this.label64);
            this.settingsContainersTab.Location = new System.Drawing.Point(4, 27);
            this.settingsContainersTab.Name = "settingsContainersTab";
            this.settingsContainersTab.Padding = new System.Windows.Forms.Padding(3);
            this.settingsContainersTab.Size = new System.Drawing.Size(668, 288);
            this.settingsContainersTab.TabIndex = 1;
            this.settingsContainersTab.Text = "Containers";
            // 
            // mp4Faststart
            // 
            this.mp4Faststart.AutoSize = true;
            this.mp4Faststart.Location = new System.Drawing.Point(220, 11);
            this.mp4Faststart.Name = "mp4Faststart";
            this.mp4Faststart.Size = new System.Drawing.Size(15, 14);
            this.mp4Faststart.TabIndex = 50;
            this.mp4Faststart.UseVisualStyleBackColor = true;
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.ForeColor = System.Drawing.Color.White;
            this.label64.Location = new System.Drawing.Point(5, 10);
            this.label64.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(119, 15);
            this.label64.TabIndex = 47;
            this.label64.Text = "MP4: Web Optimized";
            // 
            // progressCircle
            // 
            this.progressCircle.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.Liner;
            this.progressCircle.AnimationSpeed = 500;
            this.progressCircle.BackColor = System.Drawing.Color.Transparent;
            this.progressCircle.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold);
            this.progressCircle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.progressCircle.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.progressCircle.InnerMargin = 2;
            this.progressCircle.InnerWidth = -1;
            this.progressCircle.Location = new System.Drawing.Point(174, 9);
            this.progressCircle.MarqueeAnimationSpeed = 2000;
            this.progressCircle.Name = "progressCircle";
            this.progressCircle.OuterColor = System.Drawing.Color.Gray;
            this.progressCircle.OuterMargin = -21;
            this.progressCircle.OuterWidth = 21;
            this.progressCircle.ProgressColor = System.Drawing.Color.LimeGreen;
            this.progressCircle.ProgressWidth = 8;
            this.progressCircle.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 36F);
            this.progressCircle.Size = new System.Drawing.Size(40, 40);
            this.progressCircle.StartAngle = 270;
            this.progressCircle.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressCircle.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.progressCircle.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.progressCircle.SubscriptText = ".23";
            this.progressCircle.SuperscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.progressCircle.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
            this.progressCircle.SuperscriptText = "°C";
            this.progressCircle.TabIndex = 38;
            this.progressCircle.TextMargin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.progressCircle.Value = 33;
            this.progressCircle.Visible = false;
            // 
            // busyControlsPanel
            // 
            this.busyControlsPanel.Controls.Add(this.pauseBtn);
            this.busyControlsPanel.Controls.Add(this.stopBtn);
            this.busyControlsPanel.Location = new System.Drawing.Point(12, 544);
            this.busyControlsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.busyControlsPanel.Name = "busyControlsPanel";
            this.busyControlsPanel.Size = new System.Drawing.Size(314, 45);
            this.busyControlsPanel.TabIndex = 39;
            this.busyControlsPanel.Visible = false;
            // 
            // pauseBtn
            // 
            this.pauseBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.pauseBtn.BackgroundImage = global::Nmkoder.Properties.Resources.baseline_pause_white_48dp;
            this.pauseBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pauseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pauseBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pauseBtn.ForeColor = System.Drawing.Color.White;
            this.pauseBtn.Location = new System.Drawing.Point(0, 0);
            this.pauseBtn.Name = "pauseBtn";
            this.pauseBtn.Size = new System.Drawing.Size(45, 45);
            this.pauseBtn.TabIndex = 37;
            this.pauseBtn.UseVisualStyleBackColor = false;
            this.pauseBtn.Click += new System.EventHandler(this.pauseBtn_Click);
            // 
            // stopBtn
            // 
            this.stopBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.stopBtn.BackgroundImage = global::Nmkoder.Properties.Resources.baseline_stop_white_48dp;
            this.stopBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.stopBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stopBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopBtn.ForeColor = System.Drawing.Color.White;
            this.stopBtn.Location = new System.Drawing.Point(51, 0);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(45, 45);
            this.stopBtn.TabIndex = 36;
            this.stopBtn.UseVisualStyleBackColor = false;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // currentActionLabel
            // 
            this.currentActionLabel.AutoSize = true;
            this.currentActionLabel.ForeColor = System.Drawing.Color.White;
            this.currentActionLabel.Location = new System.Drawing.Point(13, 524);
            this.currentActionLabel.Margin = new System.Windows.Forms.Padding(4);
            this.currentActionLabel.Name = "currentActionLabel";
            this.currentActionLabel.Size = new System.Drawing.Size(0, 13);
            this.currentActionLabel.TabIndex = 40;
            // 
            // checkItemsContextMenu
            // 
            this.checkItemsContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.checkItemsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkAllToolStripMenuItem,
            this.checkNoneToolStripMenuItem,
            this.invertSelectionToolStripMenuItem,
            this.checkAllVideoTracksToolStripMenuItem,
            this.checkAllAudioTracksToolStripMenuItem,
            this.checkAllSubtitleTracksToolStripMenuItem});
            this.checkItemsContextMenu.Name = "checkItemsContextMenu";
            this.checkItemsContextMenu.Size = new System.Drawing.Size(203, 136);
            // 
            // checkAllToolStripMenuItem
            // 
            this.checkAllToolStripMenuItem.Name = "checkAllToolStripMenuItem";
            this.checkAllToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.checkAllToolStripMenuItem.Text = "Check All";
            this.checkAllToolStripMenuItem.Click += new System.EventHandler(this.checkAllToolStripMenuItem_Click);
            // 
            // checkNoneToolStripMenuItem
            // 
            this.checkNoneToolStripMenuItem.Name = "checkNoneToolStripMenuItem";
            this.checkNoneToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.checkNoneToolStripMenuItem.Text = "Check None";
            this.checkNoneToolStripMenuItem.Click += new System.EventHandler(this.checkNoneToolStripMenuItem_Click);
            // 
            // invertSelectionToolStripMenuItem
            // 
            this.invertSelectionToolStripMenuItem.Name = "invertSelectionToolStripMenuItem";
            this.invertSelectionToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.invertSelectionToolStripMenuItem.Text = "Invert Selection";
            this.invertSelectionToolStripMenuItem.Click += new System.EventHandler(this.invertSelectionToolStripMenuItem_Click);
            // 
            // checkAllVideoTracksToolStripMenuItem
            // 
            this.checkAllVideoTracksToolStripMenuItem.Name = "checkAllVideoTracksToolStripMenuItem";
            this.checkAllVideoTracksToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.checkAllVideoTracksToolStripMenuItem.Text = "Check All Video Tracks";
            this.checkAllVideoTracksToolStripMenuItem.Click += new System.EventHandler(this.checkAllVideoTracksToolStripMenuItem_Click);
            // 
            // checkAllAudioTracksToolStripMenuItem
            // 
            this.checkAllAudioTracksToolStripMenuItem.Name = "checkAllAudioTracksToolStripMenuItem";
            this.checkAllAudioTracksToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.checkAllAudioTracksToolStripMenuItem.Text = "Check All Audio Tracks";
            this.checkAllAudioTracksToolStripMenuItem.Click += new System.EventHandler(this.checkAllAudioTracksToolStripMenuItem_Click);
            // 
            // checkAllSubtitleTracksToolStripMenuItem
            // 
            this.checkAllSubtitleTracksToolStripMenuItem.Name = "checkAllSubtitleTracksToolStripMenuItem";
            this.checkAllSubtitleTracksToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.checkAllSubtitleTracksToolStripMenuItem.Text = "Check All Subtitle Tracks";
            this.checkAllSubtitleTracksToolStripMenuItem.Click += new System.EventHandler(this.checkAllSubtitleTracksToolStripMenuItem_Click);
            // 
            // sortFileListContextMenu
            // 
            this.sortFileListContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.sortFileListContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortMenuAbcDesc,
            this.sortMenuAbcAsc,
            this.sortMenuSizeDesc,
            this.sortMenuSizeAsc,
            this.sortMenuRecentDesc,
            this.sortMenuRecentAsc});
            this.sortFileListContextMenu.Name = "checkItemsContextMenu";
            this.sortFileListContextMenu.Size = new System.Drawing.Size(262, 136);
            // 
            // sortMenuAbcDesc
            // 
            this.sortMenuAbcDesc.Name = "sortMenuAbcDesc";
            this.sortMenuAbcDesc.Size = new System.Drawing.Size(261, 22);
            this.sortMenuAbcDesc.Text = "Sort By Filename (A-Z)";
            this.sortMenuAbcDesc.Click += new System.EventHandler(this.sortMenuAbcDesc_Click);
            // 
            // sortMenuAbcAsc
            // 
            this.sortMenuAbcAsc.Name = "sortMenuAbcAsc";
            this.sortMenuAbcAsc.Size = new System.Drawing.Size(261, 22);
            this.sortMenuAbcAsc.Text = "Sort By Filename (Z-A)";
            this.sortMenuAbcAsc.Click += new System.EventHandler(this.sortMenuAbcAsc_Click);
            // 
            // sortMenuSizeDesc
            // 
            this.sortMenuSizeDesc.Name = "sortMenuSizeDesc";
            this.sortMenuSizeDesc.Size = new System.Drawing.Size(261, 22);
            this.sortMenuSizeDesc.Text = "Sort By Filesize (Large To Small)";
            this.sortMenuSizeDesc.Click += new System.EventHandler(this.sortMenuSizeDesc_Click);
            // 
            // sortMenuSizeAsc
            // 
            this.sortMenuSizeAsc.Name = "sortMenuSizeAsc";
            this.sortMenuSizeAsc.Size = new System.Drawing.Size(261, 22);
            this.sortMenuSizeAsc.Text = "Sort By Filesize (Small To Large)";
            this.sortMenuSizeAsc.Click += new System.EventHandler(this.sortMenuSizeAsc_Click);
            // 
            // sortMenuRecentDesc
            // 
            this.sortMenuRecentDesc.Name = "sortMenuRecentDesc";
            this.sortMenuRecentDesc.Size = new System.Drawing.Size(261, 22);
            this.sortMenuRecentDesc.Text = "Sort By Modified Date (New To Old)";
            this.sortMenuRecentDesc.Click += new System.EventHandler(this.sortMenuRecentDesc_Click);
            // 
            // sortMenuRecentAsc
            // 
            this.sortMenuRecentAsc.Name = "sortMenuRecentAsc";
            this.sortMenuRecentAsc.Size = new System.Drawing.Size(261, 22);
            this.sortMenuRecentAsc.Text = "Sort By Modified Date (Old To New)";
            this.sortMenuRecentAsc.Click += new System.EventHandler(this.sortMenuRecentAsc_Click);
            // 
            // runBtn
            // 
            this.runBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.runBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.runBtn.Enabled = false;
            this.runBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.runBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runBtn.ForeColor = System.Drawing.Color.White;
            this.runBtn.Image = ((System.Drawing.Image)(resources.GetObject("runBtn.Image")));
            this.runBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.runBtn.Location = new System.Drawing.Point(12, 544);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(314, 45);
            this.runBtn.TabIndex = 13;
            this.runBtn.Text = "Start";
            this.runBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.runBtn.UseVisualStyleBackColor = false;
            this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.ForeColor = System.Drawing.Color.White;
            this.label47.Location = new System.Drawing.Point(7, 100);
            this.label47.Margin = new System.Windows.Forms.Padding(4, 4, 4, 11);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(128, 15);
            this.label47.TabIndex = 62;
            this.label47.Text = "Chunk Encoding Order";
            // 
            // av1anOptsChunkOrder
            // 
            this.av1anOptsChunkOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.av1anOptsChunkOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.av1anOptsChunkOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.av1anOptsChunkOrder.ForeColor = System.Drawing.Color.White;
            this.av1anOptsChunkOrder.FormattingEnabled = true;
            this.av1anOptsChunkOrder.Items.AddRange(new object[] {
            "Long-to-Short (Best CPU utilization)",
            "Short-to-Long (Not very useful)",
            "Sequential (Same order as input video)",
            "Random (Most accurate filesize estimation)"});
            this.av1anOptsChunkOrder.Location = new System.Drawing.Point(220, 97);
            this.av1anOptsChunkOrder.Name = "av1anOptsChunkOrder";
            this.av1anOptsChunkOrder.Size = new System.Drawing.Size(250, 23);
            this.av1anOptsChunkOrder.TabIndex = 63;
            this.toolTip.SetToolTip(this.av1anOptsChunkOrder, "Select the chunk merging method. Use mkvmerge if ffmpeg doesn\'t work correctly.");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(1184, 601);
            this.Controls.Add(this.currentActionLabel);
            this.Controls.Add(this.busyControlsPanel);
            this.Controls.Add(this.progressCircle);
            this.Controls.Add(this.tabList);
            this.Controls.Add(this.progBar);
            this.Controls.Add(this.runBtn);
            this.Controls.Add(this.inputPanel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.logTbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nmkoder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.inputPanel.ResumeLayout(false);
            this.inputPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thumbnail)).EndInit();
            this.tabList.ResumeLayout(false);
            this.fileListPage.ResumeLayout(false);
            this.fileListPage.PerformLayout();
            this.streamListPage.ResumeLayout(false);
            this.streamListPage.PerformLayout();
            this.quickConvertPage.ResumeLayout(false);
            this.quickConvertPage.PerformLayout();
            this.quickEncTabControl.ResumeLayout(false);
            this.encVid.ResumeLayout(false);
            this.encVid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.encVidQuality)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.encAudPerTrackPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.encAudQuality)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.encMetaTab.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.metadataGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.av1anPage.ResumeLayout(false);
            this.av1anPage.PerformLayout();
            this.av1anTabControl.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.av1anThreads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.av1anGrainSynthStrength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.av1anQuality)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.av1anAudQuality)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.av1anOptsWorkerCount)).EndInit();
            this.utilsPage.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.utilsBitratesPanel.ResumeLayout(false);
            this.utilsMetricsPanel.ResumeLayout(false);
            this.utilsOcrPanel.ResumeLayout(false);
            this.utilsColorDataPanel.ResumeLayout(false);
            this.utilsConcatPanel.ResumeLayout(false);
            this.utilsBitratePlotPanel.ResumeLayout(false);
            this.settingsPage.ResumeLayout(false);
            this.htTabControl1.ResumeLayout(false);
            this.settingsGeneralTab.ResumeLayout(false);
            this.settingsGeneralTab.PerformLayout();
            this.settingsContainersTab.ResumeLayout(false);
            this.settingsContainersTab.PerformLayout();
            this.busyControlsPanel.ResumeLayout(false);
            this.checkItemsContextMenu.ResumeLayout(false);
            this.sortFileListContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox logTbox;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Panel inputPanel;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Panel inputDropPanel;
        private System.Windows.Forms.Button runBtn;
        private HTAlt.WinForms.HTProgressBar progBar;
        private System.Windows.Forms.ComboBox taskMode;
        private System.Windows.Forms.Label formatInfo;
        private Cyotek.Windows.Forms.TabList tabList;
        private Cyotek.Windows.Forms.TabListPage streamListPage;
        private Cyotek.Windows.Forms.TabListPage quickConvertPage;
        private Cyotek.Windows.Forms.TabListPage utilsPage;
        private System.Windows.Forms.PictureBox thumbnail;
        private System.Windows.Forms.TextBox streamDetails;
        private HTAlt.WinForms.HTTabControl quickEncTabControl;
        private System.Windows.Forms.TabPage encVid;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox outputPath;
        private System.Windows.Forms.ComboBox containers;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TextBox encVidFps;
        private System.Windows.Forms.ComboBox encVidColors;
        private System.Windows.Forms.ComboBox encVidPreset;
        private System.Windows.Forms.ComboBox encVidCodec;
        private System.Windows.Forms.ComboBox encAudCodec;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.TextBox encCustomArgsIn;
        private System.Windows.Forms.NumericUpDown encAudQuality;
        private System.Windows.Forms.NumericUpDown encVidQuality;
        private System.Windows.Forms.ComboBox encSubCodec;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label thumbInfo;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.Button pauseBtn;
        private CircularProgressBar.CircularProgressBar progressCircle;
        private System.Windows.Forms.Panel busyControlsPanel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox encAudChannels;
        private System.Windows.Forms.TabPage encMetaTab;
        private System.Windows.Forms.DataGridView metadataGrid;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ComboBox metaMode;
        private System.Windows.Forms.ComboBox encSubBurn;
        private System.Windows.Forms.Label label25;
        private Cyotek.Windows.Forms.TabListPage fileListPage;
        private System.Windows.Forms.Button fileListMoveUpBtn;
        private System.Windows.Forms.Button fileListRemoveBtn;
        private System.Windows.Forms.Button fileListMoveDownBtn;
        private System.Windows.Forms.Button addTracksFromFileBtn;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.ComboBox encCropMode;
        private System.Windows.Forms.Label label27;
        private Cyotek.Windows.Forms.TabListPage settingsPage;
        private System.Windows.Forms.TextBox encScaleH;
        private System.Windows.Forms.TextBox encScaleW;
        private System.Windows.Forms.ComboBox fileListMode;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TextBox encCustomArgsOut;
        private HTAlt.WinForms.HTTabControl htTabControl1;
        private System.Windows.Forms.TabPage settingsGeneralTab;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.TabPage settingsContainersTab;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.CheckBox mp4Faststart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label currentActionLabel;
        private System.Windows.Forms.Label qInfo;
        private System.Windows.Forms.Label presetInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel utilsBitratesPanel;
        private System.Windows.Forms.Label label5;
        private HTAlt.WinForms.HTButton utilsBitratesSelBtn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox trackListDefaultAudio;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox trackListDefaultSubs;
        private Cyotek.Windows.Forms.TabListPage av1anPage;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox av1anOutputPath;
        private System.Windows.Forms.TextBox av1anCustomArgs;
        private System.Windows.Forms.ComboBox av1anContainer;
        private HTAlt.WinForms.HTTabControl av1anTabControl;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox av1anScaleH;
        private System.Windows.Forms.TextBox av1anScaleW;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox av1anCrop;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown av1anQuality;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox av1anColorSpace;
        private System.Windows.Forms.ComboBox av1anPreset;
        private System.Windows.Forms.ComboBox av1anCodec;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.ComboBox av1anAudChannels;
        private System.Windows.Forms.NumericUpDown av1anAudQuality;
        private System.Windows.Forms.ComboBox av1anAudCodec;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.ComboBox av1anQualityMode;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox av1anFps;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.ComboBox av1anOptsChunkMode;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.ComboBox av1anOptsSplitMode;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.NumericUpDown av1anOptsWorkerCount;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.TextBox av1anCustomEncArgs;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.CheckBox av1anGrainSynthDenoise;
        private System.Windows.Forms.NumericUpDown av1anGrainSynthStrength;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Button trackListMoveUpBtn;
        private System.Windows.Forms.Button trackListMoveDownBtn;
        private System.Windows.Forms.Panel utilsMetricsPanel;
        private System.Windows.Forms.Label label1;
        private HTAlt.WinForms.HTButton utilsMetricsSelBtn;
        private HTAlt.WinForms.HTButton utilsMetricsConfBtn;
        private System.Windows.Forms.ComboBox encQualMode;
        private System.Windows.Forms.Panel utilsOcrPanel;
        private System.Windows.Forms.Label label2;
        private HTAlt.WinForms.HTButton htButton2;
        private System.Windows.Forms.Button trackListCheckTracksBtn;
        private System.Windows.Forms.ContextMenuStrip checkItemsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem checkAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkNoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invertSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkAllVideoTracksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkAllAudioTracksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkAllSubtitleTracksToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private HTAlt.WinForms.HTButton encAudConfigureBtn;
        private System.Windows.Forms.Panel encAudPerTrackPanel;
        private System.Windows.Forms.ComboBox encAudConfMode;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Panel utilsColorDataPanel;
        private HTAlt.WinForms.HTButton utilsColorDataConfBtn;
        private System.Windows.Forms.Label label40;
        private HTAlt.WinForms.HTButton htButton3;
        private System.Windows.Forms.ComboBox av1anOptsConcatMode;
        private System.Windows.Forms.Label label41;
        private HTAlt.WinForms.HTButton av1anResumeBtn;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Panel utilsConcatPanel;
        private System.Windows.Forms.Label label44;
        private HTAlt.WinForms.HTButton htButton1;
        private System.Windows.Forms.Button fileListSortBtn;
        private System.Windows.Forms.ContextMenuStrip sortFileListContextMenu;
        private System.Windows.Forms.ToolStripMenuItem sortMenuAbcDesc;
        private System.Windows.Forms.ToolStripMenuItem sortMenuAbcAsc;
        private System.Windows.Forms.ToolStripMenuItem sortMenuSizeDesc;
        private System.Windows.Forms.ToolStripMenuItem sortMenuSizeAsc;
        private System.Windows.Forms.ToolStripMenuItem sortMenuRecentDesc;
        private System.Windows.Forms.ToolStripMenuItem sortMenuRecentAsc;
        private System.Windows.Forms.Panel utilsBitratePlotPanel;
        private System.Windows.Forms.Label label45;
        private HTAlt.WinForms.HTButton htButton4;
        private System.Windows.Forms.NumericUpDown av1anThreads;
        private System.Windows.Forms.Label label46;
        private HTAlt.WinForms.HTButton encCropConfBtn;
        private HTAlt.WinForms.HTButton av1anCropConfBtn;
        private System.Windows.Forms.ListView streamListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListView fileList;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label fileCountLabel;
        private System.Windows.Forms.ComboBox av1anOptsChunkOrder;
        private System.Windows.Forms.Label label47;
    }
}

