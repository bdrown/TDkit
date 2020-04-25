using System;
using System.Collections.Generic;
using System.Text;

namespace TDkit.MassSpec
{
    /// <summary>
    /// Neutral distribution of isotopes.
    /// </summary>
    public class IsotopicDistribution
    {
        private double[] mass;

        private double[] abundance;

        public int Length { get; }

        public IsotopicDistribution(List<double> mass, List<double> abundance)
        {
            this.mass = mass.ToArray();
            this.abundance = abundance.ToArray();
            this.Length = this.mass.Length;
        }

        public IsotopicDistribution(double[] mass, double[] abundance)
        {
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
    }
}
