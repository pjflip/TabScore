using System;
using System.Drawing;
using System.Windows.Forms;

namespace TabScoreStarter
{
    public partial class ViewResultsForm : Form
    {
        private ResultsList resultsList;
        private readonly string connectionString;

        public ViewResultsForm(string connectionString, Point location)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            Location = location;
        }

        private void ViewResultsForm_Load(object sender, EventArgs e)
        {
            if (connectionString == null) return;
            resultsList = new ResultsList(connectionString);
            dataGridViewResults.AutoGenerateColumns = false;
            // dataGridViewResults.DataSource = resultsList;
            dataGridViewResults.Columns[0].DataPropertyName = "Section";
            dataGridViewResults.Columns[1].DataPropertyName = "Table";
            dataGridViewResults.Columns[2].DataPropertyName = "Round";
            dataGridViewResults.Columns[3].DataPropertyName = "Board";
            dataGridViewResults.Columns[4].DataPropertyName = "PairNS";
            dataGridViewResults.Columns[5].DataPropertyName = "PairEW";
            foreach (Result result in resultsList)
            {
                dataGridViewResults.Rows.Add(result.Section, result.Table, result.Round, result.Board, result.PairNS, result.PairEW);
            }
            if (dataGridViewResults.SelectedRows.Count == 0) EditResultButton.Enabled = false;
        }

        private void EditResultButton_Click(object sender, EventArgs e)
        {
            if (dataGridViewResults.SelectedRows.Count == 0) return;
            int selectedRow = dataGridViewResults.SelectedRows[0].Index;
            EditResultForm editResultForm = new EditResultForm(resultsList[selectedRow], resultsList.IsIndividual, connectionString, new Point(Location.X + 30, Location.Y + 30));
            editResultForm.ShowDialog();
        }

        private void DataGridViewResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewResults.SelectedRows.Count > 0) EditResultButton.Enabled = true;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
