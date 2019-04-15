using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetMQ.Sockets;
using NetMQ;
using rtChart;
using com.espertech.esper.client;
using com.espertech.esper.compat.container;
using com.espertech.esper.client.time;
using com.espertech.esper.collection;
using com.espertech.esper.client.scopetest;
using System.Net;
using System.Net.Sockets;

namespace ILP
{
    /// <summary>
    /// ToDo
    /// </summary>
    public partial class GUIForm : Form
    {
        // Global variables
        Robot robot = new Robot();
        CurrentMonitor currentMonitor;
        string FORM_TYPE = "GUIForm";

        /// <summary>
        /// ToDo
        /// </summary>
        public GUIForm()
        {
            InitializeComponent();
            Logger.form1 = this;
            Logger.textBox = textBox1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread robotInitializeThread = new Thread(robot.initialize);
            robotInitializeThread.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread robotMovementThread = new Thread(robot.moveSample);
            robotMovementThread.Start();    
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Logger.updateLogFile("\r\nSetting up CEP Engine...", false);
            currentMonitor = new CurrentMonitor(this);
            Logger.updateLogFile("\r\nFinished!", false);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TrainDataForm trainDataForm = new TrainDataForm(currentMonitor);
            trainDataForm.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            currentMonitor.PauseStream();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            currentMonitor.StartStream();
        }
    }
}
