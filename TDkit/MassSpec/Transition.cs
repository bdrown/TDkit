using System;
using System.Collections.Generic;
using System.Text;

namespace TDkit.MassSpec
{
    class Transition
    {
        public int PrecursorCharge { get; }

        public int FragmentCharge { get; }

        public Proteoform Precursor { get; }

        public Fragment Fragment { get; }

        public ChargedIsotopicDistribution PrecursorDistribution { get; }

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
            this.Precursor = parent;
            this.Fragment = frag;
            this.PrecursorCharge = parentCharge;
            this.FragmentCharge = fragCharge;

            IIsotopeDistGenerator gen = new Mercury7();
            this.PrecursorDistribution = gen.GenerateChargeIsotopicDistribution(parent.GetFormula(), parentCharge);
            this.FragmentDistribution = gen.GenerateChargeIsotopicDistribution(frag.GetFormula(), fragCharge);
        }

        
    }
}
