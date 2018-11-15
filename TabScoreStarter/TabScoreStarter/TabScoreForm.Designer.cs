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
            this.lblDBName = new System.Windows.Forms.Label();
            this.btnHRF = new System.Windows.Forms.Button();
            this.lblHandRecordFile = new System.Windows.Forms.Label();
            this.lblHandRecord = new System.Windows.Forms.Label();
            this.btnSDB = new System.Windows.Forms.Button();
            this.fd = new System.Windows.Forms.OpenFileDialog();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblServerState = new System.Windows.Forms.Label();
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
            // lblDBName
            // 
            this.lblDBName.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblDBName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDBName.Location = new System.Drawing.Point(16, 39);
            this.lblDBName.Name = "lblDBName";
            this.lblDBName.Size = new System.Drawing.Size(477, 21);
            this.lblDBName.TabIndex = 1;
            this.lblDBName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnHRF
            // 
            this.btnHRF.Enabled = false;
            this.btnHRF.Location = new System.Drawing.Point(302, 90);
            this.btnHRF.Name = "btnHRF";
            this.btnHRF.Size = new System.Drawing.Size(191, 23);
            this.btnHRF.TabIndex = 2;
            this.btnHRF.Text = "Add/modify hand record file...";
            this.btnHRF.UseVisualStyleBackColor = true;
            this.btnHRF.Click += new System.EventHandler(this.btnHRF_Click);
            // 
            // lblHandRecordFile
            // 
            this.lblHandRecordFile.AutoSize = true;
            this.lblHandRecordFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHandRecordFile.Location = new System.Drawing.Point(15, 93);
            this.lblHandRecordFile.Name = "lblHandRecordFile";
            this.lblHandRecordFile.Size = new System.Drawing.Size(134, 16);
            this.lblHandRecordFile.TabIndex = 3;
            this.lblHandRecordFile.Text = "Hand Record File:";
            // 
            // lblHandRecord
            // 
            this.lblHandRecord.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblHandRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblHandRecord.Location = new System.Drawing.Point(18, 114);
            this.lblHandRecord.Name = "lblHandRecord";
            this.lblHandRecord.Size = new System.Drawing.Size(475, 21);
            this.lblHandRecord.TabIndex = 4;
            this.lblHandRecord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSDB
            // 
            this.btnSDB.Location = new System.Drawing.Point(302, 14);
            this.btnSDB.Name = "btnSDB";
            this.btnSDB.Size = new System.Drawing.Size(191, 23);
            this.btnSDB.TabIndex = 7;
            this.btnSDB.Text = "Add/modify scoring database file...";
            this.btnSDB.UseVisualStyleBackColor = true;
            this.btnSDB.Click += new System.EventHandler(this.btnSDB_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Gray;
            this.btnStart.Enabled = false;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(16, 151);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(102, 42);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblServerState
            // 
            this.lblServerState.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerState.ForeColor = System.Drawing.Color.Red;
            this.lblServerState.Location = new System.Drawing.Point(152, 162);
            this.lblServerState.Name = "lblServerState";
            this.lblServerState.Size = new System.Drawing.Size(203, 23);
            this.lblServerState.TabIndex = 10;
            this.lblServerState.Text = "Session Not Started";
            this.lblServerState.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // TabScoreForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(505, 205);
            this.Controls.Add(this.lblServerState);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnSDB);
            this.Controls.Add(this.lblHandRecord);
            this.Controls.Add(this.lblHandRecordFile);
            this.Controls.Add(this.btnHRF);
            this.Controls.Add(this.lblDBName);
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
        private System.Windows.Forms.Label lblDBName;
        private System.Windows.Forms.Button btnHRF;
        private System.Windows.Forms.Label lblHandRecordFile;
        private System.Windows.Forms.Label lblHandRecord;
        private System.Windows.Forms.Button btnSDB;
        private System.Windows.Forms.OpenFileDialog fd;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblServerState;
    }
}