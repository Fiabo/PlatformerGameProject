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
namespace PlatformerGameProject
{
    public partial class Form1 : Form
    {
        SoundPlayer Leos;

        Image ImLeo = Properties.Resources.LeoImage;
        Image ImOscarBottom = Properties.Resources.OscarImageBottom;
        Image ImOscarTop = Properties.Resources.OscarImageTop;

        List<Oscar> oList = new List<Oscar>();

        //Rectangle LineCheck = new Rectangle(240, 240, 500, 1);

        Random r = new Random();

        Leo LeoCr = new Leo();
        Oscar o = new Oscar();

        float maxLeoSpeed = -8;

        int sec = 0;

        int maxTime = 1500;

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

            LeoCr.leoWidth = Width / 6;
            LeoCr.leoHeight = Width / 6;

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
                oList[i] = osc;

                g.DrawImage(ImOscarBottom, osc.BottomOscar);
                g.DrawImage(ImOscarTop, osc.TopOscar);
                
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (gameOver) return;

            LeoCr.Yspeed -= 10;

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
            
            g.DrawImage(ImLeo, 0, t, LeoCr.leoWidth, LeoCr.leoHeight);

            if (CollisionCheck())
            {
                EndOfGame();

                if (LeoCr.Yspeed < 0)
                {
                    LeoCr.Yspeed = -maxLeoSpeed;
                }

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            g = CreateGraphics();
            
            g.Transform = new System.Drawing.Drawing2D.Matrix(1, 0, 0, 1, 200, 250);

            Leos.PlayLooping();

            LeoCreating();

            pictureBox1.Image = Properties.Resources.bear;

            timer1.Start();

        }

        private Oscar OscarCreating()
        {
            o.DistanceBetween = 95;

            o.Xlocation = Width;
            o.Ylocation = -r.Next(50, 200);

            o.TopOscar = new RectangleF(o.Xlocation, o.Ylocation - 2*o.DistanceBetween, OscarWidth, OscarHeight);
            o.BottomOscar = new RectangleF(o.Xlocation, o.Ylocation + 2*o.DistanceBetween, OscarWidth, OscarHeight);

            return o;
        }

        private bool CollisionCheck()
        {
            foreach(Oscar osc in oList)
            {
                if (LeoCr.LeoSquare.IntersectsWith(osc.BottomOscar) || LeoCr.LeoSquare.IntersectsWith(osc.TopOscar))
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
    }
    
}
