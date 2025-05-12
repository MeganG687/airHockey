using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Media;

namespace airHockey
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SolidBrush blueBrush = new SolidBrush(Color.RoyalBlue);
        SolidBrush testBrush = new SolidBrush(Color.Green);
        Pen redPen = new Pen(Color.Crimson, 5);

        Rectangle player1 = new Rectangle(10, 170, 60, 60);
        Rectangle p1Top = new Rectangle(20, 168, 40, 10);
        Rectangle p1Bottom = new Rectangle(20, 222, 40, 10);

        Rectangle player2 = new Rectangle(580, 170, 60, 60);
        Rectangle p2Top = new Rectangle(590, 168, 40, 10);
        Rectangle p2Bottom = new Rectangle(590, 222, 40, 10);

        Rectangle puck = new Rectangle(360, 210, 40, 40);

        int player1Score = 0;
        int player2Score = 0;

        int playerSpeed = 6;
        int puckXSpeed = -8;
        int puckYSpeed = 8;

        bool wDown = false;
        bool sDown = false;
        bool aDown = false;
        bool dDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;
        bool rightArrowDown = false;
        bool leftArrowDown = false;

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
            }
        }

        public void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(System.Drawing.SystemColors.Control);

            e.Graphics.DrawRectangle(redPen, -5, 166, 100, 168);
            e.Graphics.DrawRectangle(redPen, 700, 166, 805, 168);
            e.Graphics.DrawLine(redPen, 400, 0, 400, 600);
            e.Graphics.DrawLine(redPen, 250, 83, 255, 83);
            e.Graphics.DrawEllipse(redPen, 212, 45, 76, 76);

            e.Graphics.FillEllipse(blueBrush, player1);
            e.Graphics.FillEllipse(blueBrush, player2);
            e.Graphics.FillRectangle(testBrush, p1Top);
            e.Graphics.FillRectangle(testBrush, p1Bottom);
            e.Graphics.FillRectangle(testBrush, p2Top);
            e.Graphics.FillRectangle(testBrush, p2Bottom);


            e.Graphics.FillEllipse(blueBrush, puck);
        }

        private void gameEngine_Tick(object sender, EventArgs e)
        {
            //move puck
            puck.X += puckXSpeed;
            puck.Y += puckYSpeed;

            playerMove();

            collisions();

            scoring();

            endGame();

            Refresh();
        }

        void playerMove()
        {
            //move p1
            if (wDown == true && player1.Y > 0)
            {
                player1.Y -= playerSpeed;
                p1Top.Y -= playerSpeed;
                p1Bottom.Y -= playerSpeed;
            }

            if (sDown == true && player1.Y < 500 - player1.Height)
            {
                player1.Y += playerSpeed;
                p1Top.Y += playerSpeed;
                p1Bottom.Y += playerSpeed;
            }

            if (aDown == true && player1.X > 0)
            {
                player1.X -= playerSpeed;
                p1Top.X -= playerSpeed;
                p1Bottom.X -= playerSpeed;
            }

            if (dDown == true && player1.X < 400 - player1.Width)
            {
                player1.X += playerSpeed;
                p1Top.X += playerSpeed;
                p1Bottom.X += playerSpeed;
            }

            //move p2
            if (upArrowDown == true && player2.Y > 0)
            {
                player2.Y -= playerSpeed;
                p2Top.Y -= playerSpeed;
                p2Bottom.Y -= playerSpeed;
            }

            if (downArrowDown == true && player2.Y < 500 - player2.Height)
            {
                player2.Y += playerSpeed;
                p2Top.Y += playerSpeed;
                p2Bottom.Y += playerSpeed;
            }

            if (leftArrowDown == true && player2.X > 400)
            {
                player2.X -= playerSpeed;
                p2Top.X -= playerSpeed;
                p2Bottom.X -= playerSpeed;
            }

            if (rightArrowDown == true && player2.X < 800 - player2.Width)
            {
                player2.X += playerSpeed;
                p2Top.X += playerSpeed;
                p2Bottom.X += playerSpeed;
            }
        }

        void collisions()
        {
            SoundPlayer hit = new SoundPlayer(Properties.Resources.hitNoise);

            //check if puck hit wall
            if (puck.Y < 0 || puck.Y > 500 - puck.Height)
            {
                puckYSpeed = -puckYSpeed;
                hit.Play();
            }

            if (puck.X > 800 - puck.Width || puck.X < 0)
            {
                puckXSpeed = -puckXSpeed;
                hit.Play();
            }

            //check if puck hit player
            //if yes, change direction l/r
            //and place puck infront of player hit

            if (p1Top.IntersectsWith(puck))
            {
                puckYSpeed = -puckYSpeed;
                puck.Y = player1.Y - puck.Height;
                hit.Play();
            }
            else if (p1Bottom.IntersectsWith(puck))
            {
                puckYSpeed = -puckYSpeed;
                puck.Y = player1.Y + player2.Height;
                hit.Play();
            }
            else if (player1.IntersectsWith(puck) && puck.X > player1.X)
            {
                puckXSpeed = -puckXSpeed;
                puck.X = player1.X + player1.Width;
                hit.Play();
            }
            else if (player1.IntersectsWith(puck) && puck.X < player1.X)
            {
                puckXSpeed = -puckXSpeed;
                puck.X = player1.X - puck.Width;
                hit.Play();
            }

            //p2
            if (p2Top.IntersectsWith(puck))
            {
                puckYSpeed = -puckYSpeed;
                puck.Y = player2.Y - puck.Height;
                hit.Play();
            }
            else if (p2Bottom.IntersectsWith(puck))
            {
                puckYSpeed = -puckYSpeed;
                puck.Y = player2.Y + player2.Height;
                hit.Play();
            }
            else if (player2.IntersectsWith(puck) && puck.X < player2.X)
            {
                puckXSpeed = -puckXSpeed;
                puck.X = player2.X - puck.Width;
                hit.Play();
            }
            else if (player2.IntersectsWith(puck) && puck.X > player2.X)
            {
                puckXSpeed = -puckXSpeed;
                puck.X = player2.X + player2.Width;
                hit.Play();
            }
        }

        void scoring()
        {
            SoundPlayer score = new SoundPlayer(Properties.Resources.scoreNoise);

            //p2 score
            if (puck.X < 0 && puck.Y > 166 && puck.Y < 334)
            {
                player2Score++;

                puck.X = 360 + (puck.Width / 2);
                puck.Y = 210 + (puck.Width / 2);

                score.Play();

                Refresh();
                Thread.Sleep(100);
                score.Play();
            }

            //p1 score
            if ((puck.X + puck.Width) > 800 && puck.Y > 166 && puck.Y < 334)
            {
                player1Score++;

                puck.X = 360 + (puck.Width / 2);
                puck.Y = 210 + (puck.Width / 2);

                score.Play();

                Refresh();
                Thread.Sleep(100);
                score.Play();
            }

            scoreLabel.Text = $"{player1Score} | {player2Score}";
        }

        void endGame()
        {
            SoundPlayer score = new SoundPlayer(Properties.Resources.scoreNoise);

            if (player2Score == 3)
            {
                gameEngine.Stop();

                winLabel.Text = "Player 2 wins";

                restartButton.Visible = true;

                score.Play();
            }

            if (player1Score == 3)
            {
                gameEngine.Stop();

                winLabel.Text = "Player 1 wins";

                restartButton.Visible = true;

                score.Play();
            }
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            player1Score = 0;
            player2Score = 0;

            restartButton.Visible = false;

            //Form1_Paint();
        }
    }
}
