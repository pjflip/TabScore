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
            dataGridViewResults.SortCompare += new DataGridViewSortCompareEventHandler(this.DataGridViewResults_SortCompare);
            this.connectionString = connectionString;
            Location = location;
        }

        private void ViewResultsForm_Load(object sender, EventArgs e)
        {
            if (connectionString == null) return;
            resultsList = new ResultsList(connectionString);
            dataGridViewResults.AutoGenerateColumns = false;
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
            dataGridViewResults.Sort(dataGridViewResults.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
            if (dataGridViewResults.SelectedRows.Count == 0) EditResultButton.Enabled = false;
        }

        private void EditResultButton_Click(object sender, EventArgs e)
        {
            if (dataGridViewResults.SelectedRows.Count == 0) return;
            DataGridViewCellCollection selectedRowCells = dataGridViewResults.SelectedRows[0].Cells;
            Result selectedResult = resultsList.Find(x =>
                x.Section == Convert.ToInt32(selectedRowCells[0].Value) &&
                x.Table   == Convert.ToInt32(selectedRowCells[1].Value) &&
                x.Round   == Convert.ToInt32(selectedRowCells[2].Value) &&
                x.Board   == Convert.ToInt32(selectedRowCells[3].Value) &&
                x.PairNS  == Convert.ToInt32(selectedRowCells[4].Value) &&
                x.PairEW  == Convert.ToInt32(selectedRowCells[5].Value)
                );
            EditResultForm editResultForm = new EditResultForm(selectedResult, connectionString, new Point(Location.X + 30, Location.Y + 30));
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

        private void DataGridViewResults_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            // Compares based on Section, clicked column, Board, PairNS in that order
            int section1 = Convert.ToInt32(dataGridViewResults.Rows[e.RowIndex1].Cells[0].Value);
            int cellValue1 = Convert.ToInt32(e.CellValue1);
            int board1 = Convert.ToInt32(dataGridViewResults.Rows[e.RowIndex1].Cells[3].Value);
            int pairNS1 = Convert.ToInt32(dataGridViewResults.Rows[e.RowIndex1].Cells[4].Value);
            int compareValue1 = 1000000 * section1 + 10000 * cellValue1 + 100 * board1 + pairNS1;
            int section2 = Convert.ToInt32(dataGridViewResults.Rows[e.RowIndex2].Cells[0].Value);
            int cellValue2 = Convert.ToInt32(e.CellValue2);
            int board2 = Convert.ToInt32(dataGridViewResults.Rows[e.RowIndex2].Cells[3].Value);
            int pairNS2 = Convert.ToInt32(dataGridViewResults.Rows[e.RowIndex2].Cells[4].Value);
            int compareValue2 = 1000000 * section2 + 10000 * cellValue2 + 100 * board2 + pairNS2;
            e.SortResult = compareValue1.CompareTo(compareValue2);
            e.Handled = true;
        }
    }
}
