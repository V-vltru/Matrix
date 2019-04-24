using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            }
            else
            {
                MessageBox.Show("Параметры матрицы заданы некорректно!");
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
    }
}
