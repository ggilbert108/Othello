using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Othello.Logic;

namespace Othello.Auxillary
{
    static class BoardUtil
    {
        private const int NORTH = 0;
        private const int SOUTH = 1;
        private const int WEST = 2;
        private const int EAST = 3;
        private const int NORTHEAST = 4;
        private const int SOUTHEAST = 5;
        private const int NORTHWEST = 6;
        private const int SOUTHWEST = 7;

        public static void moveInDirection(int direction, ref Coord coord)
        {
            switch (direction)
            {
                case NORTH:
                {
                    coord.row--;
                    break;
                }
                case SOUTH:
                {
                    coord.row++;
                    break;
                }
                case WEST:
                {
                    coord.col--;
                    break;
                }
                case EAST:
                {
                    coord.col++;
                    break;
                }
                case NORTHEAST:
                {
                    coord.row--;
                    coord.col++;
                    break;
                }
                case SOUTHEAST:
                {
                    coord.row++;
                    coord.col--;
                    break;
                }
                case NORTHWEST:
                {
                    coord.row--;
                    coord.col++;
                    break;
                }
                case SOUTHWEST:
                {
                    coord.row++;
                    coord.col--;
                    break;
                }
            }
        }

        public static int oppositeDirection(int direction)
        {
            switch (direction)
            {
                case NORTH:
                    return SOUTH;
                case SOUTH:
                    return NORTH;
                case EAST:
                    return WEST;
                case WEST:
                    return EAST;
                case NORTHEAST:
                    return SOUTHWEST;
                case SOUTHEAST:
                    return NORTHWEST;
                case NORTHWEST:
                    return SOUTHEAST;
                case SOUTHWEST:
                    return NORTHEAST;
                default:
                    return -1;
            }
        }

        public static void printBoard(Board board, HashSet<Coord> overlay)
        {
            char[,] boardDisplay = new char[Board.SIZE, Board.SIZE];
            for (int i = 0; i < Board.SIZE; i++)
            {
                for (int j = 0; j < Board.SIZE; j++)
                {
                    Coord coord = new Coord(i, j);
                    char displayChar = getPieceChar(board[coord]);
                    boardDisplay[i, j] = displayChar;
                }
            }
            if (overlay != null)
            {
                foreach (Coord coord in overlay)
                {
                    boardDisplay[coord.row, coord.col] = '#';
                }
            }
            
            for (int i = 0; i < Board.SIZE; i++)
            {
                for (int j = 0; j < Board.SIZE; j++)
                {
                    Console.Write(boardDisplay[i, j]);
                }
                Console.WriteLine();
            }
        }

        public static char getPieceChar(Piece piece)
        {
            if (piece == Piece.None)
                return '.';
            if (piece == Piece.Black)
                return 'x';
            return 'o';
        }
    }
}
