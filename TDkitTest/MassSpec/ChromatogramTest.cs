using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDkit.MassSpec;
using System;
using System.Collections.Generic;

namespace TDkitTest.MassSpec
{
    [TestClass]
    public class ChromatogramTest
    {
        Chromatogram chrom;

        [TestInitialize]
        public void TestInitialize()
        {
            double[] time = { 0.0, 1.0, 2.0, 2.5, 3.0, 3.5, 4.0, 5.0 };
            double[] intensity = { 0.0, 1e6, 2e5, 3e6, 2e7, 3e5, 2.4e5, 2e4 };

            chrom = new Chromatogram(time, intensity);
        }

        [TestMethod]
        public void Valid_Construct_From_Arrays()
        {
            double[] time = { 0.0, 1.0, 2.0, 2.5, 3.0, 3.5, 4.0, 5.0 };
            double[] intensity = { 0.0, 1e6, 2e5, 3e6, 2e7, 3e5, 2.4e5, 2e4 };

            Chromatogram test = new Chromatogram(time, intensity);

            Assert.AreEqual(8, test.Length);
        }

        [TestMethod]
        public void Valid_Construct_From_Lists()
        {
            double[] time = { 0.0, 1.0, 2.0, 2.5, 3.0, 3.5, 4.0, 5.0 };
            double[] intensity = { 0.0, 1e6, 2e5, 3e6, 2e7, 3e5, 2.4e5, 2e4 };

            Chromatogram test = new Chromatogram(new List<double>(time), new List<double>(intensity));

            Assert.AreEqual(8, test.Length);
        }

        [TestMethod]
        public void Mismatched_Array_Lengths()
        {
            double[] time = { 0.0, 1.0, 2.0, 2.5, 3.0, 3.5, 4.0, 5.0 };
            double[] intensity = { 0.0, 1e6, 2e5, 3e6, 2e7, 3e5, 2.4 };

            Chromatogram test;

            Assert.ThrowsException<ArgumentException>(() => test = new Chromatogram(time, intensity));
        }

        [TestMethod]
        public void Mismatched_List_Lengths()
        {
            double[] time = { 0.0, 1.0, 2.0, 2.5, 3.0, 3.5, 4.0, 5.0 };
            double[] intensity = { 0.0, 1e6, 2e5, 3e6, 2e7, 3e5, 2.4 };

            Chromatogram test;

            Assert.ThrowsException<ArgumentException>(() => test = new Chromatogram(new List<double>(time), new List<double>(intensity)));
        }

        [TestMethod]
        public void Exact_Time_Slice()
        {
            Assert.AreEqual(2e5, chrom.IntensityAtTime(2));
        }

        [TestMethod]
        public void Inexact_Time_Slice()
        {
            // Should return the previous datapoint
            Assert.AreEqual(2e5, chrom.IntensityAtTime(2.2));

            // Should return the next datapoint
            Assert.AreEqual(3e6, chrom.IntensityAtTime(2.3));
        }

        [TestMethod]
        public void Outside_Time_Range()
        {
            double test;
            Assert.ThrowsException<IndexOutOfRangeException>(() => test = chrom.IntensityAtTime(6));
        }

        [TestMethod]
        public void Get_Time()
        {
            Assert.AreEqual(2.0, chrom.GetTimes()[2]);
        }

        [TestMethod]
        public void Get_Intensity()
        {
            Assert.AreEqual(2e5, chrom.GetIntensity()[2]);
        }
    }
}
