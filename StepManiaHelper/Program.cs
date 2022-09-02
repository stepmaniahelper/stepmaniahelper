using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace StepManiaHelper
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
            Options StartingForm = new Options();
            Application.Run(StartingForm);
        }
    }
}
