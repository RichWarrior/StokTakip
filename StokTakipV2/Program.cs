using StokTakipV2.View;
using System;
using System.Windows.Forms;

namespace StokTakipV2
{
    static class Program
    {
        public static string version = "1";
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginScreen());
        }
    }
}
