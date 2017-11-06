using System;
using System.Collections.Generic;
using System.Linq;

namespace Game_winning_chance
{
    public class Set
    {
        public int NumberOfGames { get; private set; }
        public List<Game> Games { get; private set; }
        public Set(int numberOfGames, int winChancePct)
        {
            NumberOfGames = numberOfGames;
            GamesNeededToWin = Convert.ToInt32(Math.Ceiling(NumberOfGames / 2m));
            Games = Enumerable.Range(1, numberOfGames)
                              .Select(i => new Game(winChancePct))
                              .ToList();
        }

        public int GamesNeededToWin { get; private set; }
        public bool WonSet => Games.Count(g => g.Won) >= GamesNeededToWin;
        public string WonSetString => WonSet ? "Win" : "Loss";

        //Used for the detailed outputs; can be copied and pasted into Excel
        public override string ToString() => $"{WonSetString}\t{NumberOfGames}\t{String.Join(string.Empty, Games.Select(g => g.ToString()))}";
    }
}
