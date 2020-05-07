using TDkit.Chemistry;

namespace TDkit.MassSpec
{
    /// <summary>
    /// Implementation of a transition (pairing precursor and fragment ions)
    /// Also contains information about their isotopic distributions.
    /// </summary>
    public class Transition
    {
        /// <summary>
        /// Charge of the precursor ion
        /// </summary>
        public int PrecursorCharge { get; }

        /// <summary>
        /// Charge of the fragment ion
        /// </summary>
        public int FragmentCharge { get; }

        /// <summary>
        /// Structural information about the precursor
        /// </summary>
        public Proteoform Precursor { get; }

        /// <summary>
        /// Structural information about the fragment
        /// </summary>
        public Fragment Fragment { get; }

        /// <summary>
        /// Isotopic distribution of the precursor ion
        /// </summary>
        public ChargedIsotopicDistribution PrecursorDistribution { get; }

        /// <summary>
        /// Isotopic distribution of the fragment ion
        /// </summary>
        public ChargedIsotopicDistribution FragmentDistribution { get; }

        /// <summary>
        /// Initializes a Transition instance.
        /// </summary>
        /// <param name="parent">Proteoform that represents parent ion</param>
        /// <param name="frag">Fragment that represents fragment ion</param>
        /// <param name="parentCharge">Charge of parent ion</param>
        /// <param name="fragCharge">Charge of fragment ion</param>
        public Transition(Proteoform parent, Fragment frag, int parentCharge, int fragCharge)
        {
            // Set member values
            this.Precursor = parent;
            this.Fragment = frag;
            this.PrecursorCharge = parentCharge;
            this.FragmentCharge = fragCharge;

            // Generate isotopic distributions using the Mercury7 algorithm
            IIsotopeDistGenerator gen = new Mercury7();
            this.PrecursorDistribution = gen.GenerateChargedIsotopicDistribution(parent.GetFormula(), parentCharge);
            this.FragmentDistribution = gen.GenerateChargedIsotopicDistribution(frag.GetFormula(), fragCharge);
        }
    }
}
