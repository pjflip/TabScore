namespace TabScoreStarter
{
    partial class EditResultForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditResultForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxContractLevel = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxSuit = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBoxDouble = new System.Windows.Forms.ComboBox();
            this.comboBoxDeclarer = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBoxLead = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBoxTricksTaken = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBoxRemarks = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CanxButton = new System.Windows.Forms.Button();
            this.labelSection = new System.Windows.Forms.Label();
            this.labelTable = new System.Windows.Forms.Label();
            this.labelRound = new System.Windows.Forms.Label();
            this.labelBoard = new System.Windows.Forms.Label();
            this.labelNorth = new System.Windows.Forms.Label();
            this.labelEast = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Section";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(78, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Table";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(138, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Round";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(199, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Board";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(258, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "North";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(319, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "East";
            // 
            // comboBoxContractLevel
            // 
            this.comboBoxContractLevel.FormattingEnabled = true;
            this.comboBoxContractLevel.Items.AddRange(new object[] {
            "",
            "PASS",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7"});
            this.comboBoxContractLevel.Location = new System.Drawing.Point(19, 91);
            this.comboBoxContractLevel.Name = "comboBoxContractLevel";
            this.comboBoxContractLevel.Size = new System.Drawing.Size(73, 21);
            this.comboBoxContractLevel.TabIndex = 6;
            this.comboBoxContractLevel.SelectedIndexChanged += new System.EventHandler(this.ComboBoxContractLevel_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 75);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Contract Level";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(97, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Suit";
            // 
            // comboBoxSuit
            // 
            this.comboBoxSuit.FormattingEnabled = true;
            this.comboBoxSuit.Items.AddRange(new object[] {
            "",
            "C",
            "D",
            "H",
            "S",
            "NT"});
            this.comboBoxSuit.Location = new System.Drawing.Point(98, 91);
            this.comboBoxSuit.Name = "comboBoxSuit";
            this.comboBoxSuit.Size = new System.Drawing.Size(49, 21);
            this.comboBoxSuit.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(151, 75);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Double";
            // 
            // comboBoxDouble
            // 
            this.comboBoxDouble.FormattingEnabled = true;
            this.comboBoxDouble.Items.AddRange(new object[] {
            "",
            "x",
            "xx"});
            this.comboBoxDouble.Location = new System.Drawing.Point(153, 91);
            this.comboBoxDouble.Name = "comboBoxDouble";
            this.comboBoxDouble.Size = new System.Drawing.Size(49, 21);
            this.comboBoxDouble.TabIndex = 11;
            // 
            // comboBoxDeclarer
            // 
            this.comboBoxDeclarer.FormattingEnabled = true;
            this.comboBoxDeclarer.Items.AddRange(new object[] {
            "",
            "N",
            "S",
            "E",
            "W"});
            this.comboBoxDeclarer.Location = new System.Drawing.Point(208, 91);
            this.comboBoxDeclarer.Name = "comboBoxDeclarer";
            this.comboBoxDeclarer.Size = new System.Drawing.Size(49, 21);
            this.comboBoxDeclarer.TabIndex = 12;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(206, 75);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "Declarer";
            // 
            // comboBoxLead
            // 
            this.comboBoxLead.FormattingEnabled = true;
            this.comboBoxLead.Items.AddRange(new object[] {
            "",
            "C2",
            "C3",
            "C4",
            "C5",
            "C6",
            "C7",
            "C8",
            "C9",
            "C10",
            "CJ",
            "CQ",
            "CK",
            "CA",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "D10",
            "DJ",
            "DQ",
            "DK",
            "DA",
            "H2",
            "H3",
            "H4",
            "H5",
            "H6",
            "H7",
            "H8",
            "H9",
            "H10",
            "HJ",
            "HQ",
            "HK",
            "HA",
            "S2",
            "S3",
            "S4",
            "S5",
            "S6",
            "S7",
            "S8",
            "S9",
            "S10",
            "SJ",
            "SQ",
            "SK",
            "SA"});
            this.comboBoxLead.Location = new System.Drawing.Point(263, 91);
            this.comboBoxLead.Name = "comboBoxLead";
            this.comboBoxLead.Size = new System.Drawing.Size(49, 21);
            this.comboBoxLead.TabIndex = 14;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(261, 75);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 13);
            this.label11.TabIndex = 15;
            this.label11.Text = "Lead";
            // 
            // comboBoxTricksTaken
            // 
            this.comboBoxTricksTaken.FormattingEnabled = true;
            this.comboBoxTricksTaken.Items.AddRange(new object[] {
            "",
            "+6",
            "+5",
            "+4",
            "+3",
            "+2",
            "+1",
            "=",
            "-1",
            "-2",
            "-3",
            "-4",
            "-5",
            "-6",
            "-7",
            "-8",
            "-9",
            "-10",
            "-11",
            "-12",
            "-13"});
            this.comboBoxTricksTaken.Location = new System.Drawing.Point(318, 91);
            this.comboBoxTricksTaken.Name = "comboBoxTricksTaken";
            this.comboBoxTricksTaken.Size = new System.Drawing.Size(55, 21);
            this.comboBoxTricksTaken.TabIndex = 16;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(316, 75);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Result";
            // 
            // comboBoxRemarks
            // 
            this.comboBoxRemarks.FormattingEnabled = true;
            this.comboBoxRemarks.Items.AddRange(new object[] {
            "",
            "Not played",
            "40%-40%",
            "50%-40%",
            "60%-40%",
            "40%-50%",
            "50%-50%",
            "60%-50%",
            "40%-60%",
            "50%-60%",
            "60%-60%",
            "Arbitral score",
            "Wrong direction"});
            this.comboBoxRemarks.Location = new System.Drawing.Point(20, 144);
            this.comboBoxRemarks.Name = "comboBoxRemarks";
            this.comboBoxRemarks.Size = new System.Drawing.Size(353, 21);
            this.comboBoxRemarks.TabIndex = 20;
            this.comboBoxRemarks.SelectedIndexChanged += new System.EventHandler(this.ComboBoxRemarks_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(18, 128);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(257, 13);
            this.label13.TabIndex = 21;
            this.label13.Text = "Remarks (Not played, Arbitral score, Wrong direction)";
            // 
            // SaveButton
            // 
            this.SaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveButton.Location = new System.Drawing.Point(19, 184);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(141, 23);
            this.SaveButton.TabIndex = 82;
            this.SaveButton.Text = "Save and Close";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CanxButton
            // 
            this.CanxButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CanxButton.Location = new System.Drawing.Point(298, 184);
            this.CanxButton.Name = "CanxButton";
            this.CanxButton.Size = new System.Drawing.Size(75, 23);
            this.CanxButton.TabIndex = 83;
            this.CanxButton.Text = "Cancel";
            this.CanxButton.UseVisualStyleBackColor = true;
            this.CanxButton.Click += new System.EventHandler(this.CanxButton_Click);
            // 
            // labelSection
            // 
            this.labelSection.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelSection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelSection.Location = new System.Drawing.Point(19, 35);
            this.labelSection.Name = "labelSection";
            this.labelSection.Size = new System.Drawing.Size(52, 20);
            this.labelSection.TabIndex = 84;
            this.labelSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTable
            // 
            this.labelTable.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelTable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTable.Location = new System.Drawing.Point(80, 35);
            this.labelTable.Name = "labelTable";
            this.labelTable.Size = new System.Drawing.Size(52, 20);
            this.labelTable.TabIndex = 85;
            this.labelTable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRound
            // 
            this.labelRound.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelRound.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelRound.Location = new System.Drawing.Point(140, 35);
            this.labelRound.Name = "labelRound";
            this.labelRound.Size = new System.Drawing.Size(52, 20);
            this.labelRound.TabIndex = 86;
            this.labelRound.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelBoard
            // 
            this.labelBoard.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelBoard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelBoard.Location = new System.Drawing.Point(200, 35);
            this.labelBoard.Name = "labelBoard";
            this.labelBoard.Size = new System.Drawing.Size(52, 20);
            this.labelBoard.TabIndex = 87;
            this.labelBoard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelNorth
            // 
            this.labelNorth.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelNorth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelNorth.Location = new System.Drawing.Point(260, 35);
            this.labelNorth.Name = "labelNorth";
            this.labelNorth.Size = new System.Drawing.Size(52, 20);
            this.labelNorth.TabIndex = 88;
            this.labelNorth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelEast
            // 
            this.labelEast.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelEast.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelEast.Location = new System.Drawing.Point(321, 35);
            this.labelEast.Name = "labelEast";
            this.labelEast.Size = new System.Drawing.Size(52, 20);
            this.labelEast.TabIndex = 89;
            this.labelEast.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EditResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 225);
            this.Controls.Add(this.labelEast);
            this.Controls.Add(this.labelNorth);
            this.Controls.Add(this.labelBoard);
            this.Controls.Add(this.labelRound);
            this.Controls.Add(this.labelTable);
            this.Controls.Add(this.labelSection);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.CanxButton);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.comboBoxRemarks);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.comboBoxTricksTaken);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.comboBoxLead);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.comboBoxDeclarer);
            this.Controls.Add(this.comboBoxDouble);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboBoxSuit);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBoxContractLevel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditResultForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TabScoreStarter - Edit Result";
            this.Load += new System.EventHandler(this.EditResultForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxContractLevel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxSuit;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBoxDouble;
        private System.Windows.Forms.ComboBox comboBoxDeclarer;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBoxLead;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBoxTricksTaken;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboBoxRemarks;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button CanxButton;
        private System.Windows.Forms.Label labelSection;
        private System.Windows.Forms.Label labelTable;
        private System.Windows.Forms.Label labelRound;
        private System.Windows.Forms.Label labelBoard;
        private System.Windows.Forms.Label labelNorth;
        private System.Windows.Forms.Label labelEast;
    }
}