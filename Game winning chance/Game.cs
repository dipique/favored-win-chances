using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_winning_chance
{
    public class Game
    {
        public bool Won { get; set; }
        private static Random rnd = new Random(TickSeed()); //this is static to control the shittyness of pseudo randomness, and also to improve performance
        public Game(int winChance)
        {
            Won = rnd.Next(1, 100) <= winChance;
        }

        //Used for detailed outputs where set results look like this "WWWLW"
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
