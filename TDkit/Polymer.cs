using System;
using System.Collections.Generic;
using System.Linq;

namespace TDkit
{
    public class Polymer
    {
        /// <summary>
        /// String containing the base sequence of the polymer.
        /// </summary>
        public string Sequence;

        /// <summary>
        /// Collection of the residues that make up the polymer.
        /// </summary>
        public List<Residue> residues;

        /// <summary>
        /// Collection of chemical modifications on residues. Keys provide location of mod in sequence.
        /// Key 0 provides first-terminal modification and key -1 provides last-terminal modification.
        /// </summary>
        public List<Modification> residueMods;

        /// <summary>
        /// Number of residues that make up the proteoform
        /// </summary>
        public int Length => residues.Count;

        /// <summary>
        /// Initializes an instance of Polymer from all private properties.
        /// </summary>
        /// <param name="sequence">String that represents the sequence of the polymer</param>
        /// <param name="residues">List of all the individual residues that make up polymer</param>
        /// <param name="mods">List of mods that are on residues</param>
        public Polymer(string sequence, List<Residue> residues, List<Modification> mods)
        {
            this.Sequence = sequence;
            this.residues = residues;
            this.residueMods = mods;
        }

        public Polymer()
        {
            //TODO: use inherited constructors to call base constructor explicitly so this empty constructor is no necessary
        }

        /// <summary>
        /// Fragmentation of the polymer
        /// </summary>
        /// <param name="index">Location of fragmentation in polymer</param>
        /// <param name="fragtype">Type of fragmentation</param>
        /// <returns></returns>
        public Fragment MakeFragment(int index, char fragtype)
        {
            // Error-check the index
            if (index < 1)
                throw new ArgumentOutOfRangeException("Location of fragmentation is less than 1", "index");
            if (index > this.Length)
                throw new ArgumentOutOfRangeException("Location of fragmentation is greater than length of polymer", "index");

            // 1 if N-terminal, -1 if C-terminal, 0 if unknown fragtype
            int direction = Fragment.Direction(fragtype);
            if (direction == 0)
                throw new ArgumentException("Fragment type is not recognized", "fragtype");

            
            if (direction > 0)
            {
                // For N-terminal
                var seq = this.Sequence.Substring(0, index);
                var res = this.residues.GetRange(0, index);
                IEnumerable<Modification> mods =
                    from mod in this.residueMods
                    where mod.Position <= index && mod.Position != -1
                    select mod;

                return new Fragment(seq, res, mods.ToList(), fragtype, index);
            }
            else
            {
                // For C-terminal
                var seq = this.Sequence.Substring(this.Length - index);
                var res = this.residues.GetRange(this.Length - index, index);
                IEnumerable<Modification> mods =
                    from mod in this.residueMods
                    where mod.Position >= this.Length - index || mod.Position == -1
                    select mod;

                return new Fragment(seq, res, mods.ToList(), fragtype, index);
            } 
        }

        /// <summary>
        /// Provides the number of residues of a particular type.
        /// </summary>
        /// <param name="symbol">One-character symbol of residue to count</param>
        /// <returns>Number of occurances of a residue</returns>
        public int ResidueCount(char symbol)
        {
            int count = 0;
            foreach (char c in Sequence)
            {
                if (c == symbol) count++;
            }
            return count;
        }

        /// <summary>
        /// Provides the total number of modifications on proteoform. Total 
        /// does not include the default N- and C-terminal modifications (NH2- and -CO2H).
        /// </summary>
        /// <returns></returns>
        public int NumMods()
        {
            // Don't count the default N- and C-terminal mods
            return residueMods.Count - 2;
        }

        /// <summary>
        /// Provides the monoisotopic mass of a proteoform.
        /// </summary>
        /// <returns>Monoisotopic mass</returns>
        public double MonoisotopicMass()
        {
            // Sum mass of base sequence
            double mass = residues.Sum(residue => residue.MonoisotopicMass());

            // Sum mass of all modifications
            mass += residueMods.Sum(mod => mod.MonoisotopicMass());

            return mass;
        }

        /// <summary>
        /// Provides the average mass of a proteoform.
        /// </summary>
        /// <returns>Average mass</returns>
        public double AverageMass()
        {
            double mass = residues.Sum(residue => residue.AverageMass());
            mass += residueMods.Sum(mod => mod.AverageMass());

            return mass;
        }

        /// <summary>
        /// Provides the combined chemical formula of the base sequence and all
        /// of the modifications.
        /// </summary>
        /// <returns>Simplified ChemicalFormula for polymer</returns>
        public ChemicalFormula GetFormula()
        {
            ChemicalFormula aggregator = new ChemicalFormula();

            // Generate base formula
            foreach (var res in this.residues)
            {
                aggregator.Merge(res.GetFormula());
            }

            // Add on all of the mods
            foreach (var mod in this.residueMods)
            {
                aggregator.Merge(mod.GetFormula());
            }

            return aggregator;
        }
    }
}
