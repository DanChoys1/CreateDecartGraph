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

namespace UI
{
    public partial class MainForm : Form
    {
        private readonly AboutProgram _aboutDialog = null;

        public MainForm()
        {
            InitializeComponent();

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

            PointF[] points = FoliumOfDescartes.PointsArray(a, scale, step);

            for (int i = 0; i < points.Length; i++)
            {
                chart.Series[0].Points.AddXY(points[i].X, points[i].Y);

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

        }

        private void saveDataToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
