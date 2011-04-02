namespace Importer_System
{
    partial class ProgressForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressForm));
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.startBtn = new System.Windows.Forms.Button();
            this.logfileLink = new System.Windows.Forms.LinkLabel();
            this.statusTable = new System.Windows.Forms.DataGridView();
            this.hidedetailsLink = new System.Windows.Forms.LinkLabel();
            this.currentAction = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.cPSC594EntitiesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cPSC594EntitiesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // bottomPanel
            // 
            this.bottomPanel.BackColor = System.Drawing.Color.White;
            this.bottomPanel.Controls.Add(this.startBtn);
            this.bottomPanel.Controls.Add(this.logfileLink);
            this.bottomPanel.Controls.Add(this.statusTable);
            this.bottomPanel.Controls.Add(this.hidedetailsLink);
            this.bottomPanel.Controls.Add(this.currentAction);
            this.bottomPanel.Controls.Add(this.progressBar);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomPanel.Location = new System.Drawing.Point(0, 0);
            this.bottomPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(512, 329);
            this.bottomPanel.TabIndex = 0;
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(427, 10);
            this.startBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(76, 32);
            this.startBtn.TabIndex = 12;
            this.startBtn.Text = "Run";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // logfileLink
            // 
            this.logfileLink.AutoSize = true;
            this.logfileLink.Location = new System.Drawing.Point(448, 80);
            this.logfileLink.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.logfileLink.Name = "logfileLink";
            this.logfileLink.Size = new System.Drawing.Size(50, 13);
            this.logfileLink.TabIndex = 11;
            this.logfileLink.TabStop = true;
            this.logfileLink.Text = "Open log";
            this.logfileLink.Visible = false;
            this.logfileLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.logfileLink_LinkClicked);
            // 
            // statusTable
            // 
            this.statusTable.AllowUserToAddRows = false;
            this.statusTable.AllowUserToDeleteRows = false;
            this.statusTable.AllowUserToResizeColumns = false;
            this.statusTable.AllowUserToResizeRows = false;
            this.statusTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.statusTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.statusTable.BackgroundColor = System.Drawing.Color.White;
            this.statusTable.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.statusTable.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.statusTable.ColumnHeadersHeight = 28;
            this.statusTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.statusTable.Location = new System.Drawing.Point(8, 113);
            this.statusTable.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.statusTable.MultiSelect = false;
            this.statusTable.Name = "statusTable";
            this.statusTable.ReadOnly = true;
            this.statusTable.RowHeadersVisible = false;
            this.statusTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.statusTable.RowTemplate.Height = 24;
            this.statusTable.Size = new System.Drawing.Size(494, 206);
            this.statusTable.TabIndex = 10;
            this.statusTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.statusTable_CellContentClick);
            // 
            // hidedetailsLink
            // 
            this.hidedetailsLink.AutoSize = true;
            this.hidedetailsLink.Location = new System.Drawing.Point(9, 80);
            this.hidedetailsLink.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.hidedetailsLink.Name = "hidedetailsLink";
            this.hidedetailsLink.Size = new System.Drawing.Size(62, 13);
            this.hidedetailsLink.TabIndex = 9;
            this.hidedetailsLink.TabStop = true;
            this.hidedetailsLink.Text = "Hide details";
            this.hidedetailsLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.hidedetailsLink_LinkClicked);
            // 
            // currentAction
            // 
            this.currentAction.AutoSize = true;
            this.currentAction.Location = new System.Drawing.Point(6, 23);
            this.currentAction.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.currentAction.Name = "currentAction";
            this.currentAction.Size = new System.Drawing.Size(70, 13);
            this.currentAction.TabIndex = 8;
            this.currentAction.Text = "Initializing . . .";
            this.currentAction.Click += new System.EventHandler(this.currentAction_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(8, 55);
            this.progressBar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(494, 21);
            this.progressBar.TabIndex = 7;
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 329);
            this.Controls.Add(this.bottomPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "ProgressForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Metric Importer";
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cPSC594EntitiesBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.LinkLabel hidedetailsLink;
        private System.Windows.Forms.Label currentAction;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.BindingSource cPSC594EntitiesBindingSource;
        private System.Windows.Forms.DataGridView statusTable;
        private System.Windows.Forms.LinkLabel logfileLink;
        private System.Windows.Forms.Button startBtn;


    }
}

