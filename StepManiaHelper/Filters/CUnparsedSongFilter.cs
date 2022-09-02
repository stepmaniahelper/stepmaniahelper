using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace StepManiaHelper
{
    class CUnparsedSongFilter : CFilterLogic
    {
        public static string FilterFolder => "_UNPARSED";

        private List<CSong> aUnparsedSongs;

        public CUnparsedSongFilter()
        {
            aUnparsedSongs = new List<CSong>();
        }

        public override string GetResultString()
        {
            return "Moved " + this.aUnparsedSongs.Count.ToString() + " songs into the '" + FilterFolder + "' folder because the parser couldn't read them.\n";
        }

        internal override void Filter(Options OutputForm, List<CSong> lstSongs)
        {
            int nSong = 0;
            OutputForm.SetStatus("Searching For Songs That Couldn't Be Parsed", 2);

            // Loop through all the parsed songs
            foreach (CSong ParsedSong in lstSongs)
            {
                // Let the user know which song we're currently examining the playability of
                OutputForm.AddText("\t" + ParsedSong.FolderName + "\n");

                // If the song couldn't be parsed correctly
                if (((ParsedSong.aDifficulties?.Count ?? 0) <= 0)
                ||  ((ParsedSong.aDifficulties?.Count ?? 0) < ParsedSong.nExpectedDifficultyCount))
                {
                    this.aUnparsedSongs.Add(ParsedSong);
                }
                nSong += 1;
            }

            OutputForm.SetStatus("Moving Songs That Couldn't Be Parsed", 2);
            OutputForm.SetStatus("", 3);

            // For songs that are unplayable on a dance pad, move them into the "NONPAD" folder
            foreach (CSong UnparsedSong in this.aUnparsedSongs)
            {
                // Let the user know which song we're currently moving
                OutputForm.AddText("\t" + UnparsedSong.FolderName + "\n");

                UnparsedSong.MoveFilterSong(FilterFolder);
                lstSongs.Remove(UnparsedSong);
            }
        }
    }
}
