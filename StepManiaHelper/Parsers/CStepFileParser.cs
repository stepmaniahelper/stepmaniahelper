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
    class CStepFileParser
    {
        public void Parse(string strFileContents, CSong Song, string StepFile, CSavedOptions SavedOptions)
        {
            // If there are multiple step files, we need to composite them together, so we 
            // shouldn't overwrite data if it was created by another step file (the original
            // reset for repeated parses happens at the layer above this)
            Song.astrNames = Song.astrNames ?? new List<string>();
            Song.astrSimplifiedNames = Song.astrSimplifiedNames ?? new List<string>();
            Song.aDifficulties = Song.aDifficulties ?? new List<CDifficulty>();
            Song.aBpms = Song.aBpms ?? new List<CBpmSegment>();
            Song.aStops = Song.aStops ?? new List<CStop>();
            Song.astrArtists = Song.astrArtists ?? new List<string>();
            Song.astrSimplifiedArtists = Song.astrSimplifiedArtists ?? new List<string>();
            Song.astrGenres = Song.astrGenres ?? new List<string>();

            // Ignored commented-out lines from the file
            strFileContents = Regex.Replace(strFileContents, @"//(.*?)[\r\n]", "", RegexOptions.Compiled);

            ParseTitleAndArtist(strFileContents, Song, StepFile);
            ParseBpms(strFileContents, Song, StepFile);
            ParseDifficulties(strFileContents, Song, StepFile);
            ParseStops(strFileContents, Song, StepFile);
            ParseGenres(strFileContents, Song, StepFile);
            ParseImages(strFileContents, Song, StepFile, SavedOptions);

            // Copy the song folder into the list of titles, since it can help catch duplicates
            Song.astrNames?.AddIfUnique(Song.FolderName);
            Song.astrSimplifiedNames.AddIfUnique(CSong.RemovePunctuation(CSong.RemoveParenthensis(Song.FolderName.ToLower())));

            // Sort the difficulties
            Song.aDifficulties?.Sort((x, y) => x.Difficulty - y.Difficulty);

            // If the step file could not be correctly parsed, keep the beat count at default (0)
            if (((Song.aDifficulties?.Count ?? 0) > 0)
            &&  (Song.aDifficulties[0].NumberOfMeasures > 0))
            {
                // Calculate the number of beats in the song (there are 4 beats per measure)
                Song.nNumberOfBeats = Song.nNumberOfBeats ?? Song.aDifficulties[0].NumberOfMeasures * 4;

                // Loop through each difficulty
                foreach (CDifficulty Difficulty in Song.aDifficulties)
                {
                    // If the difficulty wasn't correctly parsed, then there will be no measure count
                    if (Difficulty.NumberOfMeasures > 0)
                    {
                        // Calculate the average notes per measure for each difficulty
                        Difficulty.NotesPerMeasure = Difficulty.Notes / Difficulty.NumberOfMeasures;
                    }
                }
            }
            else
            {
                Console.Write("Unable to parse any difficulties for: " + Song.strFolderPath);
            }

            // We can only calculate the average BPM if the step file was parsed correctly
            if (((Song.nNumberOfBeats ?? 0) > 0)
            &&  ((Song.aBpms?.Count ?? 0) > 0))
            {
                Song.AverageBpm = Song.AverageBpm ?? CalculateAverageBpm(Song);
                Song.fModeBpm = Song.fModeBpm ?? CalculateModeBpm(Song);
            }
            // If the individual steps weren't parsed, but the BPM segments were, 
            // assume the first BPM segment represents the entire song
            else if ((Song.aBpms?.Count ?? 0) > 0)
            {
                Song.AverageBpm = Song.AverageBpm ?? Song.aBpms[0].fBpm;
            }

            // Calculate the relative difficulty by multiplying the notes-per-measure by average BPM
            if ((Song.AverageBpm > 0)
            &&  ((Song.aDifficulties?.Count ?? 0) > 0)
            &&  (Song.aDifficulties[0].NumberOfMeasures > 0))
            {
                // Loop through each difficulty
                foreach (CDifficulty Difficulty in Song.aDifficulties)
                {
                    Difficulty.RelativeDifficulty = Difficulty.NotesPerMeasure * (Song.AverageBpm ?? 0);
                }
            }
        }

        public virtual double CalculateAverageBpm(CSong Song)
        {
            CBpmSegment LastBpmSegment = null;
            CBpmSegment CurrentBpmSegment = null;
            double fAverageBpm = 0.0;

            // Calculate the duration in beats for each BPM segment
            foreach (CBpmSegment BpmSegment in Song.aBpms)
            {
                // If a last BPM segment is defined (will be for every iteration but the first)
                if (LastBpmSegment != null)
                {
                    // Calculate the duration of the last BPM segment
                    LastBpmSegment.nDurationInBeats = BpmSegment.nBeat - LastBpmSegment.nBeat;
                    LastBpmSegment.fDurationInMinutes = LastBpmSegment.nDurationInBeats * (1 / LastBpmSegment.fBpm);
                }

                // Save this as the last BPM segment for the next loop iteration
                LastBpmSegment = BpmSegment;
            }

            // Since the last BPM segment doesn't have a next segment to do math with, we have 
            // to use the end of the song. It's worth noting that there are 4 beats per measure.
            CurrentBpmSegment = Song.aBpms[Song.aBpms.Count - 1];
            CurrentBpmSegment.nDurationInBeats = (Song.nNumberOfBeats ?? 0) - LastBpmSegment.nBeat;
            CurrentBpmSegment.fDurationInMinutes = CurrentBpmSegment.nDurationInBeats * (1 / CurrentBpmSegment.fBpm);

            // Sum the BPM values using the duration of each segment
            foreach (CBpmSegment BpmSegment in Song.aBpms)
            {
                fAverageBpm += (BpmSegment.nDurationInBeats * BpmSegment.fBpm);
            }

            // Divide by the total number of beats to get the average BPM
            fAverageBpm /= (Song.nNumberOfBeats ?? 0);

            return fAverageBpm;
        }

        public virtual double CalculateModeBpm(CSong Song)
        {
            double fModeBpm = 0;
            double fModeBpmDuration = 0;
            Dictionary<double, double> dicTotalBpmDurations = new Dictionary<double,double>();

            // Loop through all BPM segments
            foreach (CBpmSegment BpmSegment in Song.aBpms)
            {
                // If the current BPM hasn't been seen before, add it to the list of keys
                if (dicTotalBpmDurations.Keys.Contains(BpmSegment.fBpm) == false)
                {
                    dicTotalBpmDurations.Add(BpmSegment.fBpm, BpmSegment.fDurationInMinutes);
                }
                else
                {
                    dicTotalBpmDurations[BpmSegment.fBpm] += BpmSegment.fDurationInMinutes;
                }

                // Determine if this is the longest duration or not
                if (fModeBpmDuration < dicTotalBpmDurations[BpmSegment.fBpm])
                {
                    fModeBpmDuration = dicTotalBpmDurations[BpmSegment.fBpm];
                    fModeBpm = BpmSegment.fBpm;
                }
            }

            return fModeBpm;
        }

        public virtual void ParseTitleAndArtist(string strFileContents, CSong Song, string StepFile)
        {
            int nStartIndex = 0;
            int nEndIndex = 0;

            // Whether it's a DWI or SM file, the format for title is the same
            nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, "#TITLE:", 0);
            if (nStartIndex != -1)
            {
                nEndIndex = strFileContents.IndexOf(";", nStartIndex);
                if (nEndIndex != -1)
                {
                    Song.Title = Song.Title ?? strFileContents.Substring(nStartIndex, nEndIndex - nStartIndex);
                    Song.astrNames.AddIfUnique(Song.Title);
                    Song.astrSimplifiedNames.AddIfUnique(CSong.RemovePunctuation(CSong.RemoveParenthensis(Song.astrNames.LastOrDefault().ToLower())));
                }
            }

            // Whether it's a DWI or SM file, the format for author is the same
            nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, "#ARTIST:", 0);
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

        public virtual void ParseBpms(string strFileContents, CSong Song, string StepFile)
        {

        }

        public virtual void ParseDifficulties(string strFileContents, CSong Song, string StepFile)
        {

        }

        public virtual void ParseStops(string strFileContents, CSong Song, string StepFile)
        {

        }

        public virtual void ParseGenres(string strFileContents, CSong Song, string StepFile)
        {

        }

        public virtual void ParseImages(string strFileContents, CSong Song, string StepFile, CSavedOptions SavedOptions)
        {

        }
    }
}
