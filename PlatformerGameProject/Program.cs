using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlatformerGameProject
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        struct Leo
        {
            public RectangleF Leo;

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

        Image ImLeo = Properties.Resources.LeoImage;
        Image ImOscar = Properties.Resources.OscarImage;

    }
}
