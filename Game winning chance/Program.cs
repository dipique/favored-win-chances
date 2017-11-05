using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Game_winning_chance
{
    class Program
    {
        const int numberOfSets = 300000;
        const string outputFile = "output.txt";

        static void Main(string[] args)
        {
            //int winChance = 65; //65%
            int numberOfGamesMax = 20;
            File.Delete(outputFile);

            //OutputDetailedResultsByWinChance(winChance, numberOfGamesMax);
            OutputSummaryResultsVaryingChances(numberOfGamesMax);
        }

        static void OutputSummaryResultsVaryingChances(int maxGames)
        {
            int winChanceStep = 5;
            int minWinChance = 30;
            int maxWinChance = 70;
            List<string> output = new List<string>();
            output.Add("Set Count\tWonSets\tGame Count\tWin Chance");
            for (int gameCount = 1; gameCount <= maxGames; gameCount += 2)
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

    public class Set
    {
        public int NumberOfGames { get; private set; }
        public List<Game> Games { get; private set; }
        public Set(int numberOfGames, int winChancePct)
        {
            NumberOfGames = numberOfGames;
            GamesNeedToWin = Convert.ToInt32(Math.Floor(NumberOfGames / 2m));
            Games = Enumerable.Range(1, numberOfGames)
                              .Select(i => new Game(winChancePct))
                              .ToList();
        }

        public int GamesNeedToWin { get; private set; }
        public bool WonSet => Games.Where(g => g.Won).Count() > GamesNeedToWin;
        public string WonSetString => WonSet ? "Win" : "Loss";

        public override string ToString() => $"{WonSetString}\t{NumberOfGames}\t{String.Join(string.Empty, Games.Select(g => g.ToString()))}";
    }

    public class Game
    {
        public bool Won { get; set; } //True=Win
        private static Random rnd = new Random(TickSeed());
        public Game(int winChance)
        {
            Won = rnd.Next(1, 100) <= winChance;
        }

        public override string ToString() => Won ? "W" : "L";

        private static int TickSeed()
        {
            long currentVal = DateTime.Now.Ticks;
            while (currentVal > int.MaxValue)
            {
                currentVal = currentVal / (1 + DateTime.Now.Second);
            }
            return Convert.ToInt32(currentVal);
        }
    }
}
