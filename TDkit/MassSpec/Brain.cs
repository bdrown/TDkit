using System;
using System.Collections.Generic;
using System.Text;

namespace TDkit.MassSpec
{
    /// <summary>
    /// .NET Implementation of BRAIN algorithm for calculating isotopic distribution
    /// 
    /// First described in 2012 JASM paper:
    /// Claesen, J., Dittwald, P., Burzykowski, T., Valkenborg, D. (2012). An 
    /// Efficient Method to Calculate the Aggregated Isotopic Distribution 
    /// and Exact Center-Masses Journal of The American Society for Mass 
    /// Spectrometry  23(4), 753-763.
    /// https://dx.doi.org/10.1007/s13361-011-0326-2
    /// 
    /// Some improvements were described in a 2014 JASM paper:
    /// Dittwald, P., Valkenborg, D. (2014). BRAIN 2.0: time and memory 
    /// complexity improvements in the algorithm for calculating the 
    /// isotope distribution. Journal of the American Society for Mass 
    /// Spectrometry  25(4), 588-94. 
    /// https://dx.doi.org/10.1007/s13361-013-0796-5
    /// 
    /// Originally implemented in R, the BRAIN algorithm has also been ported
    /// to Python
    /// R implementation: https://github.com/ditti2/BRAIN
    /// Python implementation: https://github.com/mobiusklein/brainpy
    /// 
    /// This implementation borrows heavily from brainpy
    /// </summary>
    public class Brain : IIsotopeDistGenerator
    {

        // The number of peaks to produce and the number of terms in the generating polynomial expression
        private int order;

        public ChargedIsotopicDistribution GenerateChargeIsotopicDistribution(ChemicalFormula formula, int charge)
        {
            throw new NotImplementedException();
        }

        public IsotopicDistribution GenerateIsotopicDistribution(ChemicalFormula formula)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Given the coefficients of a polynomial of a single variable,
        /// compute an elementary symmetric polynomial of the roots of the
        /// input polynomial by Viete's Formula.
        /// </summary>
        /// <param name="coefficients">An array containing the coefficients of the input polynomial of arbitrary length (degree) n</param>
        /// <returns>An array containing the expression of the roots of the input polynomial of length n</returns>
        private double[] Vietes(double[] coefficients)
        {
            int size = coefficients.Length;
            double tail = coefficients[size - 1];
            double[] elementary_symetric_polynomial = new double[size];

            int sign;
            for(int i = 0; i < size; i++)
            {
                sign = (i % 2) == 0 ? 1 : -1;
                elementary_symetric_polynomial[i] = sign * coefficients[size - i - 1] / tail;
            }

            return elementary_symetric_polynomial;
        }

        private double[] PhiValues(ChemicalFormula formula, int npeaks)
        {
            double[] power_sum = new double[npeaks + 1];
            for(int i = 0; i <= npeaks; i++)
            {
                power_sum[i] = PhiValue(formula, i);
            }
            return power_sum;
        }

        private double PhiValue(ChemicalFormula formula, int order)
        {
            double phi = 0;
            foreach (KeyValuePair<Element, int> kvp in formula.GetElements())
            {
                phi += kvp.Value * ElementPowerSum(kvp.Key, kvp.Value);
            }
            return phi;
        }

        /// <summary>
        /// Calculates the maximum number of isotopic variants that could be produced by a chemical formula
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public int MaxVariants(ChemicalFormula formula)
        {
            int maxVariants = 0;

            foreach (KeyValuePair<Element, int> kvp in formula.GetElements())
            {
                maxVariants += kvp.Value * kvp.Key.MaxNeutronShift();
            }

            return maxVariants;
        }

        public IsotopicDistribution IsotopicVariants(ChemicalFormula formula, int npeaks=-1, int charge = 0)
        {
            if(npeaks == -1)
            {
                int max_variants = MaxVariants(formula);
                npeaks = sqrt(max_variants) - 2;
                npeaks = max(npeaks, 3);
            }
            else
            {
                // Don't include the monoisotopic peak
                npeaks -= 1;
            }
            return AggregateIsotopicVariants(formula, npeaks, charge);

        }

        public IsotopicDistribution AggregateIsotopicVariants(ChemicalFormula formula, int npeaks, int charge)
        {
            double[] probability_v = Probability();
            double[] center_mass_v = CenterMass(probability_v);

            double[] peak_set = new double[npeaks];
            double average_mass = 0;

            for(int i = 0; i < npeaks; i++)
            {
                if (center_mass_v[i] < 1)
                    continue;
                if( pea)

            }

        }
    }
}
