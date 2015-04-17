using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Othello.Logic;
using Othello.Player;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private HumanGame game;
        private Rectangle r1;
        private Rectangle r2;

        private bool gameOver;
        public Form1()
        {
            InitializeComponent();

            Invalidate();
            IPlayer enemy = new MinimaxPlayer(3);
            game = new HumanGame(enemy);
            gameOver = false;
        }

        public void update(object m)
        {
            Invalidate();
        }

        private void mouseClick(object sender, MouseEventArgs e)
        {
            HashSet<Coord> moves = game.getHumanMoves();
            int cellSize = Width/Board.SIZE;
            int r = e.Y/cellSize;
            int c = e.X/cellSize;
            Coord move = new Coord(r, c);

            if (moves.Count == 0)
            {
                game.passTurn();
                return;
            }
            if (moves.Contains(move))
            {
                if (game.playTurn(move))
                {
                    
                }
                else
                {
                    gameOver = true;
                    Invalidate();
                }
            }
                

            System.Threading.Timer timer = 
                new System.Threading.Timer(update, null, 5000, -1);

            Invalidate();
        }

        private void paintAll(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            Pen p = new Pen(Color.Black, 2);
            Brush limeBrush = new SolidBrush(Color.ForestGreen);
            Brush blackBrush = new SolidBrush(Color.Black);
            Brush whiteBrush = new SolidBrush(Color.White);
            Brush redBrush = new SolidBrush(Color.Red);

            if (gameOver)
            {
                g.FillRectangle(redBrush, 0.0f, 0.0f, (float)Width, (float)Height);
                return;
            }

            g.FillRectangle(limeBrush, 0.0f, 0.0f, (float)Width, (float)Height);
            drawPieces(g, blackBrush, whiteBrush);

            paintGrid(g, p);            
        }

        private void drawPieces(Graphics g, Brush wb, Brush bb)
        {
            for (int i = 0; i < Board.SIZE; i++)
            {
                for (int j = 0; j < Board.SIZE; j++)
                {
                    Coord coord = new Coord(i, j);
                    Piece piece = game.board[coord];
                    if (piece == Piece.Black)
                    {
                        drawPiece(g, bb, i, j);
                    }
                    else if(piece == Piece.White)
                    {
                        drawPiece(g, wb, i, j);
                    }
                }
            }
        }

        private void drawPiece(Graphics g, Brush b, int r, int c)
        {
            int offset = 20;
            int size = (ClientSize.Width / Board.SIZE);
            int x = c*size;
            int y = r*size;
            size -= offset;
            x += offset/2;
            y += offset/2;
            g.FillPie(b, x, y, size, size, 0, 360);
        }

        private void paintGrid(Graphics g, Pen p)
        {
            int size = ClientSize.Width / Board.SIZE;
            for (int i = 0; i < Board.SIZE; i++)
            {
                int sx = 0;
                int sy = i * size;
                int ex = this.ClientSize.Width;
                int ey = sy;
                g.DrawLine(p, sx, sy, ex, ey);
            }

            for (int i = 0; i < Board.SIZE; i++)
            {
                int sy = 0;
                int sx = i * size;
                int ey = this.ClientSize.Height;
                int ex = sx;
                g.DrawLine(p, sx, sy, ex, ey);
            }
        }
    }
}
