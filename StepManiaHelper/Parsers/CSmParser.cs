using ExtensionMethods;
using StepManiaHelper.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace StepManiaHelper
{
    class CSmParser : CStepFileParser
    {
        public override void ParseTitleAndArtist(string strFileContents, CSong Song, string StepFile)
        {
            int nStartIndex = 0;
            int nEndIndex = 0;

            base.ParseTitleAndArtist(strFileContents, Song, StepFile);

            // Search for translated titles
            nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, "#TITLETRANSLIT:", 0);
            if (nStartIndex != -1)
            {
                nEndIndex = strFileContents.IndexOf(";", nStartIndex);
                if (nEndIndex != -1)
                {
                    Song.astrNames.AddIfUnique(strFileContents.Substring(nStartIndex, nEndIndex - nStartIndex));
                    Song.astrSimplifiedNames.AddIfUnique(CSong.RemovePunctuation(CSong.RemoveParenthensis(Song.astrNames.LastOrDefault().ToLower())));
                }
            }

            // Search for translated artists
            nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, "#ARTISTTRANSLIT:", 0);
            if (nStartIndex != -1)
            {
                nEndIndex = strFileContents.IndexOf(";", nStartIndex);
                if (nEndIndex != -1)
                {
                    Song.astrArtists.AddIfUnique(strFileContents.Substring(nStartIndex, nEndIndex - nStartIndex));
                    Song.astrSimplifiedArtists.AddIfUnique(CSong.RemovePunctuation(CSong.RemoveParenthensis(Song.astrArtists.LastOrDefault().ToLower())));
                }
            }
        }

        public override void ParseBpms(string strFileContents, CSong Song, string StepFile)
        {
            int nStartIndex = 0;
            int nEndIndex = 0;
            int nBpmEndIndex = 0;
            int nBeatIdentifier = 0;
            double fBpmValue = 0.0;
            string strSubstring = "";
            CBpmSegment Bpm = null;

            // Find the start index of the BPM section
            nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, "#DISPLAYBPM:");

            // If the BPM section exists, find the end of the BPM section
            if (nStartIndex != -1)
            {
                // Find the end of the BPM section
                nEndIndex = strFileContents.IndexOf(";", nStartIndex);

                // If the BPM section ends, parse the value within
                if (nEndIndex != -1)
                {
                    // Save the BPM value
                    strSubstring = strFileContents.Substring(nStartIndex, nEndIndex - nStartIndex);

                    // Attempt to parse the BPM value from the above substring.
                    // This can fail if the file is incorrectly formatted
                    fBpmValue = double.PositiveInfinity;
                    try
                    {
                        fBpmValue = Convert.ToDouble(strSubstring);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message + "\nIn " + StepFile);
                    }

                    // If the beat identifier parse above failed, then don't save the BPM value
                    if (fBpmValue != double.PositiveInfinity)
                    {
                        // Create a new BPM instance
                        Song.DisplayBpm = Song.DisplayBpm ?? fBpmValue;
                    }
                }
            }

            // If we already have a BPM list from another stepfile, we shouldn't add to it
            if (Song.aBpms.Count == 0)
            { 
                // Find the start index of the BPM section
                nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, "#BPMS:");

                // If the BPM section exists, find the end of the BPM section
                if (nStartIndex != -1)
                {
                    // Find the end of the entire BPM section
                    nBpmEndIndex = strFileContents.IndexOf(";", nStartIndex);

                    // If the BPM section ends, parse each pair of BPM values
                    if (nBpmEndIndex != -1)
                    {
                        while (true)
                        {
                            // Find the end of the beat identifier
                            nEndIndex = strFileContents.IndexOf("=", nStartIndex);
                            if (nEndIndex == -1)
                            {
                                break;
                            }

                            // Save the beat identifier
                            strSubstring = strFileContents.Substring(nStartIndex, (nEndIndex - nStartIndex)).Trim();

                            // Attempt to parse the beat identifier from the above substring.
                            // This can fail if the file is incorrectly formatted
                            nBeatIdentifier = -1;
                            try
                            {
                                nBeatIdentifier = Convert.ToInt32(Convert.ToDouble(strSubstring));
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message + "\nIn " + StepFile);
                            }

                            // If the beat identifier parse above failed, then the file is faulty and there's no point continuing
                            if (nBeatIdentifier == -1)
                            {
                                break;
                            }

                            // Set to the start of the BPM value
                            nStartIndex = nEndIndex + 1;

                            // Find the end of the BPM value
                            nEndIndex = strFileContents.IndexOf(",", nStartIndex);
                            if (nEndIndex == -1)
                            {
                                // If there are no more BPM values after this one, the end delimiter will be a semicolon, not a comma
                                nEndIndex = nBpmEndIndex;
                            }

                            // If the identified end of the BPM value is past the end of the BPM section, adjust the end index accordingly
                            if (nEndIndex > nBpmEndIndex)
                            {
                                nEndIndex = nBpmEndIndex;
                            }

                            // Save the BPM value
                            strSubstring = strFileContents.Substring(nStartIndex, (nEndIndex - nStartIndex)).Trim();

                            // Attempt to parse the beat identifier from the above substring.
                            // This can fail if the file is incorrectly formatted
                            fBpmValue = double.PositiveInfinity;
                            try
                            {
                                fBpmValue = Convert.ToDouble(strSubstring);
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message + "\nIn " + StepFile);
                            }

                            // If the beat identifier parse above failed, then the file is faulty and there's no point continuing
                            if (fBpmValue == double.PositiveInfinity)
                            {
                                break;
                            }

                            // Set to the start of the next beat identifier
                            nStartIndex = nEndIndex + 1;

                            // Create a new BPM instance
                            Bpm = new CBpmSegment(nBeatIdentifier, fBpmValue);

                            // Add the BPM to the BPM list for the song
                            Song.aBpms.Add(Bpm);
                        }
                    }
                }
            }
        }

        public override void ParseDifficulties(string strFileContents, CSong Song, string StepFile)
        {
            int nStartIndex = 0;
            int nEndIndex = 0;
            int nDifficulty = 0;
            string strSubstring = "";
            CDifficulty NewDifficulty = null;

            while (true)
            {
                // Loop through the colon separated information pertaining to each difficulty

                // We only care about single difficulties 
                nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, "dance-single:", nStartIndex);
                if (nStartIndex == -1)
                {
                    break;
                }
                // Description / Author end
                nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, ":", nStartIndex);
                if (nStartIndex == -1)
                {
                    break;
                }
                // Difficulty in text form end
                nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, ":", nStartIndex);
                if (nStartIndex == -1)
                {
                    break;
                }
                // Difficulty in number form end
                nEndIndex = strFileContents.IndexOf(":", nStartIndex);
                if (nEndIndex == -1)
                {
                    break;
                }

                // Save the reported difficulty value
                strSubstring = strFileContents.Substring(nStartIndex, nEndIndex - nStartIndex).Trim();

                // Attempt to parse the difficulty from the above substring.
                // This can fail if the file is incorrectly formatted
                nDifficulty = 0;
                try
                {
                    nDifficulty = Convert.ToInt32(strSubstring);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message + "\nIn " + StepFile);
                }

                // Groove radar data end
                nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, ":", nEndIndex + 1);
                if (nStartIndex == -1)
                {
                    break;
                }
                // Note section end
                nEndIndex = strFileContents.IndexOf("#", nStartIndex);
                if (nEndIndex == -1)
                {
                    // If there's no # symbol, then the end of the note section is the end of the file
                    nEndIndex = strFileContents.Length;
                }

                // Save the note section
                strSubstring = strFileContents.Substring(nStartIndex, nEndIndex - nStartIndex).Trim();
                strSubstring = Regex.Replace(strSubstring, @"[^\w,]+", "", RegexOptions.Compiled);

                // If the difficulty doesn't have a number associated with it, it won't be displayed, and we can ignore it.
                // However, we should continue searching for other difficulties
                if (nDifficulty != 0)
                {
                    // Create a new difficulty
                    NewDifficulty = new CDifficulty(StepFile);
                    NewDifficulty.Difficulty = nDifficulty;

                    // Increment the count of expected difficulties
                    Song.nExpectedDifficultyCount = (Song.nExpectedDifficultyCount ?? 0) + 1;

                    // Parse the note section
                    ParseNotes(strSubstring, NewDifficulty, StepFile);

                    // Save the difficulty information into the song's data
                    Song.aDifficulties.Add(NewDifficulty);
                    NewDifficulty.ParentSong = Song;
                }
            }
        }

        public override void ParseStops(string strFileContents, CSong Song, string StepFile)
        {
            int nStartIndex = 0;
            int nEndIndex = 0;
            int nEndStopsIndex = 0;
            string strSubstring = "";
            int nBeatIdentifier = -1;
            double fDuration = 0;
            CStop NewStop = null;

            // If we already have a list of stops from another stepfile, don't add any more to it
            if (Song.aStops.Count > 0)
            { 
                // Find the start index of the BPM section
                nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, "#STOPS:");

                // If the BPM section exists, find the end of the BPM section
                if (nStartIndex != -1)
                {
                    // Find the end of the entire BPM section
                    nEndStopsIndex = strFileContents.IndexOf(";", nStartIndex);

                    // If the BPM section ends, parse each pair of BPM values
                    if (nEndStopsIndex != -1)
                    {
                        while (true)
                        {
                            // Find the end of the beat identifier
                            nEndIndex = strFileContents.IndexOf("=", nStartIndex);
                            if (nEndIndex == -1)
                            {
                                break;
                            }

                            // Save the beat identifier
                            strSubstring = strFileContents.Substring(nStartIndex, (nEndIndex - nStartIndex)).Trim();

                            // Attempt to parse the beat identifier from the above substring.
                            // This can fail if the file is incorrectly formatted
                            nBeatIdentifier = -1;
                            try
                            {
                                nBeatIdentifier = Convert.ToInt32(Convert.ToDouble(strSubstring));
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message + "\nIn " + StepFile);
                            }

                            // If the beat identifier parse above failed, then the file is faulty and there's no point continuing
                            if (nBeatIdentifier == -1)
                            {
                                break;
                            }

                            // Set the start of the BPM value
                            nStartIndex = nEndIndex + 1;

                            // Find the end of the BPM value
                            nEndIndex = strFileContents.IndexOf(",", nStartIndex);
                            if (nEndIndex == -1)
                            {
                                // If there are no more BPM values after this one, the end delimiter will be a semicolon, not a comma
                                nEndIndex = nEndStopsIndex;
                            }

                            // If the identified end of the BPM value is past the end of the BPM section, adjust the end index accordingly
                            if (nEndIndex > nEndStopsIndex)
                            {
                                nEndIndex = nEndStopsIndex;
                            }

                            // Save the BPM value
                            strSubstring = strFileContents.Substring(nStartIndex, (nEndIndex - nStartIndex)).Trim();

                            // Attempt to parse the beat identifier from the above substring.
                            // This can fail if the file is incorrectly formatted
                            fDuration = double.PositiveInfinity;
                            try
                            {
                                fDuration = Convert.ToDouble(strSubstring);
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message + "\nIn " + StepFile);
                            }

                            // If the beat identifier parse above failed, then the file is faulty and there's no point continuing
                            if (fDuration == double.PositiveInfinity)
                            {
                                break;
                            }

                            // Create a new BPM instance
                            NewStop = new CStop(nBeatIdentifier, fDuration);

                            // Add the BPM to the BPM list for the song
                            Song.aStops.Add(NewStop);
                        }
                    }
                }
            }
        }

        public void ParseNotes(string strNotesContents, CDifficulty Difficulty, string StepFile)
        {
            int nMeasureStartIndex = 0;
            int nMeasureEndIndex = 0;
            int nGroupsInMeasure = 0;
            string strMeasure = "";
            string strGroup = "";
            int nNumberofNotesBeforeGroup = 0;
            int nNumberofNotesInGroup = 0;
            int nNumberofNonHoldNotesInGroup = 0;
            char nNote = '0';
            int nNumberOfCurrentHolds = 0;
            bool[] abHolds = new bool[4];

            while (true)
            {
                // Find the end of the current measure
                nMeasureEndIndex = strNotesContents.IndexOf(",", nMeasureStartIndex);
                if (nMeasureEndIndex == -1)
                {
                    break;
                }

                // Since we found a complete measure, increase the measure count
                Difficulty.NumberOfMeasures++;

                // Save the measure's contents
                strMeasure = strNotesContents.Substring(nMeasureStartIndex, nMeasureEndIndex - nMeasureStartIndex);

                // Reset the start index for the next try
                nMeasureStartIndex = nMeasureEndIndex + 1;

                // If the measure's contents aren't a length divisible by 4, there's a problem
                if (strMeasure.Length % 4 != 0)
                {
                    break;
                }

                // Loop through each group in the measure
                nGroupsInMeasure = strMeasure.Length / 4;
                for (int nCurrentGroup = 0; nCurrentGroup < nGroupsInMeasure; nCurrentGroup++)
                {
                    // Save the group's contents
                    strGroup = strMeasure.Substring((nCurrentGroup * 4), 4).ToUpper();

                    // Save the current note count; will be used to determine how many notes are in the group
                    nNumberofNotesBeforeGroup = Difficulty.Notes;

                    // Reset the number of non hold notes in the group
                    nNumberofNonHoldNotesInGroup = 0;

                    // Loop through each note in the group
                    for (int nCurrentNote = 0; nCurrentNote < 4; nCurrentNote++)
                    {
                        // Save the note
                        nNote = strGroup[nCurrentNote];

                        // If the note is held, and a new note is in the same collumn,
                        // end the hold
                        if ((nNote != '0')
                        &&  (abHolds[nCurrentNote] == true))
                        {
                            abHolds[nCurrentNote] = false;
                            nNumberOfCurrentHolds--;
                        }

                        // If the note is a step, increase the note count 
                        // (don't increase the step count yet because it might be part of a jump)
                        if (nNote == '1')
                        {
                            Difficulty.Notes++;
                        }

                        // If the note is a hold head, increase the hold count
                        if (nNote == '2')
                        {
                            Difficulty.Notes++;
                            Difficulty.Holds++;
                            nNumberOfCurrentHolds++;
                            nNumberofNonHoldNotesInGroup--;

                            // Set the direction as held
                            abHolds[nCurrentNote] = true;
                        }

                        // If the note is a hold or roll tail, do nothing
                        if (nNote == '3')
                        {
                            
                        }

                        // If the note is a roll head, increase the roll count
                        if (nNote == '4')
                        {
                            Difficulty.Notes++;
                            Difficulty.Rolls++;
                            nNumberOfCurrentHolds++;
                            nNumberofNonHoldNotesInGroup--;

                            // Set the direction as held
                            abHolds[nCurrentNote] = true;
                        }

                        // If the note is a mine, increase the mine count 
                        // (mines don't count as notes)
                        if (nNote == 'M')
                        {
                            Difficulty.Mines++;
                        }
                    }

                    // Calculate the number of notes that were present in the group
                    nNumberofNotesInGroup = Difficulty.Notes - nNumberofNotesBeforeGroup;
                    nNumberofNonHoldNotesInGroup += nNumberofNotesInGroup;

                    // If the number of notes is one, it counts as a step
                    if (nNumberofNotesInGroup == 1)
                    {
                        Difficulty.Steps++;
                    }

                    // If the number of notes is more than one, it counts as a jump
                    if (nNumberofNotesInGroup > 1)
                    {
                        Difficulty.Jumps++;
                    }

                    // If the number of simultaneous notes is more than 2, it can't be played on a pad
                    if ((nNumberofNotesInGroup > 2)
                    || ((nNumberOfCurrentHolds + nNumberofNonHoldNotesInGroup) > 2))
                    {
                        Difficulty.NonPadJumps++;
                    }
                }
            }
        }

        public override void ParseGenres(string strFileContents, CSong Song, string StepFile)
        {
            int nStartIndex = 0;
            int nEndIndex = 0;
            string tag = "#GENRE:";

            // Search for the genre tag
            nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, tag, 0);
            if (nStartIndex != -1)
            {
                nEndIndex = strFileContents.IndexOf(";", nStartIndex);
                if (nEndIndex != -1)
                {
                    Song.astrGenres.AddIfUnique(strFileContents.Substring(nStartIndex, nEndIndex - nStartIndex));
                }
            }
        }

        public override void ParseImages(string strFileContents, CSong Song, string StepFile, CSavedOptions SavedOptions)
        {
            int nStartIndex = 0;
            int nEndIndex = 0;
            string tag;

            // Search for the banner tag
            tag = "#BANNER:";
            nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, tag, 0);
            if (nStartIndex != -1)
            {
                nEndIndex = strFileContents.IndexOf(";", nStartIndex);
                if (nEndIndex != -1)
                {
                    Song.BannerPath = Song.BannerPath ?? strFileContents.Substring(nStartIndex, nEndIndex - nStartIndex);
                    Song.ImageFromFile(nameof(Song.Banner), SavedOptions);
                }
            }

            // Search for the background tag
            tag = "#BACKGROUND:";
            nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, tag, 0);
            if (nStartIndex != -1)
            {
                nEndIndex = strFileContents.IndexOf(";", nStartIndex);
                if (nEndIndex != -1)
                {
                    Song.BackgroundPath = Song.BackgroundPath ?? strFileContents.Substring(nStartIndex, nEndIndex - nStartIndex);
                    Song.ImageFromFile(nameof(Song.Background), SavedOptions);
                }
            }

            // Search for the CD title tag
            tag = "#CDTITLE:";
            nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, tag, 0);
            if (nStartIndex != -1)
            {
                nEndIndex = strFileContents.IndexOf(";", nStartIndex);
                if (nEndIndex != -1)
                {
                    Song.CdTitlePath = Song.CdTitlePath ?? strFileContents.Substring(nStartIndex, nEndIndex - nStartIndex);
                    Song.ImageFromFile(nameof(Song.CdTitle), SavedOptions);
                }
            }

            // Search for the jacket tag
            tag = "#JACKET:";
            nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, tag, 0);
            if (nStartIndex != -1)
            {
                nEndIndex = strFileContents.IndexOf(";", nStartIndex);
                if (nEndIndex != -1)
                {
                    Song.JacketPath = Song.JacketPath ?? strFileContents.Substring(nStartIndex, nEndIndex - nStartIndex);
                    Song.ImageFromFile(nameof(Song.Jacket), SavedOptions);
                }
            }

            // Search for the jacket tag
            tag = "#DISKIMAGE:";
            nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, tag, 0);
            if (nStartIndex != -1)
            {
                nEndIndex = strFileContents.IndexOf(";", nStartIndex);
                if (nEndIndex != -1)
                {
                    Song.DiskImagePath = Song.DiskImagePath ?? strFileContents.Substring(nStartIndex, nEndIndex - nStartIndex);
                    Song.ImageFromFile(nameof(Song.DiskImage), SavedOptions);
                }
            }
        }
    }
}
