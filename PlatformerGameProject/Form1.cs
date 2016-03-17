using System;
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



        public Form1()
        {

            InitializeComponent();
        }

        float leoWidth;
        float leoHeight;
        float OscarWidth;
        float OscarHeight;

        public void SizesInit(float leoWidth, float leoHeight, float OscarWidth, float OscarHeight)
        {
             leoWidth = Width / 6;
             leoHeight = Width / 6;
             OscarWidth = Width / 10;
             OscarHeight = Height / 3;
        }

        public void LeoCreating(float leoWidth, float leoHeight)
        {
          
            
             Leo LeoCr = new Leo();
                LeoCr.LeoFace = ImLeo;
                LeoCr.Yspeed = 0;
                LeoCr.LeoSquare = new RectangleF(Width / 2 - leoWidth, Height / 2, leoWidth, leoHeight);
            LeoCr.rotation = 0;
            }

        private Oscar OscarCreating()
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


         Image ImLeo = Properties.Resources.LeoImage;
         Image ImOscarBottom = Properties.Resources.OscarImageBottom;
         Image ImOscarTop = Properties.Resources.OscarImageTop;

        

    }



    struct Leo
    {
        public RectangleF LeoSquare;
        
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
