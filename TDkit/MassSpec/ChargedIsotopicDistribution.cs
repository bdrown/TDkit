using System;
using System.Collections.Generic;

namespace TDkit.MassSpec
{
    /// <summary>
    /// Minimal implementation of a charged isotopic distribution
    /// </summary>
    public class ChargedIsotopicDistribution : IMzData
    {
        private readonly double[] mz;
        private readonly double[] intensity;

        /// <summary>
        /// Initializes a ChargedIsotopicDistribution from m/z, intensity, and
        /// charge data.
        /// </summary>
        /// <param name="mz">Array of m/z values</param>
        /// <param name="intensity">Array of intensity values</param>
        /// <param name="charge">Charge of species</param>
        public ChargedIsotopicDistribution(double[] mz, double[] intensity, int charge)
        {
            // The mass and abundance arrays must have the same length
            if (mz.Length != intensity.Length)
                throw new ArgumentException("The lengths of m/z and abundance arrays must be the same for an IsotopicDistrubtion");

            this.intensity = intensity;
            this.mz = mz;
            this.Charge = charge;
        }

        /// <summary>
        /// Charge of species
        /// </summary>
        public int Charge { get; }

        /// <summary>
        /// First m/z value
        /// </summary>
        public double FirstMz => mz[0];

        /// <summary>
        /// Last m/z value
        /// </summary>
        public double LastMz => mz[mz.Length - 1];

        /// <summary>
        /// Number of peaks in distribution
        /// </summary>
        public int Length => mz.Length;

        /// <summary>
        /// Provides the list of intensities
        /// </summary>
        /// <returns>List of intensities</returns>
        public IList<double> GetIntensity()
        {
            return intensity;
        }

        /// <summary>
        /// Provides the list of m/z values
        /// </summary>
        /// <returns>List of m/z values</returns>
        public IList<double> GetMz()
        {
            return mz;
        }
    }
}
