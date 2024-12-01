using StepManiaHelper.Helpers;
using StepManiaHelper.Search;
using System;
using System.Collections.Generic;
using System.Data.Entity.Internal;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace StepManiaHelper.Logic
{
    internal class CSongSearcher
    {
        private Options OutputForm;

        private Thread BackgroundThread;
        private bool IsRunning = false;
        private bool RunThread = true;
        public SortableBindingList<CSong> Songs { get; }
        public List<CSong> lstAllSongs = new List<CSong>();
        Action<bool> Callback;

        public CSongSearcher(Options OutputForm)
        {
            this.OutputForm = OutputForm;
            this.Songs = new SortableBindingList<CSong>(this.lstAllSongs);
        }

        public void StartStopSearch(Action<bool> Callback)
        {
            // Save the callback for use from the background thead
            this.Callback = Callback;
            if (IsRunning == false)
            {
                OutputForm.btnApplySearch.Text = OutputForm.strCancelBtnText;
                OutputForm.ChangeSongListSource(Songs);
                IsRunning = true;
                StartSearching();
            }
            else
            {
                StopSearching();
            }
        }

        private void StartSearching()
        {
            this.BackgroundThread = new Thread(Search);
            this.RunThread = true;
            this.BackgroundThread.Start();
        }

        private void StopSearching()
        {
            new Thread(() =>
            {
                // The background thread will need to run stuff on the GUI thread, so the GUI 
                // thread can't lock here waiting for the background thread to exit. Instead 
                // we wait in a separate thread with a callback to handle when the thread completes
                this.RunThread = false;
                this.BackgroundThread?.Join();

            }).Start();
        }

        private void Search()
        {
            DetermineRowVisibilities(OutputForm.SelectedSearch);
            this.Callback(!RunThread);
            IsRunning = false;
        }

        private void DetermineRowVisibilities(CSavedSearch SelectedSearch)
        {
            // Counter to keep track of how many songs we've already evaluated
            int nSong = 0;

            // Create a list of rows and their new visibility states
            lstAllSongs.Clear();

            // Update the current progress so the user knows what's going on
            OutputForm.UpdateTitleAndText("Determining Row Visibilities");

            // Loop through all rows in the data grid            
            foreach (CSong song in OutputForm.StepManiaParser.lstAllSongs)
            {
                // Gracefully handle aborting the thread
                if (!RunThread)
                {
                    break;
                }

                // Update the user as to our progress
                OutputForm.SetStatus("(Song " + (nSong++) + " of " + OutputForm.StepManiaParser.lstAllSongs.Count + ")", 1);

                // The default visibility depends on whether all or just one operand needs to match
                bool visible = (SelectedSearch.Type == ESearchTypes.AND) ? true : false;

                // Search through each operand in our search list
                foreach (CSearchOperand operand in SelectedSearch.Operands)
                {
                    // Get the property associated with the operand
                    PropertyInfo propertyInfo = typeof(CSong).GetProperty(operand.Property);
                    Type propertyType = propertyInfo?.PropertyType;

                    bool? match = null;

                    // If the property is a string
                    if (propertyType == typeof(string))
                    {
                        // Get the value of the property in the song
                        string value = propertyInfo.GetValue(song) as string;

                        // Check to see if the regex matches the current value
                        match = Regex.IsMatch(value, operand.Value);
                    }
                    // If the property is an int or double
                    else if ((propertyType == typeof(int?))
                    || (propertyType == typeof(double?)))
                    {
                        // Get the value of the property in the song
                        double? value = Convert.ToDouble(propertyInfo.GetValue(song));

                        // Convert the string value saved in the operand to a number
                        double compare = 0;
                        double.TryParse(operand.Value, out compare);

                        // Check if the operand evaluates to true
                        switch (operand.OpCode)
                        {
                            case NumericSearch.EQUALS: match = (value == compare); break;
                            case NumericSearch.NOTEQUALS: match = (value != compare); break;
                            case NumericSearch.LESSTHAN: match = (value < compare); break;
                            case NumericSearch.LESSTHANEQUALS: match = (value <= compare); break;
                            case NumericSearch.GREATERTHAN: match = (value > compare); break;
                            case NumericSearch.GREATERTHANEQUALS: match = (value >= compare); break;
                            default: Console.Write("Invalid opcode in numeric search?"); break;
                        }
                    }

                    // If it doesn't match and we need all to match
                    if ((match == false)
                    && (SelectedSearch.Type == ESearchTypes.AND))
                    {
                        // We can hide the row and stop checking operands
                        visible = false;
                        break;
                    }
                    // If it does match and we only need one to match
                    else if ((match == true)
                    && (SelectedSearch.Type == ESearchTypes.OR))
                    {
                        // We can set the visibility and stop checking operands
                        visible = true;
                        break;
                    }
                }

                // Set the row's visibility
                if (visible == true)
                {
                    lstAllSongs.Add(song);
                }
            }
        }
    }
}
