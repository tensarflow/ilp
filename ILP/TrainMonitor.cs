using com.espertech.esper.client;
using com.espertech.esper.client.time;
using com.espertech.esper.compat.container;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ILP
{
    class TrainMonitor
    {
        // Global Variables: -------------------BEGIN
        private TrainDataForm trainDataForm;
        private static Chart chartTrainingData;
        private EPServiceProvider _epService;
        private string LOCAL_IP_ADRESS = "172.17.33.114";
        private double newDataTrain;
        private double newTimeTrain = 0;
        private UDPSocket socketTrainVirtSensor;
        public List<double> referencePatternTraining;
        private int referencePatternBeginIndex = 0;
        private int referencePatternEndIndex = 0;
        public int referencePatternDuration;
        private SubsequenceDTW dtwTraining;
        public float OptimalThreshold = 0;

        // Global Variables: -------------------END

        public TrainMonitor(TrainDataForm tempForm)
        {
            trainDataForm = tempForm;
            Setup();
        }

        private void Setup()
        {
            // Get chart from training form
            chartTrainingData = trainDataForm.chart1;

            // Setup CEP
            var container = ContainerExtensions.CreateDefaultContainer().InitializeDefaultServices().InitializeDatabaseDrivers();

            // Set configuration, important: Here the type of the event is configured!
            var configuration = new Configuration(container);
            configuration.AddEventType("TrainCurrent", typeof(TrainCurrentEvent).FullName);

            // Set engine
            _epService = EPServiceProviderManager.GetProvider(container, "TrainingEngine", configuration);
            _epService.EPRuntime.SendEvent(new TimerControlEvent(TimerControlEvent.ClockTypeEnum.CLOCK_EXTERNAL));

            // Create EPL statement to Plot cleaned average data of ROBOT AND attach listener to it
            EPStatement plotTrainPower;
            string plotTrainPower_stmt = "select avg(AmpereData) as AvgAmpereData, TimeStamp from TrainCurrent.win:length(15)";
            plotTrainPower = _epService.EPAdministrator.CreateEPL(plotTrainPower_stmt);
            plotTrainPower.Events += PlotEvent_Train;
        }

        public void DetectWithClickedThreshold(float xValue)
        {
            trainDataForm.chart3.ChartAreas[0].AxisX.StripLines.Clear();

            StripLine thresholdMarker = new StripLine();
            thresholdMarker.BorderColor = System.Drawing.Color.Red;
            thresholdMarker.BorderWidth = 5;
            thresholdMarker.BorderDashStyle = ChartDashStyle.Solid;
            thresholdMarker.BackColor = System.Drawing.Color.Red;
            thresholdMarker.StripWidth = 0.25;
            thresholdMarker.IntervalOffset = xValue;
            trainDataForm.chart3.ChartAreas[0].AxisX.StripLines.Add(thresholdMarker);

            trainDataForm.textBox1.Text = Convert.ToString(xValue);

            DetectEvent(xValue);

        }

        public void UpdateStreamPlot()
        {
            chartTrainingData.Invalidate();
        }

        public void DetectWithManualThreshold(float threshold)
        {
            int noOfDetectedPatterns = DetectEvent(threshold);
            MarkThreshold(threshold);
        }

        public void Start()
        {
            socketTrainVirtSensor = new UDPSocket(_epService);
            socketTrainVirtSensor.Server_Train(LOCAL_IP_ADRESS, 80);
        }

        public void Stop()
        {
            socketTrainVirtSensor.EndReceiving();
        }

        /// <summary>
        /// Event that happens when new data arrives in engine
        /// </summary>
        private void PlotEvent_Train(object sender, UpdateEventArgs e)
        {
            foreach (var @event in e.NewEvents)
            {
                newDataTrain = double.Parse(@event.Get("AvgAmpereData").ToString());
                newTimeTrain = long.Parse(@event.Get("TimeStamp").ToString());
            }

            //InvokePlotData_Train(newTimeTrain, newDataTrain/* * ROBOT_VOLTAGE*/);

            writeRefDataTraining(newDataTrain);
        }

        /// <summary>
        /// Method to validate cross thread operation
        /// </summary>
        public void InvokePlotData_Train(double newX, double newY)
        {
            if (chartTrainingData.InvokeRequired)
            {
                trainDataForm.Invoke(new MethodInvoker(delegate
                {
                    PlotDataTrain(newX, newY, chartTrainingData);
                }));
            }

            else
            {
                PlotDataTrain(newX, newY, chartTrainingData);
            }
        }

        /// <summary>
        /// Method to validate cross thread operation
        /// </summary>
        public void InvokePlotData(double newX, double newY, Chart chart)
        {
            if (chart.InvokeRequired)
            {
                trainDataForm.Invoke(new MethodInvoker(delegate
                {
                    chart.Series[0].Points.AddXY(newX, newY);
                    // Redraw chart
                    chart.Invalidate();
                }));
            }

            else
            {
                chart.Series[0].Points.AddXY(newX, newY);
                // Redraw chart
                chart.Invalidate();
            }
        }

        /// <summary>
        /// Plot data in chart
        /// </summary>
        private void PlotDataTrain(double newX, double newY, Chart chart)
        {
            int timeToMonitor = 360000;      // Define time in miliseconds

            // Adding new data points
            chart.Series[0].Points.AddXY(newX, newY);

            // Keep a constant number of points by removing them from the left
            while (chart.Series[0].Points[0].XValue < newX - timeToMonitor)
            {
                chart.Series[0].Points.RemoveAt(0);
            }

            // Adjust X axis scale
            chart.ChartAreas[0].AxisX.Minimum = newX - timeToMonitor;
            chart.ChartAreas[0].AxisX.Maximum = newX;

            // Redraw chart 
            //chart.Invalidate(); // implemented as button action
        }

        private void writeRefDataTraining(double data)
        {
            String line = data.ToString() + "\n";
            File.AppendAllText(@"C:\Users\teozk\OneDrive\Desktop\ref_realSensor_Printer.csv", line);
        }

        public void ClearChart()
        {
            chartTrainingData.Series[0].Points.Clear();
        }

        private int numberClicked = 1;
        private StripLine patternMark;

        public void SetClickedXValue(int xValue)
        {
            if (numberClicked == 1)
            {
                patternMark = new StripLine();
                patternMark.BorderColor = System.Drawing.Color.Red;
                patternMark.BorderWidth = 5;
                patternMark.BorderDashStyle = ChartDashStyle.Solid;
                patternMark.BackColor = System.Drawing.Color.Transparent;

                patternMark.StripWidth = 5;
                patternMark.IntervalOffset = xValue;
                numberClicked++;
            }

            else if (numberClicked == 2)
            {
                try
                {
                    patternMark.StripWidth = xValue - patternMark.IntervalOffset;
                    numberClicked = 1;
                }
                catch
                {
                    MessageBox.Show("Chose first the left value, the the right value!");
                }
            }

            if (chartTrainingData.InvokeRequired && numberClicked != 0)
            {
                trainDataForm.Invoke(new MethodInvoker(delegate
                {
                    chartTrainingData.ChartAreas[0].AxisX.StripLines.Add(patternMark);
                }));
            }

            else if (numberClicked != 0)
            {
                chartTrainingData.ChartAreas[0].AxisX.StripLines.Add(patternMark);
            }
        }

        public void ClearMarker()
        {
            chartTrainingData.ChartAreas[0].AxisX.StripLines.Clear();
            numberClicked = 1;
        }

        public void CommitReferencePattern()
        {
            referencePatternBeginIndex = (int)patternMark.IntervalOffset;
            referencePatternEndIndex = (int)patternMark.IntervalOffset + (int)patternMark.StripWidth;
            referencePatternDuration = referencePatternEndIndex - referencePatternBeginIndex;
            referencePatternTraining = new List<double>();
            referencePatternTraining = GetSequenceData(referencePatternBeginIndex, referencePatternEndIndex, chartTrainingData);

            Console.WriteLine("Copied the sequence to seperate list.. Specs:");
            Console.WriteLine("Beginning of sequence: " + referencePatternBeginIndex);
            Console.WriteLine("End of sequence: " + referencePatternEndIndex);
            Console.WriteLine("Duration of sequence: " + (referencePatternDuration));
            Console.WriteLine("Number of Elements of sequence: " + referencePatternTraining.Count);

            // Plot reference pattern on chart 2
            trainDataForm.chart2.Series[0].Points.Clear();
            for (int i = 0; i < referencePatternTraining.Count; i++)
                trainDataForm.chart2.Series[0].Points.AddY(referencePatternTraining.ElementAt(i));
            trainDataForm.chart2.Invalidate();

            Console.WriteLine("Commiting finished..");
        }

        public void BeginLearning()
        {
            trainDataForm.chart3.Series[0].Points.Clear();

            int numberOfExpectedPatterns = Convert.ToInt32(trainDataForm.textBox2.Text);

            bool foundOptimalThreshold = false;

            for (float i = 0; i < 10; i += .25F)
            {
                int numberOfDetectedPatterns = DetectEvent(i);

                InvokePlotData(i, numberOfDetectedPatterns, trainDataForm.chart3);

                if (numberOfDetectedPatterns == numberOfExpectedPatterns && !foundOptimalThreshold)
                {
                    OptimalThreshold = i;
                    trainDataForm.label4.Text = "Optimal Threshold: " + i;
                    foundOptimalThreshold = true;
                    MarkThreshold(OptimalThreshold);
                }
            }
            ClearMarker();
            DetectEvent(OptimalThreshold);
        }

        private void MarkThreshold(float optimalThreshold)
        {
            // Mark optimal threshold in chart 3
            trainDataForm.chart3.ChartAreas[0].AxisX.StripLines.Clear();
            StripLine thresholdMarker = new StripLine();
            thresholdMarker.BorderColor = System.Drawing.Color.Red;
            thresholdMarker.BorderWidth = 5;
            thresholdMarker.BorderDashStyle = ChartDashStyle.Solid;
            thresholdMarker.BackColor = System.Drawing.Color.Red;
            thresholdMarker.StripWidth = 0.25;
            thresholdMarker.IntervalOffset = optimalThreshold;
            trainDataForm.chart3.ChartAreas[0].AxisX.StripLines.Add(thresholdMarker);
        }

        private List<double> GetSequenceData(int referencePatternBeginIndex, int referencePatternEndIndex, Chart chart)
        {
            List<double> sequencePattern = new List<double>();
            int i = 0;

            while (chart.Series[0].Points.ElementAt(i).XValue < referencePatternEndIndex)
            {
                if (chart.Series[0].Points.ElementAt(i).XValue > referencePatternBeginIndex)
                {
                    sequencePattern.Add(chartTrainingData.Series[0].Points.ElementAt(i).YValues[0]);
                }
                i++;
            }

            return sequencePattern;
        }

        private int DetectEvent(float epsilon)
        {
            // Generate subsequnece to detect
            List<double> referencePatternTrainingCpy = new List<double>(referencePatternTraining);
            dtwTraining = new SubsequenceDTW(referencePatternTrainingCpy, epsilon, (referencePatternDuration) / 2);

            // Number of detected patterns
            int numberOfDetectedPatterns = 0;

            // Clear all marker from chart
            chartTrainingData.ChartAreas[0].AxisX.StripLines.Clear();

            // get data from chart
            var streamData = chartTrainingData.Series[0].Points;

            // Run detection algorithm for all points of chart
            for (int i = 0; i < streamData.Count; i++)
            {
                Subsequence sequence = dtwTraining.compareDataStream(streamData.ElementAt(i).YValues[0], (int)streamData.ElementAt(i).XValue);

                if ((sequence.Status == SubsequenceStatus.Optimal) /*|| (sequence.Status == SubsequenceStatus.NotOptimal)*/)
                {
                    SetMarker(sequence, chartTrainingData);
                    numberOfDetectedPatterns++;
                }
            }

            return numberOfDetectedPatterns;
        }

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
                trainDataForm.Invoke(new MethodInvoker(delegate
                {
                    chart.ChartAreas[0].AxisX.StripLines.Add(detectedPatternMark);
                }));
            }

            else
            {
                chart.ChartAreas[0].AxisX.StripLines.Add(detectedPatternMark);
            }
        }
    }
}
