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
        /// <summary>.
      
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            Form1 start = new Form1();
            start.LeoCreating();
            
        }

       

        

            
    }
}
