using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;
using System.Windows;

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
            try
            {
                //ProgressForm form = new ProgressForm();
                //form.ShowDialog();

                Window importerGui = new Importer_Gui();
                importerGui.ShowDialog();
                
                // Boot the engine that reads configuration file and begins importing
                //Reporter.OpenReporter();
                // Start engine to initialize config file
                //ImportEngine engine = new ImportEngine();
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
