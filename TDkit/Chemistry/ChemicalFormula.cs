﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TDkit.Chemistry
{
    public class ChemicalFormula
    {
        /// <summary>
        /// Dictionary of Elements that relates the identity of each element
        /// with its cardinality.
        /// </summary>
        private Dictionary<Element, int> elements;

        /// <summary>
        /// Intializes a ChemicalFormula instance from a Dictionary of Elements
        /// </summary>
        /// <param name="elements">Dictionary of Elements with their cardinalities</param>
        public ChemicalFormula(Dictionary<Element,int> elements)
        {
            this.elements = elements;
        }

        /// <summary>
        /// Intiailizes a ChemicalFormula instance from chemForma string
        /// </summary>
        /// <param name="chemFormaString">String that represents the chemical
        /// formula in Hill notation or chemForma</param>
        public ChemicalFormula(string chemForma)
        {
            this.elements = ParseChemForma(chemForma);
        }

        /// <summary>
        /// Empty initializer for a chemical formula with no atoms
        /// </summary>
        public ChemicalFormula()
        {
            this.elements = new Dictionary<Element, int>();
        }

        /// <summary>
        /// Parses a chemical formula in Hill notation into a dictionary containing the count of each element.
        /// Does not support condensed formula, repeated elements, or specific isotopes.
        /// </summary>
        /// <param name="chemForma">Chemical formula in Hill notation or chemForma</param>
        /// <returns>Dictionary of Elements that relates identity of element with cardinality.</returns>
        public static Dictionary<Element, int> ParseChemForma(string chemForma)
        {
            // TODO: provide support for condensed formula or repeated elements
            // TODO: provide support for recognizing isotope enriched elements

            Dictionary<Element, int> toReturn = new Dictionary<Element, int>();

            // In case of empty string, just return empty dictionary of elements
            if (String.IsNullOrEmpty(chemForma))
                return toReturn;

            // Perform some error-checking
            // Lower case immediately following digit
            var match = Regex.Match(chemForma, @"\d*[a-z]");
            if (match.Success)
                throw new ArgumentException($"Invalid Element symbol. Must be upper case: {chemForma}", "chemForma");

            // Regex recognizes a capital letter followed one or none lower case letter and one or none number
            MatchCollection matches = Regex.Matches(chemForma, @"([A-Z][a-z]?)(-?\d*)");

            // In this case, the string is not empty but the string is not formated like a formula
            if (matches.Count == 0)
                throw new ArgumentException($"No matches were found in formula: {chemForma}", "chemForma");

            foreach (Match m in matches)
            {
                // Each match contains two groups. The first group contains the element symbol
                var symbol = m.Groups[1].Value;

                // The second group contains the cardinality of the element. If no number is matched,
                // then assume one atom.
                var cardinality = String.IsNullOrEmpty(m.Groups[2].Value) ? "1" : m.Groups[2].Value;

                toReturn.Add(Element.GetElementFromSymbol(symbol), Int32.Parse(cardinality));
            }

            return toReturn;
        }

        /// <summary>
        /// Provide the chemical formula in Hill notation. 
        /// </summary>
        /// <returns>String containing Hill notation of chemical formula</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            List<Element> keys = elements.Keys.ToList();
            keys.Sort();

            foreach (var element in keys)
            {
                sb.Append(element.Symbol);

                // When there is only one of an element, then no number needs to be added
                string cardinality = elements[element] == 1 ? "" : $"{elements[element]}";
                sb.Append(cardinality);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Provides the monoisotopic mass of a chemical formula
        /// </summary>
        /// <returns>Chemical formula monoisotopic mass</returns>
        public double MonoisotopicMass()
        {
            return elements.Sum(element => element.Key.MonoisotopicMass() * element.Value);
        }

        /// <summary>
        /// Provides the average mass of a chemical formula
        /// </summary>
        /// <returns>Chemical formula average mass</returns>
        public double AverageMass()
        {
            return elements.Sum(element => element.Key.AverageMass() * element.Value);
        }

        public Dictionary<Element, int> GetElements()
        {
            return elements;
        }

        /// <summary>
        /// Merge with another chemical formula
        /// </summary>
        /// <param name="other">The other formula to merge with</param>
        public void Merge(ChemicalFormula other)
        {
            // Iterate through elements in the other formula, find matches, and update
            foreach (var kvp in other.GetElements())
            {
                if (this.elements.ContainsKey(kvp.Key))
                {
                    // If this formula already contains the element, add to count
                    this.elements[kvp.Key] += kvp.Value;
                }
                else
                {
                    // IF this formula does not contain the element, append key value pair
                    this.elements.Add(kvp.Key, kvp.Value);
                }
            }
        }
    }
}
