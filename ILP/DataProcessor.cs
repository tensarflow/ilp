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
    public class DataProcessor
    {
        /// <summary>
        /// ToDo!!
        /// </summary>
        public float[] String2floatArr(string receivedData)
        {
            receivedData = receivedData.Remove(0, 1);
            receivedData = receivedData.Remove(receivedData.Length - 1, 1);
            receivedData = receivedData.Replace(" ", string.Empty);
            return Array.ConvertAll(receivedData.Split(','), float.Parse);
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public float String2float(string receivedData)
        {
            receivedData = receivedData.Remove(0, 1);
            receivedData = receivedData.Remove(receivedData.Length - 1, 1);
            receivedData = receivedData.Replace(" ", string.Empty);
            return float.Parse(receivedData);
        }
    }
}
