using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Threading;

namespace PlatformerGameProject
{
    public partial class Form1 : Form
    {
        SoundPlayer Leos;

        Image ImLeo = Properties.Resources.LeoImage;
        Image ImOscarBottom = Properties.Resources.OscarImageBottom;
        Image ImOscarTop = Properties.Resources.OscarImageTop;

        List<Oscar> oList = new List<Oscar>();

        Random r = new Random();

        Leo LeoCr = new Leo();
        Oscar o = new Oscar();

        float maxLeoSpeed = -13;

        int sec = 0;

        int endsec = 0;

        int score = 0;

        int maxTime = 1000;

        public const float SpeedUp = 0.4f;

        Graphics g;

        float t;

        float OscarHeight = 210;
        float OscarWidth = 70;

        public Form1()
        {
            InitializeComponent();

            Leos = new SoundPlayer();
            Leos.Stream = Properties.Resources.LeoSound;

            BackgroundImage = Properties.Resources.mountains;
            ImageAnimator.Animate(BackgroundImage, OnFrameChanged);

            this.BackgroundImageLayout = ImageLayout.Stretch;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        }

        private void OnFrameChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => OnFrameChanged(sender, e)));
                return;
            }
            ImageAnimator.UpdateFrames();
            Invalidate(false);
        }
        public void LeoCreating()
        {
            LeoCr.LeoFace = ImLeo;
            LeoCr.Yspeed = 0;

            LeoCr.leoWidth = Width/6;
            LeoCr.leoHeight = Width/6;

            LeoCr.LeoSquare = new RectangleF(Width / 2 - LeoCr.leoWidth, Height / 2, LeoCr.leoWidth, LeoCr.leoHeight);

        }
        private void UpdateLeo()
        {
            LeoCr.Yspeed += SpeedUp;
            LeoCr.LeoSquare.Location = new PointF(LeoCr.LeoSquare.Location.X, LeoCr.LeoSquare.Location.Y + LeoCr.Yspeed);
        }
        private void UpdateOscar()
        {
            for (int i = 0; i < oList.Count; i++)
            {
                Oscar osc = oList[i];
                osc.TopOscar.Location = new PointF(osc.TopOscar.Location.X - 5, osc.TopOscar.Location.Y);
                osc.BottomOscar.Location = new PointF(osc.BottomOscar.Location.X - 5, osc.BottomOscar.Location.Y);

                if (!osc.Scored && osc.TopOscar.Location.X < LeoCr.LeoSquare.Location.X && !gameOver)
                {
                    score++;
                    osc.Scored = true;
                }

                oList[i] = osc;

                g.DrawImage(ImOscarBottom, osc.BottomOscar);
                g.DrawImage(ImOscarTop, osc.TopOscar);

            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (gameOver) return;

            LeoCr.Yspeed -= 13;

            if (LeoCr.Yspeed < maxLeoSpeed)
            {
                LeoCr.Yspeed = maxLeoSpeed;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateLeo();

            t += LeoCr.Yspeed;

            //g.Clear(Color.Snow);

            UpdateOscar();

            sec += timer1.Interval;

            if (sec >= maxTime)
            {
                sec = 0;
                Oscar osc = OscarCreating();
                oList.Add(osc);
            }
            
            g.DrawImage(ImLeo, 200, 200+t, LeoCr.leoWidth, LeoCr.leoHeight);

            if (CollisionCheck())
            {
                EndOfGame();               

                if (LeoCr.Yspeed < 0)
                {
                    LeoCr.Yspeed = -maxLeoSpeed;
                }

            }

            
            g.DrawString(score.ToString(), LeoFont, Brushes.AntiqueWhite, 200, 0);

            if (gameOver)
            {
                g.DrawString("YOU LOST", LeoFont, Brushes.Red, 100, 140);
                g.DrawString("double-click to try again", LostFont, Brushes.Red, 80, 220);
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            g = CreateGraphics();
            
            //g.Transform = new System.Drawing.Drawing2D.Matrix(1, 0, 0, 1, 200, 250);

            

        }

        private Oscar OscarCreating()
        {
            o.DistanceBetween = 95;

            o.Xlocation = Width;
            o.Ylocation = -r.Next(50, 200);

            o.TopOscar = new RectangleF(o.Xlocation, 250+o.Ylocation - 2*o.DistanceBetween, OscarWidth, OscarHeight);
            o.BottomOscar = new RectangleF(o.Xlocation, 250+o.Ylocation + 2*o.DistanceBetween, OscarWidth, OscarHeight);

            return o;
        }

        Font LeoFont = new Font(new FontFamily("Arial"), 40, FontStyle.Bold);
        Font LostFont = new Font(new FontFamily("Arial"), 20, FontStyle.Bold);


        private bool CollisionCheck()
        {
            

            foreach(Oscar osc in oList)
            {
                RectangleF CheckRectTop = osc.TopOscar;
                RectangleF CheckRectBot = osc.BottomOscar;

                RectangleF UpperCheck = new RectangleF(0, 0, 500, 1);
                RectangleF BottomCheck = new RectangleF(0, 500, 500, 1);

                CheckRectTop.Y += 30;
                CheckRectTop.X -= 10;

                CheckRectBot.Y += 50;
                CheckRectTop.X -= 10;

                if (LeoCr.LeoSquare.IntersectsWith(CheckRectTop) || LeoCr.LeoSquare.IntersectsWith(CheckRectBot) || LeoCr.LeoSquare.IntersectsWith(UpperCheck) || LeoCr.LeoSquare.IntersectsWith(BottomCheck))
                {
                    return true;
                }
            }

            return false;
        }

        private void EndOfGame()
        {
            gameOver = true;
        }
        
        private bool gameOver;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Leos.PlayLooping();

            LeoCreating();

            pictureBox1.Image = Properties.Resources.bear;
            pictureBox1.Visible = true;
            timer1.Start();
           
            pictureBox2.Hide();
            pictureBox3.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (gameOver)
            {
                Application.Restart();
            }
        }      
    }


    struct Leo
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
