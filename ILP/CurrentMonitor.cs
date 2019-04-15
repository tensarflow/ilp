//#define TCP_FROM_ROBOT
#define UDP_FROM_NODEMCU
#undef TCP_FROM_URSIM

using com.espertech.esper.client;
using com.espertech.esper.client.time;
using com.espertech.esper.compat.container;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;



namespace ILP
{
    /// <summary>
    /// ToDo!!
    /// </summary>
    public class CurrentMonitor
    {
        // Global Variables: -------------------BEGIN
        private EPServiceProvider _epService;
        private ResponseSocket server = null;
        private GUIForm mainForm;

        private double VOLTAGE_ROBOT = 220;
        private double VOLTAGE_PRINTER = 220;
        private string EVENTTYPE_ROBOTCURRENT = "ROBOTCURRENT";
        private string EVENTTYPE_PRINTERCURRENT = "PINTERCURRENT";

        private string LOCAL_IP_ADRESS = "172.17.33.114";

        private List<double> referencePatternRobot;
        private List<double> referencePatternPrinter;
        private SubsequenceDTW dtwRobot;
        private SubsequenceDTW dtwPrinter;
        private double newDataRobot;
        private double newDataPrinter;
        private double newTimeRobot = 0;
        private double newTimePrinter = 0;
        private static Chart chartRobot;
        private static Chart chartPrinter;
        private int textBoxPosY = 0;
        private UDPSocket socketRobotSensor;
        private UDPSocket socketPrinterSensor;

        public int durationLearned;
        public float thresholdLearned;
        public List<double> referencePatternLearned;
        private bool isInitExecuted_Robot = false;
        private bool isInitExecuted_Printer = false;

        // Global Variables: -------------------END

        /// <summary>
        /// ToDo!!
        /// </summary>
        public CurrentMonitor(GUIForm tempForm1)
        {
            mainForm = tempForm1;
            Setup();
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void Setup()
        {
            // get chart of mainForm
            chartRobot = mainForm.chart2;
            chartPrinter = mainForm.chart1;

            // Setup CEP
            var container = ContainerExtensions.CreateDefaultContainer()
                .InitializeDefaultServices()
                .InitializeDatabaseDrivers();

            // Set configuration, important: Here the type of the event is configured!
            var configuration = new Configuration(container);
            configuration.AddEventType("RobotCurrent", typeof(RobotCurrentEvent).FullName);
            configuration.AddEventType("PrinterCurrent", typeof(PrinterCurrentEvent).FullName);
            configuration.AddEventType("DetectedEvent", typeof(DetectedEvent).FullName);

            // Set engine
            _epService = EPServiceProviderManager.GetProvider(container, "IlepTest", configuration);
            _epService.EPRuntime.SendEvent(new TimerControlEvent(TimerControlEvent.ClockTypeEnum.CLOCK_EXTERNAL));

            // Create EPL statement to Plot cleaned average data of ROBOT AND attach listener to it
            EPStatement plotRobotPower;
            string plotRobotPower_stmt = "select avg(AmpereData) as AvgAmpereData, TimeStamp from RobotCurrent.win:length(15)";
            plotRobotPower = _epService.EPAdministrator.CreateEPL(plotRobotPower_stmt);
            plotRobotPower.Events += PlotAndDetectEvent_Robot;

            // Create EPL statement to Plot cleaned average data of PRINTER AND attach listener to it
            EPStatement plotPrinterPower;
            string plotPrinterPower_stmt = "select avg(AmpereData) as AvgAmpereData, TimeStamp from PrinterCurrent.win:length(70)"; // deleted "avg"()
            plotPrinterPower = _epService.EPAdministrator.CreateEPL(plotPrinterPower_stmt);
            plotPrinterPower.Events += PlotAndDetectEvent_Printer;

            // Create EPL statement to calculate cumulative energy of ROBOT AND attach listener to it
            EPStatement showCumEnergyRobot;
            string showCumEnergyRobot_stmt = "select sum(AmpereData) as SumAmpereData from RobotCurrent";
            showCumEnergyRobot = _epService.EPAdministrator.CreateEPL(showCumEnergyRobot_stmt);
            showCumEnergyRobot.Events += CastDataWriteSumEnergyAndCost_Robot;

            // Create EPL statement to calculate cumulative energy of PRINTER AND attach listener to it
            EPStatement showCumEnergyPrinter;
            string showCumEnergyPrinter_stmt = "select sum(AmpereData) as SumAmpereData from PrinterCurrent";
            showCumEnergyPrinter = _epService.EPAdministrator.CreateEPL(showCumEnergyPrinter_stmt);
            showCumEnergyPrinter.Events += CastDataWriteSumEnergyAndCost_Printer;

            // Create EPL statement to write detected sample movement of ROBOT
            EPStatement writeDetectedSampleMovementEvent;
            string writeDetectedSampleMovementEvent_stmt = "select StartTime, EndTime, Duration, UsedEnergy, Cost, EventSource from DetectedEvent";
            writeDetectedSampleMovementEvent = _epService.EPAdministrator.CreateEPL(writeDetectedSampleMovementEvent_stmt);
            writeDetectedSampleMovementEvent.Events += OnDetectedPattern;

            // Create EPL statement to write average energy used for detected event of ROBOT AND PRINTER
            EPStatement writeAvgEnergyDetecteEvent;
            string writeAvgEnergyDetecteEvent_stmt = "select avg(UsedEnergy) as AvgEnergyDetectedEvent, EventSource from DetectedEvent";
            writeAvgEnergyDetecteEvent = _epService.EPAdministrator.CreateEPL(writeAvgEnergyDetecteEvent_stmt);
            writeAvgEnergyDetecteEvent.Events += CastDataForWriteAvgEnergyDetectedEvent;

            // Create EPL statement to write average cost of sample movement of ROBOT AND PRINTER
            EPStatement writeAvgCostDetectedEvent;
            string writeAvgCostDetectedEvent_stmt = "select avg(Cost) as AvgCostDetectedEvent, EventSource from DetectedEvent";
            writeAvgCostDetectedEvent = _epService.EPAdministrator.CreateEPL(writeAvgCostDetectedEvent_stmt);
            writeAvgCostDetectedEvent.Events += CastDataForWriteAvgCostDetectedEvent;
        }

        public void PauseStream()
        {
            socketRobotSensor.EndReceiving();
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public void StartStream()
        {

#if TCP_FROM_URSIM
            InitSocket();
            Thread getDataAndSendToEngineThread = new Thread(getDataAndSendToEngine);
            getDataAndSendToEngineThread.Start();
#endif

#if UDP_FROM_NODEMCU
            socketRobotSensor = new UDPSocket(_epService);
            socketRobotSensor.Server_Robot(LOCAL_IP_ADRESS, 80);

            socketPrinterSensor = new UDPSocket(_epService);
            socketPrinterSensor.Server_Printer(LOCAL_IP_ADRESS, 90);
#endif
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void InitSocket()
        {
            server = new ResponseSocket();
            try
            {
                server.Bind("tcp://localhost:5000");
            }
            catch (Exception err)
            {
                MessageBox.Show("Binding socket not successful. Call support. \n" + err.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void getDataAndSendToEngine()
        {
            while (true)
            {
                // Receive data through socket and send answer
                string message;
                try
                {
                    message = server.ReceiveFrameString();
                    //Console.WriteLine(message);
                    //server.SendFrame("Roger");
                    _epService.EPRuntime.SendEvent(new RobotCurrentEvent(float.Parse(message)));
                }

                catch (Exception e)
                {
                    MessageBox.Show("getDataAndSendToEngine(): " + e.Message, "Error in function getDataAndSendToEngine", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void CastDataWriteSumEnergyAndCost_Robot(Object sender, UpdateEventArgs e)
        {
            foreach (var @event in e.NewEvents)
            {
                double totalEnergy = ((double.Parse(@event.Get("SumAmpereData").ToString()) * VOLTAGE_ROBOT) / 1000) * 0.0002777; // total energy in kWh
                double totalCost = totalEnergy * .4612;

                updateLabelTotalEnergy_Robot(totalEnergy.ToString() + " kWh");
                updateLabelTotalCost_Robot(totalCost.ToString() + " TL");
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void CastDataWriteSumEnergyAndCost_Printer(Object sender, UpdateEventArgs e)
        {
            foreach (var @event in e.NewEvents)
            {
                double totalEnergy = ((double.Parse(@event.Get("SumAmpereData").ToString()) * VOLTAGE_PRINTER) / 1000) * 0.0002777; // total energy in kWh
                double totalCost = totalEnergy * .4612;

                updateLabelTotalEnergy_Printer(totalEnergy.ToString() + " kWh");
                updateLabelTotalCost_Printer(totalCost.ToString() + " TL");
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        delegate void StringArgReturningVoidDelegate(string text);

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void updateLabelTotalEnergy_Robot(string text)
        {
            if (mainForm.label2.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(updateLabelTotalEnergy_Robot);
                mainForm.Invoke(d, new object[] { text });
            }
            else
            {
                mainForm.label2.Text = text;
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void updateLabelTotalEnergy_Printer(string text)
        {
            if (mainForm.label23.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(updateLabelTotalEnergy_Printer);
                mainForm.Invoke(d, new object[] { text });
            }
            else
            {
                mainForm.label23.Text = text;
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void updateLabelTotalCost_Robot(string text)
        {
            if (mainForm.label5.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(updateLabelTotalCost_Robot);
                mainForm.Invoke(d, new object[] { text });
            }
            else
            {
                mainForm.label5.Text = text;
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void updateLabelTotalCost_Printer(string text)
        {
            if (mainForm.label15.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(updateLabelTotalCost_Printer);
                mainForm.Invoke(d, new object[] { text });
            }
            else
            {
                mainForm.label15.Text = text;
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void updateLabelTime(string text)
        {
            if (mainForm.label3.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(updateLabelTime);
                mainForm.Invoke(d, new object[] { text });
            }
            else
            {
                mainForm.label3.Text = text;
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void CastDataForWriteAvgEnergyDetectedEvent(Object sender, UpdateEventArgs e)
        {
            string eventType = "";
            string AvgEnergySampleMovement = "";
            foreach (var @event in e.NewEvents)
            {
                eventType = @event.Get("EventSource").ToString();
                AvgEnergySampleMovement = @event.Get("AvgEnergyDetectedEvent").ToString();
            }

            if (eventType == EVENTTYPE_ROBOTCURRENT)
            {
                updateLabelAvgEnergyDetectedEventRobot(AvgEnergySampleMovement + " kWh");     // Calculations to convert from joule to kWh
            }

            else if (eventType == EVENTTYPE_ROBOTCURRENT)
            {
                updateLabelAvgEnergyDetectedEventPrinter(AvgEnergySampleMovement + " kWh");     // Calculations to convert from joule to kWh
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void updateLabelAvgEnergyDetectedEventRobot(string text)
        {
            if (mainForm.label7.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(updateLabelAvgEnergyDetectedEventRobot);
                mainForm.Invoke(d, new object[] { text });
            }
            else
            {
                mainForm.label7.Text = text;
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void updateLabelAvgEnergyDetectedEventPrinter(string text)
        {
            if (mainForm.label19.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(updateLabelAvgEnergyDetectedEventPrinter);
                mainForm.Invoke(d, new object[] { text });
            }
            else
            {
                mainForm.label19.Text = text;
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void CastDataForWriteAvgCostDetectedEvent(Object sender, UpdateEventArgs e)
        {
            string eventType = "";
            string avgCostSampleMovement = "";

            foreach (var @event in e.NewEvents)
            {
                eventType = @event.Get("EventSource").ToString();
                avgCostSampleMovement = @event.Get("AvgCostDetectedEvent").ToString() + " TL";
            }

            if (eventType == EVENTTYPE_ROBOTCURRENT)
            {
                updateLabelAvgCostDetectedEventRobot(avgCostSampleMovement);
            }

            else if (eventType == EVENTTYPE_PRINTERCURRENT)
            {
                updateLabelAvgCostDetectedEventPrinter(avgCostSampleMovement);
            }

        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void updateLabelAvgCostDetectedEventRobot(string text)
        {
            if (mainForm.label9.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(updateLabelAvgCostDetectedEventRobot);
                mainForm.Invoke(d, new object[] { text });
            }
            else
            {
                mainForm.label9.Text = text;
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void updateLabelAvgCostDetectedEventPrinter(string text)
        {
            if (mainForm.label17.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(updateLabelAvgCostDetectedEventPrinter);
                mainForm.Invoke(d, new object[] { text });
            }
            else
            {
                mainForm.label17.Text = text;
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private List<double> getRefDataRobot()
        {
            List<double> template = new List<double>();
            using (var reader = new StreamReader(@"C:\ref_1.csv"))
            {
                double redData = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    redData = double.Parse(values[0]);

                    template.Add(redData);
                }
            }
            return template;
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private List<double> getRefDataPrinter()
        {
            List<double> template = new List<double>();
            using (var reader = new StreamReader(@"C:\ref_2.csv"))
            {
                double redData = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    redData = double.Parse(values[0]);

                    template.Add(redData);
                }
            }
            return template;
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void writeRefDataRobot(double data)
        {

            String line = data.ToString() + "\n";

            File.AppendAllText(@"C:\Users\teozk\OneDrive\Desktop\ref_realSensorRobot.csv", line);

        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void writeRefDataPrinter(double data)
        {

            String line = data.ToString() + "\n";

            File.AppendAllText(@"C:\Users\teozk\OneDrive\Desktop\ref_realSensorPrinter.csv", line);

        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void InitDetectEventForRobot()
        {
            referencePatternRobot = getRefDataRobot();
            dtwRobot = new SubsequenceDTW(referencePatternLearned, thresholdLearned, durationLearned/2);
            newDataRobot = 0;
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void InitDetectEventForPrinter()
        {
            referencePatternPrinter = getRefDataPrinter();
            dtwPrinter = new SubsequenceDTW(referencePatternPrinter, 1, 6800);
            newDataPrinter = 0;
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void PlotAndDetectEvent_Robot(Object sender, UpdateEventArgs e)
        {
            if (!isInitExecuted_Robot)
            {
                InitDetectEventForRobot();
                isInitExecuted_Robot = true;
            }

            foreach (var @event in e.NewEvents)
            {
                newDataRobot = double.Parse(@event.Get("AvgAmpereData").ToString());
                newTimeRobot = long.Parse(@event.Get("TimeStamp").ToString());
            }

            InvokePlotData_Robot(newTimeRobot, newDataRobot/* * ROBOT_VOLTAGE*/);

            updateLabelTime((newTimeRobot / 1000).ToString() + " s");

            writeRefDataRobot(newDataRobot);

            Subsequence sequence = dtwRobot.compareDataStream(newDataRobot, (int)newTimeRobot);        // Attention!! Converting long to int

            if ((sequence.Status == SubsequenceStatus.Optimal) || (sequence.Status == SubsequenceStatus.NotOptimal))
            {
                SetMarker(sequence, chartRobot);

                double usedEnery_KiloJoule = getUsedEnergyDetectedEvent(sequence.TStart, sequence.TEnd) / 1000;

                double usedEnery_kWh = usedEnery_KiloJoule * 0.0002777;

                double cost = usedEnery_kWh * .4612;

                _epService.EPRuntime.SendEvent(new DetectedEvent(sequence.TStart, sequence.TEnd, usedEnery_kWh, cost, EVENTTYPE_ROBOTCURRENT));
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void PlotAndDetectEvent_Printer(Object sender, UpdateEventArgs e)
        {
            if (!isInitExecuted_Printer)
            {
                InitDetectEventForPrinter();
                isInitExecuted_Printer = true;
            }

            foreach (var @event in e.NewEvents)
            {
                newDataPrinter = double.Parse(@event.Get("AvgAmpereData").ToString());
                newTimePrinter = long.Parse(@event.Get("TimeStamp").ToString());
            }

            InvokePlotData_Printer(newTimePrinter, newDataPrinter/* * PRINTER_VOLTAGE*/);

            //writeRefDataPrinter(newDataPrinter);

            Subsequence sequence = dtwPrinter.compareDataStream(newDataPrinter, (int)newTimePrinter);        // Attention!! Converting long to int

            if ((sequence.Status == SubsequenceStatus.Optimal) || (sequence.Status == SubsequenceStatus.NotOptimal))
            {
                SetMarker(sequence, chartPrinter);

                double usedEnery_KiloJoule = getUsedEnergyDetectedEvent(sequence.TStart, sequence.TEnd) / 1000;

                double usedEnery_kWh = usedEnery_KiloJoule * 0.0002777;

                double cost = usedEnery_kWh * .4612;

                _epService.EPRuntime.SendEvent(new DetectedEvent(sequence.TStart, sequence.TEnd, usedEnery_kWh, cost, EVENTTYPE_PRINTERCURRENT));
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private double getUsedEnergyDetectedEvent(int startTime, int endTime)
        {
            double usedEnergy = 0;

            var startTime_DataPoint = chartRobot.Series[0].Points.Where(point => point.XValue == startTime).ToList();
            int startTime_index = chartRobot.Series[0].Points.IndexOf(startTime_DataPoint[0]);

            var endTime_DataPoint = chartRobot.Series[0].Points.Where(point => point.XValue == startTime).ToList();
            int endTime_index = chartRobot.Series[0].Points.IndexOf(endTime_DataPoint[0]);

            for (int i = startTime_index; i <= endTime_index; i++)
            {
                usedEnergy = usedEnergy + chartRobot.Series[0].Points[i].YValues[0] * VOLTAGE_ROBOT;
            }

            return usedEnergy;
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void SetMarker(Subsequence sequence, Chart chart)
        {
            StripLine detectedPatternMark = new StripLine();
            detectedPatternMark.StripWidth = sequence.TEnd - sequence.TStart;
            detectedPatternMark.BorderColor = System.Drawing.Color.Red;
            detectedPatternMark.BorderWidth = 5;
            detectedPatternMark.BorderDashStyle = ChartDashStyle.Solid;
            detectedPatternMark.IntervalOffset = sequence.TStart;
            detectedPatternMark.BackColor = System.Drawing.Color.Transparent;

            if (chart.InvokeRequired)
            {
                mainForm.Invoke(new MethodInvoker(delegate
                {
                    chart.ChartAreas[0].AxisX.StripLines.Add(detectedPatternMark);
                }));
            }

            else
            {
                chart.ChartAreas[0].AxisX.StripLines.Add(detectedPatternMark);
            }
        }

        /// <summary>
        /// Method to validate cross thread operation
        /// </summary>
        public void InvokePlotData_Robot(double newX, double newY)
        {
            if (chartRobot.InvokeRequired)
            {
                mainForm.Invoke(new MethodInvoker(delegate
                {
                    PlotDataRobot(newX, newY, chartRobot);
                }));
            }

            else
            {
                PlotDataRobot(newX, newY, chartRobot);
            }
        }

        /// <summary>
        /// Method to validate cross thread operation
        /// </summary>
        public void InvokePlotData_Printer(double newX, double newY)
        {
            if (chartPrinter.InvokeRequired)
            {
                mainForm.Invoke(new MethodInvoker(delegate
                {
                    PlotDataPrinter(newX, newY, chartPrinter);
                }));
            }

            else
            {
                PlotDataPrinter(newX, newY, chartPrinter);
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void PlotDataRobot(double newX, double newY, Chart chart)
        {
            int timeToMonitor = 120000;      // Define time in miliseconds

            // Adding new data points
            chart.Series[0].Points.AddXY(newX, newY);

            // Adjust Y & X axis scale

            // Keep a constant number of points by removing them from the left
            while (chart.Series[0].Points[0].XValue < newX - timeToMonitor)
            {
                chart.Series[0].Points.RemoveAt(0);
            }

            // Adjust X axis scale
            chart.ChartAreas[0].AxisX.Minimum = newX - timeToMonitor;
            chart.ChartAreas[0].AxisX.Maximum = newX;

            // Redraw chart
            chart.Invalidate();
        }

        private void PlotDataPrinter(double newX, double newY, Chart chart)
        {
            int timeToMonitor = 45000;      // Define time in miliseconds

            // Adding new data points
            chart.Series[0].Points.AddXY(newX, newY);

            // Adjust Y & X axis scale

            // Keep a constant number of points by removing them from the left
            while (chart.Series[0].Points[0].XValue < newX - timeToMonitor)
            {
                chart.Series[0].Points.RemoveAt(0);
            }

            // Adjust X axis scale
            chart.ChartAreas[0].AxisX.Minimum = newX - timeToMonitor;
            chart.ChartAreas[0].AxisX.Maximum = newX;

            // Redraw chart
            chart.Invalidate();
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void OnDetectedPattern(Object sender, UpdateEventArgs e)
        {
            string startTime = "XX";
            string endTime = "XX";
            string duration = "XX";
            string usedEnergy = "XX";
            string cost = "XX";
            string eventType = "XX";
            foreach (var @event in e.NewEvents)
            {
                startTime = @event.Get("StartTime").ToString();
                endTime = @event.Get("EndTime").ToString();
                duration = @event.Get("Duration").ToString();
                usedEnergy = @event.Get("UsedEnergy").ToString();
                cost = @event.Get("Cost").ToString();
                eventType = @event.Get("EventSource").ToString();
            }
            try
            {
                createSampleMovementDetectedTextBox(startTime, endTime, duration, usedEnergy, cost, eventType);
            }
            catch (Exception err)
            {
                Console.WriteLine("Exceptionnn: " + err.Message);
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void createSampleMovementDetectedTextBox(string startTime, string endTime, string duration, string usedEnergy, string cost, string eventType)
        {
            TextBox textBoxTest = new TextBox();
            textBoxTest.Location = new Point(0, textBoxPosY);
            textBoxTest.Multiline = true;
            textBoxTest.Name = "textBoxTest";
            textBoxTest.ReadOnly = true;
            textBoxTest.ScrollBars = ScrollBars.Horizontal;
            textBoxTest.Size = new Size(250, 95);
            textBoxTest.TabIndex = 4;
            textBoxTest.Text = "New Event Detected: " +
                "\r\nEvent Type: " + eventType +
                "\r\nStarted: " + startTime +
                "\r\nEndend: " + endTime +
                "\r\nDuration:" + duration + " ms" +
                "\r\nUsed Energy: " + usedEnergy + " kWh" +
                "\r\nCost: " + cost + " TL";

            if (mainForm.panel1.InvokeRequired)
            {
                mainForm.panel1.Invoke(new MethodInvoker(delegate
                {
                    mainForm.panel1.Controls.Add(textBoxTest);
                }));
            }

            else
            {
                mainForm.panel1.Controls.Add(textBoxTest);
            }

            textBoxPosY = textBoxPosY + 100;
        }
    }
}
