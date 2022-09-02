using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StepManiaHelper
{
    [Serializable]
    public class CDifficulty
    {
        public static List<CDifficulty> aAllDifficulties;
        public static Dictionary<int, int> DifficultyCounts;
        public static Dictionary<int, double> DifficultySums;
        public static SortedDictionary<int, double> AverageDifficultyIndexes;

        public CSong ParentSong;
        public string StepFile { get; set; }
        public int Difficulty { get; set; }
        public int Notes { get; set; }
        public int Steps { get; set; }
        public int Jumps { get; set; }
        public int NumberOfMeasures;
        public int Holds { get; set; }
        public int Rolls { get; set; }
        public int Mines { get; set; }
        public int NonPadJumps { get; set; }
        public double NotesPerMeasure;
        public double AverageDifficulty => CDifficulty.AverageDifficultyIndexes[Difficulty];
        public double RelativeDifficulty { get; set; }
        public double DifficultyInaccuracy => RelativeDifficulty - CDifficulty.AverageDifficultyIndexes[Difficulty];
        public bool bFlagged;

        static CDifficulty()
        {
            CDifficulty.aAllDifficulties = new List<CDifficulty>();
            CDifficulty.DifficultyCounts = new Dictionary<int, int>();
            CDifficulty.DifficultySums = new Dictionary<int, double>();
            CDifficulty.AverageDifficultyIndexes = new SortedDictionary<int, double>();
        }

        public CDifficulty(string StepFile)
        {
            CDifficulty.aAllDifficulties.Add(this);

            this.StepFile = Path.GetFileName(StepFile);
            this.ParentSong = null;
            this.Difficulty = 0;
            this.Notes = 0;
            this.Steps = 0;
            this.Jumps = 0;
            this.NumberOfMeasures = 0;
            this.Holds = 0;
            this.Rolls = 0;
            this.Mines = 0;
            this.NonPadJumps = 0;
        }

        public static void AddDifficulty(CDifficulty Difficulty)
        {
            int nKey = Difficulty.Difficulty;

            // If this difficulty has not been seen before
            if (CDifficulty.DifficultyCounts.ContainsKey(nKey) == false)
            {
                CDifficulty.DifficultyCounts.Add(nKey, 0);
                CDifficulty.DifficultySums.Add(nKey, 0);
                CDifficulty.AverageDifficultyIndexes.Add(nKey, 0);
            }

            // Increment the count and sum for the current difficulty, and calculate the average
            CDifficulty.DifficultyCounts[nKey]++;
            CDifficulty.DifficultySums[nKey] += Difficulty.RelativeDifficulty;
            CDifficulty.AverageDifficultyIndexes[nKey] = CDifficulty.DifficultySums[nKey] / CDifficulty.DifficultyCounts[nKey];
        }

        public static double GetExpectedDifficulty(double fDifficultyIndex)
        {
            int nDifficultyBelow = 0;
            int nDifficultyAbove = 0;
            int nTemp = 0;
            double fExpectedDifficulty = 0;
            double fRange = 0;
            double fRemainder = 0;
            double fFractional = 0;

            // Increase the difficulty until it is no longer below the specified difficulty index
            foreach (KeyValuePair<int, double> KeyValPair in CDifficulty.AverageDifficultyIndexes)
            {
                // Since some people put insane difficulties, stop after the "normal" difficulty range
                if (KeyValPair.Key > 25)
                {
                    break;
                }
                else if (KeyValPair.Value == fDifficultyIndex)
                {
                    fExpectedDifficulty = KeyValPair.Key;
                    break;
                }
                else if (KeyValPair.Value < fDifficultyIndex)
                {
                    nDifficultyBelow = KeyValPair.Key;
                }
                else
                {
                    nDifficultyAbove = KeyValPair.Key;
                    break;
                }                
            }

            // If it was easier than the average of all normal difficulties, set it to the min normal difficulty
            if (nDifficultyBelow == 0)
            {
                fExpectedDifficulty = 1;
            }

            // If it was harder than the average of all normal difficulties, set it to the max normal difficulty
            if (nDifficultyAbove == 0)
            {
                fExpectedDifficulty = nDifficultyBelow;
            }

            // If the expected difficulty has not yet been determined
            if (fExpectedDifficulty == 0)
            {
                // Calculte the range between the difficulty above and the difficulty below
                fRange = CDifficulty.AverageDifficultyIndexes[nDifficultyAbove] - CDifficulty.AverageDifficultyIndexes[nDifficultyBelow];

                // Calculate how far into the range the supplied value is
                fRemainder = fDifficultyIndex - CDifficulty.AverageDifficultyIndexes[nDifficultyBelow];

                // If the difficulty value decreases as the difficulty key increases, reverse the below and above
                if (nDifficultyBelow > nDifficultyAbove)
                {
                    nDifficultyBelow = nTemp;
                    nDifficultyBelow = nDifficultyAbove;
                    nDifficultyAbove = nTemp;
                }

                // Calculate how much the remainder aboves contributes to a whole difficulty
                fFractional = (fRemainder / fRange) * (nDifficultyAbove - nDifficultyBelow);

                // Calculate expected difficulty
                fExpectedDifficulty = nDifficultyBelow + fFractional;
            }

            return fExpectedDifficulty;
        }

        public static Dictionary<int, double> GetStandardDeviations()
        {
            int nKey = 0;
            double fVariance = 0.0;
            Dictionary<int, double> StandardDeviations = new Dictionary<int, double>();

            // Initialize each key in the standard deviation dictionary
            foreach (KeyValuePair<int, int> KeyValPair in CDifficulty.DifficultyCounts)
            {
                StandardDeviations.Add(KeyValPair.Key, 0);
            }

            // First part of the calculation for standard deviation (sum of variances)
            foreach (CDifficulty Sample in CDifficulty.aAllDifficulties)
            {
                if (Sample.NumberOfMeasures > 0)
                {
                    nKey = Sample.Difficulty;
                    fVariance = Sample.RelativeDifficulty - CDifficulty.AverageDifficultyIndexes[nKey];
                    StandardDeviations[nKey] += Math.Pow(fVariance, 2);
                }
            }

            // Second part of the calculation for standard deviation (average of variances)
            foreach (KeyValuePair<int, int> KeyValPair in CDifficulty.DifficultyCounts)
            {
                StandardDeviations[KeyValPair.Key] /= CDifficulty.DifficultyCounts[KeyValPair.Key];
            }

            return StandardDeviations;
        }

        public static string GetDifficultyStatistics()
        {
            string strStatistics = "";
            List<double> aStandardDeviation = new List<double>();
            List<double> aStepsPerMeasure = new List<double>();
            List<int> aNumberOfDifficulties = new List<int>();
            List<int> aDifficultiesOutsideStandardDeviation1 = new List<int>();
            List<int> aDifficultiesOutsideStandardDeviation2 = new List<int>();
            List<int> aDifficultiesOutsideStandardDeviation3 = new List<int>();
            int nMaxDifficulty = 25;

            // Initialize each list
            for (int nDifficulty = 0; nDifficulty <= nMaxDifficulty; nDifficulty++)
            {
                aStepsPerMeasure.Add(0.0);
                aNumberOfDifficulties.Add(0);
                aStandardDeviation.Add(0.0);
                aDifficultiesOutsideStandardDeviation1.Add(0);
                aDifficultiesOutsideStandardDeviation2.Add(0);
                aDifficultiesOutsideStandardDeviation3.Add(0);
            }

            // Sum all the notes-per-measure values
            foreach (CDifficulty Difficulty in CDifficulty.aAllDifficulties)
            {
                if ((Difficulty.NumberOfMeasures > 0)
                &&  (Difficulty.Difficulty < nMaxDifficulty))
                {
                    aStepsPerMeasure[Difficulty.Difficulty] += Difficulty.Notes / Difficulty.NumberOfMeasures;
                    aNumberOfDifficulties[Difficulty.Difficulty] += 1;
                }
            }

            // Calculate the average notes-per-measure for each difficulty
            for (int nDifficulty = 1; nDifficulty <= nMaxDifficulty; nDifficulty++)
            {
                if (aNumberOfDifficulties[nDifficulty] > 0)
                {
                    aStepsPerMeasure[nDifficulty] /= aNumberOfDifficulties[nDifficulty];
                    strStatistics += nDifficulty.ToString() + ": " + aStepsPerMeasure[nDifficulty].ToString() + " Notes Per Measure (averaged from " + aNumberOfDifficulties[nDifficulty].ToString() + " samples)\n";
                }
            }

            strStatistics += "\n";

            // First part of the calculation for standard deviation
            foreach (CDifficulty Difficulty in CDifficulty.aAllDifficulties)
            {
                if ((Difficulty.NumberOfMeasures > 0)
                && (Difficulty.Difficulty < nMaxDifficulty))
                {
                    aStandardDeviation[Difficulty.Difficulty] += Math.Pow((Difficulty.Notes / Difficulty.NumberOfMeasures) - aStepsPerMeasure[Difficulty.Difficulty], 2);
                }
            }

            // Second part of the calculation for standard deviation
            for (int nDifficulty = 1; nDifficulty <= nMaxDifficulty; nDifficulty++)
            {
                if (aNumberOfDifficulties[nDifficulty] > 0)
                {
                    aStandardDeviation[nDifficulty] /= aNumberOfDifficulties[nDifficulty];
                    strStatistics += nDifficulty.ToString() + ": Has a standard deviation of " + aStandardDeviation[nDifficulty].ToString() + "\n";
                }
            }

            strStatistics += "\n";

            // Sum all the notes-per-measure values
            foreach (CDifficulty Difficulty in CDifficulty.aAllDifficulties)
            {
                if ((Difficulty.NumberOfMeasures > 0)
                && (Difficulty.Difficulty < nMaxDifficulty))
                {
                    double dStepsPerMeasure = (Difficulty.Notes / Difficulty.NumberOfMeasures);
                    double dAverageStepsPerMeasure = aStepsPerMeasure[Difficulty.Difficulty];
                    double dStandardDeviation = aStandardDeviation[Difficulty.Difficulty];
                    if ((dStepsPerMeasure - dAverageStepsPerMeasure) > dStandardDeviation)
                    {
                        aDifficultiesOutsideStandardDeviation1[Difficulty.Difficulty]++;
                    }
                    if ((dStepsPerMeasure - dAverageStepsPerMeasure) > (dStandardDeviation * 2))
                    {
                        aDifficultiesOutsideStandardDeviation2[Difficulty.Difficulty]++;
                    }
                    if ((dStepsPerMeasure - dAverageStepsPerMeasure) > (dStandardDeviation * 3))
                    {
                        aDifficultiesOutsideStandardDeviation3[Difficulty.Difficulty]++;
                    }
                }
            }

            // Second part of the calculation for standard deviation
            for (int nDifficulty = 1; nDifficulty <= nMaxDifficulty; nDifficulty++)
            {
                if (aNumberOfDifficulties[nDifficulty] > 0)
                {
                    strStatistics += nDifficulty.ToString() + ": Has " + aDifficultiesOutsideStandardDeviation1[nDifficulty].ToString() + " difficulties above one standard deviation\n";
                    strStatistics += nDifficulty.ToString() + ": Has " + aDifficultiesOutsideStandardDeviation2[nDifficulty].ToString() + " difficulties above two standard deviations\n";
                    strStatistics += nDifficulty.ToString() + ": Has " + aDifficultiesOutsideStandardDeviation3[nDifficulty].ToString() + " difficulties above three standard deviations\n";
                }
            }

            return strStatistics;
        }
    }
}
