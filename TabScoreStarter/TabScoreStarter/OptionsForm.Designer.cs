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
            this.TravellerGroup = new System.Windows.Forms.GroupBox();
            this.ShowPercentageCheckbox = new System.Windows.Forms.CheckBox();
            this.ShowHandRecordCheckbox = new System.Windows.Forms.CheckBox();
            this.ShowTravellerCheckbox = new System.Windows.Forms.CheckBox();
            this.PlayersGroup = new System.Windows.Forms.GroupBox();
            this.NameSourceCombobox = new System.Windows.Forms.ComboBox();
            this.NumberEntryEachRoundCheckbox = new System.Windows.Forms.CheckBox();
            this.RankingListGroup = new System.Windows.Forms.GroupBox();
            this.ShowRankingCombobox = new System.Windows.Forms.ComboBox();
            this.LeadCardGroup = new System.Windows.Forms.GroupBox();
            this.ValidateLeadCardCheckbox = new System.Windows.Forms.CheckBox();
            this.EnterLeadCardCheckbox = new System.Windows.Forms.CheckBox();
            this.CanxButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.EnterResultsMethodGroup = new System.Windows.Forms.GroupBox();
            this.EnterResultsMethodCombobox = new System.Windows.Forms.ComboBox();
            this.TabletMovesGroupBox = new System.Windows.Forms.GroupBox();
            this.TabletModePersonalRadioButton = new System.Windows.Forms.RadioButton();
            this.TabletModeTraditionalRadioButton = new System.Windows.Forms.RadioButton();
            this.RoundTimerGroupBox = new System.Windows.Forms.GroupBox();
            this.AdditionalMinutesPerRoundLabel = new System.Windows.Forms.Label();
            this.AdditionalMinutesPerRoundNud = new System.Windows.Forms.NumericUpDown();
            this.MinutesPerBoardLabel = new System.Windows.Forms.Label();
            this.MinutesPerBoardNud = new System.Windows.Forms.NumericUpDown();
            this.ShowTimerCheckbox = new System.Windows.Forms.CheckBox();
            this.TravellerGroup.SuspendLayout();
            this.PlayersGroup.SuspendLayout();
            this.RankingListGroup.SuspendLayout();
            this.LeadCardGroup.SuspendLayout();
            this.EnterResultsMethodGroup.SuspendLayout();
            this.TabletMovesGroupBox.SuspendLayout();
            this.RoundTimerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AdditionalMinutesPerRoundNud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinutesPerBoardNud)).BeginInit();
            this.SuspendLayout();
            // 
            // TravellerGroup
            // 
            this.TravellerGroup.Controls.Add(this.ShowPercentageCheckbox);
            this.TravellerGroup.Controls.Add(this.ShowHandRecordCheckbox);
            this.TravellerGroup.Controls.Add(this.ShowTravellerCheckbox);
            this.TravellerGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TravellerGroup.Location = new System.Drawing.Point(12, 74);
            this.TravellerGroup.Name = "TravellerGroup";
            this.TravellerGroup.Size = new System.Drawing.Size(220, 99);
            this.TravellerGroup.TabIndex = 0;
            this.TravellerGroup.TabStop = false;
            this.TravellerGroup.Text = "Traveller";
            // 
            // ShowPercentageCheckbox
            // 
            this.ShowPercentageCheckbox.AutoSize = true;
            this.ShowPercentageCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowPercentageCheckbox.Location = new System.Drawing.Point(6, 46);
            this.ShowPercentageCheckbox.Name = "ShowPercentageCheckbox";
            this.ShowPercentageCheckbox.Size = new System.Drawing.Size(190, 19);
            this.ShowPercentageCheckbox.TabIndex = 21;
            this.ShowPercentageCheckbox.Text = "Show Percentage on Traveller";
            this.ShowPercentageCheckbox.UseVisualStyleBackColor = true;
            // 
            // ShowHandRecordCheckbox
            // 
            this.ShowHandRecordCheckbox.AutoSize = true;
            this.ShowHandRecordCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowHandRecordCheckbox.Location = new System.Drawing.Point(6, 71);
            this.ShowHandRecordCheckbox.Name = "ShowHandRecordCheckbox";
            this.ShowHandRecordCheckbox.Size = new System.Drawing.Size(133, 19);
            this.ShowHandRecordCheckbox.TabIndex = 22;
            this.ShowHandRecordCheckbox.Text = "Show Hand Record";
            this.ShowHandRecordCheckbox.UseVisualStyleBackColor = true;
            // 
            // ShowTravellerCheckbox
            // 
            this.ShowTravellerCheckbox.AutoSize = true;
            this.ShowTravellerCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowTravellerCheckbox.Location = new System.Drawing.Point(6, 21);
            this.ShowTravellerCheckbox.Name = "ShowTravellerCheckbox";
            this.ShowTravellerCheckbox.Size = new System.Drawing.Size(107, 19);
            this.ShowTravellerCheckbox.TabIndex = 20;
            this.ShowTravellerCheckbox.Text = "Show Traveller";
            this.ShowTravellerCheckbox.UseVisualStyleBackColor = true;
            this.ShowTravellerCheckbox.CheckedChanged += new System.EventHandler(this.ShowTraveller_CheckedChanged);
            // 
            // PlayersGroup
            // 
            this.PlayersGroup.Controls.Add(this.NameSourceCombobox);
            this.PlayersGroup.Controls.Add(this.NumberEntryEachRoundCheckbox);
            this.PlayersGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayersGroup.Location = new System.Drawing.Point(244, 12);
            this.PlayersGroup.Name = "PlayersGroup";
            this.PlayersGroup.Size = new System.Drawing.Size(251, 81);
            this.PlayersGroup.TabIndex = 0;
            this.PlayersGroup.TabStop = false;
            this.PlayersGroup.Text = "Player Names/Numbers";
            // 
            // NameSourceCombobox
            // 
            this.NameSourceCombobox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameSourceCombobox.FormattingEnabled = true;
            this.NameSourceCombobox.Items.AddRange(new object[] {
            "Internal database",
            "External database",
            "No name source",
            "Internal database first, then external"});
            this.NameSourceCombobox.Location = new System.Drawing.Point(6, 20);
            this.NameSourceCombobox.Name = "NameSourceCombobox";
            this.NameSourceCombobox.Size = new System.Drawing.Size(239, 23);
            this.NameSourceCombobox.TabIndex = 50;
            // 
            // NumberEntryEachRoundCheckbox
            // 
            this.NumberEntryEachRoundCheckbox.AutoSize = true;
            this.NumberEntryEachRoundCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NumberEntryEachRoundCheckbox.Location = new System.Drawing.Point(6, 50);
            this.NumberEntryEachRoundCheckbox.Name = "NumberEntryEachRoundCheckbox";
            this.NumberEntryEachRoundCheckbox.Size = new System.Drawing.Size(209, 19);
            this.NumberEntryEachRoundCheckbox.TabIndex = 51;
            this.NumberEntryEachRoundCheckbox.Text = "Player Number Entry Each Round";
            this.NumberEntryEachRoundCheckbox.UseVisualStyleBackColor = true;
            // 
            // RankingListGroup
            // 
            this.RankingListGroup.Controls.Add(this.ShowRankingCombobox);
            this.RankingListGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RankingListGroup.Location = new System.Drawing.Point(12, 261);
            this.RankingListGroup.Name = "RankingListGroup";
            this.RankingListGroup.Size = new System.Drawing.Size(220, 56);
            this.RankingListGroup.TabIndex = 0;
            this.RankingListGroup.TabStop = false;
            this.RankingListGroup.Text = "Ranking List";
            // 
            // ShowRankingCombobox
            // 
            this.ShowRankingCombobox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowRankingCombobox.FormattingEnabled = true;
            this.ShowRankingCombobox.Items.AddRange(new object[] {
            "Don\'t show",
            "Show after each round",
            "Show at end of session"});
            this.ShowRankingCombobox.Location = new System.Drawing.Point(7, 22);
            this.ShowRankingCombobox.Name = "ShowRankingCombobox";
            this.ShowRankingCombobox.Size = new System.Drawing.Size(207, 23);
            this.ShowRankingCombobox.TabIndex = 40;
            // 
            // LeadCardGroup
            // 
            this.LeadCardGroup.Controls.Add(this.ValidateLeadCardCheckbox);
            this.LeadCardGroup.Controls.Add(this.EnterLeadCardCheckbox);
            this.LeadCardGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LeadCardGroup.Location = new System.Drawing.Point(12, 182);
            this.LeadCardGroup.Name = "LeadCardGroup";
            this.LeadCardGroup.Size = new System.Drawing.Size(220, 73);
            this.LeadCardGroup.TabIndex = 0;
            this.LeadCardGroup.TabStop = false;
            this.LeadCardGroup.Text = "Lead Card";
            // 
            // ValidateLeadCardCheckbox
            // 
            this.ValidateLeadCardCheckbox.AutoSize = true;
            this.ValidateLeadCardCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ValidateLeadCardCheckbox.Location = new System.Drawing.Point(6, 46);
            this.ValidateLeadCardCheckbox.Name = "ValidateLeadCardCheckbox";
            this.ValidateLeadCardCheckbox.Size = new System.Drawing.Size(130, 19);
            this.ValidateLeadCardCheckbox.TabIndex = 31;
            this.ValidateLeadCardCheckbox.Text = "Validate Lead Card";
            this.ValidateLeadCardCheckbox.UseVisualStyleBackColor = true;
            // 
            // EnterLeadCardCheckbox
            // 
            this.EnterLeadCardCheckbox.AutoSize = true;
            this.EnterLeadCardCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnterLeadCardCheckbox.Location = new System.Drawing.Point(6, 21);
            this.EnterLeadCardCheckbox.Name = "EnterLeadCardCheckbox";
            this.EnterLeadCardCheckbox.Size = new System.Drawing.Size(115, 19);
            this.EnterLeadCardCheckbox.TabIndex = 30;
            this.EnterLeadCardCheckbox.Text = "Enter Lead Card";
            this.EnterLeadCardCheckbox.UseVisualStyleBackColor = true;
            this.EnterLeadCardCheckbox.CheckedChanged += new System.EventHandler(this.EnterLeadCard_CheckedChanged);
            // 
            // CanxButton
            // 
            this.CanxButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CanxButton.Location = new System.Drawing.Point(420, 294);
            this.CanxButton.Name = "CanxButton";
            this.CanxButton.Size = new System.Drawing.Size(75, 23);
            this.CanxButton.TabIndex = 81;
            this.CanxButton.Text = "Cancel";
            this.CanxButton.UseVisualStyleBackColor = true;
            this.CanxButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OKButton.Location = new System.Drawing.Point(244, 294);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 80;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // EnterResultsMethodGroup
            // 
            this.EnterResultsMethodGroup.Controls.Add(this.EnterResultsMethodCombobox);
            this.EnterResultsMethodGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnterResultsMethodGroup.Location = new System.Drawing.Point(12, 12);
            this.EnterResultsMethodGroup.Name = "EnterResultsMethodGroup";
            this.EnterResultsMethodGroup.Size = new System.Drawing.Size(220, 56);
            this.EnterResultsMethodGroup.TabIndex = 0;
            this.EnterResultsMethodGroup.TabStop = false;
            this.EnterResultsMethodGroup.Text = "Enter Results Method";
            // 
            // EnterResultsMethodCombobox
            // 
            this.EnterResultsMethodCombobox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnterResultsMethodCombobox.FormattingEnabled = true;
            this.EnterResultsMethodCombobox.Items.AddRange(new object[] {
            "Tricks plus or minus",
            "Total tricks won"});
            this.EnterResultsMethodCombobox.Location = new System.Drawing.Point(7, 22);
            this.EnterResultsMethodCombobox.Name = "EnterResultsMethodCombobox";
            this.EnterResultsMethodCombobox.Size = new System.Drawing.Size(207, 23);
            this.EnterResultsMethodCombobox.TabIndex = 10;
            // 
            // TabletMovesGroupBox
            // 
            this.TabletMovesGroupBox.Controls.Add(this.TabletModePersonalRadioButton);
            this.TabletMovesGroupBox.Controls.Add(this.TabletModeTraditionalRadioButton);
            this.TabletMovesGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabletMovesGroupBox.Location = new System.Drawing.Point(244, 99);
            this.TabletMovesGroupBox.Name = "TabletMovesGroupBox";
            this.TabletMovesGroupBox.Size = new System.Drawing.Size(251, 73);
            this.TabletMovesGroupBox.TabIndex = 0;
            this.TabletMovesGroupBox.TabStop = false;
            this.TabletMovesGroupBox.Text = "Table-top Device Mode";
            // 
            // TabletModePersonalRadioButton
            // 
            this.TabletModePersonalRadioButton.AutoSize = true;
            this.TabletModePersonalRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabletModePersonalRadioButton.Location = new System.Drawing.Point(8, 45);
            this.TabletModePersonalRadioButton.Name = "TabletModePersonalRadioButton";
            this.TabletModePersonalRadioButton.Size = new System.Drawing.Size(220, 19);
            this.TabletModePersonalRadioButton.TabIndex = 61;
            this.TabletModePersonalRadioButton.TabStop = true;
            this.TabletModePersonalRadioButton.Text = "Personal - tablets move with players";
            this.TabletModePersonalRadioButton.UseVisualStyleBackColor = true;
            // 
            // TabletModeTraditionalRadioButton
            // 
            this.TabletModeTraditionalRadioButton.AutoSize = true;
            this.TabletModeTraditionalRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabletModeTraditionalRadioButton.Location = new System.Drawing.Point(8, 21);
            this.TabletModeTraditionalRadioButton.Name = "TabletModeTraditionalRadioButton";
            this.TabletModeTraditionalRadioButton.Size = new System.Drawing.Size(184, 19);
            this.TabletModeTraditionalRadioButton.TabIndex = 60;
            this.TabletModeTraditionalRadioButton.TabStop = true;
            this.TabletModeTraditionalRadioButton.Text = "Traditional - 1 tablet per table";
            this.TabletModeTraditionalRadioButton.UseVisualStyleBackColor = true;
            // 
            // RoundTimerGroupBox
            // 
            this.RoundTimerGroupBox.Controls.Add(this.AdditionalMinutesPerRoundLabel);
            this.RoundTimerGroupBox.Controls.Add(this.AdditionalMinutesPerRoundNud);
            this.RoundTimerGroupBox.Controls.Add(this.MinutesPerBoardLabel);
            this.RoundTimerGroupBox.Controls.Add(this.MinutesPerBoardNud);
            this.RoundTimerGroupBox.Controls.Add(this.ShowTimerCheckbox);
            this.RoundTimerGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RoundTimerGroupBox.Location = new System.Drawing.Point(244, 182);
            this.RoundTimerGroupBox.Name = "RoundTimerGroupBox";
            this.RoundTimerGroupBox.Size = new System.Drawing.Size(251, 104);
            this.RoundTimerGroupBox.TabIndex = 0;
            this.RoundTimerGroupBox.TabStop = false;
            this.RoundTimerGroupBox.Text = "Round Timer";
            // 
            // AdditionalMinutesPerRoundLabel
            // 
            this.AdditionalMinutesPerRoundLabel.AutoSize = true;
            this.AdditionalMinutesPerRoundLabel.Enabled = false;
            this.AdditionalMinutesPerRoundLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AdditionalMinutesPerRoundLabel.Location = new System.Drawing.Point(3, 74);
            this.AdditionalMinutesPerRoundLabel.Name = "AdditionalMinutesPerRoundLabel";
            this.AdditionalMinutesPerRoundLabel.Size = new System.Drawing.Size(170, 15);
            this.AdditionalMinutesPerRoundLabel.TabIndex = 0;
            this.AdditionalMinutesPerRoundLabel.Text = "Additional Minutes Per Round";
            this.AdditionalMinutesPerRoundLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AdditionalMinutesPerRoundNud
            // 
            this.AdditionalMinutesPerRoundNud.DecimalPlaces = 1;
            this.AdditionalMinutesPerRoundNud.Enabled = false;
            this.AdditionalMinutesPerRoundNud.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AdditionalMinutesPerRoundNud.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.AdditionalMinutesPerRoundNud.Location = new System.Drawing.Point(187, 71);
            this.AdditionalMinutesPerRoundNud.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.AdditionalMinutesPerRoundNud.Name = "AdditionalMinutesPerRoundNud";
            this.AdditionalMinutesPerRoundNud.Size = new System.Drawing.Size(58, 22);
            this.AdditionalMinutesPerRoundNud.TabIndex = 72;
            this.AdditionalMinutesPerRoundNud.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AdditionalMinutesPerRoundNud.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // MinutesPerBoardLabel
            // 
            this.MinutesPerBoardLabel.AutoSize = true;
            this.MinutesPerBoardLabel.Enabled = false;
            this.MinutesPerBoardLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinutesPerBoardLabel.Location = new System.Drawing.Point(3, 47);
            this.MinutesPerBoardLabel.Name = "MinutesPerBoardLabel";
            this.MinutesPerBoardLabel.Size = new System.Drawing.Size(109, 15);
            this.MinutesPerBoardLabel.TabIndex = 0;
            this.MinutesPerBoardLabel.Text = "Minutes Per Board";
            this.MinutesPerBoardLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MinutesPerBoardNud
            // 
            this.MinutesPerBoardNud.DecimalPlaces = 1;
            this.MinutesPerBoardNud.Enabled = false;
            this.MinutesPerBoardNud.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinutesPerBoardNud.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.MinutesPerBoardNud.Location = new System.Drawing.Point(187, 43);
            this.MinutesPerBoardNud.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.MinutesPerBoardNud.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.MinutesPerBoardNud.Name = "MinutesPerBoardNud";
            this.MinutesPerBoardNud.Size = new System.Drawing.Size(58, 22);
            this.MinutesPerBoardNud.TabIndex = 71;
            this.MinutesPerBoardNud.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MinutesPerBoardNud.Value = new decimal(new int[] {
            65,
            0,
            0,
            65536});
            // 
            // ShowTimerCheckbox
            // 
            this.ShowTimerCheckbox.AutoSize = true;
            this.ShowTimerCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowTimerCheckbox.Location = new System.Drawing.Point(6, 21);
            this.ShowTimerCheckbox.Name = "ShowTimerCheckbox";
            this.ShowTimerCheckbox.Size = new System.Drawing.Size(132, 19);
            this.ShowTimerCheckbox.TabIndex = 70;
            this.ShowTimerCheckbox.Text = "Show Round Timer";
            this.ShowTimerCheckbox.UseVisualStyleBackColor = true;
            this.ShowTimerCheckbox.CheckedChanged += new System.EventHandler(this.ShowTimerCheckbox_CheckedChanged);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 325);
            this.Controls.Add(this.RoundTimerGroupBox);
            this.Controls.Add(this.TabletMovesGroupBox);
            this.Controls.Add(this.EnterResultsMethodGroup);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.CanxButton);
            this.Controls.Add(this.PlayersGroup);
            this.Controls.Add(this.RankingListGroup);
            this.Controls.Add(this.LeadCardGroup);
            this.Controls.Add(this.TravellerGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(517, 332);
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TabScore Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.TravellerGroup.ResumeLayout(false);
            this.TravellerGroup.PerformLayout();
            this.PlayersGroup.ResumeLayout(false);
            this.PlayersGroup.PerformLayout();
            this.RankingListGroup.ResumeLayout(false);
            this.LeadCardGroup.ResumeLayout(false);
            this.LeadCardGroup.PerformLayout();
            this.EnterResultsMethodGroup.ResumeLayout(false);
            this.TabletMovesGroupBox.ResumeLayout(false);
            this.TabletMovesGroupBox.PerformLayout();
            this.RoundTimerGroupBox.ResumeLayout(false);
            this.RoundTimerGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AdditionalMinutesPerRoundNud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinutesPerBoardNud)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox TravellerGroup;
        private System.Windows.Forms.CheckBox ShowPercentageCheckbox;
        private System.Windows.Forms.CheckBox ShowHandRecordCheckbox;
        private System.Windows.Forms.CheckBox ShowTravellerCheckbox;
        private System.Windows.Forms.GroupBox PlayersGroup;
        private System.Windows.Forms.ComboBox NameSourceCombobox;
        private System.Windows.Forms.CheckBox NumberEntryEachRoundCheckbox;
        private System.Windows.Forms.GroupBox RankingListGroup;
        private System.Windows.Forms.ComboBox ShowRankingCombobox;
        private System.Windows.Forms.GroupBox LeadCardGroup;
        private System.Windows.Forms.CheckBox ValidateLeadCardCheckbox;
        private System.Windows.Forms.CheckBox EnterLeadCardCheckbox;
        private System.Windows.Forms.Button CanxButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.GroupBox EnterResultsMethodGroup;
        private System.Windows.Forms.ComboBox EnterResultsMethodCombobox;
        private System.Windows.Forms.GroupBox TabletMovesGroupBox;
        private System.Windows.Forms.RadioButton TabletModePersonalRadioButton;
        private System.Windows.Forms.RadioButton TabletModeTraditionalRadioButton;
        private System.Windows.Forms.GroupBox RoundTimerGroupBox;
        private System.Windows.Forms.Label AdditionalMinutesPerRoundLabel;
        private System.Windows.Forms.NumericUpDown AdditionalMinutesPerRoundNud;
        private System.Windows.Forms.Label MinutesPerBoardLabel;
        private System.Windows.Forms.NumericUpDown MinutesPerBoardNud;
        private System.Windows.Forms.CheckBox ShowTimerCheckbox;
    }
}