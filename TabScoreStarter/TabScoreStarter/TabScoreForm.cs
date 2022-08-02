using System;
using System.ComponentModel;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Reflection;
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
            Text = $"TabScoreStarter - Version {Assembly.GetExecutingAssembly().GetName().Version}";
            
            string argsString = "", pathToDB = "";
            string[] arguments = Environment.GetCommandLineArgs();

            // Parse command line args correctly to get DB path
            foreach (string s in arguments)
            {
                argsString = argsString + s + " ";
            }
            arguments = argsString.Split(new Char[] { '/' });
            foreach (string s in arguments)
            {
                if (s.StartsWith("f:["))
                {
                    pathToDB = s.Split(new char[] { '[', ']' })[1];
                    break;
                }
            }

            if (pathToDB != "" && !File.Exists(pathToDB))
            {
                MessageBox.Show("Database passed in parameter string either does not exist or is not accessible", "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pathToDB = "";
            }

            OdbcConnectionStringBuilder connectionString = null;
            if (pathToDB != "") connectionString = Database.ConnectionString(pathToDB);
            if (!Database.Initialize(connectionString))
            {
                AddDatabaseFileButton.Visible = true;   // No valid database in arguments
            }
            else
            {
                SetDBFile(pathToDB);
                SessionStatusLabel.Text = "Session Running";
                SessionStatusLabel.ForeColor = Color.Green;
                OptionsButton.Visible = true;
                AddHandRecordFileButton.Visible = true;
                HandsList handsList = new HandsList(connectionString);
                if (handsList.Count > 0)
                {
                    AddHandRecordFileButton.Enabled = false;
                    PathToHandRecordFileLabel.Text = "Included in Scoring Database";
                    AnalysingLabel.Text = "Analysing...";
                    AnalysingLabel.Visible = true;
                    AnalysingProgressBar.Visible = true;
                    AnalysisCalculationBackgroundWorker.RunWorkerAsync();
                }
            }
        }

        private void AddDatabaseFileButton_Click(object sender, EventArgs e)
        {
            if (DatabaseFileDialog.ShowDialog() == DialogResult.OK)
            {
                string pathToDB = DatabaseFileDialog.FileName;
                OdbcConnectionStringBuilder connectionString = Database.ConnectionString(pathToDB);
                if (Database.Initialize(connectionString))
                {
                    AddDatabaseFileButton.Enabled = false;
                    SetDBFile(pathToDB);
                    SessionStatusLabel.Text = "Session Running";
                    SessionStatusLabel.ForeColor = Color.Green;
                    OptionsButton.Visible = true;
                    AddHandRecordFileButton.Visible = true;
                    HandsList handsList = new HandsList(connectionString);
                    if (handsList.Count > 0)
                    {
                        AddHandRecordFileButton.Enabled = false;
                        PathToHandRecordFileLabel.Text = "Included in Scoring Database";
                        AnalysingLabel.Text = "Analysing...";
                        AnalysingLabel.Visible = true;
                        AnalysingProgressBar.Visible = true;
                        AnalysisCalculationBackgroundWorker.RunWorkerAsync();
                    }
                }
            }
        }

        private void AddHandRecordFileButton_Click(object sender, EventArgs e)
        {
            if (HandRecordFileDialog.ShowDialog() == DialogResult.OK)
            {
                PathToHandRecordFileLabel.Text = HandRecordFileDialog.FileName;
                HandsList handsList = new HandsList(HandRecordFileDialog.FileName);
                if (handsList.Count == 0)
                {
                    MessageBox.Show("File contains no hand records", "TabScoreStarter", MessageBoxButtons.OK);
                }
                else
                {
                    handsList.WriteToDB(Database.ConnectionString(PathToDBLabel.Text));
                    AddHandRecordFileButton.Enabled = false;
                    AnalysingLabel.Text = "Analysing...";
                    AnalysingLabel.Visible = true;
                    AnalysingProgressBar.Visible = true;
                    AnalysisCalculationBackgroundWorker.RunWorkerAsync();
                }
            }
        }

        private void TabScoreForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClearDBFile();
        }

        public void SetDBFile(string pathToDB)
        {
            PathToDBLabel.Text = pathToDB;
            string pathToTabScoreDB = Environment.ExpandEnvironmentVariables(@"%Public%\TabScore\TabScoreDB.txt");
            System.IO.File.WriteAllText(pathToTabScoreDB, Database.ConnectionString(pathToDB).ToString());
        }

        public void ClearDBFile()
        {
            PathToDBLabel.Text = "";
            string pathToTabScoreDB = Environment.ExpandEnvironmentVariables(@"%Public%\TabScore\TabScoreDB.txt");
            System.IO.File.WriteAllText(pathToTabScoreDB, "");
        }

        private void AnalysisCalculation_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            OdbcConnectionStringBuilder connectionString = Database.ConnectionString(PathToDBLabel.Text);
            HandsList handsList = new HandsList(connectionString);
            HandEvaluationsList handEvaluationsList = new HandEvaluationsList();
            int counter = 0;
            foreach (Hand hand in handsList)
            {
                HandEvaluation handEvaluation = new HandEvaluation(hand);
                handEvaluationsList.Add(handEvaluation);
                counter++;
                worker.ReportProgress((int)((float)counter / (float)handsList.Count * 100.0));
            }
            handEvaluationsList.WriteToDB(connectionString);
        }

        private void AnalysisCalculation_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            AnalysingProgressBar.Value = e.ProgressPercentage;
        }

        private void AnalysisCalculation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AnalysingProgressBar.Value = 100;
            AnalysingLabel.Text = "Analysis Complete";
            AddHandRecordFileButton.Text = "Change hand record file...";
            AddHandRecordFileButton.Enabled = true;
        }

        private void OptionsButton_Click(object sender, EventArgs e)
        {
            Point mainFormLocation = Location;
            OptionsForm frmOptions = new OptionsForm
            {
                Tag = PathToDBLabel.Text,
                Location = new Point(mainFormLocation.X + 30, mainFormLocation.Y + 30)
            };
            frmOptions.ShowDialog();
        }
    }
}
