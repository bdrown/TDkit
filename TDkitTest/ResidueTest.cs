using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using TDkit;

namespace TDkitTest
{

    [TestClass]
    public class ResidueTest
    {
        Residue alanine;

        [TestInitialize]
        public void TestInitialize()
        {
            alanine = new Residue('A', "Alanine", "C3H5ON");
        }

        [TestMethod]
        public void Calc_Monoisotopic_Mass()
        {
            Assert.AreEqual(71.0371137878, alanine.MonoisotopicMass(), 0.00000001);
        }

        [TestMethod]
        public void Calc_Avg_Mass()
        {
            Assert.AreEqual(71.07794, alanine.AverageMass(), 0.01);
        }

        [TestMethod]
        public void Valid_Symbol_Retrieval()
        {
            Residue valine = Residue.GetResidue('V');
        }

        [TestMethod]
        public void Invalid_Symbol_Retrieval()
        {
            Residue res;
            Assert.ThrowsException<ArgumentException>(() => res = Residue.GetResidue('X'));
        }

        [TestMethod]
        public void Residue_Equality()
        {
            Residue first = Residue.GetResidue('V');
            Residue second = Residue.GetResidue('V');

            Assert.AreEqual(first, second);
        }
    }
}
