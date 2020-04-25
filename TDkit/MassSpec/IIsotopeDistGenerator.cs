using System;
using System.Collections.Generic;
using System.Text;

namespace TDkit.MassSpec
{
    public interface IIsotopeDistGenerator
    {
        IsotopicDistribution GenerateIsotopicDistribution(ChemicalFormula formula);

        ChargedIsotopicDistribution GenerateChargeIsotopicDistribution(ChemicalFormula formula, int charge);

    }
}
