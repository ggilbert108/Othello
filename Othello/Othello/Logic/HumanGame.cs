using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Othello.Player;

namespace Othello.Logic
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

            Timer timer = new Timer(enemyTurn, null, 50, -1);
            return true;
        }

        public void passTurn()
        {
            passes++;
            enemyTurn(null);
        }

        private void enemyTurn(object m)
        {
            HashSet<Coord> moves = board.getMoves(Piece.Black);
            if (moves.Count == 0)
            {
                passes++;
                return;
            }
            Coord move = enemyPlayer.chooseMove(board, moves);
            board.makeMove(move, Piece.Black, null);
        }

        public HashSet<Coord> getHumanMoves()
        {
            return board.getMoves(Piece.White);
        }
    }
}
