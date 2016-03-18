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

        Leo LeoCr = new Leo();

        float maxLeoSpeed = -123;

        Graphics g;

        public const float SpeedUp = 1.5f;
        
        public Form1()
        {
            InitializeComponent();
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

        private Oscar OscarCreating(float leoHeight, float OscarHeight, float OscarWidth)
         {
             Oscar o = new Oscar();

             Random rnd = new Random();

             o.DistanceBetween = leoHeight*5;

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

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            LeoCr.Yspeed -= 15;

            if (LeoCr.Yspeed < maxLeoSpeed)
            {
                LeoCr.Yspeed = maxLeoSpeed;
            }
        }

         private void timer1_Tick(object sender, EventArgs e)
         {
             UpdateLeo();

             g.Clear(Color.SkyBlue);
             g.DrawImage(ImLeo, 0, LeoCr.Yspeed, LeoCr.leoWidth, LeoCr.leoHeight);
         }

         private void Form1_Load(object sender, EventArgs e)
         {
             g = CreateGraphics();

             LeoCreating();

             g.Transform = new System.Drawing.Drawing2D.Matrix(1, 0, 0, 1, 120, 120);

             Oscar o = OscarCreating(20, 50, 50);

             g.DrawImage(ImOscarBottom, o.BottomOscar);
             g.DrawImage(ImOscarTop, o.TopOscar);

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
