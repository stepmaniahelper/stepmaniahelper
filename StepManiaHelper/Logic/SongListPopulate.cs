using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Shell32;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Data.Entity.Internal;
using System.Drawing;
using System.Reflection;
using StepManiaHelper.Helpers;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json;

namespace StepManiaHelper
{
    internal class CSongListPopulator
    {
        static private string strTempFileName = "StepManiaHelper.json";

        private Options OutputForm;

        private Thread BackgroundThread;
        private bool RunThread = true;

        private String strInputFolderPath;
        private String strStepmaniaSongFolderPath;
        private List<CFilterLogic> lstFilters;
        private List<string> lstFilterFolders;
        public SortableBindingList<CSong> Songs { get; }
        public List<CSong> lstAllSongs;
        private List<CSong> lstRemainingSongs;

        public CSongListPopulator()
        {
            this.OutputForm = null;
            this.strInputFolderPath = "";
            this.strStepmaniaSongFolderPath = "";
            this.lstFilters = new List<CFilterLogic>();
            this.lstFilterFolders = new List<string>();
            this.lstAllSongs = new List<CSong>();
            this.lstRemainingSongs = new List<CSong>();
            this.Songs = new SortableBindingList<CSong>(this.lstAllSongs);
        }

        public void SetStartingData(Options OutputForm,
                                    String strInputFolderPath,
                                    List<string> lstFilterFolders,
                                    List<CFilterLogic> lstFilters)
        {
            this.OutputForm = OutputForm;
            this.strInputFolderPath = strInputFolderPath;
            this.strStepmaniaSongFolderPath = strInputFolderPath;
            CFilterLogic.SetStepmaniaSongFolderPath(strInputFolderPath);
            this.lstFilters = lstFilters ?? new List<CFilterLogic>();
            this.lstFilterFolders = lstFilterFolders ?? new List<string>();
        }

        private Shell32.Folder GetShell32Folder(string folderPath)
        {
            Type shellAppType = Type.GetTypeFromProgID("Shell.Application");
            Object shell = Activator.CreateInstance(shellAppType);
            return (Shell32.Folder)shellAppType.InvokeMember("NameSpace",
            System.Reflection.BindingFlags.InvokeMethod, null, shell, new object[] { folderPath });
        }

        public void StartParsing()
        {
            this.BackgroundThread = new Thread(ParseFolderStart);
            this.RunThread = true;
            this.BackgroundThread.Start();
        }

        public void StopParsing(Action Callback)
        {
            new Thread(() =>
            {
                // The background thread will need to run stuff on the GUI thread, so the GUI 
                // thread can't lock here waiting for the background thread to exit. Instead 
                // we wait in a separate thread with a callback to handle when the thread completes
                this.RunThread = false;
                this.BackgroundThread?.Join();
                Callback();
            }).Start();            
        }

        public void ParseFolderStart()
        {
            bool bSuccessful = false;

            // Search for new songs if necessary
            bSuccessful = this.SearchForNewSongs();

            // If any songs were detected, output the effectiveness stats of the StepMania Helper
            if (bSuccessful)
            {
                // Now that we have a list of all the songs, parse the song files for more information
                this.ProcessSongFiles();

                // Now that we have all the needed data, apply any filters that need to be applied
                this.FilterSongs();
                    
                // Output final statistical information for the user
                OutputForm.AddText("\n\n\n" + this.lstAllSongs.Count.ToString() + " total songs parsed.\n");
                OutputForm.AddText(this.lstRemainingSongs.Count.ToString() + " songs remain unmodified.\n");

                // Get the result string for each filter in the list
                foreach (CFilterLogic Filter in this.lstFilters)
                {
                    OutputForm.AddText(Filter.GetResultString());
                }

                OutputForm.AddText("COMPLETE!!!\n");
                OutputForm.ClearStatus();
                OutputForm.SetStatus("All Songs Parsed");
                OutputForm.ParseFilterComplete();
            }
            // The user provided an incorrect folder selection
            else
            {
                OutputForm.ClearStatus();
                OutputForm.SetStatus("Song Detection Failed");
                OutputForm.AddText("\n\n\nNo songs detected. Ensure that the selected folder is either a song-pack directory, the stepmania songs directory, or the stepmania directory.\n");
                OutputForm.ParseFilterComplete();
            }
        }

        public void UpdateSongDataUsingFile(List<CSong> ListToUpdate)
        {
            List<CSong> ListFileSongs = null;
            CSong MatchingSong = null;
            string strListFilePath = strStepmaniaSongFolderPath + "/" + strTempFileName;

            // Open and read the created list file if it exists
            try
            {
                using (Stream stream = File.Open(strListFilePath, FileMode.Open))
                {
                    ListFileSongs = JsonSerializer.Deserialize<List<CSong>>(stream);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            // If the list file was successfully read, copy its data into our parsed song list
            if (ListFileSongs != null)
            {
                // Update the current progress so the user knows what's going on
                OutputForm.UpdateTitleAndText("Loading Song Information From Binary File");

                foreach (CSong FileSong in ListFileSongs)
                {
                    // See if the song already exists in the list                    
                    IEnumerable<CSong> MatchingSongs = ListToUpdate.Where(x => x.FolderPath == FileSong.FolderPath);

                    // If it's already in the list, we can only overwrite data which isn't already populated
                    if (MatchingSongs.Any())
                    {
                        MatchingSong = MatchingSongs.First();
                        MatchingSong.ReplaceWith(FileSong, true);
                        MatchingSong.bFlagged = true;
                    }
                    // If it's not in the list, and we didn't search for the current list of songs, add the new song
                    else if (OutputForm.SavedOptions.SearchForNewSongs == false)
                    {
                        // Loop through each difficulty and add them to the global list
                        if (FileSong?.aDifficulties != null)
                        {
                            foreach (CDifficulty Difficulty in FileSong.aDifficulties)
                            {
                                CDifficulty.AddDifficulty(Difficulty);
                            }
                        }

                        FileSong.bFlagged = true;
                        OutputForm.AddSong(FileSong);
                    }
                }
            }
        }

        public void SaveSongDataUsingFile(List<CSong> ListToSave)
        {
            string strListFilePath = strStepmaniaSongFolderPath + "/" + strTempFileName;

            // Save the created list to file for future fast access
            try
            {
                using (Stream stream = File.Open(strListFilePath, FileMode.Create))
                {
                    JsonSerializer.Serialize(stream, ListToSave);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public Boolean SearchForNewSongs()
        {
            Boolean bSuccessful = false;
            DirectoryInfo SelectedFolder = null;

            // If we're searching for new songs
            if (OutputForm.SavedOptions.SearchForNewSongs == true)
            { 
                // Update the current progress so the user knows what's going on
                OutputForm.ClearStatus();
                OutputForm.SetStatus("Searching For Songs");

                // Attempt to open the folder the user selected          
                try
                {
                    SelectedFolder = new DirectoryInfo(this.strInputFolderPath);
                }
                catch (Exception ex)
                {
                    OutputForm.SetError(ex.Message);
                }

                // If the folder was successfully opened, attempt to find songs
                if (SelectedFolder != null)
                {
                    // Attempt to treat the selected folder as song pack folder
                    bSuccessful = ParseSongPackFolder(SelectedFolder);

                    // If it was not a song pack folder, attempt to treat it as the songs folder
                    if (bSuccessful == false)
                    {
                        bSuccessful = ParseStepmaniaSongsFolder(SelectedFolder);

                        // If it was not the songs folder, attempt to treat it as the stepmania folder
                        if (bSuccessful == false)
                        {
                            bSuccessful = ParseStepmaniaFolder(SelectedFolder);
                        }
                    }
                }
            }
            // If we aren't looking for new songs, we successfully didn't
            else
            {
                bSuccessful = true;
            }

            return bSuccessful;
        }

        public void ProcessSongFiles()
        {
            // Clear the song flags
            CFilterLogic.ClearSongFlags(this.lstAllSongs);

            // Attempt to load song data from a temporary file, if it exists
            if (OutputForm.SavedOptions.LoadSaveFile == true)
            {
                UpdateSongDataUsingFile(this.lstAllSongs);
            }

            // Clear the list of songs to parse in preparation for the below logic
            this.lstRemainingSongs.Clear();

            // If we need to parse all songs
            if (OutputForm.SavedOptions.ParseType == Helpers.EParseTypes.All)
            {
                this.lstRemainingSongs.AddRange(lstAllSongs);
            }
            // If we only need to parse songs that are missing data
            else if (OutputForm.SavedOptions.ParseType == Helpers.EParseTypes.Unparsed)
            {
                // If the new song already has been parsed though, set its parsed flag
                this.lstAllSongs.ForEach(x =>
                {
                    // If we only care about properties in the song list
                    if (OutputForm.SavedOptions.DetectOnlyDisplayedData)
                    {
                        foreach (string prop in OutputForm.SavedOptions.SongColumns)
                        {
                            if (typeof(CSong).GetProperties().FirstOrDefault(y => y.Name == prop).GetValue(x, null) == null)
                            {
                                this.lstRemainingSongs.Add(x);
                                break;
                            }
                        }

                    }
                    // If we care about all properties
                    else
                    {
                        PropertyInfo[] props = typeof(CSong).GetProperties();
                        foreach (PropertyInfo prop in props)
                        {
                            if (prop.GetValue(x, null) == null)
                            {
                                this.lstRemainingSongs.Add(x);
                                break;
                            }
                        }
                    }
                });
            }
            // If we don't need to parse songs, the list will remain empty from the above clear

            // If there are new songs
            if (this.lstRemainingSongs.Count > 0)
            {
                // Update the current progress so the user knows what's going on
                OutputForm.UpdateTitleAndText("Processing Song Files");
            }
            else
            {
                OutputForm.AddText("\n\n\nNo Songs to Parse\n");
            }

            // Count the number of unflagged songs (they need to be parsed)
            int nParsedCount = 0;

            // Loop through every song that was identified
            foreach (CSong ParsedSong in this.lstRemainingSongs)
            {
                // Gracefully handle aborting the thread
                if (!RunThread)
                {
                    break;
                }

                // Let the user know which song we're currently parsing files of
                OutputForm.AddText("\t" + ParsedSong.FolderName + "\n");
                OutputForm.SetStatus("(Song " + nParsedCount + " of " + this.lstRemainingSongs.Count + ")", 1);

                // Attempt to get the duration of the MP3, WAV, or OGG file
                try
                {
                    Folder folder = GetShell32Folder(ParsedSong.FolderPath);
                    FolderItem folderItem = folder.ParseName(ParsedSong.MusicPath.Substring(ParsedSong.FolderPath.Length).Trim('\\'));
                    string strLength = folder.GetDetailsOf(folderItem, 27).Trim();
                    string[] strHms = strLength.Split(':');
                    ParsedSong.SongLength = (int.Parse(strHms[0]) * 60 * 60) + (int.Parse(strHms[1]) * 60) +(int.Parse(strHms[2]));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }

                // Parse the stepfiles for the song and populate the rest of the members
                CSong.ParseSong(ParsedSong, OutputForm.SavedOptions);
                nParsedCount += 1;

                // Loop through each difficulty and add them to the global list
                if (ParsedSong?.aDifficulties != null)
                {
                    foreach (CDifficulty Difficulty in ParsedSong.aDifficulties)
                    {
                        CDifficulty.AddDifficulty(Difficulty);
                    }
                }

                // If this song wasn't loaded from the song list file
                if (ParsedSong.bFlagged == false)
                {
                    // Let the user know which song data was correctly parsed
                    OutputForm.AddText("\t\t" + ParsedSong.GetSongDataString(ESongData.EAdvanced) + "\n");
                }
            }
        }

        public void FilterSongs()
        {
            int nFilter = 0;
            int nFilterCount = this.lstFilters.Count;

            // Perform the filter logic for each filter in the list
            foreach (CFilterLogic Filter in this.lstFilters)
            {
                // Gracefully handle aborting the thread
                if (!RunThread)
                {
                    break;
                }

                OutputForm.SetStatus("(Filter " + nFilter + " of " + nFilterCount + ")", 1);
                Filter.Filter(OutputForm, this.lstAllSongs);
                nFilter += 1;
            }
        }

        static public bool HasExtension(string strFileName, string strExtension)
        {
            return Path.GetExtension(strFileName).ToLower().Trim('.') == strExtension.ToLower();
        }

        static public int GetIndexAfter(string strSearchIn, string strSearchFor, int nStartIndex = 0)
        {
            int nIndex = strSearchIn.IndexOf(strSearchFor, nStartIndex);
            if (nIndex != -1)
            {
                nIndex += strSearchFor.Length;
            }
            return nIndex;
        }

        #region SongSearch

        public bool ParseStepmaniaFolder(DirectoryInfo StepManiaFolder)
        {
            bool bFoundSongs = false;
            DirectoryInfo SelectedFolder = null;

            // If this is the stepmania folder, the songs will be located in a subfolder titled "Songs"
            string strNewFolderPath = StepManiaFolder.FullName + "\\Songs";

            // If a "Songs" directory exists, try to open it
            if (Directory.Exists(strNewFolderPath) == true)
            {
                // Try-Catch to prevent application crash
                try
                {
                    SelectedFolder = new DirectoryInfo(strNewFolderPath);
                }
                catch (Exception ex)
                {
                    OutputForm.SetError(ex.Message);
                }

                // If the "Songs" directory was successfully open, try parsing it for song packs
                if (SelectedFolder != null)
                {
                    bFoundSongs = ParseStepmaniaSongsFolder(SelectedFolder);
                }

                // Save the file path of the stepmania songs folder
                if (bFoundSongs)
                {
                    this.strStepmaniaSongFolderPath = SelectedFolder.FullName;
                    CFilterLogic.SetStepmaniaSongFolderPath(SelectedFolder.FullName);
                    OutputForm.SavedOptions.SongDirectory = SelectedFolder.FullName;
                }
            }
            return bFoundSongs;
        }

        public bool ParseStepmaniaSongsFolder(DirectoryInfo StepManiaSongsFolder)
        {
            bool bFoundSongs = false;
            bool bFoundSongsInSongPack = false;
            IEnumerable<DirectoryInfo> SongPackFolders = null;
            int nFolderCount = 0;

            // Attempt to create a list of all the song pack folders
            try
            {
                SongPackFolders = StepManiaSongsFolder.EnumerateDirectories();
            }
            catch (Exception ex)
            {
                OutputForm.SetError(ex.Message);
            }

            // If the list was successfully created
            if (SongPackFolders != null)
            {
                // Loop through all song pack folders
                foreach (DirectoryInfo SongPackFolder in SongPackFolders)
                {
                    // Gracefully handle aborting the thread
                    if (!RunThread)
                    {
                        break;
                    }

                    // Parse the song pack folder for songs
                    bFoundSongsInSongPack = ParseSongPackFolder(SongPackFolder);

                    // If any songs were found then the caller needs to be made aware
                    if (bFoundSongsInSongPack)
                    {
                        OutputForm.SetStatus("(Pack " + nFolderCount + " of " + SongPackFolders.Count() + ")", 2);
                        bFoundSongs = true;
                    }
                    nFolderCount += 1;
                }

                // Save the file path of the stepmania songs folder
                if (bFoundSongs)
                {
                    this.strStepmaniaSongFolderPath = StepManiaSongsFolder.FullName;
                    CFilterLogic.SetStepmaniaSongFolderPath(StepManiaSongsFolder.FullName);
                    OutputForm.SavedOptions.SongDirectory = StepManiaSongsFolder.FullName;
                }
            }
            return bFoundSongs;
        }

        public bool ParseSongPackFolder(DirectoryInfo SongPackFolder, int nContinueFolderCountFrom = 0)
        {
            bool bFoundSongs = false;
            bool bIsValidSongFolder = false;
            IEnumerable<DirectoryInfo> SongFolders = null;
            DirectoryInfo FilteredSongPackFolder = null;
            int nFolderCount = nContinueFolderCountFrom;
            int nTotalFolderCount = nContinueFolderCountFrom;

            // Attempt to create a list of all the song folders
            try
            {
                SongFolders = SongPackFolder.EnumerateDirectories();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            // If the list was successfully created
            if (SongFolders != null)
            {
                // Update the total folder count
                nTotalFolderCount += SongFolders.Count();

                // Let the user know which folder we're currently parsing
                OutputForm.AddText(SongPackFolder.Parent.Name + "\\" + SongPackFolder.Name + "\n");

                // Loop through all songs in the song pack
                foreach (DirectoryInfo SongFolder in SongFolders)
                {
                    // Gracefully handle aborting the thread
                    if (!RunThread)
                    {
                        break;
                    }

                    // Parse the song folder for relevant information
                    bIsValidSongFolder = ParseSongFolder(SongFolder, SongPackFolder);

                    // If the folder actually contained song information then the caller needs to be made aware
                    if (bIsValidSongFolder)
                    {
                        OutputForm.SetStatus("(" + lstAllSongs.Count + " Songs Found)", 1);
                        OutputForm.SetStatus("(Song " + nFolderCount + " of " + nTotalFolderCount + ")", 3);
                        bFoundSongs = true;
                    }

                    nFolderCount += 1;
                }

                // If the folder actually contained song information, and if we're not already in a filter folder
                if ((bFoundSongs)
                && (SongPackFolder.Parent.Name == "Songs")
                && (OutputForm.SavedOptions.IncludeAlreadyFiltered))
                {
                    IEnumerable<DirectoryInfo> FilterFolders = new List<DirectoryInfo>();

                    // Attempt to create a list of all the folders at the same level as the songs directory
                    try
                    {
                        FilterFolders = SongPackFolder.Parent.Parent.EnumerateDirectories().Where(x => x.Name != SongPackFolder.Parent.Name);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                    // If this appears to be a song folder pack, we should check to see if any songs 
                    // have already been removed from this pack and placed into one of the filter folders
                    foreach (DirectoryInfo FilterFolder in FilterFolders)
                    {
                        // Gracefully handle aborting the thread
                        if (!RunThread)
                        {
                            break;
                        }

                        // If the filter folder existed, look for the same sing pack folder within it
                        try
                        {
                            FilteredSongPackFolder = FilterFolder?.EnumerateDirectories()?.FirstOrDefault(x => x.Name == SongPackFolder.Name);
                        }
                        catch (Exception ex)
                        {
                            OutputForm.SetError(ex.Message);
                            FilteredSongPackFolder = null;
                        }

                        // If such a folder exists, also parse the songs within it
                        if (FilteredSongPackFolder != null)
                        {
                            // If there were songs found in this folder, and this folder isn't recognized as a filter folder already
                            if ((ParseSongPackFolder(FilteredSongPackFolder, nTotalFolderCount) == true)
                            &&  (OutputForm.SavedOptions.Folders?.FirstOrDefault(x => (x as CSavedFolder)?.Name == FilterFolder.Name) == null))
                            {
                                OutputForm.AddNewFolder(FilterFolder.Name, EFolderTypes.Filter);
                            }
                        }
                    }
                }
            }
            else
            {
                // Let the user know which folder we failed to open
                OutputForm.AddText(SongPackFolder.Name + " [ERROR: UNABLE TO OPEN FOLDER]\n");
            }
            return bFoundSongs;
        }

        public bool ParseSongFolder(DirectoryInfo SongFolder, DirectoryInfo SongPackFolder)
        {
            bool bIsValidSongFolder = false;
            IEnumerable<FileInfo> SongFiles = null;

            // If there's a song already in the list with the same path
            if (this.lstAllSongs.FirstOrDefault(x => x.FolderPath == SongFolder.FullName) != null)
            {
                bIsValidSongFolder = true;
            }
            else
            {
                // Attempt to create a list of all the files in the song folder
                try
                {
                    SongFiles = SongFolder.EnumerateFiles();
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }

                // If the list was successfully created
                if (SongFiles != null)
                {
                    // Let the user know which folder we're currently parsing
                    OutputForm.AddText("\t" + SongFolder.Name + "\n");

                    CSong NewSong = new CSong();

                    // Sets the path to the song folder
                    NewSong.FolderPath = SongFolder.FullName;
                    NewSong.FolderName = SongFolder.Name;

                    // Save the song pack's name
                    NewSong.Pack = SongPackFolder.Name;

                    // Create the list of step files
                    NewSong.StepFilePaths = new List<string>();

                    // Find the "dwi", "sm", "ssc" files
                    foreach (FileInfo SongFile in SongFiles)
                    {
                        // In the case where both an "SM" and a "DWI" file exist, the "SM" takes higher priority
                        if (CSongListPopulator.HasExtension(SongFile.Name, "sm") 
                        ||  CSongListPopulator.HasExtension(SongFile.Name, "ssc") 
                        ||  CSongListPopulator.HasExtension(SongFile.Name, "dwi"))
                        {
                            NewSong.StepFilePaths.Add(SongFile.Name);
                        }
                        else if (CSongListPopulator.HasExtension(SongFile.Name, "mp3") 
                        || CSongListPopulator.HasExtension(SongFile.Name, "ogg") 
                        || CSongListPopulator.HasExtension(SongFile.Name, "wav") 
                        || CSongListPopulator.HasExtension(SongFile.Name, "opus"))
                        {
                            NewSong.MusicPath = SongFile.Name;
                        }
                    }

                    // If both a music file and step file have been found, this is a legitimate song
                    if (((NewSong.StepFilePaths?.Count ?? 0) > 0) && ((NewSong.MusicPath?.Length ?? 0) > 0))
                    {
                        // See if this song is already in the list
                        OutputForm.AddSong(NewSong);
                        //this.lstParsedSongs.Add(NewSong);
                        bIsValidSongFolder = true;
                    }
                    else
                    {
                        Console.Write("No song or stepfiles found in \"" + NewSong.FolderPath + "\"");
                    }
                }
                else
                {
                    // Let the user know which folder we failed to open
                    OutputForm.AddText("\t" + SongFolder.Name + " [ERROR: UNABLE TO OPEN FOLDER]\n");
                }
            }
            return bIsValidSongFolder;
        }

        #endregion
    }
}
