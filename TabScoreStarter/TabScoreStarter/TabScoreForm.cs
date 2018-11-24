using System;
using System.ComponentModel;
using System.Data.Odbc;
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
            if (pathToDB == "" || !DataBase.ConnectionOK(SetDBConnectionString(pathToDB)))
            {
                btnAddSDBFile.Visible = true;   // No valid database in arguments
            }
            else
            {
                SetDBFilePath(pathToDB);
                lblSessionStatus.Text = "Session Running";
                lblSessionStatus.ForeColor = Color.Green;

                if (DataBase.InitializeHandRecords(SetDBConnectionString(pathToDB)))
                {
                    btnAddHandRecordFile.Visible = true;    // No hand records in database, so let user add them
                }
                else
                {
                    lblPathToHandRecordFile.Text = "Included in Scoring Database";
                    lblDDAnalysing.Text = "Analysing...";
                    lblDDAnalysing.Visible = true;
                    pbDDAnalysing.Visible = true;
                    bwAnalysisCalculation.RunWorkerAsync();
                }
            }
        }

        private void btnAddSDBFile_Click(object sender, EventArgs e)
        {
            if (fdSDBFileDialog.ShowDialog() == DialogResult.OK)
            {
                string pathToDB = fdSDBFileDialog.FileName;
                string DBConnection = SetDBConnectionString(pathToDB);
                if (DataBase.ConnectionOK(DBConnection))
                {
                    btnAddSDBFile.Enabled = false;
                    SetDBFilePath(pathToDB);
                    lblSessionStatus.Text = "Session Running";
                    lblSessionStatus.ForeColor = Color.Green;

                    if (DataBase.InitializeHandRecords(DBConnection))
                    {
                        btnAddHandRecordFile.Visible = true;    // No hand records in database, so let user add them
                    }
                    else
                    {
                        lblPathToHandRecordFile.Text = "Included in Scoring Database";
                        lblDDAnalysing.Text = "Analysing...";
                        lblDDAnalysing.Visible = true;
                        pbDDAnalysing.Visible = true;
                        bwAnalysisCalculation.RunWorkerAsync();
                    }
                }
            }
        }

        private void btnAddHandRecordFile_Click(object sender, EventArgs e)
        {
            if (fdHRFFileDialog.ShowDialog() == DialogResult.OK)
            {
                lblPathToHandRecordFile.Text = fdHRFFileDialog.FileName;
                HandsList handsList = new HandsList();
                handsList.ReadFromPBNFile(fdHRFFileDialog.FileName);
                if (handsList.Hands == null)
                {
                    MessageBox.Show("File contains no hand records", "TabScoreStarter", MessageBoxButtons.OK);
                }
                else
                {
                    string DBConnection = SetDBConnectionString(lblPathToDB.Text);
                    handsList.WriteToDB(DBConnection);
                    btnAddHandRecordFile.Enabled = false;
                    lblDDAnalysing.Text = "Analysing...";
                    lblDDAnalysing.Visible = true;
                    pbDDAnalysing.Visible = true;
                    bwAnalysisCalculation.RunWorkerAsync();
                }
            }
        }

        private void TabScoreForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClearDBFilePath();
        }

        private string SetDBConnectionString(string pathToDB)
        {
            OdbcConnectionStringBuilder cs = new OdbcConnectionStringBuilder();
            cs.Driver = "Microsoft Access Driver (*.mdb)";
            cs.Add("Dbq", pathToDB);
            cs.Add("Uid", "Admin");
            return cs.ToString();
        }

        public void SetDBFilePath(string pathToDB)
        {
            lblPathToDB.Text = pathToDB;
            string pathToTabScoreDB = Environment.ExpandEnvironmentVariables(@"%Public%\TabScore\TabScoreDB.txt");
            System.IO.File.WriteAllText(pathToTabScoreDB, pathToDB);
        }

        public void ClearDBFilePath()
        {
            lblPathToDB.Text = "";
            string pathToTabScoreDB = Environment.ExpandEnvironmentVariables(@"%Public%\TabScore\TabScoreDB.txt");
            System.IO.File.WriteAllText(pathToTabScoreDB, "");
        }

        private void bwAnalysisCalculation_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            HandsList handsList = new HandsList();
            string DBConnection = SetDBConnectionString(lblPathToDB.Text);
            handsList.ReadFromDB(DBConnection);
            DataBase.InitializeHandEvaluations(DBConnection);
            HandEvaluationsList handEvaluationList = new HandEvaluationsList();
            int counter = 0;
            foreach (Hand hand in handsList.Hands)
            {
                HandEvaluation handEvaluation = new HandEvaluation();
                handEvaluation.SetFromHand(hand);
                handEvaluationList.HandEvaluations.Add(handEvaluation);
                counter++;
                worker.ReportProgress((int)((float)counter / (float)handsList.Hands.Count * 100.0));
            }
            handEvaluationList.WriteToDB(DBConnection);
        }

        private void bwAnalysisCalculation_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbDDAnalysing.Value = e.ProgressPercentage;
        }

        private void bwAnalysisCalculation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbDDAnalysing.Value = 100;
            lblDDAnalysing.Text = "Analysis Complete";
        }
    }
}
