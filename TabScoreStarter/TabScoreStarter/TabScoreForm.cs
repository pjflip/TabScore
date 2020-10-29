using System;
using System.ComponentModel;
using System.Drawing;
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

            Database db = new Database(pathToDB);
            if (pathToDB == "" || !db.Initialize())
            {
                AddDatabaseFileButton.Visible = true;   // No valid database in arguments
            }
            else
            {
                SetDBFilePath(pathToDB);
                SessionStatusLabel.Text = "Session Running";
                SessionStatusLabel.ForeColor = Color.Green;
                OptionsButton.Visible = true;

                HandsList handsList = new HandsList(db);
                if (handsList.Count == 0)
                {
                    AddHandRecordFileButton.Visible = true;    // No hand records in database, so let user add them
                }
                else
                {
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
                Database db = new Database(pathToDB);
                if (db.Initialize())
                {
                    AddDatabaseFileButton.Enabled = false;
                    SetDBFilePath(pathToDB);
                    SessionStatusLabel.Text = "Session Running";
                    SessionStatusLabel.ForeColor = Color.Green;
                    OptionsButton.Visible = true;

                    HandsList handsList = new HandsList(db);
                    if (handsList.Count == 0)
                    {
                        AddHandRecordFileButton.Visible = true;    // No hand records in database, so let user add them
                    }
                    else
                    {
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
                    handsList.WriteToDB(new Database(PathToDBLabel.Text));
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
            ClearDBFilePath();
        }

        public void SetDBFilePath(string pathToDB)
        {
            PathToDBLabel.Text = pathToDB;
            string pathToTabScoreDB = Environment.ExpandEnvironmentVariables(@"%Public%\TabScore\TabScoreDB.txt");
            System.IO.File.WriteAllText(pathToTabScoreDB, pathToDB);
        }

        public void ClearDBFilePath()
        {
            PathToDBLabel.Text = "";
            string pathToTabScoreDB = Environment.ExpandEnvironmentVariables(@"%Public%\TabScore\TabScoreDB.txt");
            System.IO.File.WriteAllText(pathToTabScoreDB, "");
        }

        private void AnalysisCalculation_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Database db = new Database(PathToDBLabel.Text);
            HandsList handsList = new HandsList(db);
            HandEvaluationsList handEvaluationsList = new HandEvaluationsList(db);
            int counter = 0;
            foreach (Hand hand in handsList)
            {
                HandEvaluation handEvaluation = new HandEvaluation(hand);
                handEvaluationsList.Add(handEvaluation);
                counter++;
                worker.ReportProgress((int)((float)counter / (float)handsList.Count * 100.0));
            }
            handEvaluationsList.WriteToDB();
        }

        private void AnalysisCalculation_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            AnalysingProgressBar.Value = e.ProgressPercentage;
        }

        private void AnalysisCalculation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AnalysingProgressBar.Value = 100;
            AnalysingLabel.Text = "Analysis Complete";
        }

        private void OptionsButton_Click(object sender, EventArgs e)
        {
            OptionsForm frmOptions = new OptionsForm
            {
                Tag = PathToDBLabel.Text
            };
            frmOptions.ShowDialog();
        }
    }
}
