using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace StepManiaHelper
{
    public class CSimilarSongFilter : CFilterLogic
    {
        public static string FilterFolder => "_ALT";

        private int nSongSimilarity;
        private List<CSong> aSimilarSongs;

        public CSimilarSongFilter(int nSongSimilarity)
        {
            this.aSimilarSongs = new List<CSong>();
            this.nSongSimilarity = nSongSimilarity;
        }

        public override string GetResultString()
        {
            return "Moved " + this.aSimilarSongs.Count.ToString() + " songs into the '" + FilterFolder + "' folder because a better version exists.\n";
        }

        internal override void Filter(Options OutputForm, List<CSong> lstSongs)
        {
            int nSong = 0;
            OutputForm.SetStatus("Searching For Similar Songs", 2);

            // Ensure no flags are set from previous logic
            ClearSongFlags(lstSongs);

            // Loop through all the parsed songs
            foreach (CSong ParsedSong in lstSongs)
            {
                // Because of the nature of the double loop, the first "ParsedSong" will flag all similar songs.
                // If we come across an already flagged song, we know that all similar songs have already been found, so we can skip it.
                if (ParsedSong.bFlagged == false)
                {
                    // Let the user know which song we're currently searching for duplicates for
                    OutputForm.AddText("\t" + ParsedSong.FolderName + "\n");
                    OutputForm.SetStatus("(Song " + nSong + " of " + lstSongs.Count + ")", 3);

                    // Loop through all the parsed songs again, comparing the inner loop's song to the outer loop's song
                    foreach (CSong OtherParsedSong in lstSongs)
                    {
                        // If the "OtherParsedSong" is already flagged, we can ignore it because it's already being moved
                        if (OtherParsedSong.bFlagged == false)
                        {
                            // If the song objects don't point to the same place, 
                            // and if the "OtherParsedSong" hasn't already been scanned
                            if ((ParsedSong != OtherParsedSong)
                            && (OtherParsedSong.bAlreadyScanned == false))
                            {
                                // Compare the songs to determine if they are similar
                                if (CSong.AreSongsSimilar(OtherParsedSong, ParsedSong, nSongSimilarity))
                                {
                                    // If they have the same number of difficulties under the specified threshold, we'll have to use a different metric
                                    if (ParsedSong.aDifficulties.Count == OtherParsedSong.aDifficulties.Count)
                                    {
                                        // If they have the same number of total difficulties, we'll have to use a different metric
                                        if (ParsedSong.aDifficulties.Count == OtherParsedSong.aDifficulties.Count)
                                        {
                                            // If they have the same number of measures, we'll have to use a different metric
                                            if (ParsedSong.aDifficulties[0].NumberOfMeasures == OtherParsedSong.aDifficulties[0].NumberOfMeasures)
                                            {
                                                // Keep the longer version of the song, or the first version if they are identical
                                                if (ParsedSong.SongLength >= OtherParsedSong.SongLength)
                                                {
                                                    this.aSimilarSongs.Add(OtherParsedSong);
                                                    OtherParsedSong.bFlagged = true;
                                                }
                                                else
                                                {
                                                    this.aSimilarSongs.Add(ParsedSong);
                                                    ParsedSong.bFlagged = true;
                                                }
                                            }
                                            // If the "OtherParsedSong" has more measure, replace the "ParsedSong"
                                            else if (OtherParsedSong.aDifficulties[0].NumberOfMeasures > ParsedSong.aDifficulties[0].NumberOfMeasures)
                                            {
                                                this.aSimilarSongs.Add(ParsedSong);
                                                ParsedSong.bFlagged = true;
                                            }
                                            // If the "OtherParsedSong" has less measures, replace it
                                            else
                                            {
                                                this.aSimilarSongs.Add(OtherParsedSong);
                                                OtherParsedSong.bFlagged = true;
                                            }
                                        }
                                        // If the "OtherParsedSong" has more difficulties, replace the "ParsedSong"
                                        else if (OtherParsedSong.aDifficulties.Count > ParsedSong.aDifficulties.Count)
                                        {
                                            this.aSimilarSongs.Add(ParsedSong);
                                            ParsedSong.bFlagged = true;
                                        }
                                        // If the "OtherParsedSong" has less difficulties, replace it
                                        else
                                        {
                                            this.aSimilarSongs.Add(OtherParsedSong);
                                            OtherParsedSong.bFlagged = true;
                                        }
                                    }
                                    // If the "OtherParsedSong" has more difficulties, replace the "ParsedSong"
                                    else if (ParsedSong.aDifficulties.Count < OtherParsedSong.aDifficulties.Count)
                                    {
                                        this.aSimilarSongs.Add(ParsedSong);
                                        ParsedSong.bFlagged = true;
                                    }
                                    // If the "OtherParsedSong" has less difficulties, replace it
                                    else
                                    {
                                        this.aSimilarSongs.Add(OtherParsedSong);
                                        OtherParsedSong.bFlagged = true;
                                    }
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

            OutputForm.SetStatus("Moving Similar Songs", 2);
            OutputForm.SetStatus("", 3);

            // For songs that are similar, move all worse versions into "ALT" folder
            foreach (CSong SimilarSong in this.aSimilarSongs)
            {
                // Let the user know which song we're currently deleting
                OutputForm.AddText("\t" + SimilarSong.FolderName + SimilarSong.GetSongDataString(ESongData.EBasic) + "\n" +
                                   "\t\tSimilar To: " + SimilarSong.aSimilarSongs[0].GetSongDataString(ESongData.EBasic) + "\n");

                SimilarSong.MoveFilterSong(FilterFolder);

                if (OutputForm.SavedOptions.IncludeAlreadyFiltered == false)
                {
                    lstSongs.Remove(SimilarSong);
                }
            }
        }
    }
}
