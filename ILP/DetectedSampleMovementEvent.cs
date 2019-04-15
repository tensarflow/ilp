using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILP
{
    /// <summary>
    /// ToDo!!
    /// </summary>
    public class DetectedSampleMovementEvent
    {
        private int _startTime;
        private int _endTime;
        private int _duration;
        private double _usedEnergy;
        private double _cost;

        /// <summary>
        /// ToDo!!
        /// </summary>
        public DetectedSampleMovementEvent(int aStartTime, int aEndTime, double aUsedEnergy, double aCost)
        {
            _startTime = aStartTime;
            _endTime = aEndTime;
            _duration = aEndTime - aStartTime;
            _usedEnergy = aUsedEnergy;
            _cost = aCost;
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public int StartTime
        {
            get => _startTime;
            set => _startTime = value;
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public int EndTime
        {
            get => _endTime;
            set => _endTime = value;
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public int Duration
        {
            get => _duration;
            set => _duration = value;
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public double UsedEnergy
        {
            get => _usedEnergy;
            set => _usedEnergy = value;
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public double Cost
        {
            get => _cost;
            set => _cost = value;
        }

    }
}
