using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using MetricAnalyzer.ImporterSystem.Gui;

namespace Importer_System
{
    /// <summary>
    /// Interaction logic for Importer_Gui.xaml
    /// </summary>
    public partial class Importer_Gui : Window
    {
        ObservableCollection<DisplayMetric> metricList = new ObservableCollection<DisplayMetric>();
        
        public Importer_Gui()
        {
            InitializeComponent();
            setupMetricList();
            metricStatusList.ItemsSource = metricList;
        }

        private void setupMetricList()
        {
            metricList.Add(new DisplayMetric(1, "Code Coverage", ""));
            metricList.Add(new DisplayMetric(2, "Value for Tests", ""));
            metricList.Add(new DisplayMetric(3, "Defect Injection Rate", ""));
            metricList.Add(new DisplayMetric(4, "Defect Repair Rate", ""));
            metricList.Add(new DisplayMetric(5, "Resource Utilization", ""));
            metricList.Add(new DisplayMetric(6, "Out of Scope Work", ""));
            metricList.Add(new DisplayMetric(7, "Rework", ""));
            metricList.Add(new DisplayMetric(8, "Velocity Trend", ""));
        }

/*        private void OnListChanged(object sender, EventArgs eventArgs)
        {
            var list = (ObservableCollection<DisplayMetric>) sender;
            metricStatusList.ItemsSource = list;
        }*/

        private void Button_Click_Run(object sender, RoutedEventArgs e)
        {
            // Boot the engine that reads configuration file and begins importing
            Reporter.OpenReporter();
            // Start engine to initialize config file
            ImportEngine engine = new ImportEngine();
            // Start the metric importing
            engine.BeginImporting(metricList);
        }

    }
}
