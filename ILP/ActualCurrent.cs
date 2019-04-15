using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILP
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class ActualCurrent
    {
        /// <summary>
        /// ToDo!!
        /// </summary>
        public float[] amperetData;

        /// <summary>
        /// ToDo!!
        /// </summary>
        public ActualCurrent(float[] currentData)
        {
            this.amperetData = currentData;
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public ActualCurrent()
        {
            this.amperetData = new float[] { 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F };
        }
    }
}