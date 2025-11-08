using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;
using Shell32;
using System.Drawing;
using System.Reflection;
using System.Collections;
using StepManiaHelper.Helpers;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace StepManiaHelper
{
    public enum ESongData
    {
        EBasic,
        EAdvanced,
        EDifficulties,
        EBpm,
        EDifficultyVariance,
        ENonPadJumps
    };

    [Serializable]
    public class CSong
    {
        // NOTE: Null is reserved for unparsed data, if data was parsed but simply not found in a file, it should be saved as an empty string/image/0

        public static Image DefaultImage = new Bitmap(1, 1);

        public string Pack { get; set; }
        public string FolderName { get; set; }
        public string Title { get; set; }
        public string Artists => (astrArtists != null) ? string.Join(",", astrArtists) : null;
        public string Genres => (astrGenres != null) ? string.Join(",", astrGenres) : null;
        public string Extensions => (StepFilePaths != null) ? string.Join(",", StepFilePaths.Select(x => Path.GetExtension(x).ToUpper().Trim('.'))) : null;
        public int? LowDifficulty => aDifficulties?.OrderBy(x => x.Difficulty)?.FirstOrDefault()?.Difficulty;
        public int? HighDifficulty => aDifficulties?.OrderBy(x => x.Difficulty)?.LastOrDefault()?.Difficulty;
        public string Difficulties => (aDifficulties != null) ? string.Join(",", aDifficulties.OrderBy(x => x.Difficulty).Select(x => x.Difficulty.ToString())) : "";
        public int? DifficultyCount => aDifficulties?.Count;
        public double? SongLength { get; set; }
        public double? MinBpm => aBpms?.OrderBy(x => x.fBpm)?.FirstOrDefault()?.fBpm;
        public double? AverageBpm { get; set; }
        public double? MaxBpm => aBpms?.OrderBy(x => x.fBpm)?.LastOrDefault()?.fBpm;
        public double? BpmVariance => ((MinBpm == 0) || (MinBpm == null)) ? 0 : Math.Round((double)(((MaxBpm - MinBpm) / MinBpm) * 100), 3);
        public double? DisplayBpm { get; set; }
        public int? BpmCount => aBpms?.Count;
        public string Bpms => (aBpms != null) ? string.Join(",", aBpms.Select(x => x.fBpm.ToString())) : null;
        public int? Stops => aStops?.Count;
        public double? MinDifficultyInaccuracy => aDifficulties?.OrderBy(x => Math.Abs(x.RelativeDifficulty - CDifficulty.AverageDifficultyIndexes[x.Difficulty]))?.Select(x => x.RelativeDifficulty - CDifficulty.AverageDifficultyIndexes[x.Difficulty])?.FirstOrDefault();
        public double? MaxDifficultyInaccuracy => aDifficulties?.OrderBy(x => Math.Abs(x.RelativeDifficulty - CDifficulty.AverageDifficultyIndexes[x.Difficulty]))?.Select(x => x.RelativeDifficulty - CDifficulty.AverageDifficultyIndexes[x.Difficulty])?.LastOrDefault();
        public int? NonPadJumps => aDifficulties?.Select(x => x.NonPadJumps).Sum();
        public string MusicPath { get; set; }

        // NOTE: All images must have a matching property with the same name plus "Path" (e.g. "Banner" and "BannerPath")
        public string BannerPath { get; set; }
        [JsonIgnore]
        public Image Banner { get; set; }
        public string BackgroundPath { get; set; }
        [JsonIgnore]
        public Image Background { get; set; }
        public string CdTitlePath { get; set; }
        [JsonIgnore]
        public Image CdTitle { get; set; }
        public string JacketPath { get; set; }
        [JsonIgnore]
        public Image Jacket { get; set; }
        public string DiskImagePath { get; set; }
        [JsonIgnore]
        public Image DiskImage { get; set; }
        public string FolderPath { get; set; }
        public List<string> StepFilePaths { get; set; }

        public int? nExpectedDifficultyCount;
        public List<CDifficulty> aDifficulties;
        public List<CSong> aSimilarSongs;        
        public List<string> astrNames;
        public List<string> astrArtists;
        public List<string> astrSimplifiedNames;
        public List<string> astrSimplifiedArtists;
        public List<string> astrGenres;        
        public List<CBpmSegment> aBpms;
        public List<CStop> aStops;        
        public bool bFlagged;
        public bool bAlreadyScanned; // Used in the duplicate and similar song filter logic       
        public double? fModeBpm;
        public int? nNumberOfBeats;

        public CSong()
        {
            // Let the default values be mull for everything
        }

        public void ReplaceWith(CSong Replacement, Boolean OnlyReplaceNull = false)
        {
            // If the replacement song is null, do nothing
            if (Replacement != null)
            { 
                // Use reflection to get all members
                var members = typeof(CSong).GetMembers().ToList();

                // Loop through each member in the class
                foreach (MemberInfo member in members)
                {
                    // Check if the memeber is a property or field
                    PropertyInfo property = member as PropertyInfo;
                    FieldInfo field = member as FieldInfo;

                    // If the member isn't a property or field, we can skip it
                    if ((property == null)
                    &&  (field == null))
                    {
                        continue;
                    }

                    // If the member is a property, and isn't readable or writeable, we can skip it
                    if ((property != null)
                    &&  (  (property.CanRead == false)
                        || (property.CanWrite == false)))
                    {
                        continue;
                    }

                    // Get the current value and possible new value
                    var valueOld = property?.GetValue(this) ?? field?.GetValue(this);
                    var valueNew = property?.GetValue(Replacement) ?? field?.GetValue(Replacement);

                    // If we can only replace null values, and the current value isn't null, we can skip it
                    if ((OnlyReplaceNull == true)
                    &&  (valueOld != null))
                    {
                        continue;
                    }

                    // Get the type of the property/field
                    Type type = property?.PropertyType ?? field?.FieldType;

                    // Attempt to get the methods used to update an already existing list
                    MethodInfo clear = type?.GetMethod(nameof(IList.Clear));
                    MethodInfo add = type?.GetMethod(nameof(IList.Add));

                    // If the old value is null, but the type is a collection, we need to
                    // initialize the collection before we can copy stuff into it
                    if ((valueOld == null)
                    &&  (clear != null)
                    &&  (add != null)
                    &&  (type?.GetInterface(nameof(IList)) != null))
                    {
                        valueOld = Activator.CreateInstance(type);
                        property?.SetValue(this, valueOld);
                        field?.SetValue(this, valueOld);
                    }

                    // We might mess up bindings if we simply overwrite an existing collection, so 
                    // instead we should call clear and then add range if the collection already exists
                    if ((valueOld != null)
                    &&  (clear != null)
                    &&  (add != null)
                    &&  (type?.GetInterface(nameof(IList)) != null))
                    {
                        clear.Invoke(valueOld, null);
                        if (valueNew != null)
                        {
                            foreach (var value in (valueNew as IList))
                            {
                                add.Invoke(valueOld, new object[1] { value });
                            }
                        }
                    }
                    // If the member is a primitive, or is still null, we can simply overwrite the value
                    else
                    { 
                        // Overwrite the old value
                        property?.SetValue(this, valueNew);
                        field?.SetValue(this, valueNew);
                    }
                }
            }
        }

        public void ResetAllToNull()
        {
            // Use reflection to get all members
            var members = typeof(CSong).GetMembers().ToList();

            // Loop through each member in the class
            foreach (MemberInfo member in members)
            {
                // Check if the memeber is a property or field
                PropertyInfo property = member as PropertyInfo;
                FieldInfo field = member as FieldInfo;

                // If the member isn't a property or field, we can skip it
                if ((property == null)
                && (field == null))
                {
                    continue;
                }

                // If the member is a property, and isn't writeable, we can skip it
                if ((property != null)
                &&  (property.CanWrite == false))
                {
                    continue;
                }

                // Members and properties set during a song search should not be reset
                if ((member.Name != nameof(CSong.Pack))
                &&  (member.Name != nameof(CSong.FolderName))
                &&  (member.Name != nameof(CSong.FolderPath))
                &&  (member.Name != nameof(CSong.MusicPath))
                &&  (member.Name != nameof(CSong.StepFilePaths)))
                {
                    // Overwrite the old value
                    property?.SetValue(this, null);
                    field?.SetValue(this, null);
                }
            }
        }

        public void ClearNulls()
        {
            // Ensure the default image isn't still null somehow
            if (DefaultImage == null)
            {
                DefaultImage = new Bitmap(1, 1);
            }

            // Use reflection to get all properties
            var properties = typeof(CSong).GetProperties().ToList();

            // Loop through each property in the class
            foreach (PropertyInfo property in properties)
            {
                // If the member is a property, and isn't writeable, we can skip it
                if (property.CanWrite == false)
                {
                    continue;
                }

                // Get the current value and possible new value
                var valueOld = property?.GetValue(this);

                // We can only replace null values
                if (valueOld != null)
                {
                    continue;
                }

                // Get the type of the property/field. All of our properties are nullable, so we should get the non-nullable version of the type
                Type type = property?.PropertyType;

                // Lists should already be non-null, so we should only have to handle strings, images, and numerics
                if (type == typeof(string))
                {
                    property?.SetValue(this, String.Empty);
                }
                else if (type == typeof(Image))
                {
                    property?.SetValue(this, DefaultImage);
                }
                else if (type == typeof(double?))
                {
                    property?.SetValue(this, 0.0);
                }
                else
                {
                    property?.SetValue(this, 0);
                }
            }
        }

        static public void ParseSong(CSong NewSong, CSavedOptions SavedOptions)
        {
            CStepFileParser FileParser = null;

            // Reset all non-search members to null (needed for repeated parses)
            NewSong.ResetAllToNull();

            // Once a property is set, another step file can't overwrite it, 
            // so higher priority step files should be parsed first
            // Priority order is: ssc > sm > dwi
            NewSong.StepFilePaths.Sort((x, y) =>
            {
                if (CSongListPopulator.HasExtension(x, "dwi"))
                {
                    return 1;
                }
                else if (CSongListPopulator.HasExtension(y, "dwi"))
                {
                    return -1;
                }
                else if (CSongListPopulator.HasExtension(y, "ssc"))
                {
                    return 1;
                }
                else if (CSongListPopulator.HasExtension(x, "ssc"))
                {
                    return -1;
                }
                return 0;
            });

            if (NewSong.StepFilePaths.Count > 1)
            {
                Console.Write(NewSong.FolderPath + " has multiple step files");
            }

            // Parse each of the step files
            foreach (string path in NewSong.StepFilePaths)
            {
                string strFileContents = "";

                // Attempt to open the step file and read its contents
                try
                {
                    string fullpath = NewSong.FolderPath + "\\" + path;
                    using (StreamReader sr = new StreamReader(fullpath))
                    {
                        strFileContents = sr.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }

                // If the file contents were successfuly read
                if (strFileContents != "")
                {
                    if (CSongListPopulator.HasExtension(path, "dwi"))
                    {
                        FileParser = new CDwiParser();
                    }
                    else if (CSongListPopulator.HasExtension(path, "sm"))
                    {
                        FileParser = new CSmParser();
                    }
                    else if (CSongListPopulator.HasExtension(path, "ssc"))
                    {
                        FileParser = new CSscParser();
                    }
                    else
                    {
                        Console.Write("ERROR: No stepfile exists for song?");
                    }
                }

                // If a file parser was created
                if (FileParser != null)
                {
                    FileParser.Parse(strFileContents, NewSong, path, SavedOptions);
                }
            }

            // Ensure that all properties are no longer null since they've been parsed
            NewSong.ClearNulls();
        }

        public string GetSongDataString(ESongData eSongData)
        {
            string strDataString = "";

            // Start with an open bracket
            strDataString += "[";

            // If the song name wasn't identified, the step file couldn't be parsed
            if (((this.astrNames?.Count ?? 0) > 0)
            && (this.astrNames[0].Length > 0))
            {
                // Conditionally print the title and author
                if (eSongData == ESongData.EBasic 
                ||  eSongData == ESongData.EAdvanced)
                {
                    // Add song title
                    strDataString += "Title: " + this.astrNames[0];

                    // Add a delimiter
                    strDataString += " | ";

                    // Add song author
                    if ((this.astrArtists?.Count ?? 0) > 0)
                    {
                        strDataString += "Author: " + this.astrArtists[0];

                        // Add a delimiter
                        strDataString += " | ";
                    }
                }

                // Conditionally print the BPM
                if (eSongData == ESongData.EBpm
                ||  eSongData == ESongData.EBasic
                ||  eSongData == ESongData.EAdvanced)
                {
                    // Add song BPM, if it's available
                    if (this.AverageBpm > 0.0)
                    {
                        strDataString += "Average BPM: " + this.AverageBpm.ToString();
                    }
                    else
                    {
                        strDataString += "ERROR: Unable to parse BPM data";
                    }

                    // Conditionally print the delimiter
                    if (eSongData != ESongData.EBpm)
                    {
                        // Add a delimiter
                        strDataString += " | ";
                    }
                }

                // Conditionally print the length and difficulties
                if (eSongData == ESongData.EDifficulties
                || eSongData == ESongData.EDifficultyVariance
                || eSongData == ESongData.ENonPadJumps
                || eSongData == ESongData.EBasic
                || eSongData == ESongData.EAdvanced)
                {
                    // Add song length and dificulty info only if a step chart was correctly parsed
                    if ((this.aDifficulties?.Count ?? 0) > 0)
                    {
                        // Conditionally print the length
                        if (eSongData == ESongData.EBasic
                        || eSongData == ESongData.EAdvanced)
                        {
                            // Add song length in measures
                            strDataString += "Length in Beats: " + this.nNumberOfBeats.ToString();

                            // Add a delimiter
                            strDataString += " | ";
                        }

                        // Add song difficluties in a list
                        strDataString += "Difficulties:";

                        // Loop through difficulties
                        foreach (CDifficulty Difficulty in this.aDifficulties)
                        {
                            // For every difficulty after the first add a comma
                            if (this.aDifficulties.IndexOf(Difficulty) > 0)
                            {
                                strDataString += ",";
                            }

                            // Conditionally print a newline character
                            if (eSongData == ESongData.EAdvanced
                            ||  eSongData == ESongData.EDifficultyVariance
                            ||  eSongData == ESongData.ENonPadJumps)
                            {
                                strDataString += "\n\t\t";
                            }

                            // Conditionally print the calculated difficulty
                            if (eSongData == ESongData.EDifficultyVariance)
                            {
                                strDataString += "Reported Difficulty:";
                            }

                            // Conditionally print the number of unplayable jumps on a dance pad
                            if (eSongData == ESongData.ENonPadJumps)
                            {
                                strDataString += "Difficulty";
                            }

                            strDataString += " " + Difficulty.Difficulty.ToString();

                            // Conditionally print step chart data
                            if (eSongData == ESongData.EAdvanced)
                            {
                                // Add the open paren
                                strDataString += ": (";

                                // Add number of single steps
                                strDataString += "Single Steps: " + Difficulty.Steps.ToString();

                                // Add a delimiter
                                strDataString += " | ";

                                // Add number of jumps
                                strDataString += "Jumps: " + Difficulty.Jumps.ToString();

                                // Add a delimiter
                                strDataString += " | ";

                                // Add number of holds
                                strDataString += "Holds: " + Difficulty.Holds.ToString();

                                // Add a delimiter
                                strDataString += " | ";

                                // Add number of mines
                                strDataString += "Mines: " + Difficulty.Mines.ToString();

                                // Add the close paren
                                strDataString += ")";
                            }

                            // Conditionally print the calculated difficulty
                            if (eSongData == ESongData.EDifficultyVariance)
                            {
                                strDataString += ", Calculated Difficulty: " + CDifficulty.GetExpectedDifficulty(Difficulty.RelativeDifficulty).ToString();
                            }

                            // Conditionally print the number of unplayable jumps on a dance pad
                            if (eSongData == ESongData.ENonPadJumps)
                            {
                                strDataString += " had " + Difficulty.NonPadJumps.ToString() + " jumps that were unplayable on a dance pad";
                            }
                        }
                    }
                    else
                    {
                        strDataString += "ERROR: Unable to parse step chart data";
                    }
                }
            }
            else
            {
                // Log the error
                strDataString += "ERROR: Unable to parse step file for song information";
            }

            // End with a close bracket
            strDataString += "]";

            return strDataString;
        }

        static public byte[] GetHash(string strFilepath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(strFilepath))
                {
                    return md5.ComputeHash(stream);
                }
            }
        }

        static public bool DoHashesMatch(byte[] anOne, byte[] anTwo)
        {
            bool bReturnValue = true;
            for (int nIndex = 0; nIndex < anOne.Length; nIndex++)
            {
                if (anOne[nIndex] != anTwo[nIndex])
                {
                    bReturnValue = false;
                    break;
                }
            }
            return bReturnValue;
        }

        static public string RemoveParenthensis(string strText)
        {
            int nStartIndex = 0;
            int nEndIndex = 0;

            nStartIndex = strText.IndexOf("(");
            if (nStartIndex != -1)
            {
                nEndIndex = strText.IndexOf(")", nStartIndex);
                if (nEndIndex != -1)
                {
                    strText = strText.Substring(0, nStartIndex) + strText.Substring(nEndIndex + 1);
                }
            }

            nStartIndex = strText.IndexOf("[");
            if (nStartIndex != -1)
            {
                nEndIndex = strText.IndexOf("]", nStartIndex);
                if (nEndIndex != -1)
                {
                    strText = strText.Substring(0, nStartIndex) + strText.Substring(nEndIndex + 1);
                }
            }

            return strText;
        }

        static public string RemovePunctuation(string strText)
        {
            return Regex.Replace(strText, @"[^\w]+", "", RegexOptions.Compiled).Replace("_", "");
        }

        static public int GetStringMatchScore(string strName1, string strName2, string strNameSimplified1, string strNameSimplified2)
        {
            int nReturnValue = 0;

            // If the names exactly match, then the match is worth 4 points
            if ((strName1.Length > 0) && (strName1 == strName2))
            {
                nReturnValue = 5;
            }
            else
            {
                // If the name hasn't been completely stripped away by the above logic
                if ((strNameSimplified1.Length > 0) && (strNameSimplified2.Length > 0))
                {
                    // If the names exactly match without punctuation and parenthensis, then the match is worth 4 points
                    if (strNameSimplified1 == strNameSimplified2)
                    {
                        nReturnValue = 4;
                    }
                    else
                    {
                        // If half of either name is present in the other, then the match is worth 3 points
                        if (DoesHalfStringMatch(strNameSimplified1, strNameSimplified2)
                        ||  DoesHalfStringMatch(strNameSimplified2, strNameSimplified1))
                        {
                            nReturnValue = 3;
                        }
                        // If half of the words from one string can be found in the other, then the match is worth 3 points
                        else if (DoWordSubstringsMatch(strNameSimplified1, strNameSimplified2)
                        ||       DoWordSubstringsMatch(strNameSimplified2, strNameSimplified1))
                        {
                            nReturnValue = 3;
                        }
                        // If only a few letters are different, then the match is worth 3 points
                        else if (AreFewLettersDifferent(strNameSimplified1, strNameSimplified2))
                        {
                            nReturnValue = 3;
                        }
                        // If the letters are rearranged, then the match is worth 2 points
                        /*else if (IsPartialAnagram(strNameSimplified1, strNameSimplified2)
                        ||       IsPartialAnagram(strNameSimplified2, strNameSimplified1))
                        {
                            nReturnValue = 2;
                        }*/
                    }
                }
            }
            return nReturnValue;
        }

        static public int GetStringListMatchScore(List<string> astrName1, List<string> astrName2, List<string> astrNameSimplified1, List<string> astrNameSimplified2)
        {
            int nReturnValue = 0;
            int nHighestReturnValue = 0;

            // Compare all names from one list to all names in the other list
            for (int nOuterIndex = 0; nOuterIndex < astrName1.Count; nOuterIndex++)
            {
                for (int nInnerIndex = 0; nInnerIndex < astrName2.Count; nInnerIndex++)
                {
                    // Compare the names
                    nReturnValue = GetStringMatchScore(astrName1[nOuterIndex], astrName2[nInnerIndex], astrNameSimplified1[nOuterIndex], astrNameSimplified2[nInnerIndex]);

                    // If this value is higher than the highest, overwrite the highest
                    if (nReturnValue > nHighestReturnValue)
                    {
                        nHighestReturnValue = nReturnValue;
                    }
                }
            }

            return nHighestReturnValue;
        }

        static public bool DoWordSubstringsMatch(string strSearchIn, string strSearchFor)
        {
            int nWordSubstringsMatches = 0;

            // Split the string into individual words
            string[] astrWords = strSearchFor.Split(' ');

            // Loop through the list of words
            foreach (string strWord in astrWords)
            {
                // If the word is present in the search-in string
                if (strSearchIn.IndexOf(strWord) != -1)
                {
                    nWordSubstringsMatches++;
                }
            }

            // If at least half of the words matched
            return (nWordSubstringsMatches >= astrWords.Length);
        }

        static public bool DoesHalfStringMatch(string strSearchIn, string strSearchFor)
        {
            bool bDoesHalfStringMatch = false;
            int nHalfLength = 0;
            string strFirstHalf = "";
            string strLastHalf = "";

            // Determine how long half the string is
            nHalfLength = Convert.ToInt32(Math.Floor(strSearchFor.Length / 2.0));

            // If half of the string isn't long enough, try increasing the length
            if ((nHalfLength < 5) && (strSearchFor.Length >= 5))
            {
                nHalfLength = 5;
            }

            // If the search-for string long enough to consider the match valid (at least 5 characters)
            if (nHalfLength >= 5)
            {
                // Get the half strings
                strFirstHalf = strSearchFor.Substring(0, nHalfLength);
                strLastHalf = strSearchFor.Substring(strSearchFor.Length - nHalfLength);

                // If either half string matches the beginning or end of the search-in string, return true
                if ((strSearchIn.IndexOf(strFirstHalf) == 0)
                || (    (strSearchIn.IndexOf(strLastHalf) != -1)
                    &&  (strSearchIn.IndexOf(strLastHalf) >= ((strSearchIn.Length - strLastHalf.Length) - 1))))
                {
                    bDoesHalfStringMatch = true;
                }
            }

            return bDoesHalfStringMatch;
        }

        static public bool AreFewLettersDifferent(string strWord1, string strWord2)
        {
            bool bAreFewLettersDifferent = false;
            int nNumDifferences = 0;

            // If the words are the same length
            if (strWord1.Length == strWord2.Length)
            {
                // Count the number of characters which are different
                for (int nIndex = 0; nIndex < strWord1.Length; nIndex++)
                {
                    if (strWord1[nIndex] != strWord2[nIndex])
                    {
                        nNumDifferences++;
                    }
                }

                // If only a few characters were different (one for every 10 characters), then return true
                if (nNumDifferences < (Math.Floor(strWord1.Length / 10.0) + 1))
                {
                    bAreFewLettersDifferent = true;
                }
            }

            return bAreFewLettersDifferent;
        }

        static public bool IsPartialAnagram(string strSearchIn, string strSearchFor)
        {
            bool bIsPartialAnagram = true;
            int nIndex = 0;

            // If the search-for string is too short, any match is invalid, 
            // unless the search-in string is eqally short
            if ((strSearchFor.Length == strSearchIn.Length)
            || (strSearchFor.Length >= 6))
            {
                foreach (Char nChar in strSearchFor)
                {
                    // Find the character in the provided string
                    nIndex = strSearchIn.IndexOf(nChar);

                    // If the character isn't found, it's not a partial anagram
                    if (nIndex == -1)
                    {
                        bIsPartialAnagram = false;
                        break;
                    }

                    // Replace only the first instance of the occurance. This will prevent future matches
                    strSearchIn = strSearchIn.Substring(0, nIndex) + strSearchIn.Substring(nIndex + 1);
                }
            }
            else
            {
                bIsPartialAnagram = false;
            }

            return bIsPartialAnagram;
        }

        static public bool DoesBpmMatch(double fBpm1, double fBpm2)
        {
            bool bReturnValue = false;

            if (Math.Abs(fBpm1 - fBpm2) < 1)
            {
                bReturnValue = true;
            }

            return bReturnValue;
        }

        static public bool AreSongsSimilar(CSong Song1, CSong Song2, int nSongSimilarity)
        {
            bool bAreSongsSimilar = false;
            int nPoints = 0;

            // Determine how closely the song names match (can return with 5, 4, or 3 points)
            nPoints += GetStringListMatchScore(Song1.astrNames, Song2.astrNames, Song1.astrSimplifiedNames, Song2.astrSimplifiedNames);

            // If the names aren't at all similar, then stop the rest of the comparison
            if (nPoints > 0)
            {
                // Determine how closely the song artists match (can return with 5, 4, or 3 points)
                nPoints += GetStringListMatchScore(Song1.astrArtists, Song2.astrArtists, Song1.astrSimplifiedArtists, Song2.astrSimplifiedArtists);

                // 3 points are awarded for the same BPM and number of beats
                if ((DoesBpmMatch(Song1.AverageBpm ?? 0, Song2.AverageBpm ?? 0))
                &&  (Song1.nNumberOfBeats == Song2.nNumberOfBeats)
                &&  (Song1.nNumberOfBeats > 0))
                {
                    nPoints += 3;
                }

                // 2 points are awarded double BPM and half the number of beats
                else if ((DoesBpmMatch((Song1.AverageBpm ?? 0) * 2, (Song2.AverageBpm ?? 0)))
                && ((Song1.nNumberOfBeats / 2) == Song2.nNumberOfBeats)
                && (Song1.nNumberOfBeats > 0))
                {
                    nPoints += 2;
                }

                // 2 points are awarded half BPM and double the number of beats
                else if ((DoesBpmMatch((Song1.AverageBpm ?? 0) / 2, (Song2.AverageBpm ?? 0)))
                && ((Song1.nNumberOfBeats * 2) == Song2.nNumberOfBeats)
                && (Song1.nNumberOfBeats > 0))
                {
                    nPoints += 2;
                }
                // 1 point is awarded for same, half, or double BPM, or the same number of measures
                else if ((DoesBpmMatch((Song1.AverageBpm ?? 0) / 2, (Song2.AverageBpm ?? 0)))
                ||       (DoesBpmMatch((Song1.AverageBpm ?? 0) * 2, (Song2.AverageBpm ?? 0)))
                ||       (DoesBpmMatch((Song1.AverageBpm ?? 0),     (Song2.AverageBpm ?? 0)))
                ||          ((Song1.nNumberOfBeats == Song2.nNumberOfBeats)
                    &&      (Song1.nNumberOfBeats > 0)))
                {
                    nPoints += 1;
                }

                // 100 points are awarded for identical music
                if (DoHashesMatch(CSong.GetHash(Song1.FolderPath + "\\" + Song1.MusicPath), 
                                  CSong.GetHash(Song2.FolderPath + "\\" + Song2.MusicPath)))
                {
                    nPoints += 100;
                }

                // If the number of points meets or exceeds the required similarity
                if (nPoints >= nSongSimilarity)
                {
                    bAreSongsSimilar = true;
                    if (Song1.aSimilarSongs == null)
                    {
                        Song1.aSimilarSongs = new List<CSong>();
                    }
                    Song1.aSimilarSongs.Add(Song2);
                    if (Song2.aSimilarSongs == null)
                    {
                        Song2.aSimilarSongs = new List<CSong>();
                    }
                    Song2.aSimilarSongs.Add(Song1);
                }
            }

            return bAreSongsSimilar;
        }

        static public bool DoSongsMatch(CSong Song1, CSong Song2)
        {
            bool bReturnValue = true;

            if ((Song1.astrNames.Count > 0) 
            &&  (Song2.astrNames.Count > 0) 
            &&  (Song1.astrNames[0].ToLower() != Song2.astrNames[0].ToLower()))
            {
                bReturnValue = false;
            }

            if ((DoesBpmMatch((Song1.AverageBpm ?? 0), (Song2.AverageBpm ?? 0)) == false))
            {
                bReturnValue = false;
            }

            if (Song1.SongLength.ToString() != Song2.SongLength.ToString())
            {
                bReturnValue = false;
            }

            foreach (CDifficulty Difficulty1 in Song1.aDifficulties)
            {
                bool bDifficultyMatch = false;

                foreach (CDifficulty Difficulty2 in Song2.aDifficulties)
                {
                    if (Difficulty1.Difficulty == Difficulty2.Difficulty)
                    {
                        bDifficultyMatch = true;
                        break;
                    }
                }

                if (bDifficultyMatch == false)
                {
                    bReturnValue = false;
                    break;
                }
            }

            foreach (CDifficulty Difficulty2 in Song2.aDifficulties)
            {
                bool bDifficultyMatch = false;

                foreach (CDifficulty Difficulty1 in Song1.aDifficulties)
                {
                    if (Difficulty1.Difficulty == Difficulty2.Difficulty)
                    {
                        bDifficultyMatch = true;
                        break;
                    }
                }

                if (bDifficultyMatch == false)
                {
                    bReturnValue = false;
                    break;
                }
            }

            return bReturnValue;
        }

        static public int CountPlayableDifficulties(CSong TestSong, int nMaxDifficulty)
        {
            int nPlayableDifficulties = 0;

            foreach (CDifficulty Difficulty in TestSong.aDifficulties)
            {
                if (Difficulty.Difficulty <= nMaxDifficulty)
                {
                    nPlayableDifficulties++;
                }
            }

            return nPlayableDifficulties;
        }

        static public void DeleteSongFiles(CSong Song)
        {
            DirectoryInfo SongFolder = null;
            IEnumerable<FileInfo> SongFiles = null;

            // Attempt to open the song folder     
            try
            {
                SongFolder = new DirectoryInfo(Song.FolderPath);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            // If the folder was successfully opened
            if (SongFolder != null)
            {
                // Attempt to create a list of all the song files
                try
                {
                    SongFiles = SongFolder.EnumerateFiles();
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }

            // If the list was successfully created
            if (SongFiles != null)
            {
                // If this song was already seen before, delete all files in the folder
                // Leave the folder though
                foreach (FileInfo SongFile in SongFiles)
                {
                    try
                    {
                        SongFile.Delete();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
        }

        public void RecursivelyMoveFolderContents(DirectoryInfo src, DirectoryInfo dst)
        {
            FileInfo[] dstFiles = dst?.GetFiles();
            FileInfo[] srcFiles = src?.GetFiles();
            foreach (var file in srcFiles)
            {
                // Exact match
                var match = dstFiles.FirstOrDefault(x => (x.Name == file.Name) && (x.Length == file.Length));
                if (match == null)
                {
                    // If there's not an exact match, we'd ideally want to move the file to the destination folder.
                    // This will throw an exception if a file with the same name already exists.
                    file.MoveTo(dst.FullName);
                }
                // If the file already exists in the destination, we can delete it from the source
                else
                {
                    file.Delete();
                }
            }
            DirectoryInfo[] dstFolders = dst?.GetDirectories();
            DirectoryInfo[] srcFolders = src?.GetDirectories();
            foreach (var folder in srcFolders)
            {
                // Exact match
                var match = dstFolders.FirstOrDefault(x => (x.Name == folder.Name));
                if (match == null)
                {
                    // If there's not an exact match, we'd ideally want to move the folder to the destination folder.
                    folder.MoveTo(dst.FullName);
                }
                // If the folder already exists in the destination, we can need to recursively check the contents
                else
                {
                    RecursivelyMoveFolderContents(folder, match);
                }
            }
            // If every file from the source folder is now in the destination folder,
            // we can safely delete the source folder. This will throw an exception if
            // not all files and folders were moved out of the folder.
            src.Delete();
        }

        // Called with a folder will filter the song to that folder, without will restore it to "Songs"
        public void MoveFilterSong(string strFolderName = null)
        {
            try
            {
                string NewPath = FolderPath + "\\..\\..\\..\\" + (strFolderName ?? "Songs") + "\\" + Pack + "\\" + FolderName;
                NewPath = Path.GetFullPath((new Uri(NewPath)).LocalPath);

                // This will create all the parent directories (if necessary), which is all we really care about
                if (!Directory.Exists(NewPath))
                {
                    Directory.CreateDirectory(NewPath);

                    // This will only delete the song folder (we can't move it here if there's a folder already there)
                    Directory.Delete(NewPath);

                    // Move the song folder to the new directory
                    Directory.Move(FolderPath, NewPath);
                }
                // If the destination already exists (and it's not the same as the source), try moving individual files instead
                else if (FolderPath != NewPath)
                {
                    RecursivelyMoveFolderContents(new DirectoryInfo(FolderPath), new DirectoryInfo(NewPath));
                }

                // If the song pack folder is now empty, delete it
                DirectoryInfo packFolder = Directory.GetParent(FolderPath);
                if (packFolder.EnumerateDirectories().Count() == 0)
                {
                    Directory.Delete(packFolder.FullName);
                }

                // The parent of the pack folder could be the filter folder or the "Songs" folder.
                // If no filter folder name was supplied, the old path was in a filter folder.
                // If the filter folder is now empty, delete it
                DirectoryInfo filterFolder = packFolder.Parent;
                if ((strFolderName == null)
                && (filterFolder.EnumerateDirectories().Count() == 0)
                && (filterFolder.EnumerateFiles().Count() == 0))
                {
                    Directory.Delete(filterFolder.FullName);
                }

                FolderPath = NewPath;
            }
            catch (IOException exp)
            {
                MessageBox.Show($"{exp.Message}", "Error Filtering Song(s)",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Called with a folder will copy the song to that folder, without will delete the copy
        public void MoveCustomPackSong(string strFolderName, bool create)
        {
            try
            {
                // Safeguard against copying from a filtered song
                string NewPath = FolderPath + "\\..\\..\\..\\Songs\\" + strFolderName + "\\" + FolderName;
                NewPath = Path.GetFullPath((new Uri(NewPath)).LocalPath);

                // If we're moving to the custom song pack folder
                if (create)
                {
                    // This will create all the parent directories, which is all we really care about
                    if (!Directory.Exists(NewPath))
                    {
                        Directory.CreateDirectory(NewPath);
                    }
                    // This will only delete the song folder
                    if (Directory.Exists(NewPath))
                    {
                        Directory.Delete(NewPath);
                    }

                    // Move the song folder to the new directory
                    JunctionPoint.Create(NewPath, FolderPath, false);
                }
                // If we're deleting a song from a custom song pack folder
                else
                {
                    JunctionPoint.Delete(NewPath);
                }
            }
            catch (IOException exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        public Image ImageFromFile(string propertyName, CSavedOptions SavedOptions)
        {
            Image image = null;

            // Only attempt to find and create an image if we have to
            if ((SavedOptions.DetectOnlyDisplayedData == false)
            || (SavedOptions.SongColumns.Contains(propertyName)))
            {
                // Get the targetted property
                PropertyInfo prop = typeof(CSong).GetProperty(propertyName);

                // Only update the value if it exists and is currently null
                if ((prop != null)
                &&  (prop.GetValue(this) == null))
                {
                    // Find the matching path property
                    string path = typeof(CSong).GetProperty(propertyName + "Path")?.GetValue(this) as string;

                    try
                    {
                        // Generate the original image. We have to be careful about how we
                        // do this so that we don't accidentally leave the file locked
                        using (var fs = new FileStream(FolderPath + "\\" + path, FileMode.Open))
                        {
                            var ms = new MemoryStream();
                            fs.CopyTo(ms);
                            ms.Position = 0;
                            Image tmpImage = Image.FromStream(ms);
                            // Resize the image to consume less memory
                            image = new Bitmap(tmpImage, new Size(tmpImage.Width / (tmpImage.Height / 20), 20));
                            tmpImage.Dispose();
                        }
                    }
                    catch
                    {
                        image = DefaultImage;
                    }

                    // Set the property
                    prop?.SetValue(this, image);
                }
            }

            return image;
        }
    }
}
