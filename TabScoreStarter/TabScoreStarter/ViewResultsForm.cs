using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace TabScoreStarter
{
    public partial class ViewResultsForm : Form
    {
        private ResultsList resultsList;
        private readonly string connectionString;
        private readonly ResourceManager resourceManager;

        public ViewResultsForm(string connectionString, Point location)
        {
            resourceManager = new ResourceManager("TabScoreStarter.Strings", typeof(TabScoreForm).Assembly);
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
            dataGridViewResults.Columns[0].DataPropertyName = resourceManager.GetString("Section");
            dataGridViewResults.Columns[1].DataPropertyName = resourceManager.GetString("Table");
            dataGridViewResults.Columns[2].DataPropertyName = resourceManager.GetString("Round");
            dataGridViewResults.Columns[3].DataPropertyName = resourceManager.GetString("Board");
            dataGridViewResults.Columns[4].DataPropertyName = resourceManager.GetString("PairNS");
            dataGridViewResults.Columns[5].DataPropertyName = resourceManager.GetString("PairEW");
            foreach (Result result in resultsList)
            {
                dataGridViewResults.Rows.Add(result.SectionLetter, result.Table, result.Round, result.Board, result.PairNS, result.PairEW);
            }
            dataGridViewResults.Sort(dataGridViewResults.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
            if (dataGridViewResults.SelectedRows.Count == 0) EditResultButton.Enabled = false;
        }

        private void EditResultButton_Click(object sender, EventArgs e)
        {
            if (dataGridViewResults.SelectedRows.Count == 0) return;
            DataGridViewCellCollection selectedRowCells = dataGridViewResults.SelectedRows[0].Cells;
            Result selectedResult = resultsList.Find(x =>
                x.SectionLetter == Convert.ToString(selectedRowCells[0].Value) &&
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
            string sectionLetter1 = Convert.ToString(dataGridViewResults.Rows[e.RowIndex1].Cells[0].Value);
            string sectionLetter2 = Convert.ToString(dataGridViewResults.Rows[e.RowIndex2].Cells[0].Value);
            e.SortResult = sectionLetter1.CompareTo(sectionLetter2);
            if (e.SortResult == 0 )
            {
                if (e.CellValue1 is string)
                {
                    string cellValue1 = Convert.ToString(e.CellValue1);
                    string cellValue2 = Convert.ToString(e.CellValue2);
                    e.SortResult = cellValue1.CompareTo(cellValue2);
                }
                else
                {
                    int cellValue1 = Convert.ToInt32(e.CellValue1);
                    int cellValue2 = Convert.ToInt32(e.CellValue2);
                    e.SortResult = cellValue1.CompareTo(cellValue2);
                }
                if (e.SortResult == 0)
                {
                    int board1 = Convert.ToInt32(dataGridViewResults.Rows[e.RowIndex1].Cells[3].Value);
                    int board2 = Convert.ToInt32(dataGridViewResults.Rows[e.RowIndex2].Cells[3].Value);
                    e.SortResult = board1.CompareTo(board2);
                    if (e.SortResult == 0 )
                    {
                        int pairNS1 = Convert.ToInt32(dataGridViewResults.Rows[e.RowIndex1].Cells[4].Value);
                        int pairNS2 = Convert.ToInt32(dataGridViewResults.Rows[e.RowIndex2].Cells[4].Value);
                        e.SortResult = pairNS1.CompareTo(pairNS2);
                    }
                }
            }
            e.Handled = true;
        }
    }
}
