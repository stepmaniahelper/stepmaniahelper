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

namespace StepManiaHelper.Helpers
{
    internal class DataGridViewHelpers
    {
        public Options Form { get; set; }

        private int grpSongInfoBaseHeight;

        public DataGridViewHelpers(Options Form)
        {
            this.Form = Form;
            grpSongInfoBaseHeight = Form.grpSongInfo.Height;
        }

        public void SetupLogic()
        {
            // Set up the data grid views
            Form.ChangeSongListSource(Form.StepManiaParser.Songs);
            Form.cSongBindingSource.DataSource = Form.StepManiaParser.Songs;
            Form.dgvSongList.AutoGenerateColumns = true;
            Form.cDifficultyBindingSource.DataSource = Form.Difficulties;
            Form.dgvDifficulties.AutoGenerateColumns = true;
            Form.dgvSongList.Columns.OfType<DataGridViewImageColumn>()?.ToList().ForEach(x => x.ImageLayout = DataGridViewImageCellLayout.Zoom);
            Form.dgvSongList.Columns.OfType<DataGridViewColumn>().ToList().ForEach(x => x.AutoSizeMode = DataGridViewAutoSizeColumnMode.None);
            Form.dgvSongList.DataError += DgvSongList_DataError;

            // Build the list of options for the song list header drop down
            // NOTE: This will include custom folders if they have already been loaded
            foreach (DataGridViewColumn column in Form.dgvSongList.Columns)
            {
                Form.cbSongListHeaders.Items.Add(column.HeaderText);
            }
            // The below event handler will ensure the correct default checkbox value for the first header in the list
            Form.cbSongListHeaders.SelectedIndexChanged += cbSongListHeaders_SelectedIndexChanged;
            Form.cbSongListHeaders.SelectedIndex = 0;

            // Build the list of options for the difficulty list header drop down
            foreach (DataGridViewColumn column in Form.dgvDifficulties.Columns)
            {
                Form.cbDiffListHeaders.Items.Add(column.HeaderText);
            }
            // The below event handler will ensure the correct default checkbox value for the first header in the list
            Form.cbDiffListHeaders.SelectedIndexChanged += cbDiffListHeaders_SelectedIndexChanged;
            Form.cbDiffListHeaders.SelectedIndex = 0;

            // If song column styles weren't loaded, set some defaults
            // NOTE: Be sure to load custom folders before doing this
            if (Form.SavedOptions.SongColumns == null)
            {
                Form.SavedOptions.SongColumns = new List<string>();
                Form.SavedOptions.SongColumns.Add(nameof(CSong.Pack));
                Form.SavedOptions.SongColumns.Add(nameof(CSong.FolderName));
                Form.SavedOptions.SongColumns.Add(nameof(CSong.Difficulties));
            }
            else
            {
                // Remove columns that no longer exist (properties/folders were changed or removed)
                for (int i = Form.SavedOptions.SongColumns.Count - 1; i >= 0; i--)
                {
                    string header = Form.SavedOptions.SongColumns.ElementAt(i);
                    if ((typeof(CSong).GetProperty(header) == null)
                    && (Form.SavedOptions.Folders?.FirstOrDefault(x => x.Name == header) == null))
                    {
                        Form.SavedOptions.SongColumns.RemoveAt(i);
                    }
                }
            }

            // Before re-arranging and hiding columns, we need to load the columns for custom folders
            // Setup for the folder logic
            if (Form.SavedOptions.Folders != null)
            {
                foreach (CSavedFolder folder in Form.SavedOptions.Folders)
                {
                    // Add the folder's column to the songs list and the header dropdown
                    Form.AddFolderToSongList(folder);
                }
            }

            // Enable and re-arrange song columns based on the saved options
            foreach (DataGridViewColumn column in Form.dgvSongList.Columns)
            {
                int ColumnIndex = Form.SavedOptions.SongColumns.IndexOf(column.HeaderText);
                column.Visible = (ColumnIndex != -1);
            }
            // Must re-arrange the order of columns by increasing display index, 
            // because higher number display indexes can be modified automatically 
            // when modifying lower number display indexes
            foreach (string Header in Form.SavedOptions.SongColumns)
            {
                DataGridViewColumn column = Form.dgvSongList.Columns.OfType<DataGridViewColumn>().FirstOrDefault(x => x.HeaderText == Header);
                if (column != null)
                {
                    column.DisplayIndex = Form.SavedOptions.SongColumns.IndexOf(column.HeaderText);
                }
            }

            // If difficulty column styles weren't loaded, set some defaults
            if (Form.SavedOptions.DiffColumns == null)
            {
                Form.SavedOptions.DiffColumns = new List<string>();
                Form.SavedOptions.DiffColumns.Add(nameof(CDifficulty.Difficulty));
                Form.SavedOptions.DiffColumns.Add(nameof(CDifficulty.Notes));
            }
            else
            {
                // Remove columns that no longer exist (properties were changes or removed)
                for (int i = Form.SavedOptions.DiffColumns.Count - 1; i >= 0; i--)
                {
                    if (typeof(CDifficulty).GetProperty(Form.SavedOptions.DiffColumns.ElementAt(i)) == null)
                    {
                        Form.SavedOptions.DiffColumns.RemoveAt(i);
                    }
                }
            }

            // Enable and re-arrange difficulty columns based on the saved options
            foreach (DataGridViewColumn column in Form.dgvDifficulties.Columns)
            {
                int ColumnIndex = Form.SavedOptions.DiffColumns.IndexOf(column.HeaderText);
                if (ColumnIndex == -1)
                {
                    column.Visible = false;
                }
            }
            // Must re-arrange the order of columns by increasing display index, 
            // because higher number display indexes can be modified automatically 
            // when modifying lower number display indexes
            foreach (string Header in Form.SavedOptions.DiffColumns)
            {
                DataGridViewColumn column = Form.dgvDifficulties.Columns.OfType<DataGridViewColumn>().FirstOrDefault(x => x.HeaderText == Header);
                if (column != null)
                {
                    column.DisplayIndex = Form.SavedOptions.DiffColumns.IndexOf(column.HeaderText);
                }
            }

            // Don't add event handlers until all of the above setup has occurred

            // Add event handlers for the song list            
            Form.chkSongHeaderVisible.CheckedChanged += chkHeaderVisible_CheckedChanged;
            Form.dgvSongList.ColumnHeaderMouseClick += DgvSongList_ColumnHeaderMouseClick;
            Form.dgvSongList.CellContentClick += dgvSongList_CellContentClick;
            Form.dgvSongList.RowsAdded += dgvSongList_RowsAdded;
            Form.dgvSongList.ColumnDisplayIndexChanged += dgvSongList_ColumnDisplayIndexChanged;
            Form.dgvSongList.SelectionChanged += DgvSongList_SelectionChanged;

            // Add data bindings for the song info section
            Form.txtPack.DataBindings.Add(nameof(TextBox.Text), Form.SelectedSong, nameof(CSong.Pack));
            Form.txtFolderName.DataBindings.Add(nameof(TextBox.Text), Form.SelectedSong, nameof(CSong.FolderName));

            // Add event handlers for the song info section
            Form.chkDiffHeaderVisible.CheckedChanged += ChkDiffHeaderVisible_CheckedChanged;
            Form.dgvDifficulties.ColumnDisplayIndexChanged += DgvDifficulties_ColumnDisplayIndexChanged;
            Form.dgvDifficulties.SelectionChanged += DgvDifficulties_SelectionChanged;
            Form.dgvDifficulties.DataBindingComplete += DgvDifficulties_DataBindingComplete;

            // Set the default size of the difficulties data grid view
            UpdateDgvHeight(Form.dgvDifficulties);
        }

        // Used to sort the checkbox colums which aren't databound
        private void DgvSongList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Get the clicked column
            DataGridViewColumn column = Form.dgvSongList.Columns[e.ColumnIndex];

            // If sorting one of the columns that normally doesn't support sorting
            /*if (column is DataGridViewCheckBoxColumn)
            {
                SortOrder order = column.HeaderCell.SortGlyphDirection;

                // Attempt to get the folder associated with the column
                CSavedFolder folder = column.Tag as CSavedFolder;
                Form.dgvSongList.Sort(Comparer<CSong>.Create((x, y) =>
                {
                    if (GetValueOfFolderCheckbox(folder, x) == GetValueOfFolderCheckbox(folder, y))
                    {
                        return 0;
                    }
                    return (GetValueOfFolderCheckbox(folder, x) == true) ? 1 : -1;
                }));
                column.HeaderCell.SortGlyphDirection = (order == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            }*/
        }

        private void DgvSongList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Console.Write("Data Error in the songs list?");
            e.Cancel = true;
        }

        #region DataGridViewEventHandlers

        // Click event handler for the song list, used to handle the folder checkboxes
        public void dgvSongList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Get the column the click occurred in
            DataGridViewColumn column = Form.dgvSongList.Columns[e.ColumnIndex];

            // Attempt to determine what song was clicked
            DataGridViewRow row = (e.RowIndex >= 0) ? Form.dgvSongList.Rows[e.RowIndex] : null;
            CSong song = row?.DataBoundItem as CSong;

            // If the column has checkboxes, allow the checkbox change to occur before doing further logic
            if (column is DataGridViewCheckBoxColumn)
            {
                Form.dgvSongList.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }

            // Find the folder associated with this column
            CSavedFolder folder = Form.SavedOptions.Folders?.FirstOrDefault(x => x.Name == column.HeaderText);

            // If a matching folder was found
            if ((folder != null)
            && (song != null))
            {
                // Determine if the checkbox is checked or not
                bool value = (bool)row.Cells[e.ColumnIndex].Value;

                // If this is a custom song pack folder
                if (folder.Type == EFolderTypes.CustomSongPack)
                {
                    song.MoveCustomPackSong(folder.Name, value);
                }
                // If this is a filter folder
                else if (folder.Type == EFolderTypes.Filter)
                {
                    song.MoveFilterSong(value ? folder.Name : null);
                }
            }
            else
            {
                Console.Write("Unable to toggle custom folder?");
            }
        }

        public bool GetValueOfFolderCheckbox(CSavedFolder folder, CSong song)
        {
            bool Value = false;

            // For filter folders, we care if it's in the filter folder
            if (folder.Type == EFolderTypes.Filter)
            {
                Value = song.strFolderPath.Contains("\\" + folder.Name + "\\");
            }
            // For custom song pack folders, we care if there's a copy in the custom song pack
            else if (folder.Type == EFolderTypes.CustomSongPack)
            {
                try
                {
                    string NewPath = song.strFolderPath + "\\..\\..\\..\\Songs\\" + folder.Name + "\\" + song.FolderName;
                    NewPath = Path.GetFullPath((new Uri(NewPath)).LocalPath);
                    Value = Directory.Exists(NewPath);
                }
                catch
                {

                }
            }

            return Value;
        }

        // Handler for when rows are added, to allow us to set non-data-bound values
        public void dgvSongList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // Loop through all the added rows
            int endIndex = e.RowIndex + e.RowCount;
            for (int indexRow = e.RowIndex; indexRow < endIndex; indexRow++)
            {
                // Get the current row and song associated with it
                DataGridViewRow row = Form.dgvSongList.Rows[indexRow];
                CSong song = row.DataBoundItem as CSong;

                // Only continue if the row represents a song
                if (song != null)
                {
                    // Loop through all the columns in the data grid view
                    for (int indexCol = 0; indexCol < Form.dgvSongList.Columns.Count; indexCol++)
                    {
                        // Attempt to get the folder associated with the column
                        CSavedFolder folder = Form.dgvSongList.Columns[indexCol].Tag as CSavedFolder;

                        // Attempt to get the cell assocaited with the column
                        DataGridViewCheckBoxCell cell = row.Cells[indexCol] as DataGridViewCheckBoxCell;

                        // If there is a folder associated with the column
                        if ((folder != null)
                        && (cell != null))
                        {
                            cell.Value = GetValueOfFolderCheckbox(folder, song);
                        }
                    }
                }
            }
        }

        // Helper function for determining displayindex after omitting hidden columns
        public int GetColumnVisibleDisplayIndex(DataGridViewColumn target)
        {
            int DisplayIndex = 0;
            int VisibleDisplayIndex = 0;

            List<DataGridViewColumn> Columns = target.DataGridView?.Columns?.OfType<DataGridViewColumn>()?.ToList();

            for (DisplayIndex = 0; DisplayIndex < Columns.Count; DisplayIndex++)
            {
                DataGridViewColumn column = Columns.FirstOrDefault(x => x.DisplayIndex == DisplayIndex);
                if (target == column)
                {
                    break;
                }
                if (column?.Visible ?? false)
                {
                    VisibleDisplayIndex += 1;
                }
            }

            return VisibleDisplayIndex;
        }

        // Helper function to update the saved data and column visibility when hiding/showing a column in a data grid view
        public void ChangeDgvColumnVisibility(DataGridView dgv, ComboBox combo, CheckBox check, List<string> lstCols)
        {
            string headerText = (combo.SelectedItem as string) ?? (combo.SelectedItem as CSavedFolder)?.Name;
            DataGridViewColumn Column = dgv?.Columns?.OfType<DataGridViewColumn>()?.ToList()?.FirstOrDefault(x => x.HeaderText == headerText);
            if (Column != null)
            {
                Column.Visible = check.Checked;
                if (Column.Visible)
                {
                    // Prevent the same item from being added to the list twice somehow
                    if (!lstCols.Contains(Column.HeaderText))
                    {
                        lstCols.Insert(GetColumnVisibleDisplayIndex(Column), Column.HeaderText);
                    }
                }
                else
                {
                    lstCols.RemoveAll(x => x == Column.HeaderText);
                }
            }
        }

        public void ColumnMoved(List<string> lstCols, DataGridViewColumn dgvc)
        {
            // Only move the column if it was visible, and we aren't changing the list source
            if ((lstCols.Contains(dgvc.HeaderText) == true)
            &&  (Form.ChangingSongListSource == false))
            {
                lstCols.Remove(dgvc.HeaderText);
                lstCols.Insert(GetColumnVisibleDisplayIndex(dgvc), dgvc.HeaderText);
            }
        }

        // When the user hides a column in the song list grid view, we need to update our saved data to match
        public void chkHeaderVisible_CheckedChanged(object sender, EventArgs e)
        {
            ChangeDgvColumnVisibility(Form.dgvSongList, Form.cbSongListHeaders, Form.chkSongHeaderVisible, Form.SavedOptions.SongColumns);
        }

        // When the user re-orders the columns in the song list grid view, we need to update our saved data to match
        public void dgvSongList_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnMoved(Form.SavedOptions.SongColumns, e.Column);
        }

        public void DgvSongList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Clicking a column in the song list grid view will select that column from the headers combobox
            Form.cbSongListHeaders.SelectedItem = Form.dgvSongList?.Columns?.OfType<DataGridViewColumn>()?.ToList()?.FirstOrDefault(x => x.Index == e.ColumnIndex)?.HeaderText;

            // When clicking on a header, the index will be negative, avoid problems created by that
            if ((e.RowIndex >= 0)
            && (e.RowIndex < (Form.StepManiaParser?.Songs?.Count ?? 0)))
            {
                // We only need to replace the song if it's not the same one
                if (Form.SelectedSong.strFolderPath != Form.StepManiaParser?.Songs?.ElementAt(e.RowIndex)?.strFolderPath)
                {
                    Form.SelectedSong.ReplaceWith(Form.StepManiaParser?.Songs?.ElementAt(e.RowIndex));
                    Form.txtPack.DataBindings.OfType<Binding>().FirstOrDefault().ReadValue();
                    Form.txtFolderName.DataBindings.OfType<Binding>().FirstOrDefault().ReadValue();
                    Form.Difficulties.ResetBindings();
                }
            }
        }

        public void DgvSongList_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewCell cell = Form.dgvSongList.SelectedCells.OfType<DataGridViewCell>().FirstOrDefault();
            if (cell != null)
            {
                DgvSongList_CellClick(sender, new DataGridViewCellEventArgs(cell.ColumnIndex, cell.RowIndex));
            }
        }

        // When the song list header dropdown selects a new item, we have to update the checkbox state to match the new header
        public void cbSongListHeaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            Form.chkSongHeaderVisible.Checked = Form.dgvSongList?.Columns?.OfType<DataGridViewColumn>()?.ToList()?.FirstOrDefault(x => x.HeaderText == Form.cbSongListHeaders.SelectedItem as string)?.Visible ?? false;
        }

        // When the user hides a column in the difficulty grid view, we need to update our saved data to match
        public void ChkDiffHeaderVisible_CheckedChanged(object sender, EventArgs e)
        {
            ChangeDgvColumnVisibility(Form.dgvDifficulties, Form.cbDiffListHeaders, Form.chkDiffHeaderVisible, Form.SavedOptions.DiffColumns);
        }

        // When the user re-orders the columns in the difficulty grid view, we need to update our saved data to match
        public void DgvDifficulties_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnMoved(Form.SavedOptions.DiffColumns, e.Column);
        }

        // Clicking a column in the difficulties grid view will select that column from the combobox
        public void DgvDifficulties_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Form.cbDiffListHeaders.SelectedItem = Form.dgvDifficulties?.Columns?.OfType<DataGridViewColumn>()?.ToList()?.FirstOrDefault(x => x.Index == e.ColumnIndex)?.HeaderText;
        }

        public void DgvDifficulties_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewCell cell = Form.dgvDifficulties.SelectedCells.OfType<DataGridViewCell>().FirstOrDefault();
            if (cell != null)
            {
                DgvDifficulties_CellClick(sender, new DataGridViewCellEventArgs(cell.ColumnIndex, cell.RowIndex));
            }
        }

        // When the difficulty header dropdown selects a new item, we have to update the checkbox state to match the new header
        public void cbDiffListHeaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            Form.chkDiffHeaderVisible.Checked = Form.dgvDifficulties?.Columns?.OfType<DataGridViewColumn>()?.ToList()?.FirstOrDefault(x => x.HeaderText == Form.cbDiffListHeaders.SelectedItem as string)?.Visible ?? false;
        }

        // The difficulties data grid view should resize itself to fit the number of difficulties, we do this by subscribing to the row added and removed events
        public void UpdateDgvHeight(DataGridView dgv)
        {
            int height = dgv.ColumnHeadersHeight;

            foreach (DataGridViewRow row in dgv.Rows)
                height += row.Height + 1; // I dont think the height property includes the cell border size, so + 1

            dgv.MinimumSize = new Size(dgv.MinimumSize.Width, height);
            dgv.MaximumSize = new Size(int.MaxValue, height);

            // Update the height of the container since the auot-size property doesn't appear to do what it's supposed to
            // NOTE: The column header height is alrady included in the base.
            Form.grpSongInfo.Height = grpSongInfoBaseHeight + (height - dgv.ColumnHeadersHeight);
        }

        public void DgvDifficulties_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            UpdateDgvHeight(sender as DataGridView);
        }

        #endregion
    }
}
