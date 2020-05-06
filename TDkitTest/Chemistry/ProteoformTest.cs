using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDkit.Chemistry;

namespace TDkitTest.Chemistry
{
    [TestClass]
    public class ProteoformTest
    {
        Proteoform myo;
        Proteoform h4;

        [TestInitialize]
        public void TestInitialize()
        {
            // Myoglobin (Uniprot P68082) with N-terminal methionine removed
            string proForma = "GLSDGEWQQVLNVWGKVEADIAGHGQEVLIRLFTGHPETLEKFDKFKHLKTEAEMKASEDLKKHGTVVLTALGGILKKKGHHEAELKPLAQSHATKHKIPIKYLEFISDAIIHVLHSKHPGDFGADAQGAMTKALELFRNDIAAKYKELGFQG";
            myo = new Proteoform(proForma);

            // Histone H4 (Uniprot P62805) PFR1033
            proForma = "S[Acetyl|formula:C2H2O]GRGKGGKGLGKGGAKRHRK[Dimethyl|formula:C2H4]VLRDNIQGITKPAIRRLARRGGVKRISGLIYEETRGVLKVFLENVIRDAVTYTEHAKRKTVTAMDVVYALKRQGRTLYGFGG";
            h4 = new Proteoform(proForma);
        }

        [TestMethod]
        public void Reads_Simple_Sequence()
        {
            // All amino acids are read
            Assert.AreEqual(153, myo.Length);

            // Number of serines
            Assert.AreEqual(5, myo.ResidueCount('S'));
        }

        [TestMethod]
        public void Reads_Sequence_With_Common_Modifications()
        {
            // All amino acids are read
            Assert.AreEqual(102, h4.Length);

            // Number of modifications
            Assert.AreEqual(2, h4.NumMods());
        }

        [TestMethod]
        public void Calc_Mass_Simple_Sequence()
        {
            // Value calculated in mMass
            Assert.AreEqual(16940.9650, myo.MonoisotopicMass(), 0.0001);
        }

        [TestMethod]
        public void Calc_Mass_Sequence_With_Common_Mods()
        {
            // Value calculated in ProSightLite
            Assert.AreEqual(11299.38, h4.MonoisotopicMass(), 0.01);
        }

        [TestMethod]
        public void Provide_Simple_Formula()
        {
            // Expected value calculated using ProtParam
            Assert.AreEqual("C769H1212N210O218S2", myo.GetFormula().ToString());
        }

        [TestMethod]
        public void Provide_Modification_Formula()
        {
            // Base sequence formula from ProtParam: C494H837N163O134S1
            // Plus acetyl: C2H2O
            // Plus dimetyhl: C2H4
            // Total: C498H843N163O135S1
            Assert.AreEqual("C498H843N163O135S", h4.GetFormula().ToString());
        }

    }
}
