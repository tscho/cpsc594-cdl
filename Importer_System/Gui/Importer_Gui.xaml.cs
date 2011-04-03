using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Importer_System.Gui;

namespace Importer_System
{
    /// <summary>
    /// Interaction logic for Importer_Gui.xaml
    /// </summary>
    public partial class Importer_Gui : Window
    {
        GUIElements gui_elements = new GUIElements();

        public Importer_Gui()
        {
            InitializeComponent();
            metricStatusList.ItemsSource = gui_elements.metricList;
            progressBar.DataContext = gui_elements.progressPercent;
        }

        private void Button_Click_Config(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Run(object sender, RoutedEventArgs e)
        {
            // Boot the engine that reads configuration file and begins importing
            Reporter.OpenReporter();
            // Start engine to initialize config file
            ImportEngine engine = new ImportEngine();
            // Start the metric importing
            engine.BeginImporting(gui_elements.metricList);
        }
    }
}
