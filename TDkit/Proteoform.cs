using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace TDkit
{
    /// <summary>
    /// Basic implementation of a Proteoform. Contains a base sequence and indexed modifications.
    /// </summary>
    public class Proteoform : Polymer
    {
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
        /// ProForma parser with limited functionality. Will look for tags 
        /// denoted with square brackets: [ ]. Tags are assumed to follow 
        /// immediately after the residue being modified.
        /// </summary>
        /// <param name="proForma"></param>
        /// <returns></returns>
        private static (string BaseSequence, List<Residue> Residues, List<Modification> Mods) ParseProForma(string proForma)
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


    }
}
