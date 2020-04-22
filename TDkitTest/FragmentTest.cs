using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDkit;

namespace TDkitTest
{
    [TestClass]
    public class FragmentTest
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
        public void Fragment_Correct_Sequence_B()
        {
            int index = 5;

            var frag = myo.MakeFragment(index, 'b');
            Assert.AreEqual("GLSDG", frag.Sequence);
        }

        [TestMethod]
        public void Fragment_Correct_Sequence_Y()
        {
            int index = 5;

            var frag = myo.MakeFragment(index, 'y');
            Assert.AreEqual("LGFQG", frag.Sequence);
        }

        [TestMethod]
        public void Fragment_Correct_Length_B()
        {
            int index = 5;

            var frag = myo.MakeFragment(index, 'b');
            Assert.AreEqual(5, frag.Length);
        }

        [TestMethod]
        public void Fragment_Correct_Length_Y()
        {
            int index = 5;

            var frag = myo.MakeFragment(index, 'y');
            Assert.AreEqual(5, frag.Length);
        }

        [TestMethod]
        public void Fragment_Correct_Mass_B()
        {
            int index = 5;
            var mass = 430.1932 - 1.00782503223;

            var frag = myo.MakeFragment(index, 'b');
            Assert.AreEqual(mass, frag.MonoisotopicMass(), 0.001);
        }

        [TestMethod]
        public void Fragment_Correct_Mass_Y()
        {
            int index = 5;
            var mass = 521.2718 - 1.00782503223;

            var frag = myo.MakeFragment(index, 'y');
            Assert.AreEqual(mass, frag.MonoisotopicMass(), 0.001);
        }

        [TestMethod]
        public void Fragment_of_a_Fragment()
        {
            // GLSDGEWQQV
            var frag1 = myo.MakeFragment(10, 'b');

            var frag2 = frag1.MakeFragment(5, 'y');
            Assert.AreEqual("EWQQV", frag2.Sequence);
        }

        // TODO: write tests for other fragmentation types
    }
}
