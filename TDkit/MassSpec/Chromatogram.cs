using System;
using System.Collections.Generic;

namespace TDkit.MassSpec
{
    /// <summary>
    /// Base class for representing a chromatogram
    /// </summary>
    class Chromatogram
    {
        /// <summary>
        /// Times that data was collected at.
        /// In this case, time is not in a particular unit.
        /// </summary>
        private readonly double[] time;

        /// <summary>
        /// Intensity of signal
        /// </summary>
        private readonly double[] intensity;

        public Chromatogram(double[] time, double[] intensity)
        {
            this.time = time;
            this.intensity = intensity;
        }

        public Chromatogram(List<double> time, List<double> intensity)
        {
            this.time = time.ToArray();
            this.intensity = intensity.ToArray();
        }

        public int Length => time.Length;

        public double[] GetTimes()
        {
            return time;
        }

        public double[] GetIntensity()
        {
            return intensity;
        }

        /// <summary>
        /// Provides the intensity of the chromatogram at a given time.
        /// If the exact time is not found, the nearest time is used.
        /// </summary>
        /// <param name="time">Time to slice at</param>
        /// <returns>Intensity at the provided time</returns>
        public double IntensityAtTime(double time)
        {
            // Argument checking - provided time should be within the first and last values
            if (time < this.time[0] || time > this.time[this.Length - 1])
                throw new IndexOutOfRangeException("Asking for a time that is outside the range of the chromatogram");

            // For the cases in which the exact value is provided
            int idx = Array.FindIndex(this.time,
                element => element.Equals(time));

            if (idx != -1)
                return this.intensity[idx];

            // If the exact time does not exist, provide the closest value
            // Iterate through array and minimize the difference
            for(int i = 0; i < this.Length; i++)
            {
                double diff = this.time[i] - time;

                // When diff becomes positive, we just jumped over desired index
                if (Double.IsNegative(diff))
                    continue;

                // Determine if this index or the previous is closer
                if (Math.Abs(diff) < this.time[i - 1] - time)
                    return this.intensity[i];
                else
                    return this.intensity[i - 1];
            }
            // shouldn't be reached
            throw new InvalidOperationException("Time slice of chromatogram is invalid or arrays are out of order");
        }
    }
}
