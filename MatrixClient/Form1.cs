using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MatrixLib;

namespace MatrixClient
{
    public enum Operations
    {
        Sum,
        Multiply
    }

    public partial class Form1 : Form
    {
        public int MatrixARowsCount { get; set; }

        public int MatrixAColumnsCount { get; set; }

        public int MatrixBRowsCount { get; set; }

        public int MatrixBColumnsCount { get; set; }

        public readonly string LabelOperationSum = "+";

        public readonly string LabelOperationMul = "X";

        public readonly string DefaultLabelState = "Статус: ";

        public readonly string LabelStateOk = "операция может быть выполнена.";

        public readonly string LabelStateError = "операция не может быть выполнена.";

        private bool IsOperationApproved = false;

        public Form1()
        {
            InitializeComponent();

            this.SetDefaultLabelState();

            labelOperation.Text = string.Empty;

            dataGridViewA.Enabled = false;
            dataGridViewB.Enabled = false;
            dataGridViewResult.Enabled = false;

            buttonFillRandom.Enabled = false;
            buttonCalculate.Enabled = false;

            labelOperation.Font = new Font("Arial", 36, FontStyle.Bold);
        }

        private void buttonFillMatrixes_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxARows.Text, out int result) && int.TryParse(textBoxAColumns.Text, out result)
                && int.TryParse(textBoxBRows.Text, out result) && int.TryParse(textBoxBColumns.Text, out result)
                && int.Parse(textBoxARows.Text) > 0 && int.Parse(textBoxAColumns.Text) > 0
                && int.Parse(textBoxBRows.Text) > 0 && int.Parse(textBoxBColumns.Text) > 0)
            {
                this.MatrixARowsCount = int.Parse(textBoxARows.Text);
                this.MatrixAColumnsCount = int.Parse(textBoxAColumns.Text);

                this.MatrixBRowsCount = int.Parse(textBoxBRows.Text);
                this.MatrixBColumnsCount = int.Parse(textBoxBColumns.Text);

                dataGridViewA.Enabled = true;
                dataGridViewB.Enabled = true;
                dataGridViewResult.Enabled = true;

                buttonFillRandom.Enabled = true;
                buttonCalculate.Enabled = true;

                this.SetColumnsAndRowsForDatagridView(dataGridViewA, this.MatrixARowsCount, this.MatrixAColumnsCount);
                this.SetColumnsAndRowsForDatagridView(dataGridViewB, this.MatrixBRowsCount, this.MatrixBColumnsCount);
            }
            else
            {
                MessageBox.Show("Параметры матрицы заданы некорректно!");
            }
        }

        private void buttonFillRandom_Click(object sender, EventArgs e)
        {
            Random random = new Random();

            this.SetRandomValuesForGrid(dataGridViewA, dataGridViewB);
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            if (!this.IsOperationApproved)
            {
                MessageBox.Show("Операция не может быть выполнена так как параметры матриц заданы не верно!");
                return;
            }

            if (this.VerifyMatrixParameters(dataGridViewA, dataGridViewB))
            {
                double[,] matrixA = this.GetArrayFromGrid(dataGridViewA);
                double[,] matrixB = this.GetArrayFromGrid(dataGridViewB);

                MatrixT<double> A = new MatrixT<double>(matrixA);
                MatrixT<double> B = new MatrixT<double>(matrixB);

                MatrixT<double> Result = null;

                Operations operation = (Operations)comboBoxOperation.SelectedIndex;
                switch (operation)
                {
                    case Operations.Sum:
                        {
                            Result = A + B;
                            break;
                        }
                    case Operations.Multiply:
                        {
                            Result = A * B;
                            break;
                        }
                    default:
                        {
                            throw new Exception("Операция не установлена");
                        }
                }

                this.SetResultGrid(Result);
            }
            else
            {
                MessageBox.Show("Значения матрицы установлены не верно!");
            }
        }

        private void textBoxARows_TextChanged(object sender, EventArgs e)
        {
            this.SetDefaultLabelState();

            if (this.CheckOperation())
            {
                this.ApproveOperation();
            }
            else
            {
                this.DenyOperation();
            }
        }

        private void comboBoxOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetDefaultLabelState();

            if (this.CheckOperation())
            {
                this.ApproveOperation();
            }
            else
            {
                this.DenyOperation();
            }

            if (comboBoxOperation.SelectedIndex > -1)
            {
                int selectedOperationIdx = comboBoxOperation.SelectedIndex;
                Operations operation = (Operations)selectedOperationIdx;

                switch (operation)
                {
                    case Operations.Multiply:
                        {
                            labelOperation.Text = this.LabelOperationMul;
                            break;
                        }
                    case Operations.Sum:
                        {
                            labelOperation.Text = this.LabelOperationSum;
                            break;
                        }
                    default:
                        {
                            labelOperation.Text = string.Empty;
                            break;
                        }
                }
            }
        }

        private bool CheckOperation()
        {
            if (this.CheckInputVariables())
            {
                int matrixARowsCount = int.Parse(textBoxARows.Text);
                int matrixAColumnsCount = int.Parse(textBoxAColumns.Text);

                int matrixBRowsCount = int.Parse(textBoxBRows.Text);
                int matrixBColumnsCount = int.Parse(textBoxBColumns.Text);

                if (comboBoxOperation.SelectedIndex > -1)
                {
                    int selectedOperationIdx = comboBoxOperation.SelectedIndex;
                    Operations operation = (Operations)selectedOperationIdx;

                    switch (operation)
                    {
                        case Operations.Multiply:
                            {
                                if (matrixAColumnsCount == matrixBRowsCount)
                                {
                                    return true;
                                }

                                break;
                            }
                        case Operations.Sum:
                            {
                                if (matrixARowsCount == matrixBRowsCount && matrixAColumnsCount == matrixBColumnsCount)
                                {
                                    return true;
                                }

                                break;
                            }
                        default:
                            {
                                throw new Exception($"Could not retrieve the operation corectly: Id is: {selectedOperationIdx}");
                            }
                    }
                }
            }

            return false;
        }

        private bool CheckInputVariables()
        {
            if (int.TryParse(textBoxARows.Text, out int result) && int.TryParse(textBoxAColumns.Text, out result)
                && int.TryParse(textBoxBRows.Text, out result) && int.TryParse(textBoxBColumns.Text, out result)
                && int.Parse(textBoxARows.Text) > 0 && int.Parse(textBoxAColumns.Text) > 0
                && int.Parse(textBoxBRows.Text) > 0 && int.Parse(textBoxBColumns.Text) > 0)
            {
                return true;
            }

            return false;
        }

        private bool CheckMatrixSize(DataGridView dataGridView, int rows, int columns)
        {
            if (dataGridView.ColumnCount == columns && dataGridView.RowCount == rows)
            {
                return true;
            }

            return false;
        }

        private void SetDefaultLabelState()
        {
            labelState.ForeColor = Color.Black;
            labelState.Text = DefaultLabelState;

            IsOperationApproved = false;
        }

        private void ApproveOperation()
        {
            labelState.ForeColor = Color.Green;
            labelState.Text = this.DefaultLabelState + this.LabelStateOk;

            IsOperationApproved = true;
        }

        private void DenyOperation()
        {
            labelState.ForeColor = Color.Red;
            labelState.Text = DefaultLabelState + this.LabelStateError;

            IsOperationApproved = false;
        }

        private void SetColumnsAndRowsForDatagridView(DataGridView dataGridView, int rows, int columns)
        {
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            dataGridView.ColumnCount = columns;

            dataGridView.AllowUserToAddRows = false;

            List<string> column = new List<string>();
            for (int i = 0; i < columns; i++)
            {
                column.Add(0.0.ToString());
            }

            for (int i = 0; i < rows; i++)
            {
                dataGridView.Rows.Add(column.ToArray());
            }
        }

        private void SetRandomValuesForGrid(params DataGridView[] dataGridViews)
        {
            Random random = new Random();

            foreach (DataGridView dataGridView in dataGridViews)
            {
                for (int i = 0; i < dataGridView.RowCount; i++)
                {
                    for (int j = 0; j < dataGridView.ColumnCount; j++)
                    {
                        dataGridView[j, i].Value = random.Next(-100, 100).ToString();
                    }
                }
            }
        }

        private bool VerifyMatrixParameters(params DataGridView[] dataGridViews)
        {
            foreach(DataGridView dataGridView in dataGridViews)
            {
                for (int i = 0; i < dataGridView.RowCount; i++)
                {
                    for (int j = 0; j < dataGridView.ColumnCount; j++)
                    {
                        if (!double.TryParse(dataGridView[j,i].Value.ToString(), out double result))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private double[,] GetArrayFromGrid(DataGridView dataGridView)
        {
            double[,] result = new double[dataGridView.RowCount, dataGridView.ColumnCount];

            for (int i = 0; i < dataGridView.RowCount; i++)
            {
                for (int j = 0; j < dataGridView.ColumnCount; j++)
                {
                    result[i, j] = double.Parse(dataGridView[j, i].Value.ToString());
                }
            }

            return result;
        }

        private void SetResultGrid(MatrixT<double> result)
        {
            dataGridViewResult.Rows.Clear();
            dataGridViewResult.Columns.Clear();

            dataGridViewResult.Enabled = true;
            dataGridViewResult.AllowUserToAddRows = false;

            dataGridViewResult.ColumnCount = result.cols;

            for (int i = 0; i < result.rows; i++)
            {
                string[] row = new string[dataGridViewResult.ColumnCount];
                for (int j = 0; j < dataGridViewResult.ColumnCount; j++)
                {
                    row[j] = result[i, j].ToString();                  
                }

                dataGridViewResult.Rows.Add(row);
            }

            dataGridViewResult.ReadOnly = true;
        }
    }
}
