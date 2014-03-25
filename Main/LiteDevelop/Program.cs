using System;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Extensions;

namespace LiteDevelop
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {  
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LiteDevelopApplication.Run(args);
        }

        class settings : LiteDevelop.Framework.SettingsMap
        {
            public settings()
            {

            }

            public settings(LiteDevelop.Framework.FileSystem.FilePath filePath)
                : base(filePath)
            {

            }

            public override string DocumentRoot
            {
                get { return "Settings"; }
            }
        }
    }
}
