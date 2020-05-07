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
    }
}
