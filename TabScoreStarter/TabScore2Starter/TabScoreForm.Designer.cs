namespace TabScore2Starter
{
    partial class TabScoreForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TabScoreForm));
            this.ScoringDatabaseLabel = new System.Windows.Forms.Label();
            this.PathToDBLabel = new System.Windows.Forms.Label();
            this.AddHandRecordFileButton = new System.Windows.Forms.Button();
            this.HandRecordFileLabel = new System.Windows.Forms.Label();
            this.PathToHandRecordFileLabel = new System.Windows.Forms.Label();
            this.AddDatabaseFileButton = new System.Windows.Forms.Button();
            this.DatabaseFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SessionStatusLabel = new System.Windows.Forms.Label();
            this.AnalysingLabel = new System.Windows.Forms.Label();
            this.AnalysingProgressBar = new System.Windows.Forms.ProgressBar();
            this.HandRecordFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.AnalysisCalculationBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.OptionsButton = new System.Windows.Forms.Button();
            this.ResultsViewerButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ScoringDatabaseLabel
            // 
            resources.ApplyResources(this.ScoringDatabaseLabel, "ScoringDatabaseLabel");
            this.ScoringDatabaseLabel.Name = "ScoringDatabaseLabel";
            // 
            // PathToDBLabel
            // 
            resources.ApplyResources(this.PathToDBLabel, "PathToDBLabel");
            this.PathToDBLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.PathToDBLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PathToDBLabel.Name = "PathToDBLabel";
            // 
            // AddHandRecordFileButton
            // 
            resources.ApplyResources(this.AddHandRecordFileButton, "AddHandRecordFileButton");
            this.AddHandRecordFileButton.Name = "AddHandRecordFileButton";
            this.AddHandRecordFileButton.TabStop = false;
            this.AddHandRecordFileButton.UseVisualStyleBackColor = true;
            this.AddHandRecordFileButton.Click += new System.EventHandler(this.AddHandRecordFileButton_Click);
            // 
            // HandRecordFileLabel
            // 
            resources.ApplyResources(this.HandRecordFileLabel, "HandRecordFileLabel");
            this.HandRecordFileLabel.Name = "HandRecordFileLabel";
            // 
            // PathToHandRecordFileLabel
            // 
            resources.ApplyResources(this.PathToHandRecordFileLabel, "PathToHandRecordFileLabel");
            this.PathToHandRecordFileLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.PathToHandRecordFileLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PathToHandRecordFileLabel.Name = "PathToHandRecordFileLabel";
            // 
            // AddDatabaseFileButton
            // 
            resources.ApplyResources(this.AddDatabaseFileButton, "AddDatabaseFileButton");
            this.AddDatabaseFileButton.Name = "AddDatabaseFileButton";
            this.AddDatabaseFileButton.TabStop = false;
            this.AddDatabaseFileButton.UseVisualStyleBackColor = true;
            this.AddDatabaseFileButton.Click += new System.EventHandler(this.AddDatabaseFileButton_Click);
            // 
            // DatabaseFileDialog
            // 
            resources.ApplyResources(this.DatabaseFileDialog, "DatabaseFileDialog");
            // 
            // SessionStatusLabel
            // 
            resources.ApplyResources(this.SessionStatusLabel, "SessionStatusLabel");
            this.SessionStatusLabel.ForeColor = System.Drawing.Color.Red;
            this.SessionStatusLabel.Name = "SessionStatusLabel";
            // 
            // AnalysingLabel
            // 
            resources.ApplyResources(this.AnalysingLabel, "AnalysingLabel");
            this.AnalysingLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.AnalysingLabel.Name = "AnalysingLabel";
            // 
            // AnalysingProgressBar
            // 
            resources.ApplyResources(this.AnalysingProgressBar, "AnalysingProgressBar");
            this.AnalysingProgressBar.ForeColor = System.Drawing.Color.Blue;
            this.AnalysingProgressBar.Name = "AnalysingProgressBar";
            this.AnalysingProgressBar.Step = 1;
            // 
            // HandRecordFileDialog
            // 
            resources.ApplyResources(this.HandRecordFileDialog, "HandRecordFileDialog");
            // 
            // AnalysisCalculationBackgroundWorker
            // 
            this.AnalysisCalculationBackgroundWorker.WorkerReportsProgress = true;
            this.AnalysisCalculationBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.AnalysisCalculation_DoWork);
            this.AnalysisCalculationBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.AnalysisCalculation_ProgressChanged);
            this.AnalysisCalculationBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.AnalysisCalculation_RunWorkerCompleted);
            // 
            // OptionsButton
            // 
            resources.ApplyResources(this.OptionsButton, "OptionsButton");
            this.OptionsButton.Name = "OptionsButton";
            this.OptionsButton.TabStop = false;
            this.OptionsButton.UseVisualStyleBackColor = true;
            this.OptionsButton.Click += new System.EventHandler(this.OptionsButton_Click);
            // 
            // ResultsViewerButton
            // 
            resources.ApplyResources(this.ResultsViewerButton, "ResultsViewerButton");
            this.ResultsViewerButton.Name = "ResultsViewerButton";
            this.ResultsViewerButton.TabStop = false;
            this.ResultsViewerButton.UseVisualStyleBackColor = true;
            this.ResultsViewerButton.Click += new System.EventHandler(this.ResultsViewerButton_Click);
            // 
            // TabScoreForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.ResultsViewerButton);
            this.Controls.Add(this.OptionsButton);
            this.Controls.Add(this.AnalysingProgressBar);
            this.Controls.Add(this.AnalysingLabel);
            this.Controls.Add(this.SessionStatusLabel);
            this.Controls.Add(this.AddDatabaseFileButton);
            this.Controls.Add(this.PathToHandRecordFileLabel);
            this.Controls.Add(this.HandRecordFileLabel);
            this.Controls.Add(this.AddHandRecordFileButton);
            this.Controls.Add(this.PathToDBLabel);
            this.Controls.Add(this.ScoringDatabaseLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "TabScoreForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TabScoreForm_FormClosed);
            this.Load += new System.EventHandler(this.TabScoreForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ScoringDatabaseLabel;
        private System.Windows.Forms.Label PathToDBLabel;
        private System.Windows.Forms.Button AddHandRecordFileButton;
        private System.Windows.Forms.Label HandRecordFileLabel;
        private System.Windows.Forms.Label PathToHandRecordFileLabel;
        private System.Windows.Forms.Button AddDatabaseFileButton;
        private System.Windows.Forms.OpenFileDialog DatabaseFileDialog;
        private System.Windows.Forms.Label SessionStatusLabel;
        private System.Windows.Forms.Label AnalysingLabel;
        private System.Windows.Forms.ProgressBar AnalysingProgressBar;
        private System.Windows.Forms.OpenFileDialog HandRecordFileDialog;
        private System.ComponentModel.BackgroundWorker AnalysisCalculationBackgroundWorker;
        private System.Windows.Forms.Button OptionsButton;
        private System.Windows.Forms.Button ResultsViewerButton;
    }
}