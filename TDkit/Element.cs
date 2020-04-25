using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.Linq;

namespace TDkit
{
    /// <summary>
    /// Basic implementation of an element
    /// </summary>
    public class Element : IEquatable<Element>, IComparable<Element>
    {
        /// <summary>
        /// Collection that contains data for all elements in the periodic table. Initialized
        /// using CreateElements method.
        /// </summary>
        private static List<Element> elementData = CreateElements();

        /// <summary>
        /// Collection that contains data for all possible isotopes of the element.
        /// This collection even contains isotopes with zero abundance.
        /// </summary>
        private List<Isotope> fullIsotopeDistribution;
        
        /// <summary>
        /// Element symbol (e.g. C).
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Atomic number of element, also number of protons.
        /// </summary>
        public int AtomicNumber { get; }

        /// <summary>
        /// Collection of isotopes that exist for this element
        /// </summary>
        public List<Isotope> IsotopeDistribution {
            get
            {
                // Just return the isotopes that have some abundance
                IEnumerable<Isotope> toReturn =
                from isotope in fullIsotopeDistribution
                where isotope.Abundance > 0
                select isotope;
                return toReturn.ToList();
            }
        }
        
        /// <summary>
        /// Initializes an instance of Element
        /// </summary>
        /// <param name="symbol">Element symbol</param>
        /// <param name="name">Long name of element</param>
        /// <param name="atomicNumber">Atomic number of element</param>
        /// <param name="isotopes">Collection of isotopes associated with element</param>
        public Element(string symbol, int atomicNumber, List<Isotope> isotopes)
        {
            this.Symbol = symbol;
            this.AtomicNumber = atomicNumber;
            this.fullIsotopeDistribution = isotopes;
        }

        /// <summary>
        /// Obtains element corresponding to provided symbol using stored data.
        /// </summary>
        /// <param name="symbol">Element symbol.</param>
        /// <returns></returns>
        public static Element GetElementFromSymbol(string symbol)
        {
            // Element symbols are one or two characters. If param is longer than 2, throw an ArgumentException.
            if (symbol.Length > 2)
                throw new ArgumentException($"{symbol} is not a valid element, must be one or two characters.", "symbol");
            
            // Linq query for elements that match the provided symbol
            IEnumerable<Element> toReturn =
                from element in elementData
                where element.Symbol.Equals(symbol)
                select element;

            // If no element was found that matches the provided symbol, throw an ArgumentException
            if (toReturn.Count() == 0)
                throw new ArgumentException($"{symbol} is not a known element.", "symbol");

            // Query result should only have one element, return it
            // Should this be a deep copy?
            return toReturn.First();
        }

        /// <summary>
        /// The average mass of an element is the weighted average of all isotopes based
        /// on their natural abundance.
        /// </summary>
        /// <returns>Average mass of an element</returns>
        public double AverageMass()
        {
            // Although the weight sum could be assumed to be 1, we'll avoid that assumption and do the extra step
            double weightedMassSum = this.IsotopeDistribution.Sum(isotope => isotope.RelativeAtomicMass * isotope.Abundance);
            double weightSum = this.IsotopeDistribution.Sum(isotope => isotope.Abundance);

            if (weightSum != 0)
                return weightedMassSum / weightSum;
            else
                throw new DivideByZeroException("Sum of weights cannot be zero.");
            
        }

        /// <summary>
        /// The monoisotopic mass of an element is the mass of the primary isotope (most abundant)
        /// </summary>
        /// <returns>Monoisotopic mass of an element</returns>
        public double MonoisotopicMass() { 
            // If the element contains no isotopes, throw exception.
            // Should this be a different exception?
            if (IsotopeDistribution.Count == 0)
                throw new InvalidOperationException();

            // Find max abundance
            double max = this.IsotopeDistribution.Max(isotope => isotope.Abundance);

            // Return mass of first isotope with max abundance
            return this.IsotopeDistribution.First(isotope => isotope.Abundance == max).RelativeAtomicMass;
        }

        /// <summary>
        /// Calculate the difference in neutron count between lightest and heaviest isotope
        /// that is abundant.
        /// </summary>
        /// <returns>Max number of added neutrons</returns>
        public int MaxNeutronShift()
        {
            IEnumerable<int> aWeights =
                from isotope in fullIsotopeDistribution
                where isotope.Abundance > 0
                select isotope.AtomicWeight;
            return aWeights.Max() - aWeights.Min();
        }

        /// <summary>
        /// Data about elements is loaded from txt file. Obtained from NIST accessed on April 9, 2020.
        /// https://physics.nist.gov/cgi-bin/Compositions/stand_alone.pl?ele=&all=all&ascii=ascii2&isotype=all
        /// All isotopes are used, even ones with no natural abundance.
        /// </summary>
        /// <param name="fileName">Path to Json file containing element data</param>
        /// <returns></returns>
        private static List<Element> CreateElements()
        {
            List<Element> toReturn = new List<Element>();

            //   Data obtained from NIST https://physics.nist.gov/cgi-bin/Compositions/stand_alone.pl?ele=&ascii=ascii&isotype=all
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "TDkit.ElementData.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader sr = new StreamReader(stream))
            {
                string line;
                string currentSymbol = "";
                int currentAtomicNumber = -1;
                int currentAtomicWeight = -1;
                double currentMass = 0.0;
                double currentAbundance = 0.0;
                List<Isotope> currentIsotopes = new List<Isotope>();

                // Matches values after an equals sign but stops at a parenthesis
                string pattern = @"\s\=\s([A-Za-z\d\.]+)\(?";
                Regex r = new Regex(pattern);
                
                string value;

                while ((line = sr.ReadLine()) != null)
                {
                    // Get whatever is between the equals sign and possibly a parenthesis
                    Match match = r.Match(line);

                    // If there is no match, then it's a blank line
                    if (match.Success)
                        value = match.Groups[1].Value;
                    else
                        value = "";

                    // Atomic Number
                    if (line.StartsWith("Atomic Number"))
                    {
                        // If first iteration, set symbol and continue
                        if (currentAtomicNumber == -1)
                        {
                            currentAtomicNumber = Int32.Parse(value);
                        }
                        // Check if processing a new Element
                        else if (Int32.Parse(value) != currentAtomicNumber)
                        {
                            // If starting a new element, add old element to list and reset
                            toReturn.Add(new Element(currentSymbol, currentAtomicNumber, currentIsotopes));

                            // Reset
                            currentAtomicNumber = Int32.Parse(value);
                            currentIsotopes = new List<Isotope>();
                        }
                    }
                    // Symbol
                    else if (line.StartsWith("Atomic Symbol"))
                    {
                        currentSymbol = value;
                    }
                    // Atomic Weight
                    else if (line.StartsWith("Mass Number"))
                    {
                        currentAtomicWeight = Int32.Parse(value);
                    }
                    // Relative Atomic Mass
                    else if (line.StartsWith("Relative Atomic Mass"))
                    {
                        currentMass = Double.Parse(value);
                    }
                    // Abundance 
                    else if(line.StartsWith("Isotopic Composition"))
                    {
                        currentAbundance = String.IsNullOrEmpty(value) ? 0.0 : Double.Parse(value);

                        // Assume that the abundance is the last relevant property in the text block
                        // Create an isotope instance and add it to list
                        currentIsotopes.Add(new Isotope(currentAtomicNumber, currentAtomicWeight, currentMass, currentAbundance));  
                    }
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Implements IEquatable.Equals. Elements are equal if the atomic numbers and isotopic distributions are the same.
        /// </summary>
        /// <param name="other">Other Element to compare</param>
        /// <returns></returns>
        public bool Equals(Element other)
        {
            if (other == null) return false;

            if (this.AtomicNumber != other.AtomicNumber) return false;
            return this.IsotopeDistribution.SequenceEqual(other.IsotopeDistribution);
        }

        /// <summary>
        /// Implements IComparable.CompareTo. When sorting elements for Hill notations, carbon and hydrogen are first followed
        /// by the rest of the elements alphabetically. The same elements with different isotope distributions are sorted by
        /// average mass; the larger is first.
        /// </summary>
        /// <param name="other">Other Element to compare</param>
        /// <returns>Int32: 1 when this instance is first, 0 when both are the same, and -1 when other is first.</returns>
        public int CompareTo(Element other)
        {
            if (other == null) return 1;

            // If they are the same element, then look at the isotopes. The instance with the smaller average mass is first.
            if (this.AtomicNumber == other.AtomicNumber)
            {
                return this.AverageMass().CompareTo(other.AverageMass());
            }
            // Carbon is alway first and Hydrogen second.
            if (this.AtomicNumber == 6) return -1;
            if (other.AtomicNumber == 6) return 1;
            if (this.AtomicNumber == 1) return -1;
            if (other.AtomicNumber == 1) return 1;

            // After that, go alphabetical.
            return this.Symbol.CompareTo(other.Symbol);
        }
    }
}
