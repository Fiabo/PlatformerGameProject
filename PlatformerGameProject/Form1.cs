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

        Font LeoFont = new Font(new FontFamily("Arial"), 40, FontStyle.Bold);
        Font LostFont = new Font(new FontFamily("Arial"), 20, FontStyle.Bold);

        Graphics g;

        Random r = new Random();

        Leo LeoCr = new Leo();
        Oscar o = new Oscar();

        List<Oscar> oList = new List<Oscar>();

        private bool gameOver;

        int sec = 0;

        int score = 0;

        int maxTime = 1000;

        float t = 0;

        public const float SpeedUp = 0.4f;

        public const float maxLeoSpeed = -12;

        public const float OscarHeight = 210;
        public const float OscarWidth = 70;

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

            LeoCr.leoWidth = Width / 8;
            LeoCr.leoHeight = Width / 8;

            LeoCr.LeoSquare = new RectangleF(Width / 2 - LeoCr.leoWidth, Height / 2, LeoCr.leoWidth, LeoCr.leoHeight);
        }

        private Oscar OscarCreating()
        {
            o.DistanceBetween = 190;

            o.Xlocation = Width;
            o.Ylocation = r.Next(50, 200);

            o.TopOscar = new RectangleF(o.Xlocation, o.Ylocation - o.DistanceBetween, OscarWidth, OscarHeight);
            o.BottomOscar = new RectangleF(o.Xlocation, o.Ylocation + o.DistanceBetween, OscarWidth, OscarHeight);

            return o;
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

        private void Form1_Load(object sender, EventArgs e)
        {
            g = CreateGraphics();
        }    

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (gameOver) return;

            LeoCr.Yspeed -= 15;

            if (LeoCr.Yspeed < maxLeoSpeed)
            {
                LeoCr.Yspeed = maxLeoSpeed;
            }
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (gameOver)
            {
                Application.Restart();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateLeo();

            UpdateOscar();

            t += LeoCr.Yspeed;            

            sec += timer1.Interval;

            if (sec >= maxTime)
            {
                sec = 0;

                Oscar osc = OscarCreating();

                oList.Add(osc);
            }

            g.DrawImage(ImLeo, 200, 200+t, LeoCr.leoWidth, LeoCr.leoHeight);

            g.DrawString(score.ToString(), LeoFont, Brushes.AntiqueWhite, 200, 0);

            if (CollisionCheck())
            {
                gameOver = true;               

                if (LeoCr.Yspeed < 0)
                {
                    LeoCr.Yspeed = -maxLeoSpeed;
                }
            }

            if (gameOver)
            {
                g.DrawString("YOU LOST", LeoFont, Brushes.Red, 100, 140);
                g.DrawString("double-click to try again", LostFont, Brushes.Red, 80, 220);
            }
        }

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

                CheckRectBot.Y += 60;
                CheckRectBot.X -= 10;

                if (LeoCr.LeoSquare.IntersectsWith(CheckRectTop) || LeoCr.LeoSquare.IntersectsWith(CheckRectBot) || LeoCr.LeoSquare.IntersectsWith(UpperCheck) || LeoCr.LeoSquare.IntersectsWith(BottomCheck))
                {
                    return true;
                }
            }

            return false;
        }

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
