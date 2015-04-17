using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Othello.Auxillary;
using Othello.Logic;

namespace Othello.Player
{
    public class MinimaxPlayer : IPlayer
    {
        public Piece color { get; set; }
        public string name { get; set; }
        public int functionCalls { get; private set; }
        protected readonly int maxPlies;

        public float times = 0;
        public float avg = 0;

        public MinimaxPlayer(int maxPlies)
        {
            this.name = "minimax player";
            this.maxPlies = maxPlies;
            functionCalls = 0;
        }
        
        public virtual Coord chooseMove(Board board, HashSet<Coord> availableMoves)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            
            float maxValue = float.MinValue;
            Coord bestMove = availableMoves.First();

            foreach (Coord move in availableMoves)
            {
                Dictionary<Coord, Piece> moveState = board.makeMove(move, color, availableMoves);
                Piece oppositeColor =  (color == Piece.Black) ? Piece.White : Piece.Black;

                float moveValue = minimax(board, maxPlies, float.MinValue, float.MaxValue, oppositeColor, 0);
                if (moveValue > maxValue)
                {
                    maxValue = moveValue;
                    bestMove = move;
                }
                board.undoMove(moveState);
            }
            if (Math.Abs(maxValue) < 400)
            {
                times++;
                //Console.WriteLine(maxValue);
                avg += maxValue;
            }


            watch.Stop();

            return bestMove;
        }

        protected float minimax(Board board, int depth, float alpha, float beta, Piece playerColor, int timesPassed)
        {
            functionCalls++;
            Piece opponentPiece = (playerColor == Piece.Black) ? Piece.White : Piece.Black;
            bool isMaximizing = (playerColor == color);
            HashSet<Coord> moves = board.getMoves(playerColor);
            if (timesPassed >= 2)
            {
                float score = BoardEvaluator.evaluateScore(board, color);
                if (score > 0)
                {
                    return 500;
                }
                else
                {
                    return -500;
                }
            }
            if (depth <= 0)
            {
                return BoardEvaluator.evaluateBoard(board, color);
            }
            if (moves.Count == 0)
            {
                timesPassed++;
                return minimax(board, depth - 1, alpha, beta, opponentPiece, timesPassed);
            }


            float bestValue;
            if (isMaximizing)
            {
                bestValue = float.MinValue;
                foreach (Coord move in moves)
                {
                    Dictionary<Coord, Piece> moveState = board.makeMove(move, playerColor, moves);
                    float moveValue = minimax(board, depth - 1, alpha, beta, opponentPiece, timesPassed);
                    board.undoMove(moveState);

                    bestValue = Math.Max(bestValue, moveValue);
                    alpha = Math.Max(alpha, moveValue);

                    if (alpha >= beta)
                    {
                        break;
                    }
                }
            }
            else
            {
                bestValue = float.MaxValue;
                foreach (Coord move in moves)
                {
                    Dictionary<Coord, Piece> moveState = board.makeMove(move, playerColor, moves);
                    float moveValue = minimax(board, depth - 1, alpha, beta, opponentPiece, timesPassed);
                    board.undoMove(moveState);

                    bestValue = Math.Min(bestValue, moveValue);
                    beta = Math.Min(beta, moveValue);

                    if (beta <= alpha)
                    {
                        break;
                    }
                }
            }
            return bestValue;
        }        
    }
}
