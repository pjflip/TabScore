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
            Options opt = new Options(Database.ConnectionString(Tag.ToString()));

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
            MinutesPerBoardNud.Value = Convert.ToDecimal(opt.MinutesPerBoard);
            AdditionalMinutesPerRoundNud.Value = Convert.ToDecimal(opt.AdditionalMinutesPerRound);

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

        private void OKButton_Click(object sender, EventArgs e)
        {
            Options opt = new Options(Database.ConnectionString(Tag.ToString()))
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
                MinutesPerBoard = Convert.ToDouble(MinutesPerBoardNud.Value),
                AdditionalMinutesPerRound = Convert.ToDouble(AdditionalMinutesPerRoundNud.Value)
            };
            opt.UpdateDB();
            Properties.Settings.Default.TabletsMove = opt.TabletsMove;
            Properties.Settings.Default.HandRecordReversePerspective = opt.HandRecordReversePerspective;
            Properties.Settings.Default.ShowTimer = opt.ShowTimer;
            Properties.Settings.Default.MinutesPerBoard = opt.MinutesPerBoard;
            Properties.Settings.Default.AdditionalMinutesPerRound = opt.AdditionalMinutesPerRound;
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
