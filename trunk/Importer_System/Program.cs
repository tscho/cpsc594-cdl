using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Importer_System
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new ProgressForm());
            try
            {
                // Boot the engine that reads configuration file and begins importing
                Reporter.OpenReporter();
                // Start engine to initialize config file
                ImportEngine engine = new ImportEngine();

                Thread threadTest = new Thread(engine.BeginImporting);          // Kick off a new thread
                threadTest.Start();                               // running WriteY()
                // Start the metric importing
                //engine.BeginImporting();
            }
            catch (TerminateException terminateException)
            {
                Reporter.AddTerminateMessageToReporter(terminateException.Message);
            }
            finally
            {
                Reporter.CloseReporter();
            }
            
        }
    }
}
