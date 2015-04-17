using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Othello.Logic;

namespace Othello.Player
{
    public interface IPlayer
    {
        Piece color { get; set; }
        string name { get; set; }
        Coord chooseMove(Board board, HashSet<Coord> availableMoves);
    }
}