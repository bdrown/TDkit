using System;
using System.Collections.Generic;
using System.Text;

namespace TDkit
{
    /// <summary>
    /// Representation of a single isotope of an element. Contains a unique atomic weight,
    /// relative atomic mass, and natural abundance. 
    /// </summary>
    public class Isotope
    {
        /// <summary>
        /// The atomic weight of an isotope is the sum of the count of protons and neutrons.
        /// </summary>
        public int AtomicWeight { get; set; }
        
        /// <summary>
        /// The relative atomic mass of an isotope is the mass normalized to carbon-12.
        /// </summary>
        public double RelativeAtomicMass { get; set; }

        /// <summary>
        /// Natural abundance of an isotope is used to calculate the average mass of an 
        /// element as well as the isotopic distribution.
        /// </summary>
        public double Abundance { get; set; }

        /// <summary>
        /// An empty constructor for the Isotope class.
        /// </summary>
        public Isotope() 
        {
            // An empty constructor is necessary for reading in data through JSON deserialization.
        }

        /// <summary>
        /// Initializes an instance of the Isotope class.
        /// </summary>
        /// <param name="atomicWeight">The atomic weight.</param>
        /// <param name="relativeAtomicMass">The relative atomic mass.</param>
        /// <param name="abundance">The natural abundance.</param>
        public Isotope(int atomicWeight, double relativeAtomicMass, double abundance)
        {
            this.AtomicWeight = atomicWeight;
            this.RelativeAtomicMass = relativeAtomicMass;
            this.Abundance = abundance;
        }
    }
}
