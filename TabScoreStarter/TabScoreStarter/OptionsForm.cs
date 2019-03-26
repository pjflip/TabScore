using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;


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
            string DBConnection = ScoringDatabase.SetDBConnectionString(Tag.ToString());
            Options opt = new Options();
            opt.GetOptions(DBConnection);

            cbShowTraveller.Checked = opt.showTraveller;
            cbShowPercentage.Checked = opt.showPercentage;
            cbShowHandRecord.Checked = opt.showHandRecord;
            cbShowRanking.SelectedIndex = opt.showRanking;
            cbEnterLeadCard.Checked = opt.enterLeadCard;
            cbValidateLeadCard.Checked = opt.validateLeadCard;
            cbNameSource.SelectedIndex = opt.nameSource;
            cbNumberEntryEachRound.Checked = opt.numberEntryEachRound;
            cbEnterResultsMethod.SelectedIndex = opt.enterResultsMethod;

            cbShowPercentage.Enabled = cbShowTraveller.Checked;
            cbShowHandRecord.Enabled = cbShowTraveller.Checked;
            cbValidateLeadCard.Enabled = cbEnterLeadCard.Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string DBConnection = ScoringDatabase.SetDBConnectionString(Tag.ToString());
            Options opt = new Options
            {
                showTraveller = cbShowTraveller.Checked,
                showPercentage = cbShowPercentage.Checked,
                showHandRecord = cbShowHandRecord.Checked,
                showRanking = cbShowRanking.SelectedIndex,
                enterLeadCard = cbEnterLeadCard.Checked,
                validateLeadCard = cbValidateLeadCard.Checked,
                nameSource = cbNameSource.SelectedIndex,
                numberEntryEachRound = cbNumberEntryEachRound.Checked,
                enterResultsMethod = cbEnterResultsMethod.SelectedIndex
            };
            opt.SetOptions(DBConnection);
            Close();
        }

        private void cbShowTraveller_CheckedChanged(object sender, EventArgs e)
        {
            cbShowPercentage.Enabled = cbShowTraveller.Checked;
            cbShowHandRecord.Enabled = cbShowTraveller.Checked;
        }

        private void cbEnterLeadCard_CheckedChanged(object sender, EventArgs e)
        {
            cbValidateLeadCard.Enabled = cbEnterLeadCard.Checked;
        }
    }
}
