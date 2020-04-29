using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

        private IsotopicConstants isotopicConstants;

        public Brain()
        {
            this.isotopicConstants = new IsotopicConstants();
        }

        public ChargedIsotopicDistribution GenerateChargeIsotopicDistribution(ChemicalFormula formula, int charge)
        {
            throw new NotImplementedException();
        }

        public IsotopicDistribution GenerateIsotopicDistribution(ChemicalFormula formula)
        {
            return IsotopicVariants(formula);
        }

        /// <summary>
        /// Given two arrays of values, the first array being the `power sum`s
        /// of a polynomial, the second being expressions of the roots of the
        /// polynomial as found by Viete's Formula, use information from the
        /// longer list to fill out the shorter list using Newton's Identities
        /// </summary>
        /// <param name="power_sum"></param>
        /// <param name="elementary_symmetric_polynomial"></param>
        /// <param name="order"></param>
        private static void Newton(ref List<double> power_sum, ref List<double> elementary_symmetric_polynomial, int order)
        {
            if (power_sum.Count > elementary_symmetric_polynomial.Count)
                UpdateElementarySymmetricPolynomial(ref power_sum, ref elementary_symmetric_polynomial, order);
            else if (power_sum.Count < elementary_symmetric_polynomial.Count)
                UpdatePowerSum(ref power_sum, ref elementary_symmetric_polynomial);
        }

        /// <summary>
        /// Helper function for Newton
        /// </summary>
        /// <param name="power_sum"></param>
        /// <param name="elementary_symmetric_polynomial"></param>
        /// <param name="order"></param>
        private static void UpdateElementarySymmetricPolynomial(ref List<double> power_sum, ref List<double> elementary_symmetric_polynomial, int order)
        {
            int begin = elementary_symmetric_polynomial.Count;
            int end = power_sum.Count;

            for (int k = begin; k < end; k++)
            {
                if (k == 0)
                    elementary_symmetric_polynomial.Add(1.0);
                else if (k > order)
                    elementary_symmetric_polynomial.Add(0.0);
                else
                {
                    double el = 0.0;
                    for (int j = 1; j < k + 1; j++)
                    {
                        int sign = (j % 2) == 1 ? 1 : -1;
                        el += sign * power_sum[j] * elementary_symmetric_polynomial[k - j];
                    }
                    el /= k;
                    elementary_symmetric_polynomial.Add(el);
                }
            }
        }

        /// <summary>
        /// Helper function for Newton
        /// </summary>
        /// <param name="power_sum"></param>
        /// <param name="esp"></param>
        private static void UpdatePowerSum(ref List<double> power_sum, ref List<double> esp)
        {
            int begin = power_sum.Count;
            int end = esp.Count;
            for (int k = begin; k < end; k++)
            {
                if (k == 0)
                {
                    power_sum.Add(0.0);
                    continue;
                }
                double temp = 0.0;
                int sign = -1;
                for (int j = 1; j < k; j++)
                {
                    sign *= -1;
                    temp += sign * esp[j] * power_sum[k - j];
                }
                sign *= -1;
                temp += sign * esp[k] * k;
                power_sum.Add(temp);
            }
        }

        /// <summary>
        /// Given the coefficients of a polynomial of a single variable,
        /// compute an elementary symmetric polynomial of the roots of the
        /// input polynomial by Viete's Formula.
        /// </summary>
        /// <param name="coefficients">An array containing the coefficients of the input polynomial of arbitrary length (degree) n</param>
        /// <returns>An array containing the expression of the roots of the input polynomial of length n</returns>
        private static List<double> Vietes(List<double> coefficients)
        {
            int size = coefficients.Count;
            double tail = coefficients[size - 1];
            List<double> elementary_symetric_polynomial = new List<double>();

            int sign;
            for (int i = 0; i < size; i++)
            {
                sign = (i % 2) == 0 ? 1 : -1;
                elementary_symetric_polynomial.Add(sign * coefficients[size - i - 1] / tail);
            }

            return elementary_symetric_polynomial;
        }

        private List<double> PhiValues(ChemicalFormula formula, int npeaks)
        {
            double[] power_sum = new double[npeaks + 1];
            for (int i = 0; i <= npeaks; i++)
            {
                power_sum[i] = PhiValue(formula, i);
            }
            return new List<double>(power_sum);
        }

        private double PhiValue(ChemicalFormula formula, int order)
        {
            double phi = 0;
            foreach (KeyValuePair<Element, int> kvp in formula.GetElements())
            {
                phi += kvp.Value * this.isotopicConstants.ElementPowerSum(kvp.Key, kvp.Value);
            }
            return phi;
        }

        private List<double> ModifiedPhiValues(ChemicalFormula formula, Element element, int order)
        {
            double[] power_sum = new double[order + 1];
            power_sum[0] = 0.0;
            for (int i = 1; i <= order; i++)
            {
                power_sum[i] = ModifiedPhiValue(formula, element, i);
            }
            return new List<double>(power_sum);
        }

        private double ModifiedPhiValue(ChemicalFormula formula, Element element, int order)
        {
            double phi = 0.0;
            foreach (KeyValuePair<Element, int> kvp in formula.GetElements())
            {
                int coef = element.Equals(kvp.Key) ? kvp.Value - 1 : kvp.Value;
                phi += this.isotopicConstants.ElementPowerSum(kvp.Key, order);
            }
            phi += this.isotopicConstants.ModifiedElementPowerSum(element, order);
            return phi;
        }

        private List<double> Probability(ChemicalFormula formula, int order)
        {
            List<double> phi_values = PhiValues(formula, order);
            int max_var_count = MaxVariants(formula);
            List<double> probabilities = new List<double>();
            Newton(ref phi_values, ref probabilities, max_var_count);

            for (int i = 0; i < probabilities.Count; i++)
            {
                int sign = (i % 2) == 0 ? 1 : -1;
                // q(j) = q(0)  * e(j) * (-1)^j
                // intensity of the jth peak is |probability[j]| * the intensity of monoisotopic peak
                probabilities[i] *= sign;
            }
            return probabilities;
        }

        private List<double> CenterMass(ChemicalFormula formula, List<double> probabilities, int order)
        {
            int max_var_count = MaxVariants(formula);
            List<double> masses = new List<double>();

            Dictionary<Element, List<double>> ele_sym_poly_map = new Dictionary<Element, List<double>>();
            List<double> power_sum;
            List<double> ele_sym_poly;
            foreach (Element element in formula.GetElements().Keys)
            {
                power_sum = ModifiedPhiValues(formula, element, order);
                ele_sym_poly = new List<double>();
                Newton(ref power_sum, ref ele_sym_poly, max_var_count);
                ele_sym_poly_map[element] = ele_sym_poly;
            }
            for(int i = 0; i <= order; i++)
            {
                int sign = (i % 2) == 0 ? 1 : -1;
                double center = 0.0;
                foreach (var kvp in ele_sym_poly_map)
                {
                    // TODO: incorporate monoisotopic peak intensity
                    // center += formula.GetElements()[kvp.Key] * sign * kvp.Value[i] * monoisotopic_peak.intensity * this.isotopicConstants[kvp.Key].Element.MonoisotopicMass;
                    center += formula.GetElements()[kvp.Key] * sign * kvp.Value[i] * this.isotopicConstants[kvp.Key].Element.MonoisotopicMass();
                }
                masses.Add(probabilities[i] > 0 ? center / probabilities[i] : 0);
            }
            return masses;
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

        public IsotopicDistribution IsotopicVariants(ChemicalFormula formula, int npeaks = -1)
        {
            if (npeaks == -1)
            {
                int max_variants = MaxVariants(formula);
                npeaks = Convert.ToInt32(Math.Sqrt(max_variants)) - 2;
                npeaks = Math.Max(npeaks, 3);
                this.isotopicConstants.Order = npeaks;
            }
            else
            {
                // Don't include the monoisotopic peak
                npeaks -= 1;
            }
            return AggregateIsotopicVariants(formula, npeaks);

        }

        public IsotopicDistribution AggregateIsotopicVariants(ChemicalFormula formula, int npeaks)
        {
            List<double> probabilities = Probability(formula, npeaks);
            List<double> center_masses = CenterMass(formula, probabilities, npeaks);
            List<double> intensities = new List<double>();
            
            double total = probabilities.Sum();

            for (int i = 0; i < npeaks; i++)
            {
                if (center_masses[i] < 1)
                    continue;
                double adjusted = center_masses[i];
                intensities.Add(probabilities[i] / total);
            }
            return new IsotopicDistribution(center_masses, intensities);
        }

        private class PolynomialParameters : IEnumerable
        {
            public List<double> ElementaryPolynomial;

            public List<double> PowerSum;

            public PolynomialParameters(List<double> elementary_polynomial, List<double> power_sum)
            {
                this.ElementaryPolynomial = elementary_polynomial;
                this.PowerSum = power_sum;
            }

            public PolyParamEnum GetEnumerator()
            {
                return new PolyParamEnum(this.PowerSum, this.ElementaryPolynomial);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return (IEnumerator)GetEnumerator();
            }
        }

        private class PolyParamEnum : IEnumerator
        {
            public List<double> ElementaryPolynomial { get; }

            public List<double> PowerSum { get; }

            int position = -1;

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public PolyParamEnum(List<double> power_sum, List<double> elementary_polynomial)
            {
                this.ElementaryPolynomial = elementary_polynomial;
                this.PowerSum = power_sum;
            }

            public (double, double) Current
            {
                get
                {
                    try
                    {
                        return (this.PowerSum[position], this.ElementaryPolynomial[position]);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            public bool MoveNext()
            {
                position++;
                return (position < PowerSum.Count);
            }

            public void Reset()
            {
                position = -1;
            }
        }

        private class PhiConstants
        {
            public int Order { get; set; }

            public Element Element { get; }

            public PolynomialParameters ElementCoefficients { get; }

            public PolynomialParameters MassCoefficients { get; }

            public PhiConstants(int order, Element element, PolynomialParameters element_coefficients, PolynomialParameters mass_coefficients)
            {
                this.Order = order;
                this.Element = element;
                this.ElementCoefficients = element_coefficients;
                this.MassCoefficients = mass_coefficients;
            }
        }

        private class IsotopicConstants : Dictionary<Element, PhiConstants>
        {
            public int Order { get; set; }

            public IsotopicConstants(int order)
            {
                this.Order = order;
            }

            public IsotopicConstants()
            {
            }

            public void AddElement(Element element)
            {
                if (this.ContainsKey(element))
                    return;
                int order = element.MaxNeutronShift();
                PolynomialParameters element_parameters = Coefficients(element, false);
                PolynomialParameters mass_paramters = Coefficients(element, true);
                this[element] = new PhiConstants(order, element, element_parameters, mass_paramters);
            }

            public void UpdateCoefficients()
            {
                foreach (KeyValuePair<Element, PhiConstants> entry in this){
                    if (this.Order < entry.Value.Order)
                        continue;

                    for (int i = entry.Value.Order; i <= this.Order; i++)
                    {
                        entry.Value.ElementCoefficients.ElementaryPolynomial.Add(0.0);
                        entry.Value.MassCoefficients.ElementaryPolynomial.Add(0.0);
                    }

                    entry.Value.Order = entry.Value.ElementCoefficients.ElementaryPolynomial.Count;
                    Newton(ref entry.Value.ElementCoefficients.PowerSum, ref entry.Value.ElementCoefficients.ElementaryPolynomial, this.Order);
                    Newton(ref entry.Value.MassCoefficients.PowerSum, ref entry.Value.MassCoefficients.ElementaryPolynomial, this.Order);
                }
            }

            public PolynomialParameters Coefficients(Element element, bool with_mass = false)
            {
                int max_isotope_number = element.MaxNeutronShift();
                List<double> accumulator = new List<double>();
                double coef;

                foreach (Isotope isotope in element.IsotopeDistribution)
                { // should this list be reversed?
                    int current_order = max_isotope_number - isotope.NeutronShift;
                    if (with_mass)
                        coef = isotope.RelativeAtomicMass;
                    else
                        coef = 1.0;

                    if (current_order > accumulator.Count)
                    {
                        for (int i = accumulator.Count; i < current_order; i++)
                        {
                            accumulator.Add(0.0); // seems like there's a better way to do this
                        }
                        accumulator.Add(isotope.Abundance * coef);
                    }
                    else if (current_order == accumulator.Count)
                    {
                        accumulator.Add(isotope.Abundance * coef);
                    }
                    else
                        throw new InvalidOperationException("This list of neutron shifts is not ordered");
                }

                List<double> elementary_symmetric_polynomial = Vietes(accumulator);
                List<double> power_sum = new List<double>();
                Newton(ref power_sum, ref elementary_symmetric_polynomial, accumulator.Count - 1);
                return new PolynomialParameters(elementary_symmetric_polynomial, power_sum);
            }

            public double ElementPowerSum(Element element, int order)
            {
                PhiConstants contants = this[element];
                return contants.ElementCoefficients.PowerSum[order];
            }

            public double ModifiedElementPowerSum(Element element, int order)
            {
                PhiConstants contants = this[element];
                return contants.MassCoefficients.PowerSum[order];
            }


        }
    }
}
