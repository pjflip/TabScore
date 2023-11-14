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
            this.SaveButton = new System.Windows.Forms.Button();
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
            this.HandRecordGroup = new System.Windows.Forms.GroupBox();
            this.HandRecordReversePerspectiveCheckbox = new System.Windows.Forms.CheckBox();
            this.DoubleDummyCheckbox = new System.Windows.Forms.CheckBox();
            this.ShowHandRecordCheckbox = new System.Windows.Forms.CheckBox();
            this.ManualHandEntryCheckbox = new System.Windows.Forms.CheckBox();
            this.TravellerGroup.SuspendLayout();
            this.PlayersGroup.SuspendLayout();
            this.RankingListGroup.SuspendLayout();
            this.LeadCardGroup.SuspendLayout();
            this.EnterResultsMethodGroup.SuspendLayout();
            this.TabletMovesGroupBox.SuspendLayout();
            this.RoundTimerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AdditionalMinutesPerRoundNud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinutesPerBoardNud)).BeginInit();
            this.HandRecordGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // TravellerGroup
            // 
            this.TravellerGroup.Controls.Add(this.ShowPercentageCheckbox);
            this.TravellerGroup.Controls.Add(this.ShowTravellerCheckbox);
            resources.ApplyResources(this.TravellerGroup, "TravellerGroup");
            this.TravellerGroup.Name = "TravellerGroup";
            this.TravellerGroup.TabStop = false;
            // 
            // ShowPercentageCheckbox
            // 
            resources.ApplyResources(this.ShowPercentageCheckbox, "ShowPercentageCheckbox");
            this.ShowPercentageCheckbox.Name = "ShowPercentageCheckbox";
            this.ShowPercentageCheckbox.UseVisualStyleBackColor = true;
            // 
            // ShowTravellerCheckbox
            // 
            resources.ApplyResources(this.ShowTravellerCheckbox, "ShowTravellerCheckbox");
            this.ShowTravellerCheckbox.Name = "ShowTravellerCheckbox";
            this.ShowTravellerCheckbox.UseVisualStyleBackColor = true;
            this.ShowTravellerCheckbox.CheckedChanged += new System.EventHandler(this.ShowTraveller_CheckedChanged);
            // 
            // PlayersGroup
            // 
            this.PlayersGroup.Controls.Add(this.NameSourceCombobox);
            this.PlayersGroup.Controls.Add(this.NumberEntryEachRoundCheckbox);
            resources.ApplyResources(this.PlayersGroup, "PlayersGroup");
            this.PlayersGroup.Name = "PlayersGroup";
            this.PlayersGroup.TabStop = false;
            // 
            // NameSourceCombobox
            // 
            resources.ApplyResources(this.NameSourceCombobox, "NameSourceCombobox");
            this.NameSourceCombobox.FormattingEnabled = true;
            this.NameSourceCombobox.Items.AddRange(new object[] {
            resources.GetString("NameSourceCombobox.Items"),
            resources.GetString("NameSourceCombobox.Items1"),
            resources.GetString("NameSourceCombobox.Items2"),
            resources.GetString("NameSourceCombobox.Items3")});
            this.NameSourceCombobox.Name = "NameSourceCombobox";
            // 
            // NumberEntryEachRoundCheckbox
            // 
            resources.ApplyResources(this.NumberEntryEachRoundCheckbox, "NumberEntryEachRoundCheckbox");
            this.NumberEntryEachRoundCheckbox.Name = "NumberEntryEachRoundCheckbox";
            this.NumberEntryEachRoundCheckbox.UseVisualStyleBackColor = true;
            // 
            // RankingListGroup
            // 
            this.RankingListGroup.Controls.Add(this.ShowRankingCombobox);
            resources.ApplyResources(this.RankingListGroup, "RankingListGroup");
            this.RankingListGroup.Name = "RankingListGroup";
            this.RankingListGroup.TabStop = false;
            // 
            // ShowRankingCombobox
            // 
            resources.ApplyResources(this.ShowRankingCombobox, "ShowRankingCombobox");
            this.ShowRankingCombobox.FormattingEnabled = true;
            this.ShowRankingCombobox.Items.AddRange(new object[] {
            resources.GetString("ShowRankingCombobox.Items"),
            resources.GetString("ShowRankingCombobox.Items1"),
            resources.GetString("ShowRankingCombobox.Items2")});
            this.ShowRankingCombobox.Name = "ShowRankingCombobox";
            // 
            // LeadCardGroup
            // 
            this.LeadCardGroup.Controls.Add(this.ValidateLeadCardCheckbox);
            this.LeadCardGroup.Controls.Add(this.EnterLeadCardCheckbox);
            resources.ApplyResources(this.LeadCardGroup, "LeadCardGroup");
            this.LeadCardGroup.Name = "LeadCardGroup";
            this.LeadCardGroup.TabStop = false;
            // 
            // ValidateLeadCardCheckbox
            // 
            resources.ApplyResources(this.ValidateLeadCardCheckbox, "ValidateLeadCardCheckbox");
            this.ValidateLeadCardCheckbox.Name = "ValidateLeadCardCheckbox";
            this.ValidateLeadCardCheckbox.UseVisualStyleBackColor = true;
            // 
            // EnterLeadCardCheckbox
            // 
            resources.ApplyResources(this.EnterLeadCardCheckbox, "EnterLeadCardCheckbox");
            this.EnterLeadCardCheckbox.Name = "EnterLeadCardCheckbox";
            this.EnterLeadCardCheckbox.UseVisualStyleBackColor = true;
            this.EnterLeadCardCheckbox.CheckedChanged += new System.EventHandler(this.EnterLeadCard_CheckedChanged);
            // 
            // CanxButton
            // 
            resources.ApplyResources(this.CanxButton, "CanxButton");
            this.CanxButton.Name = "CanxButton";
            this.CanxButton.UseVisualStyleBackColor = true;
            this.CanxButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SaveButton
            // 
            resources.ApplyResources(this.SaveButton, "SaveButton");
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // EnterResultsMethodGroup
            // 
            this.EnterResultsMethodGroup.Controls.Add(this.EnterResultsMethodCombobox);
            resources.ApplyResources(this.EnterResultsMethodGroup, "EnterResultsMethodGroup");
            this.EnterResultsMethodGroup.Name = "EnterResultsMethodGroup";
            this.EnterResultsMethodGroup.TabStop = false;
            // 
            // EnterResultsMethodCombobox
            // 
            resources.ApplyResources(this.EnterResultsMethodCombobox, "EnterResultsMethodCombobox");
            this.EnterResultsMethodCombobox.FormattingEnabled = true;
            this.EnterResultsMethodCombobox.Items.AddRange(new object[] {
            resources.GetString("EnterResultsMethodCombobox.Items"),
            resources.GetString("EnterResultsMethodCombobox.Items1")});
            this.EnterResultsMethodCombobox.Name = "EnterResultsMethodCombobox";
            // 
            // TabletMovesGroupBox
            // 
            this.TabletMovesGroupBox.Controls.Add(this.TabletModePersonalRadioButton);
            this.TabletMovesGroupBox.Controls.Add(this.TabletModeTraditionalRadioButton);
            resources.ApplyResources(this.TabletMovesGroupBox, "TabletMovesGroupBox");
            this.TabletMovesGroupBox.Name = "TabletMovesGroupBox";
            this.TabletMovesGroupBox.TabStop = false;
            // 
            // TabletModePersonalRadioButton
            // 
            resources.ApplyResources(this.TabletModePersonalRadioButton, "TabletModePersonalRadioButton");
            this.TabletModePersonalRadioButton.Name = "TabletModePersonalRadioButton";
            this.TabletModePersonalRadioButton.TabStop = true;
            this.TabletModePersonalRadioButton.UseVisualStyleBackColor = true;
            // 
            // TabletModeTraditionalRadioButton
            // 
            resources.ApplyResources(this.TabletModeTraditionalRadioButton, "TabletModeTraditionalRadioButton");
            this.TabletModeTraditionalRadioButton.Name = "TabletModeTraditionalRadioButton";
            this.TabletModeTraditionalRadioButton.TabStop = true;
            this.TabletModeTraditionalRadioButton.UseVisualStyleBackColor = true;
            // 
            // RoundTimerGroupBox
            // 
            this.RoundTimerGroupBox.Controls.Add(this.AdditionalMinutesPerRoundLabel);
            this.RoundTimerGroupBox.Controls.Add(this.AdditionalMinutesPerRoundNud);
            this.RoundTimerGroupBox.Controls.Add(this.MinutesPerBoardLabel);
            this.RoundTimerGroupBox.Controls.Add(this.MinutesPerBoardNud);
            this.RoundTimerGroupBox.Controls.Add(this.ShowTimerCheckbox);
            resources.ApplyResources(this.RoundTimerGroupBox, "RoundTimerGroupBox");
            this.RoundTimerGroupBox.Name = "RoundTimerGroupBox";
            this.RoundTimerGroupBox.TabStop = false;
            // 
            // AdditionalMinutesPerRoundLabel
            // 
            resources.ApplyResources(this.AdditionalMinutesPerRoundLabel, "AdditionalMinutesPerRoundLabel");
            this.AdditionalMinutesPerRoundLabel.Name = "AdditionalMinutesPerRoundLabel";
            // 
            // AdditionalMinutesPerRoundNud
            // 
            this.AdditionalMinutesPerRoundNud.DecimalPlaces = 1;
            resources.ApplyResources(this.AdditionalMinutesPerRoundNud, "AdditionalMinutesPerRoundNud");
            this.AdditionalMinutesPerRoundNud.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.AdditionalMinutesPerRoundNud.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.AdditionalMinutesPerRoundNud.Name = "AdditionalMinutesPerRoundNud";
            this.AdditionalMinutesPerRoundNud.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // MinutesPerBoardLabel
            // 
            resources.ApplyResources(this.MinutesPerBoardLabel, "MinutesPerBoardLabel");
            this.MinutesPerBoardLabel.Name = "MinutesPerBoardLabel";
            // 
            // MinutesPerBoardNud
            // 
            this.MinutesPerBoardNud.DecimalPlaces = 1;
            resources.ApplyResources(this.MinutesPerBoardNud, "MinutesPerBoardNud");
            this.MinutesPerBoardNud.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
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
            this.MinutesPerBoardNud.Value = new decimal(new int[] {
            65,
            0,
            0,
            65536});
            // 
            // ShowTimerCheckbox
            // 
            resources.ApplyResources(this.ShowTimerCheckbox, "ShowTimerCheckbox");
            this.ShowTimerCheckbox.Name = "ShowTimerCheckbox";
            this.ShowTimerCheckbox.UseVisualStyleBackColor = true;
            this.ShowTimerCheckbox.CheckedChanged += new System.EventHandler(this.ShowTimerCheckbox_CheckedChanged);
            // 
            // HandRecordGroup
            // 
            this.HandRecordGroup.Controls.Add(this.HandRecordReversePerspectiveCheckbox);
            this.HandRecordGroup.Controls.Add(this.DoubleDummyCheckbox);
            this.HandRecordGroup.Controls.Add(this.ShowHandRecordCheckbox);
            this.HandRecordGroup.Controls.Add(this.ManualHandEntryCheckbox);
            resources.ApplyResources(this.HandRecordGroup, "HandRecordGroup");
            this.HandRecordGroup.Name = "HandRecordGroup";
            this.HandRecordGroup.TabStop = false;
            // 
            // HandRecordReversePerspectiveCheckbox
            // 
            resources.ApplyResources(this.HandRecordReversePerspectiveCheckbox, "HandRecordReversePerspectiveCheckbox");
            this.HandRecordReversePerspectiveCheckbox.Name = "HandRecordReversePerspectiveCheckbox";
            this.HandRecordReversePerspectiveCheckbox.UseVisualStyleBackColor = true;
            // 
            // DoubleDummyCheckbox
            // 
            resources.ApplyResources(this.DoubleDummyCheckbox, "DoubleDummyCheckbox");
            this.DoubleDummyCheckbox.Name = "DoubleDummyCheckbox";
            this.DoubleDummyCheckbox.UseVisualStyleBackColor = true;
            // 
            // ShowHandRecordCheckbox
            // 
            resources.ApplyResources(this.ShowHandRecordCheckbox, "ShowHandRecordCheckbox");
            this.ShowHandRecordCheckbox.Name = "ShowHandRecordCheckbox";
            this.ShowHandRecordCheckbox.UseVisualStyleBackColor = true;
            // 
            // ManualHandEntryCheckbox
            // 
            resources.ApplyResources(this.ManualHandEntryCheckbox, "ManualHandEntryCheckbox");
            this.ManualHandEntryCheckbox.Name = "ManualHandEntryCheckbox";
            this.ManualHandEntryCheckbox.UseVisualStyleBackColor = true;
            this.ManualHandEntryCheckbox.CheckedChanged += new System.EventHandler(this.ManualHandRecordEntryCheckbox_CheckedChanged);
            // 
            // OptionsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.HandRecordGroup);
            this.Controls.Add(this.RoundTimerGroupBox);
            this.Controls.Add(this.TabletMovesGroupBox);
            this.Controls.Add(this.EnterResultsMethodGroup);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.CanxButton);
            this.Controls.Add(this.PlayersGroup);
            this.Controls.Add(this.RankingListGroup);
            this.Controls.Add(this.LeadCardGroup);
            this.Controls.Add(this.TravellerGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
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
            this.HandRecordGroup.ResumeLayout(false);
            this.HandRecordGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox TravellerGroup;
        private System.Windows.Forms.CheckBox ShowPercentageCheckbox;
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
        private System.Windows.Forms.Button SaveButton;
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
        private System.Windows.Forms.GroupBox HandRecordGroup;
        private System.Windows.Forms.CheckBox HandRecordReversePerspectiveCheckbox;
        private System.Windows.Forms.CheckBox DoubleDummyCheckbox;
        private System.Windows.Forms.CheckBox ShowHandRecordCheckbox;
        private System.Windows.Forms.CheckBox ManualHandEntryCheckbox;
    }
}