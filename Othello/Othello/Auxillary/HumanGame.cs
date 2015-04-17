using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Othello.Logic;
using Othello.Player;

namespace Othello.Auxillary
{
    public class HumanGame
    {
        private IPlayer enemyPlayer;
        public Board board { get; private set; }
        private int passes;

        public HumanGame(IPlayer enemy)
        {
            enemyPlayer = enemy;
            board = new Board();
            passes = 0;
            enemyPlayer.color = Piece.Black;
        }

        public bool playTurn(Coord move)
        {
            board.makeMove(move, Piece.White, null);
            if (passes >= 2)
            {
                return false;
            }
            return true;
        }

        public void passTurn()
        {
            passes++;
            enemyTurn();
        }

        public bool enemyTurn()
        {
            HashSet<Coord> moves = board.getMoves(Piece.Black);
            if (moves.Count == 0)
            {
                passes++;
                if (passes >= 2)
                {
                    return false;
                }
            }
            Coord move = enemyPlayer.chooseMove(board, moves);
            board.makeMove(move, Piece.Black, null);
            return true;
        }

        public HashSet<Coord> getHumanMoves()
        {
            return board.getMoves(Piece.White);
        }

        public bool isGameOver()
        {
            return passes >= 2;
        }
    }
}
