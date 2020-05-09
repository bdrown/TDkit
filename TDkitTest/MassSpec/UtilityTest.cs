using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDkit.MassSpec;

namespace TDkitTest.MassSpec
{
    [TestClass]
    public class UtilityTest
    {
        [TestMethod]
        public void Valid_MasstoMz_Myo()
        {
            // myoglobin, test data generated with mMass
            Assert.AreEqual(1695.2040648, Utilities.MassToMz(16941.9678834, 10), 0.00001);
        }

        [TestMethod]
        public void Valid_MztoMass_Myo()
        {
            // myoglobin, test data generated with mMass
            Assert.AreEqual(16941.9678834, Utilities.MztoMass(1695.2040648, 10), 0.00001);
        }

        [TestMethod]
        public void Negative_MasstoMz_Carbonic()
        {
            // carbonic anhydrase, -10 charge state, generated with mMass
            Assert.AreEqual(2884.63271632, Utilities.MassToMz(28856.39992770, 10, false), 0.00001);
        }

        [TestMethod]
        public void Negative_MztoMass_Carbonic()
        {
            // carbonic anhydrase, -10 charge state, generated with mMass
            Assert.AreEqual(28856.39992770, Utilities.MztoMass(2884.63271632, 10, false), 0.00001);
        }
    }
}
