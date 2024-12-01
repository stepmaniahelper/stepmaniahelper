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
    class CSscParser : CSmParser
    {
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
                nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, "dance-single", nStartIndex);
                if (nStartIndex == -1)
                {
                    break;
                }
                // Difficulty in number form start
                nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, "#METER:", nStartIndex);
                if (nStartIndex == -1)
                {
                    break;
                }
                // Difficulty in number form end
                nEndIndex = strFileContents.IndexOf(";", nStartIndex);
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

                // Note section start
                nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, "#NOTES:", nEndIndex + 1);
                if (nStartIndex == -1)
                {
                    break;
                }
                // Note section end
                nEndIndex = strFileContents.IndexOf(";", nStartIndex);
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
    }
}
