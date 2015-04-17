using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Othello.Auxillary;
using Othello.Player;

namespace Othello.Logic
{
    class Game
    {
        private IPlayer player1;
        private IPlayer player2;

        public Game(IPlayer p1, IPlayer p2)
        {
            player1 = p1;
            player2 = p2;

            player1.color = Piece.Black;
            player2.color = Piece.White;
        }

        public void playGame(ref int p1Score, ref int p2Score, bool print)
        {
            Board board = new Board();

            IPlayer currentPlayer = player1;
            int passes = 0;
            while (passes < 2)
            {
                HashSet<Coord> moves = board.getMoves(currentPlayer.color);
                bool pass = false;
                if (moves.Count == 0)
                {
                    pass = true;
                    passes++;
                }
                else
                {
                    passes = 0;
                }

                if (!pass)
                {
                    Coord move = currentPlayer.chooseMove(board, moves);
                    board.makeMove(move, currentPlayer.color, moves);
                }
                if (print)
                {
                    BoardUtil.printBoard(board, null);
                    Console.WriteLine();    
                }
                currentPlayer = (currentPlayer == player1) ? player2 : player1;
            }
            board.getScore(ref p1Score, ref p2Score, true);
        }
    }
}
