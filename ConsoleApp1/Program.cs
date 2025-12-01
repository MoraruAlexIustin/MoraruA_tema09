using OpenTK;
using System;
using System.Windows.Forms; // <-- ACEST IMPORT TREBUIE ADĂUGAT!

namespace ConsoleApp1
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Initializare OpenTK
            Toolkit.Init();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Form1());
        }
    }
}