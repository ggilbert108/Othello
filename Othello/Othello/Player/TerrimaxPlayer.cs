using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Othello.Logic;

namespace Othello.Player
{
    class TerrimaxPlayer : MinimaxPlayer
    {
        public TerrimaxPlayer(int maxPlies) :
            base(maxPlies)
        {

        }

        public override Coord chooseMove(Board board, HashSet<Coord> availableMoves)
        {
            float minValue = float.MaxValue;
            Coord worstMove = availableMoves.First();
            foreach (Coord move in availableMoves)
            {
                Dictionary<Coord, Piece> moveState = board.makeMove(move, color, availableMoves);
                Piece oppositeColor = (color == Piece.Black) ? Piece.White : Piece.Black;

                float moveValue = minimax(board, maxPlies, float.MinValue, float.MaxValue, oppositeColor, 0);
                if (moveValue < minValue)
                {
                    minValue = moveValue;
                    worstMove = move;
                }
                board.undoMove(moveState);
            }

            return worstMove;
        }
    }
}
