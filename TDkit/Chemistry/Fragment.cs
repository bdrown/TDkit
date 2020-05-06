using System;
using System.Collections.Generic;

namespace TDkit.Chemistry
{
    /// <summary>
    /// Basic implementation of a Fragment.
    /// 
    /// Currently support support a,b,c,x,y,z ions.
    /// </summary>
    public class Fragment : Polymer
    {
        /// <summary>
        /// The type of fragment.
        /// </summary>
        public char Type { get; }

        /// <summary>
        /// The position of the fragmentation in the precursor
        /// </summary>
        public int Position { get; }
        
        /// <summary>
        /// Initializes and instance of Fragment
        /// </summary>
        /// <param name="sequence">Base sequence of the fragment</param>
        /// <param name="residues">List of Residue that make up the base sequence</param>
        /// <param name="mods">List of modifications on the fragment</param>
        /// <param name="type">The type of fragment (b, y, c, z, a, x)</param>
        /// <param name="pos">Position of the fragmentation in the precursor sequence</param>
        public Fragment(string sequence, List<Residue> residues, List<Modification> mods, char type, int pos) 
            : base(sequence, residues, mods)
        {
            this.Type = type;
            this.Position = pos;
            var tuple = FragmentMods(type);
            if (! String.IsNullOrEmpty(tuple.formula)) 
                this.residueMods.Add(new Modification(tuple.formula, tuple.position));
        }

        /// <summary>
        /// Indicates whether a fragmentation is N- or C-terminal.
        /// 
        /// N-terminal fragmentations return 1 and C-terminal fragmentations return -1
        /// Unknown fragmentation types return 0
        /// </summary>
        /// <param name="fragtype">Type of fragmentation</param>
        /// <returns>1 for N-terminal fragmentations, -1 for C-terminal fragmentations, 0 for unknown</returns>
        public static int Direction(char fragtype)
        {
            switch (fragtype)
            {
                case 'b':
                case 'c':
                case 'a':
                    return 1;

                case 'x':
                case 'y':
                case 'z':
                    return -1;

                default:
                    return 0;
            }
        }

        /// <summary>
        /// Provides the modifications that arise from the fragmentation
        /// </summary>
        /// <param name="type">Type of fragment</param>
        /// <returns>Tuple containing the modification string and position of mod</returns>
        public static (string formula, int position) FragmentMods(char type)
        {
            switch (type)
            {
                case 'b':
                    return ("acylium|formula:H-1", -1);
                case 'y': 
                    return ("ammonium|formula:H", 1);

                 
                case 'c':
                        return ("amido|formula:NH2", -1);

                case 'z':
                    return ("carbenium|formula:N-1H-2", 1);


                case 'a':
                        return ("iminium|formula:C-1H-1O-1", -1);

                case 'x':
                        return ("acylium|formula:COH-1", 1);

                default:
                    return ("None|formula:C0", 0);

            }
        }

    }
}
