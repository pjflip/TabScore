namespace TabScoreStarter
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
            this.lblScoringDatabase = new System.Windows.Forms.Label();
            this.lblPathToDB = new System.Windows.Forms.Label();
            this.btnAddHandRecordFile = new System.Windows.Forms.Button();
            this.lblHandRecordFile = new System.Windows.Forms.Label();
            this.lblPathToHandRecordFile = new System.Windows.Forms.Label();
            this.btnAddSDBFile = new System.Windows.Forms.Button();
            this.fdSDBFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.lblSessionStatus = new System.Windows.Forms.Label();
            this.lblDDAnalysing = new System.Windows.Forms.Label();
            this.pbDDAnalysing = new System.Windows.Forms.ProgressBar();
            this.fdHRFFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.bwAnalysisCalculation = new System.ComponentModel.BackgroundWorker();
            this.btnOptions = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblScoringDatabase
            // 
            this.lblScoringDatabase.AutoSize = true;
            this.lblScoringDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScoringDatabase.Location = new System.Drawing.Point(15, 17);
            this.lblScoringDatabase.Name = "lblScoringDatabase";
            this.lblScoringDatabase.Size = new System.Drawing.Size(137, 16);
            this.lblScoringDatabase.TabIndex = 0;
            this.lblScoringDatabase.Text = "Scoring Database:";
            // 
            // lblPathToDB
            // 
            this.lblPathToDB.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblPathToDB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPathToDB.Location = new System.Drawing.Point(16, 39);
            this.lblPathToDB.Name = "lblPathToDB";
            this.lblPathToDB.Size = new System.Drawing.Size(477, 21);
            this.lblPathToDB.TabIndex = 0;
            this.lblPathToDB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAddHandRecordFile
            // 
            this.btnAddHandRecordFile.Location = new System.Drawing.Point(302, 90);
            this.btnAddHandRecordFile.Name = "btnAddHandRecordFile";
            this.btnAddHandRecordFile.Size = new System.Drawing.Size(191, 23);
            this.btnAddHandRecordFile.TabIndex = 2;
            this.btnAddHandRecordFile.Text = "Add hand record file...";
            this.btnAddHandRecordFile.UseVisualStyleBackColor = true;
            this.btnAddHandRecordFile.Visible = false;
            this.btnAddHandRecordFile.Click += new System.EventHandler(this.btnAddHandRecordFile_Click);
            // 
            // lblHandRecordFile
            // 
            this.lblHandRecordFile.AutoSize = true;
            this.lblHandRecordFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHandRecordFile.Location = new System.Drawing.Point(15, 93);
            this.lblHandRecordFile.Name = "lblHandRecordFile";
            this.lblHandRecordFile.Size = new System.Drawing.Size(134, 16);
            this.lblHandRecordFile.TabIndex = 0;
            this.lblHandRecordFile.Text = "Hand Record File:";
            // 
            // lblPathToHandRecordFile
            // 
            this.lblPathToHandRecordFile.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblPathToHandRecordFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPathToHandRecordFile.Location = new System.Drawing.Point(18, 114);
            this.lblPathToHandRecordFile.Name = "lblPathToHandRecordFile";
            this.lblPathToHandRecordFile.Size = new System.Drawing.Size(475, 21);
            this.lblPathToHandRecordFile.TabIndex = 0;
            this.lblPathToHandRecordFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAddSDBFile
            // 
            this.btnAddSDBFile.Location = new System.Drawing.Point(302, 14);
            this.btnAddSDBFile.Name = "btnAddSDBFile";
            this.btnAddSDBFile.Size = new System.Drawing.Size(191, 23);
            this.btnAddSDBFile.TabIndex = 1;
            this.btnAddSDBFile.Text = "Add scoring database file...";
            this.btnAddSDBFile.UseVisualStyleBackColor = true;
            this.btnAddSDBFile.Visible = false;
            this.btnAddSDBFile.Click += new System.EventHandler(this.btnAddSDBFile_Click);
            // 
            // fdSDBFileDialog
            // 
            this.fdSDBFileDialog.Filter = "BWS Files (*.bws)|*.bws";
            this.fdSDBFileDialog.Title = "Select Scoring Database";
            // 
            // lblSessionStatus
            // 
            this.lblSessionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSessionStatus.ForeColor = System.Drawing.Color.Red;
            this.lblSessionStatus.Location = new System.Drawing.Point(150, 194);
            this.lblSessionStatus.Name = "lblSessionStatus";
            this.lblSessionStatus.Size = new System.Drawing.Size(203, 23);
            this.lblSessionStatus.TabIndex = 0;
            this.lblSessionStatus.Text = "Session Not Started";
            this.lblSessionStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDDAnalysing
            // 
            this.lblDDAnalysing.AutoSize = true;
            this.lblDDAnalysing.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDDAnalysing.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDDAnalysing.Location = new System.Drawing.Point(15, 151);
            this.lblDDAnalysing.Name = "lblDDAnalysing";
            this.lblDDAnalysing.Size = new System.Drawing.Size(120, 16);
            this.lblDDAnalysing.TabIndex = 0;
            this.lblDDAnalysing.Text = "Analysis Complete";
            this.lblDDAnalysing.Visible = false;
            // 
            // pbDDAnalysing
            // 
            this.pbDDAnalysing.ForeColor = System.Drawing.Color.Blue;
            this.pbDDAnalysing.Location = new System.Drawing.Point(141, 147);
            this.pbDDAnalysing.Name = "pbDDAnalysing";
            this.pbDDAnalysing.Size = new System.Drawing.Size(352, 23);
            this.pbDDAnalysing.Step = 1;
            this.pbDDAnalysing.TabIndex = 0;
            this.pbDDAnalysing.Visible = false;
            // 
            // fdHRFFileDialog
            // 
            this.fdHRFFileDialog.Filter = "PBN Files (*.pbn)|*.pbn";
            this.fdHRFFileDialog.Title = "Select Hand Record File";
            // 
            // bwAnalysisCalculation
            // 
            this.bwAnalysisCalculation.WorkerReportsProgress = true;
            this.bwAnalysisCalculation.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwAnalysisCalculation_DoWork);
            this.bwAnalysisCalculation.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwAnalysisCalculation_ProgressChanged);
            this.bwAnalysisCalculation.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwAnalysisCalculation_RunWorkerCompleted);
            // 
            // btnOptions
            // 
            this.btnOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOptions.Location = new System.Drawing.Point(12, 189);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(84, 34);
            this.btnOptions.TabIndex = 3;
            this.btnOptions.Text = "Options";
            this.btnOptions.UseVisualStyleBackColor = true;
            this.btnOptions.Visible = false;
            this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
            // 
            // TabScoreForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(505, 234);
            this.Controls.Add(this.btnOptions);
            this.Controls.Add(this.pbDDAnalysing);
            this.Controls.Add(this.lblDDAnalysing);
            this.Controls.Add(this.lblSessionStatus);
            this.Controls.Add(this.btnAddSDBFile);
            this.Controls.Add(this.lblPathToHandRecordFile);
            this.Controls.Add(this.lblHandRecordFile);
            this.Controls.Add(this.btnAddHandRecordFile);
            this.Controls.Add(this.lblPathToDB);
            this.Controls.Add(this.lblScoringDatabase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TabScoreForm";
            this.Text = "TabScoreStarter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TabScoreForm_FormClosed);
            this.Load += new System.EventHandler(this.TabScoreForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblScoringDatabase;
        private System.Windows.Forms.Label lblPathToDB;
        private System.Windows.Forms.Button btnAddHandRecordFile;
        private System.Windows.Forms.Label lblHandRecordFile;
        private System.Windows.Forms.Label lblPathToHandRecordFile;
        private System.Windows.Forms.Button btnAddSDBFile;
        private System.Windows.Forms.OpenFileDialog fdSDBFileDialog;
        private System.Windows.Forms.Label lblSessionStatus;
        private System.Windows.Forms.Label lblDDAnalysing;
        private System.Windows.Forms.ProgressBar pbDDAnalysing;
        private System.Windows.Forms.OpenFileDialog fdHRFFileDialog;
        private System.ComponentModel.BackgroundWorker bwAnalysisCalculation;
        private System.Windows.Forms.Button btnOptions;
    }
}