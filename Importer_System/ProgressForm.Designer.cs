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
            this.bottomPanel.Controls.Add(this.statusTable);
            this.bottomPanel.Controls.Add(this.hidedetailsLink);
            this.bottomPanel.Controls.Add(this.currentAction);
            this.bottomPanel.Controls.Add(this.progressBar);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomPanel.Location = new System.Drawing.Point(0, 0);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(682, 405);
            this.bottomPanel.TabIndex = 0;
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
            this.statusTable.Location = new System.Drawing.Point(11, 139);
            this.statusTable.MultiSelect = false;
            this.statusTable.Name = "statusTable";
            this.statusTable.ReadOnly = true;
            this.statusTable.RowHeadersVisible = false;
            this.statusTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.statusTable.RowTemplate.Height = 24;
            this.statusTable.Size = new System.Drawing.Size(659, 254);
            this.statusTable.TabIndex = 10;
            // 
            // hidedetailsLink
            // 
            this.hidedetailsLink.AutoSize = true;
            this.hidedetailsLink.Location = new System.Drawing.Point(12, 99);
            this.hidedetailsLink.Name = "hidedetailsLink";
            this.hidedetailsLink.Size = new System.Drawing.Size(82, 17);
            this.hidedetailsLink.TabIndex = 9;
            this.hidedetailsLink.TabStop = true;
            this.hidedetailsLink.Text = "Hide details";
            this.hidedetailsLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.hidedetailsLink_LinkClicked);
            // 
            // currentAction
            // 
            this.currentAction.AutoSize = true;
            this.currentAction.Location = new System.Drawing.Point(8, 28);
            this.currentAction.Name = "currentAction";
            this.currentAction.Size = new System.Drawing.Size(93, 17);
            this.currentAction.TabIndex = 8;
            this.currentAction.Text = "Initializing . . .";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(11, 68);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(659, 26);
            this.progressBar.TabIndex = 7;
            // 
            // cPSC594EntitiesBindingSource
            // 
            this.cPSC594EntitiesBindingSource.DataSource = typeof(cpsc594_cdl.Common.Models.CPSC594Entities);
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 405);
            this.Controls.Add(this.bottomPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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


    }
}

