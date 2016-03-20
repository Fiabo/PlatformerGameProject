using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;              //used for enabling sound

namespace PlatformerGameProject
{
    public partial class FlappyLeo : Form
    {
        SoundPlayer Leos;  //soundplayer for playing

        Image ImLeo = Properties.Resources.LeoImage;                    //creating image variables with images
        Image ImOscarBottom = Properties.Resources.OscarImageBottom;
        Image ImOscarTop = Properties.Resources.OscarImageTop;

        Font LeoFont = new Font(new FontFamily("Arial"), 40, FontStyle.Bold);        //creating font for drawing strings with text
        Font LostFont = new Font(new FontFamily("Arial"), 20, FontStyle.Bold);

        Graphics g;   //enabling graphics

        Random r = new Random();   //enabling random for randomization purposes

        Leo LeoCr = new Leo();   //creating sample structs for global using
        Oscar o = new Oscar();

        List<Oscar> oList = new List<Oscar>();   //creating a list of oscars to keep numerous oscars in it

        private bool gameOver;  //a flag variable for determining the end of the game

        int sec = 0;  //a variable for time

        int score = 0;  //a variable for score

        float t = 0;  //a temporary variable for determining the Y coordinate of LeoSquare

        public const int maxTime = 1000;  //a constant for determining a period between oscar spawning

        public const float SpeedUp = 0.4f;  //a constant for gravity to imitate uniformly accelerated motiom

        public const float maxLeoSpeed = -12;  //a constant to limit the speed of Leo going up

        public const float OscarHeight = 210;  //constants for Oscar size
        public const float OscarWidth = 70;

        public FlappyLeo()  //initializing form
        {
            InitializeComponent();

            Leos = new SoundPlayer();  //enabling sound
            Leos.Stream = Properties.Resources.LeoSound;

            BackgroundImage = Properties.Resources.mountains;  //enabling background
            ImageAnimator.Animate(BackgroundImage, OnFrameChanged);

            this.BackgroundImageLayout = ImageLayout.Stretch;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        }

        private void OnFrameChanged(object sender, EventArgs e)  //enabling animation of background
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => OnFrameChanged(sender, e)));
                return;
            }
            ImageAnimator.UpdateFrames();
            Invalidate(false);
        }

        public void LeoCreating() //creating Leo instance
        {
            LeoCr.LeoFace = ImLeo;

            LeoCr.Yspeed = 0;

            LeoCr.leoWidth = Width / 8;
            LeoCr.leoHeight = Width / 8;

            LeoCr.LeoSquare = new RectangleF(Width / 2 - LeoCr.leoWidth, Height / 2, LeoCr.leoWidth, LeoCr.leoHeight);
        }

        private Oscar OscarCreating()  //creating Oscar instance
        {
            o.DistanceBetween = 190;

            o.Xlocation = Width;
            o.Ylocation = r.Next(50, 200);

            o.TopOscar = new RectangleF(o.Xlocation, o.Ylocation - o.DistanceBetween, OscarWidth, OscarHeight);
            o.BottomOscar = new RectangleF(o.Xlocation, o.Ylocation + o.DistanceBetween, OscarWidth, OscarHeight);

            return o;
        }

        private void UpdateLeo()  //updating Leo's position in the game 
        {
            LeoCr.Yspeed += SpeedUp;  //changing speed because of gravity

            LeoCr.LeoSquare.Location = new PointF(LeoCr.LeoSquare.Location.X, LeoCr.LeoSquare.Location.Y + LeoCr.Yspeed); 
        }
        private void UpdateOscar()  //updating Oscars' positions in the game
        {
            for (int i = 0; i < oList.Count; i++)  //as we have multiple Oscars, we have to move them all at the same time 
            {
                Oscar osc = oList[i];

                osc.TopOscar.Location = new PointF(osc.TopOscar.Location.X - 5, osc.TopOscar.Location.Y);  //moving them all 5 pixels to the left
                osc.BottomOscar.Location = new PointF(osc.BottomOscar.Location.X - 5, osc.BottomOscar.Location.Y);

                if (!osc.Scored && osc.TopOscar.Location.X < LeoCr.LeoSquare.Location.X && !gameOver)  //updating score
                {
                    score++;
                    osc.Scored = true;  //setting Scored property of each oscar to true. if Leo has already passed this Oscar, he doesn't get points for it
                }

                oList[i] = osc;

                g.DrawImage(ImOscarBottom, osc.BottomOscar);  //drawing both Oscars
                g.DrawImage(ImOscarTop, osc.TopOscar);
            }
        }

        private void Form1_Load(object sender, EventArgs e)  //initializing graphics during the form load
        {
            g = CreateGraphics();
        }    

        private void Form1_MouseDown(object sender, MouseEventArgs e)  //handling the mouse click event
        {
            if (gameOver) return;  //mouse click is disabled when game is over

            LeoCr.Yspeed -= 15; //changing Leo speed to go up

            if (LeoCr.Yspeed < maxLeoSpeed)  //if Leo exceeds maximum speed, it's set to a constant
            {
                LeoCr.Yspeed = maxLeoSpeed;
            }
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)  //handling the double mouse click event needed to relaunch the game after losing
        {
            if (gameOver)
            {
                Application.Restart();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)  //main part of the program - timer tick event. almost everything happens here
        {
            UpdateLeo();  //updating Leo and Oscar with each tick

            UpdateOscar();

            t += LeoCr.Yspeed;   //changing the Y coordinate       

            sec += timer1.Interval;  //changing the time variable to know when to spawn a new Oscar

            if (sec >= maxTime)
            {
                sec = 0;

                Oscar osc = OscarCreating();

                oList.Add(osc);
            }

            g.DrawImage(ImLeo, 200, 200+t, LeoCr.leoWidth, LeoCr.leoHeight);   //drawing Leo

            g.DrawString(score.ToString(), LeoFont, Brushes.AntiqueWhite, 200, 0);  //drawing a string with our score

            if (CollisionCheck())  //checking for Leo's collision with Oscars
            {
                gameOver = true;   //the game is lost when Leo collides with an Oscar          

                if (LeoCr.Yspeed < 0)
                {
                    LeoCr.Yspeed = -maxLeoSpeed;  //he immediately goes down when the game is lost
                }
            }

            if (gameOver)  //showing the YOU LOST sign and further instructions after the loss
            {
                g.DrawString("YOU LOST", LeoFont, Brushes.Red, 100, 140);
                g.DrawString("double-click to try again", LostFont, Brushes.Red, 80, 220);
            }
        }

        private bool CollisionCheck()  //a method for checking collisions 
        {
            foreach(Oscar osc in oList)  //is checked for every oscar
            {
                RectangleF CheckRectTop = osc.TopOscar;     //special rectangles used for checking, because there was some sort of misbehaviour with Oscar image and rectangle size
                RectangleF CheckRectBot = osc.BottomOscar;  //causing some problems with collision checking. therefore, we had to correct them a little so that collisions are calculated more or less accurately

                RectangleF UpperCheck = new RectangleF(0, 0, 500, 1);     //upper and lower rectangles for checking if Leo has left the screen and lost the game
                RectangleF BottomCheck = new RectangleF(0, 500, 500, 1);

                CheckRectTop.Y += 30;   //minor adjustments to test rectangles to correct the collision checks
                CheckRectTop.X -= 10;

                CheckRectBot.Y += 60;
                CheckRectBot.X -= 10;

                if (LeoCr.LeoSquare.IntersectsWith(CheckRectTop) || LeoCr.LeoSquare.IntersectsWith(CheckRectBot) || LeoCr.LeoSquare.IntersectsWith(UpperCheck) || LeoCr.LeoSquare.IntersectsWith(BottomCheck))   //collision check
                {
                    return true;
                }
            }

            return false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)  //main menu of the game, Play button
        {
            Leos.PlayLooping();  //launching music

            LeoCreating();  //creating Leo

            pictureBox1.Image = Properties.Resources.bear;   //drawing a bear chasing Leo in the bottom left corner
            pictureBox1.Visible = true;

            timer1.Start();   //launching timer
           
            pictureBox2.Hide();  //hiding main menu after pressing Play button
            pictureBox3.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)  //exit button
        {
            Application.Exit();
        }
      
    }


    struct Leo  //we used structs instead of classes because it came in handier than classes
    {
        public RectangleF LeoSquare;

        public float leoWidth;
        public float leoHeight;

        public Image LeoFace;

        public float Yspeed;
    }

    struct Oscar
    {
        public RectangleF TopOscar;
        public RectangleF BottomOscar;

        public float Xlocation;
        public float Ylocation;

        public float DistanceBetween;

        public bool Scored;
    }
    
}
