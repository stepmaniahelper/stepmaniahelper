using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Session;
using Shell32;
using StepManiaHelper.Helpers;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StepManiaHelper.Logic
{
    internal class CGameMonitor
    {
        public Options Owner;
        private KeyboardHook Hook = new KeyboardHook();
        private FileInfo Executable;
        private TraceEventSession Session;
        private Thread BackgroundThread;
        private bool RunThread = false;
        private bool IsRunning = false;
        private CSong SelectedSong = null;
        private Dictionary<Tuple<ModifierKeys, Keys>, CSavedFolder> Hotkeys = new Dictionary<Tuple<ModifierKeys, Keys>, CSavedFolder>();
        private Dictionary<CSong, CSavedFolder> PendingEdits = new Dictionary<CSong, CSavedFolder>();
        public CGameMonitor(Options options) 
        {
            Owner = options;
            Hook.KeyPressed += Hook_KeyPressed;
        }

        public void RegisterHotKey(ModifierKeys modifier, Keys key, CSavedFolder cSavedFolder)
        {
            Hook.RegisterHotKey(modifier, key);
            Hotkeys.Add(new Tuple<ModifierKeys, Keys>(modifier, key), cSavedFolder);
        }

        private void Hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (SelectedSong != null)
            {
                Tuple<ModifierKeys, Keys>? key = Hotkeys.Keys.FirstOrDefault(x => (x.Item1 == e.Modifier) && (x.Item2 == e.Key));
                if (key != null)
                {
                    CSavedFolder folder = Hotkeys[key];
                    if (folder != null)
                    {
                        ApplyFolderToSong(SelectedSong, folder);
                    }
                }
            }
        }

        private void ApplyFolderToSong(CSong song, CSavedFolder folder)
        {
            // Force any pending edits to be applied
            Owner.dgvSongList.EndEdit();

            // Get the column associated with the selected folder
            DataGridViewColumn column = Owner.dgvSongList.Columns.Cast<DataGridViewColumn>().FirstOrDefault(x => x.HeaderText == folder?.Name);

            // If a matching column was found
            if (column != null)
            {
                // Loop through all rows in the data grid            
                foreach (DataGridViewRow row in Owner.dgvSongList.Rows)
                {
                    // If this is the row for the specified song
                    if (row.DataBoundItem == song)
                    {
                        // Get the cell associated with the checkbox
                        DataGridViewCheckBoxCell cell = row.Cells[column.Index] as DataGridViewCheckBoxCell;

                        // Determined the new checked state
                        bool newValue = ((cell.Value == null) || ((bool)cell.Value == false)) ? true : false;

                        // Duplicating or restoring a filtered song can always be done,
                        // and modifications to the non-selected-song can also always be done
                        if ((folder.Type == EFolderTypes.CustomSongPack)
                        ||  (newValue == false)
                        ||  (song != SelectedSong))
                        {
                            // Run the proper folder logic
                            if (folder.Toggle(song, newValue) == true)
                            {
                                // If the logic was successful, update the GUI to match the new state
                                cell.Value = newValue;
                            }
                            // Delete any pending edits for this song
                            if (PendingEdits.ContainsKey(song))
                            {
                                PendingEdits.Remove(song);
                            }
                        }
                        // Filtering a song cannot be done while the song is selected,
                        // so create a pending edit for it. This will be applied when
                        // the song selection changes.
                        else
                        {
                            PendingEdits[song] = folder;
                        }
                    }
                }
            }
        }

        public void FindExecutable(string path)
        {
            Executable = null;
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            // Loop until we find the executable or run out of folders to search
            while ((directoryInfo?.Exists ?? false)
            &&     (Executable == null))
            {
                // Loop through all sub-folders
                DirectoryInfo folder = directoryInfo.GetDirectories()?.FirstOrDefault(x => x.Name == "Program");
                Executable = folder?.GetFiles()?.FirstOrDefault(x => x.Extension == ".exe");

                // Search the parent if the executable wasn't found
                directoryInfo = directoryInfo.Parent;
            }

            Owner.txtMonitorExe.Text = Executable?.Name ?? "N/A";
            Owner.btnMonitor.Enabled = (Executable != null);
        }

        public void ToggleMonitoring()
        {
            if (IsRunning == false)
            {
                Owner.btnMonitor.Text = "Stop Monitoring";
                IsRunning = true;
                BackgroundThread = new Thread(Monitor);
                this.BackgroundThread.Start();
            }
            else
            {
                // To prevent locking up the GUI thread if the session doesn't
                // terminate in a timely manner, we run this logic in a separate thread.
                new Thread(() =>
                {
                    Session?.Source?.Dispose();
                    Session?.Dispose();
                    Session = null;
                    BackgroundThread.Join();
                    IsRunning = false;
                    Owner.btnMonitor.Text = "Start Monitoring";
                    Owner.txtMonitorSong.Text = "N/A";
                    SelectedSong = null;

                }).Start();
            }
        }

        private void Monitor()
        {
            try
            {
                if (Session == null)
                {
                    Session = new TraceEventSession("FileAccessSession");
                    Session.EnableKernelProvider(KernelTraceEventParser.Keywords.FileIOInit |
                                                 KernelTraceEventParser.Keywords.FileIO);
                    Session.Source.Kernel.FileIOCreate += Kernel_FileIOCreate;
                }
                Owner.txtMonitorSong.BeginInvoke(new Action(() => { Owner.txtMonitorSong.Text = "N/A"; }));
                SelectedSong = null;
                // The below call will block
                Session.Source.Process();
            }
            catch (Exception ex)
            {
                Session = null;
                MessageBox.Show($"{ex.Message}", "Error Monitoring Game",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Kernel_FileIOCreate(FileIOCreateTraceData obj)
        {
            if (((obj.FileName?.Length ?? 0) > 0)
            && (obj.ProcessName?.Contains(Executable?.Name?.Replace(Executable?.Extension, ""), StringComparison.OrdinalIgnoreCase) == true))
            {
                FileInfo file = new FileInfo(obj.FileName);
                DirectoryInfo songFolder = file?.Directory;
                DirectoryInfo packFolder = songFolder?.Parent;

                CSong song = Owner.StepManiaParser.lstAllSongs.FirstOrDefault(x => 
                    (x.FolderName == songFolder.Name) &&
                    (new DirectoryInfo(x.FolderPath)?.Parent.Name == packFolder.Name));

                if (song != null)
                {
                    CSong OldSelectedSong = SelectedSong;
                    SelectedSong = song;
                    // If the selected song changed, apply any pending edits
                    if ((OldSelectedSong != null)
                    &&  (SelectedSong != OldSelectedSong)
                    &&  (PendingEdits.ContainsKey(OldSelectedSong)))
                    {
                        ApplyFolderToSong(OldSelectedSong, PendingEdits[OldSelectedSong]);
                    }
                    
                    if (Owner.txtMonitorSong.Text != SelectedSong.FolderName)
                    {
                        Owner.txtMonitorSong.BeginInvoke(new Action(() => { Owner.txtMonitorSong.Text = SelectedSong.FolderName; }));
                    }
                }
            }
        }
    }
}
