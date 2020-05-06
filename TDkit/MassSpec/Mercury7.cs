using System;
using System.Collections.Generic;
using TDkit.Chemistry;

namespace TDkit.MassSpec
{
    /// <summary>
    /// Implementation of Mercury7 algorithm for generating isotopic distributions
    /// 
    /// Algorithm was first described by Alan Rockwood in 2004 JASM paper:
    /// Rockwood, A., Orman, J., Dearden, D. (2004). Isotopic compositions and accurate masses of single isotopic peaks
    /// Journal of the American Society for Mass Spectrometry  15(1), 12-21. https://dx.doi.org/10.1016/j.jasms.2003.08.011
    /// </summary>
    public class Mercury7 : IIsotopeDistGenerator
    {

        private double limit;

        public Mercury7(double limit = 1E-26)
        {
            this.limit = limit;
        }

        public IsotopicDistribution GenerateIsotopicDistribution(ChemicalFormula formula)
        {
            return this.Mercury(formula, this.limit);
        }

        public ChargedIsotopicDistribution GenerateChargeIsotopicDistribution(ChemicalFormula formula, int charge)
        {
            throw new NotImplementedException();
        }

        private IsotopicDistribution Mercury(ChemicalFormula formula, double limit)
        {
            double[] msaMz = null;
            double[] msaAbundance = null;

            double[] tmpMz = null;
            double[] tmpAbundance = null;
            bool msaInitialized = false;

            foreach (KeyValuePair<Element, int> kvp in formula.GetElements())
            {
                int n = kvp.Value;

                if (n == 0)
                    continue;

                int isotopeCount = kvp.Key.IsotopeDistribution.Count;
                double[] esaMz = new double[isotopeCount];
                double[] esaAbundance = new double[isotopeCount];

                int i = 0;
                foreach (var isotope in kvp.Key.IsotopeDistribution)
                {
                    esaMz[i] = isotope.RelativeAtomicMass;
                    esaAbundance[i] = isotope.Abundance;
                    i++;
                }

                while(true)
                {
                    if ((n & 1) == 1)
                    {
                        if (msaInitialized)
                        {
                            Convolve(ref tmpMz, ref tmpAbundance, msaMz, msaAbundance, esaMz, esaAbundance);

                            msaMz = this.CopyArray(tmpMz);
                            msaAbundance = this.CopyArray(tmpAbundance);
                        }
                        else
                        {
                            msaMz = this.CopyArray(esaMz);
                            msaAbundance = this.CopyArray(esaAbundance);
                            msaInitialized = true;
                        }

                        Prune(ref msaMz, ref msaAbundance, limit);

                    }

                    if (n == 1)
                        break;

                    Convolve(ref tmpMz, ref tmpAbundance, esaMz, esaAbundance, esaMz, esaAbundance);

                    esaMz = this.CopyArray(tmpMz);
                    esaAbundance = this.CopyArray(tmpAbundance);

                    Prune(ref esaMz, ref esaAbundance, limit);
                    n = n >> 1;
                }
            }

            return new IsotopicDistribution(msaMz, msaAbundance);
        }

        private void Prune(ref double[] mz, ref double[] ab, double limit)
        {
            int start = 0;
            int end = mz.Length - 1;

            while (ab[start] < limit)
                start++;

            while (ab[end] < limit)
                end--;

            // See if we need to prune
            if (end - start < mz.Length - 1)
            {
                int length = end - start + 1;
                double[] tmpMz = new double[length];
                double[] tmpAb = new double[length];

                Array.Copy(mz, start, tmpMz, 0, length);
                Array.Copy(ab, start, tmpAb, 0, length);

                mz = tmpMz;
                ab = tmpAb;
            }
        }

        private void Convolve(ref double[] resultMz, ref double[] resultAb, double[] mz1, double[] ab1, double[] mz2, double[] ab2)
        {
            int n1 = mz1.Length;
            int n2 = mz2.Length;

            if (n1 + n2 == 0)
                return;

            // Adaptation of speed optimization from C++...may not do anything similar in C#
            resultMz = new double[n1 + n2];
            resultAb = new double[n1 + n2];

            // For each isotopic peak in the compound...
            for (int k = 0; k < n1 + n2 - 1; k++)
            {
                double totalAbundance = 0;
                double massExpectation = 0;
                int start = k < (n2 - 1) ? 0 : k - n2 + 1; // start = max(0, k - n2 + 1)
                int end = k < (n1 - 1) ? k : n1 - 1; // end = min(n1 - 1, k)

                // Calculate the mass expectation value and the abundance
                for (int i = start; i <= end; i++)
                {
                    double ithAbundance = ab1[i] * ab2[k - i];
                    if (ithAbundance > 0)
                    {
                        totalAbundance += ithAbundance;
                        massExpectation += ithAbundance * (mz1[i] + mz2[k - i]);
                    }
                }

                //Do NOT throw away isotopes with zero probability, this would screw up the isotope count k!
                resultMz[k] = totalAbundance > 0 ? massExpectation / totalAbundance : 0;
                resultAb[k] = totalAbundance;
            }
        }

        private double[] CopyArray(double[] toCopy)
        {
            double[] newArray = new double[toCopy.Length];
            Array.Copy(toCopy, newArray, toCopy.Length);
            return newArray;
        }

        
    }
}
