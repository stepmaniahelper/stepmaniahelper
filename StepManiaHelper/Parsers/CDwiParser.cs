using StepManiaHelper.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace StepManiaHelper
{
    class CDwiParser : CStepFileParser
    {
        // NOTE: See "https://web.archive.org/web/20030115204103/http://dwi.ddrei.com/readme.html" for breakdown of DWI file format

        public override void ParseBpms(string strFileContents, CSong Song, string StepFile)
        {
            int nStartIndex = 0;
            int nEndIndex = 0;
            int nBpmEndIndex = 0;
            int nBeatIdentifier = 0;
            double fBpmValue = 0.0;
            string strSubstring = "";
            CBpmSegment Bpm = null;
            bool AlreadyHaveBpmList = Song.aBpms.Count > 0;

            // Find the start index of the BPM section
            nStartIndex = strFileContents.IndexOf("#BPM:");

            // If the BPM section exists, find the end of the BPM section
            if (nStartIndex != -1)
            {
                // Increase the start index to the end of the colon
                nStartIndex += 5;

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
                        Bpm = new CBpmSegment(0, fBpmValue);

                        // Add the BPM to the BPM list for the song
                        Song.DisplayBpm = Song.DisplayBpm ?? fBpmValue;

                        // If we already have a list of BPMs don't add to it
                        if (!AlreadyHaveBpmList)
                        {
                            Song.aBpms.Add(Bpm);
                        }
                    }        
                }
            }

            // If we already have a list of BPMs don't add to it
            if (!AlreadyHaveBpmList)
            {
                // Find the start index of the BPM Change section
                nStartIndex = strFileContents.IndexOf("#CHANGEBPM:");

                // If the BPM Change section exists, find the end of the BPM Change section
                if (nStartIndex != -1)
                {
                    // Increase the start index to the end of the colon
                    nStartIndex += 11;

                    // Find the end of the BPM Change section
                    nBpmEndIndex = strFileContents.IndexOf(";", nStartIndex);

                    // If the BPM Change section ends, parse each pair of BPM values
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

                            // Set the start of the BPM value
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
                nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, "#SINGLE:", nStartIndex);
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

                // If the difficulty parse above failed, then the file is faulty and there's no point continuing
                if (nDifficulty == 0)
                {
                    break;
                }

                // Note section start
                nStartIndex = nEndIndex + 1;

                // Note section end
                nEndIndex = strFileContents.IndexOf(";", nStartIndex);
                if (nEndIndex == -1)
                {
                    break;
                }

                // Save the note section
                strSubstring = strFileContents.Substring(nStartIndex, nEndIndex - nStartIndex).ToUpper().Trim();
                strSubstring = Regex.Replace(strSubstring, @"[^\w\<\>\(\)\[\]\{\}\`\'\!]+", "", RegexOptions.Compiled);

                // Create a new difficulty
                NewDifficulty = new CDifficulty(StepFile);
                NewDifficulty.Difficulty = nDifficulty;

                // Increment the count of expected difficulties
                Song.nExpectedDifficultyCount = (Song.nExpectedDifficultyCount ?? 0) + 1;

                // Parse the note section, and add the difficulty to the difficulty list if there were no
                // parsing problems
                if (ParseNotes(strSubstring, NewDifficulty, StepFile) == true)
                {
                    // Save the difficulty information into the song's data
                    Song.aDifficulties.Add(NewDifficulty);
                    NewDifficulty.ParentSong = Song;
                }
                else
                {
                    Console.Write("Unable to parse difficulty in stepfile: " + StepFile);
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
                nStartIndex = CSongListPopulator.GetIndexAfter(strFileContents, "#FREEZE:");

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

        public List<string> GetMeasuresFromNotes(string strNotesContents)
        {
            // Keep track of the current note
            string strNote = "";

            // Keep track of the number of notes in the current 1/8th beat
            int nNoteCount = 0;

            // Keep track of the number of 1/8th beats in the measure
            int nEighthBeatCounts = 0;

            // Keep track of the characters in the current 1/8th beat
            string strEighthBeat = "";

            // Keep track of the characters in the measure
            string strMeasure = "";

            // Keep track of the characters in the group
            int nGroupEndIndex = 0;

            // Keep track of the current timing multiplier (1/8th timing is 1)
            int nTimingMultiplier = 1;

            // Keep track of whether we are in a hold or not
            bool bInHold = false;

            // Create a list of all measures in the difficulty (a measure is one full beat, typically 8 characters)
            List<string> lstMeasures = new List<string>();

            // Loop through each character in the notes block
            for (int nIndex = 0; nIndex < strNotesContents.Length; nIndex++)
            {
                // If the current character starts a timing change
                if (strNotesContents[nIndex] == '(')
                {
                    nTimingMultiplier *= 2;

                    // If the timing zone has no end, or starts in the middle of an eighth beat, that's a fatal error
                    if ((strNotesContents.IndexOf(")", nIndex) == -1)
                    ||  (strEighthBeat.Length > 0))
                    {
                        lstMeasures = null;
                        break;
                    }

                    strMeasure += strNotesContents[nIndex];
                }
                else if (strNotesContents[nIndex] == '[')
                {
                    nTimingMultiplier *= 3;

                    // If the timing zone has no end, or starts in the middle of an eighth beat, that's a fatal error
                    if ((strNotesContents.IndexOf("]", nIndex) == -1)
                    ||  (strEighthBeat.Length > 0))
                    {
                        lstMeasures = null;
                        break;
                    }

                    strMeasure += strNotesContents[nIndex];
                }
                else if (strNotesContents[nIndex] == '{')
                {
                    nTimingMultiplier *= 8;

                    // If the timing zone has no end, or starts in the middle of an eighth beat, that's a fatal error
                    if ((strNotesContents.IndexOf("}", nIndex) == -1)
                    ||  (strEighthBeat.Length > 0))
                    {
                        lstMeasures = null;
                        break;
                    }

                    strMeasure += strNotesContents[nIndex];
                }
                else if (strNotesContents[nIndex] == '`')
                {
                    nTimingMultiplier *= 24;

                    // If the timing zone has no end, or starts in the middle of an eighth beat, that's a fatal error
                    if ((strNotesContents.IndexOf("'", nIndex) == -1)
                    ||  (strEighthBeat.Length > 0))
                    {
                        lstMeasures = null;
                        break;
                    }

                    strMeasure += strNotesContents[nIndex];
                }
                // If the current character ends a timing change
                else if (strNotesContents[nIndex] == ')')
                {
                    // If the timing zone ends in the middle of an eighth beat, 
                    // or ends without having our timing multiplier properly set, that's a fatal error
                    if ((strEighthBeat.Length > 0)
                    ||  ((nTimingMultiplier % 2) != 0))
                    {
                        lstMeasures = null;
                        break;
                    }

                    nTimingMultiplier /= 2;
                    strMeasure += strNotesContents[nIndex];
                }
                else if (strNotesContents[nIndex] == ']')
                {
                    // If the timing zone ends in the middle of an eighth beat, 
                    // or ends without having our timing multiplier properly set, that's a fatal error
                    if ((strEighthBeat.Length > 0)
                    ||  ((nTimingMultiplier % 3) != 0))
                    {
                        lstMeasures = null;
                        break;
                    }

                    nTimingMultiplier /= 3;
                    strMeasure += strNotesContents[nIndex];
                }
                else if (strNotesContents[nIndex] == '}')
                {
                    // If the timing zone ends in the middle of an eighth beat, 
                    // or ends without having our timing multiplier properly set, that's a fatal error
                    if ((strEighthBeat.Length > 0)
                    ||  ((nTimingMultiplier % 8) != 0))
                    {
                        lstMeasures = null;
                        break;
                    }

                    nTimingMultiplier /= 8;
                    strMeasure += strNotesContents[nIndex];
                }
                else if (strNotesContents[nIndex] == '\'')
                {
                    // If the timing zone ends in the middle of an eighth beat, 
                    // or ends without having our timing multiplier properly set, that's a fatal error
                    if ((strEighthBeat.Length > 0)
                    ||  ((nTimingMultiplier % 24) != 0))
                    {
                        lstMeasures = null;
                        break;
                    }

                    nTimingMultiplier /= 24;
                    strMeasure += strNotesContents[nIndex];
                }
                // If the current character appears to be just a normal note character
                else
                {
                    // If the note count matches the timing multiplier, this is a full 1/8th measure
                    if (nNoteCount == nTimingMultiplier)
                    {
                        nNoteCount = 0;
                        nEighthBeatCounts += 1;
                    }

                    // If we have a full 8 1/8th measure, we have a full measure
                    if (nEighthBeatCounts == 8)
                    {
                        lstMeasures.Add(strMeasure);
                        strMeasure = "";
                        nEighthBeatCounts = 0;
                    }

                    // The default note is just this character
                    strNote = strNotesContents[nIndex].ToString();

                    // Reset the hold flag since we can only ignore one note per hold
                    bInHold = false;

                    // If the current character starts a group
                    if (strNotesContents[nIndex] == '<')
                    {
                        // Find the end of the group
                        nGroupEndIndex = strNotesContents.IndexOf(">", nIndex);

                        // If the group has no end, that's a fatal error
                        if (nGroupEndIndex == -1)
                        {
                            lstMeasures = null;
                            break;
                        }

                        // Get the group contents including the open and close tags
                        strNote = strNotesContents.Substring(nIndex, ((nGroupEndIndex + 1) - nIndex));

                        // If the group containing timing changes or holds, that's a fatal error
                        if (Regex.Matches(strNote, @"\(\)\[\]\{\}\`\'\!").Count > 0)
                        {
                            lstMeasures = null;
                            break;
                        }
                    }
                    // If the current character ends a group, that's a fatal error because end tags
                    // should have been associated with start tags in the above logic, so if we hit
                    // this case it means there's an end tag without a start tag
                    else if (strNotesContents[nIndex] == '>')
                    {
                        lstMeasures = null;
                        break;
                    }
                    // If the current character indicates a hold, that's a fatal error because it should
                    // should handled by the below logic, so if we hit this case it means there's two tags
                    // in a row
                    else if (strNotesContents[nIndex] == '!')
                    {
                        lstMeasures = null;
                        break;
                    }

                    // If there is a next character, and the next character indicates this note is part of a hold
                    if (((nIndex + 1) < strNotesContents.Length)
                    &&  (strNotesContents[nIndex + 1] == '!'))
                    {
                        strNote += strNotesContents[nIndex + 1];
                        bInHold = true;
                    }

                    // If we are not currently in a hold, we can increment the note count
                    if (bInHold == false)
                    {
                        nNoteCount += 1;
                    }

                    // Add the current note to the measure
                    strMeasure += strNote;

                    // Move the index based on the length of the note 
                    // (the main loop already increments by 1, so we need to take that into account)
                    nIndex += (strNote.Length - 1);
                }
            }

            // If we have an incomplete measure at the end, add it to the list anyways
            if ((strMeasure.Length > 0)
            &&  (lstMeasures != null))
            {
                lstMeasures.Add(strMeasure);
            }

            return lstMeasures;
        }

        public bool ParseNotes(string strNotesContents, CDifficulty Difficulty, string StepFile)
        {
            int nGroupStartIndex = 0;
            int nGroupEndIndex = 0;
            string strGroup = "";
            string strNotes = "0";
            bool bError = false;
            int nNumberofNotesBeforeGroup = 0;
            int nNumberofNotesInGroup = 0;

            // Don't parse the steps if they contain invalid characters
            if (Regex.Matches(strNotesContents, @"[^\<\>\(\)\[\]\{\}\`\'\!012346789AB]").Count == 0)
            {
                // Get the list of measures from the notes section
                List<string> lstMeasures = GetMeasuresFromNotes(strNotesContents) ?? new List<string>();

                // Set the number of measures in the difficulty
                Difficulty.NumberOfMeasures = lstMeasures.Count;

                // Loop through each measure
                foreach (string strMeasure in lstMeasures)
                { 
                    // Loop through each character in the measure to determine what it means
                    for (int nCurrentChar = 0; nCurrentChar < strMeasure.Length; nCurrentChar++)
                    {
                        // Save the group
                        strGroup = strMeasure.Substring(nCurrentChar, 1);

                        // If the notes character is timing change open or close symbol, ignore it
                        if (Regex.Matches(strGroup, @"[\(\[\{\`\)\]\}\']").Count > 0)
                        {
                            continue;
                        }

                        // If the character is "<", the notes it represents require more than one character
                        if (strGroup == "<")
                        {
                            // Save the group's start index
                            nGroupStartIndex = nCurrentChar;

                            // Find the group's end index
                            nGroupEndIndex = strMeasure.IndexOf(">", nGroupStartIndex);
                            if (nGroupEndIndex == -1)
                            {
                                bError = true;
                                break;
                            }

                            // Resave the group
                            strGroup = strMeasure.Substring(nGroupStartIndex, ((nGroupEndIndex + 1) - nGroupStartIndex));

                            // Update the current character position (at the end of this loop iteration, 
                            // the position will be increased by one, putting it after the ">")
                            nCurrentChar = nGroupEndIndex;

                            // Save the current note count; will be used to determine how many notes are in the group
                            nNumberofNotesBeforeGroup = Difficulty.Notes;

                            // Determine the number of notes in the group
                            for (int nGroupChar = 1; nGroupChar < strGroup.Length - 1; nGroupChar++)
                            {
                                strNotes = GetNotes(strGroup, nGroupChar);
                                bError = ParseNote(strNotes, Difficulty);

                                // If the string of notes already parsed is longer than one character, 
                                // we need to increase out current character index
                                nGroupChar += strNotes.Length - 1;
                            }

                            // Calculate the number of notes that were present in the group
                            nNumberofNotesInGroup = Difficulty.Notes - nNumberofNotesBeforeGroup;

                            // If more than two notes were in the group, then it can't be played on a dance pad
                            if (nNumberofNotesInGroup > 2)
                            {
                                // 
                                Difficulty.NonPadJumps++;
                            }
                        }
                        else
                        {
                            strNotes = GetNotes(strMeasure, nCurrentChar);
                            bError = ParseNote(strNotes, Difficulty);

                            // If the string of notes already parsed is longer than one character, 
                            // we need to increase out current character index
                            nCurrentChar += strNotes.Length - 1;
                        }

                        // If an error occurred, stop parsing the measure
                        if (bError)
                        {
                            break;
                        }
                    }

                    // If an error occurred, stop parsing the file
                    if (bError)
                    {
                        break;
                    }
                }
            }
            // The parse failed
            else
            {
                bError = true;
            }

            return (bError == false);
        }

        public string GetNotes(string strGroup, int nCharacterIndex)
        {
            string strNotes = "";
            char nNextChar = '0';

            // By default the note is only one character
            strNotes = strGroup.Substring(nCharacterIndex, 1);

            // If there are enough characters left in the measure, determine if the current notes represent a hold
            if ((nCharacterIndex + 3) <= strGroup.Length)
            {
                // Save the next character
                nNextChar = strGroup[nCharacterIndex + 1];

                // If the next character is "!" then this is a hold
                if (nNextChar == '!')
                {
                    // Resave the notes
                    strNotes = strGroup.Substring(nCharacterIndex, 3);
                }
            }

            return strNotes;
        }

        public bool ParseNote(string sNoteChars, CDifficulty Difficulty)
        {
            bool nError = false;
            int nOriginalJumpCount = 0;
            int nOriginalNoteCount = 0;
            int nOriginalStepCount = 0;

            // If the note characters represent a hold
            if (sNoteChars.Length == 3)
            {
                // For the displayed notes, increase the appropriate counts
                ParseNote(sNoteChars.Substring(0, 1), Difficulty);

                // The held characters will only increase the hold count, 
                // so save the step, note, and jump counts to be reverted after this next call
                nOriginalJumpCount = Difficulty.Jumps;
                nOriginalNoteCount = Difficulty.Notes;
                nOriginalStepCount = Difficulty.Steps;

                // For the held notes, the only increase should be to the held count
                ParseNote(sNoteChars.Substring(2, 1), Difficulty);

                // Revert the jump and step counts
                Difficulty.Jumps = nOriginalJumpCount;
                Difficulty.Steps = nOriginalStepCount;

                // Use the note count to determine how many hold there were
                Difficulty.Holds += (Difficulty.Notes - nOriginalNoteCount);

                // Revert the note count
                Difficulty.Notes = nOriginalNoteCount;
            }
            else
            {
                // If the note character represents a jump
                if (sNoteChars == "7"
                || sNoteChars == "9"
                || sNoteChars == "1"
                || sNoteChars == "3"
                || sNoteChars == "A"
                || sNoteChars == "B")
                {
                    Difficulty.Jumps++;
                    Difficulty.Notes += 2;
                }
                // If the note character represents a step
                else if (sNoteChars == "8"
                || sNoteChars == "4"
                || sNoteChars == "6"
                || sNoteChars == "2")
                {
                    Difficulty.Notes++;
                    Difficulty.Steps++;
                }
                // If the note character represents that nothing happened
                else if (sNoteChars == "0")
                {

                }
                else
                {
                    nError = true;
                }
            }

            return nError;
        }

        public override void ParseImages(string strFileContents, CSong Song, string StepFile, CSavedOptions SavedOptions)
        {
            int nStartIndex = 0;
            int nEndIndex = 0;
            string tag;

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

            // Attempt to open the song folder, and enumerate the files
            DirectoryInfo dir = null;
            IEnumerable<FileInfo> files = new List<FileInfo>();
            try
            {
                dir = new DirectoryInfo(Song.FolderPath);
                files = dir.EnumerateFiles();
            }
            catch
            {

            }

            // Loop through each file in the list
            foreach (FileInfo file in files)
            {
                // If the file is an image
                if (CSongListPopulator.HasExtension(file.Name, "png"))
                {
                    // If the image name indicates it's a background
                    if (file.Name.ToLower().Contains("bg.png"))
                    {
                        Song.BackgroundPath = Song.BackgroundPath ?? file.Name;
                        Song.ImageFromFile(nameof(Song.Background), SavedOptions);
                    }
                    // The default assumption if the name doesn't imply any specific type is that it's the banner.
                    else if ((file.Name != Song.CdTitlePath)
                    &&       ( (Song.BannerPath == null)
                        ||     (file.Name.ToLower().Contains("bn.png"))))
                    {
                        Song.BannerPath = Song.BannerPath ?? file.Name;
                        Song.ImageFromFile(nameof(Song.Banner), SavedOptions);
                    }
                }
            }
        }
    }
}
