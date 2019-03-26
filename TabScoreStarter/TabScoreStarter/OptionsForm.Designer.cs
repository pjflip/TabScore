namespace TabScoreStarter
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.gbTraveller = new System.Windows.Forms.GroupBox();
            this.cbShowPercentage = new System.Windows.Forms.CheckBox();
            this.cbShowHandRecord = new System.Windows.Forms.CheckBox();
            this.cbShowTraveller = new System.Windows.Forms.CheckBox();
            this.gbPlayers = new System.Windows.Forms.GroupBox();
            this.cbNameSource = new System.Windows.Forms.ComboBox();
            this.cbNumberEntryEachRound = new System.Windows.Forms.CheckBox();
            this.gbRankingList = new System.Windows.Forms.GroupBox();
            this.cbShowRanking = new System.Windows.Forms.ComboBox();
            this.gbLeadCard = new System.Windows.Forms.GroupBox();
            this.cbValidateLeadCard = new System.Windows.Forms.CheckBox();
            this.cbEnterLeadCard = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbEnterResultsMethod = new System.Windows.Forms.GroupBox();
            this.cbEnterResultsMethod = new System.Windows.Forms.ComboBox();
            this.gbTraveller.SuspendLayout();
            this.gbPlayers.SuspendLayout();
            this.gbRankingList.SuspendLayout();
            this.gbLeadCard.SuspendLayout();
            this.gbEnterResultsMethod.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbTraveller
            // 
            this.gbTraveller.Controls.Add(this.cbShowPercentage);
            this.gbTraveller.Controls.Add(this.cbShowHandRecord);
            this.gbTraveller.Controls.Add(this.cbShowTraveller);
            this.gbTraveller.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbTraveller.Location = new System.Drawing.Point(12, 74);
            this.gbTraveller.Name = "gbTraveller";
            this.gbTraveller.Size = new System.Drawing.Size(220, 99);
            this.gbTraveller.TabIndex = 0;
            this.gbTraveller.TabStop = false;
            this.gbTraveller.Text = "Traveller";
            // 
            // cbShowPercentage
            // 
            this.cbShowPercentage.AutoSize = true;
            this.cbShowPercentage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbShowPercentage.Location = new System.Drawing.Point(6, 46);
            this.cbShowPercentage.Name = "cbShowPercentage";
            this.cbShowPercentage.Size = new System.Drawing.Size(190, 19);
            this.cbShowPercentage.TabIndex = 3;
            this.cbShowPercentage.TabStop = false;
            this.cbShowPercentage.Text = "Show Percentage on Traveller";
            this.cbShowPercentage.UseVisualStyleBackColor = true;
            // 
            // cbShowHandRecord
            // 
            this.cbShowHandRecord.AutoSize = true;
            this.cbShowHandRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbShowHandRecord.Location = new System.Drawing.Point(6, 71);
            this.cbShowHandRecord.Name = "cbShowHandRecord";
            this.cbShowHandRecord.Size = new System.Drawing.Size(133, 19);
            this.cbShowHandRecord.TabIndex = 4;
            this.cbShowHandRecord.TabStop = false;
            this.cbShowHandRecord.Text = "Show Hand Record";
            this.cbShowHandRecord.UseVisualStyleBackColor = true;
            // 
            // cbShowTraveller
            // 
            this.cbShowTraveller.AutoSize = true;
            this.cbShowTraveller.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbShowTraveller.Location = new System.Drawing.Point(6, 21);
            this.cbShowTraveller.Name = "cbShowTraveller";
            this.cbShowTraveller.Size = new System.Drawing.Size(107, 19);
            this.cbShowTraveller.TabIndex = 2;
            this.cbShowTraveller.TabStop = false;
            this.cbShowTraveller.Text = "Show Traveller";
            this.cbShowTraveller.UseVisualStyleBackColor = true;
            this.cbShowTraveller.CheckedChanged += new System.EventHandler(this.cbShowTraveller_CheckedChanged);
            // 
            // gbPlayers
            // 
            this.gbPlayers.Controls.Add(this.cbNameSource);
            this.gbPlayers.Controls.Add(this.cbNumberEntryEachRound);
            this.gbPlayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbPlayers.Location = new System.Drawing.Point(238, 91);
            this.gbPlayers.Name = "gbPlayers";
            this.gbPlayers.Size = new System.Drawing.Size(251, 82);
            this.gbPlayers.TabIndex = 0;
            this.gbPlayers.TabStop = false;
            this.gbPlayers.Text = "Player Names/Numbers";
            // 
            // cbNameSource
            // 
            this.cbNameSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbNameSource.FormattingEnabled = true;
            this.cbNameSource.Items.AddRange(new object[] {
            "Internal database",
            "External database",
            "No name source",
            "Internal database first, then external"});
            this.cbNameSource.Location = new System.Drawing.Point(6, 20);
            this.cbNameSource.Name = "cbNameSource";
            this.cbNameSource.Size = new System.Drawing.Size(239, 23);
            this.cbNameSource.TabIndex = 8;
            this.cbNameSource.TabStop = false;
            // 
            // cbNumberEntryEachRound
            // 
            this.cbNumberEntryEachRound.AutoSize = true;
            this.cbNumberEntryEachRound.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbNumberEntryEachRound.Location = new System.Drawing.Point(6, 50);
            this.cbNumberEntryEachRound.Name = "cbNumberEntryEachRound";
            this.cbNumberEntryEachRound.Size = new System.Drawing.Size(209, 19);
            this.cbNumberEntryEachRound.TabIndex = 9;
            this.cbNumberEntryEachRound.TabStop = false;
            this.cbNumberEntryEachRound.Text = "Player Number Entry Each Round";
            this.cbNumberEntryEachRound.UseVisualStyleBackColor = true;
            // 
            // gbRankingList
            // 
            this.gbRankingList.Controls.Add(this.cbShowRanking);
            this.gbRankingList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbRankingList.Location = new System.Drawing.Point(12, 179);
            this.gbRankingList.Name = "gbRankingList";
            this.gbRankingList.Size = new System.Drawing.Size(220, 56);
            this.gbRankingList.TabIndex = 0;
            this.gbRankingList.TabStop = false;
            this.gbRankingList.Text = "Ranking List";
            // 
            // cbShowRanking
            // 
            this.cbShowRanking.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbShowRanking.FormattingEnabled = true;
            this.cbShowRanking.Items.AddRange(new object[] {
            "Don\'t show",
            "Show after each round",
            "Show at end of session"});
            this.cbShowRanking.Location = new System.Drawing.Point(7, 22);
            this.cbShowRanking.Name = "cbShowRanking";
            this.cbShowRanking.Size = new System.Drawing.Size(207, 23);
            this.cbShowRanking.TabIndex = 5;
            this.cbShowRanking.TabStop = false;
            // 
            // gbLeadCard
            // 
            this.gbLeadCard.Controls.Add(this.cbValidateLeadCard);
            this.gbLeadCard.Controls.Add(this.cbEnterLeadCard);
            this.gbLeadCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbLeadCard.Location = new System.Drawing.Point(238, 12);
            this.gbLeadCard.Name = "gbLeadCard";
            this.gbLeadCard.Size = new System.Drawing.Size(251, 73);
            this.gbLeadCard.TabIndex = 0;
            this.gbLeadCard.TabStop = false;
            this.gbLeadCard.Text = "Lead Card";
            // 
            // cbValidateLeadCard
            // 
            this.cbValidateLeadCard.AutoSize = true;
            this.cbValidateLeadCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbValidateLeadCard.Location = new System.Drawing.Point(6, 46);
            this.cbValidateLeadCard.Name = "cbValidateLeadCard";
            this.cbValidateLeadCard.Size = new System.Drawing.Size(130, 19);
            this.cbValidateLeadCard.TabIndex = 7;
            this.cbValidateLeadCard.TabStop = false;
            this.cbValidateLeadCard.Text = "Validate Lead Card";
            this.cbValidateLeadCard.UseVisualStyleBackColor = true;
            // 
            // cbEnterLeadCard
            // 
            this.cbEnterLeadCard.AutoSize = true;
            this.cbEnterLeadCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbEnterLeadCard.Location = new System.Drawing.Point(6, 21);
            this.cbEnterLeadCard.Name = "cbEnterLeadCard";
            this.cbEnterLeadCard.Size = new System.Drawing.Size(115, 19);
            this.cbEnterLeadCard.TabIndex = 6;
            this.cbEnterLeadCard.TabStop = false;
            this.cbEnterLeadCard.Text = "Enter Lead Card";
            this.cbEnterLeadCard.UseVisualStyleBackColor = true;
            this.cbEnterLeadCard.CheckedChanged += new System.EventHandler(this.cbEnterLeadCard_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(284, 212);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(408, 212);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 11;
            this.btnOK.TabStop = false;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbEnterResultsMethod
            // 
            this.gbEnterResultsMethod.Controls.Add(this.cbEnterResultsMethod);
            this.gbEnterResultsMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbEnterResultsMethod.Location = new System.Drawing.Point(12, 12);
            this.gbEnterResultsMethod.Name = "gbEnterResultsMethod";
            this.gbEnterResultsMethod.Size = new System.Drawing.Size(220, 56);
            this.gbEnterResultsMethod.TabIndex = 5;
            this.gbEnterResultsMethod.TabStop = false;
            this.gbEnterResultsMethod.Text = "Enter Results Method";
            // 
            // cbEnterResultsMethod
            // 
            this.cbEnterResultsMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbEnterResultsMethod.FormattingEnabled = true;
            this.cbEnterResultsMethod.Items.AddRange(new object[] {
            "Tricks plus or minus",
            "Total tricks won"});
            this.cbEnterResultsMethod.Location = new System.Drawing.Point(7, 22);
            this.cbEnterResultsMethod.Name = "cbEnterResultsMethod";
            this.cbEnterResultsMethod.Size = new System.Drawing.Size(207, 23);
            this.cbEnterResultsMethod.TabIndex = 1;
            this.cbEnterResultsMethod.TabStop = false;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 249);
            this.Controls.Add(this.gbEnterResultsMethod);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gbPlayers);
            this.Controls.Add(this.gbRankingList);
            this.Controls.Add(this.gbLeadCard);
            this.Controls.Add(this.gbTraveller);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.Text = "TabScore Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.gbTraveller.ResumeLayout(false);
            this.gbTraveller.PerformLayout();
            this.gbPlayers.ResumeLayout(false);
            this.gbPlayers.PerformLayout();
            this.gbRankingList.ResumeLayout(false);
            this.gbLeadCard.ResumeLayout(false);
            this.gbLeadCard.PerformLayout();
            this.gbEnterResultsMethod.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbTraveller;
        private System.Windows.Forms.CheckBox cbShowPercentage;
        private System.Windows.Forms.CheckBox cbShowHandRecord;
        private System.Windows.Forms.CheckBox cbShowTraveller;
        private System.Windows.Forms.GroupBox gbPlayers;
        private System.Windows.Forms.ComboBox cbNameSource;
        private System.Windows.Forms.CheckBox cbNumberEntryEachRound;
        private System.Windows.Forms.GroupBox gbRankingList;
        private System.Windows.Forms.ComboBox cbShowRanking;
        private System.Windows.Forms.GroupBox gbLeadCard;
        private System.Windows.Forms.CheckBox cbValidateLeadCard;
        private System.Windows.Forms.CheckBox cbEnterLeadCard;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox gbEnterResultsMethod;
        private System.Windows.Forms.ComboBox cbEnterResultsMethod;
    }
}