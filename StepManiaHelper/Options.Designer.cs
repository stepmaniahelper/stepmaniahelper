namespace StepManiaHelper
{
    partial class Options
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
            this.components = new System.ComponentModel.Container();
            this.grpSpecialFilters = new System.Windows.Forms.GroupBox();
            this.cboSongSimilarity = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkAltFolder = new System.Windows.Forms.CheckBox();
            this.chkExactDuplicates = new System.Windows.Forms.CheckBox();
            this.txtSongsDirectory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSongsDirectory = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnParse = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.chkSongHeaderVisible = new System.Windows.Forms.CheckBox();
            this.lblSongListHeader = new System.Windows.Forms.Label();
            this.cbSongListHeaders = new System.Windows.Forms.ComboBox();
            this.dgvSongList = new System.Windows.Forms.DataGridView();
            this.folderNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSongBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpOutput = new System.Windows.Forms.GroupBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.grpProgramOptions = new System.Windows.Forms.GroupBox();
            this.chkSaveBinaryFile = new System.Windows.Forms.CheckBox();
            this.chkLoadBinaryFile = new System.Windows.Forms.CheckBox();
            this.radParseAll = new System.Windows.Forms.RadioButton();
            this.radParseUnparsed = new System.Windows.Forms.RadioButton();
            this.radNoParse = new System.Windows.Forms.RadioButton();
            this.chkIncludeAlreadyFiltered = new System.Windows.Forms.CheckBox();
            this.chkDetectOnlyDisplayedData = new System.Windows.Forms.CheckBox();
            this.chkSearchForNewSongs = new System.Windows.Forms.CheckBox();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.tlpSearchOperands = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddSearchOperand = new System.Windows.Forms.Button();
            this.tlpSearchComboType = new System.Windows.Forms.TableLayoutPanel();
            this.radSearchAnd = new System.Windows.Forms.RadioButton();
            this.btnClearSearch = new System.Windows.Forms.Button();
            this.btnApplySearch = new System.Windows.Forms.Button();
            this.radSearchOr = new System.Windows.Forms.RadioButton();
            this.btnSearchDelete = new System.Windows.Forms.Button();
            this.cbxSearchName = new System.Windows.Forms.ComboBox();
            this.lblSearchName = new System.Windows.Forms.Label();
            this.grpSongInfo = new System.Windows.Forms.GroupBox();
            this.chkDiffHeaderVisible = new System.Windows.Forms.CheckBox();
            this.txtFolderName = new System.Windows.Forms.TextBox();
            this.lblDiffListHeader = new System.Windows.Forms.Label();
            this.cbDiffListHeaders = new System.Windows.Forms.ComboBox();
            this.lblFolderName = new System.Windows.Forms.Label();
            this.btnPlayPause = new System.Windows.Forms.Button();
            this.txtPack = new System.Windows.Forms.TextBox();
            this.lblPack = new System.Windows.Forms.Label();
            this.dgvDifficulties = new System.Windows.Forms.DataGridView();
            this.difficultyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stepsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jumpsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.holdsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rollsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nonPadJumpsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDifficultyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpFilterCustomPack = new System.Windows.Forms.GroupBox();
            this.tlpFilterCustomSongPack = new System.Windows.Forms.TableLayoutPanel();
            this.radCustomSongPack = new System.Windows.Forms.RadioButton();
            this.btnClearAllVisible = new System.Windows.Forms.Button();
            this.btnApplyAllVisible = new System.Windows.Forms.Button();
            this.radFilter = new System.Windows.Forms.RadioButton();
            this.btnFolderDelete = new System.Windows.Forms.Button();
            this.cbxFolders = new System.Windows.Forms.ComboBox();
            this.lblCustomFolderName = new System.Windows.Forms.Label();
            this.tlpLeft = new System.Windows.Forms.TableLayoutPanel();
            this.tlpRight = new System.Windows.Forms.TableLayoutPanel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.grpSpecialFilters.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cSongBindingSource)).BeginInit();
            this.grpOutput.SuspendLayout();
            this.grpProgramOptions.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.tlpSearchOperands.SuspendLayout();
            this.tlpSearchComboType.SuspendLayout();
            this.grpSongInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDifficulties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cDifficultyBindingSource)).BeginInit();
            this.grpFilterCustomPack.SuspendLayout();
            this.tlpFilterCustomSongPack.SuspendLayout();
            this.tlpLeft.SuspendLayout();
            this.tlpRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSpecialFilters
            // 
            this.grpSpecialFilters.Controls.Add(this.cboSongSimilarity);
            this.grpSpecialFilters.Controls.Add(this.label3);
            this.grpSpecialFilters.Controls.Add(this.chkAltFolder);
            this.grpSpecialFilters.Controls.Add(this.chkExactDuplicates);
            this.grpSpecialFilters.Location = new System.Drawing.Point(0, 198);
            this.grpSpecialFilters.Margin = new System.Windows.Forms.Padding(0);
            this.grpSpecialFilters.Name = "grpSpecialFilters";
            this.grpSpecialFilters.Size = new System.Drawing.Size(440, 95);
            this.grpSpecialFilters.TabIndex = 2;
            this.grpSpecialFilters.TabStop = false;
            this.grpSpecialFilters.Text = "Special Filters";
            // 
            // cboSongSimilarity
            // 
            this.cboSongSimilarity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSongSimilarity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSongSimilarity.Enabled = false;
            this.cboSongSimilarity.FormattingEnabled = true;
            this.cboSongSimilarity.Items.AddRange(new object[] {
            "4 - Lowest",
            "5",
            "6",
            "7 - Recommended",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13 - Highest"});
            this.cboSongSimilarity.Location = new System.Drawing.Point(297, 67);
            this.cboSongSimilarity.Name = "cboSongSimilarity";
            this.cboSongSimilarity.Size = new System.Drawing.Size(137, 21);
            this.cboSongSimilarity.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(163, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Song Similarity Threshold:";
            // 
            // chkAltFolder
            // 
            this.chkAltFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAltFolder.Location = new System.Drawing.Point(9, 46);
            this.chkAltFolder.Name = "chkAltFolder";
            this.chkAltFolder.Size = new System.Drawing.Size(425, 21);
            this.chkAltFolder.TabIndex = 2;
            this.chkAltFolder.Text = "Move Different Versions of Same Song to \"_ALT\" Folder";
            this.chkAltFolder.UseVisualStyleBackColor = true;
            // 
            // chkExactDuplicates
            // 
            this.chkExactDuplicates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkExactDuplicates.Location = new System.Drawing.Point(9, 19);
            this.chkExactDuplicates.Name = "chkExactDuplicates";
            this.chkExactDuplicates.Size = new System.Drawing.Size(425, 21);
            this.chkExactDuplicates.TabIndex = 0;
            this.chkExactDuplicates.Text = "Move Exactly Duplicated Songs to \"_DUPLICATE\" Folder";
            this.toolTip.SetToolTip(this.chkExactDuplicates, "If two songs have the exact same \"SM\" or \"DWI\" file, one will be moved.");
            this.chkExactDuplicates.UseVisualStyleBackColor = true;
            // 
            // txtSongsDirectory
            // 
            this.txtSongsDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSongsDirectory.Location = new System.Drawing.Point(9, 21);
            this.txtSongsDirectory.Name = "txtSongsDirectory";
            this.txtSongsDirectory.Size = new System.Drawing.Size(771, 20);
            this.txtSongsDirectory.TabIndex = 3;
            this.txtSongsDirectory.TextChanged += new System.EventHandler(this.DetermineParseButtonEnabledState);
            this.txtSongsDirectory.DoubleClick += new System.EventHandler(this.OpenFolderBrowser);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(12, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(984, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Select StepMania Directory, Songs Directory, Song Pack Directory, or Individual S" +
    "ong Directory:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnSongsDirectory
            // 
            this.btnSongsDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSongsDirectory.Location = new System.Drawing.Point(786, 18);
            this.btnSongsDirectory.Name = "btnSongsDirectory";
            this.btnSongsDirectory.Size = new System.Drawing.Size(24, 23);
            this.btnSongsDirectory.TabIndex = 5;
            this.btnSongsDirectory.Text = "...";
            this.btnSongsDirectory.UseVisualStyleBackColor = true;
            this.btnSongsDirectory.Click += new System.EventHandler(this.OpenFolderBrowser);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 20000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // btnParse
            // 
            this.btnParse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnParse.Enabled = false;
            this.btnParse.Location = new System.Drawing.Point(816, 18);
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new System.Drawing.Size(180, 23);
            this.btnParse.TabIndex = 7;
            this.btnParse.Text = "Search/Parse/Run-Special-Filter";
            this.btnParse.UseVisualStyleBackColor = true;
            this.btnParse.Click += new System.EventHandler(this.btnStartParse_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Controls.Add(this.btnExport);
            this.groupBox3.Controls.Add(this.chkSongHeaderVisible);
            this.groupBox3.Controls.Add(this.lblSongListHeader);
            this.groupBox3.Controls.Add(this.cbSongListHeaders);
            this.groupBox3.Controls.Add(this.dgvSongList);
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(544, 514);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Song List";
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(463, 15);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // chkSongHeaderVisible
            // 
            this.chkSongHeaderVisible.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSongHeaderVisible.AutoSize = true;
            this.chkSongHeaderVisible.Location = new System.Drawing.Point(364, 19);
            this.chkSongHeaderVisible.Name = "chkSongHeaderVisible";
            this.chkSongHeaderVisible.Size = new System.Drawing.Size(94, 17);
            this.chkSongHeaderVisible.TabIndex = 3;
            this.chkSongHeaderVisible.Text = "Header Visible";
            this.chkSongHeaderVisible.UseVisualStyleBackColor = true;
            // 
            // lblSongListHeader
            // 
            this.lblSongListHeader.AutoSize = true;
            this.lblSongListHeader.Location = new System.Drawing.Point(6, 22);
            this.lblSongListHeader.Name = "lblSongListHeader";
            this.lblSongListHeader.Size = new System.Drawing.Size(69, 13);
            this.lblSongListHeader.TabIndex = 2;
            this.lblSongListHeader.Text = "List Headers:";
            // 
            // cbSongListHeaders
            // 
            this.cbSongListHeaders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSongListHeaders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSongListHeaders.FormattingEnabled = true;
            this.cbSongListHeaders.Location = new System.Drawing.Point(81, 17);
            this.cbSongListHeaders.Name = "cbSongListHeaders";
            this.cbSongListHeaders.Size = new System.Drawing.Size(277, 21);
            this.cbSongListHeaders.TabIndex = 1;
            // 
            // dgvSongList
            // 
            this.dgvSongList.AllowUserToAddRows = false;
            this.dgvSongList.AllowUserToDeleteRows = false;
            this.dgvSongList.AllowUserToOrderColumns = true;
            this.dgvSongList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSongList.AutoGenerateColumns = false;
            this.dgvSongList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSongList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.folderNameDataGridViewTextBoxColumn});
            this.dgvSongList.DataSource = this.cSongBindingSource;
            this.dgvSongList.Location = new System.Drawing.Point(6, 44);
            this.dgvSongList.MultiSelect = false;
            this.dgvSongList.Name = "dgvSongList";
            this.dgvSongList.Size = new System.Drawing.Size(532, 464);
            this.dgvSongList.TabIndex = 0;
            this.dgvSongList.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvSongList_RowPostPaint);
            // 
            // folderNameDataGridViewTextBoxColumn
            // 
            this.folderNameDataGridViewTextBoxColumn.DataPropertyName = "FolderName";
            this.folderNameDataGridViewTextBoxColumn.HeaderText = "FolderName";
            this.folderNameDataGridViewTextBoxColumn.Name = "folderNameDataGridViewTextBoxColumn";
            // 
            // cSongBindingSource
            // 
            this.cSongBindingSource.DataSource = typeof(StepManiaHelper.CSong);
            // 
            // grpOutput
            // 
            this.grpOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpOutput.Controls.Add(this.txtStatus);
            this.grpOutput.Controls.Add(this.rtbOutput);
            this.grpOutput.Location = new System.Drawing.Point(0, 397);
            this.grpOutput.Margin = new System.Windows.Forms.Padding(0);
            this.grpOutput.MinimumSize = new System.Drawing.Size(0, 50);
            this.grpOutput.Name = "grpOutput";
            this.grpOutput.Size = new System.Drawing.Size(440, 273);
            this.grpOutput.TabIndex = 10;
            this.grpOutput.TabStop = false;
            this.grpOutput.Text = "Parse/Filter Output";
            // 
            // txtStatus
            // 
            this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatus.Location = new System.Drawing.Point(9, 19);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(425, 20);
            this.txtStatus.TabIndex = 1;
            // 
            // rtbOutput
            // 
            this.rtbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbOutput.Location = new System.Drawing.Point(9, 45);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.Size = new System.Drawing.Size(425, 222);
            this.rtbOutput.TabIndex = 0;
            this.rtbOutput.Text = "";
            // 
            // grpProgramOptions
            // 
            this.grpProgramOptions.Controls.Add(this.chkSaveBinaryFile);
            this.grpProgramOptions.Controls.Add(this.chkLoadBinaryFile);
            this.grpProgramOptions.Controls.Add(this.radParseAll);
            this.grpProgramOptions.Controls.Add(this.radParseUnparsed);
            this.grpProgramOptions.Controls.Add(this.radNoParse);
            this.grpProgramOptions.Controls.Add(this.chkIncludeAlreadyFiltered);
            this.grpProgramOptions.Controls.Add(this.chkDetectOnlyDisplayedData);
            this.grpProgramOptions.Controls.Add(this.chkSearchForNewSongs);
            this.grpProgramOptions.Location = new System.Drawing.Point(0, 0);
            this.grpProgramOptions.Margin = new System.Windows.Forms.Padding(0);
            this.grpProgramOptions.Name = "grpProgramOptions";
            this.grpProgramOptions.Size = new System.Drawing.Size(440, 116);
            this.grpProgramOptions.TabIndex = 12;
            this.grpProgramOptions.TabStop = false;
            this.grpProgramOptions.Text = "Program Options";
            // 
            // chkSaveBinaryFile
            // 
            this.chkSaveBinaryFile.AutoSize = true;
            this.chkSaveBinaryFile.Location = new System.Drawing.Point(245, 65);
            this.chkSaveBinaryFile.Name = "chkSaveBinaryFile";
            this.chkSaveBinaryFile.Size = new System.Drawing.Size(128, 17);
            this.chkSaveBinaryFile.TabIndex = 7;
            this.chkSaveBinaryFile.Text = "Save data after parse";
            this.chkSaveBinaryFile.UseVisualStyleBackColor = true;
            // 
            // chkLoadBinaryFile
            // 
            this.chkLoadBinaryFile.AutoSize = true;
            this.chkLoadBinaryFile.Location = new System.Drawing.Point(9, 65);
            this.chkLoadBinaryFile.Name = "chkLoadBinaryFile";
            this.chkLoadBinaryFile.Size = new System.Drawing.Size(165, 17);
            this.chkLoadBinaryFile.TabIndex = 6;
            this.chkLoadBinaryFile.Text = "Load saved data (if available)";
            this.chkLoadBinaryFile.UseVisualStyleBackColor = true;
            // 
            // radParseAll
            // 
            this.radParseAll.AutoSize = true;
            this.radParseAll.Location = new System.Drawing.Point(338, 88);
            this.radParseAll.Name = "radParseAll";
            this.radParseAll.Size = new System.Drawing.Size(96, 17);
            this.radParseAll.TabIndex = 5;
            this.radParseAll.TabStop = true;
            this.radParseAll.Text = "Parse all songs";
            this.radParseAll.UseVisualStyleBackColor = true;
            // 
            // radParseUnparsed
            // 
            this.radParseUnparsed.AutoSize = true;
            this.radParseUnparsed.Location = new System.Drawing.Point(153, 88);
            this.radParseUnparsed.Name = "radParseUnparsed";
            this.radParseUnparsed.Size = new System.Drawing.Size(152, 17);
            this.radParseUnparsed.TabIndex = 4;
            this.radParseUnparsed.TabStop = true;
            this.radParseUnparsed.Text = "Parse only unparsed songs";
            this.radParseUnparsed.UseVisualStyleBackColor = true;
            // 
            // radNoParse
            // 
            this.radNoParse.AutoSize = true;
            this.radNoParse.Location = new System.Drawing.Point(9, 88);
            this.radNoParse.Name = "radNoParse";
            this.radNoParse.Size = new System.Drawing.Size(110, 17);
            this.radNoParse.TabIndex = 3;
            this.radNoParse.TabStop = true;
            this.radNoParse.Text = "Don\'t parse songs";
            this.radNoParse.UseVisualStyleBackColor = true;
            // 
            // chkIncludeAlreadyFiltered
            // 
            this.chkIncludeAlreadyFiltered.AutoSize = true;
            this.chkIncludeAlreadyFiltered.Location = new System.Drawing.Point(245, 19);
            this.chkIncludeAlreadyFiltered.Name = "chkIncludeAlreadyFiltered";
            this.chkIncludeAlreadyFiltered.Size = new System.Drawing.Size(189, 17);
            this.chkIncludeAlreadyFiltered.TabIndex = 2;
            this.chkIncludeAlreadyFiltered.Text = "Include already filtered songs in list";
            this.chkIncludeAlreadyFiltered.UseVisualStyleBackColor = true;
            // 
            // chkDetectOnlyDisplayedData
            // 
            this.chkDetectOnlyDisplayedData.AutoSize = true;
            this.chkDetectOnlyDisplayedData.Checked = true;
            this.chkDetectOnlyDisplayedData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDetectOnlyDisplayedData.Location = new System.Drawing.Point(9, 42);
            this.chkDetectOnlyDisplayedData.Name = "chkDetectOnlyDisplayedData";
            this.chkDetectOnlyDisplayedData.Size = new System.Drawing.Size(273, 17);
            this.chkDetectOnlyDisplayedData.TabIndex = 1;
            this.chkDetectOnlyDisplayedData.Text = "If parsing, only parse songs for info visible in song list";
            this.chkDetectOnlyDisplayedData.UseVisualStyleBackColor = true;
            // 
            // chkSearchForNewSongs
            // 
            this.chkSearchForNewSongs.AutoSize = true;
            this.chkSearchForNewSongs.Location = new System.Drawing.Point(9, 19);
            this.chkSearchForNewSongs.Name = "chkSearchForNewSongs";
            this.chkSearchForNewSongs.Size = new System.Drawing.Size(129, 17);
            this.chkSearchForNewSongs.TabIndex = 0;
            this.chkSearchForNewSongs.Text = "Search for new songs";
            this.chkSearchForNewSongs.UseVisualStyleBackColor = true;
            // 
            // grpSearch
            // 
            this.grpSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grpSearch.Controls.Add(this.tlpSearchOperands);
            this.grpSearch.Controls.Add(this.tlpSearchComboType);
            this.grpSearch.Controls.Add(this.btnSearchDelete);
            this.grpSearch.Controls.Add(this.cbxSearchName);
            this.grpSearch.Controls.Add(this.lblSearchName);
            this.grpSearch.Location = new System.Drawing.Point(0, 293);
            this.grpSearch.Margin = new System.Windows.Forms.Padding(0);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Size = new System.Drawing.Size(440, 104);
            this.grpSearch.TabIndex = 13;
            this.grpSearch.TabStop = false;
            this.grpSearch.Text = "Song Search";
            // 
            // tlpSearchOperands
            // 
            this.tlpSearchOperands.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpSearchOperands.AutoScroll = true;
            this.tlpSearchOperands.AutoSize = true;
            this.tlpSearchOperands.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpSearchOperands.ColumnCount = 1;
            this.tlpSearchOperands.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearchOperands.Controls.Add(this.btnAddSearchOperand, 0, 0);
            this.tlpSearchOperands.Location = new System.Drawing.Point(9, 72);
            this.tlpSearchOperands.Name = "tlpSearchOperands";
            this.tlpSearchOperands.RowCount = 1;
            this.tlpSearchOperands.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSearchOperands.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearchOperands.Size = new System.Drawing.Size(425, 23);
            this.tlpSearchOperands.TabIndex = 7;
            // 
            // btnAddSearchOperand
            // 
            this.btnAddSearchOperand.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAddSearchOperand.AutoSize = true;
            this.btnAddSearchOperand.Location = new System.Drawing.Point(154, 0);
            this.btnAddSearchOperand.Margin = new System.Windows.Forms.Padding(0);
            this.btnAddSearchOperand.Name = "btnAddSearchOperand";
            this.btnAddSearchOperand.Size = new System.Drawing.Size(117, 23);
            this.btnAddSearchOperand.TabIndex = 0;
            this.btnAddSearchOperand.Text = "Add Search Operand";
            this.btnAddSearchOperand.UseVisualStyleBackColor = true;
            this.btnAddSearchOperand.Click += new System.EventHandler(this.btnAddSearchOperand_Click);
            // 
            // tlpSearchComboType
            // 
            this.tlpSearchComboType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpSearchComboType.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpSearchComboType.ColumnCount = 7;
            this.tlpSearchComboType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSearchComboType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpSearchComboType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSearchComboType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpSearchComboType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSearchComboType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpSearchComboType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSearchComboType.Controls.Add(this.radSearchAnd, 0, 0);
            this.tlpSearchComboType.Controls.Add(this.btnClearSearch, 6, 0);
            this.tlpSearchComboType.Controls.Add(this.btnApplySearch, 4, 0);
            this.tlpSearchComboType.Controls.Add(this.radSearchOr, 2, 0);
            this.tlpSearchComboType.Location = new System.Drawing.Point(9, 46);
            this.tlpSearchComboType.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSearchComboType.Name = "tlpSearchComboType";
            this.tlpSearchComboType.RowCount = 1;
            this.tlpSearchComboType.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearchComboType.Size = new System.Drawing.Size(424, 23);
            this.tlpSearchComboType.TabIndex = 6;
            // 
            // radSearchAnd
            // 
            this.radSearchAnd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.radSearchAnd.AutoSize = true;
            this.radSearchAnd.Checked = true;
            this.radSearchAnd.Location = new System.Drawing.Point(3, 3);
            this.radSearchAnd.Name = "radSearchAnd";
            this.radSearchAnd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radSearchAnd.Size = new System.Drawing.Size(128, 17);
            this.radSearchAnd.TabIndex = 4;
            this.radSearchAnd.TabStop = true;
            this.radSearchAnd.Text = "AND (all must be true)";
            this.radSearchAnd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radSearchAnd.UseVisualStyleBackColor = true;
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClearSearch.AutoSize = true;
            this.btnClearSearch.Location = new System.Drawing.Point(348, 0);
            this.btnClearSearch.Margin = new System.Windows.Forms.Padding(0);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(76, 23);
            this.btnClearSearch.TabIndex = 6;
            this.btnClearSearch.Text = "Clear search";
            this.btnClearSearch.UseVisualStyleBackColor = true;
            this.btnClearSearch.Click += new System.EventHandler(this.btnClearSearch_Click);
            // 
            // btnApplySearch
            // 
            this.btnApplySearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnApplySearch.AutoSize = true;
            this.btnApplySearch.Location = new System.Drawing.Point(268, 0);
            this.btnApplySearch.Margin = new System.Windows.Forms.Padding(0);
            this.btnApplySearch.Name = "btnApplySearch";
            this.btnApplySearch.Size = new System.Drawing.Size(78, 23);
            this.btnApplySearch.TabIndex = 5;
            this.btnApplySearch.Text = "Apply search";
            this.btnApplySearch.UseVisualStyleBackColor = true;
            this.btnApplySearch.Click += new System.EventHandler(this.btnApplySearch_Click);
            // 
            // radSearchOr
            // 
            this.radSearchOr.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.radSearchOr.AutoSize = true;
            this.radSearchOr.Location = new System.Drawing.Point(139, 3);
            this.radSearchOr.Name = "radSearchOr";
            this.radSearchOr.Size = new System.Drawing.Size(124, 17);
            this.radSearchOr.TabIndex = 3;
            this.radSearchOr.Text = "OR (any can be true)";
            this.radSearchOr.UseVisualStyleBackColor = true;
            // 
            // btnSearchDelete
            // 
            this.btnSearchDelete.Location = new System.Drawing.Point(359, 17);
            this.btnSearchDelete.Name = "btnSearchDelete";
            this.btnSearchDelete.Size = new System.Drawing.Size(75, 23);
            this.btnSearchDelete.TabIndex = 3;
            this.btnSearchDelete.Text = "Delete";
            this.btnSearchDelete.UseVisualStyleBackColor = true;
            this.btnSearchDelete.Click += new System.EventHandler(this.btnSearchDelete_Click);
            // 
            // cbxSearchName
            // 
            this.cbxSearchName.FormattingEnabled = true;
            this.cbxSearchName.Location = new System.Drawing.Point(87, 19);
            this.cbxSearchName.MaxLength = 255;
            this.cbxSearchName.Name = "cbxSearchName";
            this.cbxSearchName.Size = new System.Drawing.Size(266, 21);
            this.cbxSearchName.TabIndex = 1;
            this.cbxSearchName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbxSearchName_KeyPress);
            // 
            // lblSearchName
            // 
            this.lblSearchName.AutoSize = true;
            this.lblSearchName.Location = new System.Drawing.Point(6, 22);
            this.lblSearchName.Name = "lblSearchName";
            this.lblSearchName.Size = new System.Drawing.Size(75, 13);
            this.lblSearchName.TabIndex = 0;
            this.lblSearchName.Text = "Search Name:";
            // 
            // grpSongInfo
            // 
            this.grpSongInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSongInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grpSongInfo.Controls.Add(this.chkDiffHeaderVisible);
            this.grpSongInfo.Controls.Add(this.txtFolderName);
            this.grpSongInfo.Controls.Add(this.lblDiffListHeader);
            this.grpSongInfo.Controls.Add(this.cbDiffListHeaders);
            this.grpSongInfo.Controls.Add(this.lblFolderName);
            this.grpSongInfo.Controls.Add(this.btnPlayPause);
            this.grpSongInfo.Controls.Add(this.txtPack);
            this.grpSongInfo.Controls.Add(this.lblPack);
            this.grpSongInfo.Controls.Add(this.dgvDifficulties);
            this.grpSongInfo.Location = new System.Drawing.Point(0, 514);
            this.grpSongInfo.Margin = new System.Windows.Forms.Padding(0);
            this.grpSongInfo.Name = "grpSongInfo";
            this.grpSongInfo.Size = new System.Drawing.Size(544, 156);
            this.grpSongInfo.TabIndex = 4;
            this.grpSongInfo.TabStop = false;
            this.grpSongInfo.Text = "Song Info";
            // 
            // chkDiffHeaderVisible
            // 
            this.chkDiffHeaderVisible.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDiffHeaderVisible.AutoSize = true;
            this.chkDiffHeaderVisible.Location = new System.Drawing.Point(441, 102);
            this.chkDiffHeaderVisible.Name = "chkDiffHeaderVisible";
            this.chkDiffHeaderVisible.Size = new System.Drawing.Size(94, 17);
            this.chkDiffHeaderVisible.TabIndex = 6;
            this.chkDiffHeaderVisible.Text = "Header Visible";
            this.chkDiffHeaderVisible.UseVisualStyleBackColor = true;
            // 
            // txtFolderName
            // 
            this.txtFolderName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolderName.Location = new System.Drawing.Point(82, 45);
            this.txtFolderName.Name = "txtFolderName";
            this.txtFolderName.ReadOnly = true;
            this.txtFolderName.Size = new System.Drawing.Size(456, 20);
            this.txtFolderName.TabIndex = 5;
            // 
            // lblDiffListHeader
            // 
            this.lblDiffListHeader.AutoSize = true;
            this.lblDiffListHeader.Location = new System.Drawing.Point(6, 103);
            this.lblDiffListHeader.Name = "lblDiffListHeader";
            this.lblDiffListHeader.Size = new System.Drawing.Size(69, 13);
            this.lblDiffListHeader.TabIndex = 5;
            this.lblDiffListHeader.Text = "List Headers:";
            // 
            // cbDiffListHeaders
            // 
            this.cbDiffListHeaders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDiffListHeaders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDiffListHeaders.FormattingEnabled = true;
            this.cbDiffListHeaders.Location = new System.Drawing.Point(78, 100);
            this.cbDiffListHeaders.Name = "cbDiffListHeaders";
            this.cbDiffListHeaders.Size = new System.Drawing.Size(357, 21);
            this.cbDiffListHeaders.TabIndex = 4;
            // 
            // lblFolderName
            // 
            this.lblFolderName.AutoSize = true;
            this.lblFolderName.Location = new System.Drawing.Point(6, 48);
            this.lblFolderName.Name = "lblFolderName";
            this.lblFolderName.Size = new System.Drawing.Size(70, 13);
            this.lblFolderName.TabIndex = 4;
            this.lblFolderName.Text = "Folder Name:";
            // 
            // btnPlayPause
            // 
            this.btnPlayPause.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlayPause.Location = new System.Drawing.Point(9, 71);
            this.btnPlayPause.Name = "btnPlayPause";
            this.btnPlayPause.Size = new System.Drawing.Size(529, 23);
            this.btnPlayPause.TabIndex = 3;
            this.btnPlayPause.Text = "Play/Pause";
            this.btnPlayPause.UseVisualStyleBackColor = true;
            this.btnPlayPause.Click += new System.EventHandler(this.btnPlayPause_Click);
            // 
            // txtPack
            // 
            this.txtPack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPack.Location = new System.Drawing.Point(81, 19);
            this.txtPack.Name = "txtPack";
            this.txtPack.ReadOnly = true;
            this.txtPack.Size = new System.Drawing.Size(457, 20);
            this.txtPack.TabIndex = 2;
            // 
            // lblPack
            // 
            this.lblPack.AutoSize = true;
            this.lblPack.Location = new System.Drawing.Point(6, 22);
            this.lblPack.Name = "lblPack";
            this.lblPack.Size = new System.Drawing.Size(35, 13);
            this.lblPack.TabIndex = 1;
            this.lblPack.Text = "Pack:";
            // 
            // dgvDifficulties
            // 
            this.dgvDifficulties.AllowUserToAddRows = false;
            this.dgvDifficulties.AllowUserToDeleteRows = false;
            this.dgvDifficulties.AllowUserToResizeRows = false;
            this.dgvDifficulties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDifficulties.AutoGenerateColumns = false;
            this.dgvDifficulties.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvDifficulties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDifficulties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.difficultyDataGridViewTextBoxColumn,
            this.notesDataGridViewTextBoxColumn,
            this.stepsDataGridViewTextBoxColumn,
            this.jumpsDataGridViewTextBoxColumn,
            this.holdsDataGridViewTextBoxColumn,
            this.rollsDataGridViewTextBoxColumn,
            this.minesDataGridViewTextBoxColumn,
            this.nonPadJumpsDataGridViewTextBoxColumn});
            this.dgvDifficulties.DataSource = this.cDifficultyBindingSource;
            this.dgvDifficulties.Location = new System.Drawing.Point(6, 127);
            this.dgvDifficulties.MinimumSize = new System.Drawing.Size(0, 21);
            this.dgvDifficulties.MultiSelect = false;
            this.dgvDifficulties.Name = "dgvDifficulties";
            this.dgvDifficulties.Size = new System.Drawing.Size(532, 21);
            this.dgvDifficulties.TabIndex = 0;
            // 
            // difficultyDataGridViewTextBoxColumn
            // 
            this.difficultyDataGridViewTextBoxColumn.DataPropertyName = "Difficulty";
            this.difficultyDataGridViewTextBoxColumn.HeaderText = "Difficulty";
            this.difficultyDataGridViewTextBoxColumn.Name = "difficultyDataGridViewTextBoxColumn";
            // 
            // notesDataGridViewTextBoxColumn
            // 
            this.notesDataGridViewTextBoxColumn.DataPropertyName = "Notes";
            this.notesDataGridViewTextBoxColumn.HeaderText = "Notes";
            this.notesDataGridViewTextBoxColumn.Name = "notesDataGridViewTextBoxColumn";
            // 
            // stepsDataGridViewTextBoxColumn
            // 
            this.stepsDataGridViewTextBoxColumn.DataPropertyName = "Steps";
            this.stepsDataGridViewTextBoxColumn.HeaderText = "Steps";
            this.stepsDataGridViewTextBoxColumn.Name = "stepsDataGridViewTextBoxColumn";
            // 
            // jumpsDataGridViewTextBoxColumn
            // 
            this.jumpsDataGridViewTextBoxColumn.DataPropertyName = "Jumps";
            this.jumpsDataGridViewTextBoxColumn.HeaderText = "Jumps";
            this.jumpsDataGridViewTextBoxColumn.Name = "jumpsDataGridViewTextBoxColumn";
            // 
            // holdsDataGridViewTextBoxColumn
            // 
            this.holdsDataGridViewTextBoxColumn.DataPropertyName = "Holds";
            this.holdsDataGridViewTextBoxColumn.HeaderText = "Holds";
            this.holdsDataGridViewTextBoxColumn.Name = "holdsDataGridViewTextBoxColumn";
            // 
            // rollsDataGridViewTextBoxColumn
            // 
            this.rollsDataGridViewTextBoxColumn.DataPropertyName = "Rolls";
            this.rollsDataGridViewTextBoxColumn.HeaderText = "Rolls";
            this.rollsDataGridViewTextBoxColumn.Name = "rollsDataGridViewTextBoxColumn";
            // 
            // minesDataGridViewTextBoxColumn
            // 
            this.minesDataGridViewTextBoxColumn.DataPropertyName = "Mines";
            this.minesDataGridViewTextBoxColumn.HeaderText = "Mines";
            this.minesDataGridViewTextBoxColumn.Name = "minesDataGridViewTextBoxColumn";
            // 
            // nonPadJumpsDataGridViewTextBoxColumn
            // 
            this.nonPadJumpsDataGridViewTextBoxColumn.DataPropertyName = "NonPadJumps";
            this.nonPadJumpsDataGridViewTextBoxColumn.HeaderText = "NonPadJumps";
            this.nonPadJumpsDataGridViewTextBoxColumn.Name = "nonPadJumpsDataGridViewTextBoxColumn";
            // 
            // cDifficultyBindingSource
            // 
            this.cDifficultyBindingSource.DataSource = typeof(StepManiaHelper.CDifficulty);
            // 
            // grpFilterCustomPack
            // 
            this.grpFilterCustomPack.AutoSize = true;
            this.grpFilterCustomPack.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grpFilterCustomPack.Controls.Add(this.tlpFilterCustomSongPack);
            this.grpFilterCustomPack.Controls.Add(this.btnFolderDelete);
            this.grpFilterCustomPack.Controls.Add(this.cbxFolders);
            this.grpFilterCustomPack.Controls.Add(this.lblCustomFolderName);
            this.grpFilterCustomPack.Location = new System.Drawing.Point(0, 116);
            this.grpFilterCustomPack.Margin = new System.Windows.Forms.Padding(0);
            this.grpFilterCustomPack.Name = "grpFilterCustomPack";
            this.grpFilterCustomPack.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.grpFilterCustomPack.Size = new System.Drawing.Size(440, 82);
            this.grpFilterCustomPack.TabIndex = 14;
            this.grpFilterCustomPack.TabStop = false;
            this.grpFilterCustomPack.Text = "Filter Folders / Custom Song Pack Folders";
            // 
            // tlpFilterCustomSongPack
            // 
            this.tlpFilterCustomSongPack.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpFilterCustomSongPack.ColumnCount = 7;
            this.tlpFilterCustomSongPack.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterCustomSongPack.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpFilterCustomSongPack.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterCustomSongPack.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpFilterCustomSongPack.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterCustomSongPack.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpFilterCustomSongPack.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterCustomSongPack.Controls.Add(this.radCustomSongPack, 0, 0);
            this.tlpFilterCustomSongPack.Controls.Add(this.btnClearAllVisible, 6, 0);
            this.tlpFilterCustomSongPack.Controls.Add(this.btnApplyAllVisible, 4, 0);
            this.tlpFilterCustomSongPack.Controls.Add(this.radFilter, 2, 0);
            this.tlpFilterCustomSongPack.Location = new System.Drawing.Point(6, 46);
            this.tlpFilterCustomSongPack.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tlpFilterCustomSongPack.Name = "tlpFilterCustomSongPack";
            this.tlpFilterCustomSongPack.RowCount = 1;
            this.tlpFilterCustomSongPack.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterCustomSongPack.Size = new System.Drawing.Size(428, 23);
            this.tlpFilterCustomSongPack.TabIndex = 5;
            // 
            // radCustomSongPack
            // 
            this.radCustomSongPack.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.radCustomSongPack.AutoSize = true;
            this.radCustomSongPack.Checked = true;
            this.radCustomSongPack.Location = new System.Drawing.Point(3, 3);
            this.radCustomSongPack.Name = "radCustomSongPack";
            this.radCustomSongPack.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radCustomSongPack.Size = new System.Drawing.Size(116, 17);
            this.radCustomSongPack.TabIndex = 4;
            this.radCustomSongPack.TabStop = true;
            this.radCustomSongPack.Text = "Custom Song Pack";
            this.radCustomSongPack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radCustomSongPack.UseVisualStyleBackColor = true;
            this.radCustomSongPack.CheckedChanged += new System.EventHandler(this.radCustomSongPack_CheckedChanged);
            // 
            // btnClearAllVisible
            // 
            this.btnClearAllVisible.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClearAllVisible.AutoSize = true;
            this.btnClearAllVisible.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClearAllVisible.Location = new System.Drawing.Point(318, 0);
            this.btnClearAllVisible.Margin = new System.Windows.Forms.Padding(0);
            this.btnClearAllVisible.Name = "btnClearAllVisible";
            this.btnClearAllVisible.Size = new System.Drawing.Size(109, 23);
            this.btnClearAllVisible.TabIndex = 6;
            this.btnClearAllVisible.Text = "Clear from all visible";
            this.btnClearAllVisible.UseVisualStyleBackColor = true;
            this.btnClearAllVisible.Click += new System.EventHandler(this.btnClearAllVisible_Click);
            // 
            // btnApplyAllVisible
            // 
            this.btnApplyAllVisible.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnApplyAllVisible.AutoSize = true;
            this.btnApplyAllVisible.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnApplyAllVisible.Location = new System.Drawing.Point(203, 0);
            this.btnApplyAllVisible.Margin = new System.Windows.Forms.Padding(0);
            this.btnApplyAllVisible.Name = "btnApplyAllVisible";
            this.btnApplyAllVisible.Size = new System.Drawing.Size(100, 23);
            this.btnApplyAllVisible.TabIndex = 5;
            this.btnApplyAllVisible.Text = "Apply to all visible";
            this.btnApplyAllVisible.UseVisualStyleBackColor = true;
            this.btnApplyAllVisible.Click += new System.EventHandler(this.btnApplyAllVisible_Click);
            // 
            // radFilter
            // 
            this.radFilter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.radFilter.AutoSize = true;
            this.radFilter.Location = new System.Drawing.Point(139, 3);
            this.radFilter.Name = "radFilter";
            this.radFilter.Size = new System.Drawing.Size(47, 17);
            this.radFilter.TabIndex = 3;
            this.radFilter.Text = "Filter";
            this.radFilter.UseVisualStyleBackColor = true;
            this.radFilter.CheckedChanged += new System.EventHandler(this.radFilter_CheckedChanged);
            // 
            // btnFolderDelete
            // 
            this.btnFolderDelete.Location = new System.Drawing.Point(359, 17);
            this.btnFolderDelete.Name = "btnFolderDelete";
            this.btnFolderDelete.Size = new System.Drawing.Size(75, 23);
            this.btnFolderDelete.TabIndex = 2;
            this.btnFolderDelete.Text = "Delete";
            this.btnFolderDelete.UseVisualStyleBackColor = true;
            this.btnFolderDelete.Click += new System.EventHandler(this.btnFolderDelete_Click);
            // 
            // cbxFolders
            // 
            this.cbxFolders.FormattingEnabled = true;
            this.cbxFolders.Location = new System.Drawing.Point(82, 19);
            this.cbxFolders.MaxLength = 255;
            this.cbxFolders.Name = "cbxFolders";
            this.cbxFolders.Size = new System.Drawing.Size(271, 21);
            this.cbxFolders.TabIndex = 1;
            this.cbxFolders.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbxFolders_KeyPress);
            // 
            // lblCustomFolderName
            // 
            this.lblCustomFolderName.AutoSize = true;
            this.lblCustomFolderName.Location = new System.Drawing.Point(6, 22);
            this.lblCustomFolderName.Name = "lblCustomFolderName";
            this.lblCustomFolderName.Size = new System.Drawing.Size(70, 13);
            this.lblCustomFolderName.TabIndex = 0;
            this.lblCustomFolderName.Text = "Folder Name:";
            // 
            // tlpLeft
            // 
            this.tlpLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tlpLeft.ColumnCount = 1;
            this.tlpLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLeft.Controls.Add(this.grpProgramOptions, 0, 0);
            this.tlpLeft.Controls.Add(this.grpFilterCustomPack, 0, 1);
            this.tlpLeft.Controls.Add(this.grpOutput, 0, 4);
            this.tlpLeft.Controls.Add(this.grpSearch, 0, 3);
            this.tlpLeft.Controls.Add(this.grpSpecialFilters, 0, 2);
            this.tlpLeft.Location = new System.Drawing.Point(9, 47);
            this.tlpLeft.Margin = new System.Windows.Forms.Padding(0);
            this.tlpLeft.Name = "tlpLeft";
            this.tlpLeft.RowCount = 5;
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLeft.Size = new System.Drawing.Size(440, 670);
            this.tlpLeft.TabIndex = 15;
            // 
            // tlpRight
            // 
            this.tlpRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpRight.ColumnCount = 1;
            this.tlpRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRight.Controls.Add(this.groupBox3, 0, 0);
            this.tlpRight.Controls.Add(this.grpSongInfo, 0, 1);
            this.tlpRight.Location = new System.Drawing.Point(452, 47);
            this.tlpRight.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRight.Name = "tlpRight";
            this.tlpRight.RowCount = 2;
            this.tlpRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRight.Size = new System.Drawing.Size(544, 670);
            this.tlpRight.TabIndex = 16;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.tlpRight);
            this.Controls.Add(this.tlpLeft);
            this.Controls.Add(this.btnParse);
            this.Controls.Add(this.btnSongsDirectory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSongsDirectory);
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "Options";
            this.Text = "StepMania Helper V1.02.01";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Options_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Options_FormClosed);
            this.Load += new System.EventHandler(this.Options_Load);
            this.grpSpecialFilters.ResumeLayout(false);
            this.grpSpecialFilters.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cSongBindingSource)).EndInit();
            this.grpOutput.ResumeLayout(false);
            this.grpOutput.PerformLayout();
            this.grpProgramOptions.ResumeLayout(false);
            this.grpProgramOptions.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            this.tlpSearchOperands.ResumeLayout(false);
            this.tlpSearchOperands.PerformLayout();
            this.tlpSearchComboType.ResumeLayout(false);
            this.tlpSearchComboType.PerformLayout();
            this.grpSongInfo.ResumeLayout(false);
            this.grpSongInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDifficulties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cDifficultyBindingSource)).EndInit();
            this.grpFilterCustomPack.ResumeLayout(false);
            this.grpFilterCustomPack.PerformLayout();
            this.tlpFilterCustomSongPack.ResumeLayout(false);
            this.tlpFilterCustomSongPack.PerformLayout();
            this.tlpLeft.ResumeLayout(false);
            this.tlpLeft.PerformLayout();
            this.tlpRight.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.GroupBox grpSpecialFilters;
        public System.Windows.Forms.CheckBox chkAltFolder;
        public System.Windows.Forms.CheckBox chkExactDuplicates;
        public System.Windows.Forms.TextBox txtSongsDirectory;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Button btnSongsDirectory;
        public System.Windows.Forms.ToolTip toolTip;
        public System.Windows.Forms.Button btnParse;
        public System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.ComboBox cboSongSimilarity;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox grpOutput;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.DataGridViewTextBoxColumn nExpectedDifficultyCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nLengthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn strPackDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn folderNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fDurationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn strMusicFilePathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn strStepFilePathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fAverageBpmDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fModeBpmDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nNumberOfBeatsDataGridViewTextBoxColumn;
        private System.Windows.Forms.GroupBox grpProgramOptions;
        private System.Windows.Forms.CheckBox chkSearchForNewSongs;
        private System.Windows.Forms.CheckBox chkDetectOnlyDisplayedData;
        private System.Windows.Forms.CheckBox chkIncludeAlreadyFiltered;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Label lblSongListHeader;
        private System.Windows.Forms.RadioButton radParseAll;
        private System.Windows.Forms.RadioButton radParseUnparsed;
        private System.Windows.Forms.RadioButton radNoParse;
        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.ComboBox cbxSearchName;
        private System.Windows.Forms.Label lblSearchName;
        private System.Windows.Forms.Button btnSearchDelete;
        private System.Windows.Forms.Label lblFolderName;
        private System.Windows.Forms.Button btnPlayPause;
        private System.Windows.Forms.Label lblPack;
        private System.Windows.Forms.Label lblDiffListHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayDifficultyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberOfNotesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberOfStepsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberOfJumpsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberOfMeasuresDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberOfHoldsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberOfRollsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberOfMinesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberOfNonPadJumpsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn notesPerMeasureDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn relativeDifficultyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn difficultyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn notesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn stepsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jumpsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn holdsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rollsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn minesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nonPadJumpsDataGridViewTextBoxColumn;
        private System.Windows.Forms.GroupBox grpFilterCustomPack;
        private System.Windows.Forms.Label lblCustomFolderName;
        private System.Windows.Forms.CheckBox chkSaveBinaryFile;
        private System.Windows.Forms.CheckBox chkLoadBinaryFile;
        private System.Windows.Forms.ComboBox cbxFolders;
        private System.Windows.Forms.TableLayoutPanel tlpFilterCustomSongPack;
        private System.Windows.Forms.RadioButton radCustomSongPack;
        private System.Windows.Forms.RadioButton radFilter;
        private System.Windows.Forms.Button btnFolderDelete;
        private System.Windows.Forms.TableLayoutPanel tlpSearchOperands;
        private System.Windows.Forms.Button btnAddSearchOperand;
        private System.Windows.Forms.TableLayoutPanel tlpSearchComboType;
        private System.Windows.Forms.RadioButton radSearchAnd;
        private System.Windows.Forms.RadioButton radSearchOr;
        private System.Windows.Forms.Button btnApplyAllVisible;
        private System.Windows.Forms.Button btnClearAllVisible;
        internal System.Windows.Forms.DataGridView dgvSongList;
        internal System.Windows.Forms.CheckBox chkSongHeaderVisible;
        internal System.Windows.Forms.ComboBox cbSongListHeaders;
        internal System.Windows.Forms.DataGridView dgvDifficulties;
        internal System.Windows.Forms.CheckBox chkDiffHeaderVisible;
        internal System.Windows.Forms.ComboBox cbDiffListHeaders;
        internal System.Windows.Forms.TextBox txtFolderName;
        internal System.Windows.Forms.TextBox txtPack;
        private System.Windows.Forms.TableLayoutPanel tlpLeft;
        private System.Windows.Forms.TableLayoutPanel tlpRight;
        internal System.Windows.Forms.GroupBox grpSongInfo;
        internal System.Windows.Forms.BindingSource cSongBindingSource;
        internal System.Windows.Forms.BindingSource cDifficultyBindingSource;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        public System.Windows.Forms.Button btnApplySearch;
    }
}