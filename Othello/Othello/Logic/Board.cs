using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Othello.Auxillary;

namespace Othello.Logic
{
    public class Board
    {
        public const int SIZE = 8;
        private Piece[] board;
        private HashSet<Coord> blackPieces;
        private HashSet<Coord> whitePieces;
       
        public Board()
        {
            board = new Piece[SIZE * SIZE];
            blackPieces = new HashSet<Coord>();
            whitePieces = new HashSet<Coord>();

            int a = (int)Math.Ceiling(SIZE/3.0);
            int b = a + 1;

            setPiece(new Coord(a, a), Piece.Black, null);
            setPiece(new Coord(a, b), Piece.White, null);
            setPiece(new Coord(b, b), Piece.Black, null);
            setPiece(new Coord(b, a), Piece.White, null);
        }

        public Dictionary<Coord, Piece> makeMove(Coord coord, Piece piece, HashSet<Coord> moves)
        {
            if (moves == null)
            {
                moves = getMoves(piece);
            }
            if (!isLegal(coord, piece) || !moves.Contains(coord))
            {
                return null;
            }

            Dictionary<Coord, Piece> changed = new Dictionary<Coord, Piece>();
            setPiece(coord, piece, changed);
            flipPieces(coord, piece, changed);
            return changed;
        }

        public void undoMove(Dictionary<Coord, Piece> changed)
        {
            foreach (KeyValuePair<Coord, Piece> pair in changed)
            {
                Coord coord = pair.Key;
                Piece piece = pair.Value;
                setPiece(coord, piece, null);
            }
        }

        public HashSet<Coord> getMoves(Piece piece)
        {
            HashSet<Coord> moves = new HashSet<Coord>();
            HashSet<Coord> pieces = (piece == Piece.Black) ? blackPieces : whitePieces;

            foreach (Coord coord in pieces)
            {
                for (int dir = 0; dir < 8; dir++)
                {
                    Coord location = coord;
                    Piece opponent = (piece == Piece.Black) ? Piece.White : Piece.Black;
                    BoardUtil.moveInDirection(dir, ref location);
                    bool moved = false;
                    while (inBounds(location) && getPiece(location) == opponent)
                    {
                        BoardUtil.moveInDirection(dir, ref location);
                        moved = true;
                    }
                    if (!inBounds(location) || getPiece(location) != Piece.None || !moved)
                    {
                        continue;
                    }
                    if (!moves.Contains(location))
                        moves.Add(location);
                }
            }
            return moves;
        }

        public void getScore(ref int blackScore, ref int whiteScore, bool print)
        {
            for (int i = 0; i < SIZE * SIZE; i++)
            {
                Piece piece = board[i];
                if (piece == Piece.Black)
                {
                    blackScore++;                    
                }
                else if (piece == Piece.White)
                {
                    whiteScore++;
                }
            }
            if (false)
            {
                Console.WriteLine("B: " + blackScore);
                Console.WriteLine("W: " + whiteScore);
            }
        }

        public Piece this[Coord coord]
        {
            get { return getPiece(coord); }
        }

        private void flipPieces(Coord coord, Piece piece, Dictionary<Coord, Piece> moves)
        {
            Piece opposite = (piece == Piece.Black) ? Piece.White : Piece.Black;
            
            for (int dir = 0; dir < 8; dir++)
            {
                HashSet<Coord> flip = new HashSet<Coord>();

                Coord location = coord;
                Piece opponent = (piece == Piece.Black) ? Piece.White : Piece.Black;
                BoardUtil.moveInDirection(dir, ref location);
                bool moved = false;
                while (inBounds(location) && getPiece(location) == opponent)
                {
                    flip.Add(location);
                    BoardUtil.moveInDirection(dir, ref location);
                    moved = true;
                }
                if (!inBounds(location) || getPiece(location) != piece || !moved)
                {
                    continue;
                }

                foreach (Coord flipCoord in flip)
                {
                    setPiece(flipCoord, piece, moves);
                }
            }
        }

        private void setPiece(Coord coord, Piece piece, Dictionary<Coord, Piece> moves)
        {
            Piece oldPiece = getPiece(coord);
            if (oldPiece == Piece.White)
                whitePieces.Remove(coord);
            if (oldPiece == Piece.Black)
                blackPieces.Remove(coord);
            if (piece == Piece.Black)
                blackPieces.Add(coord);
            if (piece == Piece.White)
                whitePieces.Add(coord);
            if(moves != null)
                moves.Add(coord, oldPiece);
            board[coord.index()] = piece;
        }

        private Piece getPiece(Coord coord)
        {
            if (!inBounds(coord))
            {
                return Piece.None;
            }
            return board[coord.index()];
        }

        private bool isLegal(Coord coord, Piece piece)
        {
            return inBounds(coord)
                   && piece != Piece.None
                   && getPiece(coord) == Piece.None;
        }

        private bool inBounds(Coord coord)
        {
            return coord.row >= 0
                   && coord.col >= 0
                   && coord.row < SIZE
                   && coord.col < SIZE;
        }
    }

    public struct Coord
    {
        public int row;
        public int col;

        public Coord(int r, int c)
        {
            row = r;
            col = c;
        }

        public int index()
        {
            return row*Board.SIZE + col;
        }

        public override bool Equals(object obj)
        {
            Coord other = (Coord) obj;
            return row == other.row && col == other.col;
        }

        public override int GetHashCode()
        {
            return 57*row + 31*col;
        }

        public override string ToString()
        {
            return "[" + row + ", " + col + "]";
        }
    }
    
    public enum Piece { None, Black, White }
}
