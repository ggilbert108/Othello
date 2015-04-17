using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Othello.Logic;

namespace Othello.Auxillary
{
    static class BoardEvaluator
    {
        public static float evaluateBoard(Board board, Piece player)
        {
            Piece opponent = (player == Piece.Black) ? Piece.White : Piece.Black;
            float score = 0.01f * (evaluateScore(board, player));
            score += 1 * (evaluateMoves(board, player) - evaluateMoves(board, opponent));
            score += 10 * (evaluateCorners(board, player) - evaluateCorners(board, opponent));
            if (score > 10000)
                Console.WriteLine("bad eval");
            return score;
        }

        public static float evaluateScore(Board board, Piece player)
        {
            int blackScore = 0;
            int whiteScore = 0;
            board.getScore(ref blackScore, ref whiteScore, false);

            if (player == Piece.Black)
            {
                return blackScore - whiteScore;
            }
            else
            {
                return whiteScore - blackScore;
            }
        }

        private static float evaluateMoves(Board board, Piece player)
        {
            return board.getMoves(player).Count;
        }

        private static float evaluateCorners(Board board, Piece player)
        {
            int s = Board.SIZE - 1;
            Coord[] coords =
            {
                new Coord(0, 0),
                new Coord(0, s),
                new Coord(s, 0),
                new Coord(s, s)
            };
            float result = 0;
            for (int i = 0; i < 4; i++)
            {
                if (board[coords[i]] == player)
                {
                    result++;
                }
            }
            return result;
        }
    }
}
