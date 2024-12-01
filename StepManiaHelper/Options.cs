using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using StepManiaHelper.Helpers;
using System.Reflection;
using System.Data.Entity.Internal;
using LibVLCSharp.Shared;
using StepManiaHelper.Search;
using System.Text.RegularExpressions;
using StepManiaHelper.Logic;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace StepManiaHelper
{
    // TODO: Limit filters to certain difficulties
    //       Allow playing/control of music
    //       Clearing all songs from folder doesn't hit all songs?

    internal partial class Options : Form
    {
        static private string strTempFileName = "SavedOptions.json";

        public CSong SelectedSong = new CSong();
        public SortableBindingList<CDifficulty> Difficulties { get; }

        public CSavedFolder SelectedFolder;

        public CSavedSearch DefaultSearch = new CSavedSearch("Default", null);
        public CSavedSearch SelectedSearch;

        private object libVLC; // LibVLC
        private object mediaPlayer; // MediaPlayer
        private object media; // Media

        public CSavedOptions SavedOptions;
        public CSongListPopulator StepManiaParser;
        public CSongSearcher SongSearch;
        Dictionary<CheckBox, ComboBox> AssociatedControls;
        Boolean IsParsing = false;
        public Boolean ChangingSongListSource = false;
        private List<string> lstStrStatus = new List<string>();

        string strParseBtnText;
        string strSearchBtnText;
        public string strCancelBtnText = "Cancel";
        string strNewItem = "Add New";
        string strDefaultSearch = "Default";

        public DataGridViewHelpers dataGridViewHelpers;

        List<string> lstDefaultFilterFolders = new List<string>
        {
            CDuplicateFilter.FilterFolder,
            CSimilarSongFilter.FilterFolder,
        };

        public Options()
        {
            InitializeComponent();
            this.SavedOptions = new CSavedOptions();
            this.StepManiaParser = new CSongListPopulator();
            this.SongSearch = new CSongSearcher(this);
            this.AssociatedControls = new Dictionary<CheckBox, ComboBox>();

            SelectedSong.aDifficulties = new List<CDifficulty>();
            Difficulties = new SortableBindingList<CDifficulty>(SelectedSong.aDifficulties);
            dataGridViewHelpers = new DataGridViewHelpers(this);
        }

        private void LoadVlc()
        {
            LibVLCSharp.Shared.Core.Initialize();
            libVLC = new LibVLC();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            // Will attempt to load the VLC DLLs
            try
            {
                LoadVlc();
            }
            catch
            {
                btnPlayPause.Enabled = false;
            }

            // Save the default values for the buttons which can change their text
            strParseBtnText = btnParse.Text;
            strSearchBtnText = btnApplySearch.Text;

            this.AssociatedControls.Add(chkExactDuplicates, null);
            this.AssociatedControls.Add(chkAltFolder, cboSongSimilarity);

            // Loop through all checkbox-dropdown combinations
            foreach (CheckBox Key in AssociatedControls.Keys)
            {
                // Set the checkbox to use the generic check-changed handler
                Key.CheckedChanged += new System.EventHandler(this.Generic_CheckedChanged);

                // If the combobox exists, set its selected-index-changed handler
                if (AssociatedControls[Key] != null)
                {
                    AssociatedControls[Key].SelectedIndexChanged += new System.EventHandler(this.DetermineParseButtonEnabledState);
                    AssociatedControls[Key].TextChanged += new System.EventHandler(this.DetermineParseButtonEnabledState);
                }
            }

            // Load the save options is there are any
            ReadSavedOptions();

            // Loads saved options, sets up data bindings, sets up event handlers, etc. for the songs list and song info section
            dataGridViewHelpers.SetupLogic();

            // Set up the data bindings
            var binding = txtSongsDirectory.DataBindings.Add(nameof(TextBox.Text), SavedOptions, nameof(CSavedOptions.SongDirectory));
            binding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            chkSearchForNewSongs.DataBindings.Add(nameof(CheckBox.Checked), SavedOptions, nameof(CSavedOptions.SearchForNewSongs));
            chkDetectOnlyDisplayedData.DataBindings.Add(nameof(CheckBox.Checked), SavedOptions, nameof(CSavedOptions.DetectOnlyDisplayedData));
            chkLoadBinaryFile.DataBindings.Add(nameof(CheckBox.Checked), SavedOptions, nameof(CSavedOptions.LoadSaveFile));
            chkSaveBinaryFile.DataBindings.Add(nameof(CheckBox.Checked), SavedOptions, nameof(CSavedOptions.SaveFileAfterParse));
            chkIncludeAlreadyFiltered.DataBindings.Add(nameof(CheckBox.Checked), SavedOptions, nameof(CSavedOptions.IncludeAlreadyFiltered));
            chkExactDuplicates.DataBindings.Add(nameof(CheckBox.Checked), SavedOptions, nameof(CSavedOptions.FilterExactDuplicates));
            chkAltFolder.DataBindings.Add(nameof(CheckBox.Checked), SavedOptions, nameof(CSavedOptions.FilterAltSongs));

            // If the songs path wasn't loaded, set it to the current directory
            if ((SavedOptions?.SongDirectory?.Length ?? 0) == 0)
            {
                this.txtSongsDirectory.Text = Directory.GetCurrentDirectory();
                this.txtSongsDirectory.SelectionStart = this.txtSongsDirectory.Text.Length - 1;
            }

            // Setup for the folder logic
            cbxFolders.SelectedIndexChanged += CbxFolders_SelectedIndexChanged;
            if (SavedOptions.Folders != null)
            {
                foreach (CSavedFolder folder in SavedOptions.Folders)
                {
                    // Add the folders to the folder dropdown
                    cbxFolders.Items.Add(folder);
                }
            }
            cbxFolders.Items.Add(strNewItem);
            cbxFolders.SelectedIndex = 0;
            cbxFolders.Validated += CbxFolders_Validated;

            // Setup for the search logic
            cbxSearchName.SelectedIndexChanged += CbxSearchName_SelectedIndexChanged;
            if (SavedOptions.Searches != null)
            {
                foreach (CSavedSearch search in SavedOptions.Searches)
                {
                    // Add the folders to the folder dropdown
                    if (search.Name != strDefaultSearch)
                    {
                        cbxSearchName.Items.Add(search);
                    }
                    // If this is the default search
                    else
                    {
                        DefaultSearch = search;
                    }
                }
            }
            else
            {
                SavedOptions.Searches = new List<CSavedSearch>();
                SavedOptions.Searches.Add(DefaultSearch);
            }
            cbxSearchName.Items.Add(strNewItem);
            cbxSearchName.SelectedIndex = 0;
            cbxSearchName.Validated += CbxSearchName_Validated;

            // Restore the radiobutton selection
            switch (this.SavedOptions.ParseType)
            {
                case EParseTypes.None:
                    radNoParse.Checked = true;
                    break;
                case EParseTypes.Unparsed:
                    radParseUnparsed.Checked = true;
                    break;
                case EParseTypes.All:
                    radParseAll.Checked = true;
                    break;
            }            

            // Don't add event handlers until all of the above setup has occurred

            // Add event handlers for the program options
            radNoParse.CheckedChanged += ParseType_CheckedChanged;
            radParseUnparsed.CheckedChanged += ParseType_CheckedChanged;
            radParseAll.CheckedChanged += ParseType_CheckedChanged;

            radSearchAnd.CheckedChanged += RadSearch_CheckedChanged;
            radSearchOr.CheckedChanged += RadSearch_CheckedChanged;
        }

        public void ChangeSongListSource(SortableBindingList<CSong> List)
        {
            ChangingSongListSource = true;
            cSongBindingSource.DataSource = List;
            cSongBindingSource.ResetBindings(true);
            ChangingSongListSource = false;
        }

        public void RefreshSongListSource()
        {
            ChangingSongListSource = true;
            cSongBindingSource.ResetBindings(true);
            dgvSongList.Update();
            dgvSongList.Refresh();
            ChangingSongListSource = false;
        }

        #region FolderSearchGroupBox

        // For modifying folders in the song list
        void ModifyFolderInSongList(CSavedFolder folder)
        {
            DataGridViewColumn column = dgvSongList.Columns.OfType<DataGridViewColumn>().FirstOrDefault(x => x.Tag == folder);
            column.HeaderText = folder.Name;

            // Use a dirty hack to force the comboxbox to requery the value of the folder
            typeof(ComboBox).InvokeMember("RefreshItems", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, cbSongListHeaders, new object[] { });

            // TODO: We probably want to check for the existence of the folder in the filesystem 
            // and rename it if it exists, to prevent losing track of the songs inside it.
        }

        // For adding folders to the song list
        public void AddFolderToSongList(CSavedFolder folder, bool DontAddToComboBox = false)
        {
            DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
            column.Tag = folder;
            column.HeaderText = folder.Name;
            column.Visible = false;
            column.SortMode = DataGridViewColumnSortMode.Automatic;
            dgvSongList.Columns.Add(column);

            // During initialization this function will get called first, and then the combobox 
            // will have its items populated. If we add it to the combox when this function is 
            // first called, it will appear in the list twice
            if (DontAddToComboBox == false)
            {
                cbSongListHeaders.Items.Add(folder);
            }
        }

        // Force validation when enter is pressed
        private void cbxFolders_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                CbxFolders_Validated(sender, null);
                e.Handled = true;
            }
        }

        // Force validation when enter is pressed
        private void cbxSearchName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                CbxSearchName_Validated(sender, null);
                e.Handled = true;
            }
        }

        private bool ValidateFolderName(string name)
        {
            bool Success = true;
            string title = "Invalid Folder Name";

            // If the folder name contains invalid characters
            if (Regex.IsMatch(name, "[\\<\\>\\:\\\"\\\\\\/\\|\\?\\*\\.]"))
            {
                Success = false;
                MessageBox.Show(
                    "The supplied folder name contains invalid characters.",
                    title,
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
            // If the folder name is empty
            if (name.Length <= 0)
            {
                Success = false;
                MessageBox.Show(
                    "The folder name must be at least 1 character long.",
                    title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            // If the folder name is not unique
            else if (SavedOptions.Folders?.FirstOrDefault(x => (x.Name == name) && (x != SelectedFolder)) != null)
            {
                Success = false;
                MessageBox.Show(
                    "Custom folder names much be unique.",
                    title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            // If the folder name would duplicate a property of the song class
            else if (typeof(CSong).GetProperties().FirstOrDefault(x => x.Name == name) != null)
            {
                Success = false;
                MessageBox.Show(
                    "Because folders appear in the song list, they cannot mirror the name of a song property.",
                    title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            // If the folder name would duplicate the songs directory
            else if (name.ToLower() == "songs")
            {
                Success = false;
                MessageBox.Show(
                    "The foldername \"Songs\" is reserved.",
                    title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            // If the renamed folder name would duplicate the add new options
            else if ((SelectedFolder != null)
            &&       (name.ToLower() == strNewItem.ToLower()))
            {
                Success = false;
                MessageBox.Show(
                    "The foldername \""+strNewItem+"\" is reserved.",
                    title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return Success;
        }

        private bool ValidateSearchName(string name)
        {
            bool Success = true;
            string title = "Invalid Search Name";

            // If the search name is not unique
            if (SavedOptions.Searches?.FirstOrDefault(x => (x.Name == name) && (x != SelectedSearch)) != null)
            {
                Success = false;
                MessageBox.Show(
                    "Search names much be unique.",
                    title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            // If the search name would duplicate the unsaved searchoptions
            else if (name.ToLower() == strDefaultSearch.ToLower())
            {
                Success = false;
                MessageBox.Show(
                    "The search name \"" + strDefaultSearch + "\" is reserved.",
                    title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            // If the renamed folder name would duplicate the add new options
            else if ((SelectedSearch != DefaultSearch)
            &&       (name.ToLower() == strNewItem.ToLower()))
            {
                Success = false;
                MessageBox.Show(
                    "The search name \"" + strNewItem + "\" is reserved.",
                    title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return Success;
        }

        // Helper function for adding a new folder
        public object AddNewFolder(string name)
        {
            return AddNewFolder(name, null);
        }
        public object AddNewFolder(string name, EFolderTypes? type)
        {
            if (this.InvokeRequired)
            {
                return this.Invoke(new Func<object>(() => { return AddNewFolder(name, type); }));
            }
            else
            {
                // Determine whether the folder is a filter or custom song pack
                type = type ?? ((radCustomSongPack.Checked == true) ? EFolderTypes.CustomSongPack : EFolderTypes.Filter);

                // Add the folder to the list of saved folders
                if (SavedOptions.Folders == null)
                {
                    SavedOptions.Folders = new List<CSavedFolder>();
                }
                SelectedFolder = new CSavedFolder(name, (EFolderTypes)type);
                SavedOptions.Folders.Add(SelectedFolder);

                // Add the folders column to the songs list and the header dropdown
                AddFolderToSongList(SelectedFolder);

                // Update the text of the selected index to match
                cbxFolders.Items.Insert(cbxFolders.Items.Count - 1, SelectedFolder);

                return SelectedFolder;
            }
        }

        // Helper function for renaming a folder
        public void RenameFolder(string name)
        {
            // Update the text in the saved folder's info
            SelectedFolder.Name = name;

            // Update the text shown in the songs list
            ModifyFolderInSongList(SelectedFolder);
        }

        // Helper function for adding a new search
        public object AddNewSearch(string name)
        {
            // Get the list of operands from the current search to copy to the new one
            List<CSearchOperand> lstOperand = SelectedSearch.Operands;

            // Add the folder to the list of saved searches
            if (SavedOptions.Searches == null)
            {
                SavedOptions.Searches = new List<CSavedSearch>();
            }
            SelectedSearch = new CSavedSearch(name, null);
            SelectedSearch.Operands.AddRange(lstOperand);
            SavedOptions.Searches.Add(SelectedSearch);

            // Update the text of the selected index to match
            cbxSearchName.Items.Insert(cbxSearchName.Items.Count - 1, SelectedSearch);

            // Allow deletion of this search
            btnSearchDelete.Enabled = true;

            return SelectedSearch;
        }

        // Helper function for renaming a search
        public void RenameSearch(string name)
        {
            // Update the text in the saved folder's info
            SelectedSearch.Name = name;
        }

        private void ValidatedComboBox(ComboBox comboxBox, object selectedItem, object defaultItem, Func<string, bool> Validate, Func<string, object> AddNew, Action<string> Rename)
        {
            // Trim spaces from the name
            string name = comboxBox.Text.Trim();

            // Ensure the user doesn't enter an invalid name
            if (Validate(name) == false)
            {
                // If the user was editing an existing name
                if (selectedItem != null)
                {
                    // Restore the selection, which should undo the text change
                    comboxBox.SelectedItem = selectedItem;
                }
                // If the user was creating a new name
                else
                {
                    // Select the "Add New" option from the list
                    comboxBox.SelectedIndex = comboxBox.Items.Count - 1;
                }
            }
            else
            {
                // Save the selection start to restore at the end
                int cursorPos = comboxBox.SelectionStart;

                // As soon as the text is modified by the user, the selected item and index are cleared, 
                // because it thinks we are creating a new item. We want the user to be able to modify
                // an existing item though, so we save the selection using the "selectedItem"
                if ((selectedItem == defaultItem)
                && (name != strNewItem))
                {
                    // Call the function to add a new item to the saved data
                    selectedItem = AddNew(name);

                    // Select the newly created item
                    comboxBox.SelectedItem = selectedItem;
                }
                // If the user just renamed an existing folder
                else if (selectedItem != null)
                {
                    // Call the function to update the existing item
                    Rename(name);

                    // Select the updated item
                    comboxBox.SelectedItem = selectedItem;

                    // Use a dirty hack to force the comboxbox to requery the value of the selected item
                    typeof(ComboBox).InvokeMember("RefreshItems", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, comboxBox, new object[] { });
                }

                // Move the cursor back to where it was
                comboxBox.SelectionStart = cursorPos;
            }
        }

        // When the user renames the folder
        private void CbxFolders_Validated(object sender, EventArgs e)
        {
            ValidatedComboBox(sender as ComboBox,
                              SelectedFolder,
                              null,
                              ValidateFolderName,
                              AddNewFolder,
                              RenameFolder);
        }

        // When the user renames the search
        private void CbxSearchName_Validated(object sender, EventArgs e)
        {
            ValidatedComboBox(sender as ComboBox,
                              SelectedSearch,
                              DefaultSearch,
                              ValidateSearchName,
                              AddNewSearch,
                              RenameSearch);
        }

        // When the user changes the selected folder
        private void CbxFolders_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If the user selected a saved folder from the list
            CSavedFolder folder = cbxFolders.SelectedItem as CSavedFolder;
            if (folder != null)
            {
                // Save the selection
                SelectedFolder = folder;

                // Allow deletion of this folder
                btnFolderDelete.Enabled = true;

                // Update the radio buttons to match the folder type
                radCustomSongPack.Checked = (folder.Type == EFolderTypes.CustomSongPack);
                radFilter.Checked = (folder.Type == EFolderTypes.Filter);
            }
            else
            {
                // The "Add New" option can't be deleted (it's not a real folder)
                btnFolderDelete.Enabled = false;

                // Clear the selected folder
                SelectedFolder = null;
            }
        }

        // When the user changes the selected search
        private void CbxSearchName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If the user selected a saved search from the list
            CSavedSearch search = cbxSearchName.SelectedItem as CSavedSearch;
            if ((search != null)
            &&  (search != SelectedSearch))
            {
                // Save the selection
                SelectedSearch = search;

                // Allow deletion of this folder
                btnSearchDelete.Enabled = true;

                // Populate the GUI for the selected search
                PopulateGuiForSearch(SelectedSearch);
            }
            // If "Add New" was selected (it's a string, not a search)
            else if ((search == null)
            &&       (SelectedSearch != DefaultSearch))
            {
                // The "Add New" option can't be deleted (it's not a real folder)
                btnSearchDelete.Enabled = false;

                // Clear the selected search
                SelectedSearch = DefaultSearch;

                // Populate the GUI for the selected search
                PopulateGuiForSearch(DefaultSearch);
            }
        }

        // When the user deletes a folder
        private void btnFolderDelete_Click(object sender, EventArgs e)
        {
            // If the user has a folder selected
            CSavedFolder folder = cbxFolders.SelectedItem as CSavedFolder;
            if (folder != null)
            {
                // Save the selected index
                int index = cbxFolders.SelectedIndex;

                // Remove the folder from the saved data
                SavedOptions.Folders.Remove(folder);

                // Remove the option from the combobox
                cbxFolders.Items.Remove(folder);

                // Restore the selected index
                // NOTE: This should modify the selected folder
                cbxFolders.SelectedIndex = index;

                // Remove the folders column from the songs list and the headers dropdown
                this.dgvSongList.Columns.Remove(this.dgvSongList.Columns.OfType<DataGridViewColumn>().FirstOrDefault(x => x.HeaderText == folder.Name));
                cbSongListHeaders.Items.Remove(folder);
            }
        }

        // When the user deletes a search
        private void btnSearchDelete_Click(object sender, EventArgs e)
        {
            // If the user has a search selected
            CSavedSearch search = cbxSearchName.SelectedItem as CSavedSearch;
            if (search != null)
            {
                // Save the selected index
                int index = cbxSearchName.SelectedIndex;

                // Remove the folder from the saved data
                SavedOptions.Searches.Remove(search);

                // Remove the option from the combobox
                cbxSearchName.Items.Remove(search);

                // Restore the selected index
                // NOTE: This should modify the selected folder
                cbxSearchName.SelectedIndex = index;
            }
        }

        // When the user selects the custom song pack radio button
        private void radCustomSongPack_CheckedChanged(object sender, EventArgs e)
        {
            // If the user has a folder selected
            CSavedFolder folder = cbxFolders.SelectedItem as CSavedFolder;
            if ((folder != null)
            &&  (radCustomSongPack.Checked == true))
            {
                folder.Type = EFolderTypes.CustomSongPack;
            }
        }

        // When the user selects the filter radio button
        private void radFilter_CheckedChanged(object sender, EventArgs e)
        {
            // If the user has a folder selected
            CSavedFolder folder = cbxFolders.SelectedItem as CSavedFolder;
            if ((folder != null)
            &&  (radFilter.Checked == true))
            {
                folder.Type = EFolderTypes.Filter;
            }
        }

        #endregion

        private string GetExecutablePath(string strFilenameToAdd = "")
        {
            return System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/" + (strFilenameToAdd ?? "");
        }

        public void AddSong(CSong song)
        {
            try
            {
                this.Invoke(delegate
                {
                    this.cSongBindingSource.Add(song);
                });
            }
            catch (Exception e)
            {

            }
        }

        public void ClearSongs()
        {
            try
            {
                this.Invoke(delegate
                {
                    // Clear the list
                    cSongBindingSource.Clear();
                });
            }
            catch (Exception e)
            {

            }
        }

        private void OpenFolderBrowser(object sender, EventArgs e)
        {
            this.folderBrowserDialog.SelectedPath = Directory.GetCurrentDirectory();

            this.folderBrowserDialog.ShowDialog();

            // If a folder was selected, update the displayed directory
            if (this.folderBrowserDialog.SelectedPath.Length > 0)
            {
                this.txtSongsDirectory.Text = this.folderBrowserDialog.SelectedPath;
                this.txtSongsDirectory.SelectionStart = this.txtSongsDirectory.Text.Length - 1;
            }
        }

        private void DetermineParseButtonEnabledState(object sender, EventArgs e)
        {
            WriteSavedOptions();

            // We can only search/parse if the directory exists
            this.btnParse.Enabled = Directory.Exists(txtSongsDirectory.Text);
        }

        private void Generic_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox EventControl = (CheckBox)sender;
            ComboBox AssociatedControl = this.AssociatedControls[EventControl];

            if (AssociatedControl != null)
            {
                if (EventControl.Checked)
                {
                    AssociatedControl.Enabled = true;
                }
                else
                {
                    AssociatedControl.Enabled = false;
                    AssociatedControl.SelectedIndex = -1;
                    AssociatedControl.Text = "";
                }
            }

            DetermineParseButtonEnabledState(sender, e);
        }

        private void ParseButtonClick(Boolean bUseFilter)
        {
            if (IsParsing == false)
            {
                IsParsing = true;

                ChangeSongListSource(StepManiaParser.Songs);

                btnParse.Text = strCancelBtnText;
                AllowSongCollectionModification(false);
                AllowFilterEdits(false);

                // List of filters to use
                List<CFilterLogic> lstFilters = new List<CFilterLogic>();

                // Duplicates Filter
                if (this.chkExactDuplicates.Checked)
                {
                    lstFilters.Add(new CDuplicateFilter());
                }

                // Similar Song Filter
                if (this.chkAltFolder.Checked)
                {
                    int nSongSimilarity = 0;
                    switch (this.cboSongSimilarity.SelectedIndex)
                    {
                        case 0: nSongSimilarity = 4; break;
                        case 1: nSongSimilarity = 5; break;
                        case 2: nSongSimilarity = 6; break;
                        case 3: nSongSimilarity = 7; break;
                        case 4: nSongSimilarity = 8; break;
                        case 5: nSongSimilarity = 9; break;
                        case 6: nSongSimilarity = 10; break;
                        case 7: nSongSimilarity = 11; break;
                        case 8: nSongSimilarity = 12; break;
                        case 9: nSongSimilarity = 13; break;
                    }
                    lstFilters.Add(new CSimilarSongFilter(nSongSimilarity));
                }

                // Only include the unparsed song filter if we are parsing all songs
                if (SavedOptions.ParseType == EParseTypes.All)
                {
                    lstFilters.Add(new CUnparsedSongFilter());
                }

                // Add custom filters that user set up to the default list of built in filters
                List<string> lstFilterFolders = new List<string>();
                lstFilterFolders.AddRange(lstDefaultFilterFolders);
                lstFilterFolders.AddRange(SavedOptions.Folders?.Where(x => x.Type == EFolderTypes.Filter)?.Select(x => x.Name) ?? new List<string>());

                this.StepManiaParser.SetStartingData(
                    this,
                    SavedOptions.SongDirectory,
                    lstFilterFolders,
                    lstFilters
                );

                this.StepManiaParser.StartParsing();
            }
            else
            {
                this.StepManiaParser.StopParsing(ParseCancelComplete);                
            }
        }

        private void ParseCancelComplete()
        {
            ClearStatus();
            SetStatus("Operation Cancelled");
            ParseFilterComplete();
        }

        private void btnStartParse_Click(object sender, EventArgs e)
        {
            ParseButtonClick(false);
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            ParseButtonClick(true);
        }

        private void Options_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.StepManiaParser != null)
            {
                this.StepManiaParser.StopParsing(ParseCancelComplete);
            }
        }

        public void UpdateTitleAndText(string strText)
        {
            this.ClearStatus();
            this.SetStatus(strText);
            this.AddText("\n\n\n" + strText + "\n");
        }

        public void AddText(string strText)
        {
            try
            {
                this.Invoke(delegate
                {
                    this.rtbOutput.AppendText(strText);
                });
            }
            catch (Exception e)
            {

            }
        }

        public void SetStatus(string strText, int nLevel = 0)
        {
            int nIndex = 0;
            try
            {
                this.Invoke(delegate
                {
                    while ((lstStrStatus.Count - 1) < nLevel)
                    {
                        lstStrStatus.Add("");
                    }
                    lstStrStatus[nLevel] = strText;
                    strText = "";
                    for (nIndex = 0; nIndex < lstStrStatus.Count; nIndex++)
                    {
                        strText += lstStrStatus[nIndex] + ((lstStrStatus[nIndex].Length > 0) ? " " : "");
                    }
                    this.txtStatus.Text = strText;
                });
            }
            catch (Exception e)
            {

            }
        }

        public void ClearStatus()
        {
            try
            {
                this.Invoke(delegate
                {
                    for (int nIndex = 0; nIndex < lstStrStatus.Count; nIndex++)
                    {
                        lstStrStatus[nIndex] = "";
                    }
                });
            }
            catch (Exception e)
            {

            }
        }

        public void SetError(string strExceptionMessage)
        {
            SetStatus("ERROR :(");
            AddText("\nERROR: " + strExceptionMessage + "\n");
        }

        public void ParseFilterComplete()
        {
            AllowSongCollectionModification(true);
            AllowFilterEdits(true);

            // Update the proeprties on the selected song
            SelectedSong.ReplaceWith(StepManiaParser.Songs.FirstOrDefault(x => x.FolderPath == SelectedSong.FolderPath) ?? StepManiaParser.Songs.FirstOrDefault());

            try
            {
                this.Invoke(delegate
                {
                    btnParse.Text = strParseBtnText;
                    this.dgvSongList.Columns.OfType<DataGridViewColumn>().ToList().ForEach(x => x.AutoSizeMode = DataGridViewAutoSizeColumnMode.None);

                    // Update the data grids views to show the updated data
                    RefreshSongListSource();
                    dgvDifficulties.Update();
                    dgvDifficulties.Refresh();
                });
            }
            catch (Exception e)
            {

            }

            // Attempt to save song data into a file to speed up future operations
            if (SavedOptions.SaveFileAfterParse == true)
            {
                StepManiaParser.SaveSongDataUsingFile(StepManiaParser.lstAllSongs);
            }

            IsParsing = false;
        }

        public void AllowFilterEdits(bool bAllow)
        {
            try
            {
                this.Invoke(delegate
                {
                    foreach (CheckBox cb in AssociatedControls.Keys)
                    {
                        cb.Enabled = bAllow;
                    }
                    if (!bAllow)
                    {
                        foreach (ComboBox cb in AssociatedControls.Values)
                        {
                            if (cb != null)
                            {
                                cb.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        foreach (CheckBox cb in AssociatedControls.Keys)
                        {
                            Generic_CheckedChanged(cb, null);
                        }
                    }
                });
            }
            catch (Exception e)
            {

            }
        }

        public void AllowSongCollectionModification(bool bAllow)
        {
            // We cannot allow sorting while we are still modifying the collection, but after modifications are complete we should be able to
            try
            {
                this.Invoke(delegate
                {
                    foreach (DataGridViewColumn column in this.dgvSongList.Columns)
                    {
                        column.SortMode = bAllow ? DataGridViewColumnSortMode.Automatic : DataGridViewColumnSortMode.NotSortable;
                    }
                });
            }
            catch (Exception e)
            {

            }
        }

        // Adds the row number to the song list
        private void dgvSongList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        #region SaveLoad

        private void ReadSavedOptions()
        {
            CSavedOptions SavedOptionsFromFile = null;
            string strListFilePath = GetExecutablePath(strTempFileName);

            try
            {
                using (Stream stream = File.Open(strListFilePath, FileMode.Open))
                {
                    SavedOptionsFromFile = JsonSerializer.Deserialize<CSavedOptions>(stream);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            if (SavedOptionsFromFile != null)
            {
                foreach (PropertyInfo property in typeof(CSavedOptions).GetProperties().Where(p => p.CanWrite))
                {
                    property.SetValue(SavedOptions, property.GetValue(SavedOptionsFromFile, null), null);
                }
            }
        }

        private void WriteSavedOptions()
        {
            string strListFilePath = GetExecutablePath(strTempFileName);

            try
            {
                using (Stream stream = File.Open(strListFilePath, FileMode.Create))
                {
                    JsonSerializer.Serialize(stream, SavedOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private void Options_FormClosed(object sender, FormClosedEventArgs e)
        {
            WriteSavedOptions();
        }

        #endregion

        private void ParseType_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rad = sender as RadioButton;

            if (rad.Checked == true)
            {
                if (sender == radNoParse)
                {
                    this.SavedOptions.ParseType = EParseTypes.None;
                }
                else if (sender == radParseUnparsed)
                {
                    this.SavedOptions.ParseType = EParseTypes.Unparsed;
                }
                else if (sender == radParseAll)
                {
                    this.SavedOptions.ParseType = EParseTypes.All;
                }
            }
        }

        private void RadSearch_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rad = sender as RadioButton;

            if (rad.Checked == true)
            {
                if (sender == radSearchAnd)
                {
                    SelectedSearch.Type = ESearchTypes.AND;
                }
                else if (sender == radSearchOr)
                {
                    SelectedSearch.Type = ESearchTypes.OR;
                }
            }
        }

        private void cbxSearchName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnPlayPause_Click(object sender, EventArgs e)
        {
            try
            {
                PlayPauseSongLogic();
            }
            catch
            {
                // Do Nothing, we're missing the DLL
            }
        }

        private  void PlayPauseSongLogic()
        {
            try
            {
                if ((mediaPlayer as MediaPlayer)?.IsPlaying ?? false)
                {
                    (mediaPlayer as MediaPlayer).Stop();
                }
                else if ((SelectedSong?.MusicPath?.Length ?? 0) > 0)
                {
                    media = new Media(libVLC as LibVLC, new Uri(SelectedSong.MusicPath));
                    mediaPlayer = new MediaPlayer(media as Media) { EnableHardwareDecoding = true };
                    (mediaPlayer as MediaPlayer).Play();
                }
            }
            catch
            {
                // Do Nothing, we're missing the DLL
            }
        }

        #region Search

        // When a search is selected or loaded
        private void PopulateGuiForSearch(CSavedSearch search)
        {
            int sizeReduction = 0;

            // Clear all the existing search operands
            for (int row = 0; row < tlpSearchOperands.RowCount - 1; row++)
            {
                // Get the control from this row
                Control control = tlpSearchOperands.GetControlFromPosition(0, row);

                // Remove the control from this row
                tlpSearchOperands.Controls.Remove(control);

                // Update the size that we will need to subtract from the height of the table and container
                sizeReduction += control.Height;
            }

            // Move the "Add New" button to the first row
            tlpSearchOperands.SetRow(btnAddSearchOperand, 0);

            // Set the row count and update the sizes of the controls
            tlpSearchOperands.RowCount = 1;
            tlpSearchOperands.Height -= sizeReduction;
            tlpSearchOperands.Parent.Height -= sizeReduction;

            // Set the search type (AND/OR)
            switch (search.Type)
            {
                case ESearchTypes.AND: radSearchAnd.Checked = true; break;
                case ESearchTypes.OR:  radSearchOr.Checked = true;  break;
            }

            // Add controls to GUI for each operand in the search
            foreach (CSearchOperand operand in search.Operands)
            {
                AddSearchOperandToGui(search, operand);
            }
        }

        private void AddSearchOperandToGui(CSavedSearch search, CSearchOperand operand = null)
        {
            // Add a new row to the table
            tlpSearchOperands.RowCount += 1;

            // The new row should be autosized
            tlpSearchOperands.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Move the button to the last row
            tlpSearchOperands.SetCellPosition(btnAddSearchOperand, new TableLayoutPanelCellPosition(1, tlpSearchOperands.RowCount - 1));

            // If an operand wasn't supplied create one
            if (operand == null)
            {
                // Create a new saved search operand and add it to the currently selected search
                operand = new CSearchOperand();
                search.Operands.Add(operand);
                operand.Property = SearchUserControl.PropertyNames.FirstOrDefault();
            }

            // Create the new search control
            SearchUserControl control = SearchUserControl.GetControlOfProperType(tlpSearchOperands, search, operand);

            // Size the control to match the container
            control.Size = new Size(tlpSearchOperands.Width, control.Size.Height);

            // Add a new control to the added row (where the button used to be)
            tlpSearchOperands.Controls.Add(control, 1, tlpSearchOperands.RowCount - 2);

            // Ideally the auto-size propety of the containing groupbox would take care of resizing it, but it doesn't seem to do that very well
            grpSearch.Height += control.Size.Height;

            // Resize the table to fit it's contents
            tlpSearchOperands.Height = tlpSearchOperands.Controls.OfType<Control>().Sum(x => x.Height);
        }

        // When the user adds another search operand 
        private void btnAddSearchOperand_Click(object sender, EventArgs e)
        {
            // Add a new row to the table
            AddSearchOperandToGui(SelectedSearch);
        }

        // When the user apply's a search to the song list
        private void btnApplySearch_Click(object sender, EventArgs e)
        {
            SongSearch.StartStopSearch(SearchCancelComplete);
        }

        // What happens when a search is canceled or completed
        public void SearchCancelComplete(bool Canceled)
        {
            // Run the following on the GUI thread
            this.Invoke(delegate
            {
                btnApplySearch.Text = strSearchBtnText;

                // Update the status based on whether we completed or canceled the search
                UpdateTitleAndText(Canceled ? "Search Canceled" : "Search Complete");

                // Update the data grids views to show the updated data
                RefreshSongListSource();
            });
        }

        // When the user clears a search filter
        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            ChangeSongListSource(StepManiaParser.Songs);

            // Update the data grids views to show the updated data
            RefreshSongListSource();
        }

        #endregion

        private void btnExport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "csv files (*.csv)|*csv";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(saveFileDialog1.FileName))
                {                    
                    // Get a list of properties from the song class
                    IEnumerable<PropertyInfo> properties = typeof(CSong).GetProperties().Where(x => x.PropertyType != typeof(Image));

                    // Loop through each property and add it to the file
                    foreach (PropertyInfo property in properties)
                    {
                        writer.Write("\"" + property.Name + "\",");
                    }

                    // Loop through each song
                    foreach (CSong song in StepManiaParser.lstAllSongs)
                    {
                        // Start the song on a new line
                        writer.Write("\r\n");

                        // Loop through each property in the song
                        foreach (PropertyInfo property in properties)
                        {
                            writer.Write("\"" + (property.GetValue(song)?.ToString() ?? "") + "\",");
                        }
                    }
                }
            }
        }

        private void btnApplyAllVisible_Click(object sender, EventArgs e)
        {
            // Get the column associated with the selected folder
            DataGridViewColumn column = dgvSongList.Columns.Cast<DataGridViewColumn>().FirstOrDefault(x => x.HeaderText == SelectedFolder?.Name);

            // If a matching column was found
            if (column != null)
            {
                // Loop through all rows in the data grid            
                foreach (DataGridViewRow row in dgvSongList.Rows)
                {
                    // Get the cell associated with the checkbox
                    DataGridViewCheckBoxCell cell = row.Cells[column.Index] as DataGridViewCheckBoxCell;

                    // If the cell is currently unchecked
                    if ((bool)cell.Value == false)
                    {
                        // Check it
                        cell.Value = true;
                        // Simulate a click event to run the backend logic
                        dataGridViewHelpers.dgvSongList_CellContentClick(dgvSongList, new DataGridViewCellEventArgs(cell.ColumnIndex, cell.RowIndex));
                    }
                }
            }
        }

        private void btnClearAllVisible_Click(object sender, EventArgs e)
        {
            // Get the column associated with the selected folder
            DataGridViewColumn column = dgvSongList.Columns.Cast<DataGridViewColumn>().FirstOrDefault(x => x.HeaderText == SelectedFolder?.Name);

            // If a matching column was found
            if (column != null)
            {
                // Loop through all rows in the data grid            
                foreach (DataGridViewRow row in dgvSongList.Rows)
                {
                    // Get the cell associated with the checkbox
                    DataGridViewCheckBoxCell cell = row.Cells[column.Index] as DataGridViewCheckBoxCell;

                    // If the cell is currently checked
                    if ((bool)cell.Value == true)
                    {
                        // Check it
                        cell.Value = false;
                        // Simulate a click event to run the backend logic
                        dataGridViewHelpers.dgvSongList_CellContentClick(dgvSongList, new DataGridViewCellEventArgs(cell.ColumnIndex, cell.RowIndex));                        
                    }
                }
            }
        }
    }
}
