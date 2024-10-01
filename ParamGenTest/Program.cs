using System;
using System.Windows.Forms;

namespace ParamGenTest
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // ReSharper disable CommentTypo

            // Connection strings

            // SQL Server: Data Source=gigasax;Initial Catalog=DMS5;Integrated Security=SSPI;
            // PostgreSQL: Host=prismdb2.emsl.pnl.gov;Port=5432;Database=dms;UserId=svc-dms

            // ReSharper restore CommentTypo

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
