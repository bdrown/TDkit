using System;
using System.Collections.Generic;
using System.Text;

namespace TDkit
{
    public class Modification
    {
        private ChemicalFormula formula;

        public string Name { get; }

        public int Position { get;  }

        public double MonoisotopicMass()
        {
            return formula.MonoisotopicMass();
        }

        public double AverageMass()
        {
            return formula.AverageMass();
        }

        /// <summary>
        /// Initializes an instance of a modification from a ProForma tag.
        /// Currently, the only supported format is [Acetyl|formula:C2H2O].
        /// </summary>
        /// <param name="tag"></param>
        public Modification(string tag, int position)
        {
            this.Position = position;
            string[] words = tag.Split('|');
            this.Name = words[0];
            this.formula = new ChemicalFormula(words[1].Split(':')[1]);
        }

        /// <summary>
        /// Provides the chemical formula of the modification
        /// </summary>
        /// <returns></returns>
        public ChemicalFormula GetFormula()
        {
            return formula;
        }
    }
}
