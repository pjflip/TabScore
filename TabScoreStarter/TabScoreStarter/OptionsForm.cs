using System;
using System.Windows.Forms;
using System.Drawing;


namespace TabScoreStarter
{
    public partial class OptionsForm : Form
    {
        private readonly string connectionString;

        public OptionsForm(string connectionString, Point location)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            Location = location;
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            Options opt = new Options(connectionString);

            ShowTravellerCheckbox.Checked = opt.ShowTraveller;
            ShowPercentageCheckbox.Checked = opt.ShowPercentage;
            ShowHandRecordCheckbox.Checked = opt.ShowHandRecord;
            HandRecordReversePerspectiveCheckbox.Checked = opt.HandRecordReversePerspective;
            ShowRankingCombobox.SelectedIndex = opt.ShowRanking;
            EnterLeadCardCheckbox.Checked = opt.EnterLeadCard;
            ValidateLeadCardCheckbox.Checked = opt.ValidateLeadCard;
            NameSourceCombobox.SelectedIndex = opt.NameSource;
            NumberEntryEachRoundCheckbox.Checked = opt.NumberEntryEachRound;
            EnterResultsMethodCombobox.SelectedIndex = opt.EnterResultsMethod;
            TabletModePersonalRadioButton.Checked = opt.TabletsMove;
            TabletModeTraditionalRadioButton.Checked = !opt.TabletsMove;
            ShowTimerCheckbox.Checked = opt.ShowTimer;
            MinutesPerBoardNud.Value = Convert.ToDecimal(opt.SecondsPerBoard) / 60;
            AdditionalMinutesPerRoundNud.Value = Convert.ToDecimal(opt.AdditionalSecondsPerRound) / 60;

            ShowPercentageCheckbox.Enabled = ShowTravellerCheckbox.Checked;
            ShowHandRecordCheckbox.Enabled = ShowTravellerCheckbox.Checked;
            HandRecordReversePerspectiveCheckbox.Enabled = ShowTravellerCheckbox.Checked && ShowHandRecordCheckbox.Checked;
            ValidateLeadCardCheckbox.Enabled = EnterLeadCardCheckbox.Checked;
            MinutesPerBoardNud.Enabled = ShowTimerCheckbox.Checked;
            AdditionalMinutesPerRoundNud.Enabled = ShowTimerCheckbox.Checked;
            MinutesPerBoardLabel.Enabled = ShowTimerCheckbox.Checked;
            AdditionalMinutesPerRoundLabel.Enabled = ShowTimerCheckbox.Checked;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Options opt = new Options()
            {
                ShowTraveller = ShowTravellerCheckbox.Checked,
                ShowPercentage = ShowPercentageCheckbox.Checked,
                ShowHandRecord = ShowHandRecordCheckbox.Checked,
                HandRecordReversePerspective = HandRecordReversePerspectiveCheckbox.Checked,
                ShowRanking = ShowRankingCombobox.SelectedIndex,
                EnterLeadCard = EnterLeadCardCheckbox.Checked,
                ValidateLeadCard = ValidateLeadCardCheckbox.Checked,
                NameSource = NameSourceCombobox.SelectedIndex,
                NumberEntryEachRound = NumberEntryEachRoundCheckbox.Checked,
                EnterResultsMethod = EnterResultsMethodCombobox.SelectedIndex,
                TabletsMove = TabletModePersonalRadioButton.Checked,
                ShowTimer = ShowTimerCheckbox.Checked,
                SecondsPerBoard = Convert.ToInt32(MinutesPerBoardNud.Value * 60),
                AdditionalSecondsPerRound = Convert.ToInt32(AdditionalMinutesPerRoundNud.Value * 60)
            };
            opt.UpdateDB(connectionString);
            Properties.Settings.Default.TabletsMove = opt.TabletsMove;
            Properties.Settings.Default.HandRecordReversePerspective = opt.HandRecordReversePerspective;
            Properties.Settings.Default.ShowTimer = opt.ShowTimer;
            Properties.Settings.Default.SecondsPerBoard = opt.SecondsPerBoard;
            Properties.Settings.Default.AdditionalSecondsPerRound = opt.AdditionalSecondsPerRound;
            Properties.Settings.Default.Save();
            Close();
        }

        private void ShowTraveller_CheckedChanged(object sender, EventArgs e)
        {
            ShowPercentageCheckbox.Enabled = ShowTravellerCheckbox.Checked;
            ShowHandRecordCheckbox.Enabled = ShowTravellerCheckbox.Checked;
            HandRecordReversePerspectiveCheckbox.Enabled = ShowTravellerCheckbox.Checked && ShowHandRecordCheckbox.Checked;
        }

        private void ShowHandRecord_CheckedChanged(object sender, EventArgs e)
        {
            HandRecordReversePerspectiveCheckbox.Enabled = ShowTravellerCheckbox.Checked && ShowHandRecordCheckbox.Checked;
        }

        private void EnterLeadCard_CheckedChanged(object sender, EventArgs e)
        {
            ValidateLeadCardCheckbox.Enabled = EnterLeadCardCheckbox.Checked;
        }

        private void ShowTimerCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            MinutesPerBoardNud.Enabled = ShowTimerCheckbox.Checked;
            AdditionalMinutesPerRoundNud.Enabled = ShowTimerCheckbox.Checked;
            MinutesPerBoardLabel.Enabled = ShowTimerCheckbox.Checked;
            AdditionalMinutesPerRoundLabel.Enabled = ShowTimerCheckbox.Checked;
        }
    }
}
