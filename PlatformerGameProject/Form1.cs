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

        
        Graphics g;
        
        

        public Form1()
        {
            InitializeComponent();
            
        }

        

        public void LeoCreating()
        {       
             Leo LeoCr = new Leo();
                LeoCr.LeoFace = ImLeo;
                LeoCr.Yspeed = 0;
            LeoCr.leoWidth = Width / 6;
            LeoCr.leoHeight = Width / 6;

                LeoCr.LeoSquare = new RectangleF(Width / 2 - LeoCr.leoWidth, Height / 2, LeoCr.leoWidth, LeoCr.leoHeight);
            LeoCr.rotation = 0;

            g.Clear(Color.Aquamarine);
            g.DrawImage(ImLeo, 0, 0, 50, 50);  //здесь изменил два последних. надо будет вернуть и посмотреть
            

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


         Image ImLeo = Properties.Resources.LeoImage;
         Image ImOscarBottom = Properties.Resources.OscarImageBottom;
         Image ImOscarTop = Properties.Resources.OscarImageTop;

         private void Form1_Click(object sender, EventArgs e)
         {
             g = CreateGraphics();
             LeoCreating();            
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
