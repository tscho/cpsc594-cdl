using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;
using System.Windows;
using MetricAnalyzer.ImporterSystem.Gui;

namespace MetricAnalyzer.ImporterSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Window importerGui = new Importer_Gui();
            importerGui.ShowDialog();
        }
    }
}
