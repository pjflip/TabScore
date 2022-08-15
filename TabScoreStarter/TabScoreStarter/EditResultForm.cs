using System;
using System.Drawing;
using System.Windows.Forms;

namespace TabScoreStarter
{
    public partial class EditResultForm : Form
    {
        private readonly Result result;
        private readonly string connectionString;
        private readonly bool isIndividual;

        public EditResultForm(Result result, bool isIndividual, string connectionString, Point location)
        {
            InitializeComponent();
            this.result = result;
            this.connectionString = connectionString;
            this.isIndividual = isIndividual;
            Location = location;
        }

        private void EditResultForm_Load(object sender, EventArgs e)
        {
            if (result == null) return;
            labelSection.Text = result.Section.ToString();
            labelTable.Text = result.Table.ToString();
            labelRound.Text = result.Round.ToString();
            labelBoard.Text = result.Board.ToString();
            labelNorth.Text = result.PairNS.ToString();
            labelEast.Text = result.PairEW.ToString();
            if (result.ContractLevel <= 0)
            {
                if (result.ContractLevel == 0)
                {
                    comboBoxContractLevel.Text = "PASS";
                }
                else
                {
                    comboBoxContractLevel.Enabled = false;
                }
                comboBoxSuit.Enabled = false;
                comboBoxDouble.Enabled = false;
                comboBoxDeclarer.Enabled = false;
                comboBoxLead.Enabled = false;
                comboBoxTricksTaken.Enabled = false;
            }
            else
            {
                comboBoxContractLevel.Text = result.ContractLevel.ToString();
                comboBoxTricksTaken.Items.Clear();
                for (int i = 7 - result.ContractLevel; i > 0; i--)
                {
                    comboBoxTricksTaken.Items.Add("+" + i.ToString());
                }
                comboBoxTricksTaken.Items.Add("=");
                for (int i = 1; i <= result.ContractLevel + 6; i++)
                {
                    comboBoxTricksTaken.Items.Add("-" + i.ToString());
                }
                comboBoxSuit.Text = result.ContractSuit;
                comboBoxDouble.Text = result.ContractX;
                comboBoxDeclarer.Text = result.DeclarerNSEW;
                comboBoxLead.Text = result.Lead;
                comboBoxTricksTaken.Text = result.TricksTaken;
            }
            comboBoxRemarks.Text = result.Remarks;
        }

        private void CanxButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ComboBoxContractLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxContractLevel.Text == "PASS" || comboBoxContractLevel.Text == "")
            {
                comboBoxSuit.Text = "";
                comboBoxDouble.Text = "";
                comboBoxDeclarer.Text = "";
                comboBoxLead.Text = "";
                comboBoxTricksTaken.Text = "";
                comboBoxSuit.Enabled = false;
                comboBoxDouble.Enabled = false;
                comboBoxDeclarer.Enabled = false;
                comboBoxLead.Enabled = false;
                comboBoxTricksTaken.Enabled = false;
            }
            else {
                comboBoxSuit.Enabled = true;
                comboBoxDouble.Enabled = true;
                comboBoxDeclarer.Enabled = true;
                comboBoxLead.Enabled = true;
                comboBoxTricksTaken.Items.Clear();
                int contractLevel = Convert.ToInt32(comboBoxContractLevel.Text);
                for (int i = 7 - contractLevel; i > 0; i--)
                {
                    comboBoxTricksTaken.Items.Add("+" + i.ToString());
                }
                comboBoxTricksTaken.Items.Add("=");
                for (int i = 1; i <= contractLevel + 6; i++)
                {
                    comboBoxTricksTaken.Items.Add("-" + i.ToString());
                }
                comboBoxTricksTaken.Enabled = true;
            }
        }

        private void ComboBoxRemarks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxRemarks.Text == "" || comboBoxRemarks.Text == "Wrong direction")
            {
                comboBoxContractLevel.Enabled = true;
                comboBoxSuit.Enabled = true;
                comboBoxDouble.Enabled = true;
                comboBoxDeclarer.Enabled = true;
                comboBoxLead.Enabled = true;
                comboBoxTricksTaken.Enabled = true;
            }
            else
            {
                comboBoxContractLevel.Text = "";  // This invokes ComboBoxContractLevel_SelectedIndexChanged
                comboBoxContractLevel.Enabled = false;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (comboBoxContractLevel.Text == "")
            {
                if (comboBoxRemarks.Text == "" || comboBoxRemarks.Text == "Wrong direction")
                {
                    MessageBox.Show("Please enter a valid result", "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                result.ContractLevel = -1;
                result.Contract = "";
                result.ContractSuit = "";
                result.ContractX = "";
                result.DeclarerNSEW = "";
                result.Lead = "";
                result.TricksTaken = "";
                result.Remarks = comboBoxRemarks.Text;
            }
            else if (comboBoxContractLevel.Text == "PASS")
            {
                result.ContractLevel = 0;
                result.Contract = "PASS";
                result.ContractSuit = "";
                result.ContractX = "";
                result.DeclarerNSEW = "";
                result.Lead = "";
                result.TricksTaken = "";
                result.Remarks = comboBoxRemarks.Text;
            }
            else
            {
                if (comboBoxSuit.Text == "" || comboBoxDeclarer.Text == "" || comboBoxTricksTaken.Text == "")
                {
                    MessageBox.Show("Please enter valid contract details", "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                result.ContractLevel = Convert.ToInt32(comboBoxContractLevel.Text);
                result.ContractSuit = comboBoxSuit.Text;
                result.ContractX = comboBoxDouble.Text;
                string contract = $"{result.ContractLevel} {result.ContractSuit}";
                if (result.ContractX != "")
                {
                    contract = $"{contract} {result.ContractX}";
                }
                result.Contract = contract;
                result.DeclarerNSEW = comboBoxDeclarer.Text;
                result.Lead = comboBoxLead.Text;
                result.TricksTaken = comboBoxTricksTaken.Text;
                result.Remarks = comboBoxRemarks.Text;
            }
            result.UpdateDB(connectionString, isIndividual);
            Close();
        }
    }
}
