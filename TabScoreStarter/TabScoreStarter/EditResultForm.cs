using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace TabScoreStarter
{
    public partial class EditResultForm : Form
    {
        private readonly Result result;
        private readonly string connectionString;
        private readonly ResourceManager resourceManager;
        private readonly string[] suitsDatabase = { "", "NT", "S", "H", "D", "C" };
        private readonly string[] suitsKey = { "", "SuitNT", "SuitS", "SuitH", "SuitD", "SuitC" };
        private readonly string[] suitsDisplay = new string[6];
        private readonly string[] declarerDatabase = { "", "N", "S", "E", "W" };
        private readonly string[] declarerDisplay = new string[5];
        private readonly string[] leadDatabase = { "", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10", "CJ", "CQ", "CK", "CA", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "D10", "DJ", "DQ", "DK", "DA", "H2", "H3", "H4", "H5", "H6", "H7", "H8", "H9", "H10", "HJ", "HQ", "HK", "HA", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9", "S10", "SJ", "SQ", "SK", "SA"};
        private readonly string[] leadDisplay = new string[53];
        private readonly string[] remarksDatabase = { "", "Not played", "40%-40%", "50%-40%", "60%-40%", "40%-50%", "50%-50%", "60%-50%", "40%-60%", "50%-60%", "60%-60%", "Arbitral score", "Wrong direction"};
        private readonly string[] remarksDisplay = new string[13];
        private readonly string[] contractLevelDisplay = { "", "PASS", "1", "2", "3", "4", "5", "6", "7" };

        public EditResultForm(Result result, string connectionString, Point location)
        {
            resourceManager = new ResourceManager("TabScoreStarter.Strings", typeof(TabScoreForm).Assembly);
            InitializeComponent();
            this.result = result;
            this.connectionString = connectionString;
            Location = location;
        }

        private void EditResultForm_Load(object sender, EventArgs e)
        {
            if (result == null) return;
            boxSection.Text = result.SectionLetter;
            boxTable.Text = result.Table.ToString();
            boxRound.Text = result.Round.ToString();
            boxBoard.Text = result.Board.ToString();
            boxNorth.Text = result.PairNS.ToString();
            boxEast.Text = result.PairEW.ToString();

            // Set up selection lists for combo boxes based on UICulture language
            suitsDisplay[0] = "";
            for (int i = 1; i < 6; i++)
            {
                suitsDisplay[i] = resourceManager.GetString(suitsKey[i]);
            }
            comboBoxSuit.Items.AddRange(suitsDisplay);
            declarerDisplay[0] = "";
            for (int i = 1; i < 5; i++)
            {
                declarerDisplay[i] = resourceManager.GetString(declarerDatabase[i]);
            }
            comboBoxDeclarer.Items.AddRange(declarerDisplay);
            leadDisplay[0] = "";
            for (int i = 1; i < 53; i++)
            {
                leadDisplay[i] = resourceManager.GetString(leadDatabase[i]);
            }
            comboBoxLead.Items.AddRange(leadDisplay);
            remarksDisplay[0] = "";
            for (int i = 1; i < 13; i++)
            {
                remarksDisplay[i] = resourceManager.GetString(remarksDatabase[i]);
            }
            comboBoxRemarks.Items.AddRange(remarksDisplay);
            contractLevelDisplay[1] = resourceManager.GetString("PASS");
            comboBoxContractLevel.Items.AddRange(contractLevelDisplay); 

            if (result.ContractLevel <= 0)
            {
                if (result.ContractLevel == 0)
                {
                    comboBoxContractLevel.Text = resourceManager.GetString("PASS");
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
                comboBoxSuit.Text = resourceManager.GetString("Suit" + result.ContractSuit);
                comboBoxDouble.Text = result.ContractX;
                comboBoxDeclarer.Text = resourceManager.GetString(result.DeclarerNSEW);
                comboBoxLead.Text = resourceManager.GetString(result.LeadCard);
                comboBoxTricksTaken.Text = result.TricksTaken;
            }
            comboBoxRemarks.Text = resourceManager.GetString(result.Remarks);
        }

        private void CanxButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ComboBoxContractLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxContractLevel.SelectedIndex == 0 || comboBoxContractLevel.SelectedIndex == 1)
            {
                // "PASS" or ""
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
                comboBoxTricksTaken.ResetText();
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
                comboBoxTricksTaken.SelectedItem = result.TricksTaken;
                comboBoxTricksTaken.Enabled = true;
            }
        }

        private void ComboBoxRemarks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxRemarks.SelectedIndex == 0 || comboBoxRemarks.SelectedIndex == 12)
            {
                // "" or "Wrong direction"
                comboBoxContractLevel.Enabled = true;
                comboBoxSuit.Enabled = true;
                comboBoxDouble.Enabled = true;
                comboBoxDeclarer.Enabled = true;
                comboBoxLead.Enabled = true;
                comboBoxTricksTaken.Enabled = true;
            }
            else
            {
                comboBoxContractLevel.SelectedIndex = 0;  // = "".  This invokes ComboBoxContractLevel_SelectedIndexChanged
                comboBoxContractLevel.Enabled = false;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (comboBoxContractLevel.SelectedIndex <= 0)
            {
                if (comboBoxRemarks.SelectedIndex <= 0 || comboBoxRemarks.SelectedIndex == 12)  // "" or "Wrong direction"
                {
                    MessageBox.Show(resourceManager.GetString("EnterValidResult"), "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                result.ContractLevel = -1;
                result.Contract = "";
                result.ContractSuit = "";
                result.ContractX = "";
                result.DeclarerNSEW = "";
                result.LeadCard = "";
                result.TricksTaken = "";
                result.Remarks = remarksDatabase[comboBoxRemarks.SelectedIndex];
            }
            else if (comboBoxContractLevel.SelectedIndex == 1)  // = "PASS" or equivalent
            {
                result.ContractLevel = 0;
                result.Contract = "PASS";
                result.ContractSuit = "";
                result.ContractX = "";
                result.DeclarerNSEW = "";
                result.LeadCard = "";
                result.TricksTaken = "";
                result.Remarks = remarksDatabase[comboBoxRemarks.SelectedIndex];
            }
            else  // Normal contract
            {
                if (comboBoxSuit.SelectedIndex <= 0 || comboBoxDeclarer.SelectedIndex <= 0 || comboBoxTricksTaken.SelectedIndex <= 0)
                {
                    MessageBox.Show(resourceManager.GetString("EnterValidContract"), "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                result.ContractLevel = Convert.ToInt32(comboBoxContractLevel.Text);
                result.ContractSuit = suitsDatabase[comboBoxSuit.SelectedIndex];
                result.ContractX = comboBoxDouble.Text;
                string contract = $"{result.ContractLevel} {result.ContractSuit}";
                if (result.ContractX != "")
                {
                    contract = $"{contract} {result.ContractX}";
                }
                result.Contract = contract;
                result.DeclarerNSEW = declarerDatabase[comboBoxDeclarer.SelectedIndex];
                if (comboBoxLead.SelectedIndex <= 0)  // Lead might not have been selected
                {
                    result.LeadCard = "";
                }
                else
                {
                    result.LeadCard = leadDatabase[comboBoxLead.SelectedIndex];
                }
                result.TricksTaken = comboBoxTricksTaken.Text;
                if (comboBoxRemarks.SelectedIndex <= 0)  // Remarks might not have been selected
                {
                    result.Remarks = "";
                }
                else
                {
                    result.Remarks = remarksDatabase[comboBoxRemarks.SelectedIndex];
                }
            }
            result.UpdateDB(connectionString);
            Close();
        }
    }
}
