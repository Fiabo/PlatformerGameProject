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

        
        
         public void LeoCreating()
        {
           
           float leoWidth = Width / 5;
        float leoHeight = Width / 5;
                Leo LeoCr = new Leo();
                LeoCr.LeoFace = ImLeo;
                LeoCr.Yspeed = 0;
                LeoCr.LeoSquare = new RectangleF(Width / 2 - leoWidth, Height / 2, leoWidth, leoHeight);
            }

        private Oscar OscarCreating()
         {
            
             Oscar o = new Oscar();

             Random rnd = new Random();

             o.DistanceBetween = 4 * leoHeight;

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
