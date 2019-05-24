using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ILP
{
    public partial class TrainDataForm : Form
    {
        // Global Variables: -------------------BEGIN
        private TrainMonitor trainMonitor;
        private CurrentMonitor currentMonitor;

        // Global Variables: -------------------END

        public TrainDataForm(CurrentMonitor currentMonitor_tmp)
        {
            InitializeComponent();
            currentMonitor = currentMonitor_tmp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            trainMonitor = new TrainMonitor(this);
            trainMonitor.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            trainMonitor.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            trainMonitor.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            trainMonitor.ClearChart();
        }

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            int xValue = (int)chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
            Console.WriteLine("Clicked Value: " + xValue);
            trainMonitor.SetClickedXValue(xValue);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            trainMonitor.ClearMarker();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            trainMonitor.BeginLearning();
        }

        private void button7_Click(object sender, EventArgs e)
        {

            try
            {
                trainMonitor.CommitReferencePattern();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Caution: Be sure to commit pattern first! \nException thrown: " + exception);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            trainMonitor.DetectWithManualThreshold(Convert.ToSingle(textBox1.Text));
        }

        private void chart3_MouseClick(object sender, MouseEventArgs e)
        {
            float xValue = Convert.ToSingle(chart3.ChartAreas[0].AxisX.PixelPositionToValue(e.X));
            trainMonitor.DetectWithClickedThreshold(xValue);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            currentMonitor.durationLearned = trainMonitor.referencePatternDuration;
            currentMonitor.referencePatternLearned = trainMonitor.referencePatternTraining;
            currentMonitor.thresholdLearned = trainMonitor.OptimalThreshold;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            currentMonitor.durationLearned = trainMonitor.referencePatternDuration;
            currentMonitor.referencePatternLearned = trainMonitor.referencePatternTraining;
            currentMonitor.thresholdLearned = Convert.ToInt32(Convert.ToSingle(textBox1.Text));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            trainMonitor.UpdateStreamPlot();
        }
    }
}
