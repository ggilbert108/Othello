using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Othello.Auxillary;
using Othello.Logic;

namespace Othello.Player
{
    public class RandomPlayer : IPlayer
    {
        public Piece color { get; set; }
        public string name { get; set; }

        public RandomPlayer()
        {
            name = "Random Player";
        }

        public Coord chooseMove(Board board, HashSet<Coord> availableMoves)
        {
            return availableMoves.ElementAt(Util.random.Next(availableMoves.Count));
        }
    }
}
