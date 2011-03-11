using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using cpsc594_cdl.Common.Models;

namespace Importer_System
{
    public partial class ProgressForm : Form
    {
        private BindingList<StatusNode> outputList;
        private int MAX_VALUE;
        private Size EXPAND_SIZE = new Size(700, 450);
        private Size COLLAPSE_SIZE = new Size(700, 172);
        private String FATAL_ERROR = "An error has caused the program to terminate. Please view the log file.";
        private String LOGFILE_NAME = "metric.log";
        private Boolean showingDetails = true;
        private ImportEngine engine;        // Engine that computes all the metrics
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        public ProgressForm()
        {
            InitializeComponent();
            InitializeData();
            InitializeEngine();
        }

        /// <summary>
        ///     InitializeData - Initializes the form components.
        /// </summary>
        private void InitializeData()
        {
            
            statusTable.AutoGenerateColumns = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Size = EXPAND_SIZE;

            DataGridViewTextBoxColumn metricColumn = new DataGridViewTextBoxColumn();
            metricColumn.DataPropertyName = "Project";
            metricColumn.HeaderText = "Project";
            metricColumn.SortMode = DataGridViewColumnSortMode.NotSortable;

            DataGridViewTextBoxColumn statusColumn = new DataGridViewTextBoxColumn();
            statusColumn.DataPropertyName = "Status";
            statusColumn.HeaderText = "Status";
            statusColumn.SortMode = DataGridViewColumnSortMode.NotSortable;

            statusTable.Columns.Add(metricColumn);
            statusTable.Columns.Add(statusColumn);

            statusTable.CellStateChanged += new DataGridViewCellStateChangedEventHandler(statusTable_CellStateChanged);
        }

        /// <summary>
        ///     InitializeEngine - Read in the config file from the engine and validate the information.
        /// </summary>
        private void InitializeEngine()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projects"></param>
        private void SetTableData(List<String> projects)
        {
            outputList = new BindingList<StatusNode>();
            foreach (String str in projects)
            {
                outputList.Add(new StatusNode(str, "Waiting to calculate"));
            }
            statusTable.DataSource = outputList;
            if (outputList.Count == 0)
                MAX_VALUE = 100;
            else
                MAX_VALUE = 100 / outputList.Count;
            progressBar.Step = MAX_VALUE;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metric"></param>
        /// <param name="status"></param>
        delegate void SetMetricStatusDelegate(string metric, string status);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="metric"></param>
        /// <param name="status"></param>
        public void SetMetricStatus(string metric, string status)
        {
            if (InvokeRequired)
            {
                SetMetricStatusDelegate method = new SetMetricStatusDelegate(SetMetricStatus);
                Invoke(method, metric, status);
                return;
            }
            foreach (StatusNode node in outputList)
            {
                if (node.Project.CompareTo(metric) == 0)
                    node.Status = status;
                if (status.CompareTo("Done") == 0)
                    progressBar.PerformStep();
                else if (status.CompareTo("Calculating") == 0)
                    currentAction.Text = "Calculating " + node.Project + "...";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metric"></param>
        /// <param name="status"></param>
        delegate void SetFinishStatusDelegate(float timeTaken);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="metric"></param>
        /// <param name="status"></param>
        public void SetFinishStatus(long timeMS)
        {
            double timeS = Math.Round(((double)timeMS / (double)1000), 2);
            currentAction.Text = "Complete. Total time: "+timeS+" second(s).";
            logfileLink.Visible = true;
            Reporter.CloseReporter();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void statusTable_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            if (e.StateChanged == DataGridViewElementStates.Selected)
                e.Cell.Selected = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ExecuteProgram()
        {
            //Start the engine to begin the importin
            engine.BeginImporting();
            // Update the metric log files
            //engine.UpdateArchiveDirectory();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hidedetailsLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (showingDetails)
            {
                hidedetailsLink.Text = "Show details";
                this.Size = COLLAPSE_SIZE;
            }
            else
            {
                hidedetailsLink.Text = "Hide details";
                this.Size = EXPAND_SIZE;
            }
            showingDetails = !showingDetails;
        }

        /// <summary>
        ///     Open the programs log file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logfileLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(LOGFILE_NAME);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startBtn_Click(object sender, EventArgs e)
        {
            startBtn.Visible = false;
            ExecuteProgram();
        }
    }
}
