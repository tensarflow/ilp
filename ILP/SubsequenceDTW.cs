using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILP
{
    class SubsequenceDTW
    {
        private int m; 
        private float threshold;
        private List<double> y;
        private List<double> d;
        private List<double> dPast;
        private List<int> s;
        private List<int> sPast;
        private double dMin;
        private int tS;
        private int tE;
        private int minSubsequenceLength;

        public float Threshold
        {
            get => threshold;
            set => threshold = value;
        }

        /// <summary>
        /// Creates a new instance of the SubsequenceDTW class.
        /// </summary>
        /// <param name="template"></param>Template signal for comparison.
        /// <param name="distanceThreshold">Maximum distance between the subsequence 
        /// and the template.</param>
        /// <param name="minimumSubsequenceLength">Minimum length of a detected 
        /// subsequence.</param> 
        public SubsequenceDTW(List<double> template, float distanceThreshold, int minimumSubsequenceLength = 2)
        {
            m = template.Count;

            threshold = distanceThreshold;

            y = template;
            y.Insert(0, Double.PositiveInfinity);

            d = new List<double>();
            dPast = new List<double>();
            d.Add(0);
            dPast.Add(0);
            for (int i = 1; i <= m; i++)
            {
                d.Add(Double.PositiveInfinity);
                dPast.Add(Double.PositiveInfinity);
            }

            s = new List<int>();
            sPast = new List<int>();
            for (int i = 0; i <= m; i++)
            {
                s.Add(1);
                sPast.Add(1);
            }

            dMin = Double.PositiveInfinity;

            tS = 0;
            tE = 0;

            minSubsequenceLength = minimumSubsequenceLength - 1;
        }

        /// <summary>
        /// Compare the value x at time t of the data stream with the template.
        /// Returns an unset, not optimal or optimal subsequence with its distance, 
        /// starttime and endtime.
        /// 
        /// Uses the SPRING DTW algorithm.
        /// 
        /// Sakurai, Yasushi; Faloutsos, Christos; Yamamuro, Masashi (2007):
        /// Stream Monitoring under the Time Warping Distance. 
        /// In: IEEE 23rd International Conference on Data Engineering, 2007.
        /// 15 - 20 April, 2007, [Istanbul, Turkey]. 
        /// 2007 IEEE 23rd International Conference on Data Engineering. Istanbul, Turkey.
        /// Institute of Electrical and Electronics Engineers; 
        /// International Conference on Data Engineering; ICDE 2007. 
        /// Piscataway, NJ: IEEE Service Center, S. 1046–1055.
        /// </summary>
        /// <param name="xT"></param>Value x of data stream at time t.
        /// <param name="t"></param>Time t of value x. Time starts with 1.
        /// <returns></returns>A subsequence with its distance, starttime and endtime.
        public Subsequence compareDataStream(double xT, int t)
        {
            Subsequence report = new Subsequence
            {
                Status = SubsequenceStatus.NotSet,
                TDetected = t
            };
            s[0] = t;

            for (int i = 1; i <= m; i++)
            {
                double dBest = Math.Min(Math.Min(d[i - 1], dPast[i]), dPast[i - 1]);
                d[i] = Math.Abs(xT - y[i]) + dBest;

                if (d[i - 1] == dBest)
                    s[i] = s[i - 1];
                else if (dPast[i] == dBest)
                    s[i] = sPast[i];
                else if (dPast[i - 1] == dBest)
                    s[i] = sPast[i - 1];
            }

            if (dMin <= threshold)
            {
                for (int i = 1; i <= m; i++)
                {
                    if (d[i] < dMin && s[i] <= tE)
                    {
                        report.Distance = dMin;
                        report.TStart = tS;
                        report.TEnd = tE;
                        report.Status = SubsequenceStatus.Optimal;

                        dMin = Double.PositiveInfinity;
                        for (int j = 1; j <= m; j++)
                        {
                            if (s[j] <= tE)
                            {
                                d[j] = Double.PositiveInfinity;
                            }
                        }
                    }
                }
            }

            if (d[m] <= threshold && d[m] < dMin && (t - s[m]) >= minSubsequenceLength)
            {
                dMin = d[m];
                tS = s[m];
                tE = t;

                // if no optimal subsequence is reported, report the not optimal 
                // subsequence
                if (report.Status != SubsequenceStatus.Optimal)
                {
                    report.Distance = dMin;
                    report.TStart = tS;
                    report.TEnd = tE;
                    report.Status = SubsequenceStatus.NotOptimal;
                }
            }

            dPast = new List<double>(d);
            sPast = new List<int>(s);

            return report;
        }
    }

    /// <summary>
    /// Subsequence in a data stream which suits a given template.
    /// </summary>
    public struct Subsequence
    {
        private double distance;
        private int tStart;
        private int tEnd;
        private SubsequenceStatus status;
        private int tDetected;

        /// <summary>
        /// Calculates distance between the template and the data stream.
        /// </summary>
        public double Distance { get => distance; set => distance = value; }
        /// <summary>
        /// Starttime of data stream which fits to the template.
        /// </summary>
        public int TStart { get => tStart; set => tStart = value; }
        /// <summary>
        /// Endtime of data stream which fits to the template.
        /// </summary>
        public int TEnd { get => tEnd; set => tEnd = value; }
        /// <summary>
        /// Status of detected subsequence.
        /// </summary>
        public SubsequenceStatus Status { get => status; set => status = value; }
        /// <summary>
        /// Time of detection.
        /// </summary>
        public int TDetected { get => tDetected; set => tDetected = value; }
    }

    /// <summary>
    /// Status of detected subsequence.
    /// </summary>
    public enum SubsequenceStatus
    {
        /// <summary>
        /// No subsequence was detected.
        /// </summary>
        NotSet,
        /// <summary>
        /// A subsequenece with smaller distance can occur.
        /// </summary>
        NotOptimal,
        /// <summary>
        /// No more exact subsequenece can occur.
        /// </summary>
        Optimal
    }
}