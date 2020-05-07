using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDkit.MassSpec;
using TDkit.Chemistry;

namespace TDkitTest.MassSpec
{
    [TestClass]
    public class ChargedIsotopeDistTest
    {

        [TestMethod]
        public void Test_Single_Charged_Distribution_HexNAc()
        {
            ChemicalFormula hexnac_form = new ChemicalFormula("C8H13N1O5");

            // Testing data generated with mMass
            double[] mass = new double[] { 204.0866, 205.0899, 206.0915, 207.0942 };
            double[] intensity = new double[] { 0.901867, 0.084396, 0.012787, 0.000950 };
            ChargedIsotopicDistribution hexnac_ref = new ChargedIsotopicDistribution(mass, intensity, 1);

            IIsotopeDistGenerator gen = new Mercury7();

            ChargedIsotopicDistribution hexnac_mercury = gen.GenerateChargedIsotopicDistribution(hexnac_form, 1);

            for (int i = 0; i < hexnac_ref.Length; i++)
            {
                Assert.AreEqual(hexnac_ref.GetMz()[i], hexnac_mercury.GetMz()[i], 0.0001);
                Assert.AreEqual(hexnac_ref.GetIntensity()[i], hexnac_mercury.GetIntensity()[i], 0.0001);
            }
        }

        [TestMethod]
        public void Test_Single_Charged_Distribution_Carbonic()
        {
            // Testing data generated with mMass
            ChemicalFormula ca_form = new ChemicalFormula("C1290H1970N354O394S4");
            double[] mass = new double[] { 28856.40546, 28857.40794, 28858.41098, 28859.41393, 28860.41678,
                                           28861.41962, 28862.42237, 28863.42505, 28864.42769, 28865.4304,
                                           28866.43308, 28867.43578, 28868.43842, 28869.44104, 28870.44363,
                                           28871.44619, 28872.44873, 28873.45127, 28874.45379, 28875.4563,
                                           28876.4588, 28877.46128, 28878.46374, 28879.46606, 28880.46894,
                                           28881.471, 28882.47337, 28883.47578, 28884.47736, 28885.47777 };
            double[] intensity = new double[] { 6.35739E-05, 0.000250289, 0.000837294, 0.002145702, 0.004773472,
                                                0.009359079, 0.016488538, 0.026415142, 0.039026016, 0.052946508,
                                                0.06681604, 0.07886135, 0.087496192, 0.091605672, 0.09087918,
                                                0.085730901, 0.077143899, 0.066368093, 0.054709443, 0.043300049,
                                                0.032963397, 0.024176561, 0.017106652, 0.01164814, 0.007704643,
                                                0.004963036, 0.003030278, 0.001782571, 0.000980136, 0.000428152 };
            ChargedIsotopicDistribution ca_ref = new ChargedIsotopicDistribution(mass, intensity, 1);

            IIsotopeDistGenerator gen = new Mercury7();

            ChargedIsotopicDistribution ca_mercury = gen.GenerateChargedIsotopicDistribution(ca_form, 1);

            for (int i = 0; i < ca_ref.Length; i++)
            {
                // mMass doesn't calculate the intensity of the monoisotopic mass peak so need to offset by 3
                Assert.AreEqual(ca_ref.GetMz()[i], ca_mercury.GetMz()[i + 3], 0.01);
                Assert.AreEqual(ca_ref.GetIntensity()[i], ca_mercury.GetIntensity()[i + 3], 0.01);
            }
        }

        [TestMethod]
        public void Test_Thirty_Charged_Distribution_Carbonic()
        {
            // Testing data generated with mMass
            ChemicalFormula ca_form = new ChemicalFormula("C1290H1970N354O394S4");
            double[] mass = new double[] { 962.8872757, 962.9206969, 962.9541272, 962.9875463, 963.0209669,
                                           963.0543831, 963.0878052, 963.1212239, 963.1546426, 963.188077,
                                           963.2214919, 963.2549111, 963.288333, 963.3217516, 963.3551663,
                                           963.3885924, 963.4220101, 963.4554372, 963.4888482, 963.5222744,
                                           963.5556881, 963.5891017, 963.622521, 963.6559335, 963.6893484,
                                           963.7227599, 963.7561442, 963.7895819 };
            double[] intensity = new double[] { 0.000170524, 0.000533111, 0.001558402, 0.003715026, 0.007459397,
                                                0.013822287, 0.023144359, 0.035408395, 0.049036297, 0.062576509,
                                                0.075865762, 0.086457957, 0.091842426, 0.09290665, 0.089605827,
                                                0.080993753, 0.070710633, 0.058926547, 0.046819002, 0.035888922,
                                                0.026327029, 0.018401792, 0.012303588, 0.007758151, 0.004530259,
                                                0.002327596, 0.000773207, 0.000136591 };
            ChargedIsotopicDistribution ca_ref = new ChargedIsotopicDistribution(mass, intensity, 30);

            IIsotopeDistGenerator gen = new Mercury7();

            ChargedIsotopicDistribution ca_mercury = gen.GenerateChargedIsotopicDistribution(ca_form, 30);

            for (int i = 0; i < ca_ref.Length; i++)
            {
                // mMass doesn't calculate the intensity of the monoisotopic mass peak so need to offset by 4
                Assert.AreEqual(ca_ref.GetMz()[i], ca_mercury.GetMz()[i + 4], 0.0001);
                Assert.AreEqual(ca_ref.GetIntensity()[i], ca_mercury.GetIntensity()[i + 4], 0.01);
            }
        }
    }
}
