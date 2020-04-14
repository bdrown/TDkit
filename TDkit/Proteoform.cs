using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace TDkit
{
    /// <summary>
    /// Basic implementation of a Proteoform. Contains a base sequence and indexed modifications.
    /// </summary>
    public class Proteoform
    {
        /// <summary>
        /// String containing the base sequence of the proteoform.
        /// </summary>
        public string Sequence { get; }

        /// <summary>
        /// Collection of the residues that make up the proteoform.
        /// </summary>
        private List<Residue> residues;

        /// <summary>
        /// Collection of chemical modifications on residues. Keys provide location of mod in sequence.
        /// Key 0 provides N-terminal modification and key -1 provides C-terminal modification.
        /// </summary>
        private List<Modification> residueMods;

        /// <summary>
        /// Number of residues that make up the proteoform
        /// </summary>
        public int Length {
            get
            {
                return residues.Count;
            }
        }

        /// <summary>
        /// Initializes an instance of Proteoform from ProForma string.
        /// </summary>
        /// <param name="proForma"></param>
        public Proteoform(string proForma)
        {
            var results = ParseProForma(proForma);
            this.Sequence = results.BaseSequence;
            this.residues = results.Residues;
            this.residueMods = results.Mods;
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
        /// ProForma parser with limited functionality. Will look for tags 
        /// denoted with square brackets: [ ]. Tags are assumed to follow 
        /// immediately after the residue being modified.
        /// </summary>
        /// <param name="proForma"></param>
        /// <returns></returns>
        public static (string BaseSequence, List<Residue> Residues, List<Modification> Mods) ParseProForma(string proForma)
        {
            StringBuilder sequence = new StringBuilder();
            StringBuilder tag = new StringBuilder();
            List<Residue> residues = new List<Residue>();
            List<Modification> mods = new List<Modification>();
            bool inTag = false;
            int position = 0;

            // Add default N- and C-terminal modifications
            mods.Add(new Modification("Amine|formula:H", 0));
            mods.Add(new Modification("Acid|formula:OH", -1));
            
            foreach (char c in proForma)
            {
                if (inTag)
                {
                    // Tags within tags are not allowed, so should not see open bracket here
                    if (c == '[') throw new ArgumentException("Tags are incorrectly formatted, tag within tag.", proForma);

                    if (c == ']')
                    {
                        // Closing of a tag
                        mods.Add(new Modification(tag.ToString(), position));
                        tag.Clear();
                        inTag = false;
                    }
                    else
                    {
                        // Add to the tag string but do not advance position
                        tag.Append(c);
                    }
                }
                else
                {
                    if ( c== ']') throw new ArgumentException("Tags are incorrectly formatted, hanging close of tag.", proForma);
                    if (c == '[')
                    {
                        // Begin tag
                        inTag = true;
                    }
                    else if (c == '-')
                    {
                        // Indicates C-terminus
                        position = -1;
                    }
                    else
                    {
                        // Not part of a tag, just add symbol to lists
                        sequence.Append(c);
                        residues.Add(Residue.GetResidue(c));

                        // Advance position in sequence
                        position++;
                    }
                }
            }

            return (sequence.ToString(), residues, mods);
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
    }
}
