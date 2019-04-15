using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.espertech.esper.compat;

namespace ILP
{
    class PrinterCurrentEvent
    {
        private float _ampereData;
        private long _timeStamp;
        private static long initialTime;
        private static bool isTimeInitialized = false;

        /// <summary>
        /// ToDo!!
        /// </summary>
        public PrinterCurrentEvent()
        {
            _ampereData = .0F;
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public PrinterCurrentEvent(float f)
        {
            _ampereData = f;

            if (isTimeInitialized == false)
            {
                initialTime = DateTimeHelper.CurrentTimeMillis;
                isTimeInitialized = true;
            }

            _timeStamp = DateTimeHelper.CurrentTimeMillis - initialTime;
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public float AmpereData
        {
            get => _ampereData;
            set => _ampereData = value;
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public long TimeStamp
        {
            get => _timeStamp;
            set => _timeStamp = value;
        }
    }
}
