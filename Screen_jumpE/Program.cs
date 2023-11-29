using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Screen_jumpE
{
    public class Form_base
    {
        static void Main(string[] args)
        {

        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        void Form_create()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
