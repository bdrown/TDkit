using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDkit.MassSpec;
using System;
using System.Collections.Generic;

namespace TDkitTest.MassSpec
{
    [TestClass]
    public class SpectrumTest
    {
        Spectrum glucose_ms1;

        [TestInitialize]
        public void TestInitialize()
        {
            double[] mz = { 181.0707, 182.0741, 183.0752 };
            double[] intensity = { 100.0, 6.84, 1.41 };

            glucose_ms1 = new Spectrum(mz, intensity, 1.2, 2, 256.4);
        }

        [TestMethod]
        public void Valid_Constructor_Array()
        {
            double[] mz = { 181.0707, 182.0741, 183.0752 };
            double[] intensity = { 100.0, 6.84, 1.41 };

            Spectrum spec = new Spectrum(mz, intensity, 1.2, 1);

            Assert.AreEqual(3, spec.Length);
        }

        [TestMethod]
        public void Mismatched_Data()
        {
            double[] mz = { 181.0707, 182.0741, 183.0752 };
            double[] intensity = { 100.0, 6.84};

            Spectrum spec;
            Assert.ThrowsException<ArgumentException>(() => spec = new Spectrum(mz, intensity));
        }

        [TestMethod]
        public void Valid_Constructor_List()
        {
            double[] mz = { 181.0707, 182.0741, 183.0752 };
            double[] intensity = { 100.0, 6.84, 1.41 };

            Spectrum spec = new Spectrum(new List<double>(mz), new List<double>(intensity), 1.2, 1);

            Assert.AreEqual(3, spec.Length);
        }

        [TestMethod]
        public void Mismatched_Data_List()
        {
            double[] mz = { 181.0707, 182.0741, 183.0752 };
            double[] intensity = { 100.0, 6.84 };

            Spectrum spec;
            Assert.ThrowsException<ArgumentException>(() => spec = new Spectrum(new List<double>(mz), new List<double>(intensity), 1.2, 1));
        }

        [TestMethod]
        public void Gives_Length()
        {
            Assert.AreEqual(3, glucose_ms1.Length);
        }

        [TestMethod]
        public void Gives_First_Mz()
        {
            Assert.AreEqual(181.0707, glucose_ms1.FirstMz);
        }

        [TestMethod]
        public void Gives_Last_Mz()
        {
            Assert.AreEqual(183.0752, glucose_ms1.LastMz);
        }

        [TestMethod]
        public void Gives_Mz_List()
        {
            Assert.IsInstanceOfType(glucose_ms1.GetMz(),typeof(IList<double>));
            Assert.AreEqual(182.0741, glucose_ms1.GetMz()[1]);
        }

        [TestMethod]
        public void Gives_Intensity_List()
        {
            Assert.IsInstanceOfType(glucose_ms1.GetIntensity(), typeof(IList<double>));
            Assert.AreEqual(6.84, glucose_ms1.GetIntensity()[1]);
        }

        [TestMethod]
        public void Gives_Precursor()
        {
            Assert.AreEqual(256.4, glucose_ms1.Precursor);
        }

        [TestMethod]
        public void Gives_MS_Level()
        {
            Assert.AreEqual(2, glucose_ms1.MSLevel);
        }

        [TestMethod]
        public void Gives_Retention_Time()
        {
            Assert.AreEqual(1.2, glucose_ms1.RetentionTime);
        }
    }
}
