using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Graphics;
using Files;

namespace UI
{
    public partial class MainForm : Form
    {
        private readonly AboutProgram _aboutDialog = null;

        public MainForm()
        {
            InitializeComponent();

            openFileDialog.Filter = "Text files(*.txt)|*.txt|Excel files(*.xlsx)| *.xlsx| All files(*.*) | *.*";
            saveFileDialog.Filter = "Text files(*.txt)|*.txt|Excel files(*.xlsx)| *.xlsx| All files(*.*) | *.*";

            dataGridView.RowHeadersVisible = false;
            dataGridView.Columns.Add("0", "X");
            dataGridView.Columns.Add("1", "Y");

            DisplayedGraphic();

            _aboutDialog = new AboutProgram();

            if (Properties.Settings.Default.isShowAboutMenu)
            {
                _aboutDialog.Show();
            }
        }

        private void CoefficientNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            double coefficient = (double)coefficientNumericUpDown.Value;
            
            if (Math.Abs(coefficient) < 0.01)
            {
                coefficientNumericUpDown.Value = 0.01M;
            }

            DisplayedGraphic();
        }

        private void ScaleNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            DisplayedGraphic();
        }

        private void StapNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            DisplayedGraphic();
        }

        private void DisplayedGraphic()
        {
            dataGridView.Rows.Clear();
            chart.Series[0].Points.Clear();

            double a = (double)coefficientNumericUpDown.Value;
            double scale = (double)scaleNumericUpDown.Value;
            double step = (double)stepNumericUpDown.Value;

            PointD[] points = FoliumOfDescartes.PointsArray(a, scale, step);

            DrawGraphic(points);
            FillTable(points);
        }

        private void DrawGraphic(PointD[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                chart.Series[0].Points.AddXY(points[i].X, points[i].Y);
            }
        }

        private void FillTable(PointD[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                dataGridView.Rows.Add();
                dataGridView.Rows[i].Cells[0].Value = points[i].X;
                dataGridView.Rows[i].Cells[1].Value = points[i].Y;
            }
        }

        private void aboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _aboutDialog.Show();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                bool isRead = false;
                string filter = System.IO.Path.GetExtension(openFileDialog.FileName);

                double a = 0;
                double scale = 0;
                double step = 0;

                if (filter == ".xlsx")
                {
                    isRead = ExcelFile.Read(openFileDialog.FileName, out a, out scale, out step);
                }
                else if (filter == ".txt")
                {
                    isRead = TxtFile.Read(openFileDialog.FileName, out a, out scale, out step);
                }

                if (isRead)
                {
                    coefficientNumericUpDown.Value = (decimal)a;
                    scaleNumericUpDown.Value = (decimal)scale;
                    stepNumericUpDown.Value = (decimal)step;                                                                                                 }
                else
                {
                    MessageBox.Show("Файл не может быть прочитан.", "Ошибка!");
                }
            } 
        }

        private void saveDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                double a = (double)coefficientNumericUpDown.Value;
                double scale = (double)scaleNumericUpDown.Value;
                double step = (double)stepNumericUpDown.Value;
                PointD[] points = new PointD[dataGridView.Rows.Count - 1];

                for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
                {
                    points[i].X = (double)dataGridView.Rows[i].Cells[0].Value;
                    points[i].Y = (double)dataGridView.Rows[i].Cells[1].Value;
                }

                bool isWriten = false;
                string filter = System.IO.Path.GetExtension(saveFileDialog.FileName);

                if (filter == ".xlsx")
                {
                    isWriten = ExcelFile.Write(saveFileDialog.FileName, points, a, scale, step);
                }
                else if (filter == ".txt")
                {
                    isWriten = TxtFile.Write(saveFileDialog.FileName, points, a, scale, step);
                }

                if (!isWriten)
                {
                    MessageBox.Show("Невозмоно записать в файл.", "Ошибка!");
                }
            }
        }
    }
}
