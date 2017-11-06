using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Game_winning_chance
{
    class Program
    {
        const int numberOfSets = 1000000; //make sure not to export detailed results if there are > 1M rows or Excel will choke
        const string outputFile = "output.txt"; //the output will be place in the run directory in a text file

        static void Main(string[] args)
        {
            //int winChance = 65; //65%
            int numberOfGamesMax = 19;
            int numberOfGamesMin = 1;
            int winChanceStep = 10;
            int minWinChance = 30;
            int maxWinChance = 70;
            File.Delete(outputFile);

            //OutputDetailedResultsByWinChance(winChance, numberOfGamesMax);
            OutputSummaryResultsVaryingChances(numberOfGamesMax, maxWinChance, minWinChance, winChanceStep, numberOfGamesMin);
        }

        /// <summary>
        /// Outputs results for a range of win chances defined in the parameters. Exports only summary data (because there are often
        /// too many iterations for Excel to handle, 1M+)
        /// </summary>
        /// <param name="maxGames"></param>
        static void OutputSummaryResultsVaryingChances(int maxGames = 9, int maxWinChance = 70, int minWinChance = 30, int winChanceStep = 5, int minGames = 1)
        {
            var output = new List<string> {
                "Set Count\tWonSets\tGame Count\tWin Chance" //header line
            };

            for (int gameCount = minGames; gameCount <= maxGames; gameCount += 2)
            {
                Console.WriteLine($"\rAnalyzer for game count of: {gameCount}");
                for (int winChance = minWinChance; winChance <= maxWinChance; winChance += winChanceStep)
                {
                    Console.Write($"\rWin chance: {winChance}%");
                    var winCount = Enumerable.Range(1, numberOfSets).Select(i => new Set(gameCount, winChance)).Count(s => s.WonSet);
                    output.Add($"{numberOfSets}\t{winCount}\t{gameCount}\t{winChance}");
                }
            }
            File.WriteAllLines(outputFile, output);
        }

        /// <summary>
        /// Outputs results only for a single win chance. Exports detailed set results including individual game outcomes.
        /// </summary>
        /// <param name="winChance"></param>
        /// <param name="maxGames"></param>
        static void OutputDetailedResultsByWinChance(int winChance, int maxGames)
        {            
            for (int gameCount = 1; gameCount <= maxGames; gameCount += 2)
            {
                List<Set> sets = Enumerable.Range(1, numberOfSets).Select(i => new Set(gameCount, winChance)).ToList();
                if (File.Exists(outputFile))
                {
                    File.AppendAllLines(outputFile, sets.Select(s => s.ToString()));
                }
                else
                {
                    File.WriteAllLines(outputFile, sets.Select(s => s.ToString()));
                }
            }
        }
    }
}
