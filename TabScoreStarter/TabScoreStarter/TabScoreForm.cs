using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TabScoreStarter
{
    public partial class TabScoreForm : Form
    {
        public TabScoreForm()
        {
            InitializeComponent();
        }

        private void TabScoreForm_Load(object sender, EventArgs e)
        {
            String argsString = "", pathToDB = "";
            String[] arguments = Environment.GetCommandLineArgs();

            // Parse command line args correctly to get DB path
            foreach (string s in arguments)
            {
                argsString = argsString + s + " ";
            }
            arguments = argsString.Split(new Char[] { '/' });
            foreach (string s in arguments)
            {
                if (s.StartsWith("f:[")) pathToDB = s.Split(new char[] { '[', ']' })[1];
            }
            lblDBName.Text = pathToDB;
            if (DB.TestDBConnection(lblDBName.Text))
            {
                btnSDB.Visible = false;
                btnStart.Enabled = true;
                btnStart.BackColor = Color.FromName("Green");
                btnHRF.Enabled = true;
            }
        }

        private void btnSDB_Click(object sender, EventArgs e)
        {
            fd.Filter = "BWS Files (*.bws)|*.bws";
            fd.FilterIndex = 1;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                lblDBName.Text = fd.FileName;
                lblHandRecord.Text = "";
                if (DB.TestDBConnection(lblDBName.Text))
                {
                    btnStart.Enabled = true;
                    btnStart.BackColor = Color.FromName("Green");
                    btnHRF.Enabled = true;
                }
                else
                {
                    btnStart.Enabled = false;
                    btnStart.BackColor = Color.FromName("Red");
                    btnHRF.Enabled = false;
                }
            }
        }

        private void btnHRF_Click(object sender, EventArgs e)
        {
            fd.Filter = "PBN Files (*.pbn)|*.pbn";
            fd.FilterIndex = 1;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                lblHandRecord.Text = fd.FileName;
                List<HandClass> handList = PBNRead.ReadFile(fd.FileName);
                DB.WriteHandsToDB(lblDBName.Text, handList);
            }
        }

        private void TabScoreForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            string pathToTabScoreDB = Environment.ExpandEnvironmentVariables(@"%Public%\TabScore\TabScoreDB.txt");
            System.IO.File.WriteAllText(pathToTabScoreDB, "");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string pathToTabScoreDB = Environment.ExpandEnvironmentVariables(@"%Public%\TabScore\TabScoreDB.txt");
            System.IO.File.WriteAllText(pathToTabScoreDB, lblDBName.Text);
            lblServerState.Text = "Session Running";
            lblServerState.ForeColor = Color.FromName("Green");
            btnSDB.Enabled = false;
            btnStart.Enabled = false;
            btnStart.BackColor = Color.FromName("Gray");
        }
    }
}
