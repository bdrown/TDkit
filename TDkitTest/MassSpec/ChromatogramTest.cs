using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDkit.MassSpec;
using System;
using System.Collections.Generic;

namespace TDkitTest.MassSpec
{
    [TestClass]
    public class ChromatogramTest
    {
        [TestMethod]
        public void Valid_Construct_From_Arrays()
        {
            double[] time = { 0.0, 1.0, 2.0, 2.5, 3.0, 3.5, 4.0, 5.0 };
            double[] intensity = { 0.0, 1e6, 2e5, 3e6, 2e7, 3e5, 2.4e5, 2e4 };

            Chromatogram chrom = new Chromatogram(time, intensity);

            Assert.AreEqual(8, chrom.Length);
        }

        [TestMethod]
        public void Valid_Construct_From_Lists()
        {
            double[] time = { 0.0, 1.0, 2.0, 2.5, 3.0, 3.5, 4.0, 5.0 };
            double[] intensity = { 0.0, 1e6, 2e5, 3e6, 2e7, 3e5, 2.4e5, 2e4 };

            Chromatogram chrom = new Chromatogram(new List<double>(time), new List<double>(intensity));

            Assert.AreEqual(8, chrom.Length);
        }

        [TestMethod]
        public void Mismatched_Array_Lengths()
        {
            double[] time = { 0.0, 1.0, 2.0, 2.5, 3.0, 3.5, 4.0, 5.0 };
            double[] intensity = { 0.0, 1e6, 2e5, 3e6, 2e7, 3e5, 2.4 };

            Chromatogram chrom;

            Assert.ThrowsException<ArgumentException>(() => chrom = new Chromatogram(time, intensity));
        }

        [TestMethod]
        public void Mismatched_List_Lengths()
        {
            double[] time = { 0.0, 1.0, 2.0, 2.5, 3.0, 3.5, 4.0, 5.0 };
            double[] intensity = { 0.0, 1e6, 2e5, 3e6, 2e7, 3e5, 2.4 };

            Chromatogram chrom;

            Assert.ThrowsException<ArgumentException>(() => chrom = new Chromatogram(new List<double>(time), new List<double>(intensity)));
        }

        [TestMethod]
        public void Exact_Time_Slice()
        {
            double[] time = { 0.0, 1.0, 2.0, 2.5, 3.0, 3.5, 4.0, 5.0 };
            double[] intensity = { 0.0, 1e6, 2e5, 3e6, 2e7, 3e5, 2.4e5, 2e4 };

            Chromatogram chrom = new Chromatogram(time, intensity);

            Assert.AreEqual(2e5, chrom.IntensityAtTime(2));
        }

        [TestMethod]
        public void Inexact_Time_Slice()
        {
            double[] time = { 0.0, 1.0, 2.0, 2.5, 3.0, 3.5, 4.0, 5.0 };
            double[] intensity = { 0.0, 1e6, 2e5, 3e6, 2e7, 3e5, 2.4e5, 2e4 };

            Chromatogram chrom = new Chromatogram(time, intensity);

            Assert.AreEqual(2e5, chrom.IntensityAtTime(2.2));
        }

        [TestMethod]
        public void Outside_Time_Range()
        {
            double[] time = { 0.0, 1.0, 2.0, 2.5, 3.0, 3.5, 4.0, 5.0 };
            double[] intensity = { 0.0, 1e6, 2e5, 3e6, 2e7, 3e5, 2.4e5, 2e4 };

            Chromatogram chrom = new Chromatogram(time, intensity);
            
            double test;
            Assert.ThrowsException<IndexOutOfRangeException>(() => test = chrom.IntensityAtTime(6));
        }


    }
}
