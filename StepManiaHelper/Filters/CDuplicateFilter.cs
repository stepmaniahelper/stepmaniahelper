using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace StepManiaHelper
{
    public class CDuplicateFilter : CFilterLogic
    {
        public static string FilterFolder => "_DUPLICATE";

        private Dictionary<CSong, byte[]> dicMusicHashes;
        private Dictionary<string, byte[]> dicStepFileHashes;
        private List<CSong> aDuplicateSongs;

        public CDuplicateFilter()
        {
            this.aDuplicateSongs = new List<CSong>();
        }

        public override string GetResultString()
        {
            return "Moved " + this.aDuplicateSongs.Count.ToString() + " songs into the '" + FilterFolder + "' folder because they were exact duplicates.\n";
        }

        internal override void Filter(Options OutputForm, List<CSong> lstSongs)
        {
            int nSong = 0;
            OutputForm.SetStatus("Searching For Duplicate Songs", 2);

            // Create a dictionary of hashes for the step files
            Dictionary<CSong, List<byte[]>> dicStepHashes = new Dictionary<CSong, List<byte[]>>();

            // Ensure no flags are set from previous logic
            ClearSongFlags(lstSongs);

            // Loop through all the parsed songs
            foreach (CSong ParsedSong in lstSongs)
            {
                // Create the internal dictionary for the song
                List<byte[]> lstHashes = new List<byte[]>();

                // Loop through all step files assocaited with the song
                foreach (string path in ParsedSong.StepFilePaths)
                {
                    lstHashes.Add(CSong.GetHash(ParsedSong.FolderPath + "\\" + path));
                }
                // Add the hash list to the dictionary
                dicStepHashes.Add(ParsedSong, lstHashes);
            }

            // Loop through all the parsed songs
            foreach (CSong ParsedSong in lstSongs)
            {
                OutputForm.SetStatus("(Song " + nSong + " of " + lstSongs.Count + ")", 3);

                // Because of the nature of the double loop, the first "ParsedSong" will flag all duplicates as needing deletion.
                // If we come across an already flagged song, we know that all duplicates have already been found, so we can skip it.
                if (ParsedSong.bFlagged == false)
                {
                    // Let the user know which song we're currently searching for duplicates for
                    OutputForm.AddText("\t" + ParsedSong.FolderName + "\n");

                    // Loop through all the parsed songs again, comparing the inner loop's song to the outer loop's song
                    foreach (CSong OtherParsedSong in lstSongs)
                    {
                        // If the song objects don't point to the same place, 
                        // and if the "OtherParsedSong" hasn't already been scanned
                        if ((ParsedSong != OtherParsedSong)
                        && (OtherParsedSong.bAlreadyScanned == false))
                        {
                            bool AllStepFilesMatch = true;

                            // Check if all of the step files from one song can be found in the other
                            foreach (var hash in dicStepHashes[OtherParsedSong])
                            {
                                if (dicStepHashes[ParsedSong].Contains(hash) == false)
                                {
                                    AllStepFilesMatch = false;
                                    break;
                                }
                            }
                            if (AllStepFilesMatch == false)
                            {
                                AllStepFilesMatch = true;
                                foreach (var hash in dicStepHashes[ParsedSong])
                                {
                                    if (dicStepHashes[OtherParsedSong].Contains(hash) == false)
                                    {
                                        AllStepFilesMatch = false;
                                        break;
                                    }
                                }
                            }

                            // Exact Match based on Hash, and
                            // Exact Match based on Name, BPM, Difficulties, and Song Length
                            if (AllStepFilesMatch
                            || CSong.DoSongsMatch(OtherParsedSong, ParsedSong))
                            {
                                // When a duplicate is found, add it to the delete list, and flag it
                                if (OtherParsedSong.bFlagged == false)
                                {
                                    this.aDuplicateSongs.Add(OtherParsedSong);
                                    OtherParsedSong.bFlagged = true;
                                    nSong += 1;
                                }
                                // If we come across and already flagged song, the above logic was flawed 
                                else
                                {
                                    OutputForm.SetError("DuplicateSongLogic is flawed :(");
                                    return;
                                }
                            }

                        }
                    }
                }

                // Flag this song as having been scanned already. All songs have already 
                // been compared to this one, so we can safely ignore it in future comparisons.
                ParsedSong.bAlreadyScanned = true;

                nSong += 1;
            }

            OutputForm.SetStatus("Moving Duplicate Songs", 2);
            OutputForm.SetStatus("", 3);

            // Delete all duplicate song files
            foreach (CSong DuplicateSong in this.aDuplicateSongs)
            {
                // Let the user know which song we're currently deleting
                OutputForm.AddText("\t" + DuplicateSong.FolderName + DuplicateSong.GetSongDataString(ESongData.EBasic) + "\n");

                DuplicateSong.MoveFilterSong(FilterFolder);
                lstSongs.Remove(DuplicateSong);
            }
        }
    }
}
