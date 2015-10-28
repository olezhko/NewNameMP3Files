using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace NewNameMP3Files
{
    static class Program
    {
        private static Mutex m_instance;
        private const string m_appName = "NewNameMP3Files";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool tryCreateNewApp;
            m_instance = new Mutex(true, m_appName,
                    out tryCreateNewApp);
            if (tryCreateNewApp)
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Application has been running.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
