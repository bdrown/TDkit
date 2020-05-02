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

        public Transition(Proteoform parent, Fragment frag, int parentCharge, int fragCharge)
        {
            this.Precursor = parent;
            this.Fragment = frag;
            this.PrecursorCharge = parentCharge;
            this.FragmentCharge = fragCharge;
        }
    }
}
