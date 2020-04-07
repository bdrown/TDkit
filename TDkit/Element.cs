using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Linq;

namespace TDkit
{
    /// <summary>
    /// 
    /// </summary>
    public class Element
    {
        /// <summary>
        /// Collection that contains data for all elements in the periodic table. Initialized
        /// using CreateElements method.
        /// </summary>
        private static List<Element> elementData = CreateElements();
        
        /// <summary>
        /// Element symbol (e.g. C).
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Long name of Element (e.g. Carbon).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Atomic number of element, also number of protons.
        /// </summary>
        public int AtomicNumber { get; set; }

        /// <summary>
        /// Collection of isotopes that exist for this element
        /// </summary>
        public List<Isotope> IsotopeDistribution { get; set; }

        /// <summary>
        /// Empty constructor for Element.
        /// </summary>
        public Element()
        {
            // Empty constructor is necessary for Json deserialization
        }
        
        /// <summary>
        /// Initializes an instance of Element
        /// </summary>
        /// <param name="symbol">Element symbol</param>
        /// <param name="name">Long name of element</param>
        /// <param name="atomicNumber">Atomic number of element</param>
        /// <param name="isotopes">Collection of isotopes associated with element</param>
        public Element(string symbol, string name, int atomicNumber, List<Isotope> isotopes)
        {
            this.Symbol = symbol;
            this.Name = name;
            this.AtomicNumber = AtomicNumber;
            this.IsotopeDistribution = isotopes;
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
        /// Container class for Elements deserialized from Json
        /// </summary>
        private class PeriodicTable
        {
            public List<Element> Elements { get; set; }
            public int count { get; set; }
        }

        /// <summary>
        /// Data about elements is loaded from Json file.
        /// </summary>
        /// <param name="fileName">Path to Json file containing element data</param>
        /// <returns></returns>
        private static List<Element> CreateElements(string fileName = "ElementData.json")
        {
            //   Data obtained from NIST https://physics.nist.gov/cgi-bin/Compositions/stand_alone.pl?ele=&ascii=ascii&isotype=all
            string jsonString = File.ReadAllText(fileName);
            // The Json Serializer cannot deserilaize directly into a Collection, so must deserialize into a PeriodicTable object
            PeriodicTable elements = JsonSerializer.Deserialize<PeriodicTable>(jsonString);
            return elements.Elements;
        }
    }
}
