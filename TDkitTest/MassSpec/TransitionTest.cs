using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDkit.MassSpec;
using TDkit.Chemistry;

namespace TDkitTest.MassSpec
{
    [TestClass]
    public class TransitionTest
    {
        Transition trans;

        [TestInitialize]
        public void TestInitialize()
        {
            // myoglobin
            string proForma = "GLSDGEWQQVLNVWGKVEADIAGHGQEVLIRLFTGHPETLEKFDKFKHLKTEAEMKASEDLKKHGTVVLTALGGILKKKGHHEAELKPLAQSHATKHKIPIKYLEFISDAIIHVLHSKHPGDFGADAQGAMTKALELFRNDIAAKYKELGFQG";
            Proteoform myo = new Proteoform(proForma);

            // create b5 ion
            var frag = myo.MakeFragment(5, 'b');

            trans = new Transition(myo, frag, 7, 4);
        }

        [TestMethod]
        public void Transition_Sequences()
        {
            string proForma = "GLSDGEWQQVLNVWGKVEADIAGHGQEVLIRLFTGHPETLEKFDKFKHLKTEAEMKASEDLKKHGTVVLTALGGILKKKGHHEAELKPLAQSHATKHKIPIKYLEFISDAIIHVLHSKHPGDFGADAQGAMTKALELFRNDIAAKYKELGFQG";

            Assert.AreEqual(proForma, trans.Precursor.Sequence);
            Assert.AreEqual("GLSDG", trans.Fragment.Sequence);
        }

        [TestMethod]
        public void Transition_Charges()
        {
            Assert.AreEqual(7, trans.PrecursorCharge);
            Assert.AreEqual(4, trans.FragmentCharge);
        }
    }
}
