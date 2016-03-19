﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlatformerGameProject
{
    public partial class Form1 : Form
    {

        Image ImLeo = Properties.Resources.LeoImage;
        Image ImOscarBottom = Properties.Resources.OscarImageBottom;
        Image ImOscarTop = Properties.Resources.OscarImageTop;

        List<Oscar> oList = new List<Oscar>();

        Leo LeoCr = new Leo();
        Oscar o = new Oscar();
        Oscar osc;

        float maxLeoSpeed = -8;

        int sec = 0;
        int maxTime = 2000;
        
        float OscarHeight = 300;
        float OscarWidth = 100;

        Graphics g;

        public const float SpeedUp = 0.5f;

        public Form1()
        {
            InitializeComponent();
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
             LeoCr.rotation = 0;           
         }

        private Oscar OscarCreating()
         {
             Random rnd = new Random();
             o.DistanceBetween = Width/2;
             o.Xlocation = Width;
             o.Ylocation = (rnd.Next((int)o.DistanceBetween) - o.DistanceBetween / 2) + Height / 2;

             o.TopOscar = new RectangleF(o.Xlocation, o.Ylocation - o.DistanceBetween / 2 - OscarHeight, OscarWidth, OscarHeight);
             o.BottomOscar = new RectangleF(o.Xlocation, o.Ylocation + o.DistanceBetween / 2, OscarWidth, OscarHeight);

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
                o.TopOscar.Location = new PointF(o.TopOscar.Location.X - 2, o.TopOscar.Location.Y);
                o.BottomOscar.Location = new PointF(o.BottomOscar.Location.X - 2, o.BottomOscar.Location.Y);
                oList[i] = osc;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            LeoCr.Yspeed -= 10;

            if (LeoCr.Yspeed < maxLeoSpeed)
            {
                LeoCr.Yspeed = maxLeoSpeed;
            }
        }
        float t;
         private void timer1_Tick(object sender, EventArgs e)
         {
             sec += timer1.Interval;

             if (sec >= maxTime)
             {
                 sec = 0;
                 Oscar or = OscarCreating();
                 oList.Add(or);
             }

             UpdateLeo();

             UpdateOscar();

             t += LeoCr.Yspeed;


             //g.Clear(Color.SkyBlue);

            g.DrawImage(ImLeo, 0, t, LeoCr.leoWidth, LeoCr.leoHeight);

            g.DrawImage(ImOscarBottom, o.BottomOscar);
            g.DrawImage(ImOscarTop, o.TopOscar);

         }

         private void Form1_Load(object sender, EventArgs e)
         {
             g = CreateGraphics();
             

             g.Transform = new System.Drawing.Drawing2D.Matrix(1, 0, 0, 1, 120, 120);

             LeoCreating();
             
            pictureBox1.Image = Properties.Resources.bear;

             timer1.Start();
         }
        

    }

       
    struct Leo
    {
        public RectangleF LeoSquare;
        public float leoWidth;
        public float leoHeight;
        public Image LeoFace;

        public float Yspeed;
        public float rotation;
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
