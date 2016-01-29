using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace SimplexMethod_WindowsFormsApplication
{
    public partial class SimplexMethodForm : Form
    {
        public SimplexTable simplexTable;

        public SimplexMethodForm()
        {
            InitializeComponent();
            simplexTable = new SimplexTable();
            PrepareGrid();
        }

        void FillGridFromMatrix()
        {
            for (int i = 0; i < simplexTable.rowsCount; i++)
                for (int j = 0; j < simplexTable.colsCount; j++)
                    dataGridView1.Rows[i].Cells[j].Value =
                        String.Format("{0:0.00}", simplexTable.matrix[i][j]);
        }

        void FillGridWithSolvedMatrix()
        {
            // print matrix
            for (int i = 0; i < simplexTable.rowsCount; i++)
                for (int j = 0; j < simplexTable.colsCount; j++)
                    dataGridView1.Rows[i].Cells[j].Value =
                        String.Format("{0:0.00}", simplexTable.matrix[i][j]);
            // print non-basis vector
            for (int j = 0; j < simplexTable.colsCount; j++)
                dataGridView1.Rows[simplexTable.rowsCount].Cells[j].Value
                    = simplexTable.nonBasisVector[j];
            // print basis vector
            for (int j = 0; j < simplexTable.rowsCount; j++)
                dataGridView1.Rows[j].Cells[simplexTable.colsCount].Value
                    = simplexTable.basisVector[j];
        }

        void ClearAllCells()
        {
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    dataGridView1.Rows[j].Cells[i].Value = null;
        }

        void PrepareGrid()
        {
            dataGridView1.ColumnCount = 26;
            char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            for (int i = 0; i < 26; i++)
                dataGridView1.Columns[i].Name = chars[i].ToString();
            dataGridView1.RowCount = 100;
            dataGridView1.RowHeadersWidth = 50;
            foreach (DataGridViewRow row in dataGridView1.Rows)
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
        }


        // menu items
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearAllCells();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                string fileName = openFileDialog1.FileName;
                try
                {
                    simplexTable.ReadData(fileName);
                    FillGridFromMatrix();

                }
                catch (IOException) { }
            }
        }

        private void solveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // find colCount
            int i = 0;
            while (dataGridView1.Rows[0].Cells[i].Value != null)
            {
                simplexTable.colsCount = i + 1;
                i++;
            }
            // find rowCount
            i = 0;
            while (dataGridView1.Rows[i].Cells[0].Value != null)
            {
                simplexTable.rowsCount = i + 1;
                i++;
            }

            // read matrix from grid
            double[][] matrix = simplexTable.MatrixInitHelper();
            for (i = 0; i < simplexTable.rowsCount; i++)
                for (int j = 0; j < simplexTable.colsCount; j++)
                    matrix[i][j] = Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value);

            simplexTable.CreateHelperVectors();
            // Solve
            simplexTable.matrix = matrix;
            simplexTable.Solve();

            // return to grid 
            FillGridWithSolvedMatrix();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string fileName = saveFileDialog1.FileName;
            // write solution to file
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine(simplexTable.colsCount);
                sw.WriteLine(simplexTable.rowsCount);
                for (int i = 0; i < simplexTable.rowsCount; i++)
                {
                    for (int j = 0; j < simplexTable.colsCount; j++)
                    {
                        sw.Write("{0:0.00} ", simplexTable.matrix[i][j]);
                        // write basis helper vector
                        if (j == simplexTable.colsCount - 1)
                            sw.Write(simplexTable.basisVector[i]);
                    }
                    sw.WriteLine();
                }
                // write non-basis helper vector
                for (int i = 0; i < simplexTable.colsCount; i++)
                    sw.Write(simplexTable.nonBasisVector[i] + " ");
            }
        }

        private void refreshTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearAllCells();
        }
    }
}
