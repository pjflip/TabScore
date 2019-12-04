using System;
using System.Windows.Forms;


namespace TabScoreStarter
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            Options opt = new Options(new Database(Tag.ToString()));

            ShowTravellerCheckbox.Checked = opt.showTraveller;
            ShowPercentageCheckbox.Checked = opt.showPercentage;
            ShowHandRecordCheckbox.Checked = opt.showHandRecord;
            ShowRankingCombobox.SelectedIndex = opt.showRanking;
            EnterLeadCardCheckbox.Checked = opt.enterLeadCard;
            ValidateLeadCardCheckbox.Checked = opt.validateLeadCard;
            NameSourceCombobox.SelectedIndex = opt.nameSource;
            NumberEntryEachRoundCheckbox.Checked = opt.numberEntryEachRound;
            EnterResultsMethodCombobox.SelectedIndex = opt.enterResultsMethod;

            ShowPercentageCheckbox.Enabled = ShowTravellerCheckbox.Checked;
            ShowHandRecordCheckbox.Enabled = ShowTravellerCheckbox.Checked;
            ValidateLeadCardCheckbox.Enabled = EnterLeadCardCheckbox.Checked;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Options opt = new Options(new Database(Tag.ToString()))
            {
                showTraveller = ShowTravellerCheckbox.Checked,
                showPercentage = ShowPercentageCheckbox.Checked,
                showHandRecord = ShowHandRecordCheckbox.Checked,
                showRanking = ShowRankingCombobox.SelectedIndex,
                enterLeadCard = EnterLeadCardCheckbox.Checked,
                validateLeadCard = ValidateLeadCardCheckbox.Checked,
                nameSource = NameSourceCombobox.SelectedIndex,
                numberEntryEachRound = NumberEntryEachRoundCheckbox.Checked,
                enterResultsMethod = EnterResultsMethodCombobox.SelectedIndex
            };
            opt.UpdateDB();
            Close();
        }

        private void ShowTraveller_CheckedChanged(object sender, EventArgs e)
        {
            ShowPercentageCheckbox.Enabled = ShowTravellerCheckbox.Checked;
            ShowHandRecordCheckbox.Enabled = ShowTravellerCheckbox.Checked;
        }

        private void EnterLeadCard_CheckedChanged(object sender, EventArgs e)
        {
            ValidateLeadCardCheckbox.Enabled = EnterLeadCardCheckbox.Checked;
        }
    }
}
