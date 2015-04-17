using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Othello.Auxillary;
using Othello.Logic;
using Othello.Player;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private HumanGame game;
        private Rectangle r1;
        private Rectangle r2;
        private Timer enemyTimer;
        private Timer endGameTimer;
        private HashSet<Coord> moves;
        

        private bool gameOver;
        private bool justPassed;
        public Form1()
        {
            InitializeComponent();
            IPlayer enemy = new MinimaxPlayer(3);
            game = new HumanGame(enemy);
            gameOver = false;
            justPassed = false;
            enemyTimer = new Timer();
            endGameTimer = new Timer();
            endGameTimer.Interval = 200;
            endGameTimer.Tick += checkGameOver;
            endGameTimer.Start();
            moves = game.getHumanMoves();
        }

        private void mouseClick(object sender, MouseEventArgs e)
        {
            if (!justPassed)
            {
                int cellSize = Width / Board.SIZE;
                int r = e.Y / cellSize;
                int c = e.X / cellSize;
                Coord move = new Coord(r, c);

                if (moves.Contains(move))
                {
                    if (!game.playTurn(move))
                    {
                        gameOver = true;
                    }
                    else
                    {
                        Refresh();
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                justPassed = false;
            }


            Stopwatch watch = new Stopwatch();

            watch.Start();
            if (!game.enemyTurn())
            {
                gameOver = true;
            }
            watch.Stop();

            int interval = 1000 - (int)watch.ElapsedMilliseconds;
            if (interval < 0)
                interval = 0;

            enemyTimer = new Timer();
            enemyTimer.Interval = interval;
            enemyTimer.Tick += refreshScreen;
            enemyTimer.Start();

            moves = game.getHumanMoves();
            if (moves.Count == 0)
            {
                justPassed = true;
                game.passTurn();
                if (game.isGameOver())
                {
                    gameOver = true;
                }
            }
        }

        private void refreshScreen(object o, EventArgs e)
        {
            Refresh();
            if (enemyTimer.Enabled)
            {
                enemyTimer.Stop();
            }
        }

        private void checkGameOver(object o, EventArgs e)
        {
            if (gameOver)
            {
                Refresh();
                endGameTimer.Stop();
            }
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
                g.FillRectangle(blackBrush, 0.0f, 0.0f, (float)Width, (float)Height);

                string drawString = "Game Over";
                Font drawFont = new Font("Arial", 40);
                float x = 130.0F;
                float y = 50.0F;
                StringFormat drawFormat = new StringFormat();
                g.DrawString(drawString, drawFont, whiteBrush, x, y, drawFormat);
                drawFont.Dispose();
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
