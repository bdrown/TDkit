using System;
using System.Collections.Generic;

namespace TDkit.MassSpec
{
    /// <summary>
    /// Neutral distribution of isotopes.
    /// </summary>
    public class IsotopicDistribution
    {
        private double[] mass;

        private double[] abundance;

        /// <summary>
        /// Number of peaks in the isotopic distribution
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Initializes an IsotopicDistribution from lists of masses and abundances
        /// </summary>
        /// <param name="mass">List of centroid masses</param>
        /// <param name="abundance">List of abundances</param>
        public IsotopicDistribution(List<double> mass, List<double> abundance)
        {
            // The mass and abundance arrays must have the same length
            if (mass.Count != abundance.Count)
                throw new ArgumentException("The lengths of mass and abundance arrays must be the same for an IsotopicDistrubtion");

            this.mass = mass.ToArray();
            this.abundance = abundance.ToArray();
            this.Length = this.mass.Length;
        }

        /// <summary>
        /// Initializes an IsotopicDistribution from arrays of masses and abundances
        /// </summary>
        /// <param name="mass">Array of centroid masses</param>
        /// <param name="abundance">Array of abundances</param>
        public IsotopicDistribution(double[] mass, double[] abundance)
        {
            // The mass and abundance arrays must have the same length
            if (mass.Length != abundance.Length)
                throw new ArgumentException("The lengths of mass and abundance arrays must be the same for an IsotopicDistrubtion");

            this.mass = mass;
            this.abundance = abundance;
            this.Length = this.mass.Length;
        }

        /// <summary>
        /// Gets the masses.
        /// </summary>
        public IList<double> Masses => mass;

        /// <summary>
        /// Gets the intensities.
        /// </summary>
        public IList<double> Intensities => abundance;


        /// <summary>
        /// Creates a charged isotope distribution from a neutral isotope distribution.
        /// The ratio of abundances stays the same but m/z values are calculated.
        /// </summary>
        /// <param name="charge">Charge of the desired distribution</param>
        /// <returns>A charge state distribution</returns>
        public ChargedIsotopicDistribution CreateChargedDist(int charge)
        {
            double[] mz = new double[this.Length];

            for(int i = 0; i < this.Length; i++)
            {
                mz[i] = Utilities.MassToMz(mass[i], charge);
            }

            return new ChargedIsotopicDistribution(mz, this.abundance, charge);
        }
    }
}
