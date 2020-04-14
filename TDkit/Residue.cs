using System;
using System.Collections.Generic;
using System.Linq;

namespace TDkit
{
    public class Residue
    {
        /// <summary>
        /// Single character representation of amino acid
        /// </summary>
        public char Symbol { get; }

        /// <summary>
        /// Long name for amino acid
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Molecular formula for the residue that represents the collection of elements in the molecule
        /// </summary>
        private ChemicalFormula baseFormula { get; }

        /// <summary>
        /// Data for natural amino acids stored in memory.
        /// </summary>
        private static List<Residue> aminoAcids = CreateResidues();

        /// <summary>
        /// Initializes an instance of a residue.
        /// </summary>
        /// <param name="symbol">Single character symbol for residue</param>
        /// <param name="name">Long name of residue</param>
        /// <param name="chemicalFormula">Chemical formula for residue written in chemForma</param>
        public Residue(char symbol, string name, string chemicalFormula)
        {
            this.Symbol = symbol;
            this.Name = name;
            this.baseFormula = new ChemicalFormula(chemicalFormula);
        }

        /// <summary>
        /// Provides the monoisotopic mass of the residue
        /// </summary>
        /// <returns></returns>
        public double MonoisotopicMass()
        {
            return baseFormula.MonoisotopicMass();
        }

        /// <summary>
        /// Provides the average mass of the residue
        /// </summary>
        /// <returns></returns>
        public double AverageMass()
        {
            return baseFormula.AverageMass();
        }

        /// <summary>
        /// Gets the residue using one character symbol
        /// </summary>
        /// <param name="symbol">Symbol of the residue</param>
        /// <returns></returns>
        public static Residue GetResidue(char symbol)
        {
            IEnumerable<Residue> toReturn =
                from residue in aminoAcids
                where residue.Symbol.Equals(symbol)
                select residue;

            // If no amino acid was found that matches the provided symbol, throw an ArgumentException
            if (toReturn.Count() == 0)
                throw new ArgumentException($"{symbol} is not a known amino acid.", "symbol");

            // Query result should only have one residue, return it
            // Should this be a deep copy?
            return toReturn.First();
        }

        /// <summary>
        /// Creates the list of natural amino acids in memory
        /// </summary>
        /// <returns>List containing Residue objects corresponding to natural amino acids</returns>
        private static List<Residue> CreateResidues()
        {
            List<Residue> molecules = new List<Residue>();

            // Common amino acids
            // Obtained from https://www.sigmaaldrich.com/life-science/metabolomics/learning-center/amino-acid-reference-chart.html
            molecules.Add(new Residue('A', "Alanine", "C3H5NO"));
            molecules.Add(new Residue('R', "Arginine", "C6H12N4O"));
            molecules.Add(new Residue('N', "Asparagine", "C4H6N2O2"));
            molecules.Add(new Residue('D', "Aspartic acid", "C4H5NO3"));
            molecules.Add(new Residue('C', "Cysteine", "C3H5NOS"));
            molecules.Add(new Residue('Q', "Glutamine", "C5H8N2O2"));
            molecules.Add(new Residue('E', "Glutamic acid", "C5H7NO3"));
            molecules.Add(new Residue('G', "Glycine", "C2H3NO"));
            molecules.Add(new Residue('H', "Histidine", "C6H7N3O"));
            molecules.Add(new Residue('I', "Isoleucine", "C6H11NO"));
            molecules.Add(new Residue('L', "Leucine", "C6H11NO"));
            molecules.Add(new Residue('K', "Lysine", "C6H12N2O"));
            molecules.Add(new Residue('M', "Methionine", "C5H9NOS"));
            molecules.Add(new Residue('F', "Phenylalanine", "C9H9NO"));
            molecules.Add(new Residue('P', "Proline", "C5H7NO"));
            molecules.Add(new Residue('S', "Serine", "C3H5NO2"));
            molecules.Add(new Residue('T', "Threonine", "C4H7NO2"));
            molecules.Add(new Residue('W', "Tryptophan", "C11H10N2O"));
            molecules.Add(new Residue('Y', "Tyrosine", "C9H9NO2"));
            molecules.Add(new Residue('V', "Valine", "C5H9NO"));
            molecules.Add(new Residue('O', "Hydroxyproline", "C5H7NO2"));

            return molecules;
        }
    }
}
