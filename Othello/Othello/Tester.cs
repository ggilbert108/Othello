using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Othello.Auxillary;
using Othello.Logic;
using Othello.Player;

namespace Othello
{
    class Tester
    {
        static void Main(string[] args)
        {
            IPlayer dummy = new RandomPlayer();
            


            for (int j = 1; j <= 20; j++)
            {
                MinimaxPlayer test = new MinimaxPlayer(3);

                int t1 = 0;
                int t2 = 0;
                for (int i = 0; i < 10; i++)
                {
                    Game game = new Game(test, dummy);

                    int score1 = 0;
                    int score2 = 0;
                    game.playGame(ref score1, ref score2, false);
                    t1 += score1;
                    t2 += score2;

                }
                Console.Write("Depth " + j + " :");
                Console.WriteLine(t1 + " " + t2);

                Console.WriteLine(test.avg/test.times);
            }

            
            Console.WriteLine("Done");

            Console.Read();
        }
    }
}
