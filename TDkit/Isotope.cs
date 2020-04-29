using System;

namespace TDkit
{
    /// <summary>
    /// Representation of a single isotope of an element. Contains a unique atomic weight,
    /// relative atomic mass, and natural abundance. 
    /// </summary>
    public class Isotope : IEquatable<Isotope>, IComparable<Isotope>
    {
        /// <summary>
        /// Atomic number of an isotope. Equals the number of protons.
        /// </summary>
        public int AtomicNumber { get; }

        /// <summary>
        /// The atomic weight of an isotope is the sum of the count of protons and neutrons.
        /// </summary>
        public int AtomicWeight { get;}
        
        /// <summary>
        /// The relative atomic mass of an isotope is the mass normalized to carbon-12.
        /// </summary>
        public double RelativeAtomicMass { get;}

        /// <summary>
        /// Natural abundance of an isotope is used to calculate the average mass of an 
        /// element as well as the isotopic distribution.
        /// </summary>
        public double Abundance { get;}

        /// <summary>
        /// The number of neutrons in this isotope.
        /// </summary>
        public int Neutrons
        {
            get
            {
                return AtomicWeight - AtomicNumber;
            }
        }

        /// <summary>
        /// Difference in neutrons from most abundant isotope
        /// </summary>
        public int NeutronShift { get; set; }

        /// <summary>
        /// Initializes an instance of the Isotope class.
        /// </summary>
        /// <param name="atomicWeight">The atomic weight.</param>
        /// <param name="relativeAtomicMass">The relative atomic mass.</param>
        /// <param name="abundance">The natural abundance.</param>
        public Isotope(int atomicNumber, int atomicWeight, double relativeAtomicMass, double abundance)
        {
            this.AtomicNumber = atomicNumber;
            this.AtomicWeight = atomicWeight;
            this.RelativeAtomicMass = relativeAtomicMass;
            this.Abundance = abundance;
        }

        /// <summary>
        /// Sets the number of additional neutrons in isotope compared to "typical"
        /// isotope, which most frequently refers to the most abundant species.
        /// </summary>
        /// <param name="typical">Number of neutrons in typical isotope</param>
        public void SetNeutronShift(int typical)
        {
            this.NeutronShift = this.Neutrons - typical;
        }

        /// <summary>
        /// Implementation of Equals to satisfy IEquatable interface.
        /// All properties must be equal.
        /// </summary>
        /// <param name="other">Isotope instance to equate</param>
        /// <returns>True if isotopes are equal. False if not.</returns>
        public bool Equals(Isotope other)
        {
            if (other == null) return false;
            return (this.AtomicNumber == other.AtomicNumber && this.AtomicWeight == other.AtomicWeight && this.Abundance == other.Abundance);
        }
        
        /// <summary>
        /// Implementation of CompareTo to satisfy IComparable interface.
        /// The smallest isotope goes first. The first comparison is based on
        /// atomic number but then looks at relative mass. Natural isotopes
        /// should go first.
        /// </summary>
        /// <param name="other">Isotope instance to compare to.</param>
        /// <returns>-1 if this Isotope is first, 0 if they are equal, 1 if the
        /// other isotope is first</returns>
        public int CompareTo(Isotope other)
        {
            if (other == null) return 1;

            // Most of the time, comparisons are between isotopes of the same element
            if (this.AtomicNumber == other.AtomicNumber)
            {
                // The smallest, naturally occuring isotope should go first. Synthetic isotopes go last.
                if (this.Abundance > 0 && other.Abundance > 0)
                {
                    return this.RelativeAtomicMass.CompareTo(other.RelativeAtomicMass) * -1;
                }
                else
                {
                    return this.Abundance.CompareTo(other.Abundance);
                }
            }
            else
            {
                // If comparing two different elements, the smaller one goes first
                return this.AtomicNumber.CompareTo(other.AtomicNumber) * -1;
            }
        }
    }
}
