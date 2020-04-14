using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using System.Linq;
using TDkit;

namespace TDkitTest
{
    [TestClass]
    public class ChemicalFormulaTest
    {
        ChemicalFormula glucose;
        ChemicalFormula penicillin;
        ChemicalFormula ubiquitin;
        ChemicalFormula carbonic;

        [TestInitialize]
        public void TestInitialize()
        {
            glucose = new ChemicalFormula(new Dictionary<Element, int>() {
                [Element.GetElementFromSymbol("C")] = 6,
                [Element.GetElementFromSymbol("H")] = 12,
                [Element.GetElementFromSymbol("O")] = 6
            });

            penicillin = new ChemicalFormula(new Dictionary<Element, int>()
            {
                [Element.GetElementFromSymbol("C")] = 16,
                [Element.GetElementFromSymbol("H")] = 18,
                [Element.GetElementFromSymbol("N")] = 2,
                [Element.GetElementFromSymbol("O")] = 4,
                [Element.GetElementFromSymbol("S")] = 1
            });

            // C89H151N27O24 - obtained from https://pubchem.ncbi.nlm.nih.gov/compound/Ubiquitin-hexadecapeptide
            // Corresponds to peptide YNIQKESTLHLVLRLR
            ubiquitin = new ChemicalFormula(new Dictionary<Element, int>()
            {
                [Element.GetElementFromSymbol("C")] = 89,
                [Element.GetElementFromSymbol("H")] = 151,
                [Element.GetElementFromSymbol("N")] = 27,
                [Element.GetElementFromSymbol("O")] = 24
            });

            // Human Carbonic anhydrase (Uniprot P00915)
            // Sequence: MASPDWGYDDKNGPEQWSKLYPIANGNNQSPVDIKTSETKHDTSLKPISVSYNPATAKEI
            //           INVGHSFHVNFEDNDNRSVLKGGPFSDSYRLFQFHFHWGSTNEHGSEHTVDGVKYSAELH
            //           VAHWNSAKYSSLAEAASKADGLAVIGVLMKVGEANPKLQKVLDALQAIKTKGKRAPFTNF
            //           DPSTLLPSSLDFWTYPGSLTHPPLYESVTWIICKESISVSSEQLAQFRSLLSNVEGDNAV
            //           PMQHNNRPTQPLKGRTVRASF
            //
            // Submitted to Expasy ProtParam
            // Molecular weight: 28870.21
            // Chemical Formula: C1290H1970N354O394S4
            //
            // Submitted to ProSightLight
            // Average Mass: 28870.21
            // Monoisotopic Mass: 28852.39
            //
            // mMass
            // Average Mass: 28869.9095
            // Monoisotopic Mass: 28852.3882
            // Formula: C1290H1970N354O394S4
            //
            // PNNL Molecular Weight Calculator
            // Average Mass: 28869.85836
            // Monoisotopic Mass: 28852.387456
            carbonic = new ChemicalFormula(new Dictionary<Element, int>()
            {
                [Element.GetElementFromSymbol("C")] = 1290,
                [Element.GetElementFromSymbol("H")] = 1970,
                [Element.GetElementFromSymbol("N")] = 354,
                [Element.GetElementFromSymbol("O")] = 394,
                [Element.GetElementFromSymbol("S")] = 4
            });
        }

        [TestMethod]
        public void Generates_Correct_Formula_String()
        {
            Assert.AreEqual("C6H12O6", glucose.ToString());
            Assert.AreEqual("C16H18N2O4S", penicillin.ToString());
        }

        [TestMethod]
        public void Generate_Correct_Instance_From_String()
        {
            string formula = "C6H12O6";
            var glucoseTest = new ChemicalFormula(formula);

            // TODO: implement comparator for chemical formulas
            Assert.AreEqual(glucose.ToString(), glucoseTest.ToString());
        }

        [TestMethod]
        public void Input_Order_Is_Sorted()
        {
            string formula = "N4O4C2H4";
            var chem = new ChemicalFormula(formula);

            Assert.AreEqual("C2H4N4O4", chem.ToString());
        }

        [TestMethod]
        public void Unknown_Symbol_Raises_Exception()
        {
            string invalid = "C5RH12";
            ChemicalFormula formula;
            Assert.ThrowsException<ArgumentException>(() => formula = new ChemicalFormula(invalid));
        }

        [TestMethod]
        public void Lower_Case_Raises_Exception()
        {
            string invalid = "C5h12";
            ChemicalFormula formula;
            Assert.ThrowsException<ArgumentException>(() => formula = new ChemicalFormula(invalid));
        }

        [TestMethod]
        public void Correct_MW_From_Valid_Formula_Glucose()
        {
            // Obtained from PubChem https://pubchem.ncbi.nlm.nih.gov/compound/aldehydo-D-glucose#section=Chemical-and-Physical-Properties
            Assert.AreEqual(180.063388, glucose.MonoisotopicMass(), 0.00002);
            Assert.AreEqual(180.15600, glucose.AverageMass(), 0.0002);
        }

        [TestMethod]
        public void Correct_MW_From_Valid_Formula_Penicillin()
        {
            // Obtained from PubChem https://pubchem.ncbi.nlm.nih.gov/compound/5904#section=Chemical-and-Physical-Properties
            Assert.AreEqual(334.098728, penicillin.MonoisotopicMass(), 0.00002);
            Assert.AreEqual(334.39, penicillin.AverageMass(), 0.02);
        }

        [TestMethod]
        public void Correct_MW_From_Valid_Formula_Peptide()
        {
            // Obtained from PubChem
            Assert.AreEqual(1982.142529, ubiquitin.MonoisotopicMass(), 0.00002);
            Assert.AreEqual(1983.34, ubiquitin.AverageMass(), 0.01);
        }

        [TestMethod]
        public void Correct_MW_From_Valid_Formula_Carbonic()
        { 
            // Carbonic - mMass results
            Assert.AreEqual(28852.3882, carbonic.MonoisotopicMass(), 0.0001);
            Assert.AreEqual(28869.9095, carbonic.AverageMass(), 0.001);
        }
    }
}
