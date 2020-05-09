using System;
using System.Collections.Generic;
using System.Text;
using TDkit.Chemistry;

namespace TDkit.MassSpec
{
    public interface IIsotopeDistGenerator
    {
        IsotopicDistribution GenerateIsotopicDistribution(ChemicalFormula formula);

        ChargedIsotopicDistribution GenerateChargedIsotopicDistribution(ChemicalFormula formula, int charge);

    }
}
