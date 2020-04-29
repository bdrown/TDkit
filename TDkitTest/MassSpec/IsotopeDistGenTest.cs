using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDkit.MassSpec;
using TDkit;

namespace TDkitTest.MassSpec
{
    [TestClass]
    public class IsotopeDistGenTest
    {
        ChemicalFormula hexnac_form;
        ChemicalFormula ca_form;
        IsotopicDistribution hexnac_ref;
        IsotopicDistribution ca_ref;

        [TestInitialize]
        public void TestInitialize()
        {
            hexnac_form = new ChemicalFormula("C8H13N1O5");

            // Testing data from brainpy
            double[] mass = new double[] { 203.079373, 204.082545, 205.084190, 206.086971 };
            double[] intensity = new double[] { 0.901867, 0.084396, 0.012787, 0.000950 };
            hexnac_ref = new IsotopicDistribution(mass, intensity);

            // Testing data generated with mMass
            ca_form = new ChemicalFormula("C1290H1970N354O394S4");
            mass = new double[] { 28856.3995,  28857.40232, 28858.40491, 28859.40789, 28860.41027,
                                  28861.41338, 28862.41566, 28863.41873, 28864.42102, 28865.42396, 
                                  28866.42631, 28867.42913, 28868.43154, 28869.43426, 28870.43671, 
                                  28871.43936, 28872.44183, 28873.44442, 28874.44689, 28875.44944, 
                                  28876.4519,  28877.45443, 28878.45685, 28879.45936, 28880.46169, 
                                  28881.4642,  28882.46629, 28883.4688,  28884.47046, 28885.47184, 
                                  28886.47026 };
            intensity = new double[] { 0.000166925, 0.000537912, 0.00145733,  0.003399197, 0.006991671, 
                                       0.012854012, 0.021405595, 0.032612538, 0.045785523, 0.05975158,  
                                       0.072715668, 0.083213348, 0.089580389, 0.091474523, 0.088461882, 
                                       0.081698453, 0.071838131, 0.060647866, 0.04896059,  0.038109724,
                                       0.028467622, 0.020570442, 0.014305486, 0.009644266, 0.006268659,
                                       0.003949935, 0.002398549, 0.001408038, 0.000795686, 0.000406887, 
                                       0.00012157 };
            ca_ref = new IsotopicDistribution(mass, intensity);

        }

        [TestMethod]
        public void Mercury_HexNAc()
        {
            IIsotopeDistGenerator gen = new Mercury7();

            IsotopicDistribution hexnac_mercury = gen.GenerateIsotopicDistribution(hexnac_form);

            for(int i = 0; i < hexnac_ref.Length; i++)
            {
                Assert.AreEqual(hexnac_ref.Masses[i], hexnac_mercury.Masses[i], 0.0001);
                Assert.AreEqual(hexnac_ref.Intensities[i], hexnac_mercury.Intensities[i], 0.0001);
            }
        }

        [TestMethod]
        public void Mercury_Carbonic()
        {
            IIsotopeDistGenerator gen = new Mercury7();

            IsotopicDistribution ca_mercury = gen.GenerateIsotopicDistribution(ca_form);

            for (int i = 0; i < ca_ref.Length; i++)
            {
                // mMass doesn't calculate the intensity of the monoisotopic mass peak so need to offset by 4
                Assert.AreEqual(ca_ref.Masses[i], ca_mercury.Masses[i+4], 0.01);
                Assert.AreEqual(ca_ref.Intensities[i], ca_mercury.Intensities[i+4], 0.01);
            }
        }

        [TestMethod]
        public void Brain_HexNAc()
        {
            IIsotopeDistGenerator gen = new Brain();

            IsotopicDistribution hexnac_mercury = gen.GenerateIsotopicDistribution(hexnac_form);

            for (int i = 0; i < hexnac_ref.Length; i++)
            {
                Assert.AreEqual(hexnac_ref.Masses[i], hexnac_mercury.Masses[i], 0.001);
                Assert.AreEqual(hexnac_ref.Intensities[i], hexnac_mercury.Intensities[i], 0.001);
            }
        }

        [TestMethod]
        public void Brain_Carbonic()
        {
            IIsotopeDistGenerator gen = new Brain();

            IsotopicDistribution ca_brain = gen.GenerateIsotopicDistribution(ca_form);

            for (int i = 0; i < ca_ref.Length; i++)
            {
                // mMass doesn't calculate the intensity of the monoisotopic mass peak so need to offset by 4
                Assert.AreEqual(ca_ref.Masses[i], ca_brain.Masses[i + 4], 0.01);
                Assert.AreEqual(ca_ref.Intensities[i], ca_brain.Intensities[i + 4], 0.01);
            }
        }
    }
}
